using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System.Windows.Forms.VisualStyles;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace ICApiAddin.icPowerApps
{
    public partial class UserControlWebBrowser : UserControl
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        private BorderLessButton urlDeleteButton = null;
        private int textboxUrlMinWidth = 100;
        private bool _useSelfCopyPaste = false;

        /// <summary>
        /// URL削除ボタン用のボーダーレスボタン
        /// </summary>
        private class BorderLessButton : Button
        {
            public BorderLessButton() : base()
            {
                this.TabStop = false;
                this.FlatStyle = FlatStyle.Flat;
                this.FlatAppearance.BorderSize = 0;
                this.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
                this.FlatAppearance.MouseDownBackColor = Color.Transparent;
                this.FlatAppearance.MouseOverBackColor = Color.Transparent;
            }
            protected override bool ShowFocusCues
            {
                get
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// お気に入りのデータ
        /// </summary>
        private class FAVORITE_DATA
        {
            public string title;
            public string url;
            public FAVORITE_DATA()
            {
                this.title = string.Empty;
                this.url = string.Empty;
            }
            public FAVORITE_DATA(string title, string url)
            {
                this.title = title;
                this.url = url;
            }
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="useTmpFolder">データフォルダにテンポラリフォルダを使用 true:使用する  false:使用しない(プログラムの実行フォルダ)</param>
        public UserControlWebBrowser(bool useTmpFolder, bool useSelfCopyPaste)
        {
            InitializeComponent();
            this.Disposed += OnDispose;
            this._useSelfCopyPaste = useSelfCopyPaste;
            InitializeAsync(useTmpFolder);
            InitializeFavorite();
        }
        
        /// <summary>
        /// Disposeイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDispose(object sender, EventArgs e)
        {
            this.webView2Main.Dispose();
        }


        /// <summary>
        /// お気に入りを初期化する
        /// </summary>
        private void InitializeFavorite()
        {
            this.toolStripSplitButtonFavorite.DropDownItems.Clear();

            /* お気に入り編集ボタンの追加 */
            ToolStripMenuItem editFavorite = new ToolStripMenuItem();
            editFavorite.Text = "お気に入りの編集 (行データ: \"URL\",\"タイトル\" で入力します)";
            editFavorite.Click += EditFavorite_Click;
            editFavorite.Tag = string.Empty;
            this.toolStripSplitButtonFavorite.DropDownItems.Add(editFavorite);

            /* 再読み込みボタンの追加 */
            ToolStripMenuItem reloadFavorite = new ToolStripMenuItem();
            reloadFavorite.Text = "お気に入りの再読み込み";
            reloadFavorite.Click += ReloadFavorite_Click;
            reloadFavorite.Tag = string.Empty;
            this.toolStripSplitButtonFavorite.DropDownItems.Add(reloadFavorite);

            /* separator */
            this.toolStripSplitButtonFavorite.DropDownItems.Add(new ToolStripSeparator());

            /* お気に入り設定データを読み込む */
            loadFavoriteData();
        }


        /// <summary>
        /// ロード イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlWebBrowser_Load(object sender, EventArgs e)
        {
            /* アドレスバーのアドレス削除ボタンを追加 */
            urlDeleteButton = new BorderLessButton();
            urlDeleteButton.Size = new Size(toolStripTextBoxUrl.Size.Height-2, toolStripTextBoxUrl.Size.Height-2);
            urlDeleteButton.Location = new Point(toolStripTextBoxUrl.Width - (urlDeleteButton.Width+5), 0);
            urlDeleteButton.Cursor = Cursors.Default;
            urlDeleteButton.BackgroundImageLayout = ImageLayout.Stretch;
            urlDeleteButton.BackgroundImage = Properties.Resources.icon_webBrowserDelete;
            urlDeleteButton.Click += urlDeleteButton_Click;
            toolStripTextBoxUrl.Control.Controls.Add(urlDeleteButton);
            SendMessage(toolStripTextBoxUrl.Control.Handle, 0xd3, (IntPtr)2, (IntPtr)(urlDeleteButton.Width << 16));

        }


        /// <summary>
        /// WebView2を初期化する
        /// </summary>
        /// <param name="useTmpFolder"></param>
        private async void InitializeAsync(bool useTmpFolder)
        {
            if (useTmpFolder == true)
            {
                string tmpPath = System.IO.Path.GetTempPath();
                var env = await Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(null, tmpPath, null);
                await webView2Main.EnsureCoreWebView2Async(env);
            }
            else
            {
                await webView2Main.EnsureCoreWebView2Async(null);
            }
            webView2Main.CoreWebView2.NavigationStarting += webView2_NavigationStarting;
            webView2Main.CoreWebView2.NavigationCompleted += webView2_NavigationCompleted;
            webView2Main.CoreWebView2.Navigate("https://google.com");
        }


        /// <summary>
        /// お気に入りの設定保存ファイルのパスを取得する
        /// </summary>
        /// <returns></returns>
        private string getFavoriteConfigPath()
        {
            var assembly = Assembly.GetExecutingAssembly();
            string prgName = assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;                  // "タイトル"
            string companyName = assembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company;
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), companyName, prgName, "favorite.config"); ;
        }
        

        /// <summary>
        /// お気に入りのデータを読み込む
        /// </summary>
        private void loadFavoriteData()
        {
            List<FAVORITE_DATA> favoriteDataList = new List<FAVORITE_DATA>();
            string favoriteFilePath = getFavoriteConfigPath();
            if(File.Exists(favoriteFilePath) != true)
            {
                /* ファイルを作成 */
                Directory.CreateDirectory(Path.GetDirectoryName(favoriteFilePath));
                StreamWriter sw = new StreamWriter(favoriteFilePath);
                sw.Close();
            }
            string[] favoriteDate = File.ReadAllLines(favoriteFilePath);
            foreach(string lineStr in favoriteDate)
            {
                if( string.IsNullOrEmpty(lineStr.Trim()) == true)
                {
                    continue;
                }
                string[] splitData = lineStr.Split(',');
                if(splitData.Count() < 2)
                {
                    continue;
                }
                string url = splitData[0];
                string title = splitData[1];
                favoriteDataList.Add(new FAVORITE_DATA(title, url));
            }

            if(favoriteDataList.Count <= 0)
            {
                ToolStripMenuItem noneItem = new ToolStripMenuItem();
                noneItem.Text = "お気に入り(ブックマーク)はありません";
                noneItem.ForeColor = SystemColors.GrayText;
                noneItem.Tag = string.Empty;
                this.toolStripSplitButtonFavorite.DropDownItems.Add(noneItem);
                return;
            }

            foreach(FAVORITE_DATA favoriteItem in favoriteDataList)
            {
                addFavoriteItem(favoriteItem.title, favoriteItem.url);
            }
        }


        /// <summary>
        /// お気に入りのデータを保存する
        /// </summary>
        private void saveFavoriteData()
        {
            string favoriteFilePath = getFavoriteConfigPath();
            string parentDir = Path.GetDirectoryName(favoriteFilePath);
            if (Directory.Exists(parentDir) != true)
            {
                Directory.CreateDirectory(parentDir);
            }
            StreamWriter sw = new StreamWriter(favoriteFilePath);

            for (int i = 3; i < toolStripSplitButtonFavorite.DropDownItems.Count; i++)
            {
                Type dataType = toolStripSplitButtonFavorite.DropDownItems[i].GetType();
                if (dataType == typeof(ToolStripMenuItem))
                {
                    ToolStripMenuItem item = (ToolStripMenuItem)toolStripSplitButtonFavorite.DropDownItems[i];
                    string itemUrl = item.Tag.ToString();
                    if(string.IsNullOrEmpty(itemUrl) == true)
                    {
                        continue;
                    }
                    string itemTitle = item.Text.ToString();
                    sw.WriteLine(string.Format("{0},{1}", itemUrl, itemTitle));
                }
            }
            sw.Close();
        }


        /// <summary>
        /// textBoxUrlに入力されたアドレスに移動する
        /// </summary>
        private void goInputURL()
        {
            try
            {
                if (string.IsNullOrEmpty(toolStripTextBoxUrl.Text) == true)
                {
                    this.toolStripTextBoxUrl.Text = webView2Main.CoreWebView2.Source;
                    return;
                }
                string url = toolStripTextBoxUrl.Text;
                if (url.StartsWith("https://") == false)
                {
                    url = "https://www.google.com/search?q=" + url;
                }
                //                Uri uri = new Uri();
                webView2Main.CoreWebView2.Navigate(url);
            }
            catch (Exception ex)
            {

            }
        }



        /// <summary>
        /// お気に入りに項目を追加する
        /// </summary>
        /// <param name="title"></param>
        /// <param name="url"></param>
        private void addFavoriteItem(string title, string url)
        {
            ToolStripMenuItem item1 = new ToolStripMenuItem();
            if (string.IsNullOrEmpty(url) == true)
            {
                /* URLなしは無効 */
                return;
            }
            if (string.IsNullOrEmpty(title) == true)
            {
                title = url;
            }
            item1.Text = title;
            item1.Tag = url;
            item1.ToolTipText = url;
            item1.Click += favoriteItem_Click;

            this.toolStripSplitButtonFavorite.DropDownItems.Add(item1);
            return;
        }


        #region イベント
        /// <summary>
        /// WebView2の移動(表示)完了 イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
        }


        /// <summary>
        /// WebView2の移動開始 イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webView2_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            toolStripTextBoxUrl.Text = e.Uri;
        }

        /// <summary>
        /// 戻るボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonBack_Click(object sender, EventArgs e)
        {
            webView2Main.CoreWebView2.GoBack();
        }


        /// <summary>
        /// 進むボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonPrevious_Click(object sender, EventArgs e)
        {
            webView2Main.CoreWebView2.GoForward();
        }

        
        /// <summary>
        /// 更新ボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonReload_Click(object sender, EventArgs e)
        {
            webView2Main.CoreWebView2.Reload();
        }


        /// <summary>
        /// ホームボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonHome_Click(object sender, EventArgs e)
        {
            webView2Main.CoreWebView2.Navigate("https://google.com");
        }


        /// <summary>
        /// textBoxUrlの削除ボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void urlDeleteButton_Click(object sender, EventArgs e)
        {
            toolStripTextBoxUrl.Text = string.Empty;
            toolStripTextBoxUrl.Focus();
        }


        /// <summary>
        /// 入力されたURLへ移動のボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonUrlGo_Click(object sender, EventArgs e)
        {
            goInputURL();
        }


        /// <summary>
        /// お気に入りボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripSplitButtonFavorite_ButtonClick(object sender, EventArgs e)
        {
            DialogResult dret = MessageBox.Show("お気に入り（ブックマーク）に登録しますか？", "ブックマーク登録", MessageBoxButtons.YesNo);
            if (dret != DialogResult.Yes)
            {
                return;
            }
            if (toolStripSplitButtonFavorite.DropDownItems.Count == 4)
            {
                ToolStripMenuItem item = (ToolStripMenuItem)toolStripSplitButtonFavorite.DropDownItems[3];
                string itemTitle = item.Text.ToString();
                string itemUrl = item.Tag.ToString();
                if ((string.Equals(itemTitle, "お気に入り(ブックマーク)はありません") == true) &&
                    (string.IsNullOrEmpty(itemUrl) == true))
                {
                    /* なし の項目を削除する */
                    toolStripSplitButtonFavorite.DropDownItems.Remove(item);
                }
            }
            string title = this.webView2Main.CoreWebView2.DocumentTitle;
            string url = this.webView2Main.CoreWebView2.Source;
            addFavoriteItem(title, url);
            saveFavoriteData();
        }


        /// <summary>
        /// お気に入りのアイテムクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void favoriteItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            string url = item.Tag.ToString();
            try
            {
                webView2Main.CoreWebView2.Navigate(url);
            }
            catch(ArgumentException arg)
            {
                MessageBox.Show(string.Format("URLを確認してください（無効なURLです）\n URL: {0}", url));
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format("エラーが発生しました。\n URL: {0}", url));
            }
        }



        /// <summary>
        /// toolStripTextBoxUrlのキー入力イベント(IRONCADはKeyDownだとEnterが取れなかった)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripTextBoxUrl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                /* Enter押下でURLへ移動 */
                goInputURL();
            }
            if (this._useSelfCopyPaste == true)
            {
                /* IRONCADはコピー・貼り付け等が効かないので自作実装する */
                if (e.KeyCode == Keys.A && e.Control == true)
                {
                    toolStripTextBoxUrl.SelectAll();
                }
                if (e.KeyCode == Keys.V && e.Control == true)
                {
                    string temp = Clipboard.GetText();
                    int selectIndex = toolStripTextBoxUrl.SelectionStart;
                    string currStr = toolStripTextBoxUrl.Text;
                    int insertLength = temp.Length;
                    currStr = currStr.Insert(selectIndex, temp);
                    toolStripTextBoxUrl.Text = currStr;
                    toolStripTextBoxUrl.SelectionStart = selectIndex + insertLength;
                }
                if (e.KeyCode == Keys.C && e.Control == true)
                {
                    bool ret = icapiCommon.CopyToClipBoard(toolStripTextBoxUrl.SelectedText);
                    if (ret != true)
                    {
                        MessageBox.Show("クリップボードへのコピーに失敗しました。");
                        return;
                    }
                }
                if (e.KeyCode == Keys.X && e.Control == true)
                {
                    if (toolStripTextBoxUrl.SelectionLength > 0)
                    {
                        int selectIndex = toolStripTextBoxUrl.SelectionStart;
                        string currString = toolStripTextBoxUrl.Text;
                        string cutString = currString.Substring(selectIndex, toolStripTextBoxUrl.SelectionLength);
                        string newString = currString.Remove(toolStripTextBoxUrl.SelectionStart, toolStripTextBoxUrl.SelectionLength);
                        bool ret = icapiCommon.CopyToClipBoard(cutString);
                        if (ret != true)
                        {
                            MessageBox.Show("切り取りに失敗しました。（クリップボードへのコピーに失敗しました。）");
                            return;
                        }
                        toolStripTextBoxUrl.Text = newString;
                        toolStripTextBoxUrl.SelectionStart = selectIndex;
                    }
                }
                if (e.KeyCode == Keys.Z && e.Control == true)
                {

                }
            }
        }


        /// <summary>
        /// ユーザーコントロールのサイズ変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlWebBrowser_Resize(object sender, EventArgs e)
        {
            /* URL入力(toolStripTextBoxUrl)の幅を調整する */
            int headerWidth = this.toolStripHeader.Width;
            int allHeaderButtonsWidth = this.toolStripButtonBack.Width + this.toolStripButtonPrevious.Width + this.toolStripButtonReload.Width +
                                        this.toolStripButtonHome.Width + this.toolStripButtonUrlGo.Width + this.toolStripSplitButtonFavorite.Width +
                                        this.toolStripButtonUserGuide.Width + this.toolStripButtonABC.Width +
                                        textboxUrlMinWidth + 70;
            if(headerWidth > allHeaderButtonsWidth)
            {
                int newWidth = (headerWidth - (allHeaderButtonsWidth - textboxUrlMinWidth));
                if(newWidth > 600)
                {
                    newWidth = 600;
                }
                this.toolStripTextBoxUrl.Width = newWidth;
            }
            else
            {
                this.toolStripTextBoxUrl.Width = textboxUrlMinWidth;
            }
            if(urlDeleteButton != null)
            {
                /* toolStripTextBoxUrlのURLテキストの削除ボタンも場所を調整する */
                urlDeleteButton.Location = new Point(toolStripTextBoxUrl.Width - (urlDeleteButton.Width + 5), 0);
            }
        }


        /// <summary>
        /// ユーザガイドボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonUserGuide_Click(object sender, EventArgs e)
        {
            webView2Main.CoreWebView2.Navigate("https://ironcad.jp/guide/");
        }


        /// <summary>
        /// ABC for IRONCAD ボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonABC_Click(object sender, EventArgs e)
        {
            webView2Main.CoreWebView2.Navigate("https://abc.ironcad.jp/");
        }


        /// <summary>
        /// お気に入りを編集のボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditFavorite_Click(object sender, EventArgs e)
        {
            string configPath = getFavoriteConfigPath();
            if (File.Exists(configPath) != true)
            {
                string parentDir = Path.GetDirectoryName(configPath);
                if (Directory.Exists(parentDir) != true)
                {
                    Directory.CreateDirectory(parentDir);
                }
                StreamWriter sw = new StreamWriter(configPath);
                sw.Close();
            }
            Process.Start("notepad.exe", configPath);
        }


        /// <summary>
        /// お気に入りを再読み込みのボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReloadFavorite_Click(object sender, EventArgs e)
        {
            InitializeFavorite();
        }
        #endregion イベント
    }
}
