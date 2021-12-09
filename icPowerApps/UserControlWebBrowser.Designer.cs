namespace ICApiAddin.icPowerApps
{
    partial class UserControlWebBrowser
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
            this.webView2Main = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.toolStripHeader = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonBack = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPrevious = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonReload = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonHome = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBoxUrl = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButtonUrlGo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButtonFavorite = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripButtonUserGuide = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonABC = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanelBase.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webView2Main)).BeginInit();
            this.toolStripHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelBase
            // 
            this.tableLayoutPanelBase.AutoSize = true;
            this.tableLayoutPanelBase.ColumnCount = 1;
            this.tableLayoutPanelBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBase.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanelBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBase.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelBase.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanelBase.Name = "tableLayoutPanelBase";
            this.tableLayoutPanelBase.RowCount = 1;
            this.tableLayoutPanelBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBase.Size = new System.Drawing.Size(743, 653);
            this.tableLayoutPanelBase.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.webView2Main, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.toolStripHeader, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(735, 645);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // webView2Main
            // 
            this.webView2Main.CreationProperties = null;
            this.webView2Main.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView2Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webView2Main.Location = new System.Drawing.Point(4, 41);
            this.webView2Main.Margin = new System.Windows.Forms.Padding(4);
            this.webView2Main.Name = "webView2Main";
            this.webView2Main.Size = new System.Drawing.Size(727, 600);
            this.webView2Main.TabIndex = 1;
            this.webView2Main.ZoomFactor = 1D;
            // 
            // toolStripHeader
            // 
            this.toolStripHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripHeader.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStripHeader.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripHeader.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonBack,
            this.toolStripButtonPrevious,
            this.toolStripSeparator1,
            this.toolStripButtonReload,
            this.toolStripButtonHome,
            this.toolStripTextBoxUrl,
            this.toolStripButtonUrlGo,
            this.toolStripSeparator2,
            this.toolStripSplitButtonFavorite,
            this.toolStripButtonUserGuide,
            this.toolStripButtonABC});
            this.toolStripHeader.Location = new System.Drawing.Point(0, 0);
            this.toolStripHeader.Name = "toolStripHeader";
            this.toolStripHeader.Size = new System.Drawing.Size(735, 37);
            this.toolStripHeader.TabIndex = 0;
            this.toolStripHeader.Text = "toolStrip1";
            // 
            // toolStripButtonBack
            // 
            this.toolStripButtonBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonBack.Image = global::ICApiAddin.icPowerApps.Properties.Resources.icon_webBrowserBack;
            this.toolStripButtonBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonBack.Name = "toolStripButtonBack";
            this.toolStripButtonBack.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.toolStripButtonBack.Size = new System.Drawing.Size(34, 34);
            this.toolStripButtonBack.Text = "toolStripButton1";
            this.toolStripButtonBack.ToolTipText = "戻る";
            this.toolStripButtonBack.Click += new System.EventHandler(this.toolStripButtonBack_Click);
            // 
            // toolStripButtonPrevious
            // 
            this.toolStripButtonPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPrevious.Image = global::ICApiAddin.icPowerApps.Properties.Resources.icon_webBrowserForward;
            this.toolStripButtonPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPrevious.Name = "toolStripButtonPrevious";
            this.toolStripButtonPrevious.Padding = new System.Windows.Forms.Padding(5);
            this.toolStripButtonPrevious.Size = new System.Drawing.Size(34, 34);
            this.toolStripButtonPrevious.Text = "toolStripButton1";
            this.toolStripButtonPrevious.ToolTipText = "進む";
            this.toolStripButtonPrevious.Click += new System.EventHandler(this.toolStripButtonPrevious_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 37);
            // 
            // toolStripButtonReload
            // 
            this.toolStripButtonReload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonReload.Image = global::ICApiAddin.icPowerApps.Properties.Resources.icon_webBrowserReload;
            this.toolStripButtonReload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReload.Name = "toolStripButtonReload";
            this.toolStripButtonReload.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.toolStripButtonReload.Size = new System.Drawing.Size(34, 34);
            this.toolStripButtonReload.Text = "toolStripButton2";
            this.toolStripButtonReload.ToolTipText = "更新";
            this.toolStripButtonReload.Click += new System.EventHandler(this.toolStripButtonReload_Click);
            // 
            // toolStripButtonHome
            // 
            this.toolStripButtonHome.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHome.Image = global::ICApiAddin.icPowerApps.Properties.Resources.icon_webBrowserHome;
            this.toolStripButtonHome.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHome.Name = "toolStripButtonHome";
            this.toolStripButtonHome.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.toolStripButtonHome.Size = new System.Drawing.Size(34, 34);
            this.toolStripButtonHome.Text = "ホーム";
            this.toolStripButtonHome.Click += new System.EventHandler(this.toolStripButtonHome_Click);
            // 
            // toolStripTextBoxUrl
            // 
            this.toolStripTextBoxUrl.AutoSize = false;
            this.toolStripTextBoxUrl.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.toolStripTextBoxUrl.Name = "toolStripTextBoxUrl";
            this.toolStripTextBoxUrl.Size = new System.Drawing.Size(100, 27);
            this.toolStripTextBoxUrl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBoxUrl_KeyUp);
            // 
            // toolStripButtonUrlGo
            // 
            this.toolStripButtonUrlGo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUrlGo.Image = global::ICApiAddin.icPowerApps.Properties.Resources.icon_webBrowserGo;
            this.toolStripButtonUrlGo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUrlGo.Name = "toolStripButtonUrlGo";
            this.toolStripButtonUrlGo.Padding = new System.Windows.Forms.Padding(0, 3, 5, 3);
            this.toolStripButtonUrlGo.Size = new System.Drawing.Size(29, 34);
            this.toolStripButtonUrlGo.Text = "入力したURLへ移動";
            this.toolStripButtonUrlGo.ToolTipText = "入力したURLへ";
            this.toolStripButtonUrlGo.Click += new System.EventHandler(this.toolStripButtonUrlGo_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 37);
            // 
            // toolStripSplitButtonFavorite
            // 
            this.toolStripSplitButtonFavorite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButtonFavorite.Image = global::ICApiAddin.icPowerApps.Properties.Resources.icon_webBrowserBookmark;
            this.toolStripSplitButtonFavorite.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonFavorite.Name = "toolStripSplitButtonFavorite";
            this.toolStripSplitButtonFavorite.Padding = new System.Windows.Forms.Padding(3);
            this.toolStripSplitButtonFavorite.Size = new System.Drawing.Size(45, 34);
            this.toolStripSplitButtonFavorite.Text = "お気に入り";
            this.toolStripSplitButtonFavorite.ButtonClick += new System.EventHandler(this.toolStripSplitButtonFavorite_ButtonClick);
            // 
            // toolStripButtonUserGuide
            // 
            this.toolStripButtonUserGuide.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUserGuide.Image = global::ICApiAddin.icPowerApps.Properties.Resources.icon_webBrowserUserGuide;
            this.toolStripButtonUserGuide.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUserGuide.Name = "toolStripButtonUserGuide";
            this.toolStripButtonUserGuide.Padding = new System.Windows.Forms.Padding(3);
            this.toolStripButtonUserGuide.Size = new System.Drawing.Size(30, 34);
            this.toolStripButtonUserGuide.Text = "IRONCADユーザーガイド";
            this.toolStripButtonUserGuide.Click += new System.EventHandler(this.toolStripButtonUserGuide_Click);
            // 
            // toolStripButtonABC
            // 
            this.toolStripButtonABC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonABC.Image = global::ICApiAddin.icPowerApps.Properties.Resources.icon_webBrowserABC;
            this.toolStripButtonABC.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonABC.Name = "toolStripButtonABC";
            this.toolStripButtonABC.Padding = new System.Windows.Forms.Padding(3);
            this.toolStripButtonABC.Size = new System.Drawing.Size(30, 34);
            this.toolStripButtonABC.Text = "ABC for IronCAD";
            this.toolStripButtonABC.Click += new System.EventHandler(this.toolStripButtonABC_Click);
            // 
            // UserControlWebBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.tableLayoutPanelBase);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UserControlWebBrowser";
            this.Size = new System.Drawing.Size(743, 653);
            this.Load += new System.EventHandler(this.UserControlWebBrowser_Load);
            this.Resize += new System.EventHandler(this.UserControlWebBrowser_Resize);
            this.tableLayoutPanelBase.ResumeLayout(false);
            this.tableLayoutPanelBase.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webView2Main)).EndInit();
            this.toolStripHeader.ResumeLayout(false);
            this.toolStripHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBase;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStrip toolStripHeader;
        private System.Windows.Forms.ToolStripButton toolStripButtonBack;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrevious;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxUrl;
        private System.Windows.Forms.ToolStripButton toolStripButtonUrlGo;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView2Main;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonReload;
        private System.Windows.Forms.ToolStripButton toolStripButtonHome;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonFavorite;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonUserGuide;
        private System.Windows.Forms.ToolStripButton toolStripButtonABC;
    }
}
