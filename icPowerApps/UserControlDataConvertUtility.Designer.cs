namespace ICApiAddin.icPowerApps
{
    partial class UserControlDataConvertUtility
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanelBase = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSelectFiles = new System.Windows.Forms.Button();
            this.buttonSelectFolder = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxAddChildFolder = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridViewFileList = new System.Windows.Forms.DataGridView();
            this.ConvertResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanelOptionExportStep = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxStepUnit = new System.Windows.Forms.ComboBox();
            this.comboBoxExportFormat = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanelOptionExportIges = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxIgesType = new System.Windows.Forms.ComboBox();
            this.comboBoxIgesFormat = new System.Windows.Forms.ComboBox();
            this.checkBoxIgesNurbs = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonExecute = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelOptionExportCatia = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxCatiaVersion = new System.Windows.Forms.ComboBox();
            this.checkBoxCatiaExport3dCurve = new System.Windows.Forms.CheckBox();
            this.checkBoxCatiaNotConfirmNextSession = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanelBase.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFileList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanelOptionExportStep.SuspendLayout();
            this.tableLayoutPanelOptionExportIges.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanelOptionExportCatia.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelBase
            // 
            this.tableLayoutPanelBase.AutoSize = true;
            this.tableLayoutPanelBase.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelBase.ColumnCount = 1;
            this.tableLayoutPanelBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBase.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanelBase.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanelBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBase.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelBase.Margin = new System.Windows.Forms.Padding(2, 2, 2, 0);
            this.tableLayoutPanelBase.Name = "tableLayoutPanelBase";
            this.tableLayoutPanelBase.RowCount = 2;
            this.tableLayoutPanelBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelBase.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBase.Size = new System.Drawing.Size(1039, 693);
            this.tableLayoutPanelBase.TabIndex = 4;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 24);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 560F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1031, 665);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel11, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel5, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel12, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(8, 8);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1015, 649);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.AutoSize = true;
            this.tableLayoutPanel11.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel11.ColumnCount = 4;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel11.Controls.Add(this.buttonSelectFiles, 1, 0);
            this.tableLayoutPanel11.Controls.Add(this.buttonSelectFolder, 2, 0);
            this.tableLayoutPanel11.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel11.Controls.Add(this.checkBoxAddChildFolder, 3, 0);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel11.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(1015, 37);
            this.tableLayoutPanel11.TabIndex = 1;
            // 
            // buttonSelectFiles
            // 
            this.buttonSelectFiles.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonSelectFiles.Location = new System.Drawing.Point(625, 4);
            this.buttonSelectFiles.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSelectFiles.Name = "buttonSelectFiles";
            this.buttonSelectFiles.Size = new System.Drawing.Size(110, 29);
            this.buttonSelectFiles.TabIndex = 2;
            this.buttonSelectFiles.Text = "ファイルを選択";
            this.buttonSelectFiles.UseVisualStyleBackColor = true;
            this.buttonSelectFiles.Click += new System.EventHandler(this.buttonSelectFiles_Click);
            // 
            // buttonSelectFolder
            // 
            this.buttonSelectFolder.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonSelectFolder.Location = new System.Drawing.Point(743, 4);
            this.buttonSelectFolder.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSelectFolder.Name = "buttonSelectFolder";
            this.buttonSelectFolder.Size = new System.Drawing.Size(113, 29);
            this.buttonSelectFolder.TabIndex = 3;
            this.buttonSelectFolder.Text = "フォルダを選択";
            this.buttonSelectFolder.UseVisualStyleBackColor = true;
            this.buttonSelectFolder.Click += new System.EventHandler(this.buttonSelectFolder_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 22);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "一括変換するファイル一覧";
            // 
            // checkBoxAddChildFolder
            // 
            this.checkBoxAddChildFolder.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxAddChildFolder.AutoSize = true;
            this.checkBoxAddChildFolder.Location = new System.Drawing.Point(864, 9);
            this.checkBoxAddChildFolder.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxAddChildFolder.Name = "checkBoxAddChildFolder";
            this.checkBoxAddChildFolder.Size = new System.Drawing.Size(147, 19);
            this.checkBoxAddChildFolder.TabIndex = 4;
            this.checkBoxAddChildFolder.Text = "配下のフォルダも追加する";
            this.checkBoxAddChildFolder.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.dataGridViewFileList, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 37);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1015, 279);
            this.tableLayoutPanel5.TabIndex = 1;
            // 
            // dataGridViewFileList
            // 
            this.dataGridViewFileList.AllowUserToAddRows = false;
            this.dataGridViewFileList.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridViewFileList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFileList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ConvertResult,
            this.FileName,
            this.FilePath});
            this.dataGridViewFileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewFileList.Location = new System.Drawing.Point(4, 1);
            this.dataGridViewFileList.Margin = new System.Windows.Forms.Padding(4, 1, 4, 4);
            this.dataGridViewFileList.Name = "dataGridViewFileList";
            this.dataGridViewFileList.RowTemplate.Height = 21;
            this.dataGridViewFileList.Size = new System.Drawing.Size(1007, 269);
            this.dataGridViewFileList.TabIndex = 0;
            // 
            // ConvertResult
            // 
            this.ConvertResult.HeaderText = "変換";
            this.ConvertResult.Name = "ConvertResult";
            // 
            // FileName
            // 
            this.FileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.FileName.HeaderText = "ファイル名";
            this.FileName.MinimumWidth = 200;
            this.FileName.Name = "FileName";
            this.FileName.ReadOnly = true;
            this.FileName.Width = 200;
            // 
            // FilePath
            // 
            this.FilePath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FilePath.HeaderText = "ファイルパス";
            this.FilePath.MinimumWidth = 300;
            this.FilePath.Name = "FilePath";
            this.FilePath.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.tableLayoutPanel3);
            this.groupBox1.Location = new System.Drawing.Point(3, 319);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(386, 280);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "変換後の設定";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanelOptionExportStep, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.comboBoxExportFormat, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanelOptionExportIges, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanelOptionExportCatia, 1, 3);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 19);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 5;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(380, 258);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "エクスポートの種類";
            // 
            // tableLayoutPanelOptionExportStep
            // 
            this.tableLayoutPanelOptionExportStep.AutoSize = true;
            this.tableLayoutPanelOptionExportStep.ColumnCount = 2;
            this.tableLayoutPanelOptionExportStep.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelOptionExportStep.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelOptionExportStep.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanelOptionExportStep.Controls.Add(this.comboBoxStepUnit, 1, 0);
            this.tableLayoutPanelOptionExportStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelOptionExportStep.Location = new System.Drawing.Point(104, 32);
            this.tableLayoutPanelOptionExportStep.Name = "tableLayoutPanelOptionExportStep";
            this.tableLayoutPanelOptionExportStep.RowCount = 1;
            this.tableLayoutPanelOptionExportStep.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelOptionExportStep.Size = new System.Drawing.Size(273, 29);
            this.tableLayoutPanelOptionExportStep.TabIndex = 4;
            this.tableLayoutPanelOptionExportStep.Visible = false;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "エクスポートの単位";
            // 
            // comboBoxStepUnit
            // 
            this.comboBoxStepUnit.FormattingEnabled = true;
            this.comboBoxStepUnit.Items.AddRange(new object[] {
            "ミリメートル",
            "センチメートル",
            "メートル",
            "フィート",
            "インチ"});
            this.comboBoxStepUnit.Location = new System.Drawing.Point(104, 3);
            this.comboBoxStepUnit.Name = "comboBoxStepUnit";
            this.comboBoxStepUnit.Size = new System.Drawing.Size(121, 23);
            this.comboBoxStepUnit.TabIndex = 1;
            // 
            // comboBoxExportFormat
            // 
            this.comboBoxExportFormat.FormattingEnabled = true;
            this.comboBoxExportFormat.Items.AddRange(new object[] {
            "ACIS 2021 R31(*.sat)",
            "ACIS R29 (*.sat)",
            "ACIS R26 (*.sat)",
            "Parasolid 34.0 (*.x_t)",
            "Parasolid 31.1 (*.x_t)",
            "Parasolid 29.0 (*.x_t)",
            "Parasolid 26.1 (*.x_t)",
            "STEP AP203 (*.stp,*.step)",
            "STEP AP214 (*.stp,*.step)",
            "IGES (*.igs,*.iges)",
            "CATIA V4 (*.model)",
            "CATIA V5 パーツ (*.CatPart)",
            "CATIA V5 アセンブリ (*.CatProduct)",
            "3D PDF ファイル (*.pdf)"});
            this.comboBoxExportFormat.Location = new System.Drawing.Point(104, 3);
            this.comboBoxExportFormat.Name = "comboBoxExportFormat";
            this.comboBoxExportFormat.Size = new System.Drawing.Size(225, 23);
            this.comboBoxExportFormat.TabIndex = 6;
            this.comboBoxExportFormat.TextChanged += new System.EventHandler(this.comboBoxExportFormat_TextChanged);
            // 
            // tableLayoutPanelOptionExportIges
            // 
            this.tableLayoutPanelOptionExportIges.AutoSize = true;
            this.tableLayoutPanelOptionExportIges.ColumnCount = 2;
            this.tableLayoutPanelOptionExportIges.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelOptionExportIges.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelOptionExportIges.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanelOptionExportIges.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanelOptionExportIges.Controls.Add(this.comboBoxIgesType, 1, 0);
            this.tableLayoutPanelOptionExportIges.Controls.Add(this.comboBoxIgesFormat, 1, 1);
            this.tableLayoutPanelOptionExportIges.Controls.Add(this.checkBoxIgesNurbs, 0, 2);
            this.tableLayoutPanelOptionExportIges.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelOptionExportIges.Location = new System.Drawing.Point(104, 67);
            this.tableLayoutPanelOptionExportIges.Name = "tableLayoutPanelOptionExportIges";
            this.tableLayoutPanelOptionExportIges.RowCount = 3;
            this.tableLayoutPanelOptionExportIges.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelOptionExportIges.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelOptionExportIges.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelOptionExportIges.Size = new System.Drawing.Size(273, 83);
            this.tableLayoutPanelOptionExportIges.TabIndex = 8;
            this.tableLayoutPanelOptionExportIges.Visible = false;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 15);
            this.label6.TabIndex = 5;
            this.label6.Text = "出力の形式";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "出力の種類";
            // 
            // comboBoxIgesType
            // 
            this.comboBoxIgesType.FormattingEnabled = true;
            this.comboBoxIgesType.Items.AddRange(new object[] {
            "標準",
            "JAMA-IS",
            "ANSYS",
            "MSC/NASTRAN",
            "COSMOS",
            "Working Model",
            "PROCAM",
            "ESPRIT (D.P.Technology)",
            "MasterCAM",
            "SURFCAM",
            "SmartCAM",
            "AlphaCAM",
            "Pro/ENGINEER",
            "AutoCAD",
            "SDRC"});
            this.comboBoxIgesType.Location = new System.Drawing.Point(74, 3);
            this.comboBoxIgesType.Name = "comboBoxIgesType";
            this.comboBoxIgesType.Size = new System.Drawing.Size(151, 23);
            this.comboBoxIgesType.TabIndex = 0;
            // 
            // comboBoxIgesFormat
            // 
            this.comboBoxIgesFormat.FormattingEnabled = true;
            this.comboBoxIgesFormat.Items.AddRange(new object[] {
            "ソリッド",
            "トリム サーフェス",
            "図形"});
            this.comboBoxIgesFormat.Location = new System.Drawing.Point(74, 32);
            this.comboBoxIgesFormat.Name = "comboBoxIgesFormat";
            this.comboBoxIgesFormat.Size = new System.Drawing.Size(151, 23);
            this.comboBoxIgesFormat.TabIndex = 1;
            // 
            // checkBoxIgesNurbs
            // 
            this.checkBoxIgesNurbs.AutoSize = true;
            this.tableLayoutPanelOptionExportIges.SetColumnSpan(this.checkBoxIgesNurbs, 2);
            this.checkBoxIgesNurbs.Location = new System.Drawing.Point(3, 61);
            this.checkBoxIgesNurbs.Name = "checkBoxIgesNurbs";
            this.checkBoxIgesNurbs.Size = new System.Drawing.Size(267, 19);
            this.checkBoxIgesNurbs.TabIndex = 2;
            this.checkBoxIgesNurbs.Text = "すべてのサーフェスを NURBS としてエクスポートする";
            this.checkBoxIgesNurbs.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.AutoSize = true;
            this.tableLayoutPanel12.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel12.ColumnCount = 3;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel12.Controls.Add(this.buttonExecute, 2, 0);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(4, 606);
            this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 1;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel12.Size = new System.Drawing.Size(1007, 39);
            this.tableLayoutPanel12.TabIndex = 2;
            // 
            // buttonExecute
            // 
            this.buttonExecute.Location = new System.Drawing.Point(899, 4);
            this.buttonExecute.Margin = new System.Windows.Forms.Padding(23, 4, 4, 4);
            this.buttonExecute.Name = "buttonExecute";
            this.buttonExecute.Size = new System.Drawing.Size(104, 31);
            this.buttonExecute.TabIndex = 0;
            this.buttonExecute.Text = "実行";
            this.buttonExecute.UseVisualStyleBackColor = true;
            this.buttonExecute.Click += new System.EventHandler(this.buttonExecute_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1033, 14);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // tableLayoutPanelOptionExportCatia
            // 
            this.tableLayoutPanelOptionExportCatia.AutoSize = true;
            this.tableLayoutPanelOptionExportCatia.ColumnCount = 2;
            this.tableLayoutPanelOptionExportCatia.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelOptionExportCatia.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelOptionExportCatia.Controls.Add(this.checkBoxCatiaNotConfirmNextSession, 0, 2);
            this.tableLayoutPanelOptionExportCatia.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanelOptionExportCatia.Controls.Add(this.comboBoxCatiaVersion, 1, 0);
            this.tableLayoutPanelOptionExportCatia.Controls.Add(this.checkBoxCatiaExport3dCurve, 0, 1);
            this.tableLayoutPanelOptionExportCatia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelOptionExportCatia.Location = new System.Drawing.Point(104, 156);
            this.tableLayoutPanelOptionExportCatia.Name = "tableLayoutPanelOptionExportCatia";
            this.tableLayoutPanelOptionExportCatia.RowCount = 3;
            this.tableLayoutPanelOptionExportCatia.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelOptionExportCatia.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelOptionExportCatia.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelOptionExportCatia.Size = new System.Drawing.Size(273, 79);
            this.tableLayoutPanelOptionExportCatia.TabIndex = 9;
            this.tableLayoutPanelOptionExportCatia.Visible = false;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "バージョン番号(15-29)";
            // 
            // comboBoxCatiaVersion
            // 
            this.comboBoxCatiaVersion.FormattingEnabled = true;
            this.comboBoxCatiaVersion.Items.AddRange(new object[] {
            "29",
            "28",
            "27",
            "26",
            "25",
            "24",
            "23",
            "22",
            "21",
            "20",
            "19",
            "18",
            "17",
            "16",
            "15"});
            this.comboBoxCatiaVersion.Location = new System.Drawing.Point(128, 3);
            this.comboBoxCatiaVersion.Name = "comboBoxCatiaVersion";
            this.comboBoxCatiaVersion.Size = new System.Drawing.Size(97, 23);
            this.comboBoxCatiaVersion.TabIndex = 1;
            // 
            // checkBoxCatiaExport3dCurve
            // 
            this.checkBoxCatiaExport3dCurve.AutoSize = true;
            this.checkBoxCatiaExport3dCurve.Checked = true;
            this.checkBoxCatiaExport3dCurve.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanelOptionExportCatia.SetColumnSpan(this.checkBoxCatiaExport3dCurve, 2);
            this.checkBoxCatiaExport3dCurve.Location = new System.Drawing.Point(3, 32);
            this.checkBoxCatiaExport3dCurve.Name = "checkBoxCatiaExport3dCurve";
            this.checkBoxCatiaExport3dCurve.Size = new System.Drawing.Size(148, 19);
            this.checkBoxCatiaExport3dCurve.TabIndex = 3;
            this.checkBoxCatiaExport3dCurve.Text = "3D曲線をエクスポートする";
            this.checkBoxCatiaExport3dCurve.UseVisualStyleBackColor = true;
            // 
            // checkBoxCatiaNotConfirmNextSession
            // 
            this.checkBoxCatiaNotConfirmNextSession.AutoSize = true;
            this.tableLayoutPanelOptionExportCatia.SetColumnSpan(this.checkBoxCatiaNotConfirmNextSession, 2);
            this.checkBoxCatiaNotConfirmNextSession.Location = new System.Drawing.Point(3, 57);
            this.checkBoxCatiaNotConfirmNextSession.Name = "checkBoxCatiaNotConfirmNextSession";
            this.checkBoxCatiaNotConfirmNextSession.Size = new System.Drawing.Size(162, 19);
            this.checkBoxCatiaNotConfirmNextSession.TabIndex = 4;
            this.checkBoxCatiaNotConfirmNextSession.Text = "次のセッションまで確認しない";
            this.checkBoxCatiaNotConfirmNextSession.UseVisualStyleBackColor = true;
            // 
            // UserControlDataConvertUtility
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tableLayoutPanelBase);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UserControlDataConvertUtility";
            this.Size = new System.Drawing.Size(1039, 693);
            this.VisibleChanged += new System.EventHandler(this.UserControlDataConvertUtility_VisibleChanged);
            this.tableLayoutPanelBase.ResumeLayout(false);
            this.tableLayoutPanelBase.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel11.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFileList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanelOptionExportStep.ResumeLayout(false);
            this.tableLayoutPanelOptionExportStep.PerformLayout();
            this.tableLayoutPanelOptionExportIges.ResumeLayout(false);
            this.tableLayoutPanelOptionExportIges.PerformLayout();
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanelOptionExportCatia.ResumeLayout(false);
            this.tableLayoutPanelOptionExportCatia.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBase;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.DataGridView dataGridViewFileList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.Button buttonSelectFiles;
        private System.Windows.Forms.Button buttonSelectFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private System.Windows.Forms.Button buttonExecute;
        private System.Windows.Forms.CheckBox checkBoxAddChildFolder;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelOptionExportStep;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxStepUnit;
        private System.Windows.Forms.ComboBox comboBoxExportFormat;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelOptionExportIges;
        private System.Windows.Forms.ComboBox comboBoxIgesType;
        private System.Windows.Forms.ComboBox comboBoxIgesFormat;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxIgesNurbs;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConvertResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FilePath;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelOptionExportCatia;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxCatiaVersion;
        private System.Windows.Forms.CheckBox checkBoxCatiaNotConfirmNextSession;
        private System.Windows.Forms.CheckBox checkBoxCatiaExport3dCurve;
    }
}
