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
           using var db = DbClientFactory.GetSqlSugarClient();
            var tables = db.DbMaintenance.GetTableInfoList(false);
            if(tables.Any(x => x.Name.Equals("Printer") || x.Name.Equals("LabelFileInfo")))
            {
                log.Warn(LogHelper.WPF_SHOW_START + "数据库表已存在不用初始化");
                return;
            }
            db.CodeFirst.InitTables(typeof(Printer), typeof(LabelFileInfo));
        }

        public LabelFileInfo SelectByLabelName(string name)
        {
            using var db = DbClientFactory.GetSqlSugarClient();
            return  db.Queryable<LabelFileInfo>().Where( x => x.name.Equals(name)).Single();
        }

        public Printer SelectByPrinterName(string name)
        {
            using var db = DbClientFactory.GetSqlSugarClient();
            return db.Queryable<Printer>().Where( x => x.name.Equals(name)).Single();
        }

        public List<LabelFileInfo> SelectLabelFileList()
        {
            using var db = DbClientFactory.GetSqlSugarClient();
            return db.Queryable<LabelFileInfo>().ToList();
        }


        public void DelLabelFile(int id)
        {
            using var db = DbClientFactory.GetSqlSugarClient(); 
            db.Deleteable<LabelFileInfo>().Where(x => id == x.id).ExecuteCommand();
        }

        public void InsertLabelFile(LabelFileInfo labelFileInfo)
        {
            using var db = DbClientFactory.GetSqlSugarClient();
            db.Insertable<LabelFileInfo> (labelFileInfo).ExecuteCommand();
        }

        public List<Printer> SelectPrinterList()
        {
            using var db = DbClientFactory.GetSqlSugarClient();
            return db.Queryable<Printer>().ToList();
        }

        public void DelPrinter(int id)
        {
            using var db = DbClientFactory.GetSqlSugarClient();
            db.Deleteable<Printer>().Where(x => id == x.id).ExecuteCommand();
        }
        public void InsertPrinter(Printer printer)
        {
            using var db = DbClientFactory.GetSqlSugarClient();
            db.Insertable<Printer>(printer).ExecuteCommand();
        }

        public void UpdatePrinter(Printer printer)
        {
            using var db = DbClientFactory.GetSqlSugarClient();
            db.Updateable<Printer>(printer).ExecuteCommand();
        }


    }
}
