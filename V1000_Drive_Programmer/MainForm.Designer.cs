namespace V1000_Drive_Programmer
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.txtSlaveAddr = new System.Windows.Forms.TextBox();
            this.lblSlaveAddr = new System.Windows.Forms.Label();
            this.grpCommSettings = new System.Windows.Forms.GroupBox();
            this.cmbSerialPort = new System.Windows.Forms.ComboBox();
            this.lblSerialCommPort = new System.Windows.Forms.Label();
            this.spVFD = new System.IO.Ports.SerialPort(this.components);
            this.dgvParamViewFull = new System.Windows.Forms.DataGridView();
            this.cmRegAddr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmParamNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmParmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmDefVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmVFDVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statProgLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.btnVFDRead = new System.Windows.Forms.Button();
            this.bwrkReadVFDVals = new System.ComponentModel.BackgroundWorker();
            this.ctxtSchedChng = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxtSchedChng_Load = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxtSchedChng_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxtSchedChng_Remove = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxtSchedChng_Clear = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxtDriveMod = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxtDriveMod_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxtDriveMod_Clear = new System.Windows.Forms.ToolStripMenuItem();
            this.btnVFDMod = new System.Windows.Forms.Button();
            this.bwrkModVFD = new System.ComponentModel.BackgroundWorker();
            this.grpSetDrive = new System.Windows.Forms.GroupBox();
            this.lblDriveDuty = new System.Windows.Forms.Label();
            this.cmbDriveDuty = new System.Windows.Forms.ComboBox();
            this.lblParamFullList = new System.Windows.Forms.Label();
            this.lblParamGroup = new System.Windows.Forms.Label();
            this.lblDriveSel = new System.Windows.Forms.Label();
            this.cmbDriveList = new System.Windows.Forms.ComboBox();
            this.cmbParamGroup = new System.Windows.Forms.ComboBox();
            this.dgvParamViewMisMatch = new System.Windows.Forms.DataGridView();
            this.cmMisMatchParamAddr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmMisMatchParamNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmMisMatchParamName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmMisMatchDefVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmMisMatchReadVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblParamMismatch = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnVFDVer = new System.Windows.Forms.Button();
            this.btnVFDReset = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.msFile = new System.Windows.Forms.ToolStripMenuItem();
            this.msFile_LoadParamList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.msFile_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblParamModSched = new System.Windows.Forms.Label();
            this.dgvParamViewChng = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblParamChngSrc = new System.Windows.Forms.Label();
            this.txtParamChngSrc = new System.Windows.Forms.TextBox();
            this.lblVoltMachSupply = new System.Windows.Forms.Label();
            this.cmbMtrVoltSupply = new System.Windows.Forms.ComboBox();
            this.lblVoltMotorMax = new System.Windows.Forms.Label();
            this.cmbMtrVoltMax = new System.Windows.Forms.ComboBox();
            this.lblFreqMotorBase = new System.Windows.Forms.Label();
            this.cmbMtrFreqBase = new System.Windows.Forms.ComboBox();
            this.lblMotorFLC = new System.Windows.Forms.Label();
            this.lblUnitsAmps1 = new System.Windows.Forms.Label();
            this.txtMtrFLC = new System.Windows.Forms.TextBox();
            this.btnMtrSet = new System.Windows.Forms.Button();
            this.grpParamChng = new System.Windows.Forms.GroupBox();
            this.cmbMtrFreqSupply = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbMtrPartNum = new System.Windows.Forms.ComboBox();
            this.lblMotorPartNum = new System.Windows.Forms.Label();
            this.cmbMachSel = new System.Windows.Forms.ComboBox();
            this.lblSelMach = new System.Windows.Forms.Label();
            this.lblSelMotor = new System.Windows.Forms.Label();
            this.bwrkVFDVerify = new System.ComponentModel.BackgroundWorker();
            this.grpSetMotor = new System.Windows.Forms.GroupBox();
            this.btnMtrDel = new System.Windows.Forms.Button();
            this.btnMtrStore = new System.Windows.Forms.Button();
            this.grpSetMach = new System.Windows.Forms.GroupBox();
            this.txtMachDrvName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMachChrtCnt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMachDrvCnt = new System.Windows.Forms.TextBox();
            this.btnMachListDel = new System.Windows.Forms.Button();
            this.btnMachListLoad = new System.Windows.Forms.Button();
            this.btnMachListStore = new System.Windows.Forms.Button();
            this.cmbMachChrtNum = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbMachDrvNum = new System.Windows.Forms.ComboBox();
            this.grpCommSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParamViewFull)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.ctxtSchedChng.SuspendLayout();
            this.ctxtDriveMod.SuspendLayout();
            this.grpSetDrive.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParamViewMisMatch)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParamViewChng)).BeginInit();
            this.grpParamChng.SuspendLayout();
            this.grpSetMotor.SuspendLayout();
            this.grpSetMach.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSlaveAddr
            // 
            this.txtSlaveAddr.Enabled = false;
            this.txtSlaveAddr.Location = new System.Drawing.Point(558, 21);
            this.txtSlaveAddr.Name = "txtSlaveAddr";
            this.txtSlaveAddr.Size = new System.Drawing.Size(46, 20);
            this.txtSlaveAddr.TabIndex = 98;
            this.txtSlaveAddr.TabStop = false;
            this.txtSlaveAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSlaveAddr.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSlaveAddr_KeyDown);
            // 
            // lblSlaveAddr
            // 
            this.lblSlaveAddr.AutoSize = true;
            this.lblSlaveAddr.Location = new System.Drawing.Point(474, 24);
            this.lblSlaveAddr.Name = "lblSlaveAddr";
            this.lblSlaveAddr.Size = new System.Drawing.Size(78, 13);
            this.lblSlaveAddr.TabIndex = 14;
            this.lblSlaveAddr.Text = "Slave Address:";
            this.lblSlaveAddr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grpCommSettings
            // 
            this.grpCommSettings.Controls.Add(this.lblSlaveAddr);
            this.grpCommSettings.Controls.Add(this.txtSlaveAddr);
            this.grpCommSettings.Controls.Add(this.cmbSerialPort);
            this.grpCommSettings.Controls.Add(this.lblSerialCommPort);
            this.grpCommSettings.Location = new System.Drawing.Point(7, 27);
            this.grpCommSettings.Name = "grpCommSettings";
            this.grpCommSettings.Size = new System.Drawing.Size(610, 53);
            this.grpCommSettings.TabIndex = 27;
            this.grpCommSettings.TabStop = false;
            this.grpCommSettings.Text = "Serial Communication Settings";
            // 
            // cmbSerialPort
            // 
            this.cmbSerialPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSerialPort.FormattingEnabled = true;
            this.cmbSerialPort.Location = new System.Drawing.Point(145, 21);
            this.cmbSerialPort.Name = "cmbSerialPort";
            this.cmbSerialPort.Size = new System.Drawing.Size(85, 21);
            this.cmbSerialPort.TabIndex = 98;
            this.cmbSerialPort.TabStop = false;
            this.cmbSerialPort.SelectedIndexChanged += new System.EventHandler(this.cmbSerialPort_SelectedIndexChanged);
            // 
            // lblSerialCommPort
            // 
            this.lblSerialCommPort.AutoSize = true;
            this.lblSerialCommPort.Location = new System.Drawing.Point(6, 24);
            this.lblSerialCommPort.Name = "lblSerialCommPort";
            this.lblSerialCommPort.Size = new System.Drawing.Size(133, 13);
            this.lblSerialCommPort.TabIndex = 0;
            this.lblSerialCommPort.Text = "Serial Communication Port:";
            // 
            // spVFD
            // 
            this.spVFD.PortName = "COM99";
            this.spVFD.ReadBufferSize = 256;
            this.spVFD.ReceivedBytesThreshold = 7;
            this.spVFD.WriteBufferSize = 256;
            // 
            // dgvParamViewFull
            // 
            this.dgvParamViewFull.AllowUserToAddRows = false;
            this.dgvParamViewFull.AllowUserToDeleteRows = false;
            this.dgvParamViewFull.AllowUserToResizeColumns = false;
            this.dgvParamViewFull.AllowUserToResizeRows = false;
            this.dgvParamViewFull.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParamViewFull.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cmRegAddr,
            this.cmParamNum,
            this.cmParmName,
            this.cmDefVal,
            this.cmVFDVal});
            this.dgvParamViewFull.Location = new System.Drawing.Point(5, 128);
            this.dgvParamViewFull.Name = "dgvParamViewFull";
            this.dgvParamViewFull.RowHeadersVisible = false;
            this.dgvParamViewFull.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvParamViewFull.Size = new System.Drawing.Size(600, 322);
            this.dgvParamViewFull.TabIndex = 36;
            this.dgvParamViewFull.TabStop = false;
            this.dgvParamViewFull.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvParamViewFull_CellBeginEdit);
            this.dgvParamViewFull.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvParamViewFull_CellEndEdit);
            // 
            // cmRegAddr
            // 
            this.cmRegAddr.DataPropertyName = "RegAddress";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.cmRegAddr.DefaultCellStyle = dataGridViewCellStyle1;
            this.cmRegAddr.HeaderText = "Parameter Address";
            this.cmRegAddr.Name = "cmRegAddr";
            this.cmRegAddr.ReadOnly = true;
            this.cmRegAddr.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cmRegAddr.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cmRegAddr.Width = 60;
            // 
            // cmParamNum
            // 
            this.cmParamNum.DataPropertyName = "ParamNum";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.cmParamNum.DefaultCellStyle = dataGridViewCellStyle2;
            this.cmParamNum.HeaderText = "Parameter Number";
            this.cmParamNum.Name = "cmParamNum";
            this.cmParamNum.ReadOnly = true;
            this.cmParamNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cmParamNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cmParamNum.Width = 60;
            // 
            // cmParmName
            // 
            this.cmParmName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cmParmName.DataPropertyName = "ParamName";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.cmParmName.DefaultCellStyle = dataGridViewCellStyle3;
            this.cmParmName.HeaderText = "Parameter Name";
            this.cmParmName.Name = "cmParmName";
            this.cmParmName.ReadOnly = true;
            this.cmParmName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cmDefVal
            // 
            this.cmDefVal.DataPropertyName = "DefVal";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.cmDefVal.DefaultCellStyle = dataGridViewCellStyle4;
            this.cmDefVal.HeaderText = "Default Value";
            this.cmDefVal.Name = "cmDefVal";
            this.cmDefVal.ReadOnly = true;
            this.cmDefVal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cmDefVal.Width = 70;
            // 
            // cmVFDVal
            // 
            this.cmVFDVal.DataPropertyName = "VFDVal";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.cmVFDVal.DefaultCellStyle = dataGridViewCellStyle5;
            this.cmVFDVal.HeaderText = "VFD Value";
            this.cmVFDVal.Name = "cmVFDVal";
            this.cmVFDVal.ReadOnly = true;
            this.cmVFDVal.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cmVFDVal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cmVFDVal.Width = 70;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statProgLabel,
            this.statProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 783);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1266, 22);
            this.statusStrip1.TabIndex = 37;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statProgLabel
            // 
            this.statProgLabel.Name = "statProgLabel";
            this.statProgLabel.Size = new System.Drawing.Size(162, 17);
            this.statProgLabel.Text = "Parameter List Load Progress:";
            this.statProgLabel.Visible = false;
            // 
            // statProgress
            // 
            this.statProgress.Name = "statProgress";
            this.statProgress.Size = new System.Drawing.Size(900, 16);
            this.statProgress.Visible = false;
            // 
            // btnVFDRead
            // 
            this.btnVFDRead.Enabled = false;
            this.btnVFDRead.Location = new System.Drawing.Point(5, 18);
            this.btnVFDRead.Name = "btnVFDRead";
            this.btnVFDRead.Size = new System.Drawing.Size(130, 23);
            this.btnVFDRead.TabIndex = 2;
            this.btnVFDRead.Text = "Read VFD Settings";
            this.btnVFDRead.UseVisualStyleBackColor = true;
            this.btnVFDRead.Click += new System.EventHandler(this.btnReadVFD_Click);
            // 
            // bwrkReadVFDVals
            // 
            this.bwrkReadVFDVals.WorkerReportsProgress = true;
            this.bwrkReadVFDVals.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwrkReadVFDVals_DoWork);
            this.bwrkReadVFDVals.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwrkDGV_ProgressChanged);
            this.bwrkReadVFDVals.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwrkReadVFDVals_RunWorkerCompleted);
            // 
            // ctxtSchedChng
            // 
            this.ctxtSchedChng.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxtSchedChng_Load,
            this.ctxtSchedChng_Save,
            this.ctxtSchedChng_Remove,
            this.ctxtSchedChng_Clear});
            this.ctxtSchedChng.Name = "ctxtSchedChng";
            this.ctxtSchedChng.Size = new System.Drawing.Size(219, 92);
            this.ctxtSchedChng.Opening += new System.ComponentModel.CancelEventHandler(this.ctxtSchedChng_Opening);
            // 
            // ctxtSchedChng_Load
            // 
            this.ctxtSchedChng_Load.Name = "ctxtSchedChng_Load";
            this.ctxtSchedChng_Load.Size = new System.Drawing.Size(218, 22);
            this.ctxtSchedChng_Load.Text = "Load Change List from File";
            this.ctxtSchedChng_Load.Click += new System.EventHandler(this.LoadParams);
            // 
            // ctxtSchedChng_Save
            // 
            this.ctxtSchedChng_Save.Name = "ctxtSchedChng_Save";
            this.ctxtSchedChng_Save.Size = new System.Drawing.Size(218, 22);
            this.ctxtSchedChng_Save.Text = "Save Change List to File";
            this.ctxtSchedChng_Save.Click += new System.EventHandler(this.SaveParams);
            // 
            // ctxtSchedChng_Remove
            // 
            this.ctxtSchedChng_Remove.Name = "ctxtSchedChng_Remove";
            this.ctxtSchedChng_Remove.Size = new System.Drawing.Size(218, 22);
            this.ctxtSchedChng_Remove.Text = "Remove Parameter Change";
            this.ctxtSchedChng_Remove.Click += new System.EventHandler(this.ctxtSchedChng_Remove_Click);
            // 
            // ctxtSchedChng_Clear
            // 
            this.ctxtSchedChng_Clear.Name = "ctxtSchedChng_Clear";
            this.ctxtSchedChng_Clear.Size = new System.Drawing.Size(218, 22);
            this.ctxtSchedChng_Clear.Text = "Clear Change List";
            this.ctxtSchedChng_Clear.Click += new System.EventHandler(this.clearScheduledChangesToolStripMenuItem_Click);
            // 
            // ctxtDriveMod
            // 
            this.ctxtDriveMod.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxtDriveMod_Save,
            this.ctxtDriveMod_Clear});
            this.ctxtDriveMod.Name = "ctxtDriveMod";
            this.ctxtDriveMod.Size = new System.Drawing.Size(247, 48);
            this.ctxtDriveMod.Opening += new System.ComponentModel.CancelEventHandler(this.ctxtDriveMod_Opening);
            // 
            // ctxtDriveMod_Save
            // 
            this.ctxtDriveMod_Save.Name = "ctxtDriveMod_Save";
            this.ctxtDriveMod_Save.Size = new System.Drawing.Size(246, 22);
            this.ctxtDriveMod_Save.Text = "Save Modified Parameters to File";
            this.ctxtDriveMod_Save.Click += new System.EventHandler(this.SaveParams);
            // 
            // ctxtDriveMod_Clear
            // 
            this.ctxtDriveMod_Clear.Name = "ctxtDriveMod_Clear";
            this.ctxtDriveMod_Clear.Size = new System.Drawing.Size(246, 22);
            this.ctxtDriveMod_Clear.Text = "Clear List";
            this.ctxtDriveMod_Clear.Click += new System.EventHandler(this.clearListToolStripMenuItem_Click);
            // 
            // btnVFDMod
            // 
            this.btnVFDMod.Enabled = false;
            this.btnVFDMod.Location = new System.Drawing.Point(318, 19);
            this.btnVFDMod.Name = "btnVFDMod";
            this.btnVFDMod.Size = new System.Drawing.Size(130, 23);
            this.btnVFDMod.TabIndex = 4;
            this.btnVFDMod.Text = "Modify VFD Parameters";
            this.btnVFDMod.UseVisualStyleBackColor = true;
            this.btnVFDMod.Click += new System.EventHandler(this.btnModVFD_Click);
            // 
            // bwrkModVFD
            // 
            this.bwrkModVFD.WorkerReportsProgress = true;
            this.bwrkModVFD.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwrkModVFD_DoWork);
            this.bwrkModVFD.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwrkDGV_ProgressChanged);
            this.bwrkModVFD.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwrkModVFD_RunWorkerCompleted);
            // 
            // grpSetDrive
            // 
            this.grpSetDrive.Controls.Add(this.lblDriveDuty);
            this.grpSetDrive.Controls.Add(this.cmbDriveDuty);
            this.grpSetDrive.Controls.Add(this.lblParamFullList);
            this.grpSetDrive.Controls.Add(this.lblParamGroup);
            this.grpSetDrive.Controls.Add(this.lblDriveSel);
            this.grpSetDrive.Controls.Add(this.cmbDriveList);
            this.grpSetDrive.Controls.Add(this.cmbParamGroup);
            this.grpSetDrive.Controls.Add(this.dgvParamViewFull);
            this.grpSetDrive.Controls.Add(this.dgvParamViewMisMatch);
            this.grpSetDrive.Controls.Add(this.lblParamMismatch);
            this.grpSetDrive.Location = new System.Drawing.Point(7, 86);
            this.grpSetDrive.Name = "grpSetDrive";
            this.grpSetDrive.Size = new System.Drawing.Size(610, 695);
            this.grpSetDrive.TabIndex = 45;
            this.grpSetDrive.TabStop = false;
            this.grpSetDrive.Text = "VFD Complete Parameter Information";
            // 
            // lblDriveDuty
            // 
            this.lblDriveDuty.AutoSize = true;
            this.lblDriveDuty.Location = new System.Drawing.Point(410, 45);
            this.lblDriveDuty.Name = "lblDriveDuty";
            this.lblDriveDuty.Size = new System.Drawing.Size(82, 13);
            this.lblDriveDuty.TabIndex = 49;
            this.lblDriveDuty.Text = "Drive Selection:";
            // 
            // cmbDriveDuty
            // 
            this.cmbDriveDuty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDriveDuty.Enabled = false;
            this.cmbDriveDuty.FormattingEnabled = true;
            this.cmbDriveDuty.Items.AddRange(new object[] {
            "Normal Duty",
            "Heavy Duty"});
            this.cmbDriveDuty.Location = new System.Drawing.Point(498, 42);
            this.cmbDriveDuty.Name = "cmbDriveDuty";
            this.cmbDriveDuty.Size = new System.Drawing.Size(106, 21);
            this.cmbDriveDuty.TabIndex = 48;
            this.cmbDriveDuty.SelectedIndexChanged += new System.EventHandler(this.cmbDriveDuty_SelectedIndexChanged);
            // 
            // lblParamFullList
            // 
            this.lblParamFullList.AutoSize = true;
            this.lblParamFullList.Location = new System.Drawing.Point(2, 112);
            this.lblParamFullList.Name = "lblParamFullList";
            this.lblParamFullList.Size = new System.Drawing.Size(96, 13);
            this.lblParamFullList.TabIndex = 47;
            this.lblParamFullList.Text = "Full Parameter List:";
            // 
            // lblParamGroup
            // 
            this.lblParamGroup.AutoSize = true;
            this.lblParamGroup.Location = new System.Drawing.Point(6, 72);
            this.lblParamGroup.Name = "lblParamGroup";
            this.lblParamGroup.Size = new System.Drawing.Size(90, 13);
            this.lblParamGroup.TabIndex = 41;
            this.lblParamGroup.Text = "Parameter Group:";
            // 
            // lblDriveSel
            // 
            this.lblDriveSel.AutoSize = true;
            this.lblDriveSel.Location = new System.Drawing.Point(14, 45);
            this.lblDriveSel.Name = "lblDriveSel";
            this.lblDriveSel.Size = new System.Drawing.Size(82, 13);
            this.lblDriveSel.TabIndex = 40;
            this.lblDriveSel.Text = "Drive Selection:";
            // 
            // cmbDriveList
            // 
            this.cmbDriveList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDriveList.FormattingEnabled = true;
            this.cmbDriveList.Location = new System.Drawing.Point(102, 42);
            this.cmbDriveList.Name = "cmbDriveList";
            this.cmbDriveList.Size = new System.Drawing.Size(277, 21);
            this.cmbDriveList.TabIndex = 0;
            this.cmbDriveList.SelectedIndexChanged += new System.EventHandler(this.cmbDriveSel_SelectedIndexChanged);
            // 
            // cmbParamGroup
            // 
            this.cmbParamGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParamGroup.Enabled = false;
            this.cmbParamGroup.FormattingEnabled = true;
            this.cmbParamGroup.Location = new System.Drawing.Point(102, 69);
            this.cmbParamGroup.Name = "cmbParamGroup";
            this.cmbParamGroup.Size = new System.Drawing.Size(277, 21);
            this.cmbParamGroup.TabIndex = 1;
            this.cmbParamGroup.SelectedIndexChanged += new System.EventHandler(this.cmbParamGroup_SelectedIndexChanged);
            // 
            // dgvParamViewMisMatch
            // 
            this.dgvParamViewMisMatch.AllowUserToAddRows = false;
            this.dgvParamViewMisMatch.AllowUserToDeleteRows = false;
            this.dgvParamViewMisMatch.AllowUserToResizeColumns = false;
            this.dgvParamViewMisMatch.AllowUserToResizeRows = false;
            this.dgvParamViewMisMatch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParamViewMisMatch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cmMisMatchParamAddr,
            this.cmMisMatchParamNum,
            this.cmMisMatchParamName,
            this.cmMisMatchDefVal,
            this.cmMisMatchReadVal});
            this.dgvParamViewMisMatch.ContextMenuStrip = this.ctxtDriveMod;
            this.dgvParamViewMisMatch.Location = new System.Drawing.Point(6, 479);
            this.dgvParamViewMisMatch.Name = "dgvParamViewMisMatch";
            this.dgvParamViewMisMatch.RowHeadersVisible = false;
            this.dgvParamViewMisMatch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvParamViewMisMatch.Size = new System.Drawing.Size(600, 206);
            this.dgvParamViewMisMatch.TabIndex = 41;
            this.dgvParamViewMisMatch.TabStop = false;
            // 
            // cmMisMatchParamAddr
            // 
            this.cmMisMatchParamAddr.DataPropertyName = "RegAddress";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.cmMisMatchParamAddr.DefaultCellStyle = dataGridViewCellStyle6;
            this.cmMisMatchParamAddr.HeaderText = "Parameter Address";
            this.cmMisMatchParamAddr.Name = "cmMisMatchParamAddr";
            this.cmMisMatchParamAddr.ReadOnly = true;
            this.cmMisMatchParamAddr.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cmMisMatchParamAddr.Width = 60;
            // 
            // cmMisMatchParamNum
            // 
            this.cmMisMatchParamNum.DataPropertyName = "ParamNum";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.cmMisMatchParamNum.DefaultCellStyle = dataGridViewCellStyle7;
            this.cmMisMatchParamNum.HeaderText = "Parameter Number";
            this.cmMisMatchParamNum.Name = "cmMisMatchParamNum";
            this.cmMisMatchParamNum.ReadOnly = true;
            this.cmMisMatchParamNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cmMisMatchParamNum.Width = 60;
            // 
            // cmMisMatchParamName
            // 
            this.cmMisMatchParamName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cmMisMatchParamName.DataPropertyName = "ParamName";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.cmMisMatchParamName.DefaultCellStyle = dataGridViewCellStyle8;
            this.cmMisMatchParamName.HeaderText = "Parameter Name";
            this.cmMisMatchParamName.Name = "cmMisMatchParamName";
            this.cmMisMatchParamName.ReadOnly = true;
            // 
            // cmMisMatchDefVal
            // 
            this.cmMisMatchDefVal.DataPropertyName = "DefVal";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.cmMisMatchDefVal.DefaultCellStyle = dataGridViewCellStyle9;
            this.cmMisMatchDefVal.HeaderText = "Default Value";
            this.cmMisMatchDefVal.Name = "cmMisMatchDefVal";
            this.cmMisMatchDefVal.ReadOnly = true;
            this.cmMisMatchDefVal.Width = 70;
            // 
            // cmMisMatchReadVal
            // 
            this.cmMisMatchReadVal.DataPropertyName = "ReadVal";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.cmMisMatchReadVal.DefaultCellStyle = dataGridViewCellStyle10;
            this.cmMisMatchReadVal.HeaderText = "Read Value";
            this.cmMisMatchReadVal.Name = "cmMisMatchReadVal";
            this.cmMisMatchReadVal.ReadOnly = true;
            this.cmMisMatchReadVal.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cmMisMatchReadVal.Width = 70;
            // 
            // lblParamMismatch
            // 
            this.lblParamMismatch.AutoSize = true;
            this.lblParamMismatch.Location = new System.Drawing.Point(7, 463);
            this.lblParamMismatch.Name = "lblParamMismatch";
            this.lblParamMismatch.Size = new System.Drawing.Size(134, 13);
            this.lblParamMismatch.TabIndex = 46;
            this.lblParamMismatch.Text = "Drive Modified Parameters:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnVFDVer);
            this.groupBox4.Controls.Add(this.btnVFDReset);
            this.groupBox4.Controls.Add(this.btnVFDRead);
            this.groupBox4.Controls.Add(this.btnVFDMod);
            this.groupBox4.Location = new System.Drawing.Point(638, 27);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(614, 53);
            this.groupBox4.TabIndex = 47;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Drive Communication Commands";
            // 
            // btnVFDVer
            // 
            this.btnVFDVer.Enabled = false;
            this.btnVFDVer.Location = new System.Drawing.Point(478, 19);
            this.btnVFDVer.Name = "btnVFDVer";
            this.btnVFDVer.Size = new System.Drawing.Size(130, 23);
            this.btnVFDVer.TabIndex = 99;
            this.btnVFDVer.Text = "Verify VFD Programming";
            this.btnVFDVer.UseVisualStyleBackColor = true;
            this.btnVFDVer.Click += new System.EventHandler(this.btnVFDVer_Click);
            // 
            // btnVFDReset
            // 
            this.btnVFDReset.Enabled = false;
            this.btnVFDReset.Location = new System.Drawing.Point(161, 18);
            this.btnVFDReset.Name = "btnVFDReset";
            this.btnVFDReset.Size = new System.Drawing.Size(130, 23);
            this.btnVFDReset.TabIndex = 3;
            this.btnVFDReset.Text = "Reintialize VFD";
            this.btnVFDReset.UseVisualStyleBackColor = true;
            this.btnVFDReset.Click += new System.EventHandler(this.btnVFDReset_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msFile,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1266, 24);
            this.menuStrip1.TabIndex = 48;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // msFile
            // 
            this.msFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msFile_LoadParamList,
            this.toolStripSeparator1,
            this.msFile_Exit});
            this.msFile.Name = "msFile";
            this.msFile.Size = new System.Drawing.Size(37, 20);
            this.msFile.Text = "&File";
            // 
            // msFile_LoadParamList
            // 
            this.msFile_LoadParamList.Enabled = false;
            this.msFile_LoadParamList.Name = "msFile_LoadParamList";
            this.msFile_LoadParamList.Size = new System.Drawing.Size(195, 22);
            this.msFile_LoadParamList.Text = "&Load Parameter Listing";
            this.msFile_LoadParamList.Click += new System.EventHandler(this.LoadParams);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(192, 6);
            // 
            // msFile_Exit
            // 
            this.msFile_Exit.Name = "msFile_Exit";
            this.msFile_Exit.Size = new System.Drawing.Size(195, 22);
            this.msFile_Exit.Text = "E&xit";
            this.msFile_Exit.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // lblParamModSched
            // 
            this.lblParamModSched.AutoSize = true;
            this.lblParamModSched.Location = new System.Drawing.Point(6, 28);
            this.lblParamModSched.Name = "lblParamModSched";
            this.lblParamModSched.Size = new System.Drawing.Size(106, 13);
            this.lblParamModSched.TabIndex = 43;
            this.lblParamModSched.Text = "Scheduled Changes:";
            // 
            // dgvParamViewChng
            // 
            this.dgvParamViewChng.AllowUserToAddRows = false;
            this.dgvParamViewChng.AllowUserToDeleteRows = false;
            this.dgvParamViewChng.AllowUserToResizeColumns = false;
            this.dgvParamViewChng.AllowUserToResizeRows = false;
            this.dgvParamViewChng.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParamViewChng.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5});
            this.dgvParamViewChng.ContextMenuStrip = this.ctxtSchedChng;
            this.dgvParamViewChng.Location = new System.Drawing.Point(5, 51);
            this.dgvParamViewChng.Name = "dgvParamViewChng";
            this.dgvParamViewChng.RowHeadersVisible = false;
            this.dgvParamViewChng.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvParamViewChng.Size = new System.Drawing.Size(600, 384);
            this.dgvParamViewChng.TabIndex = 40;
            this.dgvParamViewChng.TabStop = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "RegAddress";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridViewTextBoxColumn1.HeaderText = "Parameter Address";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 60;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "ParamNum";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridViewTextBoxColumn2.HeaderText = "Parameter Number";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 60;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "ParamName";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle13;
            this.dataGridViewTextBoxColumn3.HeaderText = "Parameter Name";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "DefVal";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridViewTextBoxColumn4.HeaderText = "Default Value";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn4.Width = 70;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "SpecVal";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle15;
            this.dataGridViewTextBoxColumn5.HeaderText = "Specified Value";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn5.Width = 70;
            // 
            // lblParamChngSrc
            // 
            this.lblParamChngSrc.AutoSize = true;
            this.lblParamChngSrc.Location = new System.Drawing.Point(461, 28);
            this.lblParamChngSrc.Name = "lblParamChngSrc";
            this.lblParamChngSrc.Size = new System.Drawing.Size(44, 13);
            this.lblParamChngSrc.TabIndex = 44;
            this.lblParamChngSrc.Text = "Source:";
            // 
            // txtParamChngSrc
            // 
            this.txtParamChngSrc.Location = new System.Drawing.Point(511, 25);
            this.txtParamChngSrc.Name = "txtParamChngSrc";
            this.txtParamChngSrc.ReadOnly = true;
            this.txtParamChngSrc.Size = new System.Drawing.Size(97, 20);
            this.txtParamChngSrc.TabIndex = 45;
            // 
            // lblVoltMachSupply
            // 
            this.lblVoltMachSupply.AutoSize = true;
            this.lblVoltMachSupply.Location = new System.Drawing.Point(24, 22);
            this.lblVoltMachSupply.Name = "lblVoltMachSupply";
            this.lblVoltMachSupply.Size = new System.Drawing.Size(81, 13);
            this.lblVoltMachSupply.TabIndex = 47;
            this.lblVoltMachSupply.Text = "Supply Voltage:";
            // 
            // cmbMtrVoltSupply
            // 
            this.cmbMtrVoltSupply.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMtrVoltSupply.FormattingEnabled = true;
            this.cmbMtrVoltSupply.Items.AddRange(new object[] {
            "208 V",
            "220 V",
            "230 V",
            "240 V",
            "380 V",
            "400 V",
            "415 V",
            "460 V"});
            this.cmbMtrVoltSupply.Location = new System.Drawing.Point(111, 19);
            this.cmbMtrVoltSupply.Name = "cmbMtrVoltSupply";
            this.cmbMtrVoltSupply.Size = new System.Drawing.Size(68, 21);
            this.cmbMtrVoltSupply.TabIndex = 50;
            this.cmbMtrVoltSupply.SelectedIndexChanged += new System.EventHandler(this.cmbVoltMach_SelectedIndexChanged);
            // 
            // lblVoltMotorMax
            // 
            this.lblVoltMotorMax.AutoSize = true;
            this.lblVoltMotorMax.Location = new System.Drawing.Point(225, 22);
            this.lblVoltMotorMax.Name = "lblVoltMotorMax";
            this.lblVoltMotorMax.Size = new System.Drawing.Size(99, 13);
            this.lblVoltMotorMax.TabIndex = 51;
            this.lblVoltMotorMax.Text = "Max Motor Voltage:";
            // 
            // cmbMtrVoltMax
            // 
            this.cmbMtrVoltMax.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMtrVoltMax.FormattingEnabled = true;
            this.cmbMtrVoltMax.Items.AddRange(new object[] {
            "208 V",
            "220 V",
            "230 V",
            "240 V",
            "380 V",
            "400 V",
            "415 V",
            "460 V"});
            this.cmbMtrVoltMax.Location = new System.Drawing.Point(330, 19);
            this.cmbMtrVoltMax.Name = "cmbMtrVoltMax";
            this.cmbMtrVoltMax.Size = new System.Drawing.Size(68, 21);
            this.cmbMtrVoltMax.TabIndex = 52;
            this.cmbMtrVoltMax.SelectedIndexChanged += new System.EventHandler(this.cmbVoltMotorMax_SelectedIndexChanged);
            // 
            // lblFreqMotorBase
            // 
            this.lblFreqMotorBase.AutoSize = true;
            this.lblFreqMotorBase.Location = new System.Drawing.Point(207, 49);
            this.lblFreqMotorBase.Name = "lblFreqMotorBase";
            this.lblFreqMotorBase.Size = new System.Drawing.Size(117, 13);
            this.lblFreqMotorBase.TabIndex = 55;
            this.lblFreqMotorBase.Text = "Motor Base Frequency:";
            // 
            // cmbMtrFreqBase
            // 
            this.cmbMtrFreqBase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMtrFreqBase.FormattingEnabled = true;
            this.cmbMtrFreqBase.Items.AddRange(new object[] {
            "50 Hz",
            "60 Hz"});
            this.cmbMtrFreqBase.Location = new System.Drawing.Point(330, 46);
            this.cmbMtrFreqBase.Name = "cmbMtrFreqBase";
            this.cmbMtrFreqBase.Size = new System.Drawing.Size(68, 21);
            this.cmbMtrFreqBase.TabIndex = 56;
            this.cmbMtrFreqBase.SelectedIndexChanged += new System.EventHandler(this.cmbFreqMotorBase_SelectedIndexChanged);
            // 
            // lblMotorFLC
            // 
            this.lblMotorFLC.AutoSize = true;
            this.lblMotorFLC.Location = new System.Drawing.Point(475, 49);
            this.lblMotorFLC.Name = "lblMotorFLC";
            this.lblMotorFLC.Size = new System.Drawing.Size(59, 13);
            this.lblMotorFLC.TabIndex = 57;
            this.lblMotorFLC.Text = "Motor FLC:";
            // 
            // lblUnitsAmps1
            // 
            this.lblUnitsAmps1.AutoSize = true;
            this.lblUnitsAmps1.Location = new System.Drawing.Point(594, 50);
            this.lblUnitsAmps1.Name = "lblUnitsAmps1";
            this.lblUnitsAmps1.Size = new System.Drawing.Size(14, 13);
            this.lblUnitsAmps1.TabIndex = 58;
            this.lblUnitsAmps1.Text = "A";
            // 
            // txtMtrFLC
            // 
            this.txtMtrFLC.Location = new System.Drawing.Point(540, 46);
            this.txtMtrFLC.Name = "txtMtrFLC";
            this.txtMtrFLC.Size = new System.Drawing.Size(48, 20);
            this.txtMtrFLC.TabIndex = 59;
            // 
            // btnMtrSet
            // 
            this.btnMtrSet.Location = new System.Drawing.Point(478, 73);
            this.btnMtrSet.Name = "btnMtrSet";
            this.btnMtrSet.Size = new System.Drawing.Size(130, 23);
            this.btnMtrSet.TabIndex = 60;
            this.btnMtrSet.Text = "Set Motor Values";
            this.btnMtrSet.UseVisualStyleBackColor = true;
            this.btnMtrSet.Click += new System.EventHandler(this.btnSetMotorVals);
            // 
            // grpParamChng
            // 
            this.grpParamChng.Controls.Add(this.txtParamChngSrc);
            this.grpParamChng.Controls.Add(this.lblParamChngSrc);
            this.grpParamChng.Controls.Add(this.dgvParamViewChng);
            this.grpParamChng.Controls.Add(this.lblParamModSched);
            this.grpParamChng.Location = new System.Drawing.Point(638, 196);
            this.grpParamChng.Name = "grpParamChng";
            this.grpParamChng.Size = new System.Drawing.Size(614, 441);
            this.grpParamChng.TabIndex = 46;
            this.grpParamChng.TabStop = false;
            this.grpParamChng.Text = "VFD Parameter Changes";
            // 
            // cmbMtrFreqSupply
            // 
            this.cmbMtrFreqSupply.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMtrFreqSupply.FormattingEnabled = true;
            this.cmbMtrFreqSupply.Items.AddRange(new object[] {
            "50 Hz",
            "60 Hz"});
            this.cmbMtrFreqSupply.Location = new System.Drawing.Point(111, 46);
            this.cmbMtrFreqSupply.Name = "cmbMtrFreqSupply";
            this.cmbMtrFreqSupply.Size = new System.Drawing.Size(68, 21);
            this.cmbMtrFreqSupply.TabIndex = 68;
            this.cmbMtrFreqSupply.SelectedIndexChanged += new System.EventHandler(this.cmbFreqMach_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 67;
            this.label1.Text = "Supply Frequency:";
            // 
            // cmbMtrPartNum
            // 
            this.cmbMtrPartNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMtrPartNum.FormattingEnabled = true;
            this.cmbMtrPartNum.Location = new System.Drawing.Point(540, 19);
            this.cmbMtrPartNum.Name = "cmbMtrPartNum";
            this.cmbMtrPartNum.Size = new System.Drawing.Size(68, 21);
            this.cmbMtrPartNum.TabIndex = 66;
            this.cmbMtrPartNum.SelectedIndexChanged += new System.EventHandler(this.cmbMotorPartNum_SelectedIndexChanged);
            // 
            // lblMotorPartNum
            // 
            this.lblMotorPartNum.AutoSize = true;
            this.lblMotorPartNum.Location = new System.Drawing.Point(435, 22);
            this.lblMotorPartNum.Name = "lblMotorPartNum";
            this.lblMotorPartNum.Size = new System.Drawing.Size(99, 13);
            this.lblMotorPartNum.TabIndex = 65;
            this.lblMotorPartNum.Text = "Motor Part Number:";
            // 
            // cmbMachSel
            // 
            this.cmbMachSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMachSel.FormattingEnabled = true;
            this.cmbMachSel.Location = new System.Drawing.Point(110, 25);
            this.cmbMachSel.Name = "cmbMachSel";
            this.cmbMachSel.Size = new System.Drawing.Size(234, 21);
            this.cmbMachSel.TabIndex = 63;
            this.cmbMachSel.SelectedIndexChanged += new System.EventHandler(this.cmbSelMach_SelectedIndexChanged);
            // 
            // lblSelMach
            // 
            this.lblSelMach.AutoSize = true;
            this.lblSelMach.Location = new System.Drawing.Point(6, 28);
            this.lblSelMach.Name = "lblSelMach";
            this.lblSelMach.Size = new System.Drawing.Size(98, 13);
            this.lblSelMach.TabIndex = 62;
            this.lblSelMach.Text = "Machine Selection:";
            // 
            // lblSelMotor
            // 
            this.lblSelMotor.AutoSize = true;
            this.lblSelMotor.Location = new System.Drawing.Point(225, 69);
            this.lblSelMotor.Name = "lblSelMotor";
            this.lblSelMotor.Size = new System.Drawing.Size(75, 13);
            this.lblSelMotor.TabIndex = 61;
            this.lblSelMotor.Text = "Drive Number:";
            // 
            // bwrkVFDVerify
            // 
            this.bwrkVFDVerify.WorkerReportsProgress = true;
            this.bwrkVFDVerify.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwrkVFDVerify_DoWork);
            this.bwrkVFDVerify.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwrkDGV_ProgressChanged);
            this.bwrkVFDVerify.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwrkVFDVerify_RunWorkerCompleted);
            // 
            // grpSetMotor
            // 
            this.grpSetMotor.Controls.Add(this.btnMtrDel);
            this.grpSetMotor.Controls.Add(this.btnMtrStore);
            this.grpSetMotor.Controls.Add(this.btnMtrSet);
            this.grpSetMotor.Controls.Add(this.cmbMtrFreqSupply);
            this.grpSetMotor.Controls.Add(this.txtMtrFLC);
            this.grpSetMotor.Controls.Add(this.lblUnitsAmps1);
            this.grpSetMotor.Controls.Add(this.lblMotorFLC);
            this.grpSetMotor.Controls.Add(this.label1);
            this.grpSetMotor.Controls.Add(this.lblVoltMachSupply);
            this.grpSetMotor.Controls.Add(this.cmbMtrPartNum);
            this.grpSetMotor.Controls.Add(this.cmbMtrVoltSupply);
            this.grpSetMotor.Controls.Add(this.lblMotorPartNum);
            this.grpSetMotor.Controls.Add(this.lblVoltMotorMax);
            this.grpSetMotor.Controls.Add(this.cmbMtrVoltMax);
            this.grpSetMotor.Controls.Add(this.lblFreqMotorBase);
            this.grpSetMotor.Controls.Add(this.cmbMtrFreqBase);
            this.grpSetMotor.Enabled = false;
            this.grpSetMotor.Location = new System.Drawing.Point(638, 86);
            this.grpSetMotor.Name = "grpSetMotor";
            this.grpSetMotor.Size = new System.Drawing.Size(614, 104);
            this.grpSetMotor.TabIndex = 49;
            this.grpSetMotor.TabStop = false;
            this.grpSetMotor.Text = "Motor Settings";
            // 
            // btnMtrDel
            // 
            this.btnMtrDel.Location = new System.Drawing.Point(49, 75);
            this.btnMtrDel.Name = "btnMtrDel";
            this.btnMtrDel.Size = new System.Drawing.Size(130, 23);
            this.btnMtrDel.TabIndex = 69;
            this.btnMtrDel.Text = "Delete Motor Values";
            this.btnMtrDel.UseVisualStyleBackColor = true;
            this.btnMtrDel.Click += new System.EventHandler(this.btnMtrDel_Click);
            // 
            // btnMtrStore
            // 
            this.btnMtrStore.Location = new System.Drawing.Point(268, 73);
            this.btnMtrStore.Name = "btnMtrStore";
            this.btnMtrStore.Size = new System.Drawing.Size(130, 23);
            this.btnMtrStore.TabIndex = 60;
            this.btnMtrStore.Text = "Store Motor Values";
            this.btnMtrStore.UseVisualStyleBackColor = true;
            this.btnMtrStore.Click += new System.EventHandler(this.btnMtrStore_Click);
            // 
            // grpSetMach
            // 
            this.grpSetMach.Controls.Add(this.txtMachDrvName);
            this.grpSetMach.Controls.Add(this.label5);
            this.grpSetMach.Controls.Add(this.txtMachChrtCnt);
            this.grpSetMach.Controls.Add(this.label3);
            this.grpSetMach.Controls.Add(this.txtMachDrvCnt);
            this.grpSetMach.Controls.Add(this.btnMachListDel);
            this.grpSetMach.Controls.Add(this.btnMachListLoad);
            this.grpSetMach.Controls.Add(this.btnMachListStore);
            this.grpSetMach.Controls.Add(this.cmbMachChrtNum);
            this.grpSetMach.Controls.Add(this.label4);
            this.grpSetMach.Controls.Add(this.label2);
            this.grpSetMach.Controls.Add(this.cmbMachSel);
            this.grpSetMach.Controls.Add(this.lblSelMach);
            this.grpSetMach.Controls.Add(this.cmbMachDrvNum);
            this.grpSetMach.Controls.Add(this.lblSelMotor);
            this.grpSetMach.Enabled = false;
            this.grpSetMach.Location = new System.Drawing.Point(643, 643);
            this.grpSetMach.Name = "grpSetMach";
            this.grpSetMach.Size = new System.Drawing.Size(614, 138);
            this.grpSetMach.TabIndex = 50;
            this.grpSetMach.TabStop = false;
            this.grpSetMach.Text = "Machine Settings";
            // 
            // txtMachDrvName
            // 
            this.txtMachDrvName.Location = new System.Drawing.Point(433, 66);
            this.txtMachDrvName.Name = "txtMachDrvName";
            this.txtMachDrvName.ReadOnly = true;
            this.txtMachDrvName.Size = new System.Drawing.Size(175, 20);
            this.txtMachDrvName.TabIndex = 99;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Chart Count:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMachChrtCnt
            // 
            this.txtMachChrtCnt.Enabled = false;
            this.txtMachChrtCnt.Location = new System.Drawing.Point(81, 66);
            this.txtMachChrtCnt.Name = "txtMachChrtCnt";
            this.txtMachChrtCnt.ReadOnly = true;
            this.txtMachChrtCnt.Size = new System.Drawing.Size(23, 20);
            this.txtMachChrtCnt.TabIndex = 98;
            this.txtMachChrtCnt.TabStop = false;
            this.txtMachChrtCnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMachChrtCnt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSlaveAddr_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(114, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Drive Count:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMachDrvCnt
            // 
            this.txtMachDrvCnt.Enabled = false;
            this.txtMachDrvCnt.Location = new System.Drawing.Point(186, 66);
            this.txtMachDrvCnt.Name = "txtMachDrvCnt";
            this.txtMachDrvCnt.ReadOnly = true;
            this.txtMachDrvCnt.Size = new System.Drawing.Size(23, 20);
            this.txtMachDrvCnt.TabIndex = 98;
            this.txtMachDrvCnt.TabStop = false;
            this.txtMachDrvCnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMachDrvCnt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSlaveAddr_KeyDown);
            // 
            // btnMachListDel
            // 
            this.btnMachListDel.Location = new System.Drawing.Point(221, 105);
            this.btnMachListDel.Name = "btnMachListDel";
            this.btnMachListDel.Size = new System.Drawing.Size(125, 23);
            this.btnMachListDel.TabIndex = 65;
            this.btnMachListDel.Text = "Delete Parameter List";
            this.btnMachListDel.UseVisualStyleBackColor = true;
            this.btnMachListDel.Click += new System.EventHandler(this.btnMachListDel_Click);
            // 
            // btnMachListLoad
            // 
            this.btnMachListLoad.Enabled = false;
            this.btnMachListLoad.Location = new System.Drawing.Point(483, 105);
            this.btnMachListLoad.Name = "btnMachListLoad";
            this.btnMachListLoad.Size = new System.Drawing.Size(125, 23);
            this.btnMachListLoad.TabIndex = 65;
            this.btnMachListLoad.Text = "Load Parameter List";
            this.btnMachListLoad.UseVisualStyleBackColor = true;
            this.btnMachListLoad.Click += new System.EventHandler(this.btnMachLoad_Click);
            // 
            // btnMachListStore
            // 
            this.btnMachListStore.Location = new System.Drawing.Point(352, 105);
            this.btnMachListStore.Name = "btnMachListStore";
            this.btnMachListStore.Size = new System.Drawing.Size(125, 23);
            this.btnMachListStore.TabIndex = 65;
            this.btnMachListStore.Text = "Store Parameter List";
            this.btnMachListStore.UseVisualStyleBackColor = true;
            this.btnMachListStore.Click += new System.EventHandler(this.btnMachStore_Click);
            // 
            // cmbMachChrtNum
            // 
            this.cmbMachChrtNum.FormattingEnabled = true;
            this.cmbMachChrtNum.Location = new System.Drawing.Point(511, 25);
            this.cmbMachChrtNum.Name = "cmbMachChrtNum";
            this.cmbMachChrtNum.Size = new System.Drawing.Size(97, 21);
            this.cmbMachChrtNum.TabIndex = 63;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(408, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 62;
            this.label4.Text = "Chart Part Number:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(366, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 62;
            this.label2.Text = "Drive Name:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbMachDrvNum
            // 
            this.cmbMachDrvNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMachDrvNum.FormattingEnabled = true;
            this.cmbMachDrvNum.Location = new System.Drawing.Point(302, 66);
            this.cmbMachDrvNum.Name = "cmbMachDrvNum";
            this.cmbMachDrvNum.Size = new System.Drawing.Size(42, 21);
            this.cmbMachDrvNum.TabIndex = 64;
            this.cmbMachDrvNum.SelectedIndexChanged += new System.EventHandler(this.cmbMachDrvNum_SelectedIndexChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1266, 805);
            this.Controls.Add(this.grpSetMach);
            this.Controls.Add(this.grpSetMotor);
            this.Controls.Add(this.grpCommSettings);
            this.Controls.Add(this.grpSetDrive);
            this.Controls.Add(this.grpParamChng);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "VFD Parameter Programmer & Monitor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.grpCommSettings.ResumeLayout(false);
            this.grpCommSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParamViewFull)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ctxtSchedChng.ResumeLayout(false);
            this.ctxtDriveMod.ResumeLayout(false);
            this.grpSetDrive.ResumeLayout(false);
            this.grpSetDrive.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParamViewMisMatch)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParamViewChng)).EndInit();
            this.grpParamChng.ResumeLayout(false);
            this.grpParamChng.PerformLayout();
            this.grpSetMotor.ResumeLayout(false);
            this.grpSetMotor.PerformLayout();
            this.grpSetMach.ResumeLayout(false);
            this.grpSetMach.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtSlaveAddr;
        private System.Windows.Forms.Label lblSlaveAddr;
        private System.Windows.Forms.GroupBox grpCommSettings;
        private System.Windows.Forms.Label lblSerialCommPort;
        private System.Windows.Forms.ComboBox cmbSerialPort;
        private System.IO.Ports.SerialPort spVFD;
        private System.Windows.Forms.DataGridView dgvParamViewFull;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statProgLabel;
        private System.Windows.Forms.ToolStripProgressBar statProgress;
        private System.Windows.Forms.Button btnVFDRead;
        private System.ComponentModel.BackgroundWorker bwrkReadVFDVals;
        private System.Windows.Forms.Button btnVFDMod;
        private System.ComponentModel.BackgroundWorker bwrkModVFD;
        private System.Windows.Forms.GroupBox grpSetDrive;
        private System.Windows.Forms.ComboBox cmbParamGroup;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnVFDReset;
        private System.Windows.Forms.Button btnVFDVer;
        private System.Windows.Forms.ComboBox cmbDriveList;
        private System.Windows.Forms.Label lblParamGroup;
        private System.Windows.Forms.Label lblDriveSel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem msFile;
        private System.Windows.Forms.ToolStripMenuItem msFile_LoadParamList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem msFile_Exit;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ctxtDriveMod;
        private System.Windows.Forms.ToolStripMenuItem ctxtDriveMod_Save;
        private System.Windows.Forms.ContextMenuStrip ctxtSchedChng;
        private System.Windows.Forms.ToolStripMenuItem ctxtSchedChng_Save;
        private System.Windows.Forms.Label lblParamFullList;
        private System.Windows.Forms.Label lblParamModSched;
        private System.Windows.Forms.DataGridView dgvParamViewChng;
        private System.Windows.Forms.Label lblParamChngSrc;
        private System.Windows.Forms.TextBox txtParamChngSrc;
        private System.Windows.Forms.Label lblVoltMachSupply;
        private System.Windows.Forms.ComboBox cmbMtrVoltSupply;
        private System.Windows.Forms.Label lblVoltMotorMax;
        private System.Windows.Forms.ComboBox cmbMtrVoltMax;
        private System.Windows.Forms.DataGridView dgvParamViewMisMatch;
        private System.Windows.Forms.Label lblParamMismatch;
        private System.Windows.Forms.Label lblFreqMotorBase;
        private System.Windows.Forms.ComboBox cmbMtrFreqBase;
        private System.Windows.Forms.Label lblMotorFLC;
        private System.Windows.Forms.Label lblUnitsAmps1;
        private System.Windows.Forms.TextBox txtMtrFLC;
        private System.Windows.Forms.Button btnMtrSet;
        private System.Windows.Forms.GroupBox grpParamChng;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmRegAddr;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmParamNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmParmName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmDefVal;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmVFDVal;
        private System.ComponentModel.BackgroundWorker bwrkVFDVerify;
        private System.Windows.Forms.Label lblDriveDuty;
        private System.Windows.Forms.ComboBox cmbDriveDuty;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmMisMatchParamAddr;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmMisMatchParamNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmMisMatchParamName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmMisMatchDefVal;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmMisMatchReadVal;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.ToolStripMenuItem ctxtSchedChng_Clear;
        private System.Windows.Forms.ToolStripMenuItem ctxtDriveMod_Clear;
        private System.Windows.Forms.Label lblSelMotor;
        private System.Windows.Forms.ComboBox cmbMachSel;
        private System.Windows.Forms.Label lblSelMach;
        private System.Windows.Forms.ComboBox cmbMtrPartNum;
        private System.Windows.Forms.Label lblMotorPartNum;
        private System.Windows.Forms.ToolStripMenuItem ctxtSchedChng_Load;
        private System.Windows.Forms.ComboBox cmbMtrFreqSupply;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpSetMotor;
        private System.Windows.Forms.GroupBox grpSetMach;
        private System.Windows.Forms.Button btnMachListStore;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnMachListLoad;
        private System.Windows.Forms.ComboBox cmbMachDrvNum;
        private System.Windows.Forms.Button btnMtrStore;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMachDrvCnt;
        private System.Windows.Forms.ComboBox cmbMachChrtNum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMachChrtCnt;
        private System.Windows.Forms.Button btnMachListDel;
        private System.Windows.Forms.TextBox txtMachDrvName;
        private System.Windows.Forms.Button btnMtrDel;
        private System.Windows.Forms.ToolStripMenuItem ctxtSchedChng_Remove;
    }
}

