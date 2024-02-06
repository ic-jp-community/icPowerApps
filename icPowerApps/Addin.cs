using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using interop.ICApiIronCAD;
using System.Diagnostics;
using static ICApiAddin.icPowerApps.icPowerAppsSetting;
using System.IO;

namespace ICApiAddin.icPowerApps
{
    [Guid("6AE87CEF-C966-4938-A945-40D4280F6001"), ClassInterface(ClassInterfaceType.None), ProgId("icPowerApps.AddIn")]
    public class Addin : IZAddinServer
    {
        public const string ADDIN_GUID = "6AE87CEF-C966-4938-A945-40D4280F6001";
        public const string ADDIN_APP_NAME = "icPowerApps";
        public const string ADDIN_APP_DESCRIPTION= "IRONCADをさらに便利に使うためのツール群です。";

        #region [Private Members]
        private ZAddinSite m_izAddinSite;
        private ZCommandHandler m_buttonIcPowerAppsTestApp;
        private ZCommandHandler m_buttonIcWebBrowser;
        private ZCommandHandler m_buttonSuppressManager;
        private ZCommandHandler m_buttonCustomPropertyManager;
        private ZCommandHandler m_buttonSceneBrowserTreeSort;
        private ZCommandHandler m_buttonExternalLinkManager;
        private ZCommandHandler m_buttonElementManager;
        private ZCommandHandler m_buttonSetting;
        #endregion

        //Constractor
        public Addin()
        {
#if ADDIN_INIT_DEBUG
            /* アドインロードのデバッグ用 */
            System.Threading.Thread.Sleep(120 * 1000);
#endif           
        }

#region [Public Properties]
        public IZBaseApp IronCADApp
        {
            get
            {
                if (m_izAddinSite != null)
                    return m_izAddinSite.Application;
                return null;
            }

        }

        #endregion

        /// <summary>
        /// This function is called by IRONCAD when the Addin is unloaded
        /// </summary>
        public void DeInitSelf()
        {
            //ハンドラの登録
            foreach (AddInToolData tool in addInToolDataList)
            {
                try
                {
                    if (tool.button != null)
                    {
                        tool.button.OnClick -= tool.onClick;
                        tool.button.OnUpdate -= tool.onUpdate;
                    }
                }
                catch
                {

                }
            }
        }
        public class AddInToolData
        {
            public System.Drawing.Bitmap smallImage;
            public System.Drawing.Bitmap largeImage;
            public string uniqueName { get; set; }
            public string dispName { get; set; }
            public string statusBarName;
            public string toolTip;
            public bool isLargeIcon;
            public bool isEnable;
            public ZCommandHandler button;
            public _IZCommandEvents_OnClickEventHandler onClick;
            public _IZCommandEvents_OnUpdateEventHandler onUpdate;
            public AddInToolData(System.Drawing.Bitmap smallImage, System.Drawing.Bitmap largeImage, 
                                string uniqueName, string dispName, string statusBarName, string toolTip,
                                bool isLargeIcon, bool isEnable, ZCommandHandler button,
                                _IZCommandEvents_OnClickEventHandler onClick, _IZCommandEvents_OnUpdateEventHandler onUpdate)
            {
                this.smallImage = smallImage;
                this.largeImage = largeImage;
                this.uniqueName = uniqueName;
                this.statusBarName = statusBarName;
                this.dispName = dispName;
                this.toolTip = toolTip;
                this.isLargeIcon = isLargeIcon;
                this.button = button;
                this.onClick = onClick;
                this.onUpdate = onUpdate;
                this.isEnable = isEnable;
            }
        }
        public List<AddInToolData> addinAddDataList = new List<AddInToolData>();

        public void SetToolCommands(List<AddInToolData> addInDataList)
        {                    //Control bar
            ZEnvironmentMgr cEnvMgr = this.IronCADApp.EnvironmentMgr;
            IZEnvironment cEnv = cEnvMgr.get_Environment(eZEnvType.Z_ENV_SCENE);
            ZRibbonBar cRibbonBar = cEnv.GetRibbonBar(eZRibbonBarType.Z_RIBBONBAR);
            ZControlBar cControlBar;
            ZControls cControls;
            cControlBar = cEnv.AddControlBar(m_izAddinSite, "icPowerApps");
            cControls = cControlBar.Controls;

            List<AddInToolData> reOrderAddInDataList = addInDataList.Where(a=> a.isEnable == true).OrderByDescending(a => a.isLargeIcon).ToList();

            //ボタンの作成(Form)
            foreach(AddInToolData tool in reOrderAddInDataList)
            {
                stdole.IPictureDisp oImageSmall = ConvertImage.ImageToPictureDisp(tool.smallImage);
                stdole.IPictureDisp oImageLarge = ConvertImage.ImageToPictureDisp(tool.largeImage);
                tool.button = m_izAddinSite.CreateCommandHandler(tool.uniqueName, tool.dispName, tool.statusBarName, tool.toolTip, oImageSmall, oImageLarge);
                cRibbonBar.AddButton2(tool.button.ControlDescriptor, tool.isLargeIcon);
                tool.button.Enabled = true;
            }

            //ツールバーを作成する
            foreach (AddInToolData tool in reOrderAddInDataList)
            {
                cControls.Add(ezControlType.Z_CONTROL_BUTTON, tool.button.ControlDescriptor, null);
            }

            //ハンドラの登録
            foreach (AddInToolData tool in reOrderAddInDataList)
            {
                tool.button.OnClick += tool.onClick;
                tool.button.OnUpdate += tool.onUpdate;
            }

        }

        public List<AddInToolData> addInToolDataList = new List<AddInToolData>();
        #region [IZAddinServer Members]
        public void InitSelf(ZAddinSite piAddinSite)
        {
            if (piAddinSite != null)
            {
                m_izAddinSite = piAddinSite;
                try
                {
                    /* 現在の設定ファイルを読み込む(共通設定) */
                    string userConfigPath = icPowerAppsSetting.GetUserConfigFilePath();
                    if (File.Exists(userConfigPath) != true)
                    {
                        /* 共通コンフィグファイルが無いので作成する */
                        icPowerAppsSetting.WriteicPowerAppsUserSetting(userConfigPath);
                    }
                    icPowerAppsSetting.ReadicPowerAppsUserSetting(userConfigPath, false);


                    bool isLargeIcon = false;

                    //ボタンの作成(Form)
                    //addInToolDataList.Add(new AddInToolData(Properties.Resources.icon_icPowerApps_s, Properties.Resources.icon_icPowerApps_l,
                    //                                        "icPowerAppsTestApp", "icPowerAppsTestApp", "icPowerAppsTestApp", "icPowerAppsのTest確認用Appです。",
                    //                                        getIconSizeIsLarge("icPowerAppsTestApp"), getToolIsEnable("icPowerAppsTestApp"), m_buttonIcPowerAppsTestApp,
                    //                                        new _IZCommandEvents_OnClickEventHandler(m_buttonIcPowerAppsTestApp_OnClick), new _IZCommandEvents_OnUpdateEventHandler(m_buttonIcPowerAppsTestApp_OnUpdate)));

                    addInToolDataList.Add(new AddInToolData(Properties.Resources.icon_icWebBrowser_s, Properties.Resources.icon_icWebBrowser_l,
                                                             "icWebBrowser", "icWebブラウザ", "icWebブラウザ", "ブラウザを表示します。",
                                                              getIconSizeIsLarge("icWebブラウザ"), getToolIsEnable("icWebブラウザ"), m_buttonIcWebBrowser,
                                                                new _IZCommandEvents_OnClickEventHandler(m_buttonIcWebBrowser_OnClick), new _IZCommandEvents_OnUpdateEventHandler(m_buttonIcWebBrowser_OnUpdate)));

                    addInToolDataList.Add(new AddInToolData(Properties.Resources.icon_icSuppressManager_s, Properties.Resources.icon_icSuppressManager_l,
                                                             "icSuppressManager", "抑制マネージャ", "抑制マネージャ", "抑制マネージャを表示します。",
                                                             getIconSizeIsLarge("抑制マネージャ"), getToolIsEnable("抑制マネージャ"), m_buttonSuppressManager,
                                                                new _IZCommandEvents_OnClickEventHandler(m_buttonSuppressManager_OnClick), new _IZCommandEvents_OnUpdateEventHandler(m_buttonSuppressManager_OnUpdate)));

                    addInToolDataList.Add(new AddInToolData(Properties.Resources.icon_CustomPropertyManager_s, Properties.Resources.icon_CustomPropertyManager_l,
                                                            "icCustomPropertyManager", "カスタムプロパティ マネージャ", "カスタムプロパティ マネージャ", "カスタムプロパティ マネージャを表示します。",
                                                             getIconSizeIsLarge("カスタムプロパティ マネージャ"), getToolIsEnable("カスタムプロパティ マネージャ"), m_buttonCustomPropertyManager,
                                                                new _IZCommandEvents_OnClickEventHandler(m_buttonCustomPropertyManager_OnClick), new _IZCommandEvents_OnUpdateEventHandler(m_buttonCustomPropertyManager_OnUpdate)));

                    addInToolDataList.Add(new AddInToolData(Properties.Resources.icon_SceneBrowserTreeSort_s, Properties.Resources.icon_SceneBrowserTreeSort_l,
                                        "icSceneBrowserTreeSort", "シーンブラウザ 並び替え", "シーンブラウザ 並び替え", "シーンブラウザ 並び替えを表示します。",
                                         getIconSizeIsLarge("シーンブラウザ 並び替え"), getToolIsEnable("シーンブラウザ 並び替え"), m_buttonSceneBrowserTreeSort,
                                          new _IZCommandEvents_OnClickEventHandler(m_buttonSceneBrowserTreeSort_OnClick), new _IZCommandEvents_OnUpdateEventHandler(m_buttonSceneBrowserTreeSort_OnUpdate)));

                    addInToolDataList.Add(new AddInToolData(Properties.Resources.icon_ExternalLinkManager_s, Properties.Resources.icon_ExternalLinkManager_l,
                                                            "icExternalLinkManager", "外部リンク マネージャ", "外部リンク マネージャ", "外部リンク マネージャを表示します。",
                                                             getIconSizeIsLarge("外部リンク マネージャ"), getToolIsEnable("外部リンク マネージャ"), m_buttonExternalLinkManager,
                                                              new _IZCommandEvents_OnClickEventHandler(m_buttonExternalLinkManager_OnClick), new _IZCommandEvents_OnUpdateEventHandler(m_buttonExternalLinkManager_OnUpdate)));

                    addInToolDataList.Add(new AddInToolData(Properties.Resources.Icon_ElementManager_s, Properties.Resources.Icon_ElementManager_l,
                                                             "icElementManager", "Element マネージャ", "Element マネージャ", "Elementマネージャを表示します。",
                                                             getIconSizeIsLarge("Element マネージャ"), getToolIsEnable("Element マネージャ"), m_buttonElementManager,
                                                                new _IZCommandEvents_OnClickEventHandler(m_buttonElementManager_OnClick), new _IZCommandEvents_OnUpdateEventHandler(m_buttonElementManager_OnUpdate)));

                    addInToolDataList.Add(new AddInToolData(Properties.Resources.icon_setting_s, Properties.Resources.icon_setting_l,
                                        "icPowerAppsSetting", "設定", "設定", "設定を表示します。",
                                         getIconSizeIsLarge("設定"), getToolIsEnable("設定"), m_buttonSetting,
                                         new _IZCommandEvents_OnClickEventHandler(m_buttonSetting_OnClick), new _IZCommandEvents_OnUpdateEventHandler(m_buttonSetting_OnUpdate)));

                    SetToolCommands(addInToolDataList);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Addin Server is null.");
            }
        }

        public bool getIconSizeIsLarge(string displayName)
        {
            icPowerAppsConfig config = icPowerAppsSetting.GetConfig();
            AddInToolIconSize tool = config.UserConfig.ClientConfig.AppList.Where(a => a.displayName == displayName).FirstOrDefault();
            if(tool == null)
            {
                return true;
            }
            return tool.isLargeIcon;
        }


        public bool getToolIsEnable(string displayName)
        {
            icPowerAppsConfig config = icPowerAppsSetting.GetConfig();
            AddInToolIconSize tool = config.UserConfig.ClientConfig.AppList.Where(a => a.displayName == displayName).FirstOrDefault();
            if (tool == null)
            {
                return true;
            }
            return tool.isEnable;
        }
        

        #endregion

        #region [Private Methods]
        [DllImport("icAPI_CppWrapper.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr HwndToCwnd(IntPtr hwnd);
        [DllImport("icAPI_CppWrapper.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CwndToHwnd(ulong cwnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int MoveWindow(IntPtr hwnd, int x, int y, int nWidth, int nHeight, int bRepaint);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        private void m_buttonIcPowerAppsTestApp_OnUpdate()
        {
            AddInToolData tool = addInToolDataList.Where(a => a.uniqueName == "icPowerAppsTestApp").FirstOrDefault();
            tool.button.Enabled = true;  
        }
        private void m_buttonIcWebBrowser_OnUpdate()
        {
            AddInToolData tool = addInToolDataList.Where(a => a.uniqueName == "icWebBrowser").FirstOrDefault();
            tool.button.Enabled = true;  
        }
        private void m_buttonSuppressManager_OnUpdate()
        {
            AddInToolData tool = addInToolDataList.Where(a => a.uniqueName == "icSuppressManager").FirstOrDefault();
            tool.button.Enabled = true;
        }
        private void m_buttonCustomPropertyManager_OnUpdate()
        {
            AddInToolData tool = addInToolDataList.Where(a => a.uniqueName == "icCustomPropertyManager").FirstOrDefault();
            tool.button.Enabled = true;
        }

        private void m_buttonSceneBrowserTreeSort_OnUpdate()
        {
            AddInToolData tool = addInToolDataList.Where(a => a.uniqueName == "icSceneBrowserTreeSort").FirstOrDefault();
            tool.button.Enabled = true;
        }

        private void m_buttonExternalLinkManager_OnUpdate()
        {
            AddInToolData tool = addInToolDataList.Where(a => a.uniqueName == "icExternalLinkManager").FirstOrDefault();
            tool.button.Enabled = true;
        }

        private void m_buttonElementManager_OnUpdate()
        {
            AddInToolData tool = addInToolDataList.Where(a => a.uniqueName == "icElementManager").FirstOrDefault();
            tool.button.Enabled = true;
        }


        private void m_buttonSetting_OnUpdate()
        {
            AddInToolData tool = addInToolDataList.Where(a => a.uniqueName == "icPowerAppsSetting").FirstOrDefault();
            tool.button.Enabled = true;  
        }


        private void m_buttonIcPowerAppsTestApp_OnClick()
        {
            IZDoc doc = GetActiveDoc();
            IZEnvironmentMgr iZEnvMgr = GetEnvironmentMgr();
            icPowerAppsTEST frm = new icPowerAppsTEST(this.IronCADApp);
            frm.ShowDialog();
        }

        private const short SWP_NOMOVE = 0X2;
        private const short SWP_NOSIZE = 1;
        private const short SWP_NOZORDER = 0X4;
        private const int SWP_SHOWWINDOW = 0x0040;
        private const int AFX_IDW_DOCKBAR_TOP = 0xE81B;
        private const int AFX_IDW_DOCKBAR_LEFT = 0xE81C;
        private const int AFX_IDW_DOCKBAR_RIGHT = 0xE81D;
        private const int AFX_IDW_DOCKBAR_BOTTOM = 0xE81E;
        private const int AFX_IDW_DOCKBAR_FLOAT = 0xE81F;

        private string GetWindowTitle(IntPtr hWnd)
        {
            var length = GetWindowTextLength(hWnd) + 1;
            var title = new StringBuilder(length);
            GetWindowText(hWnd, title, length);
            return title.ToString();
        }

        private UserControlSuppressManager ucSuppressManager = null;
        private UserControlElementManager ucElementManager = null;
        private UserControlExternalLinkManager ucExternalLinkManager = null;
        private UserControlSceneBrowserTreeSort ucSceneBrowserTreeSort = null;
        private UserControlWebBrowser ucIcWebBrowser = null;

        private void m_buttonIcWebBrowser_OnClick()
        {
            IZAddinSite addinSite = m_izAddinSite;
            IZDockingBar dockingBar;
            IZEnvironmentMgr envMgr = GetEnvironmentMgr();
            IZEnvironment env = envMgr.ActiveEnvironment;
            uint dockPosition = AFX_IDW_DOCKBAR_FLOAT;

            RECT appRect;
            IntPtr appHWndHandle = Process.GetCurrentProcess().MainWindowHandle;
            GetWindowRect(appHWndHandle, out appRect);

            /* DockingBarの追加 */
            dockingBar = env.AddDockingBar((ZAddinSite)addinSite, 1, "icWebブラウザ", dockPosition);

            /* IRONCADウィンドウの半分のサイズで真ん中に設定(未表示)する */
            IntPtr dockingBarHWnd = CwndToHwnd(dockingBar.GetCWnd());
            int height = (appRect.Bottom - appRect.Top) / 2;
            int width = (appRect.Right - appRect.Left) / 2;
            int startY = (appRect.Bottom - appRect.Top) / 4;
            int startX = (appRect.Right - appRect.Left) / 4;
            SetWindowPos(dockingBarHWnd, 0, startX, startY, width, height, 0);

            /* ユーザコントロールをDockingBarにマッピング */
            ucIcWebBrowser = new UserControlWebBrowser(true, true);
            ucIcWebBrowser.SetBounds(0, 0, width, height);
            IntPtr cwnd = HwndToCwnd(ucIcWebBrowser.Handle);
            dockingBar.SetSubWindow((ulong)cwnd);
            if(dockPosition == AFX_IDW_DOCKBAR_FLOAT)
            {
                /* FLOATの場合はSWP_SHOWWINDOWをしないと表示されなかった */
                SetWindowPos(dockingBarHWnd, 0, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
            }
            dockingBar.ShowControlBar(1, 1, 1);
        }

        private void m_buttonSuppressManager_OnClick()
        {
            IZAddinSite addinSite = m_izAddinSite;
            IZDockingBar dockingBar;
            IZEnvironmentMgr envMgr = GetEnvironmentMgr();
            IZEnvironment env = envMgr.ActiveEnvironment;
            uint dockPosition = AFX_IDW_DOCKBAR_RIGHT;
            int left, top, right, bottom;

            /* DockingBarの追加 */
            dockingBar = env.AddDockingBar((ZAddinSite)addinSite, 1, "抑制マネージャ", dockPosition);
            dockingBar.GetClientRect(out left, out top, out right, out bottom);

            /* ユーザコントロールをDockingBarにマッピング */
            ucSuppressManager = new UserControlSuppressManager(IronCADApp);
            ucSuppressManager.SetBounds(left, top, right - left, bottom - top);
            IntPtr cwnd = HwndToCwnd(ucSuppressManager.Handle);
            dockingBar.SetSubWindow((ulong)cwnd);
            dockingBar.ShowControlBar(1, 1, 1);
        }


        private void m_buttonCustomPropertyManager_OnClick()
        {
            IZDoc doc = GetActiveDoc();
            IZEnvironmentMgr iZEnvMgr = GetEnvironmentMgr();
            Form_CustomPropertyManager frm = new Form_CustomPropertyManager(this.IronCADApp);
            frm.ShowDialog();
        }

        private void m_buttonSceneBrowserTreeSort_OnClick()
        {
            IZAddinSite addinSite = m_izAddinSite;
            IZDockingBar dockingBar;
            IZEnvironmentMgr envMgr = GetEnvironmentMgr();
            IZEnvironment env = envMgr.ActiveEnvironment;
            uint dockPosition = AFX_IDW_DOCKBAR_FLOAT;
            int left, top, right, bottom;

            /* ユーザーコントロール */
            ucSceneBrowserTreeSort = new UserControlSceneBrowserTreeSort(IronCADApp);
            ucSceneBrowserTreeSort.Width = (int)(ucSceneBrowserTreeSort.Width * ScaleReziser.getScalingFactor());
            ucSceneBrowserTreeSort.Height = (int)(ucSceneBrowserTreeSort.Height * ScaleReziser.getScalingFactor());
            /* DockingBarの追加 */
            dockingBar = env.AddDockingBar((ZAddinSite)addinSite, 1, "シーンブラウザ 並び替え", dockPosition);
            dockingBar.GetClientRect(out left, out top, out right, out bottom);

            /* IRONCADウィンドウの半分のサイズで真ん中に設定(未表示)する */
            RECT appRect;
            IntPtr appHWndHandle = Process.GetCurrentProcess().MainWindowHandle;
            GetWindowRect(appHWndHandle, out appRect);
            IntPtr dockingBarHWnd = CwndToHwnd(dockingBar.GetCWnd());
            //int height = (appRect.Bottom - appRect.Top) / 2;
            //int width = (appRect.Right - appRect.Left) / 2;
            int startY = (appRect.Bottom - appRect.Top) / 4;
            int startX = (appRect.Right - appRect.Left) / 4;
            SetWindowPos(dockingBarHWnd, 0, startX, startY, ucSceneBrowserTreeSort.Width, ucSceneBrowserTreeSort.Height+40, 0);

            /* ユーザコントロールをDockingBarにマッピング */
            ucSceneBrowserTreeSort.SetBounds(0, 0, ucSceneBrowserTreeSort.Width, ucSceneBrowserTreeSort.Height);
            IntPtr cwnd = HwndToCwnd(ucSceneBrowserTreeSort.Handle);
            dockingBar.SetSubWindow((ulong)cwnd);
            if (dockPosition == AFX_IDW_DOCKBAR_FLOAT)
            {
                /* FLOATの場合はSWP_SHOWWINDOWをしないと表示されなかった */
                SetWindowPos(dockingBarHWnd, 0, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
            }
            dockingBar.ShowControlBar(1, 1, 1);
        }

        private void m_buttonExternalLinkManager_OnClick()
        {
            IZAddinSite addinSite = m_izAddinSite;
            IZDockingBar dockingBar;
            IZEnvironmentMgr envMgr = GetEnvironmentMgr();
            IZEnvironment env = envMgr.ActiveEnvironment;
            uint dockPosition = AFX_IDW_DOCKBAR_FLOAT;
            int left, top, right, bottom;

            /* ユーザーコントロール */
            ucExternalLinkManager = new UserControlExternalLinkManager(IronCADApp);
            ucExternalLinkManager.Width = (int)(ucExternalLinkManager.Width * ScaleReziser.getScalingFactor());
            ucExternalLinkManager.Height = (int)(ucExternalLinkManager.Height * ScaleReziser.getScalingFactor());

            /* DockingBarの追加 */
            dockingBar = env.AddDockingBar((ZAddinSite)addinSite, 1, "外部リンク マネージャ", dockPosition);
            dockingBar.GetClientRect(out left, out top, out right, out bottom);

            /* IRONCADウィンドウの半分のサイズで真ん中に設定(未表示)する */
            RECT appRect;
            IntPtr appHWndHandle = Process.GetCurrentProcess().MainWindowHandle;
            GetWindowRect(appHWndHandle, out appRect);
            IntPtr dockingBarHWnd = CwndToHwnd(dockingBar.GetCWnd());
            //int height = (appRect.Bottom - appRect.Top) / 2;
            //int width = (appRect.Right - appRect.Left) / 2;
            int startY = (appRect.Bottom - appRect.Top) / 4;
            int startX = (appRect.Right - appRect.Left) / 4;
            SetWindowPos(dockingBarHWnd, 0, startX, startY, ucExternalLinkManager.Width, ucExternalLinkManager.Height+40, 0);

            /* ユーザコントロールをDockingBarにマッピング */
            ucExternalLinkManager.SetBounds(0, 0, ucExternalLinkManager.Width, ucExternalLinkManager.Height);
            IntPtr cwnd = HwndToCwnd(ucExternalLinkManager.Handle);
            dockingBar.SetSubWindow((ulong)cwnd);
            if (dockPosition == AFX_IDW_DOCKBAR_FLOAT)
            {
                /* FLOATの場合はSWP_SHOWWINDOWをしないと表示されなかった */
                SetWindowPos(dockingBarHWnd, 0, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
            }
            dockingBar.ShowControlBar(1, 1, 1);
        }
        
        private void m_buttonElementManager_OnClick()
        {
            IZAddinSite addinSite = m_izAddinSite;
            IZEnvironmentMgr envMgr = GetEnvironmentMgr();
            IZEnvironment env = envMgr.ActiveEnvironment;
            System.IntPtr iHwnd = (IntPtr)this.IronCADApp.Frame.HWND;
            Win32HWNDWrapper cWin32HWNDWrapper = new Win32HWNDWrapper(iHwnd);
            /* ElementManagerを作成 */
            ucElementManager = new UserControlElementManager(IronCADApp);
            Form frm = new Form();
            frm.Width = ucElementManager.Width + 20;
            frm.Height = ucElementManager.Height;
            frm.Controls.Add(ucElementManager);
            frm.Icon = Properties.Resources.Icon_ElementManager;
            frm.Text = UserControlElementManager.title;
            frm.TopMost = true;
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show(cWin32HWNDWrapper);
        }

        private void m_buttonSetting_OnClick()
        {
            IZDoc doc = GetActiveDoc();
            IZEnvironmentMgr iZEnvMgr = GetEnvironmentMgr();
            Form_icPowerAppsSetting frm = new Form_icPowerAppsSetting(this.IronCADApp, addInToolDataList);
            frm.ShowDialog();
        }

        private IZDoc GetActiveDoc()
        {
            if (this.IronCADApp != null)
            {
                return this.IronCADApp.ActiveDoc;
            }
            return null;
        }

        private IZEnvironmentMgr GetEnvironmentMgr()
        {
            if (this.IronCADApp != null)
            {
                return this.IronCADApp.EnvironmentMgr;
            }
            return null;
        }

#endregion

#region [Internal Methods]

        internal static List<IZElement> ConvertObjectToElementArray(object varElements)
        {
            if (varElements != null)
            {
                object[] oElements = varElements as object[];
                if (oElements != null)
                {
                    List<IZElement> izElements = new List<IZElement>();
                    foreach(object oEle in oElements)
                    {
                        IZElement izEle = oEle as IZElement;
                        if (izEle != null)
                        {
                            izElements.Add(izEle);
                        }
                    }
                    return izElements;
                }
            }
            return null;
        }

#endregion

    }
}
