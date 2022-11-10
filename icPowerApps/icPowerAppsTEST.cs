using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using interop.ICApiIronCAD;
using System.Diagnostics;
using System.Reflection;
using static ICApiAddin.icPowerApps.Addin;
using static ICApiAddin.icPowerApps.icPowerAppsSetting;

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
            bool closeResult = closeControl();
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
        /// Formを表示する
        /// </summary>
        /// <param name="frm"></param>
        public void showFormControl(Form frm)
        {
            bool closeResult = closeControl();
            if (closeResult == false)
            {
                MessageBox.Show("現在処理中のため他の機能へ移動できません。しばらくお待ちください。");
                return;
            }
            frm.TopLevel = false;
            panelFunction.Controls.Add(frm);
            frm.ControlBox = false;
            frm.Show();
            frm.BringToFront();
            frm.FormBorderStyle = FormBorderStyle.None;
            labelTitle.Text = frm.Text;
            frm.Width = panelFunction.Width;
            frm.Height = panelFunction.Height;
        }

        /// <summary>
        /// ユーザーコントロール/Formを閉じる
        /// </summary>
        /// <returns></returns>
        public bool closeControl()
        {
            bool closeResult = true;
            foreach (Control c in panelFunction.Controls)
            {
                UserControl userControl = c as UserControl;
                Form formControl = c as Form;
                if ((userControl == null) || (formControl == null))
                {
                    continue;
                }
                if(userControl != null)
                {
                    /* タグデータがある場合は処理中かどうかチェックする */
                    UserControlTagData tagData = null;
                    if (userControl.Tag != null)
                    {
                        try
                        {
                            tagData = (UserControlTagData)userControl.Tag;
                            if (tagData.canNotClose == true)
                            {
                                /* 処理中なので閉じれない */
                                closeResult = false;
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    userControl.Dispose();
                }
                if(formControl != null)
                {
                    formControl.Close();
                }
                labelTitle.Text = string.Empty;
                this.panelFunction.Controls.Remove(c);
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
            closeControl();
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

        private void toolStripButtonSuppressManager_Click(object sender, EventArgs e)
        {
            UserControlSuppressManager userControl = new UserControlSuppressManager(this._ironcadApp);
            showUserControl(userControl, UserControlSuppressManager.title);
        }

        private void toolStripButtonCustomPropertyManager_Click(object sender, EventArgs e)
        {
            Form_CustomPropertyManager form = new Form_CustomPropertyManager(this._ironcadApp);
            showFormControl(form);
        }

        private void toolStripButtonSceneBrowserTreeSort_Click(object sender, EventArgs e)
        {
            UserControlSceneBrowserTreeSort userControl = new UserControlSceneBrowserTreeSort(this._ironcadApp);
            showUserControl(userControl, UserControlSceneBrowserTreeSort.title);
        }

        private void toolStripButtonCatalogSort_Click(object sender, EventArgs e)
        {
            UserControlCatalogSort userControl = new UserControlCatalogSort(this._ironcadApp);
            showUserControl(userControl, UserControlCatalogSort.title);
        }

        private void toolStripButtonExternalLinkManager_Click(object sender, EventArgs e)
        {
            UserControlExternalLinkManager userControl = new UserControlExternalLinkManager(this._ironcadApp);
            showUserControl(userControl, UserControlExternalLinkManager.title);
        }

        private void toolStripButtonSettings_Click(object sender, EventArgs e)
        {
            List<AddInToolData> addInToolDataList = new List<AddInToolData>();
            string userConfigPath = icPowerAppsSetting.GetUserConfigFilePath();
            if (string.IsNullOrEmpty(userConfigPath) == true)
            {
                /* ユーザーコンフィグファイルが無いので作成する */
                icPowerAppsSetting.WriteicPowerAppsUserSetting(userConfigPath);
            }
            icPowerAppsSetting.ReadicPowerAppsUserSetting(userConfigPath);
            icPowerAppsConfig _config = icPowerAppsSetting.GetConfig();
            foreach(AddInToolIconSize tool in _config.UserConfig.ClientConfig.AppList)
            {
                addInToolDataList.Add(new AddInToolData(null, null, tool.uniqueName, tool.displayName, string.Empty, string.Empty, tool.isLargeIcon, tool.isEnable, null, null, null));
            }
            Form_icPowerAppsSetting form = new Form_icPowerAppsSetting(this._ironcadApp, addInToolDataList);
            showFormControl(form);
        }
    }
}
