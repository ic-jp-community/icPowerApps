using System;
using System.Windows.Forms;

namespace ICApiAddin.icPowerApps
{
    public partial class UserControlSuppressManager_ChangeHeaderNameForm : Form
    {
        /* 上位への通知用データ */
        public bool _returnIsChange = false;
        public string _returnChangeHeaderText = string.Empty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="headerText"></param>
        public UserControlSuppressManager_ChangeHeaderNameForm(string headerText)
        {
            InitializeComponent();
            this.textBoxHeaderName.Text = headerText;
        }


        /// <summary>
        /// OKボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            string changeHeaderText = textBoxHeaderName.Text;
            this._returnIsChange = true;
            this._returnChangeHeaderText = changeHeaderText;
            this.Close();
        }


        /// <summary>
        /// キャンセルボタンクリック イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
