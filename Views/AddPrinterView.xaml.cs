using PrintService.ViewModels;
using Wpf.Ui.Appearance;

namespace PrintService.Views
{
    /// <summary>
    /// AddPrinterView.xaml 的交互逻辑
    /// </summary>
    public partial class AddPrinterView 
    {
        
        public LabelAndPrinterViewModel ViewModel { get; }
        public AddPrinterView()
        {
            ViewModel = new LabelAndPrinterViewModel(this);
            SystemThemeWatcher.Watch(this);
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
