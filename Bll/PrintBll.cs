using log4net;
using PrintService.Db;
using PrintService.Domain;
using PrintService.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintService.Bll
{
    public class PrintBll
    {
        private readonly ILog log = LogManager.GetLogger(typeof(PrintBll));


        public void InitTable()
        {
            var db = DbClientFactory.GetSqlSugarClient();
            var tables = db.DbMaintenance.GetTableInfoList(false);
            if(tables.Any(x => x.Name.Equals("Printer") || x.Name.Equals("LabelFileInfo")))
            {
                log.Warn(LogHelper.WPF_SHOW_START + "数据库表已存在不用初始化");
                return;
            }
            db.CodeFirst.InitTables(typeof(Printer), typeof(LabelFileInfo));
        }


        public List<LabelFileInfo> SelectLabelFileList()
        {
           return DbClientFactory.GetSqlSugarClient().Queryable<LabelFileInfo>().ToList();
        }


        public void DelLabelFile(int id)
        {
            DbClientFactory.GetSqlSugarClient().Deleteable<LabelFileInfo>().Where(x => id == x.id).ExecuteCommand();
        }

        public void InsertLabelFile(LabelFileInfo labelFileInfo)
        {
            DbClientFactory.GetSqlSugarClient().Insertable<LabelFileInfo> (labelFileInfo).ExecuteCommand();
        }

        public List<Printer> SelectPrinterList()
        {
            return DbClientFactory.GetSqlSugarClient().Queryable<Printer>().ToList();
        }

        public void DelPrinter(int id)
        {
            DbClientFactory.GetSqlSugarClient().Deleteable<Printer>().Where(x => id == x.id).ExecuteCommand();
        }
        public void InsertPrinter(Printer printer)
        {
            DbClientFactory.GetSqlSugarClient().Insertable<Printer>(printer).ExecuteCommand();
        }

        public void UpdatePrinter(Printer printer)
        {
            DbClientFactory.GetSqlSugarClient().Updateable<Printer>(printer).ExecuteCommand();
        }


    }
}
