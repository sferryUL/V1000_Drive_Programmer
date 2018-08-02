﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using V1000_Param_Prog;
using XL = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.Data.OleDb;
using ModbusRTU;
using V1000_ModbusRTU;

namespace V1000_Param_Prog
{
    public partial class frmMain : Form
    {
        #region Global Class Object/Variable Declarations

        // Database Manipulation Variables
        //string DataDir = "C:\\Users\\steve\\source\\repos\\V1000_Drive_Programmer\\V1000_Drive_Programmer\\data\\";
        string DataDir = "C:\\Users\\sferry\\source\\repos\\V1000_Drive_Programmer\\V1000_Drive_Programmer\\data\\";
        string OLEBaseStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='";
        string OLEEndStr = "';Extended Properties='Excel 12.0 XML;HDR=YES;';";
        string DriveListFile = "DRIVE_LIST.XLSX";
        string MachDataFile = "MACH_DATA.XLSX";
        string dbFileExt = ".XLSX";

        DataTable dtDriveList = new DataTable();
        DataTable dtParamGrpDesc = new DataTable();
        DataTable dtParamListND = new DataTable();
        DataTable dtParamListHD = new DataTable();
        DataTable dtMachData = new DataTable();

        // VFD status and communication variables
        uint VFDReadRegCnt = 0;
        byte VFDSlaveAddr = 0x1F;

        // VFD Parameter Objects 
        ushort AccLvlRegAddr;
        ushort CtrlMethodRegAddr;

        ushort FreqRefRngLow;
        ushort FreqRefRngHi;

        string VoltSupplyParamNum;
        string VoltMaxOutParamNum;
        string FreqBaseParamNum;
        string RatedCurrParamNum;

        const byte VFD_V1000 = 0x01;

        List<V1000_Param_Data> Param_List;
        List<V1000_Param_Data> Param_List_ND = new List<V1000_Param_Data>();
        List<V1000_Param_Data> Param_List_HD = new List<V1000_Param_Data>();
        List<V1000_Param_Data> Param_Mod = new List<V1000_Param_Data>();
        List<V1000_Param_Data> Param_Chng = new List<V1000_Param_Data>();
        List<V1000_Param_Data> Param_Vrfy = new List<V1000_Param_Data>();

        List<string> ParamGrpList = new List<string>();

        // Background Worker status 
        ThreadProgressArgs ProgressArgs = new ThreadProgressArgs();

        // Datagridview Existing Value Storage
        string dgvCellVal;

        #endregion

        #region Main Form Functions

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            txtSlaveAddr.Text = "0x1F";
            txtSlaveAddr.SelectionLength = 0;


            // Load available serial ports
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
                cmbSerialPort.Items.Add(s);

            // select last serial port, by default it seems the add-on port is always last.
            if (cmbSerialPort.Items.Count > 1)
                cmbSerialPort.SelectedIndex = cmbSerialPort.Items.Count - 1;
            else
                cmbSerialPort.SelectedIndex = 0;

            // Get the list of VFDs available and fill the drive list combo box.
            string conn_str = OLEBaseStr + DataDir + DriveListFile + OLEEndStr;
            if (SQLGetTable(conn_str, ref dtDriveList))
            {
                foreach (DataRow dr in dtDriveList.Rows)
                {
                    string str = dr["UL_PARTNUM"].ToString() + " - " + dr["UL_DESC"].ToString();
                    cmbDriveList.Items.Add(str);
                }
            }

            GetMachData();

            SetVFDCommBtnEnable(false, false, false, false);
            cmbDriveList.Focus();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (spVFD.IsOpen)
                spVFD.Close();
        }

        #endregion

        #region Combobox Functions

        private void cmbSerialPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            spVFD.PortName = cmbSerialPort.GetItemText(cmbSerialPort.SelectedItem);
        }

        private void cmbDriveSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string file, conn_str;
            
            DataRow row = dtDriveList.Rows[cmbDriveList.SelectedIndex];

            

            if (GetParamList(row, "PARAM_ND_LIST", ref dtParamListND, ref Param_List_ND))
            {
                SetDriveParamConsts(VFD_V1000);
                if (row["PARAM_HD_LIST"].ToString() != "")
                {
                    GetParamList(row, "PARAM_HD_LIST", ref dtParamListHD, ref Param_List_HD);
                }
            }

            if (Param_List_HD.Count > 0)
            {
                cmbDriveDuty.SelectedIndex = 1;
                cmbDriveDuty.Enabled = true;
            }
            else
            {
                cmbDriveDuty.SelectedIndex = 0;
                cmbDriveDuty.Enabled = false;
            }

            RefreshParamViews();

            if (GetParamGrpList(row, "PARAM_GRP_DESC", ref dtParamGrpDesc) && (cmbParamGroup.Items.Count > 0))
            {
                cmbParamGroup.Enabled = true;
                cmbParamGroup.SelectedIndex = 0;
            }

            // Enable buttons, comboboxes, and text boxes after reading all the drive setting information
            SetVFDCommBtnEnable(true, true, false, false);  // Turn on the Read & Reinitialize buttons
            btnParamMachSet.Enabled = true;                 // Turn on the Set Machine Parameters button
            cmbVoltMach.Enabled = true;                     // enable all the machine specific parameter setting comboboxes
            cmbVoltMotorMax.Enabled = true;
            cmbFreqMotorBase.Enabled = true;
            txtFLA.Enabled = true;                          // Turn on the Motor FLA textbox
            msFile_LoadParamList.Enabled = true;            // Allow a parameter update spreadsheet to be loaded

            grpSetMach.Enabled = true;
        }

        private void cmbDriveDuty_SelectedIndexChanged(object sender, EventArgs e)
        {

            // Set the Param_List list object to point to the appropriate list based on the drive duty selection
            if (cmbDriveDuty.SelectedIndex == 0)
                Param_List = Param_List_ND;
            else
                Param_List = Param_List_HD;

            if(dgvParamViewFull.Rows.Count > 0)
                RefreshParamViews();    // Refresh the Full Parameter List datagridview
        }

        private void cmbParamGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRow row = dtParamGrpDesc.Rows[cmbParamGroup.SelectedIndex];
            int index = Convert.ToInt16(row["LIST_IDX"].ToString());

            dgvParamViewFull.Rows[index].Selected = true;
            dgvParamViewFull.FirstDisplayedScrollingRowIndex = index;

        }

        private void cmbVoltMachSupply_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbVoltMotorMax.SelectedIndex = cmbVoltMach.SelectedIndex;
        }

        private void cmbVoltMotorMax_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVoltMach.SelectedIndex == -1)
                cmbVoltMach.SelectedIndex = cmbVoltMotorMax.SelectedIndex;
        }

        private void cmbSelMach_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRow row = dtMachData.Rows[cmbSelMach.SelectedIndex];

            cmbMotorSel.Items.Clear();
            int drive_cnt = Convert.ToInt32(row["VFD_CNT"].ToString());
            if (drive_cnt > 1)
            {
                for (int i = 4; i <= 6; i++)
                {
                    if (row[i].ToString() != "")
                        cmbMotorSel.Items.Add(row[i].ToString());
                }
                cmbMotorSel.Enabled = true;
            }
            else
            {
                cmbMotorSel.Items.Add(row[4].ToString());
                cmbMotorSel.SelectedIndex = 0;
                cmbMotorSel.Enabled = false;
            }
        }
        #endregion

        #region Textbox Functions

        private void txtSlaveAddr_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyValue == (int)Keys.Enter) || (e.KeyValue == (int)Keys.Tab))
            {
                // Check if the value in the textbox is hex or base 10. If the  
                // value is invalid temp_val will be 0 and we just set the value 
                // back to the default 0x1F
                byte temp_val = Hex2Byte(txtSlaveAddr.Text);
                if (temp_val > 0)
                    VFDSlaveAddr = temp_val;
                else
                    VFDSlaveAddr = 0x1F;

                // Reformat the the text to be displayed as standard hexadecimal format.
                txtSlaveAddr.Text = "0x" + VFDSlaveAddr.ToString("X2");

                // Set the focus elsewhere and reload the parameter list
                if (btnVFDRead.Enabled)
                {
                    cmbDriveSel_SelectedIndexChanged(sender, e);
                    btnVFDRead.Focus();
                }
                else if (cmbDriveList.SelectedIndex == -1)
                    cmbDriveList.Focus();
                else
                    cmbDriveSel_SelectedIndexChanged(sender, e);

            }
        }

        #endregion

        #region Datagridview Functions

        private void dgvParamViewFull_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Single chng_val = 0;
            Single def_val;

            // try to get default and changed cell values in a version that can be compared
            try
            {
                def_val = Cell2Single((String)dgvParamViewFull.Rows[e.RowIndex].Cells[3].Value, Param_List[e.RowIndex]);
                chng_val = Cell2Single((String)dgvParamViewFull.Rows[e.RowIndex].Cells[4].Value, Param_List[e.RowIndex]);
            }
            // if an exception is thrown, then just reset the cell value back
            // to what it was before the edit took place
            catch
            {
                MessageBox.Show("Entry Error!");
                dgvParamViewFull.Rows[e.RowIndex].Cells[4].Value = dgvCellVal;
                return;
            }

            // We multiply the temporary decimal value by the parameters standard multiplier.
            // and we convert the result of the multiplication to a 16-bit register value 
            // that both Modbus RTU and the V1000 require.
            Single temp_float = (chng_val * Param_List[e.RowIndex].Multiplier);
            ushort chng_param_val = (ushort)temp_float;

            UpdateParamViews(chng_param_val, e.RowIndex);
        }

        

        private void dgvParamViewFull_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            dgvCellVal = (string)(dgvParamViewFull.Rows[e.RowIndex].Cells[4].Value);
        }

        #endregion

        #region Command Button Enable/Disable Functions

        private void SetVFDCommBtnEnable(bool p_ReadEn, bool p_InitEn, bool p_ModEnd, bool p_VerEn)
        {
            btnVFDRead.Enabled = p_ReadEn;
            btnVFDReset.Enabled = p_InitEn;
            btnVFDMod.Enabled = p_ModEnd;
            btnVFDVer.Enabled = p_VerEn;
        }

        private void SetVFDCommBtnEnable(int p_Val)
        {
            if ((p_Val & 0x0001) > 0)
                btnVFDRead.Enabled = true;
            else
                btnVFDRead.Enabled = false;

            if ((p_Val & 0x0002) > 0)
                btnVFDReset.Enabled = true;
            else
                btnVFDReset.Enabled = false;

            if ((p_Val & 0x0004) > 0)
                btnVFDMod.Enabled = true;
            else
                btnVFDMod.Enabled = false;

            if ((p_Val & 0x0008) > 0)
                btnVFDVer.Enabled = true;
            else
                btnVFDVer.Enabled = false;
        }

        private int GetVFDCommBtnStat()
        {
            int RetVal = 0x0000;

            if (btnVFDRead.Enabled)
                RetVal |= 0x0001;
            if (btnVFDReset.Enabled)
                RetVal |= 0x0002;
            if (btnVFDMod.Enabled)
                RetVal |= 0x0004;
            if (btnVFDVer.Enabled)
                RetVal |= 0x0008;

            return RetVal;
        }

        #endregion

        #region VFD Parameter Read Functions

        private void btnReadVFD_Click(object sender, EventArgs e)
        {
            if (!bwrkReadVFDVals.IsBusy)
            {
                VFDReadRegCnt = 0;
                dgvParamViewFull.Columns[4].ReadOnly = true;
                Param_Mod.Clear();
                dgvParamViewMisMatch.Rows.Clear();
                ProgressArgs.ClearVFDReadVals();    // Initialize the progress flags for a VFD read
                bwrkReadVFDVals.RunWorkerAsync();   // Start the separate thread for reading the current VFD parameter settings

                // Configure status bar for displaying VFD parameter read progress
                SetStatusBar(true, "VFD Parameter Value Read Progress: ");

                lblParamMismatch.Text = "Drive Modified Parameters:";
                cmMisMatchDefVal.HeaderText = "Default Value";
                
                // disable the VFD communication buttons while a read is in progress.
                SetVFDCommBtnEnable(false, false, false, false);
            }
        }

        private void bwrkReadVFDVals_DoWork(object sender, DoWorkEventArgs e)
        {
            int status = 0;
            V1000_ModbusRTU_Comm comm = new V1000_ModbusRTU_Comm();
            ModbusRTUMsg msg = new ModbusRTUMsg(VFDSlaveAddr);
            ModbusRTUMaster modbus = new ModbusRTUMaster();
            List<ushort> tmp = new List<ushort>();

            // proceed further only if opening of communication port is successful
            if (comm.OpenCommPort(ref spVFD) == 0x0001)
            {
                ProgressArgs.VFDRead_Total_Units = Param_List.Count;

                for (int i = 0; i < ProgressArgs.VFDRead_Total_Units; i++)
                {
                    ProgressArgs.VFDRead_Unit = i;
                    ProgressArgs.VFDRead_Progress = (byte)(((float)i / ProgressArgs.VFDRead_Total_Units) * 100);
                    bwrkReadVFDVals.ReportProgress(ProgressArgs.VFDRead_Progress);

                    msg.Clear();
                    msg = modbus.CreateMessage(msg.SlaveAddr, ModbusRTUMaster.ReadReg, Param_List[i].RegAddress, 1, tmp);

                    status = comm.DataTransfer(ref msg, ref spVFD);
                    if (status == 0x0001)
                    {
                        VFDReadRegCnt++;
                        Param_List[i].ParamVal = msg.Data[0];
                    }
                    else
                        Param_List[i].ParamVal = 0;
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
            // First check and see if there was any data received to even process
            if (VFDReadRegCnt > 0)
            {
                // populate the VFD Value column on the VFD Parameter Values datagridview
                for (int i = 0; i < dgvParamViewFull.RowCount; i++)
                {
                    dgvParamViewFull.Rows[i].Cells[4].Value = Param_List[i].ParamValDisp;
                    dgvParamViewFull.Rows[i].Cells[4].ReadOnly = false;

                    // Check if the read value from the VFD differs from the default parameter setting.
                    // If it does add it to the modified parameter list and modified parameters datagridview.
                    if (Param_List[i].ParamVal != Param_List[i].DefVal)
                    {
                        Param_Mod.Add(Param_List[i]);
                        dgvParamViewMisMatch.Rows.Add(CloneRow(dgvParamViewFull, i));
                        dgvParamViewFull.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    }
                    // Otherwise just turn the full parameter view row back to white in case it was previously changed.
                    else
                        dgvParamViewFull.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
                SetVFDCommBtnEnable(true, true, false, false);
            }
            else
            {
                cmbDriveSel_SelectedIndexChanged(sender, e);
                MessageBox.Show("Error Reading Parameter Values from the VFD, Check the connection, and drive slave address and try again!");
            }

            // clear all the status bar values and set them as invisible
            SetStatusBar(false);

        }
        #endregion

        #region VFD Reset Drive Back to Their Default Settings

        private void btnVFDReset_Click(object sender, EventArgs e)
        {
            V1000_ModbusRTU_Comm comm = new V1000_ModbusRTU_Comm();
            ModbusRTUMsg msg = new ModbusRTUMsg(VFDSlaveAddr);
            ModbusRTUMaster modbus = new ModbusRTUMaster();
            List<ushort> val = new List<ushort>();

            SetStatusBar(true, "Clearing VFD Custom Parameter Settings");
            msg.Clear();
            val.Clear();
            val.Add(2220);
            msg = modbus.CreateMessage(msg.SlaveAddr, ModbusRTUMaster.WriteReg, 0x0103, 1, val);

            if (comm.OpenCommPort(ref spVFD) == 0x0001)
            {
                SetVFDCommBtnEnable(false, false, false, false);
                int status = comm.DataTransfer(ref msg, ref spVFD);
                if (status != 0x0001)
                    MessageBox.Show("VFD Parameter Reset to Default Failure!!");
                else
                {
                    // Reset was successful, close the communication port before proceeding.
                    // The ReadVFD thread will handle its own needs for the comm port.
                    comm.CloseCommPort(ref spVFD);

                    // click the Read VFD button to refresh the datagridview for the full parameter 
                    // list and clear the datagridview for the modified parameter list 
                    btnReadVFD_Click(sender, e);
                }
            }

            SetStatusBar(false);
        }

        #endregion

        #region VFD Write Scheduled Parameter Changes

        private void btnModVFD_Click(object sender, EventArgs e)
        {
            if (!bwrkModVFD.IsBusy)
            {
                ProgressArgs.ClearVFDWriteVals();
                ProgressArgs.VFDWrite_Stat = ThreadProgressArgs.Stat_Running;
                bwrkModVFD.RunWorkerAsync();
                
                SetStatusBar(true, "VFD Parameter Modification Progress: ");    // Configure status bar for displaying VFD parameter read progress
                btnVFDMod.Enabled = false;                                      // disable the Modify VFD button while a write is in progress.
            }
        }

        private void bwrkModVFD_DoWork(object sender, DoWorkEventArgs e)
        {
            int status = 0;
            V1000_ModbusRTU_Comm comm = new V1000_ModbusRTU_Comm();
            ModbusRTUMsg msg = new ModbusRTUMsg(VFDSlaveAddr);
            ModbusRTUMaster modbus = new ModbusRTUMaster();
            List<ushort> val = new List<ushort>();

            ParamListSend2Back(FreqRefRngLow, FreqRefRngHi, ref Param_Chng);
            ParamListSend2Back(AccLvlRegAddr, AccLvlRegAddr, ref Param_Chng);
            
            // proceed further only if opening of communication port is successful
            if (comm.OpenCommPort(ref spVFD) == 0x0001)
            {
                ProgressArgs.VFDWrite_Total_Units = Param_Chng.Count;

                for (int i = 0; i < ProgressArgs.VFDWrite_Total_Units; i++)
                {
                    ProgressArgs.VFDWrite_Unit = i;
                    ProgressArgs.VFDWrite_Progress = (byte)(((float)i / ProgressArgs.VFDWrite_Total_Units) * 100);
                    bwrkModVFD.ReportProgress(ProgressArgs.VFDWrite_Progress);
                    
                    msg.Clear();
                    val.Clear();
                    val.Add(Param_Chng[i].ParamVal);
                    msg = modbus.CreateMessage(msg.SlaveAddr, ModbusRTUMaster.WriteReg, Param_Chng[i].RegAddress, 1, val);

                    status = comm.DataTransfer(ref msg, ref spVFD);
                    if (status != 0x0001)
                    {
                        MessageBox.Show("VFD Parameter Update Failure!!");
                        e.Cancel = true;
                        ProgressArgs.VFDWrite_Stat = ThreadProgressArgs.Stat_Error;
                        bwrkModVFD.ReportProgress(0);
                        break;
                    }
                }

                if (status == 0x0001)
                {
                    // Update all the progress and status flags
                    ProgressArgs.VFDWrite_Progress = 100;
                    ProgressArgs.VFDWrite_Stat = ThreadProgressArgs.Stat_Complete;
                    e.Result = 0x02;

                    // Save the parameter changes in the VFD
                    status = comm.SaveParamChanges(VFDSlaveAddr, ref spVFD);
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
            SetStatusBar(false);

            SetVFDCommBtnEnable(GetVFDCommBtnStat() | 0x08);
            btnVFDMod.Enabled = true; // re-enable the VFD read button

            if (ProgressArgs.VFDWrite_Stat == ThreadProgressArgs.Stat_Complete)
            {
                Param_Chng.Clear();
                dgvParamViewChng.Rows.Clear();
                MessageBox.Show("VFD Programming Complete!!");
            }
            else
                MessageBox.Show("VFD Programming Failed!!");
       }


        #endregion

        #region VFD Verify Parameter Settings

        private void btnVFDVer_Click(object sender, EventArgs e)
        {
            if (!bwrkVFDVerify.IsBusy)
            {
                Param_Vrfy.Clear();
                ProgressArgs.ClearVFDVerVals();
                ProgressArgs.VFDVer_Stat = ThreadProgressArgs.Stat_Running;
                bwrkVFDVerify.RunWorkerAsync();

                // Configure status bar for displaying VFD modified parameter verification progress
                SetStatusBar(true, "VFD Parameter Parameter Setting Verification Progress:");

                lblParamMismatch.Text = "Drive Mismatched Parameter Values";
                cmMisMatchDefVal.HeaderText = "Specified Value";

                dgvParamViewMisMatch.Rows.Clear(); // clear the mismatch datagridview

                btnVFDVer.Enabled = false; // disable the Modify VFD button while a write is in progress.
            }
        }

        private void bwrkVFDVerify_DoWork(object sender, DoWorkEventArgs e)
        {
            int status = 0;
            V1000_ModbusRTU_Comm comm = new V1000_ModbusRTU_Comm();
            ModbusRTUMsg msg = new ModbusRTUMsg(VFDSlaveAddr);
            ModbusRTUMaster modbus = new ModbusRTUMaster();
            List<ushort> val = new List<ushort>();

            // proceed further only if opening of communication port is successful
            if (comm.OpenCommPort(ref spVFD) == 0x0001)
            {
                ProgressArgs.VFDVer_Total_Units = Param_List.Count;

                for (int i = 0; i < ProgressArgs.VFDVer_Total_Units; i++)
                {
                    //Set progress reporting values and check for cancellation of the thread.
                    ProgressArgs.VFDVer_Unit = i;
                    ProgressArgs.VFDVer_Progress = (byte)(((float)i / ProgressArgs.VFDVer_Total_Units) * 100);
                    bwrkVFDVerify.ReportProgress(ProgressArgs.VFDVer_Progress);
                    
                    msg.Clear();
                    val.Clear();
                    val.Add(Param_List[i].ParamVal);
                    msg = modbus.CreateMessage(msg.SlaveAddr, ModbusRTUMaster.ReadReg, Param_List[i].RegAddress, 1, val);

                    status = comm.DataTransfer(ref msg, ref spVFD);
                    if (status != 0x0001)
                    {
                        MessageBox.Show("VFD Parameter Verification Failed!!");
                        e.Cancel = true;
                        ProgressArgs.VFDVer_Stat = ThreadProgressArgs.Stat_Error;
                        bwrkVFDVerify.ReportProgress(0);
                        break;
                    }
                    else
                    {
                        Param_Vrfy.Add((V1000_Param_Data)Param_List[i].Clone());
                        Param_Vrfy[i].ParamVal = msg.Data[0];
                    }
                }

                // Close the communication port
                comm.CloseCommPort(ref spVFD);

                // Update all the progress and status flags
                ProgressArgs.VFDVer_Progress = 100;
                ProgressArgs.VFDVer_Stat = ThreadProgressArgs.Stat_Complete;
                e.Result = 0x02;
                bwrkVFDVerify.ReportProgress(100);
            }
        }

        private void bwrkVFDVerify_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            SetStatusBar(false);        // clear all the status bar values and set them as invisible
            btnVFDVer.Enabled = true;   // re-enable the VFD read button

            if (ProgressArgs.VFDVer_Stat == ThreadProgressArgs.Stat_Complete)
            {
                // First make an exact copy of the master parameter list and then alter the 
                // parameter values of the ones that we are verifying should have been modified. 
                List<V1000_Param_Data> param_chk = new List<V1000_Param_Data>();
                for (int i = 0; i < Param_List.Count; i++)
                {
                    param_chk.Add((V1000_Param_Data)Param_List[i].Clone());
                    if (Param_List[i].ParamVal == 0)
                        param_chk[i].ParamVal = param_chk[i].DefVal;
                }

                // Alter the verification list to have the changed parameter values
                for (int i = 0; i < Param_Chng.Count; i++)
                {
                    int idx = GetParamIndex(Param_Chng[i].RegAddress, Param_List);
                    if (param_chk[idx].RegAddress == CtrlMethodRegAddr)
                        param_chk[idx].ParamVal = 0;
                    else
                        param_chk[idx].ParamVal = Param_Chng[i].ParamVal;
                }

                if (param_chk.Count != Param_Vrfy.Count)
                    MessageBox.Show("Parameter verification failed! Number of parameters read from drive does not match total number of parameters!");
                else
                {
                    for (int i = 0; i < param_chk.Count; i++)
                    {
                        if (Param_Vrfy[i].ParamVal != param_chk[i].ParamVal)
                        {
                            // Clone the row with the parameter that differs from the default value and add it to 
                            // the Datagridview for modified parameters. 
                            dgvParamViewMisMatch.Rows.Add(CloneRow(dgvParamViewFull, i));
                            int idx = dgvParamViewMisMatch.RowCount - 1;
                            dgvParamViewMisMatch.Rows[idx].Cells[3].Value = param_chk[i].ParamVal;
                            dgvParamViewMisMatch.Rows[idx].Cells[4].Value = Param_Vrfy[i].ParamVal;


                            ProgressArgs.VFDVer_ParamMismatch_Cnt++; // Increment parameter mismatch count
                        }
                    }

                    if (ProgressArgs.VFDVer_ParamMismatch_Cnt > 0)
                        MessageBox.Show("VFD parameter setting verification failed!. See mismatch parameter list for details.");
                    else
                        MessageBox.Show("VFD parameter setting verification successful!");
                }

            }
        }

        #endregion

        #region Database Functions
        private bool GetMachData()
        {
            bool ret_val = false;

            // Get the list of VFDs available and fill the drive list combo box.
            string conn_str = OLEBaseStr + DataDir + MachDataFile + OLEEndStr;
            if (SQLGetTable(conn_str, ref dtMachData))
            {
                foreach (DataRow dr in dtMachData.Rows)
                {
                    string str = dr["MACH_CODE"].ToString() + " - " + dr["MACH_DESC"].ToString();
                    cmbSelMach.Items.Add(str);
                }
            }

            return ret_val;
        }

        private bool GetParamList(DataRow p_Row, string p_Col, ref DataTable p_Tbl, ref List<V1000_Param_Data> p_List)
        {
            bool ret_val = false;

            string file = p_Row[p_Col].ToString() + dbFileExt;
            string conn_str = OLEBaseStr + DataDir + file + OLEEndStr;

            // Get the table containing the list of parameters automatically modified by a 
            // heavy-duty setting and fill  the the Param_HD_Mods list with all the values.
            if (SQLGetTable(conn_str, ref p_Tbl))
            {
                ret_val = true;

                p_List.Clear();
                foreach (DataRow dr in p_Tbl.Rows)
                {
                    V1000_Param_Data param = new V1000_Param_Data();
                    V1000SQLtoParam(dr, ref param);
                    p_List.Add(param);
                }
            }

            return ret_val;
        }

        private bool GetParamGrpList(DataRow p_Row, string p_Col, ref DataTable p_Tbl)
        {
            bool ret_val = false;

            cmbParamGroup.Items.Clear();

            // Get the list of parameter groupings available and fill the Parameter group combobox
            string file = p_Row[p_Col].ToString() + dbFileExt;
            string conn_str = OLEBaseStr + DataDir + file + OLEEndStr;

            if (SQLGetTable(conn_str, ref p_Tbl))
            {
                ret_val = true;
                foreach (DataRow dr in p_Tbl.Rows)
                {
                    string str = dr["PARAM_GRP"].ToString() + " - " + dr["GRP_DESC"].ToString();
                    cmbParamGroup.Items.Add(str);
                }
            }

            return ret_val;
        }

        public bool SQLGetTable(string p_ConnStr, ref DataTable p_Tbl, string p_Query = "SELECT * FROM [SHEET1$]")
        {
            bool RetVal = false;

            using (OleDbConnection dbConn = new OleDbConnection(p_ConnStr))
            {
                if (dbConn.State == ConnectionState.Closed)
                {
                    dbConn.Open();
                    if (dbConn.State == ConnectionState.Open)
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(p_Query, dbConn);
                        DataSet ds = new DataSet();
                        try
                        {
                            da.Fill(ds);
                            p_Tbl.Clear();
                            p_Tbl = ds.Tables[0];
                            if (p_Tbl.Rows.Count > 0)
                                RetVal = true;
                            else
                                RetVal = false;
                        }
                        catch
                        {
                            MessageBox.Show("Database Error!");
                            RetVal = false;
                        }

                        dbConn.Close();

                    }
                    else
                        RetVal = false;
                }
            }
            return RetVal;
        }

        public void V1000SQLtoParam(DataRow p_dr, ref V1000_Param_Data p_Data)
        {
            p_Data.RegAddress = Convert.ToUInt16(p_dr[1].ToString());
            p_Data.ParamNum = p_dr[2].ToString();
            p_Data.ParamName = p_dr[3].ToString();
            p_Data.Multiplier = Convert.ToUInt16(p_dr[5].ToString());
            p_Data.NumBase = Convert.ToByte(p_dr[6].ToString());
            p_Data.Units = p_dr[7].ToString();
            p_Data.DefVal = Convert.ToUInt16(p_dr[4].ToString());
        }

        #endregion

        #region Text Manipulation Functions

        private Single Cell2Single(string p_CellVal, V1000_Param_Data p_Param)
        {
            Single RetVal = 0;


            // First check if the default parameter is a hex value so we can trim off the "0x" from the beginning
            if (p_Param.NumBase == 16)
            {
                // Now check and see if the value is actually a hex value
                if ((p_CellVal.IndexOf('x') > 0) || (p_CellVal.IndexOf('X') > 0))
                {
                    ushort temp_val = Convert.ToUInt16(p_CellVal.Substring(2), 16);
                    RetVal = Convert.ToSingle(temp_val);
                }
                // If not just convert it to a single and treat it as a base 10 value
                else
                    RetVal = Convert.ToSingle(p_CellVal);
            }
            // Otherwise we need to trim off any units from the default cell value
            else
            {
                RetVal = Cell2Single(p_CellVal);
            }

            return RetVal;
        }

        private Single Cell2Single(string p_CellVal)
        {
            Single RetVal = -1;

            int unit_index = p_CellVal.IndexOf(' ');
            if (unit_index > 0)
            {
                p_CellVal = p_CellVal.Substring(0, unit_index);
                RetVal = Convert.ToSingle(p_CellVal);
            }
            // If there are no units then just convert the cell value to a single
            else
                RetVal = Convert.ToSingle(p_CellVal);

            return RetVal;
        }

        private Double Cell2Double(string p_CellVal, V1000_Param_Data p_Param)
        {
            Double RetVal = 0;


            // First check if the default parameter is a hex value so we can trim off the "0x" from the beginning
            if (p_Param.NumBase == 16)
            {
                // Now check and see if the value is actually a hex value
                if ((p_CellVal.IndexOf('x') > 0) || (p_CellVal.IndexOf('X') > 0))
                {
                    ushort temp_val = Convert.ToUInt16(p_CellVal.Substring(2), 16);
                    RetVal = Convert.ToDouble(temp_val);
                }
                // If not just convert it to a single and treat it as a base 10 value
                else
                    RetVal = Convert.ToDouble(p_CellVal);
            }
            // Otherwise we need to trim off any units from the default cell value
            else
            {
                RetVal = Cell2Double(p_CellVal);
            }

            return RetVal;
        }

        private Double Cell2Double(string p_CellVal, byte p_RoundVal = 2)
        {
            Double RetVal = -1;

            int unit_index = p_CellVal.IndexOf(' ');
            if (unit_index > 0)
            {
                p_CellVal = p_CellVal.Substring(0, unit_index);
                RetVal = Math.Round(Convert.ToDouble(p_CellVal), p_RoundVal);
            }
            // If there are no units then just convert the cell value to a single
            else
                RetVal = Math.Round(Convert.ToDouble(p_CellVal), p_RoundVal);

            return RetVal;
        }

        private ushort Cell2RegVal(string p_CellVal, V1000_Param_Data p_Param)
        {
            double val;
            ushort RegVal = 0;

            val = Cell2Double(p_CellVal, GetRoundCnt(p_Param.Multiplier));
            RegVal = (ushort)(val * p_Param.Multiplier);

            return RegVal;
        }

        private byte GetRoundCnt(ushort p_Multiplier)
        {
            byte RoundCnt = 0;

            switch (p_Multiplier)
            {
                case 1:
                    RoundCnt = 0;
                    break;
                case 10:
                    RoundCnt = 1;
                    break;
                case 100:
                    RoundCnt = 2;
                    break;
                case 1000:
                    RoundCnt = 3;
                    break;
            }

            return RoundCnt;
        }

        private byte Hex2Byte(string p_CellVal)
        {
            byte RetVal = 0;

            try
            {
                // Check and see if the value is actually a hex value
                if ((p_CellVal.IndexOf('x') > 0) || (p_CellVal.IndexOf('X') > 0))
                {
                    byte temp_val = Convert.ToByte(p_CellVal.Substring(2), 16);
                    RetVal = Convert.ToByte(temp_val);
                }
                // If not just convert it to a single and treat it as a base 10 value
                else
                    RetVal = Convert.ToByte(p_CellVal);
            }
            catch
            {
                MessageBox.Show("Entry Error!!");
            }

            return RetVal;
        }

        

        #endregion

        #region ToolStrip and Context Menu Functions

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctxtDriveMod_Opening(object sender, CancelEventArgs e)
        {
            if (Param_Mod.Count > 0)
            {
                ctxtDriveMod_Save.Enabled = true;
                ctxtDriveMod_Clear.Enabled = true;
            }
            else
            {
                ctxtDriveMod_Save.Enabled = false;
                ctxtDriveMod_Clear.Enabled = false;
            }
        }

        private void ctxtSchedChng_Opening(object sender, CancelEventArgs e)
        {
            if (msFile_LoadParamList.Enabled)
                ctxtSchedChng_Load.Enabled = true;
            else
                ctxtSchedChng_Load.Enabled = false;

            if (Param_Chng.Count > 0)
            {
                ctxtSchedChng_Save.Enabled = true;
                ctxtSchedChng_Clear.Enabled = true;
            }
            else
            {
                ctxtSchedChng_Save.Enabled = false;
                ctxtSchedChng_Clear.Enabled = false;
            }
        }

        private void clearScheduledChangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Param_Chng.Clear();
            RefreshParamViews();
        }

        private void clearListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearParamMismatch();
            RefreshParamViews();
        }

        private void SetStatusBar(bool p_Vis, string p_Str = "", int p_Val = 0)
        {
            statProgLabel.Visible = p_Vis;
            statProgress.Visible = p_Vis;
            statProgLabel.Text = p_Str;
            statProgress.Value = p_Val;
        }

        private void bwrkDGV_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            statProgress.Value = e.ProgressPercentage;
        }

        #endregion

        #region Parameter File Reading and Writing Functions
        private void SaveParams(object sender, EventArgs e)
        {
            XL.Application xlApp = new XL.Application();
            XL.Workbook xlWorkbook = xlApp.Workbooks.Add();
            XL._Worksheet xlWorksheet = xlWorkbook.Worksheets["Sheet1"];
            SaveFileDialog svfd = new SaveFileDialog();
            List<V1000_Param_Data> param_save = new List<V1000_Param_Data>();

            ToolStripMenuItem owner = (ToolStripMenuItem)sender;
            if (owner.Name == "ctxtDriveMod_Save")
                param_save = Param_Mod;
            else if (owner.Name == "ctxtSchedChng_Save")
                param_save = Param_Chng;

            svfd.Filter = "Excel 2007 Workbook (*.xlsx)|*.xlsx";
            svfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (svfd.ShowDialog() == DialogResult.OK)
            {
                string filename = svfd.FileName;


                xlWorksheet.Cells[1, 1].Value2 = "PARAMETER NUMBER";
                xlWorksheet.Cells[1, 2].Value2 = "PARAMETER NAME";
                xlWorksheet.Cells[1, 3].Value2 = "PARAMETER VALUE";
                xlWorksheet.Cells[1, 4].Value2 = "PARAMETER UNITS";

                for (int i = 0; i < param_save.Count; i++)
                {
                    xlWorksheet.Cells[(i + 2), 1].Value2 = param_save[i].ParamNum;
                    xlWorksheet.Cells[(i + 2), 2].Value2 = param_save[i].ParamName;
                    xlWorksheet.Cells[(i + 2), 3].Value2 = ((float)param_save[i].ParamVal / param_save[i].Multiplier);
                    xlWorksheet.Cells[(i + 2), 4].Value2 = param_save[i].Units;
                }
            
                GC.Collect();
                GC.WaitForPendingFinalizers();
                xlApp.DisplayAlerts = false;
                xlWorkbook.SaveAs(filename, ConflictResolution: XL.XlSaveConflictResolution.xlLocalSessionChanges);
                xlWorkbook.Close();
                xlApp.Quit();

                // release COM objects
                Marshal.ReleaseComObject(xlWorkbook);
                Marshal.ReleaseComObject(xlWorksheet);
                Marshal.ReleaseComObject(xlApp);
            }
        }

        private void LoadParams(object sender, EventArgs e)
        {
            OpenFileDialog opfd = new OpenFileDialog();
            List<V1000_File_Data> file_list = new List<V1000_File_Data>();

            // Clear scheduled change list and datagridview
            Param_Chng.Clear();
            dgvParamViewChng.Rows.Clear();

            opfd.Filter = "Excel 2007 Workbook (*.xlsx)|*.xlsx";
            opfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (opfd.ShowDialog() == DialogResult.OK)
            {
                XL.Application xlApp = new XL.Application();
                XL._Workbook xlWorkbook = xlApp.Workbooks.Open(opfd.FileName);
                XL._Worksheet xlWorksheet = xlWorkbook.Worksheets["Sheet1"];
                XL.Range xlRange = xlWorksheet.UsedRange;

                int cnt = xlRange.Rows.Count;
                // Get all the parameter values from the Excel spreadsheet
                for (int i = 2; i <= cnt; i++)
                {
                    V1000_File_Data file_data = new V1000_File_Data();

                    if (xlRange.Cells[i, 1] != null && xlRange.Cells[i, 1].Value2 != null)
                        file_data.ParamNum = xlRange.Cells[i, 1].Value2.ToString();
                    else
                        file_data.ParamNum = "0";

                    if (xlRange.Cells[i, 3] != null && xlRange.Cells[i, 3].Value2 != null)
                        file_data.Value = xlRange.Cells[i, 3].Value2.ToString();
                    else
                        file_data.ParamNum = "0";
                    file_list.Add(file_data);
                }

                // find the full parameter information from the master list for each
                // of the changed parameters acquired from the Excel spreadsheet
                for (int i = 0; i < file_list.Count; i++)
                {
                    // Need to check potentially every single parameter in the master list
                    // for a match
                    for (int j = 0; j < Param_List.Count; j++)
                    {
                        // See if the parameter numbers match
                        if (Param_List[j].ParamNum == file_list[i].ParamNum)
                        {
                            double val = Math.Round(Convert.ToDouble(file_list[i].Value), 2);
                            ushort chng_val = (ushort)(val * Param_List[j].Multiplier);
                            UpdateParamViews(chng_val, j);
                            break;
                        }
                    }
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
                xlWorkbook.Close();
                xlApp.Quit();

                // release COM objects
                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);
                Marshal.ReleaseComObject(xlWorkbook);
                Marshal.ReleaseComObject(xlApp);

                // Now search the master parameter list and find all the full parameter
                // information for each item that was in the spreadsheet.
            }
        }

        #endregion

        #region Machine Specific Functions
        private void btnSetMotorVals(object sender, EventArgs e)
        {
            ushort volt_supply = 0, volt_out = 0, freq_base = 0, fla = 0;

            if ((cmbVoltMach.SelectedIndex == -1) || (cmbFreqMotorBase.SelectedIndex == -1) || (txtFLA.Text == ""))
            {
                MessageBox.Show("Machine supply voltage, supply frequency, and motor FLA must have valid entries!");
                return;
            }

            // Get all the index values in the full parameter list for each of these parameters
            int idx_volt_supply = GetParamIndex(VoltSupplyParamNum, Param_List);
            int idx_volt_out = GetParamIndex(VoltMaxOutParamNum, Param_List);
            int idx_freq_base = GetParamIndex(FreqBaseParamNum, Param_List);
            int idx_fla = GetParamIndex(RatedCurrParamNum, Param_List);

            if ((idx_volt_supply > 0) && (idx_volt_out > 0) && (idx_freq_base > 0) && (idx_fla > 0))
            {
                try
                {
                    volt_supply = Cell2RegVal((string)cmbVoltMach.SelectedItem, Param_List[idx_volt_supply]);
                    volt_out = Cell2RegVal((string)cmbVoltMotorMax.SelectedItem, Param_List[idx_volt_out]);
                    freq_base = Cell2RegVal((string)cmbFreqMotorBase.SelectedItem, Param_List[idx_freq_base]);
                    fla = Cell2RegVal(txtFLA.Text, Param_List[idx_fla]);
                }
                catch
                {
                    MessageBox.Show("Entry Error!!");
                    return;
                }

                UpdateParamViews(volt_supply, idx_volt_supply); // Set supply voltage parameter
                UpdateParamViews(volt_out, idx_volt_out);       // Set the maximum output voltage parameter
                UpdateParamViews(freq_base, idx_freq_base);     // Set the base frequency parameter
                UpdateParamViews(fla, idx_fla);                 // Set the motor rated current parameter
            }
            else
            {
                MessageBox.Show("Parameter Location Error!!");
            }
        }
        #endregion

        #region Parameter View Functions

        private void UpdateParamViews(ushort p_NewParamVal, int p_Index)
        {
            bool val_chng = false;

            if (VFDReadRegCnt > 0)
            {
                if (p_NewParamVal != Param_List[p_Index].ParamVal)
                    val_chng = true;
            }
            else
            {
                if (p_NewParamVal != Param_List[p_Index].DefVal)
                    val_chng = true;
            }

            // Check and see if the parameter value actually changed. Just double-clicking on the cell and 
            // hitting enter will cause this event to trigger even if the value does not change.
            if (val_chng)
            {
                // Check and see if this parameter is already scheduled to be changed. 
                V1000_Param_Data param = new V1000_Param_Data();
                param = (V1000_Param_Data)Param_List[p_Index].Clone();

                int chng_index = -1;
                for (int i = 0; i < Param_Chng.Count; i++)
                {
                    // If there is a register address match then the parameter was scheduled for change
                    if (Param_Chng[i].RegAddress == param.RegAddress)
                    {
                        chng_index = i;                         // Set the change index
                        Param_Chng[i].ParamVal = p_NewParamVal; // Update the Param_Chng parameter value

                        // Update the both the Full List and Scheduled Change datagridviews
                        dgvParamViewChng.Rows[i].Cells[4].Value = Param_Chng[i].ParamValDisp;
                        dgvParamViewFull.Rows[p_Index].Cells[4].Value = Param_Chng[i].ParamValDisp;
                        break;
                    }
                }

                // If the change index is less then 0 then this parameter is not already scheduled to be 
                // changed. We add it to the Param_Chng list as well as to the Scheduled Change datagridview
                if (chng_index < 0)
                {

                    Param_Chng.Add((V1000_Param_Data)Param_List[p_Index].Clone());  // Copy the full parameter data to a list that contains scheduled changed values.
                    Param_Chng[Param_Chng.Count - 1].ParamVal = p_NewParamVal; // Overwrite the copied current parameter value with new changed value
                    Param_Chng.Sort(); // Sort parameter change list in ascending order based on the parameter number

                    // Clone the row with the changed value and add it to the Datagridview for scheduled parameter changes.
                    dgvParamViewChng.Rows.Add(CloneRow(dgvParamViewFull, p_Index));
                    dgvParamViewChng.Rows[dgvParamViewChng.RowCount - 1].Cells[4].Value = Param_Chng[Param_Chng.Count - 1].ParamValDisp;

                    // Fix the user entry to be the properly formatted string from any inaccuracies in formatting by the user.
                    dgvParamViewFull.Rows[p_Index].Cells[4].Value = Param_Chng[Param_Chng.Count - 1].ParamValDisp;

                    // Highlight the scheduled changed parameter in the default parameter and current VFD parameter 
                    // in Green-Yellow to signify that a change is scheduled for that particular parameter.
                    dgvParamViewFull.Rows[p_Index].DefaultCellStyle.BackColor = Color.GreenYellow;

                    dgvParamViewChng.Sort(dgvParamViewChng.Columns[1], ListSortDirection.Ascending);

                    // If there is more than one modified parameter enable the Modify VFD Parameters button.
                    if ((Param_Chng.Count > 0) && (VFDReadRegCnt > 0))
                        btnVFDMod.Enabled = true;
                }
            } // if(val_chng)
            else
            {
                // First check and see if the VFD has been read or not. 
                if (VFDReadRegCnt > 0)
                {
                    // If it has the set the VFD value back to what the display formatted  value
                    // was when it was originally read.
                    dgvParamViewFull.Rows[p_Index].Cells[4].Value = Param_List[p_Index].ParamValDisp;

                    // Check and see if that VFD value was the same as the default value or not.
                    // If it was different than the default value set the row color to yellow,
                    // if it was the same as the default value set the row color back to white.
                    if (Param_List[p_Index].ParamVal != Param_List[p_Index].DefVal)
                        dgvParamViewFull.Rows[p_Index].DefaultCellStyle.BackColor = Color.Yellow;
                    else
                        dgvParamViewFull.Rows[p_Index].DefaultCellStyle.BackColor = Color.White;
                }
                else
                // If the VFD has not been read then set the value back to blank and set the 
                // row color back to white because it may have been changed previously.
                {
                    dgvParamViewFull.Rows[p_Index].Cells[4].Value = Param_List[p_Index].DefValDisp;
                    dgvParamViewFull.Rows[p_Index].DefaultCellStyle.BackColor = Color.White;
                }

                // Check and see if this value was scheduled to be changed
                V1000_Param_Data param = new V1000_Param_Data();
                param = (V1000_Param_Data)Param_List[p_Index].Clone();
                for (int i = 0; i < Param_Chng.Count; i++)
                {
                    // We determine if the parameter was scheduled to change by the register 
                    // address. If we find a match we remove it from the list of scheduled
                    // changes as well as the Schedule Change datagridview.
                    if (Param_Chng[i].RegAddress == param.RegAddress)
                    {
                        Param_Chng.RemoveAt(i);
                        dgvParamViewChng.Rows.RemoveAt(i);
                        break;
                    }
                }
            } // else if (val_change)

            if (Param_Chng.Count > 0)
                SetVFDCommBtnEnable((GetVFDCommBtnStat() | (byte)0x0C)); // Turn on Modify VFD and Verify VFD buttons by turning on bits 2 & 3
            else
                SetVFDCommBtnEnable((GetVFDCommBtnStat() & (byte)0xF3)); // Turn off Modify VFD and Verify BFD buttons by turning off bits 2 & 3
        }

        private void RefreshParamViews()
        {
            string ReadVal = "";

            dgvParamViewFull.Rows.Clear();  // Clear the Full Parameter List datagridview

            // Populate the Full Parameter List datagridview 
            for (int i = 0; i < Param_List.Count; i++)
            {
                if (VFDReadRegCnt > 0)
                    ReadVal = Param_List[i].ParamValDisp;

                dgvParamViewFull.Rows.Add(new string[]
                    {
                            ("0x" + Param_List[i].RegAddress.ToString("X4")),
                            Param_List[i].ParamNum,
                            Param_List[i].ParamName,
                            Param_List[i].DefValDisp,
                            ReadVal
                    });

                // Clear the read-only flag for each populated datagridview row
                dgvParamViewFull.Rows[i].Cells[4].ReadOnly = false;
            }

            // Update row colors to green-yellow based on any scheduled changes
            if (Param_Chng.Count > 0)
            {
                for (int i = 0; i < Param_Chng.Count; i++)
                {
                    int idx = GetParamIndex(Param_Chng[i].RegAddress, Param_List);
                    dgvParamViewFull.Rows[idx].DefaultCellStyle.BackColor = Color.GreenYellow;
                }
            }
            else
                dgvParamViewChng.Rows.Clear();

            // Update row colors to yellow based on any parameters read from the VFD that don't match default values
            if (Param_Mod.Count > 0)
            {
                for (int i = 0; i < Param_Mod.Count; i++)
                {
                    int idx = GetParamIndex(Param_Mod[i].RegAddress, Param_List);
                    dgvParamViewFull.Rows[idx].DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
            else
                dgvParamViewMisMatch.Rows.Clear();
        }

        private DataGridViewRow CloneRow(DataGridView p_DGV, int p_Idx)
        {
            DataGridViewRow row = new DataGridViewRow();

            row = (DataGridViewRow)p_DGV.Rows[p_Idx].Clone();   // Copy the row

            // Copy the contents of each column
            for (int i = 0; i < p_DGV.ColumnCount; i++)
                row.Cells[i].Value = p_DGV.Rows[p_Idx].Cells[i].Value;

            row.DefaultCellStyle.BackColor = Color.White;       // Reset the row color as white

            return row;
        }

        #endregion

        #region Parameter List Functions

        private void ParamListCopy(ref List<V1000_Param_Data> p_Mstr, ref List<V1000_Param_Data> p_Copy)
        {
            p_Copy.Clear();
            for (int i = 0; i < p_Mstr.Count; i++)
                p_Copy.Add((V1000_Param_Data)p_Mstr[i].Clone());
        }

        private void ParamListUpdate(ref List<V1000_Param_Data> p_Mstr, ref List<V1000_Param_Data> p_NewVals, byte p_Mode)
        {
            // Mode 1 - Default Value
            // Mode 2 - Parameter Value
            // Mode 3 - Both Values
            for (int i = 0; i < p_NewVals.Count; i++)
            {
                int index = GetParamIndex(p_NewVals[i].RegAddress, p_Mstr);  // Find the index of the parameter 
                switch (p_Mode)
                {
                    case 0x01:
                        p_Mstr[index].DefVal = p_NewVals[i].DefVal;
                        break;
                    case 0x02:
                        p_Mstr[index].ParamVal = p_NewVals[i].ParamVal;
                        break;
                    case 0x03:
                        p_Mstr[index].DefVal = p_NewVals[i].DefVal;
                        p_Mstr[index].DefVal = p_NewVals[i].ParamVal;
                        break;
                }
            }
        }

        private void ClearParamMismatch()
        {
            Param_Mod.Clear();
            for (int i = 0; i < Param_List.Count; i++)
                Param_List[i].ParamVal = 0;
        }

        private int GetParamIndex(string p_ParamNum, List<V1000_Param_Data> p_List)
        {
            int idx = -1;

            for (int i = 0; i < p_List.Count; i++)
            {
                if (p_List[i].ParamNum == p_ParamNum)
                {
                    idx = i;
                    break;
                }
            }

            return idx;
        }

        private int GetParamIndex(ushort p_RegAddr, List<V1000_Param_Data> p_List)
        {
            int Index = -1;

            for (int i = 0; i < p_List.Count; i++)
            {
                if (p_List[i].RegAddress == p_RegAddr)
                {
                    Index = i;
                    break;
                }
            }

            return Index;
        }

        void ParamListSend2Back(int p_RngLo, int p_RngHi, ref List<V1000_Param_Data> p_List)
        {
            int idx = 0;

            for (int i = 0; i < Param_Chng.Count; i++)
            {
                if ((p_List[idx].RegAddress >= p_RngLo) && (p_List[idx].RegAddress <= p_RngHi))
                {
                    V1000_Param_Data param_temp = new V1000_Param_Data();
                    param_temp = (V1000_Param_Data)p_List[idx].Clone();
                    p_List.RemoveAt(idx);
                    p_List.Add(param_temp);
                }
                else
                {
                    if (++idx >= Param_List.Count)
                        break;
                }
            }
        }

        #endregion

        #region VFD Specific Functions
        public void SetDriveParamConsts(byte p_DriveType)
        {
            switch (p_DriveType)
            {
                case VFD_V1000:
                    AccLvlRegAddr = V1000_Param_Data.AccLvlReg;
                    CtrlMethodRegAddr = V1000_Param_Data.RegCtrlMethod;

                    FreqRefRngLow = V1000_Param_Data.FreqRefRngLow;
                    FreqRefRngHi = V1000_Param_Data.FreqRefRngHi;

                    VoltSupplyParamNum = V1000_Param_Data.VoltSuppParam;
                    VoltMaxOutParamNum = V1000_Param_Data.VoltMaxOutParam;
                    FreqBaseParamNum = V1000_Param_Data.FreqBaseParam;
                    RatedCurrParamNum = V1000_Param_Data.RatedCurrParam;

                    break;
            }
        }
        #endregion

        
    }

    public class ThreadProgressArgs : EventArgs
    {
        // Mode Legend:
        public const byte VFDReadMode = 0x00;
        public const byte VFDWriteMode = 0x01;
        public const byte VFDVerMode = 0x02;

        public byte Mode_Sel = 0;

        // status legend:
        public const byte Stat_NotRunning = 0x00;
        public const byte Stat_Running = 0x01;
        public const byte Stat_Complete = 0x02;
        public const byte Stat_Canceled = 0x03;
        public const byte Stat_Error = 0xFF;

        public byte     VFDRead_Stat = 0;
        public byte     VFDRead_ErrCode = 0;
        public int      VFDRead_Unit = 0;
        public int      VFDRead_Total_Units = 0;
        public byte     VFDRead_Progress = 0;

        public byte     VFDWrite_Stat = 0;
        public byte     VFDWrite_ErrCode = 0;
        public int      VFDWrite_Unit = 0;
        public int      VFDWrite_Total_Units = 0;
        public byte     VFDWrite_Progress = 0;

        public byte     VFDVer_Stat = 0;
        public byte     VFDVer_ErrCode = 0;
        public int      VFDVer_Unit = 0;
        public int      VFDVer_Total_Units = 0;
        public byte     VFDVer_Progress = 0;
        public int      VFDVer_ParamMismatch_Cnt = 0;

        public ThreadProgressArgs() { }

        public void ClearAll()
        {
            VFDRead_Stat = 0;
            VFDRead_ErrCode = 0;
            VFDRead_Unit = 0;
            VFDRead_Total_Units = 0;
            VFDRead_Progress = 0;

            VFDWrite_Stat = 0;
            VFDWrite_ErrCode = 0;
            VFDWrite_Unit = 0;
            VFDWrite_Total_Units = 0;
            VFDWrite_Progress = 0;

            VFDVer_Stat = 0;
            VFDVer_ErrCode = 0;
            VFDVer_Unit = 0;
            VFDVer_Total_Units = 0;
            VFDVer_Progress = 0;
            VFDVer_ParamMismatch_Cnt = 0;
    }

        public void ClearVFDReadVals()
        {
            VFDRead_Stat = 0;
            VFDRead_ErrCode = 0;
            VFDRead_Unit = 0;
            VFDRead_Total_Units = 0;
            VFDRead_Progress = 0;
        }

        public void ClearVFDWriteVals()
        {
            VFDWrite_Stat = 0;
            VFDWrite_ErrCode = 0;
            VFDWrite_Unit = 0;
            VFDWrite_Total_Units = 0;
            VFDWrite_Progress = 0;
        }

        public void ClearVFDVerVals()
        {
            VFDVer_Stat = 0;
            VFDVer_ErrCode = 0;
            VFDVer_Unit = 0;
            VFDVer_Total_Units = 0;
            VFDVer_Progress = 0;
            VFDVer_ParamMismatch_Cnt = 0;
        }
    }
}
