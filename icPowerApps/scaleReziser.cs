using AdvancedDataGridView;
using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ICApiAddin.icPowerApps
{
    public class ScaleReziser
    {
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117,
            LOGPIXELSY = 90
        }

        /// <summary>
        /// 表示スケールを取得する
        /// </summary>
        /// <returns></returns>
        public static float getScalingFactor()
        {
            float scale = 1;
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                IntPtr desktop = g.GetHdc();
                int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
                int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);
                int logpixelsy = GetDeviceCaps(desktop, (int)DeviceCap.LOGPIXELSY);
                float screenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;
                float dpiScalingFactor = (float)logpixelsy / (float)96;

                if (screenScalingFactor > dpiScalingFactor)
                {
                    scale = screenScalingFactor;
                }
                else
                {
                    scale = dpiScalingFactor;
                }
            }
            return scale;
        }

        public static void InitializeFormControlScale(Control control, bool resizeDataGridView, bool resizeSplitContainer, bool resizeTreeGridView, bool resizeTreeView, bool resizeListBox)
        {
            float scale = getScalingFactor();
            if (scale > 1)
            {
                Control[] allControl = GetAllControls(control);
                foreach (Control item in allControl)
                {
                    if (resizeSplitContainer == true)
                    {
                        if (item is SplitContainer)
                        {
                            ResizeSplitContainer(item as SplitContainer, scale);
                            continue;
                        }
                    }
                    if (resizeDataGridView == true)
                    {
                        if (item is DataGridView)
                        {
                            ResizeDataGridView(item as DataGridView, scale);
                            continue;
                        }
                    }
                    if (resizeTreeView == true)
                    {
                        if (item is TreeView)
                        {
                            ResizeTreeView(item as TreeView, scale);
                            continue;
                        }
                    }
                    if (resizeListBox == true)
                    {
                        if (item is ListBox)
                        {
                            ResizeListBox(item as ListBox, scale);
                            continue;
                        }
                    }
                    if (resizeTreeGridView == true)
                    {
                        if (item is TreeGridView)
                        {
                            ResizeTreeGridView(item as TreeGridView, scale);
                            continue;
                        }
                    }
                }
            }
        }

        private static Control[] GetAllControls(Control top)
        {
            ArrayList buf = new ArrayList();
            foreach (Control c in top.Controls)
            {
                buf.Add(c);
                buf.AddRange(GetAllControls(c));
            }
            return (Control[])buf.ToArray(typeof(Control));
        }


        public static void ResizeSplitContainer(SplitContainer container, float scale)
        {
            container.SplitterDistance = (int)(container.SplitterDistance * scale);
        }
        public static void ResizeDataGridView(DataGridView dgv, float scale)
        {
            dgv.ColumnHeadersHeight = (int)(dgv.ColumnHeadersHeight * scale);
            dgv.RowTemplate.Height = (int)(dgv.RowTemplate.Height * getScalingFactor());
        }
        public static void ResizeTreeView(TreeView tv, float scale)
        {
            tv.ItemHeight = (int)(tv.ItemHeight * getScalingFactor());
        }
        public static void ResizeListBox(ListBox lb, float scale)
        {
            lb.ItemHeight = (int)(lb.ItemHeight * getScalingFactor());
        }
        public static void ResizeTreeGridView(TreeGridView tgv, float scale)
        {
            tgv.ColumnHeadersHeight = (int)(tgv.ColumnHeadersHeight * scale);
            tgv.RowTemplate.Height = (int)(tgv.RowTemplate.Height * getScalingFactor());
        }

    }
}