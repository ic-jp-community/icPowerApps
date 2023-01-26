using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace icAPIAddinEnableDisable
{
    public class AddinConfig
    {
        public static string ADDIN_CONFIG_FILE_PATH = "Config\\Ironcad.Addin.config";

        #region IRONCADのインストールパス取得
        /// <summary>
        /// インストールされている全てのバージョンのIRONCADインストールディレクトリを取得する
        /// </summary>
        /// <param name="installedIcList"></param>
        public static void GetAllIronCADInstallPath(ref List<KeyValuePair<string, string>> installedIcList, bool getYearVersion)
        {
            installedIcList = new List<KeyValuePair<string, string>>();
            Microsoft.Win32.RegistryKey rkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\IronCAD\IRONCAD\");
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
                    bool installDirValueExists = subKey.GetValueNames().ToList().Contains("InstallDir");
                    if (installDirValueExists != true)
                    {
                        subKey.Close();
                        continue;
                    }
                    string installDir = subKey.GetValue("InstallDir").ToString();
                    subKey.Close();
                    if (string.IsNullOrEmpty(installDir) == true)
                    {
                        continue;
                    }
                    if (getYearVersion == true)
                    {
                        string yearVersion = convertNumberVersionToYearVersion(double.Parse(IronCADKeys[i]));
                        installedIcList.Add(new KeyValuePair<string, string>(yearVersion, installDir));
                    }
                    else
                    {
                        installedIcList.Add(new KeyValuePair<string, string>(IronCADKeys[i], installDir));
                    }
                }
                rkey.Close();
            }
        }

        private static string convertNumberVersionToYearVersion(double number)
        {
            int yearVersion = (int)(number + 1998);
            return yearVersion.ToString();
        }
        #endregion

        #region IRONCADへのアドイン追加/削除
        public static bool AddConfig(string xmlpath, string guid, string name, string description)
        {
            /* ファイルの有無チェック */
            if (File.Exists(xmlpath) != true)
            {
                return false;
            }

            //C:\test.txt の属性を取得する
            System.IO.FileAttributes attr = System.IO.File.GetAttributes(xmlpath);

            //読み取り専用属性があるか調べる
            if ((attr & System.IO.FileAttributes.ReadOnly) ==
                System.IO.FileAttributes.ReadOnly)
            {
                //読み取り専用属性を削除する
                System.IO.File.SetAttributes(xmlpath, attr & (~System.IO.FileAttributes.ReadOnly));
            }

            StreamReader sr = new StreamReader(xmlpath);
            List<string> allLine = new List<string>();
            while (sr.EndOfStream != true)
            {
                allLine.Add(sr.ReadLine());
            }
            sr.Close();

            int endIndex = -1;
            for (int i = allLine.Count - 1; i >= 0; i--)
            {
                string str = allLine[i].Trim(' ', '\t');
                if (str.Contains("</AddIns>") == true)
                {
                    endIndex = i;
                    break;
                }
            }
            int addSpaceNum = allLine[endIndex - 1].IndexOf('<');
            string space = string.Empty;
            for (int i = 0; i < addSpaceNum; i++)
            {
                space += " ";
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(space + "<AddIn>");
            sb.AppendLine(space + " <identify></identify>");
            sb.AppendLine(space + string.Format(" <inprocserver>{0}</inprocserver>", name));
            sb.AppendLine(space + string.Format(" <siteclsid>{0}</siteclsid>", "{" + guid + "}"));
            sb.AppendLine(space + " <modulever></modulever>");
            sb.AppendLine(space + " <platformver></platformver>");
            sb.AppendLine(space + " <autoload></autoload>");
            sb.AppendLine(space + string.Format(" <system>{0}</system>", true));
            sb.AppendLine(space + " <displayname>");
            sb.AppendLine(space + string.Format("   <default>{0}</default>", name));
            sb.AppendLine(space + "   <zh-cn></zh-cn>");
            sb.AppendLine(space + " </displayname>");
            sb.AppendLine(space + " <description>");
            sb.AppendLine(space + string.Format("   <default>{0}</default>", description));
            sb.AppendLine(space + "   <zh-cn></zh-cn>");
            sb.AppendLine(space + " </description>");
            sb.AppendLine(space + "</AddIn>");

            StreamWriter sw = new StreamWriter(xmlpath, false, Encoding.UTF8);
            for (int i = 0; i < allLine.Count; i++)
            {
                if (i == endIndex)
                {
                    sw.Write(sb.ToString());
                }
                sw.WriteLine(allLine[i]);
            }
            sw.Close();
            //属性を戻す
            System.IO.File.SetAttributes(xmlpath, attr);
            return true;
        }

        public static int DeleteConfig(string xmlpath, string guid)
        {
            int delCount = 0;

            /* ファイルの有無チェック */
            if (File.Exists(xmlpath) != true)
            {
                return 0;
            }

            //C:\test.txt の属性を取得する
            System.IO.FileAttributes attr = System.IO.File.GetAttributes(xmlpath);

            //読み取り専用属性があるか調べる
            if ((attr & System.IO.FileAttributes.ReadOnly) ==
                System.IO.FileAttributes.ReadOnly)
            {
                //読み取り専用属性を削除する
                System.IO.File.SetAttributes(xmlpath, attr & (~System.IO.FileAttributes.ReadOnly));
            }

            StreamReader sr = new StreamReader(xmlpath);
            List<string> allLine = new List<string>();
            while (sr.EndOfStream != true)
            {
                allLine.Add(sr.ReadLine());
            }
            sr.Close();

            int guidIndex = -1;
            for (int i = 0; i < allLine.Count; i++)
            {
                string str = allLine[i].Trim(' ', '\t');
                if (str.Contains(guid) == true)
                {
                    guidIndex = i;
                    break;
                }
            }
            if (guidIndex == -1)
            {
                return delCount;
            }
            int startIndex = -1;
            for (int i = guidIndex - 1; i >= 0; i--)
            {
                string str = allLine[i].Trim(' ', '\t');
                if (str.Contains("<AddIn>") == true)
                {
                    startIndex = i;
                    break;
                }
            }
            if (startIndex == -1)
            {
                return delCount;
            }
            int endIndex = -1;
            for (int i = startIndex; i < allLine.Count; i++)
            {
                string str = allLine[i].Trim(' ', '\t');
                if (str.Contains("</AddIn>") == true)
                {
                    endIndex = i;
                    break;
                }
            }
            if (endIndex == -1)
            {
                return delCount;
            }
            delCount = 1;
            StreamWriter sw = new StreamWriter(xmlpath, false, Encoding.UTF8);
            for (int i = 0; i < allLine.Count; i++)
            {
                if ((i >= startIndex) && (i <= endIndex))
                {
                    continue;
                }
                sw.WriteLine(allLine[i]);
            }
            sw.Close();
            //属性を戻す
            System.IO.File.SetAttributes(xmlpath, attr);
            return delCount;
        }

        public static bool GetConfigIsEnable(string xmlpath, string searchGuid)
        {
            bool retExist = false;

            /* ファイルの有無チェック */
            if (File.Exists(xmlpath) != true)
            {
                return false;
            }

            //C:\test.txt の属性を取得する
            System.IO.FileAttributes attr = System.IO.File.GetAttributes(xmlpath);

            ////読み取り専用属性があるか調べる
            //if ((attr & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
            //{
            //    //読み取り専用属性を削除する
            //    System.IO.File.SetAttributes(xmlpath, attr & (~System.IO.FileAttributes.ReadOnly));
            //}

            StreamReader sr = new StreamReader(xmlpath);
            List<string> allLine = new List<string>();
            while (sr.EndOfStream != true)
            {
                allLine.Add(sr.ReadLine());
            }
            sr.Close();

            int guidIndex = -1;
            for (int i = 0; i < allLine.Count; i++)
            {
                string str = allLine[i].Trim(' ', '\t');
                if (str.Contains(searchGuid) == true)
                {
                    retExist = true;
                    guidIndex = i;
                    break;
                }
            }
            return retExist;
        }

        #endregion
    }
}
