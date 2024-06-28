using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PrintService.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public WebStartup WebStartup = new WebStartup();

        public ICommand OpenServer { get; }



        public MainWindowViewModel()
        {
            OpenServer = new RelayCommand(openWebServer);
        }

        private void openWebServer()
        {
            WebStartup.StartServer("localhost", null);
        }


    }
}
