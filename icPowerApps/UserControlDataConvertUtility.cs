using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using AdvancedDataGridView;
using interop.ICApiIronCAD;
using System.IO;
using icAPIAddinEnableDisable;
using System.Diagnostics;
using System.Windows.Automation;
using System.Runtime.InteropServices;
using System.Text;
using System.Runtime.InteropServices.ComTypes;
using System.Configuration;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using System.Xml.Linq;

namespace ICApiAddin.icPowerApps
{
    public partial class UserControlDataConvertUtility : UserControl
    {
        #region win32Api
        // EnumWindowsから呼び出されるコールバック関数のデリゲート
        private delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        // ウィンドウの表示状態を調べる(WS_VISIBLEスタイルを持つかを調べる)
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        //ウィンドウのタイトルの長さを取得する
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        // ウィンドウのタイトルバーのテキストを取得
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder title, int size);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
        #endregion

        public class WindowProp
        {
            public IntPtr hwnd { get; set; }
            public int ProcessId { get; set; }
            public string Title { get; set; }
            public string data;

            public WindowProp()
            {
                this.hwnd = IntPtr.Zero;
                this.ProcessId = -1;
                this.Title = string.Empty;
            }
            public WindowProp(IntPtr ptr, int procId, string title)
            {
                this.hwnd = ptr;
                this.ProcessId = procId;
                this.Title = title;
            }
        }

        public const string title = "データ変換ユーティリティ";
        private IZBaseApp _ironcadApp = null;
        private static List<WindowProp> WindowPropList = new List<WindowProp>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UserControlDataConvertUtility(IZBaseApp ironcadApp)
        {
            InitializeComponent();
            this.Tag = new UserControlTagData();
            this.Dock = DockStyle.Fill;
            this._ironcadApp = ironcadApp;
        }

        private bool convertStringToExportType(string exportStr, ref eZModelType exportType, ref string ext)
        {
            exportType = eZModelType.Z_MODEL_STEP214;
            switch (exportStr)
            {
                case "ACIS 2021 R31(*.sat)":
                    exportType = eZModelType.Z_MODEL_SAT_R31;
                    ext = ".sat";
                    break;
                case "ACIS R29 (*.sat)":
                    exportType = eZModelType.Z_MODEL_SAT_R29;
                    ext = ".sat";
                    break;
                case "ACIS R26 (*.sat)":
                    exportType = eZModelType.Z_MODEL_SAT_R26;
                    ext = ".sat";
                    break;
                case "Parasolid 34.0 (*.x_t)":
                    exportType = eZModelType.Z_MODEL_X_T_340;
                    ext = ".x_t";
                    break;
                case "Parasolid 31.1 (*.x_t)":
                    exportType = eZModelType.Z_MODEL_X_T_311;
                    ext = ".x_t";
                    break;
                case "Parasolid 29.0 (*.x_t)":
                    exportType = eZModelType.Z_MODEL_X_T_290;
                    ext = ".x_t";
                    break;
                case "Parasolid 26.1 (*.x_t)":
                    exportType = eZModelType.Z_MODEL_X_T_261;
                    ext = ".x_t";
                    break;
                case "STEP AP203 (*.stp,*.step)":
                    exportType = eZModelType.Z_MODEL_STEP;
                    ext = ".stp";
                    break;
                case "STEP AP214 (*.stp,*.step)":
                    exportType = eZModelType.Z_MODEL_STEP214;
                    ext = ".stp";
                    break;
                case "IGES (*.igs,*.iges)":
                    exportType = eZModelType.Z_MODEL_IGES;
                    ext = ".igs";
                    break;
                case "CATIA V4 (*.model)":
                    exportType = eZModelType.Z_MODEL_CATIA_MODEL;
                    ext = ".model";
                    break;
                case "CATIA V5 パーツ (*.CatPart)":
                    exportType = eZModelType.Z_MODEL_CATPART;
                    ext = ".CatPart";
                    break;
                case "CATIA V5 アセンブリ (*.CatProduct)":
                    exportType = eZModelType.Z_MODEL_CATPRODUCT;
                    ext = ".CatProduct";
                    break;
                case "3D PDF ファイル (*.pdf)":
                    exportType = eZModelType.Z_MODEL_3DPDF;
                    ext = ".pdf";
                    break;
                default:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// ページ表示状態変更イベント(ページ表示イベントとして利用)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlDataConvertUtility_VisibleChanged(object sender, EventArgs e)
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

                comboBoxStepUnit.SelectedIndex = 0;
                comboBoxIgesType.SelectedIndex = 0;
                comboBoxIgesFormat.SelectedIndex = 0;
                checkBoxIgesNurbs.Checked = false;
                comboBoxCatiaVersion.SelectedIndex = 0;
                checkBoxCatiaExport3dCurve.Checked = true;
            }
        }

        #region イベント

        #endregion イベント

        public bool OpenDirectoryDialog(string title, ref string directoryPath)
        {
            /* 保存先のダイアログを表示する */
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "フォルダを選択";
            ofd.Title = title;
            ofd.Filter = "フォルダ|.";
            ofd.ValidateNames = false;
            ofd.CheckFileExists = false;
            ofd.CheckPathExists = true;
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                /* 取得キャンセル */
                return false;
            }
            directoryPath = Path.GetDirectoryName(ofd.FileName);
            return true;
        }

        public bool OpenSelectFileDialog(string title, ref List<string> selectFiles)
        {
            /* 保存先のダイアログを表示する */
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = String.Empty;
            ofd.Title = title;
            ofd.Filter = "すべてのファイル|*.*;";
            ofd.ValidateNames = false;
            ofd.Multiselect = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                /* 取得キャンセル */
                return false;
            }
            selectFiles = ofd.FileNames.ToList();
            return true;
        }

        public static List<String> GetAllFiles(String DirPath, bool includeChildFolder)
        {
            List<String> fileList = new List<String>();    // 取得したファイル名を格納するためのリスト

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(DirPath);
            System.IO.FileInfo[] files = null;
            if (includeChildFolder == true)
            {
                files = di.GetFiles("*.exb", System.IO.SearchOption.AllDirectories);
            }
            else
            {
                files = di.GetFiles("*.exb", System.IO.SearchOption.TopDirectoryOnly);
            }
            foreach (FileInfo fi in files)
            {
                fileList.Add(fi.FullName);
            }
            return fileList;

        }

        private void addDataGridViewFileList(List<string> filePathList)
        {
            foreach (string filePath in filePathList)
            {
                bool alreadyAdded = CheckAlreadyAddedFilePath(filePath);
                if (alreadyAdded == true)
                {
                    MessageBox.Show(string.Format("{0}は既に処理するファイルに含まれています。", filePath));
                    continue;
                }
                string fileName = Path.GetFileName(filePath);
                dataGridViewFileList.Rows.Add(string.Empty, fileName, filePath);
            }
        }

        private bool CheckAlreadyAddedFilePath(string filePath)
        {
            for (int i = 0; i < dataGridViewFileList.Rows.Count; i++)
            {
                string rowFilePath = icapiCommon.dataGridViewValueGet(dataGridViewFileList, i, "FilePath");
                if (string.Equals(rowFilePath.ToLower(), filePath.ToLower()) == true)
                {
                    return true;
                }
            }
            return false;
        }


        private void buttonSelectFiles_Click(object sender, EventArgs e)
        {
            List<string> filePathList = new List<string>();
            bool ret = OpenSelectFileDialog("一括更新するファイルを指定してください", ref filePathList);
            if (ret == false)
            {
                return;
            }
            addDataGridViewFileList(filePathList);
        }

        private void buttonSelectFolder_Click(object sender, EventArgs e)
        {
            List<string> filePathList = new List<string>();
            string path = string.Empty;
            bool ret = OpenDirectoryDialog("一括更新するフォルダのパスを指定してください", ref path);
            if (ret == false)
            {
                return;
            }
            filePathList = GetAllFiles(path, checkBoxAddChildFolder.Checked);
            addDataGridViewFileList(filePathList);
        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            eZModelType exportModelType = eZModelType.Z_MODEL_STEP214;
            string exportModelExt = string.Empty;
            bool convResult = convertStringToExportType(comboBoxExportFormat.Text, ref exportModelType, ref exportModelExt);
            if(convResult != true)
            {
                MessageBox.Show("エクスポートの種類が選択されていません。");
                return;
            }
            if((exportModelType == eZModelType.Z_MODEL_SAT_R29) || (exportModelType == eZModelType.Z_MODEL_SAT_R31) ||
               (exportModelType == eZModelType.Z_MODEL_X_T_311) || (exportModelType == eZModelType.Z_MODEL_X_T_340) ||
               (exportModelType == eZModelType.Z_MODEL_3DPDF))
            {
                DialogResult dret = MessageBox.Show("IRONCAD ICAPIの不具合によりエラーが発生する可能性があります。実行しますか？", "確認", MessageBoxButtons.OKCancel);
                if(dret != DialogResult.OK)
                {
                    return;
                }
            }

            int convertCount = 0;
            StringBuilder skipFiles = new StringBuilder();
            string stepUnit = comboBoxStepUnit.Text;
            string igesType = comboBoxIgesType.Text;
            string igesFormat = comboBoxIgesFormat.Text;
            bool igesNurbs = checkBoxIgesNurbs.Checked;
            string catiaVersion = comboBoxCatiaVersion.Text;
            bool export3dCurve = checkBoxCatiaExport3dCurve.Checked;
            bool skipConfirm = checkBoxCatiaNotConfirmNextSession.Checked;
            for (int rowIndex = 0; rowIndex < dataGridViewFileList.Rows.Count; rowIndex++)
            {
                string result = icapiCommon.dataGridViewValueGet(dataGridViewFileList, rowIndex, "ConvertResult");
                string filePath = icapiCommon.dataGridViewValueGet(dataGridViewFileList, rowIndex, "FilePath");
                string dirPath = Path.GetDirectoryName(filePath);
                string fileName = Path.GetFileName(filePath);
                string fileNameWithOutExt = Path.GetFileNameWithoutExtension(filePath);
                string ext = Path.GetExtension(filePath);
                if (string.Equals(result, "変換済") == true)
                {
                    skipFiles.AppendLine(string.Format("  {0}行目: {1}", rowIndex+1, fileName));
                    continue;
                }
                convertCount++;

                /* ファイルを開く */
                bool alreadyOpen = false;
                IZDoc doc = null;
                IZSceneDoc sceneDoc = null;;
                if (ext.ToLower() == ".ics")
                {
                    doc = this._ironcadApp.OpenFile2(filePath, true, false, out alreadyOpen);
                    sceneDoc = doc as IZSceneDoc;
                }
                else
                {
                    doc = this._ironcadApp.CreateNewDoc(eZDocType.Z_SCENE, true, true, string.Empty, false);
                    sceneDoc = doc as IZSceneDoc;
                    sceneDoc.ImportModel(filePath, false);
                }
                if (doc == null)
                {
                    dataGridViewFileList["ConvertResult", rowIndex].Value = "ファイルを開けませんでした。";
                    continue;
                }

                /* ツリーを取得する */
                IZElement topElem = sceneDoc.GetTopElement();
                ZArray childs = topElem.GetChildrenZArray();
                IZAssembly asm = null;
                int count = 0;
                childs.Count(out count);
                if (count == 0)
                {
                    /* データがない */
                    doc.Close();
                    dataGridViewFileList["ConvertResult", rowIndex].Value = "エラー: 変換するデータが見つかりませんでした。";
                    continue;
                }

                object obj;
                IZElement childElem;
                childs.Get(0, out obj);
                childElem = obj as IZElement;
                if ((count > 1) || (count == 1) && (childElem.Type != eZElementType.Z_ELEMENT_ASSEMBLY))
                {
                    /* 先頭がアセンブリ1つでない                                                             */
                    /* (先頭に複数のパーツ/アセンブリがある または 先頭は1つであるがアセンブリでない) 場合は */
                    /* アセンブリを作成し、それに全て入れる                                                  */
                    IZElement newAssemblyElem = null;
                    asm = sceneDoc.CreateAssembly();
                    newAssemblyElem = asm as IZElement;
                    newAssemblyElem.Name = fileNameWithOutExt;
                    for (int i = 0; i < count; i++)
                    {
                        childs.Get(i, out obj);
                        childElem = obj as IZElement;
                        asm.AddChild(childElem);
                    }
                }
                else
                {
                    /* 先頭がアセンブリ1つの場合 */
                    asm = childElem as IZAssembly;
                }
                if((exportModelType == eZModelType.Z_MODEL_STEP) || (exportModelType == eZModelType.Z_MODEL_STEP214))
                {
                    /* Stepのエクスポートオプションが開くので設定を行い閉じる */
                    StepOptionClicker(stepUnit);
                }
                if (exportModelType == eZModelType.Z_MODEL_IGES)
                {
                    /* Stepのエクスポートオプションが開くので設定を行い閉じる */
                    IgesOptionClicker(igesType, igesFormat ,igesNurbs);
                }
                if((exportModelType == eZModelType.Z_MODEL_CATPART) ||
                   (exportModelType == eZModelType.Z_MODEL_CATPRODUCT))
                {
                    CatiaOptionClicker(catiaVersion, export3dCurve, skipConfirm);
                }
                asm.Export(Path.Combine(dirPath, fileNameWithOutExt), exportModelType);

                /* ドキュメントを閉じる */
                doc.Close();
                dataGridViewFileList["ConvertResult", rowIndex].Value = "変換済";
            }
            if(convertCount > 0)
            {
                MessageBox.Show("データ変換が完了しました。");
            }
            if (skipFiles.Length > 0)
            {
                MessageBox.Show(string.Format("以下のファイルは変換済のためスキップしました。\n{0}", skipFiles.ToString()));
            }
        }

        // ウィンドウを列挙するコールバックメソッド
        private static bool EnumerateThreadWindows(IntPtr hWnd, IntPtr lParam)
        {
            // ウィンドウが可視かどうか調べて、表示してないのものを除外する
            if (IsWindowVisible(hWnd) != true)
            {
                return true;
            }

            //ウィンドウのタイトルの長さを取得する
            int textLen = GetWindowTextLength(hWnd);
            if (textLen == 0)
            {
                return true;
            }

            //ウィンドウのタイトルを取得する
            var title = new StringBuilder(textLen + 1);
            GetWindowText(hWnd, title, title.Capacity);

            // ウィンドウハンドルからプロセスIDを取得
            int processId;
            GetWindowThreadProcessId(hWnd, out processId);


            WindowPropList.Add(new WindowProp(hWnd, processId, title.ToString()));

            // 途中で列挙をやめるときは、return false;にする
            // ウィンドウの列挙を継続する
            return true;
        }


        /// <summary>
        /// STEPでのエクスポート時のウィンドウを処理する
        /// </summary>
        /// <param name="stepUnit"></param>
        public async void StepOptionClicker(string stepUnit)
        {
            int timerMs = 500;

            /* クリック対象のウィンドウ */
            AutomationElement rootElement = AutomationElement.RootElement;
            if (rootElement == null)
            {
                return;
            }

            /* IRONCADのプロセスを取得する */
            Process[] procList = Process.GetProcessesByName("IRONCAD");
            foreach (Process proc in procList)
            {
                /* 実行パスとタイトルを取得 */
                string exePath = proc.MainModule.FileName;
                string title = proc.MainWindowTitle;
                bool breakFlag = false;
                do
                {
                    await Task.Delay(timerMs);

                    WindowPropList.Clear();

                    /* 子ウィンドウを取得 */
                    EnumThreadWindows(proc.Threads[0].Id, EnumerateThreadWindows, IntPtr.Zero);

                    /* 子ウィンドウからクリックするウィンドウを見つける */
                    for (int windowIndex = 0; windowIndex < WindowPropList.Count; windowIndex++)
                    {
                        WindowProp prop = WindowPropList[windowIndex];

                        /* クリック対象のウィンドウかチェックする */
                        AutomationElement IcWindowForm = AutomationElement.FromHandle(prop.hwnd);
                        AutomationElement okButton = null;

                        /* STEPのエクスポートウィンドウ */
                        if (string.Equals(IcWindowForm.Current.Name, "STEP エクスポート オプション") != true)
                        {
                            continue;
                        }

                        /* 単位設定のリストを取得 */
                        AutomationElementCollection stepUnitOption = IcWindowForm.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem));
                        if (stepUnitOption == null || stepUnitOption.Count == 0)
                        {
                            return;
                        }

                        for (int unitIndex = 0; unitIndex < stepUnitOption.Count; unitIndex++)
                        {
                            if (string.Equals(stepUnitOption[unitIndex].Current.Name, stepUnit) == true)
                            {
                                // アイテムリストの先頭を選択する
                                SelectionItemPattern patternItem = (SelectionItemPattern)stepUnitOption[unitIndex].GetCurrentPattern(SelectionItemPattern.Pattern);
                                patternItem.Select();
                                break;
                            }
                        }

                        okButton = IcWindowForm.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "OK")); // This part gets the handle of the button inside the messagebox
                        /* 保存のウィンドウ */
                        if (okButton != null)
                        {
                            /* はいをクリックして更新する */
                            InvokePattern invokePattern = (InvokePattern)okButton.GetCurrentPattern(InvokePattern.Pattern);
                            invokePattern.Invoke();
                            breakFlag = true;
                        }
                    }
                } while (breakFlag != true);
            }
        }

        /// <summary>
        /// IGESでのエクスポート時のウィンドウを処理する
        /// </summary>
        /// <param name="exportType"></param>
        /// <param name="exportFormat"></param>
        /// <param name="outputNurbs"></param>
        public async void IgesOptionClicker(string exportType, string exportFormat, bool outputNurbs)
        {
            int timerMs = 500;

            /* クリック対象のウィンドウ */
            AutomationElement rootElement = AutomationElement.RootElement;
            if (rootElement == null)
            {
                return;
            }

            /* IRONCADのプロセスを取得する */
            Process[] procList = Process.GetProcessesByName("IRONCAD");
            foreach (Process proc in procList)
            {
                /* 実行パスとタイトルを取得 */
                string exePath = proc.MainModule.FileName;
                string title = proc.MainWindowTitle;
                bool breakFlag = false;
                do
                {
                    await Task.Delay(timerMs);

                    WindowPropList.Clear();

                    /* 子ウィンドウを取得 */
                    EnumThreadWindows(proc.Threads[0].Id, EnumerateThreadWindows, IntPtr.Zero);

                    /* 子ウィンドウからクリックするウィンドウを見つける */
                    for (int windowIndex = 0; windowIndex < WindowPropList.Count; windowIndex++)
                    {
                        WindowProp prop = WindowPropList[windowIndex];

                        /* クリック対象のウィンドウかチェックする */
                        AutomationElement IcWindowForm = AutomationElement.FromHandle(prop.hwnd);
                        AutomationElement okButton = null;

                        /* IGESのエクスポートウィンドウ */
                        if (string.Equals(IcWindowForm.Current.Name, "エクスポート - IGES") != true)
                        {
                            continue;
                        }

                        /* 出力の種類のリストを取得 */
                        AutomationElementCollection igesExportType = IcWindowForm.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem));
                        if (igesExportType == null || igesExportType.Count == 0)
                        {
                            return;
                        }

                        for (int typeIndex = 0; typeIndex < igesExportType.Count; typeIndex++)
                        {
                            if (string.Equals(igesExportType[typeIndex].Current.Name, exportType) == true)
                            {
                                // アイテムリストの先頭を選択する
                                SelectionItemPattern patternItem = (SelectionItemPattern)igesExportType[typeIndex].GetCurrentPattern(SelectionItemPattern.Pattern);
                                patternItem.Select();
                                break;
                            }
                        }


                        /* 出力形式のリストを取得 */
                        AutomationElementCollection outputFormat = IcWindowForm.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.RadioButton));
                        if (outputFormat == null || outputFormat.Count == 0)
                        {
                            return;
                        }

                        for (int index = 0; index < outputFormat.Count; index++)
                        {
                            if (string.Equals(outputFormat[index].Current.Name, exportFormat) == true)
                            {
                                // アイテムリストの先頭を選択する
                                SelectionItemPattern patternItem = (SelectionItemPattern)outputFormat[index].GetCurrentPattern(SelectionItemPattern.Pattern);
                                patternItem.Select();
                                break;
                            }
                        }

                        /* 出力形式のリストを取得 */
                        AutomationElementCollection checkBoxNurbs = IcWindowForm.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.CheckBox));
                        if (checkBoxNurbs == null || checkBoxNurbs.Count == 0)
                        {
                            return;
                        }
                        TogglePattern checkBoxItem = (TogglePattern)checkBoxNurbs[0].GetCurrentPattern(TogglePattern.Pattern);
                        if(outputNurbs == true)
                        {
                            if (checkBoxItem.Current.ToggleState == ToggleState.Off)
                            {
                                checkBoxItem.Toggle();
                            }
                        }
                        else
                        {
                            if (checkBoxItem.Current.ToggleState == ToggleState.On)
                            {
                                checkBoxItem.Toggle();
                            }
                        }
                        okButton = IcWindowForm.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "OK")); // This part gets the handle of the button inside the messagebox
                        /* 保存のウィンドウ */
                        if (okButton != null)
                        {
                            /* はいをクリックして更新する */
                            InvokePattern invokePattern = (InvokePattern)okButton.GetCurrentPattern(InvokePattern.Pattern);
                            invokePattern.Invoke();
                            breakFlag = true;
                        }
                    }
                } while (breakFlag != true);
            }
        }

        /// <summary>
        /// CATIAデータでのエクスポート時のウィンドウを処理する
        /// </summary>
        /// <param name="catiaExportVersion"></param>
        /// <param name="export3dCurve"></param>
        /// <param name="skipConfirm"></param>
        public async void CatiaOptionClicker(string catiaExportVersion, bool export3dCurve, bool skipConfirm)
        {
            int timerMs = 500;

            /* クリック対象のウィンドウ */
            AutomationElement rootElement = AutomationElement.RootElement;
            if (rootElement == null)
            {
                return;
            }

            /* IRONCADのプロセスを取得する */
            Process[] procList = Process.GetProcessesByName("IRONCAD");
            foreach (Process proc in procList)
            {
                /* 実行パスとタイトルを取得 */
                string exePath = proc.MainModule.FileName;
                string title = proc.MainWindowTitle;
                bool breakFlag = false;
                do
                {
                    await Task.Delay(timerMs);

                    WindowPropList.Clear();

                    /* 子ウィンドウを取得 */
                    EnumThreadWindows(proc.Threads[0].Id, EnumerateThreadWindows, IntPtr.Zero);

                    /* 子ウィンドウからクリックするウィンドウを見つける */
                    for (int windowIndex = 0; windowIndex < WindowPropList.Count; windowIndex++)
                    {
                        WindowProp prop = WindowPropList[windowIndex];

                        /* クリック対象のウィンドウかチェックする */
                        AutomationElement IcWindowForm = AutomationElement.FromHandle(prop.hwnd);
                        AutomationElement okButton = null;

                        /* CATIAのエクスポートウィンドウ */
                        if (string.Equals(IcWindowForm.Current.Name, "エクスポート - CATIA V5") != true)
                        {
                            continue;
                        }

                        /* バージョンを設定 */
                        AutomationElementCollection catiaTextControl= IcWindowForm.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
                        if (catiaTextControl == null || catiaTextControl.Count == 0)
                        {
                            return;
                        }
                        ValuePattern textBoxCatiaExportVersion = (ValuePattern)catiaTextControl[0].GetCurrentPattern(ValuePattern.Pattern);
                        catiaTextControl[0].SetFocus();
                        await Task.Delay(300);
                        SendKeys.SendWait("^{HOME}");   // Move to start of control
                        SendKeys.SendWait("^(a)");   // Select everything
                        SendKeys.SendWait(catiaExportVersion);
                        await Task.Delay(500);


                        /* チェックボックスの設定を行う(3D曲線エクスポートする) */
                        AutomationElementCollection checkBoxControls = IcWindowForm.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.CheckBox));
                        if (checkBoxControls == null || checkBoxControls.Count == 0)
                        {
                            return;
                        }
                        for (int index = 0; index < checkBoxControls.Count; index++)
                        {
                            if (string.Equals(checkBoxControls[index].Current.Name, "3D曲線をエクスポートする") == true)
                            {
                                // アイテムリストの先頭を選択する
                                TogglePattern patternItem = (TogglePattern)checkBoxControls[index].GetCurrentPattern(TogglePattern.Pattern);
                                if (export3dCurve == true)
                                {
                                    if (patternItem.Current.ToggleState == ToggleState.Off)
                                    {
                                        patternItem.Toggle();
                                    }
                                }
                                else
                                {
                                    if (patternItem.Current.ToggleState == ToggleState.On)
                                    {
                                        patternItem.Toggle();
                                    }
                                }
                                break;
                            }
                        }
                        /* チェックボックスの設定を行う(次のセッションまで確認しない) */
                        for (int index = 0; index < checkBoxControls.Count; index++)
                        {
                            if (string.Equals(checkBoxControls[index].Current.Name, "次のセッションまで確認しない") == true)
                            {
                                // アイテムリストの先頭を選択する
                                TogglePattern patternItem = (TogglePattern)checkBoxControls[index].GetCurrentPattern(TogglePattern.Pattern);
                                if (skipConfirm == true)
                                {
                                    if (patternItem.Current.ToggleState == ToggleState.Off)
                                    {
                                        patternItem.Toggle();
                                    }
                                }
                                else
                                {
                                    if (patternItem.Current.ToggleState == ToggleState.On)
                                    {
                                        patternItem.Toggle();
                                    }
                                }
                                break;
                            }
                        }

                        okButton = IcWindowForm.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "OK")); 
                        /* 保存のウィンドウ */
                        if (okButton != null)
                        {
                            /* はいをクリックして更新する */
                            InvokePattern invokePattern = (InvokePattern)okButton.GetCurrentPattern(InvokePattern.Pattern);
                            invokePattern.Invoke();
                            breakFlag = true;
                        }
                    }
                } while (breakFlag != true);
            }
        }


        /// <summary>
        /// エクスポートの種類の変更 イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxExportFormat_TextChanged(object sender, EventArgs e)
        {
            string selectedText = comboBoxExportFormat.Text;
            eZModelType modelType = eZModelType.Z_MODEL_STEP214;
            string ext = string.Empty;
            convertStringToExportType(selectedText, ref modelType, ref ext);
            if ((string.Equals(selectedText, "STEP AP203 (*.stp,*.step)") == true) ||
                (string.Equals(selectedText, "STEP AP214 (*.stp,*.step)") == true))
            {
                tableLayoutPanelOptionExportStep.Visible = true;
                tableLayoutPanelOptionExportIges.Visible = false;
                tableLayoutPanelOptionExportCatia.Visible = false;
            }
            else if (string.Equals(selectedText, "IGES (*.igs,*.iges)") == true)
            {
                tableLayoutPanelOptionExportStep.Visible = false;
                tableLayoutPanelOptionExportIges.Visible = true;
                tableLayoutPanelOptionExportCatia.Visible = false;
            }
            if ((string.Equals(selectedText, "CATIA V5 パーツ (*.CatPart)") == true) ||
                (string.Equals(selectedText, "CATIA V5 アセンブリ (*.CatProduct)") == true))
            {
                tableLayoutPanelOptionExportStep.Visible = false;
                tableLayoutPanelOptionExportIges.Visible = false;
                tableLayoutPanelOptionExportCatia.Visible = true;
            }
            else
            { 
                tableLayoutPanelOptionExportStep.Visible = false;
                tableLayoutPanelOptionExportIges.Visible = false;
                tableLayoutPanelOptionExportCatia.Visible = false;
            }
        }

    }
}
