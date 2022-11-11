using interop.ICApiIronCAD;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static ICApiAddin.icPowerApps.Addin;
using static ICApiAddin.icPowerApps.icPowerAppsSetting;

namespace ICApiAddin.icPowerApps
{
    public partial class Form_icPowerAppsSetting : Form
    {
        public const string title = "IRONCADの設定";
        public List<IRONCAD_REG_AND_DIR> _ironcadList = new List<IRONCAD_REG_AND_DIR>();
        public IZBaseApp IronCADApp;
        public List<AddInToolData> addInToolDataList;

        public Form_icPowerAppsSetting(IZBaseApp IronCADApp, List<AddInToolData> addInToolDataList)
        {
            InitializeComponent();
            this.IronCADApp = IronCADApp;
            this.addInToolDataList = addInToolDataList;
        }

        private void Form_icPowerAppsSetting_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            listBoxLargeIcon.DisplayMember = "dispName";
            listBoxLargeIcon.ValueMember = "uniqueName";
            listBoxSmallIcon.DisplayMember = "dispName";
            listBoxSmallIcon.ValueMember = "uniqueName";
            listBoxDisable.DisplayMember = "dispName";
            listBoxDisable .ValueMember = "uniqueName";
            foreach (AddInToolData tool in addInToolDataList)
            {
                if (tool.isEnable != true)
                {
                    listBoxDisable.Items.Add(tool);
                    continue;
                }
                if (tool.isLargeIcon == true)
                {
                    listBoxLargeIcon.Items.Add(tool);
                }
                else
                {
                    listBoxSmallIcon.Items.Add(tool);
                }
            }


            string userConfigPath = icPowerAppsSetting.GetUserConfigFilePath();
            if (File.Exists(userConfigPath) != true)
            {
                /* ユーザーコンフィグファイルが無いので作成する */
                icPowerAppsSetting.WriteicPowerAppsUserSetting(userConfigPath);
            }

            string commonConfigPath = icPowerAppsSetting.GetCommonConfigFilePath();
            if (File.Exists(commonConfigPath) != true)
            {
                /* 共通コンフィグファイルが無いので作成する */
                icPowerAppsSetting.WriteicPowerAppsCommonSetting(commonConfigPath);
            }
            icPowerAppsSetting.ReadicPowerAppsUserSetting(userConfigPath);
            icPowerAppsSetting.ReadicPowerAppsCommonSetting(commonConfigPath);
            _config = icPowerAppsSetting.GetConfig();


            this.Cursor = Cursors.Default;
        }


        /// <summary>
        /// 小さいアイコンに移動ボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSmallIcon_Click(object sender, EventArgs e)
        {
            if ((listBoxLargeIcon.SelectedItem == null) || (listBoxLargeIcon.SelectedIndex < 0))
            {
                return;
            }
            int currIndex = listBoxLargeIcon.SelectedIndex;
            AddInToolData tool = listBoxLargeIcon.SelectedItem as AddInToolData;
            listBoxLargeIcon.Items.Remove(listBoxLargeIcon.SelectedItem);
            if(currIndex >= listBoxLargeIcon.Items.Count)
            {
                currIndex = currIndex - 1;
            }
            if((currIndex >= 0) && (listBoxLargeIcon.Items.Count >= 0))
            {
                listBoxLargeIcon.SelectedIndex = currIndex;
            }
            tool.isLargeIcon = false;
            listBoxSmallIcon.Items.Add(tool);
        }

        /// <summary>
        /// 大きいアイコンに移動ボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLargeIcon_Click(object sender, EventArgs e)
        {
            if ((listBoxSmallIcon.SelectedItem == null) || (listBoxSmallIcon.SelectedIndex < 0))
            {
                return;
            }
            int currIndex = listBoxSmallIcon.SelectedIndex;
            AddInToolData tool = listBoxSmallIcon.SelectedItem as AddInToolData;
            listBoxSmallIcon.Items.Remove(listBoxSmallIcon.SelectedItem);
            if (currIndex >= listBoxSmallIcon.Items.Count)
            {
                currIndex = currIndex - 1;
            }
            if ((currIndex >= 0) && (listBoxSmallIcon.Items.Count >= 0))
            {
                listBoxSmallIcon.SelectedIndex = currIndex;
            }

            tool.isLargeIcon = true;
            listBoxLargeIcon.Items.Add(tool);
        }

        public icPowerAppsSetting.icPowerAppsConfig _config = null;
        private void buttonOK_Click(object sender, EventArgs e)
        {
            _config.UserConfig.ClientConfig.AppList.Clear();
            foreach (object obj in listBoxLargeIcon.Items)
            {
                AddInToolData toolData = obj as AddInToolData;
                _config.UserConfig.ClientConfig.AppList.Add(new AddInToolIconSize(toolData.uniqueName, toolData.dispName, toolData.isLargeIcon, toolData.isEnable));
            }
            foreach (object obj in listBoxSmallIcon.Items)
            {
                AddInToolData toolData = obj as AddInToolData;
                _config.UserConfig.ClientConfig.AppList.Add(new AddInToolIconSize(toolData.uniqueName, toolData.dispName, toolData.isLargeIcon, toolData.isEnable));
            }
            foreach (object obj in listBoxDisable.Items)
            {
                AddInToolData toolData = obj as AddInToolData;
                _config.UserConfig.ClientConfig.AppList.Add(new AddInToolIconSize(toolData.uniqueName, toolData.dispName, toolData.isLargeIcon, toolData.isEnable));
            }

            /* Configをファイル出力する */
            icPowerAppsSetting.SetCommonConfig(_config.CommonConfig);
            icPowerAppsSetting.SetUserConfig(_config.UserConfig);
            WriteicPowerAppsCommonSetting(icPowerAppsSetting.GetCommonConfigFilePath());
            WriteicPowerAppsUserSetting(icPowerAppsSetting.GetUserConfigFilePath());

            MessageBox.Show("icPowerAppsのアイコンのサイズ変更は\nアドイン設定を無効にし再度有効にする必要があります。");

            /* 閉じる */
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void buttonLargeDisable_Click(object sender, EventArgs e)
        {
            if ((listBoxLargeIcon.SelectedItem == null) || (listBoxLargeIcon.SelectedIndex < 0))
            {
                return;
            }
            int currIndex = listBoxLargeIcon.SelectedIndex;
            AddInToolData tool = listBoxLargeIcon.SelectedItem as AddInToolData;
            if(tool.uniqueName == "icPowerAppsSetting")
            {
                MessageBox.Show("設定は無効にできません。");
                return;
            }
            listBoxLargeIcon.Items.Remove(listBoxLargeIcon.SelectedItem);
            if (currIndex >= listBoxLargeIcon.Items.Count)
            {
                currIndex = currIndex - 1;
            }
            if ((currIndex >= 0) && (listBoxLargeIcon.Items.Count >= 0))
            {
                listBoxLargeIcon.SelectedIndex = currIndex;
            }

            tool.isEnable = false;
            listBoxDisable.Items.Add(tool);
        }

        private void buttonSmallDisable_Click(object sender, EventArgs e)
        {
            if ((listBoxSmallIcon.SelectedItem == null) || (listBoxSmallIcon.SelectedIndex < 0))
            {
                return;
            }
            int currIndex = listBoxSmallIcon.SelectedIndex;
            AddInToolData tool = listBoxSmallIcon.SelectedItem as AddInToolData;
            if (tool.uniqueName == "icPowerAppsSetting")
            {
                MessageBox.Show("設定は無効にできません。");
                return;
            }

            listBoxSmallIcon.Items.Remove(listBoxSmallIcon.SelectedItem);
            if (currIndex >= listBoxSmallIcon.Items.Count)
            {
                currIndex = currIndex - 1;
            }
            if ((currIndex >= 0) && (listBoxSmallIcon.Items.Count >= 0))
            {
                listBoxSmallIcon.SelectedIndex = currIndex;
            }

            tool.isEnable = false;
            listBoxDisable.Items.Add(tool);
        }

        private void buttonEnable_Click(object sender, EventArgs e)
        {
            if ((listBoxDisable.SelectedItem == null) || (listBoxDisable.SelectedIndex < 0))
            {
                return;
            }
            int currIndex = listBoxDisable.SelectedIndex;
            AddInToolData tool = listBoxDisable.SelectedItem as AddInToolData;
            listBoxDisable.Items.Remove(listBoxDisable.SelectedItem);
            if (currIndex >= listBoxDisable.Items.Count)
            {
                currIndex = currIndex - 1;
            }
            if ((currIndex >= 0) && (listBoxDisable.Items.Count >= 0))
            {
                listBoxDisable.SelectedIndex = currIndex;
            }

            tool.isEnable = true;
            if(tool.isLargeIcon == true)
            {
                listBoxLargeIcon.Items.Add(tool);
            }else
            {
                listBoxSmallIcon.Items.Add(tool);
            }
        }
    }
}
