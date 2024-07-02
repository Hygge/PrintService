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
using System.Windows.Input;
using System.Windows.Threading;

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
        //添加打印模板
        public ICommand AddLableTemplate { get; }

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

        public MainWindowViewModel()
        {
            //动态配置界面加载日志，界面获取日志
            string logPattern = "【%date{yyyy-MM-dd HH:mm:ss}】: %message";
            CusLogAppender logAppender = new CusLogAppender() { Layout = new PatternLayout(logPattern) };
            logAppender.LogEvent += LogMessage;
            BasicConfigurator.Configure(logAppender);

            OpenServer = new RelayCommand(openWebServer);
            InitDatabase = new RelayCommand(initDatabase);
            StopServer = new RelayCommand(stopServer);
            AddPrinter = new RelayCommand(addPrinter);
            AddLableTemplate = new RelayCommand(addLableTemplate);

            // 获取本机静态ip
            refreshNetwork();
            // 数据库初始
            initDatabase();

            // 初始化获取标签名称集合
            getLabelNameList();

            //初始化获取打印机集合
            getPrintNameList();
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

            LogHelper.Info(LogHelper.WPF_SHOW_START + "正在开启web服务=====");
            WebStartup.StartServer(Ip, Port);
            IsEnabled = false;
            LogHelper.Info(LogHelper.WPF_SHOW_START + "启动web服务完成 🚀 🚀 🚀 》》》》》》》》》");
        }
        //停止服务
        private void stopServer()
        {
            if (!IsEnabled)
            {
                WebStartup.StopServer();
                IsEnabled = true;
                LogHelper.Info(LogHelper.WPF_SHOW_START + "停止web服务完成 🚀 🚀 🚀 》》》》》》》");
            }
          
        }
        /// <summary>
        /// 添加打印机
        /// </summary>
        private void addPrinter()
        {
            AddPrint print = new AddPrint();
            print.Show();
            Printer printer = new Printer();
            printer.name = "a12123";
            printer.description = "测试打印机";
            printer.address = "123";
            App.printBll.InsertPrinter(printer);
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
                LogHelper.Info(LogHelper.WPF_SHOW_START + " 🚀 🚀 🚀 数据库初始化完成》》》》》》》》》》》》");
            }catch(Exception ex)
            {
                LogHelper.Error(LogHelper.WPF_SHOW_START + " 😭 😭 😭 数据库初始失败》》》》》》》》》》》》", ex);
            }
           
        }

        // 日志处理界面显示
        public void LogMessage(string message)
        {
            if (message.Contains(LogHelper.WPF_SHOW_START))
            {
                string log = message.Replace(LogHelper.WPF_SHOW_START, " ");
                LogHelper.Info("消息显示界面");
                Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                {
                    // 确保在UI线程中
                    // this.txtLog.AppendText(message + Environment.NewLine);
                    LogHelper.Info("消息显示界面");

                }));
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
