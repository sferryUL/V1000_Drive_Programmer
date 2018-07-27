using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModbusRTU;
using V1000_Param_Prog;
using System.Runtime.InteropServices;
using XL = Microsoft.Office.Interop.Excel;
using V1000_ModbusRTU;

namespace V1000_Param_Prog
{
    public partial class frmMain : Form
    {
        // Create objects for Modbus RTU data transmission with the V1000
        ModbusRTUMsg OutputMsg = new ModbusRTUMsg();
        ModbusRTUMsg ResponseMsg = new ModbusRTUMsg();
        ModbusRTUMaster Modbus = new ModbusRTUMaster();
        List<byte> SerialMsg = new List<byte>();

        // Create delegate for sending progress of an operation to the Progress Report form
        public delegate void SendProgress(object sender, ProgressEventArgs e);
        private ProgressEventArgs ProgressArgs = new ProgressEventArgs();

        // Create handling of VFD default parameter background worker read event handling
        public event SendProgress ProgressEvent;

        // Create Excel file data read objects
        XL.Application xlApp;
        XL.Workbook xlWorkbook;
        XL._Worksheet xlWorksheet;
        XL.Range xlRange;
        List<V1000_File_Data> V1000_xlRead_List = new List<V1000_File_Data>();

        // Create VFD default parameter read objects 
        List<V1000_Param_Data> VFD_Vals = new List<V1000_Param_Data>();
        List<V1000_Param_Data> VFD_Modified_Vals = new List<V1000_Param_Data>();
        List<V1000_Param_Data> VFD_Sched_Chng_Vals = new List<V1000_Param_Data>();

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Start loading the Excel VFD parameter information
            statProgLabel.Text = "Loading VFD Parameter Information: ";
            statProgLabel.Visible = true;
            statProgress.Visible = true;

            ProgressArgs.ClearVFDLoadVals();
            bwrkLoadVFDDefs.RunWorkerAsync();

            // Load available serial ports
            cmbFuncCode.SelectedIndex = 0;
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cmbSerialPort.Items.Add(s);
            }

            // select last serial port, by default it seems the add-on port is always last.
            if (cmbSerialPort.Items.Count > 1)
            {
                cmbSerialPort.SelectedIndex = cmbSerialPort.Items.Count - 1;
            }
            else
                cmbSerialPort.SelectedIndex = 0;

            // text entry will default at the starting register for data read/writes.
            this.ActiveControl = txtStartReg; 
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (spVFD.IsOpen)
                spVFD.Close();
        }

        private void btnCreateModbusRTUMsg_Click(object sender, EventArgs e)
        {
            OutputMsg.ClearAll();

            try
            {
                OutputMsg.SlaveAddr = Convert.ToByte(txtSlaveAddr.Text, 16); // Get Slave Address

                switch (cmbFuncCode.SelectedIndex) // Get Function Code
                {
                    case 0:
                        OutputMsg.FuncCode = 0x03;
                        break;
                    case 1:
                        OutputMsg.FuncCode = 0x08;
                        break;
                    case 2:
                        OutputMsg.FuncCode = 0x10;
                        break;
                }

                OutputMsg.StartReg = Convert.ToUInt16(txtStartReg.Text, 16);  // Get Starting Register
                OutputMsg.RegCount = Convert.ToUInt16(txtRegCnt.Text);        // Get number of registers to be read or written
            }
            catch
            {
                MessageBox.Show("Entry Error!");
                return;
            }

            if (txtDataBuffer.Text != "")
                OutputMsg.Data = Modbus.CreateDataPayloadUShort(txtDataBuffer.Text);

            OutputMsg = Modbus.CreateMessage(OutputMsg);
            SerialMsg = Modbus.CreateRawMessageBuffer(OutputMsg, true);

            txtDataBuffComplete.Text = CreateModbusRTUDataString(SerialMsg);

            txtBuffSize.Text = (OutputMsg.Data.Count() * 2).ToString();
            txtModCRC16Result.Text = "0x" + OutputMsg.CRC16.ToString("X4");
            txtModCRC16Upper.Text = "0x" + ((byte)(OutputMsg.CRC16 & 0x00FF)).ToString("X2");
            txtModCRC16Lower.Text = "0x" + ((byte)(OutputMsg.CRC16 >> 8)).ToString("X2");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtStartReg.Clear();
            txtRegCnt.Clear();
            txtDataBuffer.Clear();
            txtBuffSize.Clear();
            txtModCRC16Result.Clear();
            txtModCRC16Upper.Clear();
            txtModCRC16Lower.Clear();
            txtDataBuffComplete.Clear();

            OutputMsg.Clear();
            ResponseMsg.ClearAll();

            txtSlaveAddr.Focus();
        }

        private string CreateDataBufferString(List<byte> p_DataBuffer)
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
                    if(p_Msg.FuncCode == ModbusRTUMaster.ReadReg)
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

        private void btnTransmit_Click(object sender, EventArgs e)
        {
            int stat = 0;
            ModbusRTUMsg msg = new ModbusRTUMsg();
            V1000_ModbusRTU_Comm comm = new V1000_ModbusRTU_Comm();

            if (cmbSerialPort.GetItemText(cmbSerialPort.SelectedItem) == "")
            {
                MessageBox.Show("Error! No Serial Port Selected!!");
                return;
            }

            msg = OutputMsg.Copy();
            comm.OpenCommPort(ref spVFD);
            stat = comm.DataTransfer(ref msg, ref spVFD);
            if (stat == 0x0001)
            {
                lbVFDResponse.Items.Add(CreateModbusRTUDataString(msg));   // Add raw message data byte breakdown to listbox.
                
                switch (msg.FuncCode)
                {
                    case ModbusRTUMaster.ReadReg:
                        dgvVFDResponse.Rows.Add(new string[]   {
                                                                    "0x" + msg.SlaveAddr.ToString("X2"),
                                                                    "0x" + msg.FuncCode.ToString("X2"),
                                                                    "0x" + msg.StartReg.ToString("X4"),
                                                                    msg.RegCount.ToString(),
                                                                    CreateModbusRTUDataString(msg.Data)
                                                                });
                        break;
                    case ModbusRTUMaster.WriteReg:
                        dgvVFDResponse.Rows.Add(new string[]    {
                                                                    "0x" + msg.SlaveAddr.ToString("X2"),
                                                                    "0x" + msg.FuncCode.ToString("X2"),
                                                                    "0x" + msg.StartReg.ToString("X4"),
                                                                    msg.RegCount.ToString(),
                                                                });
                        break;
                    case ModbusRTUMaster.Loopback:
                        dgvVFDResponse.Rows.Add(new string[]   {
                                                                    "0x" + msg.SlaveAddr.ToString("X2"),
                                                                    "0x" + msg.FuncCode.ToString("X2"),
                                                                    "0x" + msg.SubFunction.ToString("X4"),
                                                                    CreateModbusRTUDataString(msg.Data)
                                                                });
                        break;
                }
            }
            comm.CloseCommPort(ref spVFD);
        }

        private void cmbSerialPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            spVFD.PortName = cmbSerialPort.GetItemText(cmbSerialPort.SelectedItem);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvVFDResponse.Rows.Clear();
        }

        private void btnReadExcel_Click(object sender, EventArgs e)
        {
            if (!bwrkReadExcelFile.IsBusy)
            {
                btnReadExcel.Enabled = false;
                ProgressArgs.ClearXLReadVals();
                ProgressArgs.Mode_Sel = ProgressEventArgs.xlReadMode;
                ProgressArgs.xlRead_Stat = 0x01;
                frmProgReport frmXLRead = new frmProgReport("","Data Read Progress:", "Cancel Data Read");
                frmXLRead.ProgressCancelUpdated += new frmProgReport.ProgressCancelHandler(Progress_Cancel_Clicked);
                ProgressEvent += frmXLRead.ProgressReceived;
                bwrkReadExcelFile.RunWorkerAsync();
                frmXLRead.Show();
            }
        }

        private void bwrkReadExcelFile_DoWork(object sender, DoWorkEventArgs e)
        {
            xlApp = new XL.Application();
            xlWorkbook = xlApp.Workbooks.Open("C:\\Users\\sferry\\source\\repos\\V1000_Param_Prog\\V1000_Param_Prog\\data\\Parameter List.xlsx");
            xlWorksheet = xlWorkbook.Sheets[1];
            xlRange = xlWorksheet.UsedRange;

            V1000_xlRead_List.Clear();

            ProgressArgs.xlRead_Total_Units = xlRange.Rows.Count - 1;

            //for (int i = 2; i <= 21; i++)
            for (int i = 2; i <= xlRange.Rows.Count; i++)
            {
                ProgressArgs.xlRead_Unit = i - 1;
                ProgressArgs.xlRead_Progress = (byte)(((float)ProgressArgs.xlRead_Unit / ProgressArgs.xlRead_Total_Units) * 100);
                
                bwrkReadExcelFile.ReportProgress((int)i);
                if (bwrkReadExcelFile.CancellationPending)
                {
                    e.Cancel = true;
                    ProgressArgs.xlRead_Stat = 0x03;
                    bwrkReadExcelFile.ReportProgress(0);
                    return;
                }

                V1000_File_Data ParamData = new V1000_File_Data();

                if (xlRange.Cells[i, 1] != null && xlRange.Cells[i, 1].Value2 != null)
                    ParamData.RegAddress = xlRange.Cells[i, 1].Value2.ToString();
                else
                    ParamData.RegAddress = "0";

                if (xlRange.Cells[i, 2] != null && xlRange.Cells[i, 2].Value2 != null)
                    ParamData.ParamNum = xlRange.Cells[i, 2].Value2.ToString();
                else
                    ParamData.ParamNum = "0";

                if (xlRange.Cells[i, 3] != null && xlRange.Cells[i, 3].Value2 != null)
                    ParamData.ParamName = xlRange.Cells[i, 3].Value2.ToString();
                else
                    ParamData.ParamName = "0";

                if (xlRange.Cells[i, 4] != null && xlRange.Cells[i, 4].Value2 != null)
                    ParamData.DefVal = xlRange.Cells[i, 4].Value2.ToString();
                else
                    ParamData.DefVal = "0";

                if (xlRange.Cells[i, 5] != null && xlRange.Cells[i, 5].Value2 != null)
                    ParamData.Multiplier = xlRange.Cells[i, 5].Value2.ToString();
                else
                    ParamData.Multiplier = "1";

                V1000_xlRead_List.Add(ParamData);
            }

            ProgressArgs.xlRead_Progress = 100;
            ProgressArgs.xlRead_Stat = 0x02;
            e.Result = ProgressArgs.xlRead_Stat;
            bwrkReadExcelFile.ReportProgress(100);
        }

        private void btnReadVFDDefs_Click(object sender, EventArgs e)
        {
            if (V1000_xlRead_List.Count == 0)
            {
                MessageBox.Show("You must read the excel parameters first!!");
                return;
            }
            else
            {
                if (!bwrkReadVFDDefs.IsBusy)
                {
                    btnReadVFDDefs.Enabled = false;

                    // Initiation of progress reporting form pop-up.
                    ProgressArgs.ClearVFDReadVals();
                    ProgressArgs.Mode_Sel = ProgressEventArgs.VFDReadMode;
                    ProgressArgs.VFDRead_Stat = 0x01;
                    frmProgReport frmVFDProg = new frmProgReport("VFD Read Parameter:", "Data Read Progress", "Cancel VFD Read");
                    frmVFDProg.ProgressCancelUpdated += new frmProgReport.ProgressCancelHandler(Progress_Cancel_Clicked);
                    ProgressEvent += frmVFDProg.ProgressReceived;

                    // start background worker thread for reading all the VFD values
                    bwrkReadVFDDefs.RunWorkerAsync();

                    // show the progress form.
                    frmVFDProg.Show();
                }
            }
        }

        private void bwrkReadVFDDefs_DoWork(object sender, DoWorkEventArgs e)
        {
            int status = 0;
            V1000_ModbusRTU_Comm comm = new V1000_ModbusRTU_Comm();
            ModbusRTUMsg msg = new ModbusRTUMsg(0x1F);
            ModbusRTUMaster modbus = new ModbusRTUMaster();
            List<ushort> tmp = new List<ushort>();

            VFD_Vals.Clear();
            if (comm.OpenCommPort(ref spVFD) == 0x0001)
            {
                ProgressArgs.VFDRead_Total_Units = V1000_xlRead_List.Count;

                for (int i = 0; i < V1000_xlRead_List.Count; i++)
                {
                    ProgressArgs.VFDRead_Unit = i;
                    ProgressArgs.VFDRead_Progress = (byte)(((float)i / ProgressArgs.VFDRead_Total_Units) * 100);
                    bwrkReadVFDDefs.ReportProgress(ProgressArgs.VFDRead_Progress);
                    if (bwrkReadVFDDefs.CancellationPending)
                    {
                        e.Cancel = true;
                        bwrkReadVFDDefs.ReportProgress(0);
                        return;
                    }

                    V1000_Param_Data data = new V1000_Param_Data();

                    msg.Clear();
                    msg = modbus.CreateMessage(msg.SlaveAddr, ModbusRTUMaster.ReadReg, Convert.ToUInt16(V1000_xlRead_List[i].RegAddress, 16), 1, tmp);

                    status = comm.DataTransfer(ref msg, ref spVFD);
                    if (status == 0x0001)
                    {
                        // extract pertinent VFD parameter information the Excel File
                        data.ParamNum = V1000_xlRead_List[i].ParamNum;
                        data.ParamName = V1000_xlRead_List[i].ParamName;
                        data.Multiplier = Convert.ToUInt16(V1000_xlRead_List[i].Multiplier);

                        // Supplement the Excel file information with actual response from the VFD
                        data.RegAddress = msg.StartReg;
                        data.ParamVal = msg.Data[0];
                        data.DefVal = msg.Data[0];

                        VFD_Vals.Add(data);
                    }
                }

                ProgressArgs.VFDRead_Progress = 100;
                ProgressArgs.VFDRead_Stat = 0x02;
                e.Result = 0x02;
                comm.CloseCommPort(ref spVFD);
                bwrkReadVFDDefs.ReportProgress(100);
            }
        }

        private void btnWriteExcel_Click(object sender, EventArgs e)
        {
            if (V1000_xlRead_List.Count == 0)
            {
                MessageBox.Show("You must read the excel parameters first!!");
                return;
            }
            else if (VFD_Vals.Count == 0)
            {
                MessageBox.Show("You must read the default VFD parameters first!!");
                return;
            }
            else
            {
                if (!bwrkWriteExcelFile.IsBusy)
                {
                    btnWriteExcel.Enabled = false;
                    ProgressArgs.ClearXLWriteVals();
                    ProgressArgs.Mode_Sel = ProgressEventArgs.xlWriteMode;
                    ProgressArgs.VFDRead_Stat = 0x01;
                    frmProgReport frmProgWrite = new frmProgReport("", "Data Write Progress:", "Cancel Data Write");
                    frmProgWrite.ProgressCancelUpdated += new frmProgReport.ProgressCancelHandler(Progress_Cancel_Clicked);
                    ProgressEvent += frmProgWrite.ProgressReceived;
                    bwrkWriteExcelFile.RunWorkerAsync();
                    frmProgWrite.Show();
                }
            }
        }

        private void bwrkWriteExcelFile_DoWork(object sender, DoWorkEventArgs e)
        {
            xlApp = new XL.Application();
            xlWorkbook = xlApp.Workbooks.Add();
            xlWorksheet = xlWorkbook.Worksheets["Sheet1"];

            ProgressArgs.xlWrite_Total_Units = VFD_Vals.Count;

            xlWorksheet.Cells[1, 1].Value2 = "REGISTER ADDRESS";
            xlWorksheet.Cells[1, 2].Value2 = "PARAMETER NUMBER";
            xlWorksheet.Cells[1, 3].Value2 = "PARAMETER NAME";
            xlWorksheet.Cells[1, 4].Value2 = "DEFAULT VALUE";
            xlWorksheet.Cells[1, 5].Value2 = "MULTIPLIER";

            for (int i = 2; i <= ProgressArgs.xlWrite_Total_Units + 1; i++)
            {
                ProgressArgs.xlWrite_Unit = i;
                ProgressArgs.xlWrite_Progress = (byte)(((float)i / ProgressArgs.xlWrite_Total_Units) * 100);
                bwrkWriteExcelFile.ReportProgress(ProgressArgs.xlWrite_Progress);
                if (bwrkReadExcelFile.CancellationPending)
                {
                    e.Cancel = true;
                    bwrkReadExcelFile.ReportProgress(0);
                    return;
                }

                xlWorksheet.Cells[i, 1].Value2 = VFD_Vals[i - 2].RegAddress;
                xlWorksheet.Cells[i, 2].Value2 = VFD_Vals[i - 2].ParamNum;
                xlWorksheet.Cells[i, 3].Value2 = VFD_Vals[i - 2].ParamName;
                xlWorksheet.Cells[i, 4].Value2 = VFD_Vals[i - 2].DefVal;
                xlWorksheet.Cells[i, 5].Value2 = VFD_Vals[i - 2].Multiplier;
            }

            ProgressArgs.xlWrite_Progress = 100;
            ProgressArgs.xlWrite_Stat = 0x02;
            e.Result = 0x02;
            bwrkWriteExcelFile.ReportProgress(100);
        }
        
        private void bwrk_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressEvent?.Invoke(this, ProgressArgs);
        }

        private void bwrk_ProgressComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            switch (ProgressArgs.Mode_Sel)
            {
                case ProgressEventArgs.xlReadMode: // Excel Read+
                case ProgressEventArgs.xlWriteMode: // Excel Write
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    // release COM objects
                    if (ProgressArgs.Mode_Sel == ProgressEventArgs.xlReadMode)
                    {
                        Marshal.ReleaseComObject(xlRange);
                        btnReadExcel.Enabled = true;
                    }
                    if (ProgressArgs.Mode_Sel == ProgressEventArgs.xlWriteMode)
                    {
                        xlWorkbook.SaveAs("C:\\Users\\sferry\\source\\repos\\V1000_Param_Prog\\V1000_Param_Prog\\data\\Parameter Defaults.dat");
                        btnWriteExcel.Enabled = true;
                    }
                    xlWorkbook.Close();
                    xlApp.Quit();
                    Marshal.ReleaseComObject(xlWorkbook);
                    Marshal.ReleaseComObject(xlWorksheet);
                    Marshal.ReleaseComObject(xlApp);
                    break;
                case ProgressEventArgs.VFDReadMode: // VFD Read
                    btnReadVFDDefs.Enabled = true;
                    break;
            }
        }

        private void Progress_Cancel_Clicked(object sender, ProgressCancelArgs e)
        {
            switch (e.ProgMode)
            {
                case 0x00:
                    if (e.ReadCancel && bwrkReadExcelFile.IsBusy)
                    {
                        bwrkReadExcelFile.CancelAsync();
                    }
                    break;
                case 0x01:
                    if (e.ReadCancel && bwrkReadVFDDefs.IsBusy)
                    {
                        bwrkReadVFDDefs.CancelAsync();
                    }
                    break;
                case 0x02:
                    if (e.ReadCancel && bwrkWriteExcelFile.IsBusy)
                    {
                        bwrkWriteExcelFile.CancelAsync();
                    }
                    break;
            }
        }

        private void bwrkLoadVFDDefs_DoWork(object sender, DoWorkEventArgs e)
        {
            xlApp = new XL.Application();
            xlWorkbook = xlApp.Workbooks.Open("C:\\Users\\sferry\\source\\repos\\V1000_Drive_Programmer\\V1000_Drive_Programmer\\data\\Parameter Defaults.dat");
            //xlWorkbook = xlApp.Workbooks.Open("C:\\Users\\steve\\source\\repos\\V1000_Drive_Programmer\\V1000_Drive_Programmer\\data\\Parameter Defaults.dat");
            xlWorksheet = xlWorkbook.Sheets[1];
            xlRange = xlWorksheet.UsedRange;

            V1000_xlRead_List.Clear();

            ProgressArgs.VFDLoad_Total_Units = xlRange.Rows.Count - 1;

            for (int i = 2; i <= xlRange.Rows.Count; i++)
            {
                if (bwrkLoadVFDDefs.CancellationPending)
                {
                    e.Cancel = true;
                    bwrkLoadVFDDefs.ReportProgress(0);
                    return;
                }
                
                V1000_Param_Data ParamData = new V1000_Param_Data();
                
                if (xlRange.Cells[i, 1] != null && xlRange.Cells[i, 1].Value2 != null)
                    ParamData.RegAddress = Convert.ToUInt16(xlRange.Cells[i, 1].Value2);
                else
                    ParamData.RegAddress = 0;

                if (xlRange.Cells[i, 2] != null && xlRange.Cells[i, 2].Value2 != null)
                    ParamData.ParamNum = xlRange.Cells[i, 2].Value2.ToString();
                else
                    ParamData.ParamNum = "0";

                if (xlRange.Cells[i, 3] != null && xlRange.Cells[i, 3].Value2 != null)
                    ParamData.ParamName = xlRange.Cells[i, 3].Value2.ToString();
                else
                    ParamData.ParamName = "0";
                
                if (xlRange.Cells[i, 4] != null && xlRange.Cells[i, 4].Value2 != null)
                    ParamData.DefVal = Convert.ToUInt16(xlRange.Cells[i, 4].Value2);
                else
                    ParamData.DefVal = 0;

                if (xlRange.Cells[i, 5] != null && xlRange.Cells[i, 5].Value2 != null)
                    ParamData.Multiplier = Convert.ToUInt16(xlRange.Cells[i, 5].Value2);
                else
                    ParamData.Multiplier = 1;

                if (xlRange.Cells[i, 6] != null && xlRange.Cells[i, 6].Value2 != null)
                    ParamData.NumBase = Convert.ToByte(xlRange.Cells[i, 6].Value2);
                else
                    ParamData.NumBase = 10;

                if (xlRange.Cells[i, 7] != null && xlRange.Cells[i, 7].Value2 != null)
                    ParamData.Units = xlRange.Cells[i, 7].Value2.ToString();
                else
                    ParamData.Units = "";

                // Create a string version for display purposes of the actual default paramater value
                if (ParamData.NumBase == 16)
                    ParamData.DefValDisp = "0x" + ParamData.DefVal.ToString("X4");
                else
                    ParamData.DefValDisp = ((float)ParamData.DefVal / ParamData.Multiplier).ToString() + " " + ParamData.Units;

                VFD_Vals.Add(ParamData);

                ProgressArgs.VFDLoad_Unit = i - 2;
                ProgressArgs.VFDLoad_Progress = (byte)(((float)ProgressArgs.VFDLoad_Unit / ProgressArgs.VFDLoad_Total_Units) * 100);
                bwrkLoadVFDDefs.ReportProgress((int)(((float)i / (xlRange.Rows.Count - 1)) * 100));
            }

            ProgressArgs.VFDLoad_Unit = ProgressArgs.VFDLoad_Total_Units - 1;
            e.Result = 0x02;
            bwrkLoadVFDDefs.ReportProgress(100);
        }

        private void bwrkDGV_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            statProgress.Value = e.ProgressPercentage;
        }

        private void bwrkLoadVFDDefs_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Marshal.ReleaseComObject(xlRange);
            xlWorkbook.Close();
            xlApp.Quit();
            Marshal.ReleaseComObject(xlWorkbook);
            Marshal.ReleaseComObject(xlWorksheet);
            Marshal.ReleaseComObject(xlApp);

            for (int i = 0; i < VFD_Vals.Count; i++)
            {
                dgvParamViewFull.Rows.Add(new string[]
                {
                    ("0x" + VFD_Vals[i].RegAddress.ToString("X4")),
                    VFD_Vals[i].ParamNum,
                    VFD_Vals[i].ParamName,
                    VFD_Vals[i].DefValDisp
                });
            }

            statProgLabel.Text = "";
            statProgLabel.Visible = false;
            statProgress.Value = 0;
            statProgress.Visible = false;

            btnVFDRead.Enabled = true;
        }

        private void dgVFDParamView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            String chng_cell_val_str;
            Single chng_cell_val = 0;
            String def_cell_val_str = (String)dgvParamViewFull.Rows[index].Cells[3].Value;
            Single def_cell_val;

            /************************** Get the Default Cell Value *************************/
            // First check if the default parameter is a hex value so we can trim off the "0x" from the beginning
            if (VFD_Vals[index].NumBase == 16)
            {
                ushort temp_val = Convert.ToUInt16(def_cell_val_str.Substring(2), 16);
                def_cell_val = Convert.ToSingle(temp_val);
            }
            // Otherwise we need to trim off any units from the default cell value
            else
            {
                int unit_index = def_cell_val_str.IndexOf(' ');
                if (unit_index > 0)
                {
                    def_cell_val_str = def_cell_val_str.Substring(0, unit_index);
                    def_cell_val = Convert.ToSingle(def_cell_val_str);
                }
                else
                    def_cell_val = Convert.ToSingle(def_cell_val_str);

            }

            // First check if the default parameter is a hex value so we can trim off the "0x" from the beginning
            if (VFD_Vals[index].NumBase == 16)
            {
                ushort temp_val = Convert.ToUInt16(def_cell_val_str.Substring(2), 16);
                def_cell_val = Convert.ToSingle(temp_val);
            }
            // Otherwise we need to trim off any units from the default cell value
            else
            {
                int unit_index = def_cell_val_str.IndexOf(' ');
                if (unit_index > 0)
                {
                    def_cell_val_str = def_cell_val_str.Substring(0, unit_index);
                    def_cell_val = Convert.ToSingle(def_cell_val_str);
                }
                else
                    def_cell_val = Convert.ToSingle(def_cell_val_str);

            }

            // Attempt to read the new cell value. Put this in a try...catch to prevent 
            // an exception from a bad user entry.
            try
            {
                chng_cell_val_str = (String)dgvParamViewFull.Rows[index].Cells[4].Value;
                //first we need to check to see if the number base for this parameter is 10 or hex/16
                if (VFD_Vals[index].NumBase == 16)
                {
                    // Again, we need to trim off a 0x at front in the event the user entered it trying to signify that 
                    // they are using a hex value for the replacement value
                    int unit_index = chng_cell_val_str.IndexOf('x');
                    if (unit_index > 0)
                    {
                        chng_cell_val_str = chng_cell_val_str.Substring(unit_index + 1);
                        chng_cell_val = Convert.ToUInt16(chng_cell_val_str, 16);
                    }
                    // Otherwise we assume they are actually using the decimal value
                    else
                        chng_cell_val = Convert.ToUInt16(chng_cell_val_str, 10);
                }
                else
                {
                    // Again, trim off any units in the event the user actually entered something such as "Hz"
                    int unit_index = chng_cell_val_str.IndexOf(' ');
                    if (unit_index > 0)
                        chng_cell_val_str = chng_cell_val_str.Substring(0, unit_index);
                    // Next we convert the cell to a float/single value to account for any decimals
                    chng_cell_val = Convert.ToSingle(chng_cell_val_str);

                    //chng_cell_val = Convert.ToSingle((string)dgvVFDParamView.Rows[index].Cells[4].Value);
                }
            }
            catch
            {
                MessageBox.Show("Entry Error!!");
                dgvParamViewFull.Rows[index].Cells[4].Value = ((float)VFD_Vals[index].ParamVal / VFD_Vals[index].Multiplier).ToString();
                return;
            }

            // We multiply the temporary decimal value by the parameters standard multiplier.
            // and we convert the result of the multiplication to a 16-bit register value 
            // that both Modbus RTU and the V1000 require.
            Single temp_float = (chng_cell_val * VFD_Vals[index].Multiplier);
            ushort chng_param_val = (ushort)temp_float;


            // Check and see if the parameter value actually changed. Just double-clicking on the cell and 
            // hitting enter will cause this event to trigger even if the value does not change.
            if (chng_param_val != VFD_Vals[index].ParamVal)
            {
                // Copy the full parameter data to a list that contains scheduled changed values.
                VFD_Sched_Chng_Vals.Add((V1000_Param_Data)VFD_Vals[index].Clone());

                // Overwrite the copied current parameter value with new changed value
                VFD_Sched_Chng_Vals[VFD_Sched_Chng_Vals.Count - 1].ParamVal = chng_param_val;

                // And update the displayed parameter value as well
                if (VFD_Sched_Chng_Vals[VFD_Sched_Chng_Vals.Count - 1].NumBase == 16)
                    VFD_Sched_Chng_Vals[VFD_Sched_Chng_Vals.Count - 1].ParamValDisp = "0x" + VFD_Sched_Chng_Vals[VFD_Sched_Chng_Vals.Count - 1].ParamVal.ToString("X4");
                else
                    VFD_Sched_Chng_Vals[VFD_Sched_Chng_Vals.Count - 1].ParamValDisp = ((float)VFD_Sched_Chng_Vals[VFD_Sched_Chng_Vals.Count - 1].ParamVal / VFD_Sched_Chng_Vals[VFD_Sched_Chng_Vals.Count - 1].Multiplier).ToString() + " " + VFD_Sched_Chng_Vals[VFD_Sched_Chng_Vals.Count - 1].Units;

                // Clone the row with the changed value and add it to the Datagridview for scheduled parameter changes.
                DataGridViewRow ClonedRow = (DataGridViewRow)dgvParamViewFull.Rows[index].Clone();
                for (int i = 0; i < dgvParamViewFull.Rows[index].Cells.Count; i++)
                    ClonedRow.Cells[i].Value = dgvParamViewFull.Rows[index].Cells[i].Value;
                ClonedRow.Cells[4].Value = VFD_Sched_Chng_Vals[VFD_Sched_Chng_Vals.Count - 1].ParamValDisp;
                ClonedRow.DefaultCellStyle.BackColor = Color.White;
                dgvParamViewChng.Rows.Add(ClonedRow);

                // Fix the user entry to be the properly formatted string from any inaccuracies in formatting by the user.
                dgvParamViewFull.Rows[index].Cells[4].Value = VFD_Sched_Chng_Vals[VFD_Sched_Chng_Vals.Count - 1].ParamValDisp;

                // Highlight the scheduled changed parameter in the default parameter and current VFD parameter 
                // in Green-Yellow to signify that a change is scheduled for that particular parameter.
                dgvParamViewFull.Rows[index].DefaultCellStyle.BackColor = Color.GreenYellow;

                // If there is more than one modified parameter enable the Modify VFD Parameters button.
                if (VFD_Sched_Chng_Vals.Count > 0)
                    btnVFDMod.Enabled = true;
            }
            else
            {
            }
        }

        private void btnReadVFD_Click(object sender, EventArgs e)
        {
            if (!bwrkReadVFDVals.IsBusy)
            {
                dgvParamViewFull.Columns[4].ReadOnly = true;
                VFD_Modified_Vals.Clear();
                dgvParamViewMod.Rows.Clear();
                ProgressArgs.ClearVFDReadVals();    // Initialize the progress flags for a VFD read
                bwrkReadVFDVals.RunWorkerAsync();   // Start the separate thread for reading the current VFD parameter settings

                // Configure status bar for displaying VFD parameter read progress
                statProgLabel.Text = "VFD Parameter Value Read Progress: ";
                statProgLabel.Visible = true;
                statProgress.Visible = true;

                btnVFDRead.Enabled = false; // disable the Read VFD button while a read is in progress.
            }
        }

        private void bwrkReadVFDVals_DoWork(object sender, DoWorkEventArgs e)
        {
            int status = 0;
            V1000_ModbusRTU_Comm comm = new V1000_ModbusRTU_Comm();
            ModbusRTUMsg msg = new ModbusRTUMsg(0x1F);
            ModbusRTUMaster modbus = new ModbusRTUMaster();
            List<ushort> tmp = new List<ushort>();

            // proceed further only if opening of communication port is successful
            if (comm.OpenCommPort(ref spVFD) == 0x0001) 
            {
                ProgressArgs.VFDRead_Total_Units = VFD_Vals.Count;

                for (int i = 0; i < ProgressArgs.VFDLoad_Total_Units; i++)
                {
                    ProgressArgs.VFDRead_Unit = i;
                    ProgressArgs.VFDRead_Progress = (byte)(((float)i / ProgressArgs.VFDRead_Total_Units) * 100);
                    bwrkReadVFDVals.ReportProgress(ProgressArgs.VFDRead_Progress);
                    if (bwrkReadVFDVals.CancellationPending)
                    {
                        e.Cancel = true;
                        bwrkReadVFDVals.ReportProgress(0);
                        return;
                    }

                    msg.Clear();
                    msg = modbus.CreateMessage(msg.SlaveAddr, ModbusRTUMaster.ReadReg, VFD_Vals[i].RegAddress, 1, tmp);

                    status = comm.DataTransfer(ref msg, ref spVFD);
                    if (status == 0x0001)
                    {
                        VFD_Vals[i].ParamVal = msg.Data[0];

                        // Create a string version for display purposes of the actual paramater value
                        if (VFD_Vals[i].NumBase == 16)
                            VFD_Vals[i].ParamValDisp = "0x" + VFD_Vals[i].ParamVal.ToString("X4");
                        else
                            VFD_Vals[i].ParamValDisp = ((float)VFD_Vals[i].ParamVal / VFD_Vals[i].Multiplier).ToString() + " " + VFD_Vals[i].Units;
                    }
                }

                ProgressArgs.VFDRead_Progress = 100;
                ProgressArgs.VFDRead_Stat = 0x02;
                e.Result = 0x02;
                comm.CloseCommPort(ref spVFD);
                bwrkReadVFDVals.ReportProgress(100);
            }
        }

        private void bwrkReadVFDVals_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            // populate the VFD Value column on the VFD Parameter Values datagridview
            for (int i = 0; i < dgvParamViewFull.RowCount; i++)
            {
                dgvParamViewFull.Rows[i].Cells[4].Value = VFD_Vals[i].ParamValDisp;
                dgvParamViewFull.Rows[i].Cells[4].ReadOnly = false;
                   
                // Check if the read value from the VFD differs from the default parameter setting.
                // If it does add it to the modified parameters datagridview.
                if (VFD_Vals[i].ParamVal != VFD_Vals[i].DefVal)
                {
                    // Clone the row with the parameter that differs from the default value and add it to 
                    // the Datagridview for modified parameters. 
                    DataGridViewRow ClonedRow = (DataGridViewRow)dgvParamViewFull.Rows[i].Clone();
                    ClonedRow.DefaultCellStyle.BackColor = Color.White; // don't want a custom color row for this datagridview
                    for (int j = 0; j < dgvParamViewFull.ColumnCount; j++)
                        ClonedRow.Cells[j].Value = dgvParamViewFull.Rows[i].Cells[j].Value;
                    dgvParamViewMod.Rows.Add(ClonedRow);

                    // Turn the VFD Parameter Values datagridview row with the non-default parameter yellow to signify 
                    // that the particular parameter is set to a non-default value
                    dgvParamViewFull.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                }
                else
                {
                    // Set the backcolor for the row back to white. This is done because any additional read 
                    // showing that the value is now set to default will force the previously signified as 
                    // changed row back to a default white color.
                    dgvParamViewFull.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }

            // clear all the status bar values and set them as invisible
            statProgLabel.Text = "";
            statProgLabel.Visible = false;
            statProgress.Value = 0;
            statProgress.Visible = false;

            btnVFDRead.Enabled = true; // re-enable the VFD read button
            
        }

        private void btnModVFD_Click(object sender, EventArgs e)
        {
            if (!bwrkModVFD.IsBusy)
            {
                ProgressArgs.ClearVFDWriteVals();
                bwrkModVFD.RunWorkerAsync();

                // Configure status bar for displaying VFD parameter read progress
                statProgLabel.Text = "VFD Parameter Modification Progress: ";
                statProgLabel.Visible = true;
                statProgress.Visible = true;

                btnVFDMod.Enabled = false; // disable the Modify VFD button while a write is in progress.
            }
        }

        private void bwrkModVFD_DoWork(object sender, DoWorkEventArgs e)
        {
            int status = 0;
            V1000_ModbusRTU_Comm comm = new V1000_ModbusRTU_Comm();
            ModbusRTUMsg msg = new ModbusRTUMsg(0x1F);
            ModbusRTUMaster modbus = new ModbusRTUMaster();
            List<ushort> val = new List<ushort>();

            // proceed further only if opening of communication port is successful
            if (comm.OpenCommPort(ref spVFD) == 0x0001)
            {
                ProgressArgs.VFDWrite_Total_Units = VFD_Sched_Chng_Vals.Count;

                for (int i = 0; i < ProgressArgs.VFDWrite_Total_Units; i++)
                {
                    ProgressArgs.VFDWrite_Unit = i;
                    ProgressArgs.VFDWrite_Progress = (byte)(((float)i / ProgressArgs.VFDWrite_Total_Units) * 100);
                    bwrkModVFD.ReportProgress(ProgressArgs.VFDWrite_Progress);
                    if (bwrkModVFD.CancellationPending)
                    {
                        e.Cancel = true;
                        ProgressArgs.VFDWrite_Stat = ProgressEventArgs.Stat_Canceled;
                        bwrkModVFD.ReportProgress(0);
                        return;
                    }

                    msg.Clear();
                    val.Clear();
                    val.Add(VFD_Sched_Chng_Vals[i].ParamVal);
                    msg = modbus.CreateMessage(msg.SlaveAddr, ModbusRTUMaster.WriteReg, VFD_Sched_Chng_Vals[i].RegAddress, 1, val);

                    status = comm.DataTransfer(ref msg, ref spVFD);
                    if (status != 0x0001)
                    {
                        MessageBox.Show("VFD Parameter Update Failure!!");
                        e.Cancel = true;
                        ProgressArgs.VFDWrite_Stat = ProgressEventArgs.Stat_Error;
                        bwrkModVFD.ReportProgress(0);
                        break;
                    }
                }

                //
                if (status == 0x0001)
                {
                    // Update all the progress and status flags
                    ProgressArgs.VFDWrite_Progress = 100;
                    ProgressArgs.VFDWrite_Stat = ProgressEventArgs.Stat_Complete;
                    e.Result = 0x02;

                    // Save the parameter changes in the VFD
                    status = comm.SaveParamChanges(0x1F, ref spVFD);
                    if (status != 0x0001)
                        MessageBox.Show("VFD Modified Parameter Save Failure!!");
                    bwrkModVFD.ReportProgress(100);
                }

                // Close the communication port and report the thread as complete
                comm.CloseCommPort(ref spVFD);
            }
        }

        private void bwrkModVFD_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // clear all the status bar values and set them as invisible
            statProgLabel.Text = "";
            statProgLabel.Visible = false;
            statProgress.Value = 0;
            statProgress.Visible = false;

            btnVFDMod.Enabled = true; // re-enable the VFD read button

            if (ProgressArgs.VFDWrite_Stat == ProgressEventArgs.Stat_Complete)
            {
                VFD_Sched_Chng_Vals.Clear();
                dgvParamViewChng.Rows.Clear();
                btnReadVFD_Click(sender, (EventArgs) e);
            }
        }

    }

    #region Old Code



    #endregion

    public class ProgressEventArgs : EventArgs
    {
        // Mode Legend:
        public const byte xlReadMode = 0x01;
        public const byte xlWriteMode = 0x02;
        public const byte VFDReadMode = 0x03;

        public byte Mode_Sel = 0;

        // status legend:
        public const byte Stat_NotRunning = 0x00;
        public const byte Stat_Running = 0x01;
        public const byte Stat_Complete = 0x02;
        public const byte Stat_Canceled = 0x03;
        public const byte Stat_Error = 0xFF;

        public byte xlRead_Stat = 0;
        public byte xlRead_ErrCode = 0;
        public int  xlRead_Unit = 0;
        public int  xlRead_Total_Units = 0;
        public byte xlRead_Progress = 0;

        public byte   VFDRead_Stat = 0;
        public byte   VFDRead_ErrCode = 0;
        public int    VFDRead_Unit = 0;
        public int    VFDRead_Total_Units = 0;
        public byte   VFDRead_Progress = 0;
        public string VFDRead_ParamNum = "";
        public string VFDRead_ParamName = "";

        public byte xlWrite_Stat = 0;
        public byte xlWrite_ErrCode = 0;
        public int  xlWrite_Unit = 0;
        public int  xlWrite_Total_Units = 0;
        public byte xlWrite_Progress = 0;

        public byte VFDLoad_Stat = 0;
        public byte VFDLoad_ErrCode = 0;
        public int VFDLoad_Unit = 0;
        public int VFDLoad_Total_Units = 0;
        public byte VFDLoad_Progress = 0;

        public byte VFDWrite_Stat = 0;
        public byte VFDWrite_ErrCode = 0;
        public int VFDWrite_Unit = 0;
        public int VFDWrite_Total_Units = 0;
        public byte VFDWrite_Progress = 0;


        public ProgressEventArgs() { }

        public void ClearAll()
        {
            xlRead_Stat = 0;
            xlRead_ErrCode = 0;
            xlRead_Unit = 0;
            xlRead_Total_Units = 0;
            xlRead_Progress = 0;

            VFDRead_Stat = 0;
            VFDRead_ErrCode = 0;
            VFDRead_Unit = 0;
            VFDRead_Total_Units = 0;
            VFDRead_Progress = 0;
            VFDRead_ParamNum = "";
            VFDRead_ParamName = "";

            xlWrite_Stat = 0;
            xlWrite_ErrCode = 0;
            xlWrite_Unit = 0;
            xlWrite_Total_Units = 0;
            xlWrite_Progress = 0;

            VFDLoad_Stat = 0;
            VFDLoad_ErrCode = 0;
            VFDLoad_Unit = 0;
            VFDLoad_Total_Units = 0;
            VFDLoad_Progress = 0;

            VFDWrite_Stat = 0;
            VFDWrite_ErrCode = 0;
            VFDWrite_Unit = 0;
            VFDWrite_Total_Units = 0;
            VFDWrite_Progress = 0;
    }

        public void ClearXLReadVals()
        {
            xlRead_Stat = 0;
            xlRead_ErrCode = 0;
            xlRead_Unit = 0;
            xlRead_Total_Units = 0;
            xlRead_Progress = 0;
        }

        public void ClearVFDReadVals()
        {
            VFDRead_Stat = 0;
            VFDRead_ErrCode = 0;
            VFDRead_Unit = 0;
            VFDRead_Total_Units = 0;
            VFDRead_Progress = 0;
            VFDRead_ParamNum = "";
            VFDRead_ParamName = "";
        }

        public void ClearXLWriteVals()
        {
            xlWrite_Stat = 0;
            xlWrite_ErrCode = 0;
            xlWrite_Unit = 0;
            xlWrite_Total_Units = 0;
            xlWrite_Progress = 0;
        }

        public void ClearVFDLoadVals()
        {
            VFDLoad_Stat = 0;
            VFDLoad_ErrCode = 0;
            VFDLoad_Unit = 0;
            VFDLoad_Total_Units = 0;
            VFDLoad_Progress = 0;
        }

        public void ClearVFDWriteVals()
        {
            VFDWrite_Stat = 0;
            VFDWrite_ErrCode = 0;
            VFDWrite_Unit = 0;
            VFDWrite_Total_Units = 0;
            VFDWrite_Progress = 0;
        }
    }

}
