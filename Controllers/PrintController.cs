using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PrintService.Log;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintService.Controllers
{

    [Route("[controller]/[action]")]
    [ApiController]
    public class PrintController : ControllerBase
    {
        private readonly ILog log = LogManager.GetLogger(typeof(PrintController));

        public PrintController()
        {

        }

        [HttpGet]
        public Result test()
        {
            log.Info(LogHelper.WPF_SHOW_START + "test>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            LogHelper.Info(LogHelper.WPF_SHOW_START + "test>>>>>>>>>>>>>>>>>>>>>>>>>>>1111>>>>>>>>>>>>>>>>>>>");
            return new Result(0, "测试请求");

        }

        [HttpPost]
        public Result printLabel([FromBody] PrintDataDTO dTO)
        {

            return new Result(0, "测试请求");
        }



    }

    /// <summary>
    /// 请求对象
    /// </summary>
    /// <param name="printName">打印机名称</param>
    /// <param name="param">模板变量数据(json字符串)</param>
    /// <param name="count">打印次数</param>
    public record PrintDataDTO(string printName, string? param, int? count);

    public record Result(int code,  string? message, object data = null);

}
