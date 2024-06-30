using log4net.Config;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintService.Views;

namespace PrintService.Log
{
    /// <summary>
    /// 通用日志记录
    /// </summary>
    public class LogHelper
    {
        private static readonly ILog Instance = LogManager.GetLogger(typeof(LogHelper));

        /// <summary>
        /// 日志前缀代表该日志在界面显示，根据此前缀过滤后台日志
        /// </summary>
        public static readonly string WPF_SHOW_START = "WPF-";


        public static void EnableDefault()
        {
            XmlConfigurator.Configure(new FileInfo(Path.Combine(Environment.CurrentDirectory, "log4Net.config")));
        }

        /// <summary>
        /// 日志启用并使用对应配置文件
        /// </summary>
        /// <param name="filePath">日志配置文件路径</param>
        public static void EnableAndSetConfig(string filePath)
        {
            FileInfo configFile = new FileInfo(filePath);
            XmlConfigurator.Configure(configFile);
        }

        public static void SetConfig(FileInfo configFile)
        {
            XmlConfigurator.Configure(configFile);
        }

        /// <summary>
        /// 记录普通文件记录
        /// </summary>
        /// <param name="info"></param>
        public static void Info(string info)
        {
            if (Instance.IsInfoEnabled)
            {
                Instance.Info(info);
            }
        }

        /// <summary>
        ///记录调试信息
        /// </summary>
        /// <param name="info"></param>
        public static void Debug(string info)
        {
            if (Instance.IsErrorEnabled)
            {
                Instance.Debug(info);
            }
        }

        /// <summary>
        ///记录警告信息
        /// </summary>
        /// <param name="info"></param>
        public static void Warn(string info)
        {
            if (Instance.IsWarnEnabled)
            {
                Instance.Warn(info);
            }
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="info"></param>
        /// <param name="se"></param>
        public static void Error(string info, Exception se)
        {
            if (Instance.IsErrorEnabled)
            {
                Instance.Error(info, se);
            }
        }

        /// <summary>
        /// 记录严重错误
        /// </summary>
        /// <param name="info"></param>
        /// <param name="se"></param>
        public static void Fatal(string info, Exception se)
        {
            if (Instance.IsFatalEnabled)
            {
                Instance.Fatal(info, se);
            }
        }
    }
}
