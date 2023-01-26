namespace icAPIAddinEnableDisable
{
    partial class Form_icAPIAddinEnableDisable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_icAPIAddinEnableDisable));
            this.tableLayoutPanelBase = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxGUID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxAddinName = new System.Windows.Forms.TextBox();
            this.buttonGetCurrentAddinStatus = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxAddinDescription = new System.Windows.Forms.TextBox();
            this.groupBoxCurrentAddinStatus = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonSetAddin = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.checkedListBoxUseIRONCAD = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanelBase.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBoxCurrentAddinStatus.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelBase
            // 
            this.tableLayoutPanelBase.AutoSize = true;
            this.tableLayoutPanelBase.ColumnCount = 2;
            this.tableLayoutPanelBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBase.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanelBase.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanelBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBase.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelBase.Name = "tableLayoutPanelBase";
            this.tableLayoutPanelBase.Padding = new System.Windows.Forms.Padding(10);
            this.tableLayoutPanelBase.RowCount = 2;
            this.tableLayoutPanelBase.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBase.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBase.Size = new System.Drawing.Size(686, 441);
            this.tableLayoutPanelBase.TabIndex = 45;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBoxCurrentAddinStatus, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(13, 28);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(630, 400);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.textBoxGUID, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.textBoxAddinName, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.buttonGetCurrentAddinStatus, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.textBoxAddinDescription, 1, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 4;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(624, 126);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // textBoxGUID
            // 
            this.textBoxGUID.Location = new System.Drawing.Point(97, 61);
            this.textBoxGUID.Name = "textBoxGUID";
            this.textBoxGUID.Size = new System.Drawing.Size(478, 23);
            this.textBoxGUID.TabIndex = 2;
            this.textBoxGUID.TextChanged += new System.EventHandler(this.textBoxGUID_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(35, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 15);
            this.label1.TabIndex = 47;
            this.label1.Text = "アドイン名";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(5, 65);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 15);
            this.label2.TabIndex = 48;
            this.label2.Text = "アドインのGUID";
            // 
            // textBoxAddinName
            // 
            this.textBoxAddinName.Location = new System.Drawing.Point(97, 3);
            this.textBoxAddinName.Name = "textBoxAddinName";
            this.textBoxAddinName.Size = new System.Drawing.Size(478, 23);
            this.textBoxAddinName.TabIndex = 0;
            // 
            // buttonGetCurrentAddinStatus
            // 
            this.buttonGetCurrentAddinStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGetCurrentAddinStatus.AutoSize = true;
            this.buttonGetCurrentAddinStatus.Location = new System.Drawing.Point(382, 90);
            this.buttonGetCurrentAddinStatus.Name = "buttonGetCurrentAddinStatus";
            this.buttonGetCurrentAddinStatus.Size = new System.Drawing.Size(193, 33);
            this.buttonGetCurrentAddinStatus.TabIndex = 3;
            this.buttonGetCurrentAddinStatus.Text = "現在の設定状態を取得";
            this.buttonGetCurrentAddinStatus.UseVisualStyleBackColor = true;
            this.buttonGetCurrentAddinStatus.Click += new System.EventHandler(this.buttonGetCurrentAddinStatus_Click);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(13, 36);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 15);
            this.label4.TabIndex = 52;
            this.label4.Text = "アドインの説明";
            // 
            // textBoxAddinDescription
            // 
            this.textBoxAddinDescription.Location = new System.Drawing.Point(97, 32);
            this.textBoxAddinDescription.Name = "textBoxAddinDescription";
            this.textBoxAddinDescription.Size = new System.Drawing.Size(478, 23);
            this.textBoxAddinDescription.TabIndex = 1;
            // 
            // groupBoxCurrentAddinStatus
            // 
            this.groupBoxCurrentAddinStatus.AutoSize = true;
            this.groupBoxCurrentAddinStatus.Controls.Add(this.tableLayoutPanel3);
            this.groupBoxCurrentAddinStatus.Enabled = false;
            this.groupBoxCurrentAddinStatus.Location = new System.Drawing.Point(3, 135);
            this.groupBoxCurrentAddinStatus.Name = "groupBoxCurrentAddinStatus";
            this.groupBoxCurrentAddinStatus.Padding = new System.Windows.Forms.Padding(10);
            this.groupBoxCurrentAddinStatus.Size = new System.Drawing.Size(624, 240);
            this.groupBoxCurrentAddinStatus.TabIndex = 2;
            this.groupBoxCurrentAddinStatus.TabStop = false;
            this.groupBoxCurrentAddinStatus.Text = "アドインの設定状態";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonSetAddin, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.label7, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.checkedListBoxUseIRONCAD, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(10, 26);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(604, 204);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(5, 0);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(141, 15);
            this.label6.TabIndex = 46;
            this.label6.Text = "IRONCADのアドインに表示";
            // 
            // buttonSetAddin
            // 
            this.buttonSetAddin.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonSetAddin.Location = new System.Drawing.Point(519, 5);
            this.buttonSetAddin.Margin = new System.Windows.Forms.Padding(5);
            this.buttonSetAddin.Name = "buttonSetAddin";
            this.buttonSetAddin.Size = new System.Drawing.Size(80, 35);
            this.buttonSetAddin.TabIndex = 1;
            this.buttonSetAddin.Text = "適用";
            this.buttonSetAddin.UseVisualStyleBackColor = true;
            this.buttonSetAddin.Click += new System.EventHandler(this.buttonSetAddin_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Meiryo UI", 8.25F);
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(156, 176);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(207, 28);
            this.label7.TabIndex = 48;
            this.label7.Text = "※別途 IRONCADのアドイン有効化画面から\r\n   有効化する必要があります。";
            // 
            // checkedListBoxUseIRONCAD
            // 
            this.checkedListBoxUseIRONCAD.CheckOnClick = true;
            this.checkedListBoxUseIRONCAD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxUseIRONCAD.FormattingEnabled = true;
            this.checkedListBoxUseIRONCAD.Location = new System.Drawing.Point(156, 5);
            this.checkedListBoxUseIRONCAD.Margin = new System.Windows.Forms.Padding(5);
            this.checkedListBoxUseIRONCAD.Name = "checkedListBoxUseIRONCAD";
            this.checkedListBoxUseIRONCAD.Size = new System.Drawing.Size(353, 166);
            this.checkedListBoxUseIRONCAD.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "IRONCADへアドインを設定";
            // 
            // Form_icAPIAddinEnableDisable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 441);
            this.Controls.Add(this.tableLayoutPanelBase);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form_icAPIAddinEnableDisable";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "icAPIAddinEnableDisable";
            this.Shown += new System.EventHandler(this.Form_icAPIAddinEnableDisable_Shown);
            this.tableLayoutPanelBase.ResumeLayout(false);
            this.tableLayoutPanelBase.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.groupBoxCurrentAddinStatus.ResumeLayout(false);
            this.groupBoxCurrentAddinStatus.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBase;
        private System.Windows.Forms.Button buttonSetAddin;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckedListBox checkedListBoxUseIRONCAD;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TextBox textBoxGUID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxAddinName;
        private System.Windows.Forms.Button buttonGetCurrentAddinStatus;
        private System.Windows.Forms.GroupBox groupBoxCurrentAddinStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxAddinDescription;
    }
}