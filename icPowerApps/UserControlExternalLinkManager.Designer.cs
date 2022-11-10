namespace ICApiAddin.icPowerApps
{
    partial class UserControlExternalLinkManager
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanelBase = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxOptionSaveFileNameUserName = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonExternalUnlink = new System.Windows.Forms.RadioButton();
            this.radioButtonExternalLink = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonExecute = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonDeleteSuppress = new System.Windows.Forms.Button();
            this.buttonNewSuppress = new System.Windows.Forms.Button();
            this.buttonExportFile = new System.Windows.Forms.Button();
            this.buttonImportFile = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonGetCurrentSuppress = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSetSuppress = new System.Windows.Forms.Button();
            this.comboBoxSuppress = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.treeGridViewScene = new AdvancedDataGridView.TreeGridView();
            this.Scene = new AdvancedDataGridView.TreeGridColumn();
            this.SystemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Depth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExternalLink = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.progressBarWaitProgress = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanelBase.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeGridViewScene)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelBase
            // 
            this.tableLayoutPanelBase.AutoSize = true;
            this.tableLayoutPanelBase.ColumnCount = 1;
            this.tableLayoutPanelBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBase.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanelBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBase.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelBase.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelBase.Name = "tableLayoutPanelBase";
            this.tableLayoutPanelBase.RowCount = 1;
            this.tableLayoutPanelBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBase.Size = new System.Drawing.Size(585, 228);
            this.tableLayoutPanelBase.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(579, 222);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel9);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(571, 194);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "IRONCADのシーンブラウザで選択";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.AutoSize = true;
            this.tableLayoutPanel9.ColumnCount = 1;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel6, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel10, 0, 2);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.Padding = new System.Windows.Forms.Padding(10, 10, 10, 5);
            this.tableLayoutPanel9.RowCount = 3;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(565, 188);
            this.tableLayoutPanel9.TabIndex = 1;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.AutoSize = true;
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.groupBox1, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(13, 35);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(539, 88);
            this.tableLayoutPanel6.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.tableLayoutPanel8);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(198, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(15, 8, 10, 8);
            this.groupBox1.Size = new System.Drawing.Size(286, 82);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "オプション";
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.AutoSize = true;
            this.tableLayoutPanel8.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel8.Controls.Add(this.checkBoxOptionSaveFileNameUserName, 0, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(15, 24);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 2;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(261, 50);
            this.tableLayoutPanel8.TabIndex = 0;
            // 
            // checkBoxOptionSaveFileNameUserName
            // 
            this.checkBoxOptionSaveFileNameUserName.AutoSize = true;
            this.checkBoxOptionSaveFileNameUserName.Location = new System.Drawing.Point(3, 3);
            this.checkBoxOptionSaveFileNameUserName.Name = "checkBoxOptionSaveFileNameUserName";
            this.checkBoxOptionSaveFileNameUserName.Size = new System.Drawing.Size(255, 19);
            this.checkBoxOptionSaveFileNameUserName.TabIndex = 0;
            this.checkBoxOptionSaveFileNameUserName.Text = "保存時のファイル名をユーザ名にする(自動保存)";
            this.checkBoxOptionSaveFileNameUserName.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox2.Controls.Add(this.tableLayoutPanel7);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(8);
            this.groupBox2.Size = new System.Drawing.Size(189, 82);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "外部リンクの設定/解除";
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.AutoSize = true;
            this.tableLayoutPanel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.Controls.Add(this.radioButtonExternalUnlink, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.radioButtonExternalLink, 0, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(8, 24);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 3;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(173, 50);
            this.tableLayoutPanel7.TabIndex = 0;
            // 
            // radioButtonExternalUnlink
            // 
            this.radioButtonExternalUnlink.AutoSize = true;
            this.radioButtonExternalUnlink.Location = new System.Drawing.Point(20, 28);
            this.radioButtonExternalUnlink.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.radioButtonExternalUnlink.Name = "radioButtonExternalUnlink";
            this.radioButtonExternalUnlink.Size = new System.Drawing.Size(150, 19);
            this.radioButtonExternalUnlink.TabIndex = 1;
            this.radioButtonExternalUnlink.Text = "外部リンクファイル解除する";
            this.radioButtonExternalUnlink.UseVisualStyleBackColor = true;
            // 
            // radioButtonExternalLink
            // 
            this.radioButtonExternalLink.AutoSize = true;
            this.radioButtonExternalLink.Checked = true;
            this.radioButtonExternalLink.Location = new System.Drawing.Point(20, 3);
            this.radioButtonExternalLink.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.radioButtonExternalLink.Name = "radioButtonExternalLink";
            this.radioButtonExternalLink.Size = new System.Drawing.Size(135, 19);
            this.radioButtonExternalLink.TabIndex = 0;
            this.radioButtonExternalLink.TabStop = true;
            this.radioButtonExternalLink.Text = "外部リンクファイルにする";
            this.radioButtonExternalLink.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 7);
            this.label1.Size = new System.Drawing.Size(380, 22);
            this.label1.TabIndex = 2;
            this.label1.Text = "シーンブラウザで選択したアセンブリ/パーツに外部リンクを設定または解除します。";
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.AutoSize = true;
            this.tableLayoutPanel10.ColumnCount = 2;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.Controls.Add(this.buttonExecute, 1, 0);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(10, 126);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.Size = new System.Drawing.Size(545, 57);
            this.tableLayoutPanel10.TabIndex = 1;
            // 
            // buttonExecute
            // 
            this.buttonExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExecute.Location = new System.Drawing.Point(429, 3);
            this.buttonExecute.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.buttonExecute.Name = "buttonExecute";
            this.buttonExecute.Size = new System.Drawing.Size(96, 31);
            this.buttonExecute.TabIndex = 0;
            this.buttonExecute.Text = "実行する";
            this.buttonExecute.UseVisualStyleBackColor = true;
            this.buttonExecute.Click += new System.EventHandler(this.buttonExecute_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(571, 194);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "独自のシーンブラウザで選択";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(565, 188);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.ColumnCount = 5;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.buttonDeleteSuppress, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonNewSuppress, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonExportFile, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonImportFile, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 141);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(800, 44);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // buttonDeleteSuppress
            // 
            this.buttonDeleteSuppress.Location = new System.Drawing.Point(3, 3);
            this.buttonDeleteSuppress.Name = "buttonDeleteSuppress";
            this.buttonDeleteSuppress.Size = new System.Drawing.Size(75, 29);
            this.buttonDeleteSuppress.TabIndex = 2;
            this.buttonDeleteSuppress.Text = "列を削除";
            this.buttonDeleteSuppress.UseVisualStyleBackColor = true;
            this.buttonDeleteSuppress.Click += new System.EventHandler(this.buttonDeleteSuppress_Click);
            // 
            // buttonNewSuppress
            // 
            this.buttonNewSuppress.Location = new System.Drawing.Point(632, 3);
            this.buttonNewSuppress.Name = "buttonNewSuppress";
            this.buttonNewSuppress.Size = new System.Drawing.Size(165, 29);
            this.buttonNewSuppress.TabIndex = 1;
            this.buttonNewSuppress.Text = "新しい状態セットを作成";
            this.buttonNewSuppress.UseVisualStyleBackColor = true;
            this.buttonNewSuppress.Click += new System.EventHandler(this.buttonNewSuppress_Click);
            // 
            // buttonExportFile
            // 
            this.buttonExportFile.AutoSize = true;
            this.buttonExportFile.Location = new System.Drawing.Point(505, 3);
            this.buttonExportFile.Name = "buttonExportFile";
            this.buttonExportFile.Size = new System.Drawing.Size(121, 33);
            this.buttonExportFile.TabIndex = 1;
            this.buttonExportFile.Text = "ファイルに保存";
            this.buttonExportFile.UseVisualStyleBackColor = true;
            this.buttonExportFile.Click += new System.EventHandler(this.buttonExportFile_Click);
            // 
            // buttonImportFile
            // 
            this.buttonImportFile.AutoSize = true;
            this.buttonImportFile.Location = new System.Drawing.Point(365, 3);
            this.buttonImportFile.Name = "buttonImportFile";
            this.buttonImportFile.Size = new System.Drawing.Size(134, 33);
            this.buttonImportFile.TabIndex = 0;
            this.buttonImportFile.Text = "ファイルから読込";
            this.buttonImportFile.UseVisualStyleBackColor = true;
            this.buttonImportFile.Click += new System.EventHandler(this.buttonImportFile_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.buttonGetCurrentSuppress, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(800, 44);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // buttonGetCurrentSuppress
            // 
            this.buttonGetCurrentSuppress.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonGetCurrentSuppress.AutoSize = true;
            this.buttonGetCurrentSuppress.Location = new System.Drawing.Point(3, 5);
            this.buttonGetCurrentSuppress.Name = "buttonGetCurrentSuppress";
            this.buttonGetCurrentSuppress.Size = new System.Drawing.Size(193, 33);
            this.buttonGetCurrentSuppress.TabIndex = 0;
            this.buttonGetCurrentSuppress.Text = "現在の抑制状態を取得";
            this.buttonGetCurrentSuppress.UseVisualStyleBackColor = true;
            this.buttonGetCurrentSuppress.Click += new System.EventHandler(this.buttonGetCurrentSuppress_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.buttonSetSuppress, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.comboBoxSuppress, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(202, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(306, 38);
            this.tableLayoutPanel4.TabIndex = 3;
            // 
            // buttonSetSuppress
            // 
            this.buttonSetSuppress.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonSetSuppress.Location = new System.Drawing.Point(236, 4);
            this.buttonSetSuppress.Name = "buttonSetSuppress";
            this.buttonSetSuppress.Size = new System.Drawing.Size(67, 29);
            this.buttonSetSuppress.TabIndex = 2;
            this.buttonSetSuppress.Text = "設定";
            this.buttonSetSuppress.UseVisualStyleBackColor = true;
            this.buttonSetSuppress.Click += new System.EventHandler(this.buttonSetSuppress_Click);
            // 
            // comboBoxSuppress
            // 
            this.comboBoxSuppress.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBoxSuppress.FormattingEnabled = true;
            this.comboBoxSuppress.Location = new System.Drawing.Point(3, 7);
            this.comboBoxSuppress.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.comboBoxSuppress.Name = "comboBoxSuppress";
            this.comboBoxSuppress.Size = new System.Drawing.Size(227, 23);
            this.comboBoxSuppress.TabIndex = 3;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.AutoSize = true;
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.treeGridViewScene, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.progressBarWaitProgress, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 53);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Size = new System.Drawing.Size(800, 82);
            this.tableLayoutPanel5.TabIndex = 3;
            // 
            // treeGridViewScene
            // 
            this.treeGridViewScene.AllowUserToAddRows = false;
            this.treeGridViewScene.AllowUserToDeleteRows = false;
            this.treeGridViewScene.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.treeGridViewScene.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Scene,
            this.SystemName,
            this.id,
            this.DataType,
            this.Depth,
            this.ExternalLink});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.treeGridViewScene.DefaultCellStyle = dataGridViewCellStyle1;
            this.treeGridViewScene.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeGridViewScene.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.treeGridViewScene.ImageList = null;
            this.treeGridViewScene.Location = new System.Drawing.Point(3, 3);
            this.treeGridViewScene.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.treeGridViewScene.Name = "treeGridViewScene";
            this.treeGridViewScene.RowHeadersVisible = false;
            this.treeGridViewScene.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.treeGridViewScene.Size = new System.Drawing.Size(794, 53);
            this.treeGridViewScene.TabIndex = 0;
            this.treeGridViewScene.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.treeGridViewScene_ColumnHeaderMouseDoubleClick);
            // 
            // Scene
            // 
            this.Scene.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Scene.DefaultNodeImage = null;
            this.Scene.HeaderText = "シーン";
            this.Scene.MinimumWidth = 10;
            this.Scene.Name = "Scene";
            this.Scene.ReadOnly = true;
            this.Scene.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Scene.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Scene.Width = 200;
            // 
            // SystemName
            // 
            this.SystemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.SystemName.FillWeight = 80F;
            this.SystemName.HeaderText = "システム名";
            this.SystemName.Name = "SystemName";
            this.SystemName.ReadOnly = true;
            this.SystemName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SystemName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SystemName.Width = 73;
            // 
            // id
            // 
            this.id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.id.FillWeight = 50F;
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.id.Visible = false;
            // 
            // DataType
            // 
            this.DataType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.DataType.FillWeight = 80F;
            this.DataType.HeaderText = "データ種別";
            this.DataType.Name = "DataType";
            this.DataType.ReadOnly = true;
            this.DataType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DataType.Visible = false;
            // 
            // Depth
            // 
            this.Depth.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Depth.FillWeight = 30F;
            this.Depth.HeaderText = "深さ";
            this.Depth.Name = "Depth";
            this.Depth.ReadOnly = true;
            this.Depth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Depth.Visible = false;
            // 
            // ExternalLink
            // 
            this.ExternalLink.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ExternalLink.FillWeight = 80F;
            this.ExternalLink.HeaderText = "外部リンク";
            this.ExternalLink.Name = "ExternalLink";
            this.ExternalLink.ReadOnly = true;
            this.ExternalLink.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ExternalLink.Visible = false;
            this.ExternalLink.Width = 60;
            // 
            // progressBarWaitProgress
            // 
            this.progressBarWaitProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBarWaitProgress.Location = new System.Drawing.Point(3, 56);
            this.progressBarWaitProgress.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.progressBarWaitProgress.MarqueeAnimationSpeed = 50;
            this.progressBarWaitProgress.Name = "progressBarWaitProgress";
            this.progressBarWaitProgress.Size = new System.Drawing.Size(794, 23);
            this.progressBarWaitProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarWaitProgress.TabIndex = 1;
            // 
            // UserControlExternalLinkManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tableLayoutPanelBase);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UserControlExternalLinkManager";
            this.Size = new System.Drawing.Size(585, 228);
            this.VisibleChanged += new System.EventHandler(this.UserControlExternalLinkManager_VisibleChanged);
            this.tableLayoutPanelBase.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeGridViewScene)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBase;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AdvancedDataGridView.TreeGridView treeGridViewScene;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button buttonImportFile;
        private System.Windows.Forms.Button buttonExportFile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button buttonGetCurrentSuppress;
        private System.Windows.Forms.Button buttonNewSuppress;
        private System.Windows.Forms.Button buttonDeleteSuppress;
        private System.Windows.Forms.Button buttonSetSuppress;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.ComboBox comboBoxSuppress;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.ProgressBar progressBarWaitProgress;
        private AdvancedDataGridView.TreeGridColumn Scene;
        private System.Windows.Forms.DataGridViewTextBoxColumn SystemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Depth;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExternalLink;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.RadioButton radioButtonExternalLink;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButtonExternalUnlink;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.CheckBox checkBoxOptionSaveFileNameUserName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.Button buttonExecute;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}
