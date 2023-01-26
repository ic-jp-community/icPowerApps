using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace icAPIAddinEnableDisable
{
    public class EXIT_CODE
    {
        public enum CODE
        {
            RET_NG = -1,
            RET_OK = 0,
            // Maintenance用に起こしたコード
            ERROR_BACKUP_DATABASE,
            ERROR_BACKUP_VAULT,
            ERROR_BACKUP_DATABASE_AND_VAULT,
            ERROR_INVALID_ARGUMENT_ADDIN_SETTING_ENABLE,
            ERROR_INVALID_ARGUMENT_ADDIN_SETTING_DISABLE,
        }
    }

    public class RETUEN_CODE
    {
        public enum CODE
        {
            // Maintenance用に起こしたコード
            ERROR_SCRIPT_FILE_IS_NOTHING,
            ERROR_IC_VAULT_PATH_NOT_FOUND,
            ERROR_BACKUP_CREATE_UNKNOWN_ERROR,
            ERROR_TASK_SCHEDULE_MONTHLY_PARAM,
            ERROR_TASK_SCHEDULE_WEEKLY_PARAM,
            ERROR_TASK_SCHEDULE_EVERYDAY_PARAM,
            ERROR_TASK_SCHEDULE_EMPTY_SAVE_FILE_NAME,
            ERROR_TASK_SCHEDULE_EMPTY_SAVE_DIRECTORY,
            ERROR_TASK_SCHEDULE_NOT_EXIST_SAVE_DIRECTORY,

            WARNING_FILE_IS_NOTHING,
            RET_CANCEL,
            RET_OK,
            RET_NG,
        }
        public static Dictionary<int, string> DictionaryErrMessages = new Dictionary<int, string>()
        {
              {(int)CODE.ERROR_SCRIPT_FILE_IS_NOTHING, "スクリプトファイルが見つかりません。" },
              {(int)CODE.ERROR_IC_VAULT_PATH_NOT_FOUND, "vaultのパスが取得できませんでした。" },
              {(int)CODE.ERROR_BACKUP_CREATE_UNKNOWN_ERROR, "バックアップ作成中に不明なエラーが発生しました。" },
              {(int)CODE.ERROR_TASK_SCHEDULE_MONTHLY_PARAM, "毎月の実行日設定が未入力です。" },
              {(int)CODE.ERROR_TASK_SCHEDULE_WEEKLY_PARAM, "毎週の曜日設定が未入力です。" },
              {(int)CODE.ERROR_TASK_SCHEDULE_EVERYDAY_PARAM, "毎日の設定が未入力です。" },
              {(int)CODE.ERROR_TASK_SCHEDULE_EMPTY_SAVE_FILE_NAME, "バックアップ保存ファイル名が未入力です。" },
              {(int)CODE.ERROR_TASK_SCHEDULE_EMPTY_SAVE_DIRECTORY, "バックアップ保存場所が未入力です。" },
              {(int)CODE.ERROR_TASK_SCHEDULE_NOT_EXIST_SAVE_DIRECTORY, "指定したバックアップ保存場所は存在しません。" },


              {(int)CODE.WARNING_FILE_IS_NOTHING, "ファイルがありません。"},
              {(int)CODE.RET_CANCEL, "キャンセルしました"},
              {(int)CODE.RET_OK, "OK"},
              {(int)CODE.RET_NG, "NG"},
        };
    }
}
