namespace ICApiAddin.icPowerApps
{
    partial class icPowerAppsTEST
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(icPowerAppsTEST));
            this.toolStripFunction = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelTop = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tableLayoutPanelBase = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelTitle = new System.Windows.Forms.TableLayoutPanel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelFunction = new System.Windows.Forms.Panel();
            this.toolStripButtonIcWebBrowser = new System.Windows.Forms.ToolStripButton();
            this.toolStripFunction.SuspendLayout();
            this.tableLayoutPanelBase.SuspendLayout();
            this.tableLayoutPanelTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripFunction
            // 
            this.toolStripFunction.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStripFunction.Font = new System.Drawing.Font("メイリオ", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripFunction.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripFunction.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStripFunction.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2,
            this.toolStripLabelTop,
            this.toolStripSeparator1,
            this.toolStripButtonIcWebBrowser});
            this.toolStripFunction.Location = new System.Drawing.Point(0, 0);
            this.toolStripFunction.Name = "toolStripFunction";
            this.toolStripFunction.Size = new System.Drawing.Size(188, 781);
            this.toolStripFunction.TabIndex = 8;
            this.toolStripFunction.Text = "toolStrip1";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(185, 6);
            // 
            // toolStripLabelTop
            // 
            this.toolStripLabelTop.Name = "toolStripLabelTop";
            this.toolStripLabelTop.Size = new System.Drawing.Size(185, 28);
            this.toolStripLabelTop.Text = "機能を選択";
            this.toolStripLabelTop.Click += new System.EventHandler(this.toolStripLabelTop_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 7);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(185, 6);
            // 
            // tableLayoutPanelBase
            // 
            this.tableLayoutPanelBase.AutoSize = true;
            this.tableLayoutPanelBase.ColumnCount = 1;
            this.tableLayoutPanelBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBase.Controls.Add(this.tableLayoutPanelTitle, 0, 0);
            this.tableLayoutPanelBase.Controls.Add(this.panelFunction, 0, 1);
            this.tableLayoutPanelBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBase.Location = new System.Drawing.Point(188, 0);
            this.tableLayoutPanelBase.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelBase.Name = "tableLayoutPanelBase";
            this.tableLayoutPanelBase.RowCount = 2;
            this.tableLayoutPanelBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanelBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBase.Size = new System.Drawing.Size(1079, 781);
            this.tableLayoutPanelBase.TabIndex = 9;
            // 
            // tableLayoutPanelTitle
            // 
            this.tableLayoutPanelTitle.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanelTitle.ColumnCount = 1;
            this.tableLayoutPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelTitle.Controls.Add(this.labelTitle, 0, 0);
            this.tableLayoutPanelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTitle.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelTitle.Name = "tableLayoutPanelTitle";
            this.tableLayoutPanelTitle.RowCount = 1;
            this.tableLayoutPanelTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanelTitle.Size = new System.Drawing.Size(1079, 38);
            this.tableLayoutPanelTitle.TabIndex = 8;
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("メイリオ", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelTitle.Location = new System.Drawing.Point(3, 3);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.labelTitle.Size = new System.Drawing.Size(72, 33);
            this.labelTitle.TabIndex = 8;
            this.labelTitle.Text = "Title";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelFunction
            // 
            this.panelFunction.AutoSize = true;
            this.panelFunction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFunction.Location = new System.Drawing.Point(0, 38);
            this.panelFunction.Margin = new System.Windows.Forms.Padding(0);
            this.panelFunction.Name = "panelFunction";
            this.panelFunction.Size = new System.Drawing.Size(1079, 743);
            this.panelFunction.TabIndex = 10;
            // 
            // toolStripButtonIcWebBrowser
            // 
            this.toolStripButtonIcWebBrowser.Font = new System.Drawing.Font("メイリオ", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripButtonIcWebBrowser.Image = global::ICApiAddin.icPowerApps.Properties.Resources.icon_icWebBrowser_l;
            this.toolStripButtonIcWebBrowser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonIcWebBrowser.Margin = new System.Windows.Forms.Padding(7, 1, 0, 2);
            this.toolStripButtonIcWebBrowser.Name = "toolStripButtonIcWebBrowser";
            this.toolStripButtonIcWebBrowser.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.toolStripButtonIcWebBrowser.Size = new System.Drawing.Size(178, 46);
            this.toolStripButtonIcWebBrowser.Text = "icWebブラウザ";
            this.toolStripButtonIcWebBrowser.Click += new System.EventHandler(this.toolStripButtonIcWebBrowser_Click);
            // 
            // icPowerAppsTEST
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1267, 781);
            this.Controls.Add(this.tableLayoutPanelBase);
            this.Controls.Add(this.toolStripFunction);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "icPowerAppsTEST";
            this.Text = "icPowerAppsTEST";
            this.Load += new System.EventHandler(this.icPowerAppsTEST_Load);
            this.toolStripFunction.ResumeLayout(false);
            this.toolStripFunction.PerformLayout();
            this.tableLayoutPanelBase.ResumeLayout(false);
            this.tableLayoutPanelBase.PerformLayout();
            this.tableLayoutPanelTitle.ResumeLayout(false);
            this.tableLayoutPanelTitle.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripFunction;
        private System.Windows.Forms.ToolStripButton toolStripButtonIcWebBrowser;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBase;
        private System.Windows.Forms.Panel panelFunction;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabelTop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTitle;
    }
}