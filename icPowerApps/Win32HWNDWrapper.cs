using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICApiAddin.icPowerApps
{
    public class Win32HWNDWrapper : System.Windows.Forms.IWin32Window
    {
        private System.IntPtr _hwnd;
        public System.IntPtr Handle
        {
            get
            {
                return _hwnd;
            }
        }

        public Win32HWNDWrapper(System.IntPtr Handle)
        {
            _hwnd = Handle;
        }
    }

}