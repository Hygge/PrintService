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

              
                //注册Swagger
                builder.Services.AddSwaggerGen(u => {
                    u.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Version = "Ver:1.0.0",//版本
                        Title = "Hygge标签打印服务",//标题
                        Description = "打印标签上位：界面由张老师提供",//描述
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                        {
                            Name = "西瓜程序猿",
                            Email = "xxx@qq.com"
                        }
                    });
                });

                app = builder.Build();

                int p = port == null || port == 0 ? 8888 : (int)port;
                string i = ip == null ? "localhost" : ip;
                // 指定服务访问地址
                app.Urls.Add($"http://{i}:{p}");

                app.UseSwagger();
                app.UseSwaggerUI();


                // 使用控制器
                app.MapControllers();

                log.Info("》》》》》服务启动完成 🚀🚀🚀🚀🚀🚀🚀🚀 》》》》》");

                app.Run();
         

            });


        }


        public void StopServer()
        {
            app.StopAsync();
        }


    }
}
