using PrintService.Bll;
using PrintService.Log;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace PrintService
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public readonly static PrintBll printBll = new PrintBll();

        protected override void OnStartup(StartupEventArgs e)
        {
            // 启用日志
            LogHelper.EnableDefault();
            base.OnStartup(e);
            Application.Current.StartupUri = new Uri("/Views/MainWindow.xaml", UriKind.Relative);
            string wwwroot = Path.Combine(Environment.CurrentDirectory, "wwwroot");
            // 启动时创建模板文件路径
            if (!Directory.Exists(wwwroot))
            {
                Directory.CreateDirectory(wwwroot);
            }

        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Environment.Exit(0);
        }
    }

}
