using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintService.Log;

namespace PrintService
{
    public class WebStartup
    {
        private readonly ILog log = LogManager.GetLogger(typeof(WebStartup));

        public WebApplication app { set; get; }

        public void StartServer(string? ip, int? port)
        {
            Task.Run(() =>
            {
                var builder = WebApplication.CreateBuilder();                
               
                // 控制器添加到容器
                builder.Services.AddControllers();

                app = builder.Build();

                int p = port == null || port == 0 ? 8888 : (int)port;
                string i = ip == null ? "localhost" : ip;
                // 指定服务访问地址
                app.Urls.Add($"http://{i}:{p}");

                // 使用控制器
                app.MapControllers();

                log.Info("》》》》》服务启动完成 🚀🚀🚀🚀🚀🚀🚀🚀 》》》》》");
                log.Info($"{LogHelper.WPF_SHOW_START} 服务地址：http://{i}:{p}");

                app.Run();
         

            });


        }


        public void StopServer()
        {
            app.StopAsync();
        }


    }
}
