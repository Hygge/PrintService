using SqlSugar;
using System.IO;


namespace PrintService.Db
{
    public class DbClientFactory
    {

        public static readonly string ConnectionStringSettings = Path.Combine(Environment.CurrentDirectory, "printService.db");

      

        public static ISqlSugarClient GetSqlSugarClient()
        {
            return new SqlSugarClient(new ConnectionConfig()
            {
                DbType = DbType.Sqlite,
                ConnectionString = ConnectionStringSettings,
                IsAutoCloseConnection = true,
            }); 
        }
    }
}
