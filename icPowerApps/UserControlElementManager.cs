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
        List<COMBOBOX_ON_OFF> _comboboxIncludeInBomOnOffList = new List<COMBOBOX_ON_OFF>();
        List<COMBOBOX_ON_OFF> _comboboxIncludeInBomAllSetOnOffList = new List<COMBOBOX_ON_OFF>();
        List<COMBOBOX_ON_OFF> _comboboxExpansionStateInBomOnOffList = new List<COMBOBOX_ON_OFF>();
        List<COMBOBOX_ON_OFF> _comboboxExpansionStateInBomAllSetOnOffList = new List<COMBOBOX_ON_OFF>();
        List<string> _comboboxMaterialClassificationAllSetList = new List<string>();
        List<COMBOBOX_MATERIAL> _comboboxMaterialListAllSetList = new List<COMBOBOX_MATERIAL>();
        List<COMBOBOX_MATERIAL> _comboboxMaterialListAllSetFilterdList = new List<COMBOBOX_MATERIAL>();

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
        /// 材料設定情報
        /// </summary>
        public class COMBOBOX_MATERIAL
        {
            public string classification { get; set; }
            public string displayName { get; set; }
            public string value { get; set; }
            public double density { get; set; }
            public COMBOBOX_MATERIAL()
            {
                this.classification = string.Empty;
                this.displayName = string.Empty;
                this.value = string.Empty;
                this.density = 0.0;
            }
            public COMBOBOX_MATERIAL(string classification, string displayName, double density)
            {
                this.classification = classification;
                this.displayName = displayName;
                this.value = displayName;
                this.density = density;
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
            comboBoxFunctionType.Items.Add("ユーザ名-パーツ番号-説明");
            comboBoxFunctionType.Items.Add("材料");
            comboBoxFunctionType.Items.Add("Anchor");
            comboBoxFunctionType.Items.Add("アセンブリ配下の選択を防止する");
            comboBoxFunctionType.Items.Add("パーツ配下の要素を表示しない");
            comboBoxFunctionType.Items.Add("[表示のみ] 計算重量/指定重量");
            comboBoxFunctionType.Items.Add("このシェイプをBOMに含める");
            comboBoxFunctionType.Items.Add("BOMでのアセンブリ展開");

            /* ユーザ名-パーツ番号-説明 */
            /* 特にマスターとなるデータは無し */

            /* 材料 */
            _comboboxMaterialClassificationAllSetList.Add("すべて");
            string filePath = Path.Combine(icapiCommon.GetIcApiDllPath(), "ElementManagerMaterial.csv");
            string[] materialList = File.ReadAllLines(filePath);
            foreach(string material in materialList)
            {
                string[] splitData = material.Split(',');
                double density = 0.0;
                if(splitData.Count() >= 3)
                {
                    double.TryParse(splitData[2], out density);
                }
                _comboboxMaterialListAllSetList.Add(new COMBOBOX_MATERIAL(splitData[0], splitData[1], density));

                if(_comboboxMaterialClassificationAllSetList.Contains(splitData[0]) != true)
                {
                    _comboboxMaterialClassificationAllSetList.Add(splitData[0]);
                }
            }
            _comboboxMaterialListAllSetFilterdList = new List<COMBOBOX_MATERIAL>(_comboboxMaterialListAllSetList);

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

            /* このシェイプをBOMに含める */
            _comboboxIncludeInBomOnOffList.Add(new COMBOBOX_ON_OFF("BOMに含める", true));
            _comboboxIncludeInBomOnOffList.Add(new COMBOBOX_ON_OFF("BOMに含めない", false));
            _comboboxIncludeInBomAllSetOnOffList = new List<COMBOBOX_ON_OFF>(_comboboxIncludeInBomOnOffList);

            /* BOMでのアセンブリ展開 */
            _comboboxExpansionStateInBomOnOffList.Add(new COMBOBOX_ON_OFF("展開(パーツも表示)", true));
            _comboboxExpansionStateInBomOnOffList.Add(new COMBOBOX_ON_OFF("展開しない(アセンブリのみ)", false));
            _comboboxExpansionStateInBomAllSetOnOffList = new List<COMBOBOX_ON_OFF>(_comboboxExpansionStateInBomOnOffList);

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
                if (this._ironcadApp == null)
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

                /* 列の幅をユーザーが指定可能に設定 */
                treeGridViewScene.Columns["Scene"].Width = treeGridViewScene.Columns["Scene"].Width+10;
                treeGridViewScene.Columns["SystemName"].Width = treeGridViewScene.Columns["SystemName"].Width + 10;

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
        /// 列サイズを自動調整する
        /// </summary>
        private void setColumnAutoSizeTreeGridViewScene()
        {
            for (int i = 0; i < treeGridViewScene.Columns.Count; i++)
            {
                string columnName = treeGridViewScene.Columns[i].Name;
                treeGridViewScene.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                treeGridViewScene.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
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
        /// Elementにユーザ名を設定する
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="anchorBehaviorValue"></param>
        /// <returns></returns>
        private bool setElementUserName(int rowIndex, string userName)
        {
            if (treeGridViewScene["Scene", rowIndex].Tag == null)
            {
                return false;
            }
            IZElement element = treeGridViewScene["Scene", rowIndex].Tag as IZElement;
            if (element == null)
            {
                return false;
            }
            this._isModify = true;
            element.Name = userName;
            element.SetDirtyFlag(eZDirtyFlag.Z_DIRTY_GRAPHICS);
            this._isModify = false;
            return true;
        }


        /// <summary>
        /// Elementにパーツ番号を設定する
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="anchorBehaviorValue"></param>
        /// <returns></returns>
        private bool setElementPartNumber(int rowIndex, string partNumber)
        {
            if (treeGridViewScene["Scene", rowIndex].Tag == null)
            {
                return false;
            }
            IZElement element = treeGridViewScene["Scene", rowIndex].Tag as IZElement;
            IZSceneElement sceneElement = treeGridViewScene["Scene", rowIndex].Tag as IZSceneElement;
            if (sceneElement == null)
            {
                return false;
            }
            IZPart part = sceneElement as IZPart;
            IZAssembly asm = sceneElement as IZAssembly;
            if ((asm == null) && (part == null))
            {
                return false;
            }
            this._isModify = true;
            if (asm != null)
            {
                asm.BOMPartNumber = partNumber;
            }
            if (part != null)
            {
                part.BOMPartNumber = partNumber;
            }
            element.SetDirtyFlag(eZDirtyFlag.Z_DIRTY_GRAPHICS);
            this._isModify = false;
            return true;
        }


        /// <summary>
        /// Elementにパーツの説明を設定する
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="anchorBehaviorValue"></param>
        /// <returns></returns>
        private bool setElementDescription(int rowIndex, string description)
        {
            if (treeGridViewScene["Scene", rowIndex].Tag == null)
            {
                return false;
            }
            IZElement element = treeGridViewScene["Scene", rowIndex].Tag as IZElement;
            IZSceneElement sceneElement = treeGridViewScene["Scene", rowIndex].Tag as IZSceneElement;
            if (sceneElement == null)
            {
                return false;
            }
            IZPart part = sceneElement as IZPart;
            IZAssembly asm = sceneElement as IZAssembly;
            if ((asm == null) && (part == null))
            {
                return false;
            }
            this._isModify = true;
            if (asm != null)
            {
                asm.BOMDescription = description;
            }
            if (part != null)
            {
                part.BOMDescription = description;
            }
            element.SetDirtyFlag(eZDirtyFlag.Z_DIRTY_GRAPHICS);
            this._isModify = false;
            return true;
        }


        /// <summary>
        /// Elementに材料を設定する
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="anchorBehaviorValue"></param>
        /// <returns></returns>
        private bool setElementMaterial(int rowIndex, string material)
        {
            if (treeGridViewScene["Scene", rowIndex].Tag == null)
            {
                return false;
            }
            IZElement element = treeGridViewScene["Scene", rowIndex].Tag as IZElement;
            IZSceneElement sceneElement = treeGridViewScene["Scene", rowIndex].Tag as IZSceneElement;
            if (sceneElement == null)
            {
                return false;
            }
            IZPart part = sceneElement as IZPart;
            IZAssembly asm = sceneElement as IZAssembly;
            IZAssemblyProperty asmProp = null;
            IZPartProperty partProp = null;
            bool thisOnly = false;
            string currMaterial = string.Empty;
            if ((asm == null) && (part == null))
            {
                return false;
            }
            this._isModify = true;
            if (asm != null)
            {
                asmProp = sceneElement as IZAssemblyProperty;
                asmProp.GetMaterialName(out currMaterial, out thisOnly);
                asmProp.SetMaterialName(material, thisOnly);
            }
            if (part != null)
            {
                partProp = sceneElement as IZPartProperty;
                partProp.GetMaterialName(out currMaterial, out thisOnly);
                partProp.SetMaterialName(material, thisOnly);
            }
            treeGridViewScene["_MaterialName", rowIndex].Value = material;
            element.SetDirtyFlag(eZDirtyFlag.Z_DIRTY_GRAPHICS);
            this._isModify = false;
            return true;
        }


        /// <summary>
        /// Elementに材料密度を設定する
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="anchorBehaviorValue"></param>
        /// <returns></returns>
        private bool setElementMassDensity(int rowIndex, double density)
        {
            if (treeGridViewScene["Scene", rowIndex].Tag == null)
            {
                return false;
            }
            IZElement element = treeGridViewScene["Scene", rowIndex].Tag as IZElement;
            IZSceneElement sceneElement = treeGridViewScene["Scene", rowIndex].Tag as IZSceneElement;
            if (sceneElement == null)
            {
                return false;
            }
            IZPart part = sceneElement as IZPart;
            IZAssembly asm = sceneElement as IZAssembly;
            IZPartProperty partProp = null;
            string currMaterial = string.Empty;
            if ((asm == null) && (part == null))
            {
                return false;
            }
            this._isModify = true;
            if (asm != null)
            {
                /* 処理なし */
            }
            if (part != null)
            {
                partProp = sceneElement as IZPartProperty;
                partProp.MassDensity = density;
                treeGridViewScene["_MassDensity", rowIndex].Value = density;
            }

            element.SetDirtyFlag(eZDirtyFlag.Z_DIRTY_GRAPHICS);
            this._isModify = false;
            return true;
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
            IZSceneElement sceneElement = treeGridViewScene["Scene", rowIndex].Tag as IZSceneElement;
            if (sceneElement == null)
            {
                return false;
            }
            sceneElement.AnchorBehavior = anchorBehaviorValue;
            treeGridViewScene["_Anchor", rowIndex].Value = sceneElement.AnchorBehavior;
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
            IZElement element = treeGridViewScene["Scene", rowIndex].Tag as IZElement;
            IZSceneElement sceneElement = treeGridViewScene["Scene", rowIndex].Tag as IZSceneElement;
            IZAssembly asm = sceneElement as IZAssembly;
            if (asm == null)
            {
                return false;
            }
            this._isModify = true;
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
            IZElement element = treeGridViewScene["Scene", rowIndex].Tag as IZElement;
            IZSceneElement sceneElement = treeGridViewScene["Scene", rowIndex].Tag as IZSceneElement;
            IZPart part = sceneElement as IZPart;
            if (part == null)
            {
                return false;
            }
            this._isModify = true;
            part.PreventSelectionBelowPart = setBoolValue;
            treeGridViewScene["_PartUnderShow", rowIndex].Value = part.PreventSelectionBelowPart;
            element.SetDirtyFlag(eZDirtyFlag.Z_DIRTY_GRAPHICS);
            this._isModify = false;
            return true;
        }


        /// <summary>
        /// シェイプをBOMに含めるの設定
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="setBoolValue"></param>
        /// <returns></returns>
        private bool setIncludeInBom(int rowIndex, bool setBoolValue)
        {
            if (treeGridViewScene["Scene", rowIndex].Tag == null)
            {
                return false;
            }
            IZElement element = treeGridViewScene["Scene", rowIndex].Tag as IZElement;
            IZSceneElement sceneElement = treeGridViewScene["Scene", rowIndex].Tag as IZSceneElement;
            IZPart part = sceneElement as IZPart;
            IZAssembly asm = sceneElement as IZAssembly;
            if ((asm == null) && (part == null))
            {
                return false;
            }
            this._isModify = true;
            if (asm != null)
            {
                asm.IncludedInBOM = setBoolValue;
                treeGridViewScene["_IncludeInBom", rowIndex].Value = asm.IncludedInBOM;
            }
            if (part != null)
            {
                part.IncludedInBOM = setBoolValue;
                treeGridViewScene["_IncludeInBom", rowIndex].Value = part.IncludedInBOM;
            }
            this._isModify = false;
            return true;
        }


        /// <summary>
        /// BOMの展開設定
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="setBoolValue"></param>
        /// <returns></returns>
        private bool setExpansionStateInBOM(int rowIndex, bool setBoolValue)
        {
            if (treeGridViewScene["Scene", rowIndex].Tag == null)
            {
                return false;
            }
            IZElement element = treeGridViewScene["Scene", rowIndex].Tag as IZElement;
            IZSceneElement sceneElement = treeGridViewScene["Scene", rowIndex].Tag as IZSceneElement;
            IZAssembly asm = sceneElement as IZAssembly;
            if (asm == null)
            {
                return false;
            }
            bool AllLinkedInstance = false;
            asm.GetExpansionStateInBOM(out AllLinkedInstance);
            asm.SetExpansionStateInBOM(AllLinkedInstance,setBoolValue);
            this._isModify = true;
            treeGridViewScene["_ExpansionStateInBom", rowIndex].Value = asm.GetExpansionStateInBOM(out AllLinkedInstance);
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
            bool enableCommon = true;
            bool enableMaterial = false;
            bool enableAllSet = true;
            comboBoxAllSetValue.DataSource = null;
            comboBoxAllSetMaterialValue.DataSource = null;
            comboBoxAllSetMaterialClassification.DataSource = null;
            switch (functionName)
            {
                case "ユーザ名-パーツ番号-説明":
                    comboBoxAllSetValue.DataSource = null;
                    enableAllSet = false;
                    break;
                case "材料":
                    comboBoxAllSetMaterialClassification.DataSource = _comboboxMaterialClassificationAllSetList;
                    comboBoxAllSetMaterialValue.DataSource = _comboboxMaterialListAllSetFilterdList;
                    if(comboBoxAllSetMaterialValue.SelectedIndex != -1)
                    {
                        COMBOBOX_MATERIAL materialData = comboBoxAllSetMaterialValue.Items[comboBoxAllSetMaterialValue.SelectedIndex] as COMBOBOX_MATERIAL;
                        textBoxMassDensity.Text = materialData.density.ToString();
                    }
                    enableMaterial = true;
                    enableCommon = false;
                    break;
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
                    enableAllSet = false;
                    break;
                case "このシェイプをBOMに含める":
                    comboBoxAllSetValue.DataSource = _comboboxIncludeInBomAllSetOnOffList;
                    break;
                case "BOMでのアセンブリ展開":
                    comboBoxAllSetValue.DataSource = _comboboxExpansionStateInBomAllSetOnOffList;
                    break;
                default:
                    return;
            }
            if (comboBoxAllSetValue.DataSource != null)
            {
                comboBoxAllSetValue.ValueMember = "value";
                comboBoxAllSetValue.DisplayMember = "displayName";
            }
            if (comboBoxAllSetMaterialValue.DataSource != null)
            {
                comboBoxAllSetMaterialValue.Enabled = true;
                comboBoxAllSetMaterialValue.ValueMember = "value";
                comboBoxAllSetMaterialValue.DisplayMember = "displayName";
            }
            tableLayoutPanelAllSetCommon.Visible = enableCommon;
            tableLayoutPanelAllSetMaterial.Visible = enableMaterial;
            groupBoxAllSetValue.Visible = enableAllSet;
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
            DataGridViewTextBoxColumn textBoxColumn3 = new DataGridViewTextBoxColumn();
            switch (functionName)
            {
                case "ユーザ名-パーツ番号-説明":
                    textBoxColumn1.HeaderText = "ユーザ名";
                    textBoxColumn1.Name = "_UserName";
                    textBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    textBoxColumn2.HeaderText = "パーツ番号";
                    textBoxColumn2.Name = "_PartNumber";
                    textBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    textBoxColumn3.HeaderText = "説明";
                    textBoxColumn3.Name = "_Description";
                    textBoxColumn3.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    returnColumn.Add(textBoxColumn1);
                    returnColumn.Add(textBoxColumn2);
                    returnColumn.Add(textBoxColumn3);
                    break;
                case "材料":
                    textBoxColumn1.HeaderText = "材料名";
                    textBoxColumn1.Name = "_MaterialName";
                    textBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    textBoxColumn2.HeaderText = "密度";
                    textBoxColumn2.Name = "_MassDensity";
                    textBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    returnColumn.Add(textBoxColumn1);
                    returnColumn.Add(textBoxColumn2);
                    break;
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
                    textBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    textBoxColumn1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    textBoxColumn2.HeaderText = "指定重量";
                    textBoxColumn2.Name = "_SpecifiedMass";
                    textBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    textBoxColumn2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    returnColumn.Add(textBoxColumn1);
                    returnColumn.Add(textBoxColumn2);
                    break;
                case "このシェイプをBOMに含める":
                    comboBoxColumn.HeaderText = "このシェイプをBOMに含める";
                    comboBoxColumn.Name = "_IncludeInBom";
                    comboBoxColumn.DataSource = _comboboxIncludeInBomOnOffList;
                    returnColumn.Add(comboBoxColumn);
                    break;
                case "BOMでのアセンブリ展開":
                    comboBoxColumn.HeaderText = "BOMでのアセンブリ展開";
                    comboBoxColumn.Name = "_ExpansionStateInBom";
                    comboBoxColumn.DataSource = _comboboxExpansionStateInBomOnOffList;
                    returnColumn.Add(comboBoxColumn);
                    break;
                default:
                    return null;
            }
            if (comboBoxColumn != null)
            {
                comboBoxColumn.MinimumWidth = 200;
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
        private void getCurrentSceneElementValue(string functionName)
        {
            int massUnit = (int)this._docProp.MassUnit;
            string massUnitStr = convertMassUnitToString(massUnit);
            double massUnitScale = GetMassUnitScale(massUnit);
            for (int i = 0; i < treeGridViewScene.Rows.Count; i++)
            {
                if (treeGridViewScene["Scene", i].Tag == null)
                {
                    treeGridViewScene.Rows[i].ReadOnly = true;
                    continue;
                }
                treeGridViewScene.Rows[i].ReadOnly = false;
                IZElement element = treeGridViewScene["Scene", i].Tag as IZElement;
                IZSceneElement sceneElement = treeGridViewScene["Scene", i].Tag as IZSceneElement;
                string setColumnName1 = string.Empty;
                string setColumnName2 = string.Empty;
                string setColumnName3 = string.Empty;
                string materialName = string.Empty;
                bool thisOnly = false;
                IZPart part = null;
                IZAssembly asm = null;
                IZAssemblyProperty asmProp = null;
                IZPartProperty partProp = null;

                switch (functionName)
                {
                    case "ユーザ名-パーツ番号-説明":
                        setColumnName1 = "_UserName";
                        setColumnName2 = "_PartNumber";
                        setColumnName3 = "_Description";
                        part = sceneElement as IZPart;
                        asm = sceneElement as IZAssembly;
                        if(element != null)
                        {
                            treeGridViewScene[setColumnName1, i].Value = element.Name;
                            treeGridViewScene[setColumnName1, i].ReadOnly = false;
                        }
                        else
                        {
                            treeGridViewScene[setColumnName1, i].Value = string.Empty;
                            treeGridViewScene[setColumnName1, i].ReadOnly = true;
                        }
                        if (asm != null)
                        {
                            treeGridViewScene[setColumnName2, i].Value = asm.BOMPartNumber;
                            treeGridViewScene[setColumnName3, i].Value = asm.BOMDescription;
                            treeGridViewScene[setColumnName2, i].ReadOnly = false;
                            treeGridViewScene[setColumnName3, i].ReadOnly = false;
                        }
                        if (part != null)
                        {
                            treeGridViewScene[setColumnName2, i].Value = part.BOMPartNumber;
                            treeGridViewScene[setColumnName3, i].Value = part.BOMDescription;
                            treeGridViewScene[setColumnName2, i].ReadOnly = false;
                            treeGridViewScene[setColumnName3, i].ReadOnly = false;
                        }
                        if ((asm == null) && (part == null))
                        {
                            treeGridViewScene[setColumnName2, i].Value = string.Empty;
                            treeGridViewScene[setColumnName2, i].ReadOnly = true;
                            treeGridViewScene[setColumnName3, i].Value = string.Empty;
                            treeGridViewScene[setColumnName3, i].ReadOnly = true;
                        }
                        break;
                    case "材料":
                        setColumnName1 = "_MaterialName";
                        setColumnName2 = "_MassDensity";
                        part = sceneElement as IZPart;
                        asm = sceneElement as IZAssembly;
                        if (asm != null)
                        {
                            IZAssemblyProperty asmprop = sceneElement as IZAssemblyProperty ;
                            double ach, vol;
                            asmprop.GetMaterialName(out materialName, out thisOnly);
                            treeGridViewScene[setColumnName1, i].Value = materialName;
                            if (checkBoxAssemblyDensitySkip.Checked != true)
                            {
                                vol = asmprop.GetVolume(0.0001, out ach);
                                double mass = asmprop.CalculatedMass;
                                treeGridViewScene[setColumnName2, i].Value = FastMath.Round(mass / vol, 3);
                            }
                            else
                            {
                                treeGridViewScene[setColumnName2, i].Value = "skip";
                            }
                            treeGridViewScene[setColumnName1, i].ReadOnly = false;
                            treeGridViewScene[setColumnName2, i].ReadOnly = true;
                        }
                        if (part != null)
                        {
                            IZPartProperty partprop = sceneElement as IZPartProperty;
                            partprop.GetMaterialName(out materialName, out thisOnly);
                            treeGridViewScene[setColumnName1, i].Value = materialName;
                            treeGridViewScene[setColumnName2, i].Value = FastMath.Round(partprop.MassDensity, 3);
                            treeGridViewScene[setColumnName1, i].ReadOnly = false;
                            treeGridViewScene[setColumnName2, i].ReadOnly = false;
                        }
                        if ((asm == null) && (part == null))
                        {
                            treeGridViewScene[setColumnName1, i].Value = string.Empty;
                            treeGridViewScene[setColumnName1, i].ReadOnly = true;
                            treeGridViewScene[setColumnName2, i].Value = string.Empty;
                            treeGridViewScene[setColumnName2, i].ReadOnly = true;
                        }
                        break;
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
                        if (asm != null)
                        {
                            asmProp = sceneElement as IZAssemblyProperty;
                            treeGridViewScene[setColumnName1, i].Value = string.Format("{0} {1}", FastMath.Round(asmProp.CalculatedMass,3), massUnitStr);
                            treeGridViewScene[setColumnName2, i].Value = string.Format("{0} {1}", asmProp.SpecifiedMass, massUnitStr);
                            treeGridViewScene[setColumnName1, i].ReadOnly = true;
                            treeGridViewScene[setColumnName2, i].ReadOnly = true;
                        }
                        if (part != null)
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
                                double calculatedMass = partProp.CalculatedMass / massUnitScale;
                                double specifiedMass = partProp.SpecifiedMass;
                                treeGridViewScene[setColumnName1, i].Value = string.Format("{0} {1}", FastMath.Round(calculatedMass,3), massUnitStr);
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
                    case "このシェイプをBOMに含める":
                        setColumnName1 = "_IncludeInBom";
                        asm = sceneElement as IZAssembly;
                        part = sceneElement as IZPart;
                        if ((asm == null) && (part == null))
                        {
                            treeGridViewScene[setColumnName1, i].Value = null;
                            treeGridViewScene[setColumnName1, i].ReadOnly = true;
                            break;
                        }
                        if(asm != null)
                        {
                            treeGridViewScene[setColumnName1, i].Value = asm.IncludedInBOM;
                            treeGridViewScene[setColumnName1, i].ReadOnly = false;
                        }
                        if (part != null)
                        {
                            treeGridViewScene[setColumnName1, i].Value = part.IncludedInBOM;
                            treeGridViewScene[setColumnName1, i].ReadOnly = false;
                        }
                        break;
                    case "BOMでのアセンブリ展開":
                        setColumnName1 = "_ExpansionStateInBom";
                        asm = sceneElement as IZAssembly;
                        if (asm == null)
                        {
                            treeGridViewScene[setColumnName1, i].Value = null;
                            treeGridViewScene[setColumnName1, i].ReadOnly = true;
                            break;
                        }
                        bool linkedInstance = false;
                        bool expansionStateInBom = asm.GetExpansionStateInBOM(out linkedInstance);
                        treeGridViewScene[setColumnName1, i].Value = expansionStateInBom;
                        treeGridViewScene[setColumnName1, i].ReadOnly = false;
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
                if ((i == 0) && (string.IsNullOrEmpty(setColumnName3) != true))
                {
                    treeGridViewScene[setColumnName3, i].ReadOnly = true;
                }
            }
        }


        /// <summary>
        /// Formのサイズを自動調整する(Max800)
        /// </summary>
        private void FormAutoSize()
        {
            int allWidth = 0;
            foreach (DataGridViewColumn column in treeGridViewScene.Columns)
            {
                if (column.Visible == true)
                {
                    allWidth += column.Width;
                }
            }
            allWidth = allWidth + 70;
            Form frm = this.Parent as Form;
            if (frm.Width < allWidth)
            {
                if (allWidth > 800)
                {
                    allWidth = 800;
                }
                frm.Width = allWidth;
                frm.Refresh();
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
            this._currFunctionName = functionName;
            clearFunctionColumn();
            List<DataGridViewColumn> columnList = getFunctionColumn(this._currFunctionName);
            foreach (DataGridViewColumn column in columnList)
            {
                if (treeGridViewScene.Columns.Contains(column.Name) != true)
                {
                    treeGridViewScene.Columns.Add(column);
                }
            }
            RefreshElementInformation(true);
        }

        /// <summary>
        /// Elementの情報を更新(再取得)する
        /// </summary>
        /// <param name="resizeForm"></param>
        private void RefreshElementInformation(bool resizeForm)
        {
            this._isInitializing = true;
            getCurrentSceneElementValue(this._currFunctionName);
            if(resizeForm == true)
            {
                setColumnAutoSizeTreeGridViewScene();
                FormAutoSize();
            }
            setComboboxAllSetValue(this._currFunctionName);
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
            if (string.IsNullOrEmpty(this._currFunctionName) == true)
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
            bool updateSceneBrowserFlag = false;
            bool updateCurrentElementInformation = true;
            foreach (int rowIndex in rowIndexList)
            {
                switch (this._currFunctionName)
                {
                    case "ユーザ名-パーツ番号-説明":
                        /* 一括設定なし */
                        break;
                    case "材料":
                        int index = comboBoxAllSetMaterialValue.SelectedIndex;
                        COMBOBOX_MATERIAL materialData = comboBoxAllSetMaterialValue.Items[index] as COMBOBOX_MATERIAL;
                        string materialName = (string)comboBoxAllSetMaterialValue.SelectedValue;
                        double massDensity = 0.0;
                        bool parse = double.TryParse(textBoxMassDensity.Text, out massDensity);
                        if(parse != true)
                        {
                            MessageBox.Show("密度の値が不正な値です。");
                            return;
                        }
                        setElementMaterial(rowIndex, materialData.value);
                        setElementMassDensity(rowIndex, massDensity);
                        break;
                    case "Anchor":
                        eZAnchorBehavior anchorBehaviorValue = (eZAnchorBehavior)comboBoxAllSetValue.SelectedValue;
                        setAnchorBehavior(rowIndex, anchorBehaviorValue);
                        break;
                    case "アセンブリ配下の選択を防止する":
                        updateSceneBrowserFlag = true;
                        setBoolValue = (bool)comboBoxAllSetValue.SelectedValue;
                        setPreventSelectionBelowAssembly(rowIndex, setBoolValue);
                        break;
                    case "パーツ配下の要素を表示しない":
                        updateSceneBrowserFlag = true;
                        setBoolValue = (bool)comboBoxAllSetValue.SelectedValue;
                        setPreventSelectionBelowPart(rowIndex, setBoolValue);
                        break;
                    case "このシェイプをBOMに含める":
                        setBoolValue = (bool)comboBoxAllSetValue.SelectedValue;
                        setIncludeInBom(rowIndex, setBoolValue);
                        break;
                    case "BOMでのアセンブリ展開":
                        setBoolValue = (bool)comboBoxAllSetValue.SelectedValue;
                        setExpansionStateInBOM(rowIndex, setBoolValue);
                        updateCurrentElementInformation = true;
                        break;
                    default:
                        break;
                }
            }
            if (updateSceneBrowserFlag == true)
            {
                UpdateSceneBrowser();
            }
            if(updateCurrentElementInformation == true)
            {
                RefreshElementInformation(false);
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
                switch (this._currFunctionName)
                {
                    case "ユーザ名-パーツ番号-説明":
                    case "材料":
                        /* 処理なし(columnの種類がtextboxのため) */
                        break;
                    case "Anchor":
                    case "アセンブリ配下の選択を防止する":
                    case "パーツ配下の要素を表示しない":
                    case "このシェイプをBOMに含める":
                    case "BOMでのアセンブリ展開":
                        // This fires the cell value changed handler below
                        treeGridViewScene.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        break;
                    default:
                        break;
                }
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

            /* Combobox系のみ処理するのでそれ以外は省く */
            if (((treeGridViewScene.Columns.Contains("_Anchor") != true) || (e.ColumnIndex != treeGridViewScene.Columns["_Anchor"].Index)) &&
               ((treeGridViewScene.Columns.Contains("_AssemblyUnderShow") != true) || (e.ColumnIndex != treeGridViewScene.Columns["_AssemblyUnderShow"].Index)) &&
               ((treeGridViewScene.Columns.Contains("_PartUnderShow") != true) || (e.ColumnIndex != treeGridViewScene.Columns["_PartUnderShow"].Index)) &&
               ((treeGridViewScene.Columns.Contains("_IncludeInBom") != true) || (e.ColumnIndex != treeGridViewScene.Columns["_IncludeInBom"].Index)) &&
               ((treeGridViewScene.Columns.Contains("_ExpansionStateInBom") != true) || (e.ColumnIndex != treeGridViewScene.Columns["_ExpansionStateInBom"].Index)))
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
                bool updateSceneBrowserFlag = false;
                bool updateCurrentElementInformation = false;
                foreach (int rowIndex in selectedRowIndexList)
                {
                    switch (this._currFunctionName)
                    {
                        case "ユーザ名-パーツ番号-説明":
                        case "材料":
                            /* treeGridViewScene_CellEndEditで処理する */
                            break;
                        case "Anchor":
                            eZAnchorBehavior anchorBehaviorValue = (eZAnchorBehavior)treeGridViewScene[baseColumnIndex, rowIndex].Value;
                            setAnchorBehavior(rowIndex, anchorBehaviorValue);
                            break;
                        case "アセンブリ配下の選択を防止する":
                            updateSceneBrowserFlag = true;
                            setBoolValue = (bool)treeGridViewScene[baseColumnIndex, rowIndex].Value;
                            setPreventSelectionBelowAssembly(rowIndex, setBoolValue);
                            break;
                        case "パーツ配下の要素を表示しない":
                            updateSceneBrowserFlag = true;
                            setBoolValue = (bool)treeGridViewScene[baseColumnIndex, rowIndex].Value;
                            setPreventSelectionBelowPart(rowIndex, setBoolValue);
                            break;
                        case "このシェイプをBOMに含める":
                            setBoolValue = (bool)treeGridViewScene[baseColumnIndex, rowIndex].Value;
                            setIncludeInBom(rowIndex, setBoolValue);
                            break;
                        case "BOMでのアセンブリ展開":
                            setBoolValue = (bool)treeGridViewScene[baseColumnIndex, rowIndex].Value;
                            setExpansionStateInBOM(rowIndex, setBoolValue);
                            updateCurrentElementInformation = true;
                            break;
                        default:
                            break;
                    }
                }
                if (updateSceneBrowserFlag == true)
                {
                    UpdateSceneBrowser();
                }
                if(updateCurrentElementInformation == true)
                {
                    RefreshElementInformation(false);
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

        /// <summary>
        /// アセンブリのみを表示するのチェック変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxShowAssemblyOnly_CheckedChanged(object sender, EventArgs e)
        {
            ShowAssemblyOnly(checkBoxShowAssemblyOnly.Checked);
        }


        /// <summary>
        /// セル変更終了イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeGridViewScene_CellEndEdit(object sender, DataGridViewCellEventArgs e)
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

            if (((treeGridViewScene.Columns.Contains("_UserName") != true) || (e.ColumnIndex != treeGridViewScene.Columns["_UserName"].Index)) &&
                ((treeGridViewScene.Columns.Contains("_PartNumber") != true) || (e.ColumnIndex != treeGridViewScene.Columns["_PartNumber"].Index)) &&
                ((treeGridViewScene.Columns.Contains("_Description") != true) || (e.ColumnIndex != treeGridViewScene.Columns["_Description"].Index)) &&
                ((treeGridViewScene.Columns.Contains("_MaterialName") != true) || (e.ColumnIndex != treeGridViewScene.Columns["_MaterialName"].Index)) &&
                ((treeGridViewScene.Columns.Contains("_MassDensity") != true) || (e.ColumnIndex != treeGridViewScene.Columns["_MassDensity"].Index)))
            {
                return;
            }

            try
            {
                /* 値を変更する */
                bool updateSceneBrowserFlag = false;
                bool updateCurrentElementInformation = false;
                string strValue = string.Empty;
                switch (this._currFunctionName)
                {
                    case "ユーザ名-パーツ番号-説明":
                        /*treeGridViewScene_CellEndEdit*/
                        updateSceneBrowserFlag = true;
                        updateCurrentElementInformation = true;
                        strValue = treeGridViewScene[baseColumnIndex, baseRowIndex].Value.ToString();
                        switch (treeGridViewScene.Columns[baseColumnIndex].Name)
                        {
                            case "_UserName":
                                setElementUserName(baseRowIndex, strValue);
                                break;
                            case "_PartNumber":
                                setElementPartNumber(baseRowIndex, strValue);
                                break;
                            case "_Description":
                                setElementDescription(baseRowIndex, strValue);
                                break;
                        }
                        break;
                    case "材料":
                        /*treeGridViewScene_CellEndEdit*/
                        updateSceneBrowserFlag = false;
                        updateCurrentElementInformation = true;
                        strValue = treeGridViewScene[baseColumnIndex, baseRowIndex].Value.ToString();
                        switch (treeGridViewScene.Columns[baseColumnIndex].Name)
                        {
                            case "_MaterialName":
                                setElementMaterial(baseRowIndex, strValue);
                                break;
                            case "_MassDensity":
                                double density = 0.0;
                                bool parse = double.TryParse(strValue, out density);
                                if (parse == true)
                                {
                                    setElementMassDensity(baseRowIndex, density);
                                }
                                break;
                        }
                        break;
                    case "Anchor":
                        /*treeGridViewScene_CellValueChanged*/
                        break;
                    case "アセンブリ配下の選択を防止する":
                        /*treeGridViewScene_CellValueChanged*/
                        break;
                    case "パーツ配下の要素を表示しない":
                        /*treeGridViewScene_CellValueChanged*/
                        break;
                    case "このシェイプをBOMに含める":
                        /*treeGridViewScene_CellValueChanged*/
                        break;
                    case "BOMでのアセンブリ展開":
                        /*treeGridViewScene_CellValueChanged*/
                        break;
                    default:
                        break;
                }
                if (updateSceneBrowserFlag == true)
                {
                    UpdateSceneBrowser();
                }
                if (updateCurrentElementInformation == true)
                {
                    RefreshElementInformation(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("エラーが発生しました。");
            }
        }

        /// <summary>
        /// 一括設定の材料分類変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxAllSetMaterialClassification_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((this._isInitializing == true) || (this._isModify == true))
            {
                return;
            }
            string text = comboBoxAllSetMaterialClassification.Text;
            this._comboboxMaterialListAllSetFilterdList.Clear();
            if (string.Equals(text, "すべて") == true)
            {
                _comboboxMaterialListAllSetFilterdList = new List<COMBOBOX_MATERIAL>(_comboboxMaterialListAllSetList);
            }
            else
            {
                _comboboxMaterialListAllSetFilterdList.AddRange(_comboboxMaterialListAllSetList.Where(a => a.classification == text).ToList());
            }
            this._isInitializing = true;
            comboBoxAllSetMaterialValue.DataSource = null;
            comboBoxAllSetMaterialValue.DataSource = _comboboxMaterialListAllSetFilterdList;
            comboBoxAllSetMaterialValue.ValueMember = "value";
            comboBoxAllSetMaterialValue.DisplayMember = "displayName";
            this._isInitializing = false;
            comboBoxAllSetMaterialValue.SelectedIndex = -1;
            comboBoxAllSetMaterialValue.SelectedIndex = 0;
        }


        /// <summary>
        /// 一括設定の材料変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxAllSetMaterialValue_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((this._isInitializing == true) || (this._isModify == true))
            {
                return;
            }
            if (comboBoxAllSetMaterialValue.SelectedIndex < 0)
            {
                textBoxMassDensity.Text = "0";
                return;
            }
            COMBOBOX_MATERIAL materialData = comboBoxAllSetMaterialValue.Items[comboBoxAllSetMaterialValue.SelectedIndex] as COMBOBOX_MATERIAL;
            textBoxMassDensity.Text = materialData.density.ToString();
        }

        /// <summary>
        /// キー押下イベント 数字と.(ドット)とバックスペースのみ許可する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxMassDensity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || '9' < e.KeyChar) &&
               (e.KeyChar != '.') &&
               (e.KeyChar != '\b'))
            {
                //押されたキーが 0～9でない場合は、イベントをキャンセルする
                e.Handled = true;
            }
        }
        #endregion イベント


        private void ShowAssemblyOnly(bool assemblyOnly)
        {
            if (assemblyOnly == true)
            {
                for (int i = 0; i < treeGridViewScene.Rows.Count; i++)
                {
                    string dataType = treeGridViewScene["DataType", i].Value.ToString();
                    switch (dataType)
                    {
                        case icapiCommon.SCENE_DATATYPE_SCENE:
                        case icapiCommon.SCENE_DATATYPE_ASSEMBLY:
                        case icapiCommon.SCENE_DATATYPE_LINKED_ASSEMBLY:
                            treeGridViewScene.Rows[i].Visible = true;
                            break;
                        default:
                            treeGridViewScene.Rows[i].Visible = false;
                            break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < treeGridViewScene.Rows.Count; i++)
                {
                    treeGridViewScene.Rows[i].Visible = true;
                }
            }
        }

        private void comboBoxFunctionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this._isInitializing == true) || (this._isModify == true))
            {
                return;
            }
            if (comboBoxFunctionType.SelectedIndex < 0)
            {
                return;
            }
            string functionType = comboBoxFunctionType.Items[comboBoxFunctionType.SelectedIndex].ToString();
            if(functionType == "材料")
            {
                checkBoxAssemblyDensitySkip.Visible = true;
            }
            else
            {
                checkBoxAssemblyDensitySkip.Visible = false;
            }
        }
    }
}
