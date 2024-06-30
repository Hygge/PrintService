using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintService.Domain
{
    public class LabelFileInfo
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int id { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string name { get; set; }

        public string description { get; set; }
        /// <summary>
        /// 存储地址
        /// </summary>
        public string path { get; set; }
        public string url { get; set; }


    }
}
