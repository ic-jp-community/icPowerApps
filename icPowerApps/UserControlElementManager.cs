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
        private bool _compareId = false; /* ファイルから読込時にIDの一致まで見るか否か */
        private int _currentWorkSuppress = 1;
        private IZBaseApp _ironcadApp = null;
        private bool _isInitializing = false;
        private bool _isModify = false;


        List<COMBOBOX_ELEMENT_ANCHOR> _comboboxAnchorList = new List<COMBOBOX_ELEMENT_ANCHOR>();
        List<COMBOBOX_ELEMENT_ANCHOR> _comboboxAllSetAnchorList = new List<COMBOBOX_ELEMENT_ANCHOR>();
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
        /// コンストラクタ
        /// </summary>
        public UserControlElementManager(IZBaseApp ironcadApp)
        {
            InitializeComponent();
            this.Tag = new UserControlTagData();
            this.Dock = DockStyle.Fill;
            this._ironcadApp = ironcadApp;
            _comboboxAnchorList.Add(new COMBOBOX_ELEMENT_ANCHOR("表面に沿って移動", eZAnchorBehavior.Z_ANCHOR_BEHAVIOR_SLIDE_ALONG_SURFACE));
//            _comboboxAnchorList.Add(new COMBOBOX_ELEMENT_ANCHOR("表面に付着", eZAnchorBehavior.Z_ANCHOR_BEHAVIOR_ATTACHED_TO_SURFACE));
            _comboboxAnchorList.Add(new COMBOBOX_ELEMENT_ANCHOR("シーン内を自由に移動", eZAnchorBehavior.Z_ANCHOR_BEHAVIOR_MOVE_FREELY));
            _comboboxAnchorList.Add(new COMBOBOX_ELEMENT_ANCHOR("位置を固定", eZAnchorBehavior.Z_ANCHOR_BEHAVIOR_FIXED_POSITION));
            _comboboxAllSetAnchorList = new List<COMBOBOX_ELEMENT_ANCHOR>(_comboboxAnchorList);
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
                for (int columnIndex = 0; columnIndex < treeGridViewScene.Columns.Count; columnIndex++)
                {
                    int columnWidth = treeGridViewScene.Columns[columnIndex].Width;
                    treeGridViewScene.Columns[columnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    treeGridViewScene.Columns[columnIndex].Width = columnWidth;
                }

                /* 初期値を設定 */
                comboBoxSetType.SelectedIndex = 0;

                /* 初期化完了したのでProgressBarを解除 */
                setProgressBar(false);

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

        #region イベント


        /// <summary>
        /// 設定ボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGetElementInformation_Click(object sender, EventArgs e)
        {
            string columnName = comboBoxSetType.Text as string;
            if (string.IsNullOrEmpty(columnName) == true)
            {
                return;
            }
            this._isInitializing = true;
            if (treeGridViewScene.Columns.Contains(columnName) != true)
            {
                DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
                column.HeaderText = "アンカー設定";
                column.Name = "Anchor";
                column.DataSource = _comboboxAnchorList;
                column.ValueMember = "value";
                column.DisplayMember = "displayName";
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                treeGridViewScene.Columns.Add(column);
            }

            for (int i = 0; i < treeGridViewScene.Rows.Count; i++)
            {
                if (treeGridViewScene["Scene", i].Tag == null)
                {
                    continue;
                }
                IZSceneElement element = treeGridViewScene["Scene", i].Tag as IZSceneElement;
                eZAnchorBehavior anchor = element.AnchorBehavior;
                treeGridViewScene["Anchor", i].Value = anchor;
            }
            this._isInitializing = false;
        }


        private void buttonAllSetValue_Click(object sender, EventArgs e)
        {
            List<int> rowIndexList = getSelectedRowsList();
            if(checkBoxAllSetAllItem.Checked == true)
            {
                DialogResult dret = MessageBox.Show("すべてのデータに対して設定を行います。よろしいですか？", "確認", MessageBoxButtons.YesNo);
                if (dret != DialogResult.Yes)
                {
                    return;
                }
                rowIndexList = new List<int>();
                for (int i = 0; i < treeGridViewScene.Rows.Count; i++)
                {
                    rowIndexList.Add(i);
                }
            }
            else
            {
                if ((rowIndexList.Count <= 0) ||
                    ((rowIndexList.Count == 1) && (rowIndexList[0] == 0)))
                {
                    MessageBox.Show("設定する対象のデータが選択されていません。\n設定する対象のデータを選択して設定ボタンをクリックしてください。");
                    return;
                }
            }

            eZAnchorBehavior anchorBehaviorValue = (eZAnchorBehavior)comboBoxAllSetValue.SelectedValue;
            foreach (int rowIndex in rowIndexList)
            {
                setAnchorBehavior(rowIndex, anchorBehaviorValue);
            }
        }

        private bool setAnchorBehavior(int rowIndex, eZAnchorBehavior anchorBehaviorValue)
        {
            if (treeGridViewScene["Scene", rowIndex].Tag == null)
            {
                return false;
            }
            this._isModify = true;
            IZSceneElement element = treeGridViewScene["Scene", rowIndex].Tag as IZSceneElement;
            element.AnchorBehavior = anchorBehaviorValue;
            treeGridViewScene["Anchor", rowIndex].Value = element.AnchorBehavior;
            this._isModify = false;
            return true;
        }

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
        #endregion イベント

        private void treeGridViewScene_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (treeGridViewScene.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                treeGridViewScene.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

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

            List<int> selectedRowIndexList = getSelectedRowsList();
            /* イベントを一時的に無効化する */
            this.treeGridViewScene.CellValueChanged -= treeGridViewScene_CellValueChanged;

            try
            {
                /* 値を変更する */
                foreach (int rowIndex in selectedRowIndexList)
                {
                    if (baseColumnIndex == treeGridViewScene.Columns["Anchor"].Index)
                    {
                        if (treeGridViewScene[baseColumnIndex, rowIndex].Value != null)
                        {
                            setAnchorBehavior(rowIndex, (eZAnchorBehavior)treeGridViewScene[baseColumnIndex, rowIndex].Value);
                        }
                    }
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
    }
}
