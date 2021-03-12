using System.Text;
using System.Data.SqlClient;

namespace MycoKeys.Library.PetaPocoAdapter
{
    public class MySQLDatabaseFactory
    {
        private static string MakeConnectionString(string server, int port, bool useWindowsAuthentication, string userName, string password, string dbName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Server=");
            stringBuilder.Append("localhost");
            if (useWindowsAuthentication)
            {
                stringBuilder.Append("; IntegratedSecurity=true");
            }
            else
            {
                stringBuilder.Append("; Uid=");
                stringBuilder.Append(userName);
                stringBuilder.Append("; Pwd=");
                stringBuilder.Append(password);
            }
            stringBuilder.Append("; Database=");
            stringBuilder.Append(dbName);
            stringBuilder.Append("; Port= ");
            stringBuilder.Append(port);

            return stringBuilder.ToString();
        }

        private static void OpenDatabase(out PetaPoco.NetCore.Database database, string server, int port, bool useWindowsAuthentication, string userName, string password, string dbName)
        {
            string connectionString = MakeConnectionString(server, port, useWindowsAuthentication, userName, password, dbName);
            database = new PetaPoco.NetCore.Database(connectionString, "MySql.Data.MySqlClient");
            database.OpenSharedConnection();
        }

        public static void OpenDatabase(out MycoKeys.Library.Database.IDatabase iDatabase, string server, int port, bool useWindowsAuthentication, string userName, string password, string dbName)
        {
            OpenDatabase(out PetaPoco.NetCore.Database database, server, port, useWindowsAuthentication, userName, password, dbName);
            iDatabase = new DatabaseWrapper(database);
            iDatabase.ISqlQueryBuilder = new SqlQueryBuilders.MySqlQueryBuilder();
        }

        public static void CreateDatabase(out MycoKeys.Library.Database.IDatabase iDatabase, string server, int port, bool useWindowsAuthentication, string userName, string password, string dbName)
        {
            // Connect to the master DB to create the requested database

            OpenDatabase(out PetaPoco.NetCore.Database database, server, port, useWindowsAuthentication, userName, password, "MySql");

            SqlQueryBuilders.MySqlQueryBuilder iSqlQueryBuilder = new SqlQueryBuilders.MySqlQueryBuilder();
            database.Execute(@"CREATE DATABASE " + dbName);
            database.CloseSharedConnection();

            // Connect to the new database

            OpenDatabase(out database, server, port, useWindowsAuthentication, userName, password, dbName);

            iDatabase = new DatabaseWrapper(database);
            iDatabase.ISqlQueryBuilder = iSqlQueryBuilder;

            // Create each table

            iDatabase.CreateTables();
        }
    }
}
