using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using interop.ICApiIronCAD;
using System.Diagnostics;
using System.Reflection;

namespace ICApiAddin.icPowerApps
{
    [ComVisible(false)]
    public partial class icPowerAppsTEST : Form
    {
        private IZBaseApp _ironcadApp;

        /// <summary>
        /// コンストラクタ(スタンドアロン起動時)
        /// </summary>
        public icPowerAppsTEST()
        {
            InitializeComponent();
            this._ironcadApp = null;
        }

        /// <summary>
        /// コンストラクタ(IRONCADアドインから起動時)
        /// </summary>
        /// <param name="baseApp"></param>
        public icPowerAppsTEST(IZBaseApp baseApp)
        {
            InitializeComponent();
            this._ironcadApp = baseApp;
        }


        /// <summary>
        /// Formのロード イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void icPowerAppsTEST_Load(object sender, EventArgs e)
        {
            FileVersionInfo ver = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            this.labelTitle.Text = "左メニューから機能を選択してください。";
            this.Text = string.Format("{0}  version: {1}", this.Text, ver.FileVersion);
        }


        /// <summary>
        /// ユーザーコントロールを表示する
        /// </summary>
        /// <param name="userControl"></param>
        /// <param name="title"></param>
        public void showUserControl(UserControl userControl, string title)
        {
            bool closeResult = closeUserControl();
            if(closeResult == false)
            {
                MessageBox.Show("現在処理中のため他の機能へ移動できません。しばらくお待ちください。");
                return;
            }
            panelFunction.Controls.Add(userControl);
            userControl.Visible = false;
            userControl.Show();
            userControl.BringToFront();
            labelTitle.Text = title;
            userControl.Width = panelFunction.Width;
            userControl.Height = panelFunction.Height;
        }

        /// <summary>
        /// ユーザーコントロールを閉じる
        /// </summary>
        /// <returns></returns>
        public bool closeUserControl()
        {
            bool closeResult = true;
            foreach (Control c in panelFunction.Controls)
            {
                UserControl userControl = c as UserControl;
                if(userControl == null)
                {
                    continue;
                }
                /* タグデータがある場合は処理中かどうかチェックする */
                UserControlTagData tagData = null;
                if (userControl.Tag != null)
                {
                    try
                    {
                        tagData = (UserControlTagData)userControl.Tag;
                        if(tagData.canNotClose == true)
                        {
                            /* 処理中なので閉じれない */
                            closeResult = false;
                            break;
                        }
                    }
                    catch(Exception ex)
                    {
                    }
                }
                userControl.Dispose();
                labelTitle.Text = string.Empty;
                this.panelFunction.Controls.Remove(userControl);
            }
            return closeResult;
        }


        /// <summary>
        /// 機能を選択 ラベルクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripLabelTop_Click(object sender, EventArgs e)
        {
            closeUserControl();
            this.labelTitle.Text = "左メニューから機能を選択してください。";
        }


        /// <summary>
        /// icWebブラウザ ボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonIcWebBrowser_Click(object sender, EventArgs e)
        {
            UserControlWebBrowser userControl = new UserControlWebBrowser(true, false);
            showUserControl(userControl, "Browser");
        }
        
    }
}
