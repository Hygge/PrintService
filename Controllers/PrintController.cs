using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PrintService.Domain;
using PrintService.Log;
using PrintService.Utils;
using System.IO;
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
        public Result labelFileList()
        {            
            return new Result(0, "ok!", App.printBll.SelectLabelFileList());

        }
       [HttpGet]
          public Result printerList()
          {            
              return new Result(0, "ok!", App.printBll.SelectPrinterList());
          }
          [HttpDelete]
          public Result delPrinter(int id)
          {
              App.printBll.DelPrinter(id);
              return new Result(0, "ok!");
          }
          [HttpDelete]
          public Result delLabel(int id)
          {
              App.printBll.DelLabelFile(id);
              return new Result(0, "ok!");
          }

          [HttpPost]
          public Result addPrinter(string name, string desc, string address)
          {
              if(string.IsNullOrEmpty(name))
              {
                  return new Result(1, "打印名称不能为空");
              }
              Printer p =  App.printBll.SelectByPrinterName(name);
              if(p != null)
              {
                  return new Result(1, "打印名称已存在!");
              }
              Printer printer = new Printer();
              printer.name = name;    
              printer.address = address;
              printer.description = desc;
              App.printBll.InsertPrinter(printer);
              log.Info($"{LogHelper.WPF_SHOW_START}添加新的打印机 {name} 成功，地址{address}");

              return new Result(0, "ok!");
          }


        [HttpPost]
        public Result addLabelFileInfo([FromForm] string name, [FromForm] string desc, [FromForm]IFormFile file)
        {
            if(string.IsNullOrEmpty(name))
            {
                return new Result(1, "标签名称不能为空");
            }
            if(file == null)
            {
                return new Result(1, "标签模板文件不能为空");
            }
            string[] strings = file.FileName.Split('.');
            if ( strings.Length < 2 || !strings[1].Equals("frx"))
            {
                return new Result(1, "标签模板文件类型错误");
            }

            LabelFileInfo labelFileInfo =  App.printBll.SelectByLabelName(name);
            if(labelFileInfo != null)
            {
                return new Result(1, "标签名称已存在!");
            }
            string filepath = Path.Combine(Environment.CurrentDirectory, App.configuration["labelDir"], file.FileName);
            using Stream stream = file.OpenReadStream();
            using FileStream fileStream = new FileStream(filepath, FileMode.Create);
            stream.CopyToAsync(fileStream);

            LabelFileInfo l = new LabelFileInfo();
            l.name = name;    
            l.description = desc;
            l.path = filepath;
            l.url = Path.Combine(App.configuration["labelDir"], file.FileName);
            App.printBll.InsertLabelFile(l);
            log.Info($"{LogHelper.WPF_SHOW_START}添加新的标签模板 {name} 成功，地址{l.url}");

            return new Result(0, "ok!");
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
                
                log.Info($"{LogHelper.WPF_SHOW_START}打印失败 {labelFileInfo.name}标签，使用 {printer.name}打印机，填充参数：{dTO.param}, 异常原因：{e.Message}");
                return new Result(1, "打印失败");
            }


            return new Result(0, "ok!");
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
