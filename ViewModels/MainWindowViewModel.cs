using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using log4net.Config;
using log4net.Layout;
using PrintService.Domain;
using PrintService.Log;
using PrintService.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using PrintService.Utils;

namespace PrintService.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public WebStartup WebStartup = new WebStartup();

        //启动服务
        public ICommand OpenServer { get; }
        //初始化数据
        public ICommand InitDatabase { get; }
        //停止服务
        public ICommand StopServer { get; }
        //添加打印机标签
        public ICommand AddPrinter { get; }
        public ICommand AddLabel { get; }
        //添加打印模板
        public ICommand AddLableTemplate { get; }
        
        public ICommand Refresh { get; }
        public ICommand Print { get; }

        private string ip ;
        public string Ip
        {
            get => ip;
            set => SetProperty(ref ip, value);
        }
        private int port = 8888;
        public int Port
        {
            get => port;
            set => SetProperty(ref port, value);
        }
        private ObservableCollection<string> ipList = new ObservableCollection<string>();
        public ObservableCollection<string> IpList
        {
            get => ipList;
            set => SetProperty(ref ipList, value);
        }
        private bool isEnabled = true;
        public bool IsEnabled
        {
            get => isEnabled;
            set => SetProperty(ref isEnabled, value);
        }
        /// <summary>
        /// 标签名称集合
        /// </summary>
        private ObservableCollection<string> labelNameList = new ObservableCollection<string>();
        public ObservableCollection<string> LabelNameList
        {
            get => labelNameList;
            set => SetProperty(ref labelNameList, value);
        }
        /// <summary>
        /// 选中需要打印的标签名称
        /// </summary>
        private string labelName;
        public string LabelName
        {
            get => labelName;
            set => SetProperty(ref labelName, value);
        }

        /// <summary>
        /// 打印机集合
        /// </summary>
        private ObservableCollection<string> printNameList = new ObservableCollection<string>();
        public ObservableCollection<string> PrintNameList
        { 
            get => printNameList;
            set => SetProperty(ref printNameList,value);
        }

        /// <summary>
        /// 选中需要的打印机名称
        /// </summary>
        private string printName;
        public string PrintName
        {
            get => printName;
            set =>SetProperty(ref printName, value);
        }

        private string logs;

        public string Logs
        {
            get => logs;
            set => SetProperty(ref logs, value);
        }

        private StringBuilder sb = new StringBuilder();

        public MainWindowViewModel()
        {
            //动态配置界面加载日志，界面获取日志
            string logPattern = "【%date{yyyy-MM-dd HH:mm:ss}】：%message";
            CusLogAppender logAppender = new CusLogAppender() { Layout = new PatternLayout(logPattern) };
            logAppender.LogEvent += LogMessage;
            BasicConfigurator.Configure(logAppender);

            OpenServer = new RelayCommand(openWebServer);
            InitDatabase = new RelayCommand(initDatabase);
            StopServer = new RelayCommand(stopServer);
            AddPrinter = new RelayCommand(addPrinter);
            AddLableTemplate = new RelayCommand(addLableTemplate);
            AddLabel = new RelayCommand(addLabel);
            Refresh = new RelayCommand(refreshData);
            Print = new RelayCommand(printLabel);

            // 获取本机静态ip
            refreshNetwork();
            // 数据库初始
            initDatabase();

            // 初始化获取标签名称集合
            getLabelNameList();

            //初始化获取打印机集合
            getPrintNameList();
        }


        private void refreshData()
        {
            // 获取本机静态ip
            refreshNetwork();

            LabelNameList.Clear();
            // 初始化获取标签名称集合
            getLabelNameList();

            printNameList.Clear();
            //初始化获取打印机集合
            getPrintNameList();
        }

        private void printLabel()
        {
            if (string.IsNullOrEmpty(labelName) || string.IsNullOrEmpty(printName))
            {
                return;
            }

            try
            {
                PrintUtil.FastReportPrintLabelObj(App.printBll.SelectByPrinterName(printName).address,
                    App.printBll.SelectByLabelName(labelName).path, new Dictionary<string, object>());
            }
            catch (Exception exception)
            {
                LogHelper.Error($"{LogHelper.WPF_SHOW_START} 打印失败 {labelName}，失败原因：{exception.Message}", exception );
            }
  
            
        }

        /// <summary>
        /// 获取标签名称集合
        /// </summary>
        private void getLabelNameList()
        {
            List<LabelFileInfo> list = App.printBll.SelectLabelFileList();
            foreach (var item in list)
            {
                LabelNameList.Add(item.name);
            }
        }

        /// <summary>
        /// 获取打印机名称集合
        /// </summary>
        private void getPrintNameList()
        {
            List<Printer> list = App.printBll.SelectPrinterList();
            foreach (var item in list)
            {
                printNameList.Add(item.name);
            }
        }
        //启动服务
        private void openWebServer()
        {
            if (!IsEnabled)
            {
                LogHelper.Info(LogHelper.WPF_SHOW_START + "web服务已开启了=====");
                return;
            }

            LogHelper.Info(LogHelper.WPF_SHOW_START + "正在开启web服务=====");
            WebStartup.StartServer(Ip, Port);
            IsEnabled = false;
            LogHelper.Info(LogHelper.WPF_SHOW_START + "启动web服务完成 🚀 🚀 🚀 ");
        }
        //停止服务
        private void stopServer()
        {
            if (!IsEnabled)
            {
                WebStartup.StopServer();
                IsEnabled = true;
                LogHelper.Info(LogHelper.WPF_SHOW_START + "停止web服务完成 🚀 🚀 🚀");
            }
          
        }
        /// <summary>
        /// 添加打印机
        /// </summary>
        private void addPrinter()
        {
            AddPrinterView print = new AddPrinterView();
            print.Show();
        }
        private void addLabel()
        {
            AddLabelView print = new AddLabelView();
            print.Show();
        }


        /// <summary>
        /// 添加打印模板
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void addLableTemplate()
        {
            AddLableTemplate addLable = new AddLableTemplate();
            addLable.Show();

        }
        /// <summary>
        /// 初始化数据库
        /// </summary>
        private void initDatabase()
        {
            LogHelper.Info(LogHelper.WPF_SHOW_START + "正在初始化数据库 🚀 🚀 🚀");
            try
            {
                App.printBll.InitTable();
                LogHelper.Info(LogHelper.WPF_SHOW_START + " 🚀 🚀 🚀 数据库初始化完");
            }catch(Exception ex)
            {
                LogHelper.Error(LogHelper.WPF_SHOW_START + " 😭 😭 😭 数据库初始失败", ex);
            }
           
        }

        // 日志处理界面显示
        public void LogMessage(string message)
        {
            if (message.Contains(LogHelper.WPF_SHOW_START))
            {
                string log = message.Replace(LogHelper.WPF_SHOW_START, " ");
                if (sb.Length > 1500)
                {
                    sb.Clear();
                }
                sb.Insert(0, "\n").Insert(0, log);
            
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    Logs = sb.ToString();
                });
            }
       
        }

        /// <summary>
        /// 刷新网卡
        /// </summary>
        private void refreshNetwork()
        {
            IpList.Clear();
            // 获取本机的所有网络接口
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                // 排除虚拟网络接口和回环接口
                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    // 获取网络接口的IP属性
                    IPInterfaceProperties properties = networkInterface.GetIPProperties();

                    // 获取该接口的所有IP地址
                    foreach (IPAddressInformation address in properties.UnicastAddresses)
                    {
                        // 输出IPv4地址
                        if (address.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            IpList.Add(Convert.ToString(address.Address));
                        }
                        // 输出IPv6地址
                        else if (address.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                        {
                            //Debug.WriteLine($"IPv6 Address: {address.Address}");
                        }
                    }
                }
            }

        }

    }
}
