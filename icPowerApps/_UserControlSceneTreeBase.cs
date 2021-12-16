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

namespace ICApiAddin.icPowerApps
{
    public partial class _UserControlSceneTreeBase : UserControl
    {
        public const string title = "システム情報の取得";
        IZBaseApp _ironcadApp = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public _UserControlSceneTreeBase(IZBaseApp ironcadApp)
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
        private async void _UserControlSceneTreeBase_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible && !Disposing)
            {
                icapiCommon.enableDataGridViewDoubleBuffer(treeGridViewScene);
                /* 高DPI対応 */
                ScaleReziser.InitializeFormControlScale(this, true, false, true, false, true);
                ((UserControlTagData)this.Tag).canNotClose = true;
                this.Cursor = Cursors.WaitCursor;
                ImageList imageList = new ImageList();
                icapiCommon.getImageListAssemblyParts(new Size(18, 18), ref imageList);
                treeGridViewScene.ImageList = imageList;
                getTreeGridView();
                treeGridViewScene.ShowLines = true;
                this.Cursor = Cursors.Default;
                ((UserControlTagData)this.Tag).canNotClose = false;
            }
        }        

        private void getTreeGridView()
        {
            TreeGridView tgv = treeGridViewScene;
            IZDoc doc = this._ironcadApp.ActiveDoc;
            IZSceneDoc sceneDoc = doc as IZSceneDoc;
            int TreeNodeHeight = 22;


            //string isModify = "変更なし";
            string userName = string.Empty;
            string systemName = string.Empty;
            string elementId = string.Empty;
            int currentDepth = 0;
            /* ★ データの順序変更はデザイナのColumn順番も変更する必要あり */
            /* ★ AddFileBasicCheckInのNodes.Add部分も変更する必要あり */
            TreeGridNode topNode = tgv.Nodes.Add(Path.GetFileName(doc.Name), systemName, elementId, icapiCommon.SCENE_DATATYPE_SCENE, currentDepth, doc.Name);
            topNode.ImageIndex = icapiCommon.getImageIndexAssemblyParts(icapiCommon.SCENE_DATATYPE_SCENE);
            topNode.Height = (int)(TreeNodeHeight * ScaleReziser.getScalingFactor());
            topNode.Expand();
            int depth = 1;
            icapiCommon.GetSceneTreeInfo(icapiCommon.CREATE_TREE_MODE.CHECK_IN, TreeNodeHeight, sceneDoc.GetTopElement(), ref topNode, ref depth);
            TreeGridNodeCollection nodes = topNode.Nodes;
            icapiCommon.ExpandTreeGridViewTreeNodes(ref nodes);
        }
    }
}
