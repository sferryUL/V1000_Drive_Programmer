using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;
using ModbusRTU;

namespace V1000_ModbusRTU
{
    class V1000_ModbusRTU_Comm
    {
        public V1000_ModbusRTU_Comm() { }

        // The minimum read message delay (in ms) accounts for the minimum wait time requirements by
        // the V1000 drive, a 24-bit length gap between messages, and a single register byte read.
        const int DelayReadMsgMin = 30;
        const int DelayReadByte = 2; // 1.05 ms is the actual byte transmission time (8 data bits, 1 start and 1 stop bit)
        const int RespReadByteMin = 7;

        // The minimum write message delay (in ms) accounts for the minimum wait time requirements by
        // the V1000 drive when the default setting for H5-11 is selected (1). There is a 200ms
        // delay from the end of the message to initiate a response, a 24-bit gap requirement, and 
        // also account for the master length of time to send the message. I can't tell if the serial 
        // port function waits until transmission is complete to release or if it is a background process.
        const int DelayWriteMsgMin = 210;
        const int DelayEnterMax = 4000;
        const int RespWriteByte = 8;

        const int RespLoopbackByte = 8;

        const byte ModeReadReg = 0x00;
        const byte ModeLoopback = 0x01;
        const byte ModeWriteReg = 0x02;

        public int OpenCommPort(ref SerialPort p_SPort)
        {
            int RetCode = 0;

            // Verify that the serial port is valid
            try
            {
                if (!p_SPort.IsOpen)
                    p_SPort.Open();
                else
                    RetCode = 0x8000;
            }
            catch
            {
                RetCode = 0x8001;
            }

            if (p_SPort.IsOpen)
                RetCode = 0x0001;

            return RetCode;
        }

        public void CloseCommPort(ref SerialPort p_SPort)
        {
            if (p_SPort.IsOpen)
                p_SPort.Close();
        }

        public int DataTransfer(ref ModbusRTUMsg p_Msg, ref SerialPort p_SPort)
        {
            int delay = 0, readbytes = 0, regcnt = 0, RetCode = 0;

            ModbusRTUMaster Modbus_Data = new ModbusRTUMaster();
            List<byte> V1000_Serial_Data = new List<byte>();

            // Verify that the serial port is valid
            if (!p_SPort.IsOpen)
            {
                RetCode = 0x8000;
                goto DataTransferExit;
            }

            switch (p_Msg.FuncCode)
            {
                case ModbusRTUMaster.ReadReg:
                    delay = DelayReadMsgMin + (DelayReadByte * ((p_Msg.RegCount - 1) * 2));
                    readbytes = RespReadByteMin + ((p_Msg.RegCount - 1) * p_Msg.RegByteCount);
                    break;
                case ModbusRTUMaster.Loopback:
                    delay = 1;
                    readbytes = RespLoopbackByte;
                    break;
                case ModbusRTUMaster.WriteReg:
                    // Check if the write register is one of the Enter command registers or if the register 
                    // that is being modified is the drive initialization register (A1-03/0x0103) If so, then 
                    // the delay is set to the maximum write time of 2 seconds to prevent an error for too short 
                    // of a delay between writing the register and getting a valid response from the drive.
                    if ((p_Msg.StartReg == 0x0900) || (p_Msg.StartReg == 0x0910) || (p_Msg.StartReg == 0x0103))
                        delay = DelayEnterMax;
                    else
                    // Otherwise set the delay to the minimum write delay response
                        delay = DelayWriteMsgMin;
                    readbytes = RespWriteByte;
                    regcnt = p_Msg.RegCount;
                    break;
            }

            // Calculate CRC-16 value and create the raw byte separated buffer to send out via the serial bus
            V1000_Serial_Data = Modbus_Data.CreateRawMessageBuffer(p_Msg, true);

            // Send serial data
            byte[] OutBuff = new byte[V1000_Serial_Data.Count];
            for (int i = 0; i < V1000_Serial_Data.Count; i++)
                OutBuff[i] = V1000_Serial_Data[i];
            p_SPort.Write(OutBuff, 0, OutBuff.Count());

            // Wait for response
            Thread.Sleep(delay);

            // Check if there is a valid full message to read of the same size as the number
            // of registers requested. Anything less basically means a fault occurred. 
            if (p_SPort.BytesToRead != readbytes)
            {
                if (p_SPort.BytesToRead > 0)
                {
                    byte[] TempBuff = new byte[p_SPort.BytesToRead];
                    p_SPort.Read(TempBuff, 0, p_SPort.BytesToRead);
                    //p_SPort.DiscardInBuffer();
                }
                RetCode = 0x8002;
                goto DataTransferExit;
            }

            // Get Modbus RTU serial message received back from slave
            byte[] InBuff = new byte[p_SPort.BytesToRead];
            p_SPort.Read(InBuff, 0, p_SPort.BytesToRead);

            // Update actual Modbus RTU message based on raw data received from the serial port
            int stat = Modbus_Data.ExtractMessage(InBuff.ToList(), ref p_Msg);
            if (stat == 0x0001)
            {
                if (p_Msg.FuncCode == ModbusRTUMaster.WriteReg)
                {
                    if (regcnt == p_Msg.RegCount)
                        RetCode = 0x0001;
                    else
                        RetCode = 0x8003;
                }
                else
                    RetCode = 0x0001;
            }
            else
            {
                p_Msg.ClearAll();
                RetCode = 0x8004;
            }

            DataTransferExit:

            return RetCode; // return the acquired return code from this method.
        }

        public int SaveParamChanges(byte p_SlaveAddr, ref SerialPort p_SPort)
        {
            int RetCode = 0;
            List<ushort> data = new List<ushort>();
            ModbusRTUMsg msg = new ModbusRTUMsg();

            // Add a value of 0 to the data payload. Parameter update saves are accomplished by 
            // writing a value of 0 to register 0x0900.
            data.Add(0);

            // Create Modbus RTU message and send it out to the drive via the DataTransfer() method
            ModbusRTUMaster modbus = new ModbusRTUMaster(p_SlaveAddr, ModbusRTUMaster.WriteReg, 0x0900, 1, data);
            msg = modbus.CreateMessage();
            RetCode = DataTransfer(ref msg, ref p_SPort);
            
            return RetCode;
        }

    } // class V1000_ModbusRTU_Comm



    public class V1000_Param_Data : ICloneable, IComparable<V1000_Param_Data>
    {
        public ushort RegAddress;
        public string ParamNum;
        public string ParamName;
        public ushort Multiplier;
        public byte   NumBase;
        public string Units;

        public const ushort AccLvlReg = 0x101;
        public const ushort AccLvlOpOnly = 0x0000;

        public const ushort InitReg = 0x103;

        public const ushort RegCtrlMethod = 0x0103;

        public const ushort FreqRefRngLow = 0x0280;
        public const ushort FreqRefRngHi = 0x0293;

        public const string VoltSuppParam   = "E1-01";
        public const string VoltMaxOutParam = "E1-05";
        public const string FreqBaseParam   = "E1-06";
        public const string RatedCurrParam  = "E2-01";

        private ushort _DefVal;
        private ushort _ParamVal;
        private string _ParamValDisp;
        private string _DefValDisp;

        public V1000_Param_Data()
        {
            RegAddress = 0;
            ParamNum = "";
            ParamName = "";
            Multiplier = 0;
            NumBase = 10;
            Units = "";

            _ParamVal = 0;
            _DefVal = 0;
            _ParamValDisp = "";
            _DefValDisp = "";
        }

        public V1000_Param_Data(ushort p_Addr, string p_Num, string p_Name, ushort p_Val, ushort p_Def, ushort p_Mult, byte p_Base, string p_Unit, string p_DefDisp, string p_ValDisp)
        {
            RegAddress = p_Addr;
            ParamNum = p_Num;
            ParamName = p_Name;
            Multiplier = p_Mult;
            NumBase = p_Base;
            Units = p_Unit;

            _ParamVal = p_Val;
            _DefVal = p_Def;
            _DefValDisp = p_DefDisp;
            _ParamValDisp = p_ValDisp;
        }

        public ushort ParamVal
        {
            get => _ParamVal;
            set
            {
                int round_val = 0;

                this._ParamVal = value;
                if (NumBase == 16)
                    this._ParamValDisp = "0x" + this._ParamVal.ToString("X4");
                else
                {
                    switch (this.Multiplier)
                    {
                        case 1:
                            round_val = 0;
                            break;
                        case 10:
                            round_val = 1;
                            break;
                        case 100:
                            round_val = 2;
                            break;
                        case 1000:
                            round_val = 3;
                            break;
                    }

                    this._ParamValDisp = Math.Round(((Double)this._ParamVal / this.Multiplier), round_val).ToString() + " " + Units;
                }
                    
            }
        }

        public ushort DefVal
        {
            get => _DefVal;
            set
            {
                int round_val = 0;

                _DefVal = value;
                if (NumBase == 16)
                    this._DefValDisp = "0x" + this._DefVal.ToString("X4");
                else
                {
                    switch (this.Multiplier)
                    {
                        case 1:
                            round_val = 0;
                            break;
                        case 10:
                            round_val = 1;
                            break;
                        case 100:
                            round_val = 2;
                            break;
                        case 1000:
                            round_val = 3;
                            break;
                    }

                    this._DefValDisp = Math.Round(((Double)this._DefVal / this.Multiplier), round_val).ToString() + " " + Units;
                }
            }
        }

        public string ParamValDisp { get => _ParamValDisp; }
        public string DefValDisp { get => _DefValDisp; }

        public object Clone()
        {
            return new V1000_Param_Data(this.RegAddress, this.ParamNum, this.ParamName, this.ParamVal, this.DefVal, this.Multiplier, this.NumBase, this.Units, this.DefValDisp, this.ParamValDisp);
        }

        public int CompareTo(V1000_Param_Data p_CompParam)
        {
            return this.ParamNum.CompareTo(p_CompParam.ParamNum);
        }
    }
    
    class V1000_File_Data
    {
        public string ParamName;
        public string ParamNum;
        public string Value;
        public string RegAddress;
        public string Multiplier;
        public string NumBase;
        public string Units;

        public V1000_File_Data()
        {
            RegAddress = "";
            ParamNum = "";
            ParamName = "";
            Value = "";
            Multiplier = "";
            NumBase = "";
            Units = "";
        }
    }
    
}
