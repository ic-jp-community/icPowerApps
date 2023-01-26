using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace icAPIAddinEnableDisable
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Count() > 0)
            {
                /* 引数あり */
                List<ArgsParam.AddinSettingDataSet> addinSettingList = new List<ArgsParam.AddinSettingDataSet>();
                string allArgs = string.Empty;
                foreach (string argStr in args)
                {
                    allArgs += " " + argStr;
                }

                for (int i = 0; i < args.Count(); i++)
                {
                    string arg = args[i];
                    int readEndIndex = 0;
                    bool checkResult = false;
                    switch (arg)
                    {
                        case "/AddinEnable":
                            ArgsParam.AddinSettingDataSet paramEnable = new ArgsParam.AddinSettingDataSet();
                            checkResult = checkArgsAddinEnableDisable(args, (i + 1), ref readEndIndex, ref paramEnable);
                            if (checkResult != true)
                            {
                                MessageBox.Show(string.Format("CommandLine AddinSetting Argument Error (Args:{0})", allArgs));
                                Environment.Exit((int)EXIT_CODE.CODE.ERROR_INVALID_ARGUMENT_ADDIN_SETTING_ENABLE);
                                return;
                            }
                            paramEnable.isEnable = true;
                            addinSettingList.Add(paramEnable);
                            i = readEndIndex;
                            break;

                        case "/AddinDisable":
                            ArgsParam.AddinSettingDataSet paramDisable = new ArgsParam.AddinSettingDataSet();
                            checkResult = checkArgsAddinEnableDisable(args, (i + 1), ref readEndIndex, ref paramDisable);
                            if (checkResult != true)
                            {
                                MessageBox.Show(string.Format("CommandLine AddinSetting Argument Error (Args:{0})", allArgs));
                                Environment.Exit((int)EXIT_CODE.CODE.ERROR_INVALID_ARGUMENT_ADDIN_SETTING_DISABLE);
                                return;
                            }
                            paramDisable.isEnable = false;
                            addinSettingList.Add(paramDisable);
                            i = readEndIndex;
                            break;

                        default:
                            break;
                    }
                }
                Application.Run(new Form_icAPIAddinEnableDisable(addinSettingList));
            }
            else
            {
                Application.Run(new Form_icAPIAddinEnableDisable());
            }
        }

        /// <summary>
        /// 引数が主コマンドであるかチェックする
        /// </summary>
        /// <param name="cmdStr"></param>
        /// <returns></returns>
        private static bool checkCMDword(string cmdStr)
        {
            bool findCommand = false;
            /* 主コマンド */
            string[] cmds = { "/AddinEnable", "/AddinDisable" };

            /* 主コマンドであるかチェック */
            for (int i = 0; i < cmds.Count(); i++)
            {
                if (string.Equals(cmds[i], cmdStr) == true)
                {
                    /* 主コマンド */
                    findCommand = true;
                    break;
                }
            }
            return findCommand;
        }

        /// <summary>
        /// アドイン設定コマンドのパラメータを取得する
        /// </summary>
        /// <param name="args"></param>
        /// <param name="currIndex"></param>
        /// <param name="readEndIndex"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private static bool checkArgsAddinEnableDisable(string[] args, int currIndex, ref int readEndIndex, ref ArgsParam.AddinSettingDataSet param)
        {
            bool ret = false;
            int argsSize = 4;
            int remainArgs = args.Count() - currIndex;
            if (remainArgs >= argsSize)
            {
                int argCount = 0;
                for (int i = currIndex; i < (currIndex+ argsSize); i++)
                {
                    string arg = args[i];
                    /* パラメータが主コマンドであるかチェック */
                    if (checkCMDword(arg) == true)
                    {
                        /* 主コマンドなので抜ける */
                        break;
                    }
                    switch (argCount)
                    {
                        case 0:
                            if(File.Exists(arg) != true)
                            {
                                /* コンフィグファイルがない */
                                ret = false;
                                break;
                            }
                            param.configPath = arg;
                            break;
                        case 1:
                            param.guid = arg;
                            break;
                        case 2:
                            param.addinName = arg;
                            break;
                        case 3:
                            param.addinDescription = arg;
                            ret = true;
                            break;
                        default:
                            break;
                    }
                    argCount++;
                    readEndIndex = i;
                }
            }
            else
            {
                /* 引数が足りない */
                ret = false;
            }

            return ret;
        }

    }
}
