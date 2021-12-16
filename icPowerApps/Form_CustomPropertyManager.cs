using interop.ICApiIronCAD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AdvancedDataGridView;
using System.Threading.Tasks;
using ICApiAddin.icPowerApps.Properties;
using System.Reflection;
using System.Diagnostics;

namespace ICApiAddin.icPowerApps
{
    public partial class Form_CustomPropertyManager : Form
   {
        private string g_IronCadSceneFilePath = string.Empty;
        private IZBaseApp _ironcadApp = null;
        private IZDoc _doc = null;
        private IZSceneDoc _sceneDoc = null;
        private string _userId = "dummyUser";

        /// <summary>
        /// バルク削除のラベル(選択しているパーツ名)を更新する
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        private void setLabelBulkDelete(int columnIndex, int rowIndex)
        {
            if((columnIndex != -1) && 
                (treeGridViewScene.Columns[columnIndex].Name != null) &&
                (treeGridViewScene.Columns[columnIndex].Tag != null) &&
                (treeGridViewScene.Columns[columnIndex].Tag.ToString().Contains("CustomProperty") == true))
            {
                /* 選択しているカスタムプロパティを全てのパーツアセンブリから削除のラベルを更新 */
                bool isShared = getColumnCustomPropertyIsShared(columnIndex);
                string deleteCustomProperty = string.Format("{0} \n{1}", treeGridViewScene.Columns[columnIndex].HeaderText, icapiCommon.convertIsSharedBoolToString(isShared));
                labelDeleteCustomProperty.Text = deleteCustomProperty;
            }
            else
            {
                labelDeleteCustomProperty.Text = string.Empty;
            }


            if ((rowIndex != -1) &&
                (treeGridViewScene["Scene", rowIndex].Value != null)&&
                (treeGridViewScene["AssemblyPartsSystemName", rowIndex].Value != null) &&
                (string.IsNullOrEmpty(treeGridViewScene["AssemblyPartsSystemName", rowIndex].Value.ToString()) != true))
            {
                /* パーツアセンブリのカスタムプロパティ全削除のラベルを更新 */
                string deletePartsAssembly = string.Format("{0} [{1}]", treeGridViewScene["Scene", rowIndex].Value.ToString(), treeGridViewScene["AssemblyPartsSystemName", rowIndex].Value.ToString());
                labelDeletePartsAssembly.Text = deletePartsAssembly;
            }else
            {
                labelDeletePartsAssembly.Text = string.Empty;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ironcadApp"></param>
        public Form_CustomPropertyManager(IZBaseApp ironcadApp)
        {
            InitializeComponent();
            comboBoxScope.SelectedIndex = 1;
            comboBoxDataType.SelectedIndex = 0;
            Initialize(ironcadApp);
        }

        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_InputCustomProperties_Load(object sender, EventArgs e)
        {
            splitContainerCustomProperty.Panel1Collapsed = true;
            if (string.IsNullOrEmpty(this._userId) != true)
            {
                this.buttonCustomPropertyCollectiveSetting.Visible = true;
                string settingFilePath = getCustomPropertyTemplateSettingFilePath();
                initializeCustomPropertyTemplate(settingFilePath);
            }
            else
            {
                this.buttonCustomPropertyCollectiveSetting.Visible = false;
            }
        }

        /// <summary>
        /// フォーム表示 イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form_CheckIn_Shown(object sender, EventArgs e)
        {
            if (this._ironcadApp == null)
            {
                MessageBox.Show("IRONCADと連携する機能のため、スタンドアロン起動では使用できません。");
                return;
            }
            setProgressBar(true);
            icapiCommon.enableDataGridViewDoubleBuffer(treeGridViewScene);
            ScaleReziser.InitializeFormControlScale(this, true, false, true, false, false);
            await ReloadAssemblyPartsCustomProperties();
            setProgressBar(false);
        }


        /// <summary>
        /// フォーム Closingイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_CustomPropertyManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            comboBoxCustomPropertyName.Focus();
            treeGridViewScene.ColumnHeaderMouseClick -= treeGridViewScene_ColumnHeaderMouseClick;
            treeGridViewScene.ColumnHeaderMouseDoubleClick -= treeGridViewScene_ColumnHeaderMouseDoubleClick;
            treeGridViewScene.CellEnter -= treeGridViewScene_CellEnter;
            treeGridViewScene.CellEndEdit -= treeGridViewScene_CellEndEdit;
        }


        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="ironcadApp"></param>
        private void Initialize(IZBaseApp ironcadApp)
        {
            /* アプリケーション情報を保持 */
            this._ironcadApp = ironcadApp;
            if(this._ironcadApp == null)
            {
                return;
            }

            /* 現在のドキュメントを取得し保持 */
            this._doc = ironcadApp.ActiveDoc;
            if (this._doc == null)
            {
                /* ドキュメントが取得できなかった */
                MessageBox.Show("現在のシーンが取得できませんでした。");
                this.Close();
                return;
            }
            /* ドキュメントのシーンデータを取得し保持 */
            this._sceneDoc = this._doc as IZSceneDoc;

            /* TreeGridViewのツリーの線(親子線)を表示 */
            treeGridViewScene.ShowLines = true;

            /* ImageListの作成と設定 */
            ImageList imageList = new ImageList();
            icapiCommon.getImageListAssemblyParts(new Size(18, 18), ref imageList);
            treeGridViewScene.ImageList = imageList;
            return;
        }


        /// <summary>
        /// 表示しているデータをリロードする
        /// </summary>
        private async Task<bool> ReloadAssemblyPartsCustomProperties()
        {
            /* 一括削除設定を初期化 */
            setLabelBulkDelete(-1, -1);

            /* TreeGridViewSceneを初期化する */
            initializeTreeGridViewScene();

            /* パーツ/アセンブリ情報を取得しTreeGridViewSceneに設定する */
            await setAssemblyPartsToTreeGridViewScene();
         
            /* ノードを全て展開状態にする */
            TreeGridNodeCollection nodes = treeGridViewScene.Nodes;
            icapiCommon.ExpandTreeGridViewTreeNodes(ref nodes);

            /* カスタムプロパティを取得し表示する */
            setCustomPropertiesToTreeGridViewScene();

            return true;
        }

        /// <summary>
        /// TreeGridViewSceneのデータを初期化する(InitializeComponent以降で作成されたデータを初期化)
        /// </summary>
        private void initializeTreeGridViewScene()
        {
            treeGridViewScene.ClearSelection();
            treeGridViewScene.Nodes.Clear();

            /* カスタムプロパティ列がある場合はクリアする */
            List<string> deleteColumns = new List<string>();
            for (int columnIndex = 0; columnIndex < treeGridViewScene.Columns.Count; columnIndex++)
            {
                /* Tag情報をチェック */
                if (treeGridViewScene.Columns[columnIndex].Tag == null)
                {
                    /* Tagがない */
                    continue;
                }
                /* タグの値を取得 */
                string tagName = treeGridViewScene.Columns[columnIndex].Tag.ToString();
                if (tagName.Contains("CustomProperty") == true)
                {
                    /* タグがCustomProperty*** であればカスタムプロパティ列に該当。削除対象に追加 */
                    deleteColumns.Add(treeGridViewScene.Columns[columnIndex].Name);
                }
            }

            /* 削除対象の列を削除する */
            foreach (string columnName in deleteColumns)
            {
                treeGridViewScene.Columns.Remove(columnName);
            }
        }

        /// <summary>
        /// アセンブリ/パーツのカスタムプロパティをTreeGridViewに設定する
        /// </summary>
        private void setCustomPropertiesToTreeGridViewScene()
        {
            for (int rowIndex = 0; rowIndex < treeGridViewScene.Rows.Count; rowIndex++)
            {
                /* ElementIDを取得する */
                string elementID = treeGridViewScene["ElementID", rowIndex].Value.ToString();
                if (string.IsNullOrEmpty(elementID) == true)
                {
                    /* ElementIDがない(トップシーン) */
                    continue;
                }

                /* ElementIDからElementを取得する */
                IZSceneDocUtility util = this._sceneDoc as IZSceneDocUtility;
                IZElement elem = util.GetElementById(Int32.Parse(elementID));
                if(elem == null)
                {
                    MessageBox.Show("Elementを取得できませんでした。" + string.Format("ElementID: {0}", elementID));
                    continue;
                }

                /* 当該Elementのカスタムプロパティデータを取得する */
                List<icapiCommon.CustomProperty> customProperties = new List<icapiCommon.CustomProperty>();
                icapiCommon.GetCustomProperties(elem, ref customProperties);

                /* カスタムプロパティをTreeGridViewに反映する */
                for(int propIndex = 0; propIndex < customProperties.Count; propIndex++)
                {
                    string name = customProperties[propIndex].name;
                    object value = customProperties[propIndex].value;
                    bool isShared = customProperties[propIndex].scopeIsShared;
                    string columnName = createColumnName(name, isShared);    /* 列名は範囲情報を付加する */

                    /* 既に列[カスタムプロパティ名+_xxxShared]があるかチェック */
                    if (treeGridViewScene.Columns.Contains(columnName) == false)
                    {
                        /* 列にないカスタムプロパティは新規に列を追加 */
                        AddCustomPropertyColumn(columnName, name, isShared);
                    }

                    /* プロパティ値を反映する */
                    treeGridViewScene[columnName, rowIndex].Value = value;
                }
            }
        }

        /// <summary>
        /// 範囲情報を含んだプロパティ列名を作成する
        /// </summary>
        /// <param name="name">プロパティ名</param>
        /// <param name="isShared">範囲</param>
        /// <returns></returns>
        private string createColumnName(string name, bool isShared)
        {
            string columnName = name + (isShared ? "_Shared" : "_notShared");
            return columnName;
        }


        /// <summary>
        /// アセンブリ/パーツ情報をTreeGridViewSceneに設定する
        /// </summary>
        /// <returns></returns>
        private async Task<bool> setAssemblyPartsToTreeGridViewScene()
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

            bool ret = await Task.Run(() => {
                icapiCommon.GetSceneTreeInfo(icapiCommon.CREATE_TREE_MODE.CHECK_IN, TreeNodeHeight, sceneDoc.GetTopElement(), ref topNode, ref depth);
                return true;
            });


            return true;
        }

        #region イベント
        private DataTable _customPropertyTemplateDataTable = new DataTable();
        /// <summary>
        /// カスタムプロパティのプロパティ名を初期設定
        /// </summary>
        private bool initializeCustomPropertyTemplate(string filePath)
        {
            DataTable dataTable = new DataTable();
            //icVaultDB icVaultdb = new icVaultDB();
            GetCustomPropertyNameMaster(filePath, ref dataTable);
            this._customPropertyTemplateDataTable = dataTable;

            // コンボボックスにデータテーブルをセット
            comboBoxCustomPropertyName.DataSource = dataTable;
            comboBoxCustomPropertyName.DisplayMember = "CustomPropertyName";
            comboBoxCustomPropertyName.ValueMember = "CustomPropertyName";
            comboBoxCustomPropertyName.SelectedIndex = -1;


            /* ユニークなTemplate名を取得しコンボボックスに設定 */
            DataTable customPropertyTemplateDataTable = this._customPropertyTemplateDataTable.DefaultView.ToTable(true, "CustomPropertyTemplateName");
            DataRow dr = customPropertyTemplateDataTable.NewRow();
            dr["CustomPropertyTemplateName"] = "[テンプレートを選択]";
            customPropertyTemplateDataTable.Rows.InsertAt(dr, 0);

            comboBoxCustomPropertyTemplate.DataSource = customPropertyTemplateDataTable;
            comboBoxCustomPropertyTemplate.DisplayMember = "CustomPropertyTemplateName";
            comboBoxCustomPropertyTemplate.ValueMember = "CustomPropertyTemplateName";
            if (comboBoxCustomPropertyTemplate.Items.Count > 0)
            {
                comboBoxCustomPropertyTemplate.SelectedIndex = 0;
            }
            return true;
        }

        private void GetCustomPropertyNameMaster(string filePath, ref DataTable dt)
        {
            ReadCustomPropertyTemplateSetting(filePath, ref dt);
        }
        
        #endregion

        /// <summary>
        /// 新規追加/変更ボタンのクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonAddEdit_Click(object sender, EventArgs e)
        {
            /* 新規追加と変更で処理を分ける */
            if(buttonAddEdit.Text == "新規追加")
            {
                /****************/
                /* 新規追加処理 */
                /****************/
                bool isShared = false;

                /* 新規追加するカスタムプロパティ名を取得 */
                string name = comboBoxCustomPropertyName.Text;
                if(string.IsNullOrEmpty(name) == true)
                {
                    /* カスタムプロパティ名が空白 */
                    MessageBox.Show("カスタムプロパティ名が入力されていません。");
                    return;
                }

                /* 作成するカスタムプロパティの範囲設定を取得 */
                if (string.Equals(comboBoxScope.Text, "このパーツ/アセンブリのみ") == true)
                {
                    /* このパーツ/アセンブリのみ */
                    isShared = false;
                }else
                {
                    /* すべてのリンクインスタンス */
                    isShared = true;
                }

                /* すでにカスタムプロパティの列がないかチェック */
                string columnName = createColumnName(name, isShared);
                if(treeGridViewScene.Columns.Contains(columnName) == true)
                {
                    /* すでに新規追加するカスタムプロパティの列あり */
                    MessageBox.Show(string.Format("プロパティ名: {0} は既に一覧に存在します。\n値を設定する場合は、該当プロパティ名のセルに直接入力してください。", name));
                    return;
                }
                
                /* カスタムプロパティの列を追加する */
                AddCustomPropertyColumn(columnName, name, isShared);
            }
            else if (buttonAddEdit.Text == "変更")
            {
                /**********************************/
                /* カスタムプロパティ名の変更処理 */
                /**********************************/
                bool changeIsShared = false;
                bool editIsShared = false;
                /* 変更する列が設定されているかチェック */
                if (this._editColumns < 0)
                {
                    /* 設定されていない */
                    MessageBox.Show("変更するカスタムプロパティが選択されていません。\n再度変更する列をダブルクリックしてください。");
                    return;
                }

                /* 変更後のカスタムプロパティ名を取得 */
                string changePropName = comboBoxCustomPropertyName.Text;
                if (string.IsNullOrEmpty(changePropName) == true)
                {
                    /* 変更後のカスタムプロパティ名が設定されていない */
                    MessageBox.Show("変更後のカスタムプロパティ名が入力されていません");
                    return;
                }

                /* 変更するカスタムプロパティの範囲設定を取得 */
                if (string.Equals(comboBoxScope.Text, "このパーツ/アセンブリのみ") == true)
                {
                    /* このパーツ/アセンブリのみ */
                    changeIsShared = false;
                }
                else
                {
                    /* すべてのリンクインスタンス */
                    changeIsShared = true;
                }
                editIsShared = getColumnCustomPropertyIsShared(this._editColumns);


                /* すでにカスタムプロパティの列がないかチェック */
                DialogResult dret = DialogResult.No;
                string currPropName = string.Empty;
                string currEditColumnName = treeGridViewScene.Columns[this._editColumns].Name;
                string changeColumnName = createColumnName(changePropName, changeIsShared);

                /* 名前が同じになっていないかチェック */
                if (string.Equals(currEditColumnName, changeColumnName) == true)
                {
                    /* 名前が変更されていない */
                    MessageBox.Show("変更前と変更後のカスタムプロパティ名/プロパティ範囲が同じです。");
                    return;
                }

                if (treeGridViewScene.Columns.Contains(changeColumnName) == true)
                {
                    /* すでに変更後のカスタムプロパティの列あり */
                    dret = MessageBox.Show(string.Format("プロパティ名: {0} プロパティ範囲: {1} は\n既に一覧に存在します。\n変更前のプロパティで値が設定されているデータを\n変更後のプロパティに上書きしますか？", changePropName, comboBoxScope.Text),"確認",MessageBoxButtons.YesNo);
                    if(dret == DialogResult.Yes)
                    {
                        /* カスタムプロパティ名を変更する */
                        currPropName = treeGridViewScene.Columns[this._editColumns].HeaderText;

                        /* 値が設定されているデータは変更後のプロパティに上書きする */
                        /* 変更処理(当該列のデータをなめる) */
                        for (int rowIndex = 0; rowIndex < treeGridViewScene.Rows.Count; rowIndex++)
                        {
                            /* 値が設定されていない場合は次のデータに移る */
                            if (treeGridViewScene[this._editColumns, rowIndex].Value == null)
                            {
                                continue;
                            }
                            /* 値が設定されていない場合は次のデータに移る */
                            string currDataStr = treeGridViewScene[this._editColumns, rowIndex].Value.ToString();
                            if (string.IsNullOrEmpty(currDataStr) == true)
                            {
                                continue;
                            }

                            /* 設定されている値を取得する */
                            object objValue = treeGridViewScene[this._editColumns, rowIndex].Value;

                            /* 当該データのElementを取得する */
                            IZElement elem = getElementFromTreeGridViewRow(rowIndex);
                            if (elem == null)
                            {
                                MessageBox.Show("データのElementを取得できませんでした。" + string.Format("EditColumn: {0},  rowIndex: {1}", this._editColumns, rowIndex));
                                continue;
                            }

                            /* カスタムプロパティのリネームは、削除して新規登録する */
                            icapiCommon.DeleteCustomProperty(elem, currPropName, editIsShared);
                            icapiCommon.EditCustomProperty(elem, changePropName, objValue, changeIsShared);
                        }
                        /* 列の削除が発生するのでReloadさせる */
                        await ReloadAssemblyPartsCustomProperties();

                        MessageBox.Show("カスタムプロパティ名/プロパティ範囲を変更しました。");
                        ButtonAddEditChange(false, -1);
                    }
                    return;
                }


                /* カスタムプロパティ名を変更する */
                currPropName = treeGridViewScene.Columns[this._editColumns].HeaderText;
                dret = MessageBox.Show(string.Format("プロパティ名/範囲を変更しますか？\n 変更前: {0} : {1}\n 変更後: {2} : {3}", currPropName, icapiCommon.convertIsSharedBoolToString(editIsShared), changePropName, comboBoxScope.Text), "確認", MessageBoxButtons.YesNo);
                if (dret == DialogResult.Yes)
                {
                    /* 変更処理(当該列のデータをなめる) */
                    for (int rowIndex = 0; rowIndex < treeGridViewScene.Rows.Count; rowIndex++)
                    {
                        /* 値が設定されていない場合は次のデータに移る */
                        if (treeGridViewScene[this._editColumns, rowIndex].Value == null)
                        {
                            continue;
                        }
                        /* 値が設定されていない場合は次のデータに移る */
                        string currDataStr = treeGridViewScene[this._editColumns, rowIndex].Value.ToString();
                        if (string.IsNullOrEmpty(currDataStr) == true)
                        {
                            continue;
                        }

                        /* 設定されている値を取得する */
                        object objValue = treeGridViewScene[this._editColumns, rowIndex].Value;

                        /* 当該データのElementを取得する */
                        IZElement elem = getElementFromTreeGridViewRow(rowIndex);
                        if (elem == null)
                        {
                            MessageBox.Show("データのElementを取得できませんでした。" + string.Format("EditColumn: {0},  rowIndex: {1}", this._editColumns, rowIndex));
                            continue;
                        }

                        /* カスタムプロパティのリネームは、削除して新規登録する */
                        icapiCommon.DeleteCustomProperty(elem, currPropName, editIsShared);
                        icapiCommon.EditCustomProperty(elem, changePropName, objValue, changeIsShared);


                        /* datagridviewを更新(リンクされているデータを変更) */
                        if (changeIsShared == true)
                        {
                            IZSceneElement sceneElem = elem as IZSceneElement;
                            object[] elems = sceneElem.GetInternallyLinkedElements();
                            for (int index = 0; index < elems.Count(); index++)
                            {
                                IZElement sameElem = elems[index] as IZElement;
                                string targetElemId = sameElem.Id.ToString();
                                for (int j = 0; j < treeGridViewScene.Rows.Count; j++)
                                {
                                    string currRowElemId = icapiCommon.treeGridViewValueGet(treeGridViewScene, j, "ElementID");
                                    if (string.Equals(targetElemId, currRowElemId) == true)
                                    {
                                        treeGridViewScene[this._editColumns, j].Value = objValue;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    /* ヘッダを書き換える */
                    treeGridViewScene.Columns[this._editColumns].Name = changeColumnName;
                    treeGridViewScene.Columns[this._editColumns].HeaderText = changePropName;

                    if (changeIsShared == true)
                    {
                        /* すべてのリンクインスタンス */
                        treeGridViewScene.Columns[this._editColumns].Tag = "CustomPropertyShared";
                        treeGridViewScene.Columns[this._editColumns].HeaderCell.Style.BackColor = Color.LightSkyBlue;
                    }
                    else
                    {
                        /* このパーツアセンブリのみ */
                        treeGridViewScene.Columns[this._editColumns].Tag = "CustomPropertyNotShared";
                        treeGridViewScene.Columns[this._editColumns].HeaderCell.Style.BackColor = Color.LightBlue;
                    }

                    MessageBox.Show("カスタムプロパティ名/プロパティ範囲を変更しました。");
                    ButtonAddEditChange(false, -1);
                }
            }

        }
        

        /// <summary>
        /// 指定された名前の列を追加して列indexを取得する/（列がすでにある場合は作成せずにindexを返す）
        /// </summary>
        /// <param name="name">列名</param>
        /// <returns>列のインデックス</returns>
        private int AddCustomPropertyColumn(string name, string headerName, bool isShared)
        {
            int index = -1;
            /* 作成する列名が空白でないかチェック */
            if (string.IsNullOrEmpty(name) == true)
            {
                return -1;
            }
            /* すでに列があるかチェック */
            if (treeGridViewScene.Columns.Contains(name) == true)
            {
                /* 列がある */
                index = treeGridViewScene.Columns[name].Index;
            }
            else
            {
                /* 列がない(新規列を作成) */
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                column.Name = name;
                column.HeaderText = headerName;
                Font font = column.DefaultCellStyle.Font;
                if(isShared == true)
                {
                    /* すべてのリンクインスタンス */
                    column.Tag = "CustomPropertyShared";
                    column.HeaderCell.Style.BackColor = Color.LightSkyBlue;
                }
                else
                {
                    /* このパーツアセンブリのみ */
                    column.Tag = "CustomPropertyNotShared";
                    column.HeaderCell.Style.BackColor = Color.LightBlue;
                }
                treeGridViewScene.Columns.Add(column);
                index = treeGridViewScene.Columns.Count-1;
                treeGridViewScene.Columns[index].HeaderCell.Style.Font = new Font(new Font(treeGridViewScene.Font.Name, 10), treeGridViewScene.Font.Style | FontStyle.Bold);
            }
            return index;
        }

        private bool isDeletingColumn = false;

        /// <summary>
        /// TreeGridViewSceneのセル編集完了 イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeGridViewScene_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool showMessage = true;
            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;
            string dataType = this.comboBoxDataType.Text;

            /* 入力データを指定した型にパースする */
            parseInputDataToDataType(columnIndex, rowIndex, dataType);

            setCustomProperty(columnIndex, rowIndex, showMessage);

            /* 当該カスタムプロパティが全てのパーツアセンブリで空白/無しの場合は削除する */
            checkDeleteAllEmptyCustomProperty(columnIndex);

            /* 削除機能に現在の選択セル情報を反映 */
            setLabelBulkDelete(columnIndex, rowIndex);
        }

        /// <summary>
        /// 指定したセル（行・列）のデータをカスタムプロパティとして登録する
        /// (同じパーツ・アセンブリのTreeGridViewSceneのセルも更新する)
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="showMessage"></param>
        private void setCustomProperty(int columnIndex, int rowIndex, bool showMessage)
        {
            if (isDeletingColumn == true)
            {
                isDeletingColumn = false;
                return;
            }
            int editCellColumnIndex = -1;
            int editCellRowIndex = -1;
            editCellColumnIndex = columnIndex;
            editCellRowIndex = rowIndex;

            IZElement elem = getElementFromTreeGridViewRow(editCellRowIndex);
            if (elem == null)
            {
                /* Elementが取得できない */
                if ((treeGridViewScene[editCellColumnIndex, editCellRowIndex].Value == null) ||
                    (string.IsNullOrEmpty(treeGridViewScene[editCellColumnIndex, editCellRowIndex].Value.ToString()) == true))
                {
                    return;
                }
                if (showMessage == true)
                {
                    MessageBox.Show("カスタムプロパティを入力できないパーツ/アセンブリです。");
                }
                treeGridViewScene[editCellColumnIndex, editCellRowIndex].Value = string.Empty;
                return;
            }
            object value = treeGridViewScene[editCellColumnIndex, editCellRowIndex].Value;
            string columnName = treeGridViewScene.Columns[editCellColumnIndex].Name;
            string propName = treeGridViewScene.Columns[editCellColumnIndex].HeaderText;
            bool isShared = getColumnCustomPropertyIsShared(editCellColumnIndex);

            /* カスタムプロパティをパーツ/アセンブリに設定する */
            icapiCommon.EditCustomProperty(elem, propName, value, isShared);

            /* datagridviewを更新(リンクされているデータを変更) */
            if (isShared == true)
            {
                IZSceneElement sceneElem = elem as IZSceneElement;
                object[] elems = sceneElem.GetInternallyLinkedElements();
                for (int index = 0; index < elems.Count(); index++)
                {
                    IZElement sameElem = elems[index] as IZElement;
                    string targetElemId = sameElem.Id.ToString();
                    for (int row = 0; row < treeGridViewScene.Rows.Count; row++)
                    {
                        string currRowElemId = icapiCommon.treeGridViewValueGet(treeGridViewScene, row, "ElementID");
                        if (string.Equals(targetElemId, currRowElemId) == true)
                        {
                            treeGridViewScene[columnName, row].Value = value;
                            break;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 指定した列のカスタムプロパティが全て空(未設定)であるかチェックし、不要であれば削除する
        /// </summary>
        /// <param name="columnIndex"></param>
        private void checkDeleteAllEmptyCustomProperty(int columnIndex)
        {
            string columnName = treeGridViewScene.Columns[columnIndex].Name;

            /* すべての列が空白/無しであれば削除する */
            bool isCustomPropertiesAllEmpty = true;
            for (int index = 0; index < treeGridViewScene.Rows.Count; index++)
            {
                string currRowValue = icapiCommon.treeGridViewValueGet(treeGridViewScene, index, columnName);
                if (string.IsNullOrEmpty(currRowValue) != true)
                {
                    isCustomPropertiesAllEmpty = false;
                    break;
                }
            }

            if (isCustomPropertiesAllEmpty == true)
            {
                DialogResult dret = MessageBox.Show("現在のカスタムプロパティの表示を削除しますか？", "確認", MessageBoxButtons.YesNo);
                if (dret == DialogResult.Yes)
                {
                    isDeletingColumn = true;
                    treeGridViewScene.CurrentCell.Selected = false;
                    this.BeginInvoke(new Action(() =>
                    {
                        treeGridViewScene.Columns.Remove(columnName);
                    }));
                }
            }
        }


        /// <summary>
        /// 選択した行のアセンブリ/パーツElementを取得する
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private IZElement getElementFromTreeGridViewRow(int rowIndex)
        {
            string elemId = icapiCommon.treeGridViewValueGet(treeGridViewScene, rowIndex, "ElementID");
            if(string.IsNullOrEmpty(elemId) == true)
            {
                return null;
            }
            IZSceneDocUtility util = this._doc as IZSceneDocUtility;
            return util.GetElementById(Int32.Parse(elemId));
        }


        /// <summary>
        /// TreeGridViewSceneのセル選択 イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeGridViewScene_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (treeGridViewScene.Columns[e.ColumnIndex].Tag == null)
            {
                setLabelBulkDelete(-1, e.RowIndex);
                return;
            }
            string columnTag = treeGridViewScene.Columns[e.ColumnIndex].Tag.ToString();
            if (columnTag.Contains("CustomProperty") == false)
            {
                setLabelBulkDelete(-1, e.RowIndex);
                return;
            }
            if(treeGridViewScene[e.ColumnIndex, e.RowIndex].Value == null)
            {
                comboBoxDataType.Text = icapiCommon.getCustomPropertyDataTypeString(typeof(string));
            }
            else
            {
                Type dataType = treeGridViewScene[e.ColumnIndex, e.RowIndex].Value.GetType();
                comboBoxDataType.Text = icapiCommon.getCustomPropertyDataTypeString(dataType);
            }
            ButtonAddEditChange(false, -1);
            setLabelBulkDelete(e.ColumnIndex, e.RowIndex);

        }


        /// <summary>
        /// ComboBoxScopeに範囲を設定する
        /// </summary>
        /// <param name="isShared"></param>
        private void setComboBoxScope(bool isShared)
        {
            if (isShared == true)
            {
                /* リンクインスタンスすべてに適用されるカスタムプロパティ */
                comboBoxScope.Text = "すべてのリンク インスタンス";
            }
            else
            {
                /* このパーツ/アセンブリのみに適用されるカスタムプロパティ */
                comboBoxScope.Text = "このパーツ/アセンブリのみ";
            }
        }


        /// <summary>
        /// ComboBoxScopeの選択変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(treeGridViewScene.CurrentCell == null)
            {
                return;
            }
            int rowIndex = treeGridViewScene.CurrentCell.RowIndex;
            int columnIndex = treeGridViewScene.CurrentCell.ColumnIndex;
            parseInputDataToDataType(columnIndex, rowIndex, comboBoxDataType.Text);
            setCustomProperty(columnIndex, rowIndex, false);
            if (treeGridViewScene[columnIndex, rowIndex].Value == null)
            {
                comboBoxDataType.Text = Resources.CustomPropertiesDataType_Text;
            }
            else
            {
                comboBoxDataType.Text = icapiCommon.getCustomPropertyDataTypeString(treeGridViewScene[columnIndex, rowIndex].Value.GetType());
            }
        }


        /// <summary>
        /// 入力されたデータを指定した型に変換し設定する
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        private bool parseInputDataToDataType(int columnIndex, int rowIndex, string dataType)
        {
            bool retParse = false;
            if (treeGridViewScene[columnIndex, rowIndex].Value == null)
            {
                return false;
            }
            IZElement elem = getElementFromTreeGridViewRow(rowIndex);
            if (elem == null)
            {
                return false;
            }
            string currDataStr = treeGridViewScene[columnIndex, rowIndex].Value.ToString();
            Type currDataType = treeGridViewScene[columnIndex, rowIndex].Value.GetType();
            object setObject = null;
            switch (dataType)
            {
                case "日付":
                    DateTime dateTimeData = DateTime.Now;
                    retParse = DateTime.TryParse(currDataStr, out dateTimeData);
                    if (retParse == true)
                    {
                        setObject = dateTimeData;
                    }
                    break;
                case "はい/いいえ (True/False)":
                case "はい/いいえ":
                    if (string.Equals(currDataStr, "はい") == true)
                    {
                        retParse = true;
                        setObject = true;
                    }
                    else if (string.Equals(currDataStr, "いいえ") == true)
                    {
                        retParse = true;
                        setObject = false;
                    }
                    else
                    {
                        bool boolData = false;
                        retParse = bool.TryParse(currDataStr, out boolData);
                        if (retParse == true)
                        {
                            setObject = boolData;
                        }
                    }
                    break;
                case "数値":
                    double doubleData = 0.0;
                    retParse = double.TryParse(currDataStr, out doubleData);
                    if (retParse == true)
                    {
                        setObject = doubleData;
                    }
                    break;
                case "テキスト":
                    setObject = currDataStr;
                    retParse = true;
                    break;
                default:
                    break;
            }
            if (retParse == true)
            {
                treeGridViewScene[columnIndex, rowIndex].Value = setObject;
            }
            else
            {
                treeGridViewScene[columnIndex, rowIndex].Value = currDataStr;　/* 入力されたデータそのままを設定(本当は不要なコードだが明示的にしておく) */
                MessageBox.Show("指定した型へ変換できませんでした。入力形式を確認してください。\n指定した形式へ変換できなかったため入力データはテキストに変換されました。");
            }
            return retParse;
        }


        /// <summary>
        /// 指定された列のカスタムプロパティの範囲を取得する
        /// ・このパーツのみ(false)
        /// ・すべてのリンクインスタンス(true)
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private bool getColumnCustomPropertyIsShared(int columnIndex)
        {
            bool isShared = false;
            if(treeGridViewScene.Columns[columnIndex].Tag == null)
            {
                /* 取得できなかった(値は無効だがこのパーツ/アセンブリのみを返す) */
                return false;
            }
            string columnTag = treeGridViewScene.Columns[columnIndex].Tag.ToString();
            if (string.Equals(columnTag, "CustomPropertyShared") == true)
            {
                isShared = true;
            }
            else
            {
                isShared = false;
            }
            return isShared;
        }

        /// <summary>
        /// 削除を実行ボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonDelete_Click(object sender, EventArgs e)
        {
            /**************************************************************************/
            /*  選択しているパーツ/アセンブリのカスタムプロパティをすべて削除します。 */
            /**************************************************************************/
            if (radioButtonDeletePartsAssemblyCustomProperties.Checked == true)
            {
                if (string.IsNullOrEmpty(labelDeletePartsAssembly.Text) == true)
                {
                    MessageBox.Show("パーツ/アセンブリが選択されていません。");
                    return;
                }
                int rowIndex = treeGridViewScene.CurrentCell.RowIndex;
                IZElement elem = getElementFromTreeGridViewRow(rowIndex);
                if (elem == null)
                {
                    MessageBox.Show("パーツ/アセンブリが選択されていません。");
                    return;
                }
                string name = elem.Name;
                DialogResult dret = MessageBox.Show(string.Format("このパーツ/アセンブリのカスタムプロパティをすべて削除しますか？\n【パーツ/アセンブリ名: {0}   システム名:{1}】", name, elem.SystemName), "確認", MessageBoxButtons.YesNo);
                if (dret == DialogResult.Yes)
                {
                    icapiCommon.DeleteAllCustomProperties(elem);
                    MessageBox.Show("削除しました。");
                    await ReloadAssemblyPartsCustomProperties();
                }
            }

            /****************************************************************************/
            /*  選択しているカスタムプロパティを全てのパーツ/アセンブリから削除します。 */
            /****************************************************************************/
            if (radioButtonDeleteCustomPropertyName.Checked == true)
            {
                if (string.IsNullOrEmpty(labelDeleteCustomProperty.Text) == true)
                {
                    MessageBox.Show("カスタムプロパティが選択されていません。");
                    return;
                }
                int columnIndex = treeGridViewScene.CurrentCell.ColumnIndex;
                string name = treeGridViewScene.Columns[columnIndex].HeaderText;
                if((string.IsNullOrEmpty(name) == true) || (treeGridViewScene.Columns[columnIndex].Tag == null))
                {
                    MessageBox.Show("カスタムプロパティが選択されていません。");
                    return;
                }

                string columnTag = treeGridViewScene.Columns[columnIndex].Tag.ToString();
                if (columnTag.Contains("CustomProperty") == false)
                {
                    MessageBox.Show("カスタムプロパティが選択されていません。");
                    return;
                }
                bool isShared = getColumnCustomPropertyIsShared(columnIndex);

                DialogResult dret = MessageBox.Show(string.Format("このカスタムプロパティを全てのパーツ/アセンブリから削除しますか？\n【名前: {0}   範囲: {1} 】", name, icapiCommon.convertIsSharedBoolToString(isShared)), "確認", MessageBoxButtons.YesNo);
                if (dret == DialogResult.Yes)
                {
                    for (int rowIndex = 0; rowIndex < treeGridViewScene.Rows.Count; rowIndex++)
                    {
                        IZElement elem = getElementFromTreeGridViewRow(rowIndex);
                        if (elem == null)
                        {
                            continue;
                        }
                        icapiCommon.DeleteCustomProperty(elem, name, isShared);
                        treeGridViewScene[columnIndex, rowIndex].Value = null;
                    }
                    MessageBox.Show("削除しました。");
                    await ReloadAssemblyPartsCustomProperties();
                }
            }

            /************************************************************************/
            /*  シーンの全パーツ/アセンブリのカスタムプロパティをすべて削除します。 */
            /************************************************************************/
            if (radioButtonDeleteAllPartsAssemblyCustomProperties.Checked == true)
            {
                DialogResult dret = MessageBox.Show("全パーツ/アセンブリのカスタムプロパティをすべて削除します。\nよろしいですか？", "確認", MessageBoxButtons.YesNo);
                if(dret == DialogResult.Yes)
                {
                    for (int rowIndex = 0; rowIndex < treeGridViewScene.Rows.Count; rowIndex++)
                    {
                        IZElement elem = getElementFromTreeGridViewRow(rowIndex);
                        if (elem == null)
                        {
                            continue;
                        }
                        icapiCommon.DeleteAllCustomProperties(elem);
                    }
                    MessageBox.Show("削除しました。");
                    await ReloadAssemblyPartsCustomProperties();
                }
            }
        }

        /// <summary>
        /// 列ヘッダの色を再設定する
        /// </summary>
        private void ResetCustomPropertyHeaderColor()
        {
            for (int columnIndex = 0; columnIndex < treeGridViewScene.Columns.Count; columnIndex++)
            {
                if (treeGridViewScene.Columns[columnIndex].Tag == null)
                {
                    continue;
                }
                string columnTag = treeGridViewScene.Columns[columnIndex].Tag.ToString();
                if (string.Equals(columnTag, "CustomPropertyShared") == true)
                {
                    treeGridViewScene.Columns[columnIndex].HeaderCell.Style.BackColor = Color.LightSkyBlue;
                }
                if (string.Equals(columnTag, "CustomPropertyNotShared") == true)
                {
                    treeGridViewScene.Columns[columnIndex].HeaderCell.Style.BackColor = Color.LightBlue;
                }
            }
        }

        /// <summary>
        /// 新規追加/変更ボタンのクリック イベント
        /// </summary>
        /// <param name="isEdit"></param>
        /// <param name="editColumnIndex"></param>
        private void ButtonAddEditChange(bool isEdit, int editColumnIndex)
        {
            if (isEdit == true)
            {
                /* 変更モード */
                comboBoxCustomPropertyName.Focus();
                if (string.Equals(buttonAddEdit.Text, "変更") == true)
                {
                    /* 既に変更モード(Headerの色付けをリセット) */
                    ResetCustomPropertyHeaderColor();
                }
                buttonAddEdit.BackColor = Color.LightYellow;
                buttonAddEdit.Text = "変更";
                treeGridViewScene.Columns[editColumnIndex].HeaderCell.Style.BackColor = Color.LightYellow;
                comboBoxCustomPropertyName.Text = treeGridViewScene.Columns[editColumnIndex].HeaderText;
                bool isShared = getColumnCustomPropertyIsShared(editColumnIndex);
                setComboBoxScope(isShared);
                this._editColumns = editColumnIndex;
            }
            else
            {
                /* 新規追加モードにする */
                if(string.Equals(buttonAddEdit.Text, "新規追加") == true)
                {
                    /* 既に新規追加モード */
                    return;
                }
                ResetCustomPropertyHeaderColor();
                buttonAddEdit.BackColor = SystemColors.Control;
                buttonAddEdit.Text = "新規追加";
                setComboBoxScope(true);
                this._editColumns = -1;
            }
        }


        private int _editColumns = -1;
        /// <summary>
        /// TreeGridViewSceneの列ヘッダダブルクリック イベント(カスタムプロパティ名の変更を受け付ける状態にする)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeGridViewScene_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (treeGridViewScene.Columns[e.ColumnIndex].Tag == null)
            {
                ButtonAddEditChange(false, -1);
                return;
            }
            string columnTag = treeGridViewScene.Columns[e.ColumnIndex].Tag.ToString();
            if (columnTag.Contains("CustomProperty") == true)
            {
                ButtonAddEditChange(true, e.ColumnIndex);
            }
            else
            {
                ButtonAddEditChange(false, -1);
            }
            setLabelBulkDelete(e.ColumnIndex, -1);
        }

        /// <summary>
        /// TreeGridViewSceneの列ヘッダクリック イベント(列選択の情報を設定する)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeGridViewScene_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            setLabelBulkDelete(e.ColumnIndex, -1);
        }

        /// <summary>
        /// テンプレートを表示のボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCustomPropertyCollectiveSetting_Click(object sender, EventArgs e)
        {
            if (buttonCustomPropertyCollectiveSetting.Text == "テンプレートを表示")
            {
                splitContainerCustomProperty.Panel1Collapsed = false;
                buttonCustomPropertyCollectiveSetting.Text = "テンプレートを隠す";
            }
            else
            {
                splitContainerCustomProperty.Panel1Collapsed = true;
                buttonCustomPropertyCollectiveSetting.Text = "テンプレートを表示";
            }
        }

        /// <summary>
        /// TreeGridViewSceneの選択している行を取得する
        /// </summary>
        /// <returns></returns>
        private List<int> getSelectedTreeGridViewRows()
        {
            List<int> rowIndexList = new List<int>();

            foreach (DataGridViewRow dr in treeGridViewScene.SelectedRows)
            {
                int rowIndex = dr.Index;
                if(rowIndexList.Contains(rowIndex) != true)
                {
                    rowIndexList.Add(rowIndex);
                }
            }
            foreach (DataGridViewCell cell in treeGridViewScene.SelectedCells)
            {
                int rowIndex = cell.RowIndex;
                if (rowIndexList.Contains(rowIndex) != true)
                {
                    rowIndexList.Add(rowIndex);
                }
            }
            return rowIndexList;
        }


        /// <summary>
        /// カスタムプロパティのデータ追加ボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSetTemplate_Click(object sender, EventArgs e)
        {
            bool warningValueIsEmpty = false;
            bool retSkipped = false;
            List<string> skippedList = new List<string>();
            string templateName = comboBoxCustomPropertyTemplate.SelectedValue.ToString();
            string customPropertyValue = string.Empty;
            /* 選択中のRowIndexリストを取得 */
            List<int> selectRowIndexList = getSelectedTreeGridViewRows();

            /* プロパティ名を追加 */
            foreach (object obj in checkedListBoxCustomPropertyNameList.CheckedItems)
            {
                string customPropertyName = obj.ToString();
                bool customPropertyScopeIsShared = false;
                string customPropertyDataType = string.Empty;
                string query = string.Format("CustomPropertyTemplateName = '{0}' AND CustomPropertyName = '{1}'", templateName, customPropertyName);
                DataRow[] drows = this._customPropertyTemplateDataTable.Select(query);
                foreach (DataRow dr in drows)
                {
                    customPropertyValue = icapiCommon.dataRowValueGet(dr, "CustomPropertyValue");
                    customPropertyDataType = icapiCommon.dataRowValueGet(dr, "CustomPropertyDataType");
                    customPropertyScopeIsShared = icapiCommon.dataRowBoolValueGet(dr, "CustomPropertyScopeIsShared");
                    break;
                }

                /* すでにカスタムプロパティの列がないかチェック */
                string columnName = createColumnName(customPropertyName, customPropertyScopeIsShared);
                if (treeGridViewScene.Columns.Contains(columnName) != true)
                {
                    /* カスタムプロパティの列を追加する */
                    AddCustomPropertyColumn(columnName, customPropertyName, customPropertyScopeIsShared);
                }

                /* 値を設定する */
                if(string.IsNullOrEmpty(customPropertyValue) != true)
                {
                    foreach (int setRowIndex in selectRowIndexList)
                    {
                        bool ShowMessage = false;
                        int columnIndex = treeGridViewScene.Columns[columnName].Index;
                        treeGridViewScene[columnName, setRowIndex].Value = customPropertyValue;
                        parseInputDataToDataType(columnIndex, setRowIndex, customPropertyDataType);
                        setCustomProperty(columnIndex, setRowIndex, ShowMessage);
                    }
                }
                else
                {
                    warningValueIsEmpty = true;
                }
            }

            if(warningValueIsEmpty == true)
            {
                MessageBox.Show("設定したカスタムプロパティの値に空白/なしのデータがあります。\n一時的に一覧表には出力されますが、実際のデータは入力するまでありませんのでご注意ください。");
            }
        }

        /// <summary>
        /// カスタムプロパティ テンプレートのデータ選択変更 イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBoxCustomPropertyNameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Color fontColor = SystemColors.ControlText;

            string customPropertyValue = string.Empty;
            string customPropertyName = checkedListBoxCustomPropertyNameList.Text;
            string templateName = comboBoxCustomPropertyTemplate.SelectedValue.ToString();
            string customPropertyDataType = string.Empty;
            bool customPropertyScopeIsShared = true;
            int intTemplateID = -1;
            string query = string.Format("CustomPropertyTemplateName = '{0}' AND CustomPropertyName = '{1}'", templateName, customPropertyName);
            DataRow[] drows = this._customPropertyTemplateDataTable.Select(query);
            foreach (DataRow dr in drows)
            {
                customPropertyValue = icapiCommon.dataRowValueGet(dr, "CustomPropertyValue");
                customPropertyScopeIsShared = icapiCommon.dataRowBoolValueGet(dr, "CustomPropertyScopeIsShared");
                customPropertyDataType = icapiCommon.dataRowValueGet(dr, "CustomPropertyDataType");
                break;
            }
            if (string.IsNullOrEmpty(customPropertyValue) == true)
            {
                customPropertyValue = "(値の設定なし)";
                fontColor = Color.Gray;
            }
            labelCustomPropertyValue.Text = customPropertyValue;
            labelCustomPropertyValue.ForeColor = fontColor;
            labelCustomPropertyDataType.Text = customPropertyDataType;
            labelCustomPropertyDataType.ForeColor = fontColor;
            labelCustomPropertyScope.Text = icapiCommon.boolScopeToStringScope(customPropertyScopeIsShared);
            labelCustomPropertyScope.ForeColor = fontColor;

        }

        /// <summary>
        /// カスタムプロパティのテンプレート選択 テキスト変更 イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxCustomPropertyTemplate_TextChanged(object sender, EventArgs e)
        {
            /* nullチェック */
            if (comboBoxCustomPropertyTemplate.SelectedValue == null)
            {
                return;
            }
            string templateName = comboBoxCustomPropertyTemplate.SelectedValue.ToString();
            string query = string.Format("CustomPropertyTemplateName = '{0}'", templateName);
            DataRow[] drows = this._customPropertyTemplateDataTable.Select(query);
            checkedListBoxCustomPropertyNameList.Items.Clear();
            foreach (DataRow dr in drows)
            {
                string customPropertyName = icapiCommon.dataRowValueGet(dr, "CustomPropertyName");
                if (string.IsNullOrEmpty(customPropertyName) == true)
                {
                    continue;
                }
                checkedListBoxCustomPropertyNameList.Items.Add(customPropertyName);
            }
        }
        

        /// <summary>
        /// カスタムプロパティのテンプレートファイルを編集のラベルクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelEditTemplateSettingFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string settingFilePath = getCustomPropertyTemplateSettingFilePath();
            
            /* テンプレートファイルがあるかチェックする */
            if (File.Exists(settingFilePath) != true)
            {
                /* ファイルがない場合はサンプルのテンプレートファイルを作成する */
                string parentDir = Path.GetDirectoryName(settingFilePath);
                if (Directory.Exists(parentDir) != true)
                {
                    /* 格納先ディレクトリが無いので作成する */
                    Directory.CreateDirectory(parentDir);
                }
                StreamWriter sw = new StreamWriter(settingFilePath);
                sw.WriteLine("/******************************************************************************************/");
                sw.WriteLine("/*  カスタムプロパティ テンプレート設定ファイル                                           */");
                sw.WriteLine("/******************************************************************************************/");
                sw.WriteLine("/*  入力フォーマット (カンマ区切りで下記5つ[値の省略時は4つ]のデータを入力します。)       */");
                sw.WriteLine("/*   [テンプレート名],[カスタムプロパティ名],[範囲設定],[値の種類],[値]                   */");
                sw.WriteLine("/*  ---------------------------------------------------------------------------------     */");
                sw.WriteLine("/*    テンプレート名:       任意のテンプレート名を入力します。                            */");
                sw.WriteLine("/*    カスタムプロパティ名: 任意のカスタムプロパティ名を入力します。                      */");
                sw.WriteLine("/*    範囲設定:             [TRUE]または[FALSE] を入力します。                            */");
                sw.WriteLine("/*          　                  TRUE:  すべてのリンクインスタンス                         */");
                sw.WriteLine("/*          　                  FALSE: このイノベーティブ パーツ                          */");
                sw.WriteLine("/*    値の種類:             [テキスト][数値][日付][はい/いいえ] を入力します。            */");
                sw.WriteLine("/*    値:                   値の種類がテキストまたは数値の場合は任意の値を入力します。    */");
                sw.WriteLine("/*                          日付の場合はyyyy/mm/ddの形式で入力します。                    */");
                sw.WriteLine("/*                          はい/いいえの場合は[はい]または[いいえ]の形式で入力します。   */");
                sw.WriteLine("/******************************************************************************************/");
                sw.WriteLine("テンプレートsample, メーカー, TRUE, テキスト, IronCAD");
                sw.WriteLine("テンプレートsample, 品番, TRUE, テキスト, 12345-67890");
                sw.WriteLine("テンプレートsample, 個数, TRUE, 数値, 1");
                sw.WriteLine("テンプレートsample, 廃版, TRUE, はい/いいえ, いいえ");
                sw.WriteLine("テンプレートsample2, 作成日, TRUE, 日付, 2021/12/15");
                sw.WriteLine("テンプレートsample2, 変更日, TRUE, 日付, 2022/1/1");
                sw.Close();
            }
            /* テンプレート設定ファイルを開く */
            Process.Start("notepad.exe", settingFilePath);
        }


        /// <summary>
        /// PictureBoxReload(テンプレートデータの再読み込み)のクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxReloadTemplateSettingFile_Click(object sender, EventArgs e)
        {
            DialogResult dret = MessageBox.Show("テンプレート設定ファイルを再読み込みしますか？", "確認", MessageBoxButtons.YesNo);
            if (dret != DialogResult.Yes)
            {
                return;
            }

            string settingFilePath = getCustomPropertyTemplateSettingFilePath();
            bool result = initializeCustomPropertyTemplate(settingFilePath);
            if (result != true)
            {
                MessageBox.Show("読み込みに失敗しました。");
                return;
            }
            MessageBox.Show("カスタムプロパティのテンプレート設定を読み込ました。");
        }


        /// <summary>
        /// カスタムプロパティテンプレートの設定保存ファイルのパスを取得する
        /// </summary>
        /// <returns></returns>
        private string getCustomPropertyTemplateSettingFilePath()
        {
            var assembly = Assembly.GetExecutingAssembly();
            string prgName = assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
            string companyName = assembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company;
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), companyName, prgName, "CustomPropertyTemplate.config"); ;
        }


        /// <summary>
        /// カスタムプロパティテンプレート設定ファイルを読み込む
        /// </summary>
        /// <param name="filePath">設定ファイルのパス</param>
        /// <param name="config">読み込んだ設定データ</param>
        public static bool ReadCustomPropertyTemplateSetting(string filePath, ref DataTable dt)
        {
            DataTable customPropertyTemplate = new DataTable();
            customPropertyTemplate.Columns.Add("CustomPropertyTemplateName");
            customPropertyTemplate.Columns.Add("CustomPropertyName");
            customPropertyTemplate.Columns.Add("CustomPropertyValue");
            customPropertyTemplate.Columns.Add("CustomPropertyDataType");
            customPropertyTemplate.Columns.Add("CustomPropertyScopeIsShared");

            if (File.Exists(filePath) != true)
            {
                return false;
            }

            string[] allLines = File.ReadAllLines(filePath);
            for (int i = 0; i < allLines.Count(); i++)
            {
                string line = allLines[i];
                /* コメントアウト行かチェックする */
                if (line.StartsWith("/*") == true)
                {
                    continue;
                }
                string[] split = line.Split(',');
                if (split.Count() < 4)
                {
                    continue;
                }
                string templateName = split[0].Trim();
                string propName = split[1].Trim();
                string scopeIsShared = split[2].Trim();
                string dataType = split[3].Trim();
                string value = string.Empty;
                /* カンマを含む文字列はsplitされてしまうので残りのデータを,で結合する */
                for (int j = 4; j < split.Count(); j++)
                {
                    value += split[j].Trim() + ',';
                }
                value = value.TrimEnd(',');
                DataRow dr = customPropertyTemplate.NewRow();
                dr["CustomPropertyTemplateName"] = templateName;
                dr["CustomPropertyName"] = propName;
                dr["CustomPropertyValue"] = value;
                dr["CustomPropertyDataType"] = dataType;
                dr["CustomPropertyScopeIsShared"] = scopeIsShared;
                customPropertyTemplate.Rows.Add(dr);
            }
            dt = customPropertyTemplate;
            return true;
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
    }
}
