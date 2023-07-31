using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using AdvancedDataGridView;
using interop.ICApiIronCAD;

namespace ICApiAddin.icPowerApps
{
    public partial class UserControlSceneBrowserTreeSort : UserControl
    {
        public const string title = "ソーンブラウザ ソート";
        private IZBaseApp _ironcadApp = null;

        /// <summary>
        /// ソートオプション
        /// </summary>
        private class SortOptions
        {
            public NaturalSortOrder order;
            public bool asmPartsMix;
            public bool sortAllChild;
            public SortOptions()
            {
                this.order = NaturalSortOrder.Ascending;
                this.asmPartsMix = false;
                this.sortAllChild = false;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UserControlSceneBrowserTreeSort(IZBaseApp ironcadApp)
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
        private void UserControlSceneBrowserTreeSort_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible && !Disposing)
            {
                if (this._ironcadApp == null)
                {
                    MessageBox.Show("IRONCADと連携する機能のため、スタンドアロン起動では使用できません。");
                    return;
                }
                /* 高DPI対応 */
                ScaleReziser.InitializeFormControlScale(this, true, false, true, false, true);

                /* 初期化 */
                ((UserControlTagData)this.Tag).canNotClose = false;
            }
        }


        /// <summary>
        /// ソートのオプション設定を取得する
        /// </summary>
        /// <param name="sortOptions"></param>
        private void getSortOptions(ref SortOptions sortOptions)
        {
            if (radioButtonAscending.Checked == true)
            {
                sortOptions.order = NaturalSortOrder.Ascending;
            }
            else
            {
                sortOptions.order = NaturalSortOrder.Descending;
            }
            if (radioButtonSortMix.Checked == true)
            {
                sortOptions.asmPartsMix = true;
            }
            else
            {
                sortOptions.asmPartsMix = false;
            }
            if (radioButtonUnderAll.Checked == true)
            {
                sortOptions.sortAllChild = true;
            }
            else
            {
                sortOptions.sortAllChild = false;
            }
        }

        /// <summary>
        /// 当該Elementをソートする
        /// </summary>
        /// <param name="sortOptions"></param>
        /// <param name="topElement"></param>
        private void sortChildElement(SortOptions sortOptions, IZElement topElement)
        {
            /* ソート対象のElement管理用のTreeGridView */
            TreeGridView tgv = new TreeGridView();
            initializeTreeGridView(tgv);

            /* ソート対象配下の全Elementを取得しTreeGridViewに設定する */
            List<SortData> dataList = getAllChildElementData(tgv, topElement);

            /* 空のアセンブリを配下に作成 */
            IZAssembly selectAsm = topElement as IZAssembly;
            IZAssembly emptyAsm = selectAsm.CreateSubAssembly();

            /* ソート対象(1階層)のElementを取得 */
            List<SortData> sortDataList1 = new List<SortData>();
            List<SortData> sortDataList2 = new List<SortData>();
            int sortDepth = 0;
            if (sortOptions.asmPartsMix == true)
            {
                /* パーツ アセンブリをごちゃまぜ状態でソートする場合 */
                sortDataList1 = dataList.Where(a => a.depth == sortDepth.ToString()).ToList();
            }
            else
            {
                /* パーツ アセンブリを別々でソートする場合 */
                sortDataList1 = dataList.Where(a => a.depth == sortDepth.ToString()).Where(a => a.type == eZElementType.Z_ELEMENT_ASSEMBLY).ToList();
                sortDataList2 = dataList.Where(a => a.depth == sortDepth.ToString()).Where(a => a.type != eZElementType.Z_ELEMENT_ASSEMBLY).ToList();
            }

            /* ソートした結果を仮のアセンブリに入れる */
            SortData[] sortArray1 = sortDataList1.ToArray();
            Array.Sort(sortArray1, new NaturalComparer(sortOptions.order));
            for (int num = 0; num < sortArray1.Count(); num++)
            {
                if (sortOptions.sortAllChild == true)
                {
                    /* 配下も全てソート処理する場合 */
                    if (sortArray1[num].type == eZElementType.Z_ELEMENT_ASSEMBLY)
                    {
                        /* アセンブリのデータの場合は下位層を再帰処理していく */
                        sortChildElement(sortOptions, sortArray1[num].elem);
                    }
                }
                emptyAsm.MoveChild(sortArray1[num].elem, true);
            }

            SortData[] sortArray2 = sortDataList2.ToArray();
            Array.Sort(sortArray2, new NaturalComparer(sortOptions.order));
            /* sortArray2にはアセンブリは入らないのでそのまま仮のアセンブリに突っ込む */
            for (int num = 0; num < sortArray2.Count(); num++)
            {
                emptyAsm.MoveChild(sortArray2[num].elem, true);
            }
            emptyAsm.Disassemble();
        }


        /// <summary>
        /// ソート用のTreeGridViewに初期化する
        /// </summary>
        /// <param name="tgv"></param>
        private static void initializeTreeGridView(TreeGridView tgv)
        {
            TreeGridColumn column = new TreeGridColumn();
            column.Name = "Scene";
            column.HeaderText = "Scene";
            tgv.Columns.Add(column);
            tgv.Columns.Add("SystemName", "SystemName");
            tgv.Columns.Add("Id", "Id");
            tgv.Columns.Add("DataType", "DataType");
            tgv.Columns.Add("Depth", "Depth");
            tgv.Columns.Add("ExternalLink", "ExternalLink");

            ImageList imageList = new ImageList();
            icapiCommon.getImageListAssemblyParts(new Size(18, 18), ref imageList);
            tgv.ImageList = imageList;
            tgv.ShowLines = true;
        }

        /// <summary>
        /// 当該Element配下の全Elementを取得する
        /// </summary>
        /// <param name="tgv"></param>
        /// <param name="topElement"></param>
        /// <returns></returns>
        private List<SortData> getAllChildElementData(TreeGridView tgv, IZElement topElement)
        {
            int currDepth = 0;
            TreeGridNode topNode = tgv.Nodes.Add(topElement.Name, topElement.SystemName, topElement.Id, icapiCommon.SCENE_DATATYPE_ASSEMBLY, currDepth, this._ironcadApp.ActiveDoc.Name);
            icapiCommon.GetSceneTreeInfo(icapiCommon.CREATE_TREE_MODE.CHECK_IN, 10, topElement, ref topNode, ref currDepth);

            /* ノードを全て展開状態にする */
            TreeGridNodeCollection nodes = tgv.Nodes;
            icapiCommon.ExpandTreeGridViewTreeNodes(ref nodes);


            /* 選択行配下の情報を取得 */
            List<SortData> dataList = new List<SortData>();
            for (int row = 1; row < tgv.Rows.Count; row++)
            {
                string name = icapiCommon.treeGridViewRowsValueGet(tgv, row, "Scene");
                string depth = icapiCommon.treeGridViewRowsValueGet(tgv, row, "Depth");
                IZElement element = tgv["Scene", row].Tag as IZElement;
                eZElementType elemType = eZElementType.Z_ELEMENT_UNKNOWN;
                IZElement parentElem = null;
                if (element != null)
                {
                    elemType = element.Type;
                    parentElem = element.GetParent();
                }
                dataList.Add(new SortData(row, name, depth, string.Concat(depth, "_", name), parentElem, element, elemType));
            }

            return dataList;
        }

        /// <summary>
        /// シーンブラウザで選択しているElementを取得する
        /// </summary>
        /// <param name="elemList"></param>
        private void getSelectedAssemblyElement(ref List<IZElement> elemList)
        {
            elemList.Clear();
            IZDoc doc = this._ironcadApp.ActiveDoc;
            IZSceneDoc scene = doc as IZSceneDoc;

            ZSelectionMgr selectMgr = scene.SelectionMgr;
            object elements = selectMgr.GetSelectedElements();
            List<IZElement> list = Addin.ConvertObjectToElementArray(elements);
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Type == eZElementType.Z_ELEMENT_ASSEMBLY)
                    {
                        elemList.Add(list[i]);
                    }
                }
            }
        }
        #region イベント
        /// <summary>
        /// 並び替えするボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSort_Click(object sender, EventArgs e)
        {
            /* バックアップの警告を表示する */
            if(checkBoxWarningBackup.Checked != true)
            {
                DialogResult dret = MessageBox.Show("並び替えによる不測の事態に備えてバックアップを必ずお取りください。\n並び替えを実行しますか？", "ご注意", MessageBoxButtons.YesNo);
                if(dret != DialogResult.Yes)
                {
                    return;
                }
            }

            /* ソートオプションを取得する */
            SortOptions sortOptions = new SortOptions();
            getSortOptions(ref sortOptions);

            if ((radioButtonSortAll.Checked != true) && (radioButtonSortSelectedElement.Checked != true))
            {
                MessageBox.Show("並び替え対象を選択してください。");
                return;

            }
            List<IZElement> elementList = new List<IZElement>();
            IZDoc doc = this._ironcadApp.ActiveDoc;
            IZSceneDoc scene = doc as IZSceneDoc;

            if (radioButtonSortAll.Checked == true)
            {
                /* TopSceneのElementを取得 */
                IZElement sceneTopElem = scene.GetTopElement();
                elementList.Add(sceneTopElem);
            }
            if (radioButtonSortSelectedElement.Checked == true)
            {
                /* 選択しているElementを取得 */
                getSelectedAssemblyElement(ref elementList);
            }

            if (elementList.Count <= 0)
            {
                MessageBox.Show("並び替えの対象データがないか対象を未選択です。");
                return;
            }

            /* 対象のElementをソートする */
            for (int i = 0; i < elementList.Count; i++)
            {
                sortChildElement(sortOptions, elementList[i]);
            }

            /* Sceneを再描画する */
            scene.Redraw();

            if(checkBoxNotShowComplete.Checked != true)
            {
                MessageBox.Show("並び替えしました。");
            }
        }
        #endregion イベント
    }
}
