using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Alliance
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var sb = new StringBuilder();

            sb.AppendLine("CurrentDomain_UnhandledException");
            sb.AppendLine(e.ToString());
            sb.AppendLine((e.ExceptionObject as Exception)?.ToString());

            MessageBox.Show(sb.ToString());
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Dispatcher_UnhandledException");
            sb.AppendLine(e.ToString());
            sb.AppendLine(e.Exception?.ToString());

            MessageBox.Show(sb.ToString());
        }
    }
}
