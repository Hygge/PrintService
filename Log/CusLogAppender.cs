using log4net.Appender;
using log4net.Core;
using log4net.Layout;


namespace PrintService.Log
{
    /// <summary>
    /// 自定义消息处理器
    /// </summary>
    public class CusLogAppender : AppenderSkeleton
    {
        public Action<string> LogEvent; 
        public Action<string> LogException;


        protected override void Append(LoggingEvent loggingEvent)
        {
            // 增加异步操作保证不影响其他业务执行
            Task.Run(() =>
            {
                string log;
                if (this.Layout != null)
                {
                    PatternLayout patternLayout = this.Layout as PatternLayout;
                    log = patternLayout.Format(loggingEvent);

                    if (loggingEvent.ExceptionObject != null)
                    {
                        log += loggingEvent.ExceptionObject.ToString();
                    }
                }
                else
                {
                    log = loggingEvent.RenderedMessage;
                }
                
                LogEvent?.Invoke(log);
            });      
        }


    }
}
