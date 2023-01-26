using icAPIAddinEnableDisable;
using interop.ICApiIronCAD;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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

        private List<ADDIN_ENABLE_MANAGEMENT> _currAddinSetting = new List<ADDIN_ENABLE_MANAGEMENT>();

        /// <summary>
        /// アドイン有効/無効データ
        /// </summary>
        private class ADDIN_ENABLE_MANAGEMENT
        {
            public string version { get; set; }         /* IRONCADバージョン */
            public string addinConfigPath { get; set; } /* .configのパス */
            public bool isEnable { get; set; }          /* アドインの有効無効 true:有効  false:無効 */

            public ADDIN_ENABLE_MANAGEMENT()
            {
                this.version = string.Empty;
                this.addinConfigPath = string.Empty;
                this.isEnable = false;
            }

            public ADDIN_ENABLE_MANAGEMENT(string version, string configPath, bool enable)
            {
                this.version = version;
                this.addinConfigPath = configPath;
                this.isEnable = enable;
            }
        }
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

            /* アドインの状態を表示する */
            showUseIRONCAD();

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
            /* アドインの設定への表示設定 */
            bool ret = setAddinEnable();
            if (ret == false)
            {
                MessageBox.Show("アドインの設定変更に失敗しました。再度実行してください。");
                return;
            }

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

        /// <summary>
        /// アドイン有効化
        /// </summary>
        /// <returns></returns>
        private bool setAddinEnable()
        {
            List<ADDIN_ENABLE_MANAGEMENT> addinChangeList = new List<ADDIN_ENABLE_MANAGEMENT>();

            /* 現在の設定とアドインの有効化設定に差があるものを抽出する */
            for (int i = 0; i < checkedListBoxUseIRONCAD.Items.Count; i++)
            {
                bool itemCheck = checkedListBoxUseIRONCAD.GetItemChecked(i);
                bool orgCheck = _currAddinSetting[i].isEnable;

                /* 現在の設定に変更があるか */
                if (itemCheck == orgCheck)
                {
                    /* 変更なし */
                    continue;
                }

                /* 設定に変更あり */
                ADDIN_ENABLE_MANAGEMENT item = (ADDIN_ENABLE_MANAGEMENT)checkedListBoxUseIRONCAD.Items[i];
                item.isEnable = itemCheck;
                addinChangeList.Add(item);
            }

            /* 設定に変更があるもののみ変更を行う */
            if (addinChangeList.Count > 0)
            {
                string args = string.Empty;

                foreach (ADDIN_ENABLE_MANAGEMENT item in addinChangeList)
                {
                    string mainCommand = string.Empty;
                    if (item.isEnable == true)
                    {
                        mainCommand = "/AddinEnable";
                    }
                    else
                    {
                        mainCommand = "/AddinDisable";
                    }
                    args += string.Format("{0} \"{1}\" \"{2}\" \"{3}\" \"{4}\" ", mainCommand, item.addinConfigPath, ADDIN_GUID, ADDIN_APP_NAME, ADDIN_APP_DESCRIPTION);
                }
                ProcessStartInfo psInfo = new ProcessStartInfo();
                psInfo.Arguments = args;
                psInfo.FileName = Path.Combine(icapiCommon.GetIcApiDllPath(), "icAPIAddinEnableDisable.exe");
                Process proc = Process.Start(psInfo);

                //プロセス終了を待つ
                proc.WaitForExit();

                /* 表示を更新する */
                showUseIRONCAD();

                int errorCount = 0;
                for (int i = 0; i < addinChangeList.Count; i++)
                {
                    ADDIN_ENABLE_MANAGEMENT setItem = addinChangeList[i];
                    string setPath = setItem.addinConfigPath;
                    bool setIsEnable = setItem.isEnable;

                    foreach (ADDIN_ENABLE_MANAGEMENT currItem in _currAddinSetting)
                    {
                        if (string.Equals(currItem.addinConfigPath, setPath) != true)
                        {
                            continue;
                        }
                        if (currItem.isEnable == setIsEnable)
                        {
                            /* 設定に成功しているので問題なし */
                        }
                        else
                        {
                            errorCount++;
                        }
                        break;
                    }
                }

                /* 設定でエラーしたかチェックする */
                if (errorCount != 0)
                {
                    /* いずれかの設定でエラーが発生した */
                    return false;
                }

                MessageBox.Show("アドインの設定を変更しました。");
            }
            return true;
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

        /// <summary>
        /// 現在のアドインの有効状態を取得し表示する
        /// </summary>
        private void showUseIRONCAD()
        {
            _currAddinSetting.Clear();
            List<ADDIN_ENABLE_MANAGEMENT> addinSetting = new List<ADDIN_ENABLE_MANAGEMENT>();
            List<KeyValuePair<string, string>> allIroncad = new List<KeyValuePair<string, string>>();
            List<string> addinEnabledList = new List<string>();
            AddinConfig.GetAllIronCADInstallPath(ref allIroncad, true);

            /* 現在のIRONCADバージョン別のアドイン有効状態を取得する */
            for (int i = 0; i < allIroncad.Count; i++)
            {
                string version = string.Format("IRONCAD {0}", allIroncad[i].Key);
                string configPath = Path.Combine(allIroncad[i].Value, AddinConfig.ADDIN_CONFIG_FILE_PATH);
                if (File.Exists(configPath) != true)
                {
                    /* configファイルなし */
                    continue;
                }
                /* コンフィグファイルから当該GUIDの設定有無を取得する */
                bool isEnable = AddinConfig.GetConfigIsEnable(configPath, ADDIN_GUID);

                /* 取得した設定状態をリストに追加 */
                addinSetting.Add(new ADDIN_ENABLE_MANAGEMENT(version, configPath, isEnable));
                _currAddinSetting.Add(new ADDIN_ENABLE_MANAGEMENT(version, configPath, isEnable));
                if (isEnable == true)
                {
                    /* 有効化しているデータはさらに別リストに追加 */
                    addinEnabledList.Add(version);
                }
            }

            checkedListBoxUseIRONCAD.DataSource = addinSetting;
            checkedListBoxUseIRONCAD.DisplayMember = "version";
            checkedListBoxUseIRONCAD.ValueMember = "addinConfigPath";

            /* 有効化しているデータにチェックを付ける */
            for (int i = 0; i < checkedListBoxUseIRONCAD.Items.Count; i++)
            {
                ADDIN_ENABLE_MANAGEMENT item = (ADDIN_ENABLE_MANAGEMENT)checkedListBoxUseIRONCAD.Items[i];
                checkedListBoxUseIRONCAD.SetItemChecked(i, addinEnabledList.Contains(item.version));
            }
        }

    }
}
