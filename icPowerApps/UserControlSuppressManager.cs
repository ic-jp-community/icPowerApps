using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Management;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Diagnostics;
using AdvancedDataGridView;
using interop.ICApiIronCAD;
using System.Xml.Serialization;
using System.Xml;

namespace ICApiAddin.icPowerApps
{
    public partial class UserControlSuppressManager : UserControl
    {
        public const string title = "抑制マネージャ";
        private bool _compareId = false; /* ファイルから読込時にIDの一致まで見るか否か */
        private int _currentWorkSuppress = 1;
        private IZBaseApp _ironcadApp = null;

        #region 抑制マネージャファイルの構造定義
        /// <summary>
        /// 抑制マネージャのエクスポート/インポートファイル形式
        /// </summary>
        public class SUPPRESS_MANAGER_FILE
        {
            public List<SUPPRESS_WORK_INFO> workList;
            public SUPPRESS_MANAGER_FILE()
            {
                this.workList = new List<SUPPRESS_WORK_INFO>();
            }
        }

        /// <summary>
        /// 抑制マネージャのWork情報
        /// </summary>
        public class SUPPRESS_WORK_INFO
        {
            public string name;
            public string headerText;
            public List<ASSEMBLY_PARTS_SUPPRESS> assemblyPartsSuppress;
            public SUPPRESS_WORK_INFO()
            {
                this.name = string.Empty;
                this.assemblyPartsSuppress = new List<ASSEMBLY_PARTS_SUPPRESS>();
            }
        }

        /// <summary>
        /// 各パーツアセンブリの抑制情報
        /// </summary>
        public class ASSEMBLY_PARTS_SUPPRESS
        {
            public string userName;
            public string systemName;
            public int id;
            public bool suppress;
            public ASSEMBLY_PARTS_SUPPRESS()
            {
                this.userName = string.Empty;
                this.systemName = string.Empty;
                this.id = -1;
                this.suppress = false;
            }
            public ASSEMBLY_PARTS_SUPPRESS(string userName, string systemName, int id, bool suppress)
            {
                this.userName = userName;
                this.systemName = systemName;
                this.id = id;
                this.suppress = suppress;
            }
        }
        #endregion 抑制マネージャファイルの構造定義

        List<COMBOBOX_SUPPRESS_DATA> _suppressList = new List<COMBOBOX_SUPPRESS_DATA>();
        public class COMBOBOX_SUPPRESS_DATA
        {
            public string display { get; set; }
            public string value { get; set; }
            public COMBOBOX_SUPPRESS_DATA()
            {
                this.display = string.Empty;
                this.value = string.Empty;
            }
            public COMBOBOX_SUPPRESS_DATA(string display, string value)
            {
                this.display = display;
                this.value = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UserControlSuppressManager(IZBaseApp ironcadApp)
        {
            InitializeComponent();
            this.Tag = new UserControlTagData();
            this.Dock = DockStyle.Fill;
            this._ironcadApp = ironcadApp;
        }


        /// <summary>
        /// ページ表示状態変更イベント(ページ表示イベントとして利用)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UserControlSuppressManager_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible && !Disposing)
            {
                if(this._ironcadApp == null)
                {
                    MessageBox.Show("IRONCADと連携する機能のため、スタンドアロン起動では使用できません。");
                    return;
                }
                icapiCommon.enableDataGridViewDoubleBuffer(treeGridViewScene);
                /* 高DPI対応 */
                ScaleReziser.InitializeFormControlScale(this, true, false, true, false, true);
                ((UserControlTagData)this.Tag).canNotClose = true;
                this.Cursor = Cursors.WaitCursor;

                /* 初期化中はProgressBarを設定 */
                setProgressBar(true);
                ImageList imageList = new ImageList();
                icapiCommon.getImageListAssemblyParts(new Size(18, 18), ref imageList);
                treeGridViewScene.ImageList = imageList;

                /* 現在のシーンツリー情報を取得 */
                await getTreeGridView();
                treeGridViewScene.ShowLines = true;

                /* ノードを全て展開状態にする */
                TreeGridNodeCollection nodes = treeGridViewScene.Nodes;
                icapiCommon.ExpandTreeGridViewTreeNodes(ref nodes);

                /* 現在の状態と元の状態とWorkを作成する */
                createCurrentSuppress();
                createOriginalSuppress();
                createWorkSuppress();

                /* 列の幅をユーザーが指定可能に設定 */
                treeGridViewScene.Columns["Scene"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                for (int columnIndex = 0; columnIndex < treeGridViewScene.Columns.Count; columnIndex++)
                {
                    int columnWidth = treeGridViewScene.Columns[columnIndex].Width;
                    treeGridViewScene.Columns[columnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    treeGridViewScene.Columns[columnIndex].Width = columnWidth;
                }

                /* 状態設定用のコンボボックスを設定 */
                refreshComboboxSuppress();

                /* 初期化完了したのでProgressBarを解除 */
                setProgressBar(false);
                this.Cursor = Cursors.Default;
                ((UserControlTagData)this.Tag).canNotClose = false;
            }
        }

        /// <summary>
        /// 初期化中のProgressBarを設定する
        /// </summary>
        /// <param name="start">true:初期化中  false:初期化終了</param>
        private void setProgressBar(bool start)
        {
            if (start == true)
            {
                /* 初期化中 */
                progressBarWaitProgress.Style = ProgressBarStyle.Marquee;
                progressBarWaitProgress.Enabled = true;
                progressBarWaitProgress.Visible = true;
            }
            else
            {
                /* 初期化終了 */
                progressBarWaitProgress.Style = ProgressBarStyle.Continuous;
                progressBarWaitProgress.Enabled = false;
                progressBarWaitProgress.Visible = false;
            }
        }


        /// <summary>
        /// シーンツリーを取得する　　　
        /// </summary>
        private async Task<bool> getTreeGridView()
        {
            TreeGridView tgv = this.treeGridViewScene;
            IZDoc doc = this._ironcadApp.ActiveDoc;
            IZSceneDoc sceneDoc = doc as IZSceneDoc;
            int TreeNodeHeight = 22;

            string userName = string.Empty;
            string systemName = string.Empty;
            string elementId = string.Empty;
            int currentDepth = 0;
            TreeGridNode topNode = tgv.Nodes.Add(Path.GetFileName(doc.Name), systemName, elementId, icapiCommon.SCENE_DATATYPE_SCENE, currentDepth, doc.Name);
            topNode.ImageIndex = icapiCommon.getImageIndexAssemblyParts(icapiCommon.SCENE_DATATYPE_SCENE);
            topNode.Height = (int)(TreeNodeHeight * ScaleReziser.getScalingFactor());
            int depth = 1;
            icapiCommon.GetSceneTreeInfo(icapiCommon.CREATE_TREE_MODE.CHECK_IN, TreeNodeHeight, sceneDoc.GetTopElement(), ref topNode, ref depth);
            return true;
        }


        /// <summary>
        /// 新しい抑制状態セットを作成する(既にある場合は、現在の状態で抑制状態セットを更新する)
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="headerText">列ヘッダ名</param>
        /// <param name="readOnly">読み取り専用に設定</param>
        /// <param name="setBackColor">背景色の設定有無(true:設定あり false:設定なし)</param>
        /// <param name="backColor">背景色</param>
        private void createNewSuppressFromCurrentSuppress(string columnName, string headerText, bool readOnly, bool setBackColor, Color backColor)
        {
            /* 列が既にあるかチェック */
            if (this.treeGridViewScene.Columns.Contains(columnName) == false)
            {
                /* 列がないので新規作成する */
                DataGridViewCheckBoxLabelColumn column = new DataGridViewCheckBoxLabelColumn("抑制する");
                column.Name = columnName;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                column.MinimumWidth = 10;
                column.HeaderText = headerText;
                column.ReadOnly = readOnly;
                treeGridViewScene.Columns.Insert((treeGridViewScene.Columns.Count), column);
                int currColumnIndex = treeGridViewScene.Columns.Count - 1;
                int columnWidth = treeGridViewScene.Columns[currColumnIndex].Width;
                treeGridViewScene.Columns[currColumnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                treeGridViewScene.Columns[currColumnIndex].Width = columnWidth;
                refreshComboboxSuppress();
            }

            /* 列に現在の抑制状態を設定する */
            for (int i = 0; i < treeGridViewScene.Rows.Count; i++)
            {
                if (treeGridViewScene["Scene", i].Tag == null)
                {
                    continue;
                }
                IZElement element = treeGridViewScene["Scene", i].Tag as IZElement;
                treeGridViewScene[columnName, i].Value = element.GetStateStatus(eZElementState.Z_SUPPRESSED);
                if (setBackColor == true)
                {
                    treeGridViewScene[columnName, i].Style.BackColor = backColor;
                }
            }
        }


        /// <summary>
        /// 抑制状態[列:元の状態]を作成する
        /// </summary>
        private void createOriginalSuppress()
        {
            createNewSuppressFromCurrentSuppress("OriginalSuppress", "元の状態", true, true, Color.Gainsboro);
        }


        /// <summary>
        /// 抑制状態[列:現在の状態]を作成する
        /// </summary>
        private void createCurrentSuppress()
        {
            createNewSuppressFromCurrentSuppress("CurrentSuppress", "現在の状態", true, true, Color.Gainsboro);
        }


        /// <summary>
        /// 抑制状態[列:WorkXX]を作成する
        /// </summary>
        private void createWorkSuppress()
        {
            string name = "Work" + _currentWorkSuppress.ToString();
            Color dummyColor = Color.Transparent;
            createNewSuppressFromCurrentSuppress(name, name, false, false, dummyColor);
            _currentWorkSuppress++;
        }


        /// <summary>
        /// コンボボックスを更新する
        /// </summary>
        private void refreshComboboxSuppress()
        {
            bool exists = this.treeGridViewScene.Columns.Contains("CurrentSuppress");
            if(exists != true)
            {
                MessageBox.Show("[内部エラー] 現在の状態の列[CurrentSuppress]が取得できませんでした。");
                return;
            }

            /* コンボボックスのデータを更新する */
            this._suppressList.Clear();
            int currentSuppressColumnIndex = this.treeGridViewScene.Columns["CurrentSuppress"].Index;
            for (int i = (currentSuppressColumnIndex+1); i < treeGridViewScene.Columns.Count; i++)
            {
                string name = this.treeGridViewScene.Columns[i].Name;
                string headerText = this.treeGridViewScene.Columns[i].HeaderText;
                this._suppressList.Add(new COMBOBOX_SUPPRESS_DATA(headerText, name));
            }

            comboBoxSuppress.DataSource = null;
            comboBoxSuppress.DataSource = this._suppressList;
            comboBoxSuppress.DisplayMember = "display";
            comboBoxSuppress.ValueMember = "value";
        }


        /// <summary>
        /// 抑制情報のファイルを出力する
        /// </summary>
        /// <param name="filePath">出力ファイルパス</param>
        /// <param name="config">設定データ</param>
        public static void WriteSuppressSetting(string filePath, SUPPRESS_MANAGER_FILE suppressMgr)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (Directory.Exists(dir) != true)
            {
                Directory.CreateDirectory(dir);
            }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SUPPRESS_MANAGER_FILE));
            using (var streamWriter = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // シリアライズする
                xmlSerializer.Serialize(streamWriter, suppressMgr);
                streamWriter.Flush();
            }
        }


        /// <summary>
        /// 抑制情報のファイルを読み込む
        /// </summary>
        /// <param name="filePath">設定ファイルのパス</param>
        /// <param name="config">読み込んだ設定データ</param>
        public static bool ReadSuppressSetting(string filePath, ref SUPPRESS_MANAGER_FILE suppressMgr, bool showErrorMessage)
        {
            bool readResult = false;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SUPPRESS_MANAGER_FILE));
            XmlReaderSettings xmlSettings = new XmlReaderSettings() { CheckCharacters = false };

            StreamReader streamReader = null;
            try
            {
                if (File.Exists(filePath) == false)
                {
                    suppressMgr = new SUPPRESS_MANAGER_FILE();
                    return false;
                }
                // ファイルを読み込む
                streamReader = new StreamReader(filePath, Encoding.UTF8);
                // デシリアライズする
                XmlReader xmlReader = XmlReader.Create(streamReader, xmlSettings);
                suppressMgr = (SUPPRESS_MANAGER_FILE)xmlSerializer.Deserialize(xmlReader);
                readResult = true;
            }
            catch (Exception ex)
            {
                suppressMgr = new SUPPRESS_MANAGER_FILE();
                readResult = false;
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
            }
            return readResult;
        }


        /// <summary>
        /// 抑制状態セットをTreeGridViewに読み込む
        /// </summary>
        /// <param name="suppressMgr"></param>
        private void loadSuppressInfo(SUPPRESS_MANAGER_FILE suppressMgr)
        {
            /* 既にある抑制状態セット(WorkXX)を削除する */
            if (treeGridViewScene.Columns.Contains("Work1") == true)
            {
                List<string> deleteList = new List<string>();
                int startIndex = treeGridViewScene.Columns["Work1"].Index;
                for (int columnIndex = startIndex; columnIndex < treeGridViewScene.Columns.Count; columnIndex++)
                {
                    string name = treeGridViewScene.Columns[columnIndex].Name;
                    if (name.Contains("Work") != true)
                    {
                        continue;
                    }
                    deleteList.Add(name);
                }
                foreach (string columnName in deleteList)
                {
                    treeGridViewScene.Columns.Remove(columnName);
                }
            }

            /* 抑制状態セットをTreeGridViewに設定する */
            for (int i = 0; i < suppressMgr.workList.Count; i++)
            {
                /* 列を作成 */
                SUPPRESS_WORK_INFO work = suppressMgr.workList[i];
                Color dummyColor = Color.Transparent;
                createNewSuppressFromCurrentSuppress(work.name, work.headerText, false, false, dummyColor);

                /* 作成した列の抑制状態を設定する */
                int workColumnIndex = treeGridViewScene.Columns[work.name].Index;
                for (int rowIndex = 0; rowIndex < treeGridViewScene.Rows.Count; rowIndex++)
                {
                    if (treeGridViewScene["Scene", rowIndex].Tag == null)
                    {
                        continue;
                    }
                    /* 当該行のelementを取得 */
                    IZElement element = treeGridViewScene["Scene", rowIndex].Tag as IZElement;

                    /* elementの抑制状態を抑制状態セットから取得する */
                    IEnumerable<ASSEMBLY_PARTS_SUPPRESS> assemblyPartsSuppress = null;
                    if (this._compareId == true)
                    {
                        /* IDも条件に加えて抑制状態を取得する */
                        assemblyPartsSuppress = work.assemblyPartsSuppress.Where(a => string.Equals(a.userName, element.Name) && string.Equals(a.systemName, element.SystemName) && string.Equals(a.id, element.Id));
                    }
                    else
                    {
                        /* IDも条件に加えて抑制状態を取得する */
                        assemblyPartsSuppress = work.assemblyPartsSuppress.Where(a => string.Equals(a.userName, element.Name) && string.Equals(a.systemName, element.SystemName));
                    }

                    /* 抑制状態の有無をチェック */
                    if (assemblyPartsSuppress.Count() <= 0)
                    {
                        /* ないので次の行へ */
                        continue;
                    }

                    /* 抑制状態を設定 */
                    treeGridViewScene[workColumnIndex, rowIndex].Value = assemblyPartsSuppress.ToList()[0].suppress;
                }
            }

            /* WorkXXの最大番号を取得し設定する */
            int maxWorkNumber = 1;
            for (int i = 0; i < suppressMgr.workList.Count; i++)
            {
                SUPPRESS_WORK_INFO work = suppressMgr.workList[i];
                string nameNumber = work.name.Replace("Work", string.Empty);
                int number = -1;
                bool parse = Int32.TryParse(nameNumber, out number);
                if (parse != true)
                {
                    continue;
                }
                if (number > maxWorkNumber)
                {
                    maxWorkNumber = number;
                }
            }
            this._currentWorkSuppress = maxWorkNumber;
        }

        #region イベント
        /// <summary>
        /// 現在の抑制状態の取得ボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGetCurrentSuppress_Click(object sender, EventArgs e)
        {
            createCurrentSuppress();
        }


        /// <summary>
        /// 新しい状態セットを作成ボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNewSuppress_Click(object sender, EventArgs e)
        {
            createWorkSuppress();
        }


        /// <summary>
        /// ファイルに保存ボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExportFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = string.Empty;
            sfd.Filter = "xmlファイル(*.xml)|*.xml|すべてのファイル(*.*)|*.*";
            if (sfd.ShowDialog() != DialogResult.OK)
            {
                /* 出力キャンセル */
                return;
            }
            string saveFilePath = sfd.FileName;
            SUPPRESS_MANAGER_FILE suppressMgr = new SUPPRESS_MANAGER_FILE();
            if (treeGridViewScene.Columns.Contains("Work1") != true)
            {
                return;
            }
            int startColumnIndex = treeGridViewScene.Columns["Work1"].Index;
            for (int columnIndex = startColumnIndex; columnIndex < treeGridViewScene.Columns.Count; columnIndex++)
            {
                SUPPRESS_WORK_INFO work = new SUPPRESS_WORK_INFO();
                work.name = treeGridViewScene.Columns[columnIndex].Name;
                work.headerText = treeGridViewScene.Columns[columnIndex].HeaderText;
                for (int i = 0; i < treeGridViewScene.Rows.Count; i++)
                {
                    ASSEMBLY_PARTS_SUPPRESS suppressData = new ASSEMBLY_PARTS_SUPPRESS();
                    if (treeGridViewScene["Scene", i].Tag == null)
                    {
                        continue;
                    }
                    IZElement element = treeGridViewScene["Scene", i].Tag as IZElement;
                    bool setSuppress = (bool)treeGridViewScene[columnIndex, i].Value;
                    suppressData.userName = element.Name;
                    suppressData.systemName = element.SystemName;
                    suppressData.id = element.Id;
                    suppressData.suppress = setSuppress;
                    work.assemblyPartsSuppress.Add(suppressData);
                }
                if (work.assemblyPartsSuppress.Count() <= 0)
                {
                    continue;
                }
                suppressMgr.workList.Add(work);
            }

            WriteSuppressSetting(saveFilePath, suppressMgr);
            MessageBox.Show("抑制Work情報を保存しました。");
        }


        /// <summary>
        /// ファイルから読込ボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonImportFile_Click(object sender, EventArgs e)
        {
            SUPPRESS_MANAGER_FILE suppressMgr = new SUPPRESS_MANAGER_FILE();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.RestoreDirectory = true;
            DialogResult dret = ofd.ShowDialog();
            if (dret == DialogResult.OK)
            {
                bool result = ReadSuppressSetting(ofd.FileName, ref suppressMgr, true);
                if (result != true)
                {
                    MessageBox.Show("読み込みに失敗しました。");
                    return;
                }
                loadSuppressInfo(suppressMgr);
                MessageBox.Show("抑制Work情報を読み込ました。");
            }
        }


        /// <summary>
        /// 列の削除ボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDeleteSuppress_Click(object sender, EventArgs e)
        {
            /* 選択している列を取得 */
            DataGridViewColumn selectedColumn = null;
            if ((treeGridViewScene.SelectedColumns.Count <= 0) &&
                (treeGridViewScene.SelectedCells.Count <= 0))
            {
                return;
            }

            if (treeGridViewScene.SelectedColumns.Count > 0)
            {
                /* 列を取得する(列選択ありなので列選択から取得) */
                DataGridViewSelectedColumnCollection columns = treeGridViewScene.SelectedColumns;
                if (columns[0].Name.Contains("Work") != true)
                {
                    MessageBox.Show("選択している列は削除できません");
                    return;
                }
                selectedColumn = columns[0];
            }
            else
            {
                /* 列を取得する(列選択なしなので現在のセルから取得) */
                if (treeGridViewScene.CurrentCell == null)
                {
                    return;
                }
                int columnIndex = treeGridViewScene.CurrentCell.ColumnIndex;
                if (treeGridViewScene.Columns[columnIndex].Name.Contains("Work") != true)
                {
                    MessageBox.Show("選択している列は削除できません");
                    return;
                }
                selectedColumn = treeGridViewScene.Columns[columnIndex];
            }

            /* 列を削除してよいか確認する */
            string columnName = selectedColumn.HeaderText;
            DialogResult dret = MessageBox.Show(string.Format("{0}を削除しますか？", columnName), "確認", MessageBoxButtons.YesNo);
            if (dret != DialogResult.Yes)
            {
                return;
            }

            /* 列削除 */
            treeGridViewScene.Columns.Remove(selectedColumn);

            /* コンボボックスを更新 */
            refreshComboboxSuppress();
        }


        /// <summary>
        /// 設定ボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSetSuppress_Click(object sender, EventArgs e)
        {
            string columnName = comboBoxSuppress.SelectedValue as string;
            if (string.IsNullOrEmpty(columnName) == true)
            {
                return;
            }
            if (treeGridViewScene.Columns.Contains(columnName) != true)
            {
                return;
            }
            for (int i = 0; i < treeGridViewScene.Rows.Count; i++)
            {
                if (treeGridViewScene["Scene", i].Tag == null)
                {
                    continue;
                }
                IZElement element = treeGridViewScene["Scene", i].Tag as IZElement;
                bool setSuppress = (bool)treeGridViewScene[columnName, i].Value;
                element.SetSuppressState(setSuppress);
            }
            createCurrentSuppress();
        }


        /// <summary>
        /// 列ヘッダのダブルクリックイベント(列ヘッダ名の変更)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeGridViewScene_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string name = treeGridViewScene.Columns[e.ColumnIndex].Name;
            if (name.Contains("Work") != true)
            {
                MessageBox.Show("選択した列のヘッダ名は変更できません。(変更できる列データ:Work**に該当しません)");
                return;
            }

            UserControlSuppressManager_ChangeHeaderNameForm frm_ChangeHeaderText = new UserControlSuppressManager_ChangeHeaderNameForm(treeGridViewScene.Columns[e.ColumnIndex].HeaderText);
            frm_ChangeHeaderText.ShowDialog();
            if (frm_ChangeHeaderText._returnIsChange == true)
            {
                treeGridViewScene.Columns[e.ColumnIndex].HeaderText = frm_ChangeHeaderText._returnChangeHeaderText;
                refreshComboboxSuppress();
            }
        }

        #endregion イベント
    }
}
