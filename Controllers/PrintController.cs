using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintService.Controllers
{

    [Route("[controller]/[action]")]
    [ApiController]
    public class PrintController : ControllerBase
    {
        public readonly ILogger<PrintController> logger;

        public PrintController(ILogger<PrintController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public string test()
        {

            return "测试请求";

        }

        [HttpPost]
        public string printLabel([FromBody] PrintDataDTO dTO)
        {

            return $"{dTO}";
        }



    }

    /// <summary>
    /// 请求对象
    /// </summary>
    /// <param name="printName">打印机名称</param>
    /// <param name="param">模板变量数据(json字符串)</param>
    public record PrintDataDTO(string printName, string? param);

}
