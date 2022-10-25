namespace ICApiAddin.icPowerApps
{
    partial class UserControlSuppressManager
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
            this.tableLayoutPanelBase.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanelBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBase.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelBase.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelBase.Name = "tableLayoutPanelBase";
            this.tableLayoutPanelBase.RowCount = 1;
            this.tableLayoutPanelBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBase.Size = new System.Drawing.Size(550, 782);
            this.tableLayoutPanelBase.TabIndex = 2;
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(544, 776);
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
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 729);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(538, 44);
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
            this.buttonNewSuppress.Location = new System.Drawing.Point(370, 3);
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
            this.buttonExportFile.Location = new System.Drawing.Point(243, 3);
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
            this.buttonImportFile.Location = new System.Drawing.Point(103, 3);
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
            this.tableLayoutPanel2.Size = new System.Drawing.Size(538, 44);
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
            this.comboBoxSuppress.Location = new System.Drawing.Point(3, 9);
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
            this.tableLayoutPanel5.Size = new System.Drawing.Size(538, 670);
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
            this.treeGridViewScene.Size = new System.Drawing.Size(532, 641);
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
            this.progressBarWaitProgress.Location = new System.Drawing.Point(3, 644);
            this.progressBarWaitProgress.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.progressBarWaitProgress.MarqueeAnimationSpeed = 50;
            this.progressBarWaitProgress.Name = "progressBarWaitProgress";
            this.progressBarWaitProgress.Size = new System.Drawing.Size(532, 23);
            this.progressBarWaitProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarWaitProgress.TabIndex = 1;
            // 
            // UserControlSuppressManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tableLayoutPanelBase);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UserControlSuppressManager";
            this.Size = new System.Drawing.Size(550, 782);
            this.VisibleChanged += new System.EventHandler(this.UserControlSuppressManager_VisibleChanged);
            this.tableLayoutPanelBase.ResumeLayout(false);
            this.tableLayoutPanelBase.PerformLayout();
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
    }
}
