using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace icAPIAddinEnableDisable
{
    public partial class Form_icAPIAddinEnableDisable : Form
    {
        private List<ArgsParam.AddinSettingDataSet> _addinSettingList = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Form_icAPIAddinEnableDisable()
        {
            InitializeComponent();
            Environment.ExitCode = -1;
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="addinSettingList"></param>
        public Form_icAPIAddinEnableDisable(List<ArgsParam.AddinSettingDataSet> addinSettingList)
        {
            InitializeComponent();
            this._addinSettingList = addinSettingList;
            Environment.ExitCode = -1;
        }

        /// <summary>
        /// Shownイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_icAPIAddinEnableDisable_Shown(object sender, EventArgs e)
        {
            if (this._addinSettingList != null)
            {
                Environment.ExitCode = 0;
                for (int i = 0; i < this._addinSettingList.Count(); i++)
                {
                    ArgsParam.AddinSettingDataSet item = this._addinSettingList[i];
                    try
                    {
                        if (item.isEnable == true)
                        {
                            bool ret = AddinConfig.AddConfig(item.configPath, item.guid, item.addinName, item.addinDescription);
                            if (ret == true)
                            {
                                //MessageBox.Show(string.Format("configPath:{0} にアドイン情報を追加しました。", item.configPath));
                            }
                            else
                            {
                                MessageBox.Show(string.Format("configPath:{0} へのアドイン情報追加に失敗しました。", item.configPath));
                            }
                        }
                        else
                        {
                            int tryCount = 0;
                            int deleteCount = 0;
                            /* たまにゴミが残っていることがあるので多めに削除できるようにしている */
                            do
                            {
                                tryCount++;
                                if (tryCount > 10)
                                {
                                    break;
                                }
                                deleteCount = AddinConfig.DeleteConfig(item.configPath, item.guid);
                            } while (deleteCount > 0);
                        }
                    }catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("configPath:{0} へのアドイン情報追加/削除に失敗しました。", item.configPath));
                        Environment.ExitCode = -1;
                    }
                }
                this.Close();
                return;
            }
        }

        public class ADDIN_ENABLE_MANAGEMENT
        {
            public string version { get; set; }
            public string addinConfigPath { get; set; }
            public bool isEnable { get; set; }

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

        List<ADDIN_ENABLE_MANAGEMENT> currAddinSetting = new List<ADDIN_ENABLE_MANAGEMENT>();

        private void showUseIRONCAD(string guid)
        {
            currAddinSetting.Clear();
            List<ADDIN_ENABLE_MANAGEMENT> addinSetting = new List<ADDIN_ENABLE_MANAGEMENT>();
            List<KeyValuePair<string, string>> allIroncad = new List<KeyValuePair<string, string>>();
            List<string> addinEnabledList = new List<string>();
            AddinConfig.GetAllIronCADInstallPath(ref allIroncad, true);
            for (int i = 0; i < allIroncad.Count; i++)
            {
                string version = string.Format("IRONCAD {0}", allIroncad[i].Key);
                string configPath = Path.Combine(allIroncad[i].Value, AddinConfig.ADDIN_CONFIG_FILE_PATH);
                if (File.Exists(configPath) != true)
                {
                    continue;
                }
                bool exists = AddinConfig.GetConfigIsEnable(configPath, guid);
                addinSetting.Add(new ADDIN_ENABLE_MANAGEMENT(version, configPath, exists));
                currAddinSetting.Add(new ADDIN_ENABLE_MANAGEMENT(version, configPath, exists));
                if (exists == true)
                {
                    addinEnabledList.Add(version);
                }
            }

            checkedListBoxUseIRONCAD.DataSource = addinSetting;
            checkedListBoxUseIRONCAD.DisplayMember = "version";
            checkedListBoxUseIRONCAD.ValueMember = "addinConfigPath";

            for (int i = 0; i < checkedListBoxUseIRONCAD.Items.Count; i++)
            {
                ADDIN_ENABLE_MANAGEMENT item = (ADDIN_ENABLE_MANAGEMENT)checkedListBoxUseIRONCAD.Items[i];
                checkedListBoxUseIRONCAD.SetItemChecked(i, addinEnabledList.Contains(item.version));
            }
        }

        private void buttonGetCurrentAddinStatus_Click(object sender, EventArgs e)
        {
            string sampleGUID = "6AE87CEF-C966-4938-A945-50D4280F60D8";
            string guid = textBoxGUID.Text;
            if(string.IsNullOrEmpty(guid) == true)
            {
                MessageBox.Show("アドインのGUIDが入力されていません。");
                return;
            }
            if(sampleGUID.Length != guid.Length)
            {
                MessageBox.Show("アドインのGUIDの長さが正しくありません。");
                return;
            }
            string[] sampleSplit = sampleGUID.Split('-');
            string[] guidSplit = guid.Split('-');
            if (sampleSplit.Count() != guidSplit.Count())
            {
                MessageBox.Show("アドインのGUIDの入力フォーマットが正しくありません。");
                return;
            }
            showUseIRONCAD(guid);
            groupBoxCurrentAddinStatus.Enabled = true;
        }

        private void textBoxGUID_TextChanged(object sender, EventArgs e)
        {
            groupBoxCurrentAddinStatus.Enabled = false;
        }

        private void buttonSetAddin_Click(object sender, EventArgs e)
        {
            string name = textBoxAddinName.Text;
            string guid = textBoxGUID.Text;
            string description = textBoxAddinDescription.Text;
            if (string.IsNullOrEmpty(name) == true)
            {
                MessageBox.Show("アドイン名が入力されていません。");
                return;
            }
            for (int i = 0; i < checkedListBoxUseIRONCAD.Items.Count; i++)
            {
                bool itemCheck = checkedListBoxUseIRONCAD.GetItemChecked(i);
                bool orgCheck = currAddinSetting[i].isEnable;
                if (itemCheck == orgCheck)
                {
                    continue;
                }
                ADDIN_ENABLE_MANAGEMENT item = (ADDIN_ENABLE_MANAGEMENT)checkedListBoxUseIRONCAD.Items[i];
                if (itemCheck == true)
                {
                    /* アドインの有効化 */
                    AddinConfig.AddConfig(item.addinConfigPath, guid, name, description);
                }
                else
                {
                    /* アドインの無効化 */
                    int tryCount = 0;
                    int deleteCount = 0;
                    /* たまにゴミが残っていることがあるので多めに削除できるようにしている */
                    do
                    {
                        tryCount++;
                        if (tryCount > 10)
                        {
                            break;
                        }
                        deleteCount = AddinConfig.DeleteConfig(item.addinConfigPath, guid);
                    } while (deleteCount > 0);                    
                }
            }
            MessageBox.Show("IRONCADのアドイン設定を変更しました。");
            showUseIRONCAD(guid);
        }
    }
}
