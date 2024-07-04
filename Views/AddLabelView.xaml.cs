using PrintService.ViewModels;

using Wpf.Ui.Appearance;

namespace PrintService.Views
{
    /// <summary>
    /// AddLabelView.xaml 的交互逻辑
    /// </summary>
    public partial class AddLabelView 
    {
        public LabelAndPrinterViewModel ViewModel { get; }
        public AddLabelView()
        {
            ViewModel = new LabelAndPrinterViewModel(this);
            SystemThemeWatcher.Watch(this);
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
