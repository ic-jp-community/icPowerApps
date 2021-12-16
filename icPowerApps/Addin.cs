using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using interop.ICApiIronCAD;
using System.Diagnostics;

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

#region [IZAddinServer Members]
        public void InitSelf(ZAddinSite piAddinSite)
        {
            if (piAddinSite != null)
            {
                m_izAddinSite = piAddinSite;
                try
                {
                    //ボタンの作成(Form)
                    stdole.IPictureDisp oImageSmall = ConvertImage.ImageToPictureDisp(Properties.Resources.icon_icPowerApps_s);
                    stdole.IPictureDisp oImageLarge = ConvertImage.ImageToPictureDisp(Properties.Resources.icon_icPowerApps_l);
                    m_buttonIcPowerAppsTestApp = piAddinSite.CreateCommandHandler("icPowerAppsTestApp", "icPowerAppsTestApp", "icPowerAppsTestApp", "icPowerAppsのTest確認用Appです。", oImageSmall, oImageLarge);
                    m_buttonIcPowerAppsTestApp.Enabled = true;

                    stdole.IPictureDisp oImageSmallBrowser = ConvertImage.ImageToPictureDisp(Properties.Resources.icon_icWebBrowser_s);
                    stdole.IPictureDisp oImageLargeBrowser = ConvertImage.ImageToPictureDisp(Properties.Resources.icon_icWebBrowser_l);
                    m_buttonIcWebBrowser = piAddinSite.CreateCommandHandler("icWebBrowser", "icWebブラウザ", "icWebブラウザ", "ブラウザを表示します。", oImageSmallBrowser, oImageLargeBrowser);
                    m_buttonIcWebBrowser.Enabled = true;


                    stdole.IPictureDisp oImageSmallSuppress = ConvertImage.ImageToPictureDisp(Properties.Resources.icon_icSuppressManager_s);
                    stdole.IPictureDisp oImageLargeSuppress = ConvertImage.ImageToPictureDisp(Properties.Resources.icon_icSuppressManager_l);
                    m_buttonSuppressManager = piAddinSite.CreateCommandHandler("icSuppressManager", "抑制マネージャ", "抑制マネージャ", "抑制マネージャを表示します。", oImageSmallSuppress, oImageLargeSuppress);
                    m_buttonSuppressManager.Enabled = true;

                    stdole.IPictureDisp oImageSmallCustomProperty = ConvertImage.ImageToPictureDisp(Properties.Resources.icon_CustomPropertyManager_s);
                    stdole.IPictureDisp oImageLargeCustomProperty = ConvertImage.ImageToPictureDisp(Properties.Resources.icon_CustomPropertyManager_l);
                    m_buttonCustomPropertyManager = piAddinSite.CreateCommandHandler("icCustomPropertyManager", "カスタムプロパティ マネージャ", "カスタムプロパティ マネージャ", "カスタムプロパティ マネージャを表示します。", oImageSmallCustomProperty, oImageLargeCustomProperty);
                    m_buttonCustomPropertyManager.Enabled = true;


                    //Control bar
                    ZControlBar cControlBar;
                    ZEnvironmentMgr cEnvMgr = this.IronCADApp.EnvironmentMgr;
                    ZControls cControls;
                    IZControl cControl;
                    ZRibbonBar cRibbonBar;

                    //ツールバーを作成する
                    IZEnvironment cEnv = cEnvMgr.get_Environment(eZEnvType.Z_ENV_SCENE);
                    cRibbonBar = cEnv.GetRibbonBar(eZRibbonBarType.Z_RIBBONBAR);
                    cControlBar = cEnv.AddControlBar(piAddinSite, "icPowerApps");
                    cControls = cControlBar.Controls;
                    cControl = cControls.Add(ezControlType.Z_CONTROL_BUTTON, m_buttonIcPowerAppsTestApp.ControlDescriptor, null);
                    cControl = cControls.Add(ezControlType.Z_CONTROL_BUTTON, m_buttonIcWebBrowser.ControlDescriptor, null);
                    cControl = cControls.Add(ezControlType.Z_CONTROL_BUTTON, m_buttonSuppressManager.ControlDescriptor, null);
                    cControl = cControls.Add(ezControlType.Z_CONTROL_BUTTON, m_buttonCustomPropertyManager.ControlDescriptor, null);

                    //Add button to RibbonBar
                    cRibbonBar.AddButton2(m_buttonIcPowerAppsTestApp.ControlDescriptor, true);
                    cRibbonBar.AddButton2(m_buttonIcWebBrowser.ControlDescriptor, true);
                    cRibbonBar.AddButton2(m_buttonSuppressManager.ControlDescriptor, true);
                    cRibbonBar.AddButton2(m_buttonCustomPropertyManager.ControlDescriptor, true);
                    //                    cRibbonBar.AddButton2(m_buttonForm.ControlDescriptor, false);

                    /************************************************************
                      リボンバーに大きいボタンで表示させたい時はこっち↓を使用する
                      cRibbonBar.AddButton2(m_button.ControlDescriptor, true);
                    *************************************************************/


                    //Event handlers
                    m_buttonIcPowerAppsTestApp.OnClick += new _IZCommandEvents_OnClickEventHandler(m_buttonForm_OnClick);
                    m_buttonIcPowerAppsTestApp.OnUpdate += new _IZCommandEvents_OnUpdateEventHandler(m_buttonForm_OnUpdate);

                    m_buttonIcWebBrowser.OnClick += new _IZCommandEvents_OnClickEventHandler(m_buttonIcWebBrowser_OnClick);
                    m_buttonIcWebBrowser.OnUpdate += new _IZCommandEvents_OnUpdateEventHandler(m_buttonIcWebBrowser_OnUpdate);

                    m_buttonSuppressManager.OnClick += new _IZCommandEvents_OnClickEventHandler(m_buttonSuppressManager_OnClick);
                    m_buttonSuppressManager.OnUpdate += new _IZCommandEvents_OnUpdateEventHandler(m_buttonSuppressManager_OnUpdate);

                    m_buttonCustomPropertyManager.OnClick += new _IZCommandEvents_OnClickEventHandler(m_buttonCustomPropertyManager_OnClick);
                    m_buttonCustomPropertyManager.OnUpdate += new _IZCommandEvents_OnUpdateEventHandler(m_buttonCustomPropertyManager_OnUpdate);

                    //Register App Events
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


        public void DeInitSelf()
        {
            m_buttonIcPowerAppsTestApp = null;
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

        private void m_buttonForm_OnUpdate()
        {
            m_buttonIcPowerAppsTestApp.Enabled = true;  //Change to m_button.Enabled = false; to disable the button  
        }
        private void m_buttonIcWebBrowser_OnUpdate()
        {
            m_buttonIcWebBrowser.Enabled = true;  //Change to m_button.Enabled = false; to disable the button  
        }
        private void m_buttonSuppressManager_OnUpdate()
        {
            m_buttonSuppressManager.Enabled = true;  //Change to m_button.Enabled = false; to disable the button  
        }
        private void m_buttonCustomPropertyManager_OnUpdate()
        {
            m_buttonCustomPropertyManager.Enabled = true;  //Change to m_button.Enabled = false; to disable the button  
        }
        private void m_buttonForm_OnClick()
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
            UserControlWebBrowser uc = new UserControlWebBrowser(true, true);
            uc.SetBounds(0, 0, width, height);
            IntPtr cwnd = HwndToCwnd(uc.Handle);
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
            UserControlSuppressManager uc = new UserControlSuppressManager(IronCADApp);
            uc.SetBounds(left, top, right - left, bottom - top);
            IntPtr cwnd = HwndToCwnd(uc.Handle);
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
