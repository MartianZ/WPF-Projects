using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Text;

namespace MusicSorter
{
	/// <summary>
	/// App.xaml 的交互逻辑
	/// </summary>
	public partial class App : Application
	{

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("应用程序出现了未捕获的异常，请将下列信息反馈到博客，以帮助我们改进软件，万分谢谢！\n官方博客：http://www.4321.la (提示：Ctrl+C可复制)\n\n{0}\n",
                e.Exception.Message);

            if (e.Exception.InnerException != null)
            {
                stringBuilder.AppendFormat("\n {0}", e.Exception.InnerException.Message);
            }

            stringBuilder.AppendFormat("\n {0}", e.Exception.StackTrace);
            MessageBox.Show(stringBuilder.ToString(), "ERROR");
            e.Handled = true;  
        }
    }
}