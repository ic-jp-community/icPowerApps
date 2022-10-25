using AdvancedDataGridView;
using ICApiAddin.icPowerApps.Properties;
using interop.ICApiIronCAD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICApiAddin.icPowerApps
{
    /// <summary>
    /// ユーザーコントロールのTag格納データ
    /// </summary>
    public class UserControlTagData
    {
        public bool canNotClose;
        public UserControlTagData()
        {
            this.canNotClose = false;
        }
    }

    /// <summary>
    /// IRONCADの各フォルダ・レジストリパス
    /// </summary>
    public class IRONCAD_REG_AND_DIR
    {
        public string version { get; set; }
        public string installDir { get; set; }
        public string cacheDirRoaming;
        public string cacheDirLocal;
        public string cacheDirRoamingIRONCAD;
        public string cacheDirLocalIRONCAD;
        public string cacheDirRoamingINOVATE;
        public string cacheDirLocalINOVATE;
        public string cacheDirRoamingCOMPOSE;
        public string cacheDirLocalCOMPOSE;
        public string cacheDirRoamingDRAFT;
        public string cacheDirLocalDRAFT;

        public string cacheDirRoamingCaxaIC;
        public string cacheDirLocalCaxaIC;
        public string cacheDirRoamingCaxaIntDRAFT;
        public string cacheDirLocalCaxaIntDRAFT;
        public string cacheDirRoamingCaxaIntINOVATE;
        public string cacheDirLocalCaxaIntINOVATE;
        public string cacheDirRoamingCaxaIntIC;
        public string cacheDirLocalCaxaIntIC;
        public string cacheDirRoamingCaxaPrintTool;
        public string cacheDirLocalCaxaPrintTool;
        public string cacheDirRoamingCaxaExbViewr;
        public string cacheDirLocalCaxaExbViewr;

        public string regPathUserIRONCAD;
        public string regPathMachineIRONCAD;
        public string regPathMachineIRONCAD_NewUser;
        public string regPathUserINOVATE;
        public string regPathMachineINOVATE;
        public string regPathMachineINOVATE_NewUser;
        public string regPathUserCOMPOSE;
        public string regPathMachineCOMPOSE;
        public string regPathMachineCOMPOSE_NewUser;
        public string regPathUserDRAFT;
        public string regPathMachineDRAFT;
        public string regPathMachineDRAFT_NewUser;

        public IRONCAD_REG_AND_DIR()
        {
            this.version = string.Empty;
            this.installDir = string.Empty;
            this.cacheDirRoaming = string.Empty;
            this.cacheDirLocal = string.Empty;
            this.cacheDirRoamingIRONCAD = string.Empty;
            this.cacheDirLocalIRONCAD = string.Empty;
            this.cacheDirRoamingINOVATE = string.Empty;
            this.cacheDirLocalINOVATE = string.Empty;
            this.cacheDirRoamingCOMPOSE = string.Empty;
            this.cacheDirLocalCOMPOSE = string.Empty;
            this.cacheDirRoamingDRAFT = string.Empty;
            this.cacheDirLocalDRAFT = string.Empty;

            this.cacheDirRoamingCaxaIC = string.Empty;
            this.cacheDirRoamingCaxaIntDRAFT = string.Empty;
            this.cacheDirRoamingCaxaIntINOVATE = string.Empty;
            this.cacheDirRoamingCaxaIntIC = string.Empty;
            this.cacheDirRoamingCaxaPrintTool = string.Empty;
            this.cacheDirRoamingCaxaExbViewr = string.Empty;

            this.cacheDirLocalCaxaIC = string.Empty;
            this.cacheDirLocalCaxaIntDRAFT = string.Empty;
            this.cacheDirLocalCaxaIntINOVATE = string.Empty;
            this.cacheDirLocalCaxaIntIC = string.Empty;
            this.cacheDirLocalCaxaPrintTool = string.Empty;
            this.cacheDirLocalCaxaExbViewr = string.Empty;

            this.regPathUserIRONCAD = string.Empty;
            this.regPathMachineIRONCAD = string.Empty;
            this.regPathMachineIRONCAD_NewUser = string.Empty;
            this.regPathUserINOVATE = string.Empty;
            this.regPathMachineINOVATE = string.Empty;
            this.regPathMachineINOVATE_NewUser = string.Empty;
            this.regPathUserCOMPOSE = string.Empty;
            this.regPathMachineCOMPOSE = string.Empty;
            this.regPathMachineCOMPOSE_NewUser = string.Empty;
            this.regPathUserDRAFT = string.Empty;
            this.regPathMachineDRAFT = string.Empty;
            this.regPathMachineDRAFT_NewUser = string.Empty;
        }
    }

    /// <summary>
    /// 共通処理
    /// </summary>
    public static class icapiCommon
    {
        /// <summary>
        /// DataGridViewのダブルバッファを有効にする
        /// </summary>
        /// <param name="dgv"></param>
        public static void enableDataGridViewDoubleBuffer(DataGridView dgv)
        {
            try
            {
                // DataGirdViewのTypeを取得
                System.Type dgvtype = typeof(DataGridView);

                // プロパティ設定の取得
                System.Reflection.PropertyInfo dgvPropertyInfo =
                dgvtype.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                // 対象のDataGridViewにtrueをセットする
                dgvPropertyInfo.SetValue(dgv, true, null);
            }
            catch (Exception ex)
            {
            }

        }

        /// <summary>
        /// インストールされている全てのバージョンのIRONCADインストールディレクトリを取得する
        /// </summary>
        /// <param name="installedIcList">key:バージョン  Value:インストールパス</param>
        /// <param name="getYearVersion">バージョンを年表記にするか  true:年  false:バージョン</param>
        /// <param name="sortNewVersion">新しいバージョン順にソートする  true:ソートする  false:ソートしない(古い順ソート)</param>
        public static void GetAllIronCADInstallPath(ref List<KeyValuePair<string, string>> installedIcList, bool getYearVersion, bool sortNewVersion)
        {
            installedIcList.Clear();
            List<KeyValuePair<string, string>> tmpInstalledIcList = new List<KeyValuePair<string, string>>();

            /* レジストリから探す */
            Microsoft.Win32.RegistryKey rkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\IronCAD\IRONCAD\", false);
            if (rkey != null)
            {
                string[] IronCADKeys = rkey.GetSubKeyNames();
                for (int i = 0; i < IronCADKeys.Count(); i++)
                {
                    Microsoft.Win32.RegistryKey subKey = rkey.OpenSubKey(IronCADKeys[i], false);
                    if (subKey == null)
                    {
                        continue;
                    }
                    string version = string.Empty;
                    if (getYearVersion == true)
                    {
                        version = convertNumberVersionToYearVersion(double.Parse(IronCADKeys[i]));
                    }
                    else
                    {
                        version = IronCADKeys[i];
                    }
                    string installDir = string.Empty;
                    bool installDirValueExists = subKey.GetValueNames().ToList().Contains("InstallDir");
                    if (installDirValueExists == true)
                    {
                        installDir = subKey.GetValue("InstallDir").ToString();
                    }
                    subKey.Close();
                    tmpInstalledIcList.Add(new KeyValuePair<string, string>(version, installDir));
                }
                rkey.Close();
            }

            /* ProgramFilesから探す */
            string programFilesPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string ironcadFolderPath = Path.Combine(programFilesPath, "IronCAD");
            if(Directory.Exists(ironcadFolderPath) == true)
            {
                DirectoryInfo dinfo = new DirectoryInfo(ironcadFolderPath);
                DirectoryInfo[] subDirList = dinfo.GetDirectories();
                foreach(DirectoryInfo subDir in subDirList)
                {
                    int yearVersion = -1;
                    string installDir = subDir.FullName;
                    string dirName = Path.GetFileName(installDir);
                    bool parse = Int32.TryParse(dirName, out yearVersion);
                    if(parse != true)
                    {
                        continue;
                    }
                    string version = string.Empty;
                    if (getYearVersion == true)
                    {
                        version = yearVersion.ToString();
                    }
                    else
                    {
                        version = convertYearVersionToNumberVersion(yearVersion);
                    }
                    int count = tmpInstalledIcList.Where(a => string.Equals(a.Key, version)).Count();
                    if(count != 0)
                    {
                        continue;
                    }
                    tmpInstalledIcList.Add(new KeyValuePair<string, string>(version, installDir));
                }
            }

            /* 結果をソートする */
            if (sortNewVersion == true)
            {
                installedIcList = tmpInstalledIcList.OrderByDescending(pair => pair.Key).ToList();
            }
            else
            {
                installedIcList = tmpInstalledIcList.OrderBy(pair => pair.Key).ToList();
            }
        }


        /// <summary>
        /// IRONCADバージョン(23.0など)を年バージョン(2021など)に変換する
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string convertNumberVersionToYearVersion(double number)
        {
            int yearVersion = (int)(number + 1998);
            return yearVersion.ToString();
        }


        /// <summary>
        /// IRONCAD年バージョン(2021など)をバージョン(23.0など)に変換する
        /// </summary>
        /// <param name="yearVersion"></param>
        /// <returns></returns>
        public static string convertYearVersionToNumberVersion(int yearVersion)
        {
            double version = (double)(yearVersion - 1998);
            return version.ToString("F1");
        }


        /// <summary>
        /// インストールされている全IRONCADのフォルダ・レジストリ情報を取得する
        /// </summary>
        /// <returns></returns>
        public static List<IRONCAD_REG_AND_DIR> getAllIroncadInformation()
        {
            List<IRONCAD_REG_AND_DIR> ironcadInfomationList = new List<IRONCAD_REG_AND_DIR>();
            string appDataRoamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appDataLocalPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            List<KeyValuePair<string, string>> ironcadList = new List<KeyValuePair<string, string>>();
            icapiCommon.GetAllIronCADInstallPath(ref ironcadList, true, true);
            foreach (KeyValuePair<string, string> ironcad in ironcadList)
            {
                string version = icapiCommon.convertYearVersionToNumberVersion(Int32.Parse(ironcad.Key));
                string yearVersion = ironcad.Key;
                string cacheDirRoaming = appDataRoamingPath;
                string cacheDirLocal = appDataLocalPath;
                string cacheDirRoamingINOVATE = Path.Combine(appDataRoamingPath, @"IronCAD\INOVATE", version);
                string cacheDirLocalINOVATE = Path.Combine(appDataLocalPath, @"IronCAD\INOVATE", version);
                string cacheDirRoamingIRONCAD = Path.Combine(appDataRoamingPath, @"IronCAD\IRONCAD", version);
                string cacheDirLocalIRONCAD = Path.Combine(appDataLocalPath, @"IronCAD\IRONCAD", version);
                string cacheDirRoamingCOMPOSE = Path.Combine(appDataRoamingPath, @"IronCAD\IRONCAD COMPOSE", version);
                string cacheDirLocalCOMPOSE = Path.Combine(appDataLocalPath, @"IronCAD\IRONCAD COMPOSE", version);
                string cacheDirRoamingDRAFT = Path.Combine(appDataRoamingPath, @"IronCAD\IRONCAD DRAFT", version);
                string cacheDirLocalDRAFT = Path.Combine(appDataLocalPath, @"IronCAD\IRONCAD DRAFT", version);

                string cacheDirRoamingCaxaIC = Path.Combine(appDataRoamingPath, @"CAXA\CAXA CAD CXIC " + yearVersion + " (x64)");
                string cacheDirRoamingCaxaIntDRAFT = Path.Combine(appDataRoamingPath, @"CAXA\CAXA CAD CXIC-INT-DRAFT " + yearVersion + " (x64)");
                string cacheDirRoamingCaxaIntINOVATE = Path.Combine(appDataRoamingPath, @"CAXA\CAXA CAD CXIC-INT-INOVATE " + yearVersion + " (x64)");
                string cacheDirRoamingCaxaIntIC = Path.Combine(appDataRoamingPath, @"CAXA\CAXA CAD CXIC-INT-IRONCAD " + yearVersion + " (x64)");
                string cacheDirRoamingCaxaPrintTool = Path.Combine(appDataRoamingPath, @"CAXA\CAXA PRINT TOOL " + yearVersion + " (x64)");
                string cacheDirRoamingCaxaExbViewr = Path.Combine(appDataRoamingPath, @"CAXA\CAXA EXB VIEWER " + yearVersion + " (x64)");

                string cacheDirLocalCaxaIC = Path.Combine(appDataLocalPath, @"CAXA\CAXA CAD CXIC " + yearVersion + " (x64)");
                string cacheDirLocalCaxaIntDRAFT = Path.Combine(appDataLocalPath, @"CAXA\CAXA CAD CXIC-INT-DRAFT " + yearVersion + " (x64)");
                string cacheDirLocalCaxaIntINOVATE = Path.Combine(appDataLocalPath, @"CAXA\CAXA CAD CXIC-INT-INOVATE " + yearVersion + " (x64)");
                string cacheDirLocalCaxaIntIC = Path.Combine(appDataLocalPath, @"CAXA\CAXA CAD CXIC-INT-IRONCAD " + yearVersion + " (x64)");
                string cacheDirLocalCaxaPrintTool = Path.Combine(appDataLocalPath, @"CAXA\CAXA PRINT TOOL " + yearVersion + " (x64)");
                string cacheDirLocalCaxaExbViewr = Path.Combine(appDataLocalPath, @"CAXA\CAXA EXB VIEWER " + yearVersion + " (x64)");

                string regPathUserINOVATE = @"SOFTWARE\IronCAD\INOVATE " + version;
                string regPathMachineINOVATE = @"SOFTWARE\IronCAD\INOVATE\" + version;
                string regPathMachineINOVATE_NewUser = @"SOFTWARE\IronCAD\New User Settings\INOVATE " + version;
                string regPathUserIRONCAD = @"SOFTWARE\IronCAD\IRONCAD " + version;
                string regPathMachineIRONCAD = @"SOFTWARE\IronCAD\IRONCAD\" + version;
                string regPathMachineIRONCAD_NewUser = @"SOFTWARE\IronCAD\New User Settings\IRONCAD " + version;
                string regPathUserCOMPOSE = @"SOFTWARE\IronCAD\IRONCAD COMPOSE " + version;
                string regPathMachineCOMPOSE = @"SOFTWARE\IronCAD\IRONCAD COMPOSE\" + version;
                string regPathMachineCOMPOSE_NewUser = @"SOFTWARE\IronCAD\New User Settings\IRONCAD COMPOSE " + version;
                string regPathUserDRAFT = @"SOFTWARE\IronCAD\IRONCAD DRAFT " + version;
                string regPathMachineDRAFT = @"SOFTWARE\IronCAD\IRONCAD DRAFT\" + version;
                string regPathMachineDRAFT_NewUser = @"SOFTWARE\IronCAD\New User Settings\IRONCAD DRAFT " + version;

                IRONCAD_REG_AND_DIR regAndDir = new IRONCAD_REG_AND_DIR();
                regAndDir.version = ironcad.Key;
                regAndDir.installDir = ironcad.Value;
                regAndDir.cacheDirLocal = cacheDirLocal;
                regAndDir.cacheDirRoaming = cacheDirRoaming;
                regAndDir.cacheDirLocalCOMPOSE = existDirectoryPath(cacheDirLocalCOMPOSE);
                regAndDir.cacheDirRoamingCOMPOSE = existDirectoryPath(cacheDirRoamingCOMPOSE);
                regAndDir.cacheDirLocalIRONCAD = existDirectoryPath(cacheDirLocalIRONCAD);
                regAndDir.cacheDirRoamingIRONCAD = existDirectoryPath(cacheDirRoamingIRONCAD);
                regAndDir.cacheDirLocalINOVATE = existDirectoryPath(cacheDirLocalINOVATE);
                regAndDir.cacheDirRoamingINOVATE = existDirectoryPath(cacheDirRoamingINOVATE);
                regAndDir.cacheDirLocalDRAFT = existDirectoryPath(cacheDirLocalDRAFT);
                regAndDir.cacheDirRoamingDRAFT = existDirectoryPath(cacheDirRoamingDRAFT);

                regAndDir.cacheDirRoamingCaxaIC = existDirectoryPath(cacheDirRoamingCaxaIC);
                regAndDir.cacheDirRoamingCaxaIntDRAFT = existDirectoryPath(cacheDirRoamingCaxaIntDRAFT);
                regAndDir.cacheDirRoamingCaxaIntINOVATE = existDirectoryPath(cacheDirRoamingCaxaIntINOVATE);
                regAndDir.cacheDirRoamingCaxaIntIC = existDirectoryPath(cacheDirRoamingCaxaIntIC);
                regAndDir.cacheDirRoamingCaxaPrintTool = existDirectoryPath(cacheDirRoamingCaxaPrintTool);
                regAndDir.cacheDirRoamingCaxaExbViewr = existDirectoryPath(cacheDirRoamingCaxaExbViewr);

                regAndDir.cacheDirLocalCaxaIC = existDirectoryPath(cacheDirLocalCaxaIC);
                regAndDir.cacheDirLocalCaxaIntDRAFT = existDirectoryPath(cacheDirLocalCaxaIntDRAFT);
                regAndDir.cacheDirLocalCaxaIntINOVATE = existDirectoryPath(cacheDirLocalCaxaIntINOVATE);
                regAndDir.cacheDirLocalCaxaIntIC = existDirectoryPath(cacheDirLocalCaxaIntIC);
                regAndDir.cacheDirLocalCaxaPrintTool = existDirectoryPath(cacheDirLocalCaxaPrintTool);
                regAndDir.cacheDirLocalCaxaExbViewr = existDirectoryPath(cacheDirLocalCaxaExbViewr);

                regAndDir.regPathUserCOMPOSE = existsRegistoryPath(regPathUserCOMPOSE, false);
                regAndDir.regPathMachineCOMPOSE = existsRegistoryPath(regPathMachineCOMPOSE, true);
                regAndDir.regPathMachineCOMPOSE_NewUser = existsRegistoryPath(regPathMachineCOMPOSE_NewUser, true);
                regAndDir.regPathUserIRONCAD = existsRegistoryPath(regPathUserIRONCAD, false);
                regAndDir.regPathMachineIRONCAD = existsRegistoryPath(regPathMachineIRONCAD, true);
                regAndDir.regPathMachineIRONCAD_NewUser = existsRegistoryPath(regPathMachineIRONCAD_NewUser, true);
                regAndDir.regPathUserINOVATE = existsRegistoryPath(regPathUserINOVATE, false);
                regAndDir.regPathMachineINOVATE = existsRegistoryPath(regPathMachineINOVATE, true);
                regAndDir.regPathMachineINOVATE_NewUser = existsRegistoryPath(regPathMachineINOVATE_NewUser, true);
                regAndDir.regPathUserDRAFT = existsRegistoryPath(regPathUserDRAFT, false);
                regAndDir.regPathMachineDRAFT = existsRegistoryPath(regPathMachineDRAFT, true);
                regAndDir.regPathMachineDRAFT_NewUser = existsRegistoryPath(regPathMachineDRAFT_NewUser, true);
                ironcadInfomationList.Add(regAndDir);
            }
            return ironcadInfomationList;
        }


        /// <summary>
        /// レジストリがあるかチェックする
        /// </summary>
        /// <param name="regPath">チェックするレジストリパス</param>
        /// <param name="isMachine">レジストリ種別  true:端末(MACHINE)  false: ユーザ(CURRENT_USER)</param>
        /// <returns>空白: レジストリなし  入力パスと同じ: レジストリあり</returns>
        public static string existsRegistoryPath(string regPath, bool isMachine)
        {
            if (isMachine == true)
            {
                Microsoft.Win32.RegistryKey rkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(regPath, false);
                if (rkey == null)
                {
                    return string.Empty;
                }
                rkey.Close();
            }
            else
            {
                Microsoft.Win32.RegistryKey rkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(regPath, false);
                if (rkey == null)
                {
                    return string.Empty;
                }
                rkey.Close();
            }
            return regPath;
        }


        /// <summary>
        /// ディレクトリがあるかチェックする
        /// </summary>
        /// <param name="dirPath">チェックするディレクトリパス</param>
        /// <returns>空白: ディレクトリなし  入力パスと同じ: ディレクトリあり</returns>
        public static string existDirectoryPath(string dirPath)
        {
            if (Directory.Exists(dirPath) != true)
            {
                return string.Empty;
            }
            return dirPath;
        }


        /// <summary>
        /// 読み取り専用を解除する(配下のファイルディレクトリすべて)
        /// </summary>
        /// <param name="dirInfo">解除するディレクトリトップのパス</param>
        public static void RemoveReadonlyAttribute(DirectoryInfo dirInfo)
        {
            //基のフォルダの属性を変更
            if ((dirInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                dirInfo.Attributes = FileAttributes.Normal;
            }

            //フォルダ内のすべてのファイルの属性を変更
            foreach (FileInfo fi in dirInfo.GetFiles())
            {
                if ((fi.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    fi.Attributes = FileAttributes.Normal;
            }

            //サブフォルダの属性を回帰的に変更
            foreach (DirectoryInfo di in dirInfo.GetDirectories())
            {
                RemoveReadonlyAttribute(di);
            }
        }


        /// <summary>
        /// ディレクトリを削除する(配下のディレクトリすべて)
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="maxRetries"></param>
        /// <param name="millisecondsDelay"></param>
        /// <returns></returns>
        public static async Task<bool> deleteDirectory(string directoryPath, int maxRetries = 10, int millisecondsDelay = 30)
        {
            if ((string.IsNullOrEmpty(directoryPath) == true) ||
                (Directory.Exists(directoryPath) != true))
            {
                return true;
            }

            /* 読み取り専用を解除する */
            RemoveReadonlyAttribute(new DirectoryInfo(directoryPath));

            bool ret = false;
            for (int i = 0; i < maxRetries; ++i)
            {
                try
                {
                    if (Directory.Exists(directoryPath))
                    {
                        Directory.Delete(directoryPath, true);
                    }
                    ret = true;
                    break;
                }
                catch (IOException ex)
                {
                    await Task.Delay(millisecondsDelay);
                }
                catch (UnauthorizedAccessException ex)
                {
                    await Task.Delay(millisecondsDelay);
                }
            }

            return ret;
        }


        /// <summary>
        /// ディレクトリをバックアップ(コピー)する
        /// </summary>
        /// <param name="directoryPath">コピー元</param>
        /// <param name="backupPath">コピー先</param>
        /// <returns></returns>
        public static async Task<bool> BackupDirectory(string directoryPath, string backupPath)
        {
            return await Task.Run<bool>(() =>
            {
                bool result = false;
                if ((string.IsNullOrEmpty(directoryPath) == true) ||
                   (Directory.Exists(directoryPath) != true))
                {
                    return true;
                }
                try
                {
                    Copy(directoryPath, backupPath);
                    result = true;
                }
                catch (Exception ex)
                {

                }
                return result;
            });
        }

        private static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }


        /// <summary>
        /// クリップボードへコピーする
        /// </summary>
        /// <param name="copyString"></param>
        /// <returns></returns>
        public static bool CopyToClipBoard(string copyString)
        {
            bool copyResult = false;
            int retry = 0;
            do
            {
                try
                {
                    Clipboard.SetDataObject(copyString);
                    copyResult = true;
                    break;
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(10);
                }
                retry++;
            } while (retry <= 20);

            return copyResult;
        }

        /// <summary>
        /// アセンブリ/パーツのツリー表示用の画像を取得する
        /// </summary>
        /// <param name="size"></param>
        /// <param name="imageList"></param>
        public static void getImageListAssemblyParts(Size size, ref ImageList imageList)
        {
            imageList = new ImageList();
            imageList.Images.Add(Resources.ImageNone);
            imageList.Images.Add(Resources.Assembly);
            imageList.Images.Add(Resources.LinkAssembly);
            imageList.Images.Add(Resources.Parts);
            imageList.Images.Add(Resources.LinkParts);
            imageList.Images.Add(Resources.Scene);
            imageList.Images.Add(Resources.Profile);
            imageList.Images.Add(Resources.LinkedProfile);
            imageList.Images.Add(Resources.Wire);
            imageList.Images.Add(Resources.LinkedWire);
            imageList.Images.Add(Resources.SheetMetal);
            imageList.Images.Add(Resources.LinkedSheetMetal);
            imageList.Images.Add(Resources.PartsSheet);
            imageList.Images.Add(Resources.LinkedPartsSheet);
            imageList.ImageSize = size;
            imageList.ColorDepth = ColorDepth.Depth32Bit;
        }

        /// <summary>
        /// どの機能のTreeGridViewを作成するか
        /// </summary>
        public enum CREATE_TREE_MODE
        {
            CHECK_IN = 0,
            CHECKOUT_SELECT,
            INPUT_CUSTOM_PROP,
            SCENEFILE_MANAGEMENT
        }

        public static void ExpandTreeGridViewTreeNodes(ref TreeGridNodeCollection nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Expand();
                TreeGridNodeCollection childNodes = nodes[i].Nodes;
                ExpandTreeGridViewTreeNodes(ref childNodes);
            }
        }

        /// <summary>
        /// シーンファイルのツリー表示を作成する(TreeGridView用) 再帰処理
        /// </summary>
        /// <param name="mode">表示する対象</param>
        /// <param name="nodeHeight">各ノードの高さ</param>
        /// <param name="currElem">現在のアセンブリ/パーツ Element</param>
        /// <param name="currNode">現在のノード</param>
        /// <param name="currDepth">現在の深さ</param>
        /// <returns></returns>
        public static bool GetSceneTreeInfo(CREATE_TREE_MODE mode, int nodeHeight, IZElement currElem, ref TreeGridNode currNode, ref int currDepth)
        {
            try
            {
                if ((currElem.Type != eZElementType.Z_ELEMENT_PART) &&
                    (currElem.Type != eZElementType.Z_ELEMENT_ASSEMBLY) &&
                    //                    (elem.Type != eZElementType.Z_ELEMENT_WIRE) &&
                    //                    (elem.Type != eZElementType.Z_ELEMENT_PROFILE) &&
                    (currElem.Type != eZElementType.Z_ELEMENT_SHEETMETAL_PART) &&
                    (currElem.Type != eZElementType.Z_ELEMENT_UNKNOWN))
                {

                    return false;
                }
                ZArray childs = currElem.GetChildrenZArray();
                int count = 0;
                childs.Count(out count);
                for (int i = 0; i < count; i++)
                {
                    object obj;
                    IZElement childElem;
                    childs.Get(i, out obj);
                    childElem = obj as IZElement;
                    if ((childElem.Type == eZElementType.Z_ELEMENT_PART) ||
                        (childElem.Type == eZElementType.Z_ELEMENT_ASSEMBLY) ||
                        (childElem.Type == eZElementType.Z_ELEMENT_WIRE) ||
                        (childElem.Type == eZElementType.Z_ELEMENT_PROFILE) ||
                        (childElem.Type == eZElementType.Z_ELEMENT_SHEETMETAL_PART))
                    {
                        /* パーツ/アセンブリの情報 */
                        IZDoc doc = childElem.OwningDoc;
                        IZSceneDoc scene = doc as IZSceneDoc;
                        string dataType = string.Empty;
                        bool link = false;
                        string linkStr = string.Empty;
                        string dispName = string.Empty;
                        switch (childElem.Type)
                        {
                            case eZElementType.Z_ELEMENT_ASSEMBLY:
                                IZAssembly asm = childElem as IZAssembly;
                                linkStr = asm.GetExternallyLinkedInfo(out link);
                                dataType = GetInnerDataType(childElem.Type, link, eZBodyType.Z_BODY_EMPTY);
                                break;
                            case eZElementType.Z_ELEMENT_PART:
                            case eZElementType.Z_ELEMENT_WIRE:
                            case eZElementType.Z_ELEMENT_PROFILE:
                            case eZElementType.Z_ELEMENT_SHEETMETAL_PART:
                                IZPart part = childElem as IZPart;
                                eZBodyType body = eZBodyType.Z_BODY_EMPTY;
                                linkStr = part.GetExternallyLinkedInfo(out link);
                                if (childElem.Type == eZElementType.Z_ELEMENT_PART)
                                {
                                    part.GetBodyType(ref body);
                                }
                                dataType = GetInnerDataType(childElem.Type, link, body);
                                break;
                            default:
                                break;
                        }
                        dispName = childElem.Name;

                        TreeGridNode childNode = null;
                        switch (mode)
                        {
                            case CREATE_TREE_MODE.CHECK_IN:
                                /* ★データの順序変更は本メソッド使用箇所のデザイナのColumn順番も変更する必要あり */
                                childNode = currNode.Nodes.Add(childElem.Name, childElem.SystemName, childElem.Id, dataType, currDepth, linkStr);
                                childNode.Cells[0].Tag = childElem;
                                break;
                            case CREATE_TREE_MODE.CHECKOUT_SELECT:
                                childNode = currNode.Nodes.Add(childElem.Name, childElem.SystemName, childElem.Id, dataType, currDepth, linkStr);
                                break;
                            case CREATE_TREE_MODE.INPUT_CUSTOM_PROP:
                                childNode = currNode.Nodes.Add(childElem.Name, childElem.SystemName, childElem.Id, dataType, currDepth,linkStr);
                                break;
                            case CREATE_TREE_MODE.SCENEFILE_MANAGEMENT:
                                childNode = currNode.Nodes.Add(childElem.Name, childElem.SystemName, childElem.Id, dataType, currDepth, linkStr);
                                break;
                            default:
                                break;
                        }

                        childNode.ImageIndex = getImageIndexAssemblyParts(dataType);
                        childNode.Height = (int)(nodeHeight * ScaleReziser.getScalingFactor());
                        currDepth++;
                        GetSceneTreeInfo(mode, nodeHeight, childElem, ref childNode, ref currDepth);
                        currDepth--;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return true;
        }
        /// <summary>
        /// シーンファイルのツリー表示を作成する(TreeGridView用) 再帰処理
        /// </summary>
        /// <param name="mode">表示する対象</param>
        /// <param name="nodeHeight">各ノードの高さ</param>
        /// <param name="currElem">現在のアセンブリ/パーツ Element</param>
        /// <param name="currNode1">現在のノード1</param>
        /// <param name="currNode2">現在のノード2</param>
        /// <param name="currDepth">現在の深さ</param>
        /// <returns></returns>
        public static bool GetSceneTreeInfo2(CREATE_TREE_MODE mode, int nodeHeight, IZElement currElem, ref TreeGridNode currNode1, ref TreeGridNode currNode2, ref int currDepth)
        {
            try
            {
                if ((currElem.Type != eZElementType.Z_ELEMENT_PART) &&
                    (currElem.Type != eZElementType.Z_ELEMENT_ASSEMBLY) &&
                    //                    (elem.Type != eZElementType.Z_ELEMENT_WIRE) &&
                    //                    (elem.Type != eZElementType.Z_ELEMENT_PROFILE) &&
                    (currElem.Type != eZElementType.Z_ELEMENT_SHEETMETAL_PART) &&
                    (currElem.Type != eZElementType.Z_ELEMENT_UNKNOWN))
                {

                    return false;
                }
                ZArray childs = currElem.GetChildrenZArray();
                int count = 0;
                childs.Count(out count);
                for (int i = 0; i < count; i++)
                {
                    object obj;
                    IZElement childElem;
                    childs.Get(i, out obj);
                    childElem = obj as IZElement;
                    if ((childElem.Type == eZElementType.Z_ELEMENT_PART) ||
                        (childElem.Type == eZElementType.Z_ELEMENT_ASSEMBLY) ||
                        (childElem.Type == eZElementType.Z_ELEMENT_WIRE) ||
                        (childElem.Type == eZElementType.Z_ELEMENT_PROFILE) ||
                        (childElem.Type == eZElementType.Z_ELEMENT_SHEETMETAL_PART))
                    {
                        /* パーツ/アセンブリの情報 */
                        IZDoc doc = childElem.OwningDoc;
                        IZSceneDoc scene = doc as IZSceneDoc;
                        string dataType = string.Empty;
                        bool link = false;
                        string linkStr = string.Empty;
                        string dispName = string.Empty;
                        switch (childElem.Type)
                        {
                            case eZElementType.Z_ELEMENT_ASSEMBLY:
                                IZAssembly asm = childElem as IZAssembly;
                                linkStr = asm.GetExternallyLinkedInfo(out link);
                                dataType = GetInnerDataType(childElem.Type, link, eZBodyType.Z_BODY_EMPTY);
                                break;
                            case eZElementType.Z_ELEMENT_PART:
                            case eZElementType.Z_ELEMENT_WIRE:
                            case eZElementType.Z_ELEMENT_PROFILE:
                            case eZElementType.Z_ELEMENT_SHEETMETAL_PART:
                                IZPart part = childElem as IZPart;
                                eZBodyType body = eZBodyType.Z_BODY_EMPTY;
                                linkStr = part.GetExternallyLinkedInfo(out link);
                                if (childElem.Type == eZElementType.Z_ELEMENT_PART)
                                {
                                    part.GetBodyType(ref body);
                                }
                                dataType = GetInnerDataType(childElem.Type, link, body);
                                break;
                            default:
                                break;
                        }
                        dispName = childElem.Name;

                        TreeGridNode childNode = null;
                        TreeGridNode childNode2 = null;
                        switch (mode)
                        {
                            case CREATE_TREE_MODE.CHECK_IN:
                                /* ★データの順序変更は本メソッド使用箇所のデザイナのColumn順番も変更する必要あり */
                                childNode = currNode1.Nodes.Add(childElem.Name, childElem.SystemName, childElem.Id, dataType, currDepth, linkStr);
                                childNode.Cells[0].Tag = childElem;
                                childNode2 = currNode2.Nodes.Add(childElem.Name, childElem.SystemName, childElem.Id, dataType, currDepth, linkStr);
                                childNode2.Cells[0].Tag = childElem;
                                break;
                            case CREATE_TREE_MODE.CHECKOUT_SELECT:
                                childNode = currNode1.Nodes.Add(childElem.Name, childElem.SystemName, childElem.Id, dataType, currDepth, linkStr);
                                childNode2 = currNode1.Nodes.Add(childElem.Name, childElem.SystemName, childElem.Id, dataType, currDepth, linkStr);
                                break;
                            case CREATE_TREE_MODE.INPUT_CUSTOM_PROP:
                                childNode = currNode1.Nodes.Add(childElem.Name, childElem.SystemName, childElem.Id, dataType, currDepth, linkStr);
                                childNode2 = currNode1.Nodes.Add(childElem.Name, childElem.SystemName, childElem.Id, dataType, currDepth, linkStr);
                                break;
                            case CREATE_TREE_MODE.SCENEFILE_MANAGEMENT:
                                childNode = currNode1.Nodes.Add(childElem.Name, childElem.SystemName, childElem.Id, dataType, currDepth, linkStr);
                                childNode2 = currNode1.Nodes.Add(childElem.Name, childElem.SystemName, childElem.Id, dataType, currDepth, linkStr);
                                break;
                            default:
                                break;
                        }

                        childNode2.ImageIndex = childNode.ImageIndex = getImageIndexAssemblyParts(dataType);
                        childNode2.Height = childNode.Height = (int)(nodeHeight * ScaleReziser.getScalingFactor());
                        currDepth++;
                        GetSceneTreeInfo2(mode, nodeHeight, childElem, ref childNode, ref childNode2, ref currDepth);
                        currDepth--;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return true;
        }
        public const string SCENE_DATATYPE_ASSEMBLY = "ASSEMBLY";
        public const string SCENE_DATATYPE_PART = "PARTS";
        public const string SCENE_DATATYPE_PART_SOLID = "PARTS_SOLID";
        public const string SCENE_DATATYPE_PART_SHEET = "PARTS_SHEET";
        public const string SCENE_DATATYPE_PART_UNKNOWN = "PARTS_UNKNOWN";
        public const string SCENE_DATATYPE_PART_WIRE = "PARTS_WIRE";
        public const string SCENE_DATATYPE_PART_EMPTY = "PARTS_EMPTY";
        public const string SCENE_DATATYPE_WIRE = "WIRE";
        public const string SCENE_DATATYPE_PROFILE = "PROFILE";
        public const string SCENE_DATATYPE_SHEETMETAL_PART = "SHEETMETAL_PARTS";
        public const string SCENE_DATATYPE_LINKED_ASSEMBLY = "LINKED_ASSEMBLY";
        public const string SCENE_DATATYPE_LINKED_PART = "LINKED_PARTS";
        public const string SCENE_DATATYPE_LINKED_PART_SOLID = "LINKED_PARTS_SOLID";
        public const string SCENE_DATATYPE_LINKED_PART_SHEET = "LINKED_PARTS_SHEET";
        public const string SCENE_DATATYPE_LINKED_PART_UNKNOWN = "LINKED_PARTS_UNKNOWN";
        public const string SCENE_DATATYPE_LINKED_PART_WIRE = "LINKED_PARTS_WIRE";
        public const string SCENE_DATATYPE_LINKED_PART_EMPTY = "LINKED_PARTS_EMPTY";
        public const string SCENE_DATATYPE_LINKED_WIRE = "LINKED_WIRE";
        public const string SCENE_DATATYPE_LINKED_PROFILE = "LINKED_PROFILE";
        public const string SCENE_DATATYPE_LINKED_SHEETMETAL_PART = "LINKED_SHEETMETAL_PARTS";
        public const string SCENE_DATATYPE_SCENE = "SCENE";
        public const string SCENE_DATATYPE_FILE = "FILE";

        /// <summary>
        /// element種類からicVaultのデータ種類を取得する
        /// </summary>
        /// <param name="elemType">elementの種類</param>
        /// <param name="isLinked">外部リンク有無 true;外部リンクあり false:外部リンクなし</param>
        /// <param name="body">elementのボディー種別</param>
        /// <returns></returns>
        public static string GetInnerDataType(eZElementType elemType, bool isLinked, eZBodyType body)
        {
            string dataType = string.Empty;
            switch (elemType)
            {
                case eZElementType.Z_ELEMENT_PART:
                    if (isLinked == true)
                    {
                        dataType = SCENE_DATATYPE_LINKED_PART;
                        switch (body)
                        {
                            case eZBodyType.Z_BODY_SHEET:
                                dataType = SCENE_DATATYPE_LINKED_PART_SHEET;
                                break;
                            case eZBodyType.Z_BODY_SOLID:
                                dataType = SCENE_DATATYPE_LINKED_PART_SOLID;
                                break;
                            case eZBodyType.Z_BODY_WIRE:
                                dataType = SCENE_DATATYPE_LINKED_PART_WIRE;
                                break;
                            case eZBodyType.Z_BODY_EMPTY:
                                dataType = SCENE_DATATYPE_LINKED_PART_EMPTY;
                                break;
                            case eZBodyType.Z_BODY_UNKNOWN:
                                dataType = SCENE_DATATYPE_LINKED_PART_UNKNOWN;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        dataType = SCENE_DATATYPE_PART;
                        switch (body)
                        {
                            case eZBodyType.Z_BODY_SHEET:
                                dataType = SCENE_DATATYPE_PART_SHEET;
                                break;
                            case eZBodyType.Z_BODY_SOLID:
                                dataType = SCENE_DATATYPE_PART_SOLID;
                                break;
                            case eZBodyType.Z_BODY_WIRE:
                                dataType = SCENE_DATATYPE_PART_WIRE;
                                break;
                            case eZBodyType.Z_BODY_EMPTY:
                                dataType = SCENE_DATATYPE_PART_EMPTY;
                                break;
                            case eZBodyType.Z_BODY_UNKNOWN:
                                dataType = SCENE_DATATYPE_PART_UNKNOWN;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case eZElementType.Z_ELEMENT_ASSEMBLY:
                    if (isLinked == true)
                    {
                        dataType = SCENE_DATATYPE_LINKED_ASSEMBLY;
                    }
                    else
                    {
                        dataType = SCENE_DATATYPE_ASSEMBLY;
                    }
                    break;
                case eZElementType.Z_ELEMENT_WIRE:
                    if (isLinked == true)
                    {
                        dataType = SCENE_DATATYPE_LINKED_WIRE;
                    }
                    else
                    {
                        dataType = SCENE_DATATYPE_WIRE;
                    }
                    break;
                case eZElementType.Z_ELEMENT_PROFILE:
                    if (isLinked == true)
                    {
                        dataType = SCENE_DATATYPE_LINKED_PROFILE;
                    }
                    else
                    {
                        dataType = SCENE_DATATYPE_PROFILE;
                    }
                    break;
                case eZElementType.Z_ELEMENT_SHEETMETAL_PART:
                    if (isLinked == true)
                    {
                        dataType = SCENE_DATATYPE_LINKED_SHEETMETAL_PART;
                    }
                    else
                    {
                        dataType = SCENE_DATATYPE_SHEETMETAL_PART;
                    }
                    break;
                default:
                    break;
            }
            return dataType;
        }


        /// <summary>
        /// アセンブリ/パーツ種別からツリー用の画像Indexを取得する
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static int getImageIndexAssemblyParts(string dataType)
        {
            int index = 0;
            switch (dataType)
            {
                case SCENE_DATATYPE_ASSEMBLY:
                    index = 1;
                    break;
                case SCENE_DATATYPE_LINKED_ASSEMBLY:
                    index = 2;
                    break;
                case SCENE_DATATYPE_PART:
                case SCENE_DATATYPE_PART_EMPTY:
                case SCENE_DATATYPE_PART_UNKNOWN:
                case SCENE_DATATYPE_PART_SOLID:
                    index = 3;
                    break;
                case SCENE_DATATYPE_LINKED_PART:
                case SCENE_DATATYPE_LINKED_PART_EMPTY:
                case SCENE_DATATYPE_LINKED_PART_UNKNOWN:
                case SCENE_DATATYPE_LINKED_PART_SOLID:
                    index = 4;
                    break;
                case SCENE_DATATYPE_SCENE:
                    index = 5;
                    break;
                case SCENE_DATATYPE_PROFILE:
                    index = 6;
                    break;
                case SCENE_DATATYPE_LINKED_PROFILE:
                    index = 7;
                    break;
                case SCENE_DATATYPE_WIRE:
                    index = 8;
                    break;
                case SCENE_DATATYPE_LINKED_WIRE:
                    index = 9;
                    break;
                case SCENE_DATATYPE_SHEETMETAL_PART:
                    index = 10;
                    break;
                case SCENE_DATATYPE_LINKED_SHEETMETAL_PART:
                    index = 11;
                    break;
                case SCENE_DATATYPE_PART_SHEET:
                    index = 12;
                    break;
                case SCENE_DATATYPE_LINKED_PART_SHEET:
                    index = 13;
                    break;
                default:
                    index = 0;
                    break;
            }
            return index;
        }

        #region カスタムプロパティ関連

        [Serializable]
        public class CustomProperty
        {
            public string name;
            public object value;
            public bool scopeIsShared;
            public CustomProperty(string name, object value, bool shared)
            {
                this.name = name;
                this.value = value;
                this.scopeIsShared = shared;
            }
        }
        /// <summary>
        /// カスタムプロパティを取得する
        /// </summary>
        /// <param name="element">パーツ/アセンブリのElement</param>
        /// <param name="customPropeties">カスラムプロパティ</param>
        public static void GetCustomProperties(IZElement element, ref List<CustomProperty> customPropeties)
        {
            /* elementチェック */
            if (element == null)
            {
                return;
            }

            /* カスタムプロパティの情報 */
            IZCustomPropMgr propMgr = element.CustomPropMgr[true];
            if (propMgr.Count != 0)
            {
                for (int j = 0; j < propMgr.Count; j++)
                {
                    string name = string.Empty;
                    object value = null;
                    propMgr.GetAt(j, out name, out value);
                    customPropeties.Add(new CustomProperty(name, value, true));
                }
            }
            /* カスタムプロパティの情報 */
            propMgr = element.CustomPropMgr[false];
            if (propMgr.Count != 0)
            {
                for (int j = 0; j < propMgr.Count; j++)
                {
                    string name = string.Empty;
                    object value = null;
                    propMgr.GetAt(j, out name, out value);
                    customPropeties.Add(new CustomProperty(name, value, false));
                }
            }
        }

        /// <summary>
        /// カスタムプロパティを編集(新規/削除/変更)する   値がnullだと:削除 
        /// </summary>
        /// <param name="element">パーツ/アセンブリのElement</param>
        /// <param name="name">名前</param>
        /// <param name="value">値</param>
        /// <param name="isShared">true:すべてのリンクインタンスに適用  false:このパーツ アセンブリのみ</param>
        public static void EditCustomProperty(IZElement element, string name, object value, bool isShared)
        {
            /* カスタムプロパティの情報 すべてのリンクインスタンス */
            IZCustomPropMgr propMgr = element.CustomPropMgr[isShared];
            int count = propMgr.Count;
            bool targetPropFoundFlag = false;
            for (int i = 0; i < count; i++)
            {
                string currName = string.Empty;
                object currValue = string.Empty;
                propMgr.GetAt(i, out currName, out currValue);
                if (string.Equals(currName, name) == true)
                {
                    targetPropFoundFlag = true;
                    break;
                }
            }

            if (value == null)
            {
                /* すべてのリンクインタンスから削除 */
                if (targetPropFoundFlag == true)
                {
                    propMgr.RemoveByName(name);
                }
            }
            else
            {
                /* すべてのリンクインスタンスとして扱う  */
                if (targetPropFoundFlag == true)
                {
                    /* 現在の値を変更 */
                    propMgr.SetValueByName(name, value);
                }
                else
                {
                    propMgr.AddCustomProp(name, value, true);
                }
            }
        }

        /// <summary>
        /// 当該Elementのカスタムプロパティをすべて削除する 
        /// </summary>
        /// <param name="element">パーツ/アセンブリのElement</param>
        public static void DeleteCustomProperty(IZElement element, string name, bool isShared)
        {
            /* elementチェック */
            if (element == null)
            {
                return;
            }
            EditCustomProperty(element, name, null, isShared);
        }

        /// <summary>
        /// 当該Elementのカスタムプロパティをすべて削除する 
        /// </summary>
        /// <param name="element">パーツ/アセンブリのElement</param>
        public static void DeleteAllCustomProperties(IZElement element)
        {
            /* elementチェック */
            if (element == null)
            {
                return;
            }

            /* カスタムプロパティの情報をすべて削除する */
            DeleteCustomProperties(element, true);
            DeleteCustomProperties(element, false);
        }

        /// <summary>
        /// 当該Elementのカスタムプロパティをすべて削除する 
        /// </summary>
        /// <param name="element">パーツ/アセンブリのElement</param>
        /// <param name="isShared">true:すべてのリンクインスタンス  false:このパーツ/アセンブリのみ</param>
        public static void DeleteCustomProperties(IZElement element, bool isShared)
        {
            /* elementチェック */
            if (element == null)
            {
                return;
            }

            /* カスタムプロパティの情報をすべて削除する */
            List<string> allPropName = new List<string>();
            IZCustomPropMgr propMgr = element.CustomPropMgr[isShared];
            int count = propMgr.Count;
            for (int i = 0; i < count; i++)
            {
                string currName = string.Empty;
                object currValue = string.Empty;
                propMgr.GetAt(i, out currName, out currValue);
                allPropName.Add(currName);
            }
            foreach (string name in allPropName)
            {
                propMgr.RemoveByName(name);
            }
        }

        /// <summary>
        /// カスタムプロパティの範囲設定(bool isShard)から設定名を取得する
        /// </summary>
        /// <param name="isShared">範囲設定 true:すべてのリンクインスタンス  false:このパーツ/アセンブリのみ</param>
        /// <returns></returns>
        public static string convertIsSharedBoolToString(bool isShared)
        {
            string isSharedStr = string.Empty;
            if (isShared == true)
            {
                isSharedStr = "すべてのリンク インスタンス";
            }
            else
            {
                isSharedStr = "このパーツ/アセンブリのみ";
            }
            return isSharedStr;
        }

        /// <summary>
        /// カスタムプロパティの範囲設定名(string)から設定値を取得する
        /// </summary>
        /// <param name="isSharedStr">範囲設定名</param>
        /// <returns></returns>
        public static bool getIsSharedStringToBool(string isSharedStr)
        {
            bool isSharedBool = true;
            if (string.Equals(isSharedStr, "すべてのリンク インスタンス") == true)
            {
                isSharedBool = true;
            }
            else
            {
                isSharedBool = false;
            }
            return isSharedBool;
        }

        /// <summary>
        /// カスタムプロパティのデータ型設定(DataType)を文字列に変換する
        /// </summary>
        /// <param name="dataType">データ型</param>
        /// <returns></returns>
        public static string getCustomPropertyDataTypeString(Type dataType)
        {
            string dataTypeStr = string.Empty;
            if (dataType == typeof(DateTime))
            {
                dataTypeStr = "日付";
            }
            else if (dataType == typeof(bool))
            {
                dataTypeStr = "はい/いいえ (True/False)";
            }
            else if (dataType == typeof(double))
            {
                dataTypeStr = "数値";
            }
            else if (dataType == typeof(string))
            {
                dataTypeStr = "テキスト";
            }
            return dataTypeStr;
        }

        public static string boolScopeToStringScope(bool scopeIsShared)
        {
            string retStr = string.Empty;
            if (scopeIsShared == true)
            {
                retStr = "すべてのリンクインスタンス";
            }
            else
            {
                retStr = "このパーツ/アセンブリのみ";
            }
            return retStr;
        }

        public static string convertCustomPropertyDbDataTypeToDataType(string dbDataType)
        {
            DateTime dummyData_DateTime = DateTime.Now;
            bool dummyData_Bool = true;
            Double dummyData_Number = 0.0;
            string dummyData_Text = string.Empty;

            string dataTypeName = string.Empty;

            if (string.Equals(dbDataType, dummyData_DateTime.GetType().FullName) == true)
            {
                dataTypeName = Resources.CustomPropertiesDataType_Date;
            }
            else if (string.Equals(dbDataType, dummyData_Bool.GetType().FullName) == true)
            {
                dataTypeName = Resources.CustomPropertiesDataType_YesOrNo;
            }
            else if (string.Equals(dbDataType, dummyData_Number.GetType().FullName) == true)
            {
                dataTypeName = Resources.CustomPropertiesDataType_Number;
            }
            else if (string.Equals(dbDataType, dummyData_Text.GetType().FullName) == true)
            {
                dataTypeName = Resources.CustomPropertiesDataType_Text;
            }
            return dataTypeName;
        }
        #endregion

        #region DataGridView/TreeGridViewのアクセス関連
        /// <summary>
        /// treeGridViewの値を取得する
        /// </summary>
        /// <param name="tgv"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static string treeGridViewRowsValueGet(TreeGridView tgv, int rowIndex, int columnIndex)
        {
            string retStr = string.Empty;
            if (rowIndex >= tgv.Rows.Count)
            {
                return retStr;
            }
            if (tgv.Rows[rowIndex].Cells[columnIndex].Value == null)
            {
                return retStr;
            }
            retStr = tgv.Rows[rowIndex].Cells[columnIndex].Value.ToString();
            return retStr;
        }
        public static string treeGridViewRowsValueGet(TreeGridView tgv, int rowIndex, string columnStr)
        {
            string retStr = string.Empty;
            if (rowIndex >= tgv.Rows.Count)
            {
                return retStr;
            }
            if (tgv.Rows[rowIndex].Cells[columnStr].Value == null)
            {
                return retStr;
            }
            retStr = tgv.Rows[rowIndex].Cells[columnStr].Value.ToString();
            return retStr;
        }
        public static string treeGridViewNodesValueGet(TreeGridView tree, int nodeIndex, string columnStr)
        {
            string retStr = string.Empty;
            if (nodeIndex >= tree.Nodes.Count)
            {
                return retStr;
            }
            if (tree.Nodes[nodeIndex].Cells[columnStr].Value == null)
            {
                return retStr;
            }
            retStr = tree.Nodes[nodeIndex].Cells[columnStr].Value.ToString();
            return retStr;
        }

        public static string dataRowValueGet(DataRow dr, string columnName)
        {
            string retStr = string.Empty;
            try
            {
                if (dr[columnName] != null)
                {
                    retStr = dr[columnName].ToString();
                }
            }
            catch
            {

            }

            return retStr;
        }
        public static bool dataRowBoolValueGet(DataRow dr, string columnName)
        {
            bool retValue = false;
            try
            {
                if (dr[columnName] != null)
                {
                    bool.TryParse(dr[columnName].ToString(), out retValue);
                }
            }
            catch
            {

            }
            return retValue;

        }
        public static bool treeGridViewBoolValueGet(TreeGridView tgv, int rowIndex, string columnName)
        {
            bool retValue = false;
            if (rowIndex >= tgv.Rows.Count)
            {
                return false;
            }
            if (tgv.Rows[rowIndex].Cells[columnName].Value == null)
            {
                return false;
            }
            bool.TryParse(tgv.Rows[rowIndex].Cells[columnName].Value.ToString(), out retValue);
            return retValue;
        }

        public static bool treeGridViewCheckStringBoolValueGet(TreeGridView tgv, int rowIndex, string columnName, string checkStringTrue)
        {
            bool retValue = false;
            if (rowIndex >= tgv.Rows.Count)
            {
                return false;
            }
            if (tgv.Rows[rowIndex].Cells[columnName].Value == null)
            {
                return false;
            }
            if (string.Equals(checkStringTrue, tgv.Rows[rowIndex].Cells[columnName].Value.ToString()) == true)
            {
                retValue = true;
            }
            else
            {
                retValue = false;
            }
            return retValue;
        }

        public static string treeGridViewValueGet(TreeGridView tgv, int rowIndex, string columnName)
        {
            string retStr = string.Empty;
            if (rowIndex >= tgv.Rows.Count)
            {
                return retStr;
            }
            if (tgv.Rows[rowIndex].Cells[columnName].Value == null)
            {
                return retStr;
            }
            retStr = tgv.Rows[rowIndex].Cells[columnName].Value.ToString();
            return retStr;

        }
        public static string dataGridViewValueGet(DataGridView dgv, int rowIndex, int columnIndex)
        {
            string retStr = string.Empty;
            if ((rowIndex >= dgv.Rows.Count) || (columnIndex > dgv.Columns.Count))
            {
                return retStr;
            }

            if (dgv.Rows[rowIndex].Cells[columnIndex].Value == null)
            {
                return retStr;
            }
            retStr = dgv.Rows[rowIndex].Cells[columnIndex].Value.ToString();
            return retStr;

        }
        public static string dataGridViewValueGet(DataGridView dgv, int rowIndex, string columnName)
        {
            string retStr = string.Empty;
            if (rowIndex >= dgv.Rows.Count)
            {
                return retStr;
            }
            if (dgv.Columns.Contains(columnName) == false)
            {
                return retStr;
            }
            if (dgv.Rows[rowIndex].Cells[columnName].Value == null)
            {
                return retStr;
            }
            retStr = dgv.Rows[rowIndex].Cells[columnName].Value.ToString();
            return retStr;

        }
        public static bool dataGridViewBoolValueGet(DataGridView dgv, int rowIndex, string columnName)
        {
            bool retValue = false;
            if (rowIndex >= dgv.Rows.Count)
            {
                return false;
            }
            if (dgv.Columns.Contains(columnName) == false)
            {
                return false;
            }
            if (dgv.Rows[rowIndex].Cells[columnName].Value == null)
            {
                return false;
            }
            bool ret = bool.TryParse(dgv.Rows[rowIndex].Cells[columnName].Value.ToString(), out retValue);
            if (ret == false)
            {
                retValue = false;
            }
            return retValue;

        }
        #endregion DataGridView/TreeGridViewのアクセス関連
    }
}
