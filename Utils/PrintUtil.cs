using FastReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintService.Utils
{
    /// <summary>
    /// 打印工具类
    /// </summary>
    public class PrintUtil
    {


        /// <summary>
        /// 使用FastReport库根据标签模板进行打印——简单数据类型obj
        /// </summary>
        /// <param name="printerName">打印机名称（全地址）</param>
        /// <param name="lablePath">模板名称（全路径名）</param>
        /// <param name="data">模板所需参数</param>
        /// <param name="number">打印次数</param>
        public static void FastReportPrintLabelObj(string printerName, string lablePath, Dictionary<string, object> data, int number = 1)
        {
            try
            {
                using(Report report = new Report())
                {
                    report.Load(lablePath);
                    report.PrintSettings.PageNumbers = "1";
                    report.PrintSettings.ShowDialog = false;
                    if(data != null)
                    {
                        foreach (var key in data.Keys)
                        {
                            report.SetParameterValue(key, data[key]);
                        }
                    }
                    for (int i = 0; i < number; i++)
                    {
                        report.PrintSettings.Printer = printerName;
                        report.Print();
                    }
                }
            }catch (NullReferenceException ex){
                throw new Exception("数据与模板变量不符");

            }catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }


    }
}
