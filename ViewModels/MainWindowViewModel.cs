using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using log4net.Config;
using log4net.Layout;
using PrintService.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace PrintService.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public WebStartup WebStartup = new WebStartup();

        public ICommand OpenServer { get; }
        public ICommand InitDatabase { get; }



        public MainWindowViewModel()
        {
            //动态配置界面加载日志，界面获取日志
            string logPattern = "【%date{yyyy-MM-dd HH:mm:ss}】: %message";
            CusLogAppender logAppender = new CusLogAppender() { Layout = new PatternLayout(logPattern) };
            logAppender.LogEvent += LogMessage;
            BasicConfigurator.Configure(logAppender);

            OpenServer = new RelayCommand(openWebServer);
            InitDatabase = new RelayCommand(initDatabase);
        }

        private void openWebServer()
        {
            LogHelper.Info("正在开启web服务=====");
            WebStartup.StartServer("localhost", null);
            LogHelper.Info("启动web服务完成》》》》》》》》》》》》");
        }
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

    }
}
