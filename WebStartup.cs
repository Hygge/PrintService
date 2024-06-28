using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintService
{
    public class WebStartup
    {
        public WebApplication app { set; get; }

        public void StartServer(string ip, int? port)
        {
            Task.Run(() =>
            {
                var builder = WebApplication.CreateBuilder();
                
                // 日志添加到容器，启动控制台记录日志
                builder.Services.AddLogging(config =>
                {
                    config.AddConsole();
                });
                // 控制器添加到容器
                builder.Services.AddControllers();

                app = builder.Build();

                int p = port == null ? 8888 : (int)port;
                // 指定服务访问地址
                app.Urls.Add($"http://{ip}:{p}");
                // 使用控制器
                app.MapControllers();

             
                app.Run();
            });


        }


    }
}
