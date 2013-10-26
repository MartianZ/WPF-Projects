using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Interop;

namespace MusicSorter
{
    /// <summary>
    /// Windows_Custom_Messagebox.xaml 的交互逻辑
    /// </summary>
    public partial class Windows_Custom_Messagebox : Window
    {
        public int Return_Result = 0;

        public Windows_Custom_Messagebox()
        {
            InitializeComponent();
        }

        #region 标题栏拖拽相应
        private const int WM_NCHITTEST = 0x0084;
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_NCHITTEST)
            {
                // 获取屏幕坐标
                Point p = new Point();
                int pInt = lParam.ToInt32();
                p.X = (pInt << 16) >> 16;
                p.Y = pInt >> 16;
                if (isOnTitleBar(PointFromScreen(p)))
                {
                    // 欺骗系统鼠标在标题栏上
                    handled = true;
                    return new IntPtr(2);
                }
            }

            return IntPtr.Zero;
        }

        private bool isOnTitleBar(Point p)
        {
            // 假设标题栏在0和34之间
            if (p.Y >= 0 && p.Y < 34 && p.X < 520)
                return true;
            else
                return false;
        }
        #endregion

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            Return_Result = 1;
            this.Hide();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Return_Result = 0;
            this.Hide();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(hwnd).AddHook(new HwndSourceHook(WndProc));
        }
    }
}
