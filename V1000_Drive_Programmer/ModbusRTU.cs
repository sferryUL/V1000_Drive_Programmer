using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusRTU
{
    public class ModbusRTUMsg
    {
        public byte SlaveAddr = 0;
        public byte FuncCode = 0;
        public ushort StartReg = 0;
        public ushort RegCount = 0;
        public byte DataByteCount = 0;
        public List<ushort> Data = new List<ushort>();
        public ushort CRC16 = 0xFFFF;

        public byte RegByteCount = 2;
        public ushort SubFunction = 0;

        public byte ErrCode = 0;

        public ModbusRTUMsg() { }
        public ModbusRTUMsg(byte p_SlaveAddr) { SlaveAddr = p_SlaveAddr; }
                
        public void Clear()
        {
            FuncCode = 0;
            StartReg = 0;
            RegCount = 0;
            DataByteCount = 0;
            Data.Clear();
            CRC16 = 0xFFFF;
            RegByteCount = 2;
            SubFunction = 0;
            ErrCode = 0;
        }

        public void ClearAll()
        {
            SlaveAddr = 0;
            FuncCode = 0;
            StartReg = 0;
            RegCount = 0;
            DataByteCount = 0;
            Data.Clear();
            CRC16 = 0xFFFF;
            RegByteCount = 2;
            SubFunction = 0;
            ErrCode = 0;

        }

        public ModbusRTUMsg Copy()
        {
            ModbusRTUMsg TmpMsg = new ModbusRTUMsg();

            TmpMsg.SlaveAddr = SlaveAddr;
            TmpMsg.FuncCode = FuncCode;
            TmpMsg.StartReg = StartReg;
            TmpMsg.RegCount = RegCount;
            TmpMsg.DataByteCount = DataByteCount;
            TmpMsg.Data = Data.ToList();
            TmpMsg.CRC16 = CRC16;
            TmpMsg.RegByteCount = RegByteCount;
            TmpMsg.SubFunction = SubFunction;
            TmpMsg.ErrCode = ErrCode;
            return TmpMsg;
        }
    }

    public class ModbusRTUMaster
    {
        // Fields
        List<byte> RawMsg = new List<byte>();
        ModbusRTUMsg OutMsg = new ModbusRTUMsg();
        ModbusRTUMsg RespMsg = new ModbusRTUMsg();

        // Constants
        public const byte ReadReg       = 0x03;
        public const byte Loopback      = 0x08;
        public const byte WriteReg      = 0x10;
        public const byte ReadRegErr    = 0x83;
        public const byte LoopBackErr   = 0x89;
        public const byte WriteRegErr   = 0x90;
        
        // Class Constructors
        public ModbusRTUMaster() { }

        public ModbusRTUMaster(byte p_SlaveAddr, byte p_FuncCode, ushort p_StartReg)
        {
            OutMsg.SlaveAddr = p_SlaveAddr;
            OutMsg.FuncCode = p_FuncCode;
            OutMsg.StartReg = p_StartReg;
        }

        public ModbusRTUMaster(byte p_SlaveAddr, byte p_FuncCode, ushort p_StartReg, ushort p_RegCount, List<ushort> p_Payload)
        {
            OutMsg.SlaveAddr = p_SlaveAddr;
            OutMsg.FuncCode = p_FuncCode;
            OutMsg.StartReg = p_StartReg;
            OutMsg.RegCount = p_RegCount;
            OutMsg.Data = p_Payload.ToList();
            OutMsg = CreateMessage(OutMsg);
        }

        // Property Initializers
        public byte SlaveAddr { get => OutMsg.SlaveAddr; set => OutMsg.SlaveAddr = value; }
        public byte FuncCode    { get => OutMsg.FuncCode; set => OutMsg.FuncCode = value; }
        public ushort StartReg  { get => OutMsg.StartReg; set => OutMsg.StartReg = value; }
        public ushort RegCount  { get => OutMsg.RegCount; set => OutMsg.RegCount = value; }
        public ushort RegByteCount => OutMsg.RegByteCount;
        public ushort CRC16     => OutMsg.CRC16;
        public ushort MsgSize   => (ushort)OutMsg.Data.Count();

        // Public Class Methods
        public void ClearData() { OutMsg.Clear(); }

        public void ClearAll() { OutMsg.ClearAll(); }

        public ModbusRTUMsg CreateMessage(byte p_SlaveAddr, byte p_FuncCode, ushort p_StartReg, ushort p_RegCount, List<ushort> p_Payload)
        {
            ModbusRTUMsg TmpMsg = new ModbusRTUMsg();
            List<byte> TmpBuff = new List<byte>();

            TmpMsg.SlaveAddr = p_SlaveAddr;
            TmpMsg.FuncCode = p_FuncCode;
            TmpMsg.StartReg = p_StartReg;

            if (p_FuncCode != 0x08)
                TmpMsg.RegCount = p_RegCount;

            // Add data payload to overall message - skip for read register requests
            if ((p_FuncCode == 0x08) || (p_FuncCode == 0x10))
                TmpMsg.Data = p_Payload.ToList();

            if (p_FuncCode == 0x10)
                TmpMsg.DataByteCount = (byte)(TmpMsg.Data.Count << 1);

            TmpBuff = CreateRawMessageBuffer(TmpMsg, false);
            TmpMsg.CRC16 = CalcModbusRTUCRC16(TmpBuff);

            return TmpMsg;
        }

        public ModbusRTUMsg CreateMessage(ModbusRTUMsg p_Msg)
        {
            ModbusRTUMsg TmpMsg = new ModbusRTUMsg();
            List<byte> TmpBuff = new List<byte>();

            TmpMsg.SlaveAddr = p_Msg.SlaveAddr;
            TmpMsg.FuncCode = p_Msg.FuncCode;
            TmpMsg.StartReg = p_Msg.StartReg;

            // Loopback messages do not use a register count field
            if (p_Msg.FuncCode != 0x08)
                TmpMsg.RegCount = p_Msg.RegCount;

            // Add data payload to overall message - skip for read register requests
            if ((p_Msg.FuncCode == 0x08) || (p_Msg.FuncCode == 0x10))
                TmpMsg.Data = p_Msg.Data.ToList();

            // If this is a message to be written then add the number of bytes to be written.
            // This is simply a left shift (multiply by 2) of the number of 16-bit values in the
            // data payload. Could have used the register count as well.
            if (p_Msg.FuncCode == 0x10)
                TmpMsg.DataByteCount = (byte)(TmpMsg.Data.Count << 1);

            // Create a temporary raw message buffer in order to calculate the CRC-16 value.
            // CRC-16 values are calculated off of each 8-bit data transfer 
            TmpBuff = CreateRawMessageBuffer(TmpMsg, false);

            // Add the CRC-16 value to the overall formatted Modbus RTU message
            TmpMsg.CRC16 = CalcModbusRTUCRC16(TmpBuff);

            return TmpMsg;
        }

        public List<byte> CreateRawMessageBuffer(List<byte> p_DataPayload) 
        {
            RawMsg.Add(OutMsg.SlaveAddr);                         // Add slave address to overall message
            RawMsg.Add(OutMsg.FuncCode);                          // Add function code to overall message
            RawMsg.Add((byte)(OutMsg.StartReg >> 8));             // Add starting register upper byte
            RawMsg.Add((byte)(OutMsg.StartReg & 0x00FF));         // Add starting register lower byte

            if (OutMsg.FuncCode != 0x08)
            {
                RawMsg.Add((byte)(OutMsg.RegCount >> 8));             // Add register count upper byte
                RawMsg.Add((byte)(OutMsg.RegCount & 0x00FF));         // Add register count lower byte
            }

            if(OutMsg.FuncCode == 0x10)
                RawMsg.Add(GetNumDataBytes(p_DataPayload));    // Add number of data bytes in the data payload

            // Add data payload to overall message - skip for read register requests
            if ((OutMsg.FuncCode == 0x08) || (OutMsg.FuncCode == 0x10))
                for (int i = 0; i < p_DataPayload.Count; i++)
                    RawMsg.Add(p_DataPayload[i]);

            OutMsg.CRC16 = CalcModbusRTUCRC16(RawMsg);            // Calculate the Modbus RTU CRC-16 value

            // Modbus RTU CRC16 is Big-Endian format So lower byte is added first
            RawMsg.Add((byte)(OutMsg.CRC16 & 0x00FF));
            RawMsg.Add((byte)(OutMsg.CRC16 >> 8));
            return RawMsg;
        }

        public ModbusRTUMsg CreateMessage()
        {
            return OutMsg;
        }

        public List<byte> CreateRawMessageBuffer(ModbusRTUMsg p_Msg, bool p_Mode)
        {
            List<byte> buffer = new List<byte>();

            buffer.Add(p_Msg.SlaveAddr);                    // Add slave address to overall message
            buffer.Add(p_Msg.FuncCode);                     // Add function code to overall message
            buffer.Add(((byte)(p_Msg.StartReg >> 8)));      // Add starting register upper byte
            buffer.Add(((byte)(p_Msg.StartReg & 0x00FF)));  // Add starting register lower byte

            if (p_Msg.FuncCode != 0x08)
            {
                buffer.Add(((byte)(p_Msg.RegCount >> 8)));      // Add register count upper byte
                buffer.Add(((byte)(p_Msg.RegCount & 0x00FF)));  // Add register count lower byte
            }

            if (p_Msg.FuncCode == 0x10)                         // Add number of data bytes in the data payload
                buffer.Add(p_Msg.DataByteCount);

            // Add data payload to overall message - skip for read register requests
            if ((p_Msg.FuncCode == 0x08) || (p_Msg.FuncCode == 0x10))
            {
                for (int i = 0; i < p_Msg.Data.Count; i++)
                {
                    buffer.Add((byte)(p_Msg.Data[i] >> 8));
                    buffer.Add((byte)(p_Msg.Data[i] & 0x00FF));
                }

            }

            if (p_Mode)
            {
                // Add CRC-16 calculation to message. It is stored in Big-Endian format so lower byte added first!
                buffer.Add(((byte)(p_Msg.CRC16 & 0x00FF)));  // Add starting register lower byte
                buffer.Add(((byte)(p_Msg.CRC16 >> 8)));      // Add starting register upper byte
            }

            return buffer;
        }

        public List<byte> CreateDataPayloadByte(string p_Buffer)
        {
            string[] HexBuffer;
            List<byte> RetVal = new List<byte>();

            p_Buffer = p_Buffer.Trim();
            HexBuffer = p_Buffer.Split(' ');
            foreach (String HexStr in HexBuffer)
            {
                byte HexVal = Convert.ToByte(HexStr, 16);
                RetVal.Add(HexVal);
            }

            return RetVal;
        }

        public List<ushort> CreateDataPayloadUShort(string p_Buffer)
        {
            string[] HexBuffer;
            List<ushort> RetVal = new List<ushort>();

            p_Buffer = p_Buffer.Trim();
            HexBuffer = p_Buffer.Split(' ');
            foreach (String HexStr in HexBuffer)
            {
                ushort HexVal = Convert.ToUInt16(HexStr, 16);
                RetVal.Add(HexVal);
            }

            return RetVal;
        }

        public int ExtractMessage(List<byte> p_RawMsg, ref ModbusRTUMsg p_ModbusMsg)
        {
            ushort InCRC = 0, CalcCRC16 = 0;

            /* First verify message has a valid CRC in relation to its data */
            // Extract CRC-16 value
            InCRC = (ushort)((p_RawMsg[p_RawMsg.Count - 1] << 8) | (p_RawMsg[p_RawMsg.Count - 2]));

            // Strip CRC-16 off the full message
            p_RawMsg.RemoveRange(p_RawMsg.Count - 2, 2); 

            // Calculate the CRC-16 based on the received data minus the last two bytes (received CRC-16)
            CalcCRC16 = CalcModbusRTUCRC16(p_RawMsg);
            if (InCRC != CalcCRC16)
                return 0x8000;

            /* Extract the received message and put each data byte into it's correct ModbusRTUMsg fields */
            p_ModbusMsg.CRC16 = InCRC;               // Store CRC-16 value that was previously extracted from the full message
            p_ModbusMsg.SlaveAddr = p_RawMsg[0];    // Store the slave address
            p_ModbusMsg.FuncCode = p_RawMsg[1];     // Store the function code



            // Store the different byte locations of the overall message based on the type of message it is
            switch (p_ModbusMsg.FuncCode)
            {
                case ReadReg: // Read register response from slave
                    p_ModbusMsg.DataByteCount = p_RawMsg[2]; // Get the number of data bytes in the read register(s) response.
                    p_ModbusMsg.RegCount = (ushort)(p_ModbusMsg.DataByteCount >> 1);
                    p_RawMsg.RemoveRange(0, 3);             // Leave only the data payload remaining in the message

                    // Convert the byte data payload to 16-bit values for processing
                    for (int i = 0; i < p_ModbusMsg.DataByteCount; i+=2)
                        p_ModbusMsg.Data.Add((ushort)((p_RawMsg[i] << 8) | p_RawMsg[i + 1]));
                    break;
                case Loopback: // Loopback response from slave
                    p_ModbusMsg.SubFunction = (ushort)((p_RawMsg[2] << 8) | p_RawMsg[3]);
                    p_ModbusMsg.RegCount = (ushort)((p_RawMsg[4] << 8) | p_RawMsg[5]);
                    break;
                case WriteReg: // Write register response from slave
                    p_ModbusMsg.StartReg = (ushort)((p_RawMsg[2] << 8) | p_RawMsg[3]);
                    p_ModbusMsg.RegCount = (ushort)((p_RawMsg[4] << 8) | p_RawMsg[5]);
                    break;
                case ReadRegErr:
                case LoopBackErr:
                case WriteRegErr:
                    p_ModbusMsg.ErrCode = p_RawMsg[2];
                    break;
                default:
                    return 0x8001; // Unknown function code
            }

            return 0x0001;
        }

        public string CreateDataBufferString(List<byte> p_DataBuffer)
        {
            string RetVal = "";

            for (ushort i = 0; i < p_DataBuffer.Count; i++)
            {
                RetVal += ("0x" + p_DataBuffer[i].ToString("X2") + " ");
            }

            return RetVal;
        }

        public string CreateModbusRTUDataString(List<byte> p_Buffer)
        {
            string RetVal = "";

            for (int i = 0; i < p_Buffer.Count(); i++)
                RetVal += ("0x" + p_Buffer[i].ToString("X2") + " ");

            return RetVal;
        }

        public string CreateModbusRTUDataString(List<ushort> p_Buffer)
        {
            string RetVal = "";

            for (int i = 0; i < p_Buffer.Count(); i++)
                RetVal += ("0x" + p_Buffer[i].ToString("X4") + " ");

            return RetVal;
        }

        public string CreateModbusRTUDataString(ModbusRTUMsg p_Msg)
        {
            string RetVal = "";

            RetVal = "0x" + p_Msg.SlaveAddr.ToString("X2") + " ";
            RetVal += "0x" + p_Msg.FuncCode.ToString("X2") + " ";

            switch (p_Msg.FuncCode)
            {
                case ModbusRTUMaster.ReadReg:
                case ModbusRTUMaster.Loopback:
                    if (p_Msg.FuncCode == ModbusRTUMaster.ReadReg)
                        RetVal += "0x" + p_Msg.DataByteCount.ToString("X2") + " ";
                    else
                        RetVal += "0x" + ((byte)(p_Msg.SubFunction >> 8)).ToString("X2") + " 0x" + ((byte)(p_Msg.SubFunction & 0x00FF)).ToString("X2") + " ";
                    for (int i = 0; i < p_Msg.Data.Count; i++)
                    {
                        RetVal += "0x" + ((byte)(p_Msg.Data[i] >> 8)).ToString("X2") + " ";
                        RetVal += "0x" + ((byte)(p_Msg.Data[i] & 0x00FF)).ToString("X2") + " ";
                    }
                    break;
                case ModbusRTUMaster.WriteReg:
                    RetVal += "0x " + ((byte)(p_Msg.StartReg >> 8)).ToString("X2") + " 0x" + ((byte)(p_Msg.StartReg & 0x00FF)).ToString("X2") + " ";
                    RetVal += "0x " + ((byte)(p_Msg.RegCount >> 8)).ToString("X2") + " 0x" + ((byte)(p_Msg.RegCount & 0x00FF)).ToString("X2") + " ";
                    break;
            }

            RetVal += "0x" + ((byte)p_Msg.CRC16 & 0x00FF).ToString("X2") + " 0x" + ((byte)p_Msg.CRC16 >> 8).ToString("X2");

            return RetVal;
        }


        // Private Helper Functions
        private byte GetNumDataBytes(List<byte> p_Payload)
        {
            return (byte)p_Payload.Count();
        }

        private ushort CalcModbusRTUCRC16(List<byte> p_DataBuffer)
        {
            ushort CRCResult = 0xFFFF, XORVal = 0xA001, XOR = 0x0000;

            for (int i = 0; i < p_DataBuffer.Count; i++)
            {
                CRCResult ^= p_DataBuffer[i];
                for (int j = 0; j < 8; j++)
                {
                    XOR = (ushort)(CRCResult & 0x0001);
                    CRCResult >>= 1;

                    if (XOR > 0)
                        CRCResult ^= XORVal;
                }
            }
            return CRCResult;
        }

    }
}
