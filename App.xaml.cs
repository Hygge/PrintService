using Microsoft.Extensions.Configuration;
using PrintService.Bll;
using PrintService.Log;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PrintService
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public readonly static PrintBll printBll = new PrintBll();
        public readonly static IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory).AddJsonFile("AppSettings.json").Build();

        protected override void OnStartup(StartupEventArgs e)
        {
            // 启用日志
            LogHelper.EnableDefault();
            base.OnStartup(e);
            Application.Current.StartupUri = new Uri("/Views/MainView.xaml", UriKind.Relative);

        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Environment.Exit(0);
        }
    }

}
