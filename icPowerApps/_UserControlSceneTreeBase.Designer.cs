namespace ICApiAddin.icPowerApps
{
    partial class _UserControlSceneTreeBase
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
            this.treeGridViewScene = new AdvancedDataGridView.TreeGridView();
            this.Scene = new AdvancedDataGridView.TreeGridColumn();
            this.SystemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Depth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExternalLink = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanelBase.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeGridViewScene)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelBase
            // 
            this.tableLayoutPanelBase.ColumnCount = 1;
            this.tableLayoutPanelBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBase.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanelBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBase.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelBase.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelBase.Name = "tableLayoutPanelBase";
            this.tableLayoutPanelBase.RowCount = 1;
            this.tableLayoutPanelBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBase.Size = new System.Drawing.Size(870, 778);
            this.tableLayoutPanelBase.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.treeGridViewScene, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(864, 772);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // treeGridViewScene
            // 
            this.treeGridViewScene.AllowUserToAddRows = false;
            this.treeGridViewScene.AllowUserToDeleteRows = false;
            this.treeGridViewScene.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
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
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.treeGridViewScene.DefaultCellStyle = dataGridViewCellStyle1;
            this.treeGridViewScene.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeGridViewScene.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.treeGridViewScene.ImageList = null;
            this.treeGridViewScene.Location = new System.Drawing.Point(3, 3);
            this.treeGridViewScene.Name = "treeGridViewScene";
            this.treeGridViewScene.RowHeadersVisible = false;
            this.treeGridViewScene.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.treeGridViewScene.Size = new System.Drawing.Size(858, 766);
            this.treeGridViewScene.TabIndex = 0;
            // 
            // Scene
            // 
            this.Scene.DefaultNodeImage = null;
            this.Scene.HeaderText = "シーン";
            this.Scene.Name = "Scene";
            this.Scene.ReadOnly = true;
            this.Scene.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Scene.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SystemName
            // 
            this.SystemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SystemName.FillWeight = 80F;
            this.SystemName.HeaderText = "システム名";
            this.SystemName.Name = "SystemName";
            this.SystemName.ReadOnly = true;
            this.SystemName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SystemName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SystemName.Width = 76;
            // 
            // id
            // 
            this.id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.id.FillWeight = 50F;
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.id.Width = 32;
            // 
            // DataType
            // 
            this.DataType.FillWeight = 80F;
            this.DataType.HeaderText = "データ種別";
            this.DataType.Name = "DataType";
            this.DataType.ReadOnly = true;
            this.DataType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Depth
            // 
            this.Depth.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Depth.FillWeight = 30F;
            this.Depth.HeaderText = "深さ";
            this.Depth.Name = "Depth";
            this.Depth.ReadOnly = true;
            this.Depth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Depth.Width = 40;
            // 
            // ExternalLink
            // 
            this.ExternalLink.FillWeight = 80F;
            this.ExternalLink.HeaderText = "外部リンク";
            this.ExternalLink.Name = "ExternalLink";
            this.ExternalLink.ReadOnly = true;
            this.ExternalLink.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // UserControlSystemInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tableLayoutPanelBase);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UserControlSystemInformation";
            this.Size = new System.Drawing.Size(870, 778);
            this.VisibleChanged += new System.EventHandler(this._UserControlSceneTreeBase_VisibleChanged);
            this.tableLayoutPanelBase.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeGridViewScene)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBase;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AdvancedDataGridView.TreeGridView treeGridViewScene;
        private AdvancedDataGridView.TreeGridColumn Scene;
        private System.Windows.Forms.DataGridViewTextBoxColumn SystemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Depth;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExternalLink;
    }
}
