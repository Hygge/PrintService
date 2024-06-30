using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PrintService.Domain;
using PrintService.Log;
using PrintService.Utils;
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
            int number = dTO.count == null? 1 : (int)dTO.count;
            // 解析参数 1.无数据打印 2.有参打印
            Dictionary<string, object> data = dTO.param == null ? null : JsonConvert.DeserializeObject<Dictionary<string, object>>(dTO.param);

            // 根据标签名称查询模板和打印机并打印
            LabelFileInfo labelFileInfo =  App.printBll.SelectByLabelName(dTO.labelName);
            Printer printer =  App.printBll.SelectByPrinterName(dTO.printName);
            if (labelFileInfo == null) 
            {
                return new Result(1, "打印失败，标签模板不存在");
            }
            if (printer == null) 
            {
                return new Result(1, "打印失败，标签模板不存在");
            }
            log.Info($"{LogHelper.WPF_SHOW_START}正在打印 {labelFileInfo.name}标签，使用 {printer.name}打印机，填充参数：{dTO.param}");
            try
            {
                PrintUtil.FastReportPrintLabelObj(printer.address, labelFileInfo.path, data, number);
                log.Info($"{LogHelper.WPF_SHOW_START}打印成功 {labelFileInfo.name}标签，使用 {printer.name}打印机，填充参数：{dTO.param}");
            }catch(Exception e)
            {
                PrintUtil.FastReportPrintLabelObj(printer.address, labelFileInfo.path, data, number);
                log.Info($"{LogHelper.WPF_SHOW_START}打印失败 {labelFileInfo.name}标签，使用 {printer.name}打印机，填充参数：{dTO.param}");
            }


            return new Result(0, "测试请求");
        }



    }

    /// <summary>
    /// 请求对象
    /// </summary>
    /// <param name="printName">打印机名称</param>
    /// <param name="param">模板变量数据(json字符串)</param>
    /// <param name="count">打印次数</param>
    public record PrintDataDTO(string labelName, string printName, string? param, int? count);

    public record Result(int code,  string? message, object data = null);

}
