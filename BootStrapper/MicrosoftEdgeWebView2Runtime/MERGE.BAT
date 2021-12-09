@ECHO OFF
REM 結合バッチファイル：div2.exe により生成

ECHO 分割されたファイルを復元します

IF NOT EXIST "MicrosoftEdgeWebView2RuntimeInstallerX64.001"  GOTO ERR1
IF NOT EXIST "MicrosoftEdgeWebView2RuntimeInstallerX64.002"  GOTO ERR1
IF     EXIST "MicrosoftEdgeWebView2RuntimeInstallerX64.exe"  GOTO ERR2

COPY "MicrosoftEdgeWebView2RuntimeInstallerX64.001" "MicrosoftEdgeWebView2RuntimeInstallerX64.exe"
IF ERRORLEVEL 1 GOTO ERR3
COPY /B "MicrosoftEdgeWebView2RuntimeInstallerX64.exe"+"MicrosoftEdgeWebView2RuntimeInstallerX64.002"
IF ERRORLEVEL 1 GOTO ERR3

DEL "MicrosoftEdgeWebView2RuntimeInstallerX64.001"
DEL "MicrosoftEdgeWebView2RuntimeInstallerX64.002"


REN "MicrosoftEdgeWebView2RuntimeInstallerX64.exe" "MicrosoftEdgeWebView2RuntimeInstallerX64.exe"

ECHO このバッチファイルも消してください
GOTO END

:ERR1
ECHO ファイルが一部存在しません
ECHO 処理を中止します
GOTO END

:ERR2
ECHO 復元先のファイルが既に存在しています
ECHO 処理を中止します
GOTO END

:ERR3
ECHO 復元に失敗しました
ECHO 処理を中止します
GOTO END

:END
ECHO 終了するには、何かキーを押してください
PAUSE >NUL

