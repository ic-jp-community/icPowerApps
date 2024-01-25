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
    public partial class UserControlElementManager : UserControl
    {
        public const string title = "Elementマネージャ";
        private IZBaseApp _ironcadApp = null;
        private IZDoc _doc = null;
        private IZSceneDoc _sceneDoc = null;
        IZSceneDocProperty _docProp = null;
        private bool _isInitializing = false;
        private bool _isModify = false;
        private string _currFunctionName = string.Empty;


        List<COMBOBOX_ELEMENT_ANCHOR> _comboboxAnchorList = new List<COMBOBOX_ELEMENT_ANCHOR>();
        List<COMBOBOX_ELEMENT_ANCHOR> _comboboxAllSetAnchorList = new List<COMBOBOX_ELEMENT_ANCHOR>();
        List<COMBOBOX_ON_OFF> _comboboxAssemblyUnderShowOnOffList = new List<COMBOBOX_ON_OFF>();
        List<COMBOBOX_ON_OFF> _comboboxAssemblyUnderShowAllSetOnOffList = new List<COMBOBOX_ON_OFF>();
        List<COMBOBOX_ON_OFF> _comboboxPartUnderShowOnOffList = new List<COMBOBOX_ON_OFF>();
        List<COMBOBOX_ON_OFF> _comboboxPartUnderShowAllSetOnOffList = new List<COMBOBOX_ON_OFF>();
        /// <summary>
        /// 各パーツアセンブリのAnchor設定情報
        /// </summary>
        public class COMBOBOX_ELEMENT_ANCHOR
        {
            public string displayName { get; set; }
            public eZAnchorBehavior value { get; set; }
            public COMBOBOX_ELEMENT_ANCHOR()
            {
                this.displayName = string.Empty;
                this.value = eZAnchorBehavior.Z_ANCHOR_BEHAVIOR_MOVE_FREELY;
            }
            public COMBOBOX_ELEMENT_ANCHOR(string displayName, eZAnchorBehavior anchor)
            {
                this.displayName = displayName;
                this.value = anchor;
            }
        }
        /// <summary>
        /// ONOFF設定情報
        /// </summary>
        public class COMBOBOX_ON_OFF
        {
            public string displayName { get; set; }
            public bool value { get; set; }
            public COMBOBOX_ON_OFF()
            {
                this.displayName = string.Empty;
                this.value = false;
            }
            public COMBOBOX_ON_OFF(string displayName, bool boolValue)
            {
                this.displayName = displayName;
                this.value = boolValue;
            }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UserControlElementManager(IZBaseApp ironcadApp)
        {
            InitializeComponent();
            this.Tag = new UserControlTagData();
            this.Dock = DockStyle.Fill;
            this._ironcadApp = ironcadApp;
            this._doc = this._ironcadApp.ActiveDoc;
            this._sceneDoc = this._doc as IZSceneDoc;
            this._docProp = this._sceneDoc as IZSceneDocProperty;
        }

        public static double GetMassUnitScale(int massUnit)
        {
            double scale = 1.0;
            switch (massUnit)
            {
                case (int)eZMassUnit.Z_GRAM:
                    scale = 0.001;
                    break;
                case (int)eZMassUnit.Z_KILOGRAM:
                    scale = 1.0;
                    break;
                case (int)eZMassUnit.Z_OUNCE:
                    scale = 0.028349527;
                    break;
                case (int)eZMassUnit.Z_POUND:
                    scale = 0.453592432;
                    break;
                default:
                    break;
            }
            return scale;
        }


        /// <summary>
        /// 重量の単位フォーマットを文字列に変換する
        /// </summary>
        /// <param name="massUnit"></param>
        /// <returns></returns>
        public static string convertMassUnitToString(int massUnit)
        {
            string convStr = string.Empty;
            switch (massUnit)
            {
                case (int)eZMassUnit.Z_GRAM:
                    convStr = "g";
                    break;
                case (int)eZMassUnit.Z_KILOGRAM:
                    convStr = "kg";
                    break;
                case (int)eZMassUnit.Z_OUNCE:
                    convStr = "oz";
                    break;
                case (int)eZMassUnit.Z_POUND:
                    convStr = "lb";
                    break;
                default:
                    break;
            }
            return convStr;
        }

        /// <summary>
        /// ComboBoxに表示するデータを作成する
        /// </summary>
        private void CreateComboboxDataSet()
        {
            /* 各項目機能  */
            comboBoxFunctionType.Items.Add("Anchor");
            comboBoxFunctionType.Items.Add("アセンブリ配下の選択を防止する");
            comboBoxFunctionType.Items.Add("パーツ配下の要素を表示しない");
            comboBoxFunctionType.Items.Add("[表示のみ] 計算重量/指定重量");

            /* Anchor */
            _comboboxAnchorList.Add(new COMBOBOX_ELEMENT_ANCHOR("表面に沿って移動", eZAnchorBehavior.Z_ANCHOR_BEHAVIOR_SLIDE_ALONG_SURFACE));
            //            _comboboxAnchorList.Add(new COMBOBOX_ELEMENT_ANCHOR("表面に付着", eZAnchorBehavior.Z_ANCHOR_BEHAVIOR_ATTACHED_TO_SURFACE));
            _comboboxAnchorList.Add(new COMBOBOX_ELEMENT_ANCHOR("シーン内を自由に移動", eZAnchorBehavior.Z_ANCHOR_BEHAVIOR_MOVE_FREELY));
            _comboboxAnchorList.Add(new COMBOBOX_ELEMENT_ANCHOR("位置を固定", eZAnchorBehavior.Z_ANCHOR_BEHAVIOR_FIXED_POSITION));
            _comboboxAllSetAnchorList = new List<COMBOBOX_ELEMENT_ANCHOR>(_comboboxAnchorList);

            /* アセンブリ配下の選択を防止する */
            _comboboxAssemblyUnderShowOnOffList.Add(new COMBOBOX_ON_OFF("配下の選択を防止する", true));
            _comboboxAssemblyUnderShowOnOffList.Add(new COMBOBOX_ON_OFF("配下の選択を防止しない", false));
            _comboboxAssemblyUnderShowAllSetOnOffList = new List<COMBOBOX_ON_OFF>(_comboboxAssemblyUnderShowOnOffList);

            /* パーツ配下の要素を表示しない  */
            _comboboxPartUnderShowOnOffList.Add(new COMBOBOX_ON_OFF("配下の要素を表示しない", true));
            _comboboxPartUnderShowOnOffList.Add(new COMBOBOX_ON_OFF("配下の要素を表示する", false));
            _comboboxPartUnderShowAllSetOnOffList = new List<COMBOBOX_ON_OFF>(_comboboxPartUnderShowOnOffList);

            /* 初期値を設定 */
            comboBoxFunctionType.SelectedIndex = 0;
            comboBoxAllSetValue.DataSource = _comboboxAllSetAnchorList;
            comboBoxAllSetValue.DisplayMember = "displayName";
            comboBoxAllSetValue.ValueMember = "value";
        }


        /// <summary>
        /// ページ表示状態変更イベント(ページ表示イベントとして利用)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UserControlElementManager_VisibleChanged(object sender, EventArgs e)
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

                this._isInitializing = true;

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

                /* 列の幅をユーザーが指定可能に設定 */
                treeGridViewScene.Columns["Scene"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                treeGridViewScene.Columns["SystemName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                treeGridViewScene.Columns["Scene"].Width = treeGridViewScene.Columns["Scene"].Width+10;
                treeGridViewScene.Columns["SystemName"].Width = treeGridViewScene.Columns["SystemName"].Width + 10;
                treeGridViewScene.Columns["Scene"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                treeGridViewScene.Columns["SystemName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                /* ComboBox表示用データを作成 */
                CreateComboboxDataSet();                


                /* 初期化完了したのでProgressBarを解除 */
                setProgressBar(false);

                Form frm = this.Parent as Form;
                frm.Location = new Point(frm.Location.X+300, frm.Location.Y+100);
                this._isInitializing = false;
                this.Cursor = Cursors.Default;
                ((UserControlTagData)this.Tag).canNotClose = false;
            }
            if(Disposing == true)
            {
                treeGridViewScene.SelectionChanged -= treeGridViewScene_SelectionChanged;
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
        /// シーンブラウザを更新する
        /// </summary>
        private void UpdateSceneBrowser()
        {
            /* シーンブラウザを更新する(ElementにDirty[Z_DIRTY_GRAPHICS]を設定したものが更新される) */
            this._sceneDoc.UpdateBrowser(0);
            this._sceneDoc.UpdateGraphics(0);
        }

        /// <summary>
        /// Anchorを設定する
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="anchorBehaviorValue"></param>
        /// <returns></returns>
        private bool setAnchorBehavior(int rowIndex, eZAnchorBehavior anchorBehaviorValue)
        {
            if (treeGridViewScene["Scene", rowIndex].Tag == null)
            {
                return false;
            }
            this._isModify = true;
            IZSceneElement element = treeGridViewScene["Scene", rowIndex].Tag as IZSceneElement;
            element.AnchorBehavior = anchorBehaviorValue;
            treeGridViewScene["_Anchor", rowIndex].Value = element.AnchorBehavior;
            this._isModify = false;
            return true;
        }

        /// <summary>
        /// アセンブリ配下の選択を防止する を設定する
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="setBoolValue"></param>
        /// <returns></returns>
        private bool setPreventSelectionBelowAssembly(int rowIndex, bool setBoolValue)
        {
            if (treeGridViewScene["Scene", rowIndex].Tag == null)
            {
                return false;
            }
            this._isModify = true;
            IZElement element = treeGridViewScene["Scene", rowIndex].Tag as IZElement;
            IZSceneElement sceneElement = treeGridViewScene["Scene", rowIndex].Tag as IZSceneElement;
            IZAssembly asm = sceneElement as IZAssembly;
            if (asm == null)
            {
                return false;
            }
            asm.PreventSelectionBelowAssembly = setBoolValue;
            treeGridViewScene["_AssemblyUnderShow", rowIndex].Value = asm.PreventSelectionBelowAssembly;
            element.SetDirtyFlag(eZDirtyFlag.Z_DIRTY_GRAPHICS);
            this._isModify = false;
            return true;
        }


        /// <summary>
        /// パーツ配下の要素を表示しない
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="setBoolValue"></param>
        /// <returns></returns>
        private bool setPreventSelectionBelowPart(int rowIndex, bool setBoolValue)
        {
            if (treeGridViewScene["Scene", rowIndex].Tag == null)
            {
                return false;
            }
            this._isModify = true;
            IZElement element = treeGridViewScene["Scene", rowIndex].Tag as IZElement;
            IZSceneElement sceneElement = treeGridViewScene["Scene", rowIndex].Tag as IZSceneElement;
            IZPart part = sceneElement as IZPart;
            if (part == null)
            {
                return false;
            }
            part.PreventSelectionBelowPart = setBoolValue;
            treeGridViewScene["_PartUnderShow", rowIndex].Value = part.PreventSelectionBelowPart;
            element.SetDirtyFlag(eZDirtyFlag.Z_DIRTY_GRAPHICS);
            this._isModify = false;
            return true;
        }

        /// <summary>
        /// 選択している行番号を取得する
        /// </summary>
        /// <returns></returns>
        private List<int> getSelectedRowsList()
        {
            List<int> selectedRowsList = new List<int>();
            for (int i = 0; i < treeGridViewScene.SelectedCells.Count; i++)
            {
                int rowIndex = treeGridViewScene.SelectedCells[i].RowIndex;
                if (selectedRowsList.Contains(rowIndex) != true)
                {
                    selectedRowsList.Add(rowIndex);
                }
            }
            for (int i = 0; i < treeGridViewScene.SelectedRows.Count; i++)
            {
                int rowIndex = treeGridViewScene.SelectedRows[i].Index;
                if (selectedRowsList.Contains(rowIndex) != true)
                {
                    selectedRowsList.Add(rowIndex);
                }
            }
            return selectedRowsList;
        }

        /// <summary>
        /// 各機能で表示したデータ(列)を削除する
        /// </summary>
        private void clearFunctionColumn()
        {
            List<string> removeList = new List<string>();
            for (int i = 0; i < treeGridViewScene.Columns.Count; i++)
            {
                string columnName = treeGridViewScene.Columns[i].Name;
                if (columnName[0] == '_')
                {
                    removeList.Add(columnName);
                }
            }
            foreach (string removeColumnName in removeList)
            {
                treeGridViewScene.Columns.Remove(removeColumnName);
            }
        }

        /// <summary>
        /// 一括設定のComboBoxにマスタデータを設定する
        /// </summary>
        /// <param name="functionName"></param>
        private void setComboboxAllSetValue(string functionName)
        {

            switch (functionName)
            {
                case "Anchor":
                    comboBoxAllSetValue.DataSource = _comboboxAllSetAnchorList;
                    break;
                case "アセンブリ配下の選択を防止する":
                    comboBoxAllSetValue.DataSource = _comboboxAssemblyUnderShowAllSetOnOffList;
                    break;
                case "パーツ配下の要素を表示しない":
                    comboBoxAllSetValue.DataSource = _comboboxPartUnderShowAllSetOnOffList;
                    break;
                case "[表示のみ] 計算重量/指定重量":
                    comboBoxAllSetValue.DataSource = null;
                    break;
                default:
                    return;
            }
            if (comboBoxAllSetValue.DataSource != null)
            {
                groupBoxAllSetValue.Enabled = true;
                comboBoxAllSetValue.ValueMember = "value";
                comboBoxAllSetValue.DisplayMember = "displayName";
            }
            else
            {
                groupBoxAllSetValue.Enabled = false;
            }
            return;
        }

        /// <summary>
        /// 各機能に対する作成列を取得する
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        private List<DataGridViewColumn> getFunctionColumn(string functionName)
        {
            List<DataGridViewColumn> returnColumn = new List<DataGridViewColumn>();
            DataGridViewComboBoxColumn comboBoxColumn = new DataGridViewComboBoxColumn();
            DataGridViewTextBoxColumn textBoxColumn1 = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn textBoxColumn2 = new DataGridViewTextBoxColumn();

            switch (functionName)
            {
                case "Anchor":
                    comboBoxColumn.HeaderText = "アンカー設定";
                    comboBoxColumn.Name = "_Anchor";
                    comboBoxColumn.DataSource = _comboboxAnchorList;
                    returnColumn.Add(comboBoxColumn);
                    break;
                case "アセンブリ配下の選択を防止する":
                    comboBoxColumn.HeaderText = "アセンブリ配下の選択を防止する";
                    comboBoxColumn.Name = "_AssemblyUnderShow";
                    comboBoxColumn.DataSource = _comboboxAssemblyUnderShowOnOffList;
                    returnColumn.Add(comboBoxColumn);
                    break;
                case "パーツ配下の要素を表示しない":
                    comboBoxColumn.HeaderText = "パーツ配下の要素を表示しない";
                    comboBoxColumn.Name = "_PartUnderShow";
                    comboBoxColumn.DataSource = _comboboxPartUnderShowOnOffList;
                    returnColumn.Add(comboBoxColumn);
                    break;
                case "[表示のみ] 計算重量/指定重量":
                    textBoxColumn1.HeaderText = "計算重量";
                    textBoxColumn1.Name = "_CalculatedMass";
                    textBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    textBoxColumn1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    textBoxColumn2.HeaderText = "指定重量";
                    textBoxColumn2.Name = "_SpecifiedMass";
                    textBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    textBoxColumn2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    returnColumn.Add(textBoxColumn1);
                    returnColumn.Add(textBoxColumn2);
                    break;
                default:
                    return null;
            }
            if(comboBoxColumn != null)
            {
                comboBoxColumn.ValueMember = "value";
                comboBoxColumn.DisplayMember = "displayName";
                comboBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }

            return returnColumn;
        }


        /// <summary>
        /// 現在の設定値をTreeGridViewSceneに設定する
        /// </summary>
        /// <param name="functionName"></param>
        private void setCurrentElementValue(string functionName)
        {
            for (int i = 0; i < treeGridViewScene.Rows.Count; i++)
            {
                if (treeGridViewScene["Scene", i].Tag == null)
                {
                    continue;
                }

                IZElement element = treeGridViewScene["Scene", i].Tag as IZElement;
                IZSceneElement sceneElement = treeGridViewScene["Scene", i].Tag as IZSceneElement;
                string setColumnName1 = string.Empty;
                string setColumnName2 = string.Empty;
                IZPart part = null;
                IZAssembly asm = null;
                IZAssemblyProperty asmProp = null;
                IZPartProperty partProp = null;
                switch (functionName)
                {
                    case "Anchor":
                        setColumnName1 = "_Anchor";
                        eZAnchorBehavior anchor = sceneElement.AnchorBehavior;
                        treeGridViewScene[setColumnName1, i].Value = anchor;
                        break;
                    case "アセンブリ配下の選択を防止する":
                        setColumnName1 = "_AssemblyUnderShow";
                        asm = sceneElement as IZAssembly;
                        if (asm == null)
                        {
                            treeGridViewScene[setColumnName1, i].Value = null;
                            treeGridViewScene[setColumnName1, i].ReadOnly = true;
                            break;
                        }
                        treeGridViewScene[setColumnName1, i].Value = asm.PreventSelectionBelowAssembly;
                        treeGridViewScene[setColumnName1, i].ReadOnly = false;
                        break;
                    case "パーツ配下の要素を表示しない":
                        setColumnName1 = "_PartUnderShow";
                        part = sceneElement as IZPart;
                        if (part == null)
                        {
                            treeGridViewScene[setColumnName1, i].Value = null;
                            treeGridViewScene[setColumnName1, i].ReadOnly = true;
                            break;
                        }
                        treeGridViewScene[setColumnName1, i].Value = part.PreventSelectionBelowPart;
                        treeGridViewScene[setColumnName1, i].ReadOnly = false;
                        break;
                    case "[表示のみ] 計算重量/指定重量":
                        setColumnName1 = "_CalculatedMass";
                        setColumnName2 = "_SpecifiedMass";
                        part = sceneElement as IZPart;
                        asm = sceneElement as IZAssembly;
                        int massUnit = (int)this._docProp.MassUnit;
                        string massUnitStr = convertMassUnitToString(massUnit);
                        if (asm != null)
                        {
                            asmProp = sceneElement as IZAssemblyProperty;
                            treeGridViewScene[setColumnName1, i].Value = string.Format("{0} {1}", Math.Round(asmProp.CalculatedMass,3), massUnitStr);
                            treeGridViewScene[setColumnName2, i].Value = string.Format("{0} {1}", asmProp.SpecifiedMass, massUnitStr);
                            treeGridViewScene[setColumnName1, i].ReadOnly = true;
                            treeGridViewScene[setColumnName2, i].ReadOnly = true;
                        }
                        if(part != null)
                        {
                            if ((element.Type != eZElementType.Z_ELEMENT_PROFILE) &&
                                (element.Type != eZElementType.Z_ELEMENT_PROFILE_PART) &&
                                (element.Type != eZElementType.Z_ELEMENT_WIRE) &&
                                (element.Type != eZElementType.Z_ELEMENT_WIRE_PART) &&
                                (element.Type != eZElementType.Z_ELEMENT_FACET_PART))
                            {
                                partProp = sceneElement as IZPartProperty;
                                double density = partProp.MassDensity;
                                if (density == -1)
                                {
                                    partProp.MassDensity = 7800;
                                }
                                double calculatedMass = partProp.CalculatedMass / GetMassUnitScale(massUnit);
                                double specifiedMass = partProp.SpecifiedMass;
                                treeGridViewScene[setColumnName1, i].Value = string.Format("{0} {1}", Math.Round(calculatedMass,3), massUnitStr);
                                treeGridViewScene[setColumnName2, i].Value = string.Format("{0} {1}", specifiedMass, massUnitStr);
                                treeGridViewScene[setColumnName1, i].ReadOnly = true;
                                treeGridViewScene[setColumnName2, i].ReadOnly = true;
                            }
                        }

                        if ((asm == null) && (part == null))
                        {
                            treeGridViewScene[setColumnName1, i].Value = string.Empty;
                            treeGridViewScene[setColumnName1, i].ReadOnly = true;
                        }
                        break;
                    default:
                        break;
                }
                if ((i == 0) && (string.IsNullOrEmpty(setColumnName1) != true))
                {
                    treeGridViewScene[setColumnName1, i].ReadOnly = true;
                }
                if ((i == 0) && (string.IsNullOrEmpty(setColumnName2) != true))
                {
                    treeGridViewScene[setColumnName2, i].ReadOnly = true;
                }
            }

        }



        #region イベント


        /// <summary>
        /// 現在の設定値取得ボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGetElementInformation_Click(object sender, EventArgs e)
        {
            string functionName = comboBoxFunctionType.Text as string;
            if (string.IsNullOrEmpty(functionName) == true)
            {
                return;
            }
            this._isInitializing = true;
            _currFunctionName = functionName;
            clearFunctionColumn();
            List<DataGridViewColumn> columnList = getFunctionColumn(functionName);
            foreach (DataGridViewColumn column in columnList)
            {
                if (treeGridViewScene.Columns.Contains(column.Name) != true)
                {
                    treeGridViewScene.Columns.Add(column);
                }
            }

            setCurrentElementValue(functionName);
            setComboboxAllSetValue(functionName);
            this._isInitializing = false;
        }


        /// <summary>
        /// 一括設定のボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAllSetValue_Click(object sender, EventArgs e)
        {
            List<int> rowIndexList = new List<int>();

            /* 機能が選択されているか */
            if (string.IsNullOrEmpty(_currFunctionName) == true)
            {
                return;
            }

            /* 一括設定か選択設定か */
            if (checkBoxAllSetAllItem.Checked == true)
            {
                /* 一括設定 */
                DialogResult dret = MessageBox.Show("すべてのデータに対して設定を行います。よろしいですか？", "確認", MessageBoxButtons.YesNo);
                if (dret != DialogResult.Yes)
                {
                    return;
                }
                for (int i = 0; i < treeGridViewScene.Rows.Count; i++)
                {
                    rowIndexList.Add(i);
                }
            }
            else
            {
                /* 選択したデータへの一括設定 */
                rowIndexList = getSelectedRowsList();
                if ((rowIndexList.Count <= 0) ||
                    ((rowIndexList.Count == 1) && (rowIndexList[0] == 0)))
                {
                    MessageBox.Show("設定する対象のデータが選択されていません。\n設定する対象のデータを選択して設定ボタンをクリックしてください。");
                    return;
                }
            }
            bool setBoolValue = false;
            bool updateFlag = false;
            foreach (int rowIndex in rowIndexList)
            {
                switch (_currFunctionName)
                {
                    case "Anchor":
                        eZAnchorBehavior anchorBehaviorValue = (eZAnchorBehavior)comboBoxAllSetValue.SelectedValue;
                        setAnchorBehavior(rowIndex, anchorBehaviorValue);
                        break;
                    case "アセンブリ配下の選択を防止する":
                        updateFlag = true;
                        setBoolValue = (bool)comboBoxAllSetValue.SelectedValue;
                        setPreventSelectionBelowAssembly(rowIndex, setBoolValue);
                        break;
                    case "パーツ配下の要素を表示しない":
                        updateFlag = true;
                        setBoolValue = (bool)comboBoxAllSetValue.SelectedValue;
                        setPreventSelectionBelowPart(rowIndex, setBoolValue);
                        break;
                    default:
                        break;
                }
            }
            if (updateFlag == true)
            {
                UpdateSceneBrowser();
            }
        }


        /// <summary>
        /// TreeGridViewSceneのセル状態変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeGridViewScene_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (treeGridViewScene.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                treeGridViewScene.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        /// <summary>
        /// TreeGridViewSceneのセル値 状態変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeGridViewScene_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((this._isInitializing == true) || (this._isModify == true))
            {
                return;
            }
            int baseRowIndex = e.RowIndex;
            int baseColumnIndex = e.ColumnIndex;
            if ((e.RowIndex < 0) || (e.ColumnIndex < 0))
            {
                return;
            }

            if (((treeGridViewScene.Columns.Contains("_Anchor") != true) || (e.ColumnIndex != treeGridViewScene.Columns["_Anchor"].Index)) &&
               ((treeGridViewScene.Columns.Contains("_AssemblyUnderShow") != true) || (e.ColumnIndex != treeGridViewScene.Columns["_AssemblyUnderShow"].Index)) &&
               ((treeGridViewScene.Columns.Contains("_PartUnderShow") != true) || (e.ColumnIndex != treeGridViewScene.Columns["_PartUnderShow"].Index)))
            {
                return;
            }

            List<int> selectedRowIndexList = getSelectedRowsList();
            /* イベントを一時的に無効化する */
            this.treeGridViewScene.CellValueChanged -= treeGridViewScene_CellValueChanged;

            try
            {
                /* 値を変更する */
                bool setBoolValue = false;
                bool updateFlag = false;
                foreach (int rowIndex in selectedRowIndexList)
                {
                    switch (_currFunctionName)
                    {
                        case "Anchor":
                            eZAnchorBehavior anchorBehaviorValue = (eZAnchorBehavior)treeGridViewScene[baseColumnIndex, rowIndex].Value;
                            setAnchorBehavior(rowIndex, anchorBehaviorValue);
                            break;
                        case "アセンブリ配下の選択を防止する":
                            updateFlag = true;
                            setBoolValue = (bool)treeGridViewScene[baseColumnIndex, rowIndex].Value;
                            setPreventSelectionBelowAssembly(rowIndex, setBoolValue);
                            break;
                        case "パーツ配下の要素を表示しない":
                            updateFlag = true;
                            setBoolValue = (bool)treeGridViewScene[baseColumnIndex, rowIndex].Value;
                            setPreventSelectionBelowPart(rowIndex, setBoolValue);
                            break;
                        default:
                            break;
                    }
                }
                if (updateFlag == true)
                {
                    UpdateSceneBrowser();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("エラーが発生しました。");
            }
            treeGridViewScene.CommitEdit(DataGridViewDataErrorContexts.Commit);

            /* イベントを一時的に無効化する */
            this.treeGridViewScene.CellValueChanged += treeGridViewScene_CellValueChanged;
        }

        /// <summary>
        /// TreeGridViewSceneのデータ選択イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeGridViewScene_SelectionChanged(object sender, EventArgs e)
        {
            if ((this._isInitializing == true) || (this._isModify == true))
            {
                return;
            }
            if (checkBoxSelectedHighLight.Checked != true)
            {
                return;
            }
            List<int> selectedRowIndexList = getSelectedRowsList();
            IZSelectionMgr selectMgr = this._sceneDoc.SelectionMgr;
            selectMgr.RemoveAllFromSelection();
            for (int i = 0; i < selectedRowIndexList.Count; i++)
            {
                int rowIndex = selectedRowIndexList[i];
                if (treeGridViewScene["Scene", rowIndex].Tag == null)
                {
                    continue;
                }
                IZElement element = treeGridViewScene["Scene", rowIndex].Tag as IZElement;
                selectMgr.AddElementToSelection(element, true);
            }
        }
        #endregion イベント

    }
}
