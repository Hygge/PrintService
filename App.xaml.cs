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
        protected override void OnStartup(StartupEventArgs e)
        {
            LogHelper.EnableDefault();
            base.OnStartup(e);
            Application.Current.StartupUri = new Uri("/Views/MainWindow.xaml", UriKind.Relative);

        }
    }

}
