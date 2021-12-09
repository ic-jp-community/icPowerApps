# icPowerApps
IRONCADを便利にするアドインです。

本プロジェクトのビルドには開発環境のPCにWebView2ランタイムが必要です。\
https://developer.microsoft.com/ja-jp/microsoft-edge/webview2/

## 【初めてIRONCAD JP Communityの配布するアドインをビルドする方】
環境構築を行ってください。(Microsoft Visual Studio 2015 Installer Projectsのインストールが必要です。)\
https://ironcad.fun/download/ironcad-addin-sample-cs/develop_prepare_cs/

## 【ビルド前に行ってください(初回のみ)】
1.BootStrapper\MicrosoftEdgeWebView2RuntimeのMERGE.BATを実行してください。\
2.BootStrapper\MicrosoftEdgeWebView2Runtimeのフォルダを\
  C:\Program Files (x86)\Microsoft Visual Studio 14.0\SDK\Bootstrapper\Packagesに\
  フォルダごとコピーしてください。

## 【インストール時の注意事項】
setup.exeを右クリックして[管理者として実行]でインストールしてください。\
※管理者として実行をしない場合、インストールユーザ以外でicWebブラウザが使用できない可能性があります。
