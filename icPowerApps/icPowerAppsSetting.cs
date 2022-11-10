using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace ICApiAddin.icPowerApps
{
    public class icPowerAppsSetting
    {
        /// <summary>
        /// 共通のコンフィグファイルのパスを取得する
        /// </summary>
        /// <returns></returns>
        public static string GetCommonConfigFilePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"IRONCAD JP Community\icPowerApps\icPowerAppsCommon.config");
        }

        /// <summary>
        /// ユーザのコンフィグファイルのパスを取得する
        /// </summary>
        /// <returns></returns>
        public static string GetUserConfigFilePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"IRONCAD JP Community\icPowerApps\icPowerAppsUser.config");
        }

        private static icPowerAppsConfig _config;
        public static icPowerAppsConfig GetConfig()
        {
            if (_config == null)
            {
                _config = new icPowerAppsConfig();
                return _config;
            }
            return _config;
        }

        public static icPowerAppsCommonConfig GetCommonConfig()
        {
            if (_config == null)
            {
                _config = new icPowerAppsConfig();
            }
            if (_config.CommonConfig == null)
            {
                _config.CommonConfig = new icPowerAppsCommonConfig();
            }
            return _config.CommonConfig;
        }

        public static icPowerAppsUserConfig GetUserConfig()
        {
            if (_config == null)
            {
                _config = new icPowerAppsConfig();
            }
            if (_config.UserConfig == null)
            {
                _config.UserConfig = new icPowerAppsUserConfig();
            }
            return _config.UserConfig;
        }

        public static void SetCommonConfig(icPowerAppsCommonConfig setConfig)
        {
            GetConfig().CommonConfig = setConfig;
        }
        public static void SetUserConfig(icPowerAppsUserConfig setConfig)
        {
            GetConfig().UserConfig = setConfig;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public icPowerAppsSetting()
        {

        }

        #region コンフィグファイルのデータ構造
        /// <summary>
        /// icVaultのすべての設定
        /// </summary>
        [Serializable]
        public class icPowerAppsConfig
        {
            public icPowerAppsUserConfig UserConfig = new icPowerAppsUserConfig();
            public icPowerAppsCommonConfig CommonConfig = new icPowerAppsCommonConfig();
        }

        /// <summary>
        /// icPowerApps共通アプリケーションの設定
        /// </summary>CommonAppConfig
        [Serializable]
        public class icPowerAppsCommonConfig
        {
            public string dummyConfig;
            public icPowerAppsCommonConfig()
            {
                this.dummyConfig = string.Empty;
            }
        }

        /// <summary>
        /// icPowerAppsユーザ別アプリケーションの設定
        /// </summary>CommonAppConfig
        [Serializable]
        public class icPowerAppsUserConfig
        {
            public ClientAppConfig ClientConfig;
            public icPowerAppsUserConfig()
            {
                this.ClientConfig = new ClientAppConfig();
            }
        }

 

        /// <summary>
        /// icPowerAppsClientアプリケーションの設定
        /// </summary>
        [Serializable]
        public class ClientAppConfig
        {
            public List<AddInToolIconSize> AppList;
            public ClientAppConfig()
            {
                this.AppList = new List<AddInToolIconSize>();
            }
        }


        /// <summary>
        /// 各ツールのアイコンサイズ設定
        /// </summary>
        ///        
        [Serializable]
        public class AddInToolIconSize
        {
            public string uniqueName { get; set; }
            public string displayName { get; set; }
            public bool isLargeIcon;
            public bool isEnable;
            public AddInToolIconSize()
            {
                this.uniqueName = string.Empty;
                this.displayName = string.Empty;
                this.isLargeIcon = true;
                this.isEnable = true;
            }
            public AddInToolIconSize(string value, string displayName, bool isLarge, bool enable)
            {
                this.uniqueName = value;
                this.displayName = displayName;
                this.isLargeIcon = isLarge;
                this.isEnable = enable;
            }
        }

        #endregion

        #region 設定ファイルの読み書き処理
        /// <summary>
        /// icPowerAppsのConfigファイルを出力する
        /// </summary>
        /// <param name="filePath">出力ファイルパス</param>
        /// <param name="config">設定データ</param>
        public static void WriteicPowerAppsCommonSetting(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (Directory.Exists(dir) != true)
            {
                Directory.CreateDirectory(dir);
            }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(icPowerAppsCommonConfig));
            using (var streamWriter = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // シリアライズする
                xmlSerializer.Serialize(streamWriter, GetCommonConfig());
                streamWriter.Flush();
            }
        }

        /// <summary>
        /// icPowerAppsのConfigファイルを出力する
        /// </summary>
        /// <param name="filePath">出力ファイルパス</param>
        /// <param name="config">設定データ</param>
        public static void WriteicPowerAppsUserSetting(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (Directory.Exists(dir) != true)
            {
                Directory.CreateDirectory(dir);
            }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(icPowerAppsUserConfig));
            using (var streamWriter = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // シリアライズする
                xmlSerializer.Serialize(streamWriter, GetUserConfig());
                streamWriter.Flush();
            }
        }

        public static void ReadicPowerAppsCommonSetting(string filePath)
        {
            icPowerAppsCommonConfig readConfig = new icPowerAppsCommonConfig();
            ReadicPowerAppsCommonSetting(filePath, ref readConfig, true);
        }
        public static void ReadicPowerAppsCommonSetting(string filePath, bool shorErrorMessage)
        {
            icPowerAppsCommonConfig readConfig = new icPowerAppsCommonConfig();
            ReadicPowerAppsCommonSetting(filePath, ref readConfig, shorErrorMessage);
        }
        public static void ReadicPowerAppsUserSetting(string filePath)
        {
            icPowerAppsUserConfig readConfig = new icPowerAppsUserConfig();
            ReadicPowerAppsUserSetting(filePath, ref readConfig, true);
        }
        public static void ReadicPowerAppsUserSetting(string filePath, bool shorErrorMessage)
        {
            icPowerAppsUserConfig readConfig = new icPowerAppsUserConfig();
            ReadicPowerAppsUserSetting(filePath, ref readConfig, shorErrorMessage);
        }

        /// <summary>
        /// icPowerAppsのConfigファイルを読み込む(共通データ CommonApp)
        /// </summary>
        /// <param name="filePath">設定ファイルのパス</param>
        /// <param name="config">読み込んだ設定データ</param>
        public static bool ReadicPowerAppsCommonSetting(string filePath, ref icPowerAppsCommonConfig readConfig, bool showErrorMessage)
        {
            bool readResult = false;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(icPowerAppsCommonConfig));
            XmlReaderSettings xmlSettings = new XmlReaderSettings() { CheckCharacters = false };

            StreamReader streamReader = null;
            try
            {
                if (File.Exists(filePath) == false)
                {
                    MessageBox.Show("設定ファイルがありません。\nデフォルト設定を読み込みます。");
                    readConfig = new icPowerAppsCommonConfig();
                    return false;
                }
                // ファイルを読み込む
                streamReader = new StreamReader(filePath, Encoding.UTF8);
                // デシリアライズする
                XmlReader xmlReader = XmlReader.Create(streamReader, xmlSettings);
                readConfig = (icPowerAppsCommonConfig)xmlSerializer.Deserialize(xmlReader);
                readResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("設定ファイルの読み込みに失敗しました。\nデフォルト設定を読み込みます。");
                readConfig = new icPowerAppsCommonConfig();
                readResult = false;
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
            }
            /* 読んだデータは現在のConfigに反映させる */
            SetCommonConfig(readConfig);

            return readResult;
        }

        /// <summary>
        /// icPowerAppsのConfigファイルを読み込む(ユーザデータ UserApp)
        /// </summary>
        /// <param name="filePath">設定ファイルのパス</param>
        /// <param name="config">読み込んだ設定データ</param>
        public static bool ReadicPowerAppsUserSetting(string filePath, ref icPowerAppsUserConfig readConfig, bool showErrorMessage)
        {
            bool readResult = false;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(icPowerAppsUserConfig));
            XmlReaderSettings xmlSettings = new XmlReaderSettings() { CheckCharacters = false };

            StreamReader streamReader = null;
            try
            {
                if (File.Exists(filePath) == false)
                {
                    if(showErrorMessage == true)
                    {
                        MessageBox.Show("ユーザ設定ファイルがありません。\nデフォルト設定を読み込みます。");
                    }
                    readConfig = new icPowerAppsUserConfig();
                    return false;
                }
                // ファイルを読み込む
                streamReader = new StreamReader(filePath, Encoding.UTF8);
                // デシリアライズする
                XmlReader xmlReader = XmlReader.Create(streamReader, xmlSettings);
                readConfig = (icPowerAppsUserConfig)xmlSerializer.Deserialize(xmlReader);
                readResult = true;
            }
            catch (Exception ex)
            {
                if (showErrorMessage == true)
                {
                    MessageBox.Show("ユーザ設定ファイルの読み込みに失敗しました。\nデフォルト設定を読み込みます。");
                }
                readConfig = new icPowerAppsUserConfig();
                readResult = false;
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
            }
            /* 読んだデータは現在のConfigに反映させる */
            SetUserConfig(readConfig);

            return readResult;
        }

        #endregion

    }
}
