using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintService.Domain
{
    public class Printer
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int id { set;get; }
        /// <summary>
        /// 打印机名称
        /// </summary>
        public string name { set;get; }
        /// <summary>
        /// 描述
        /// </summary>
        public string description { set;get; }
        /// <summary>
        /// 打印机地址
        /// </summary>
        public string address { set;get; }


    }
}
