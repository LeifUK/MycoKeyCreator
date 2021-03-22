using System.Data.SqlClient;

namespace MycoKeys.Library.PetaPocoAdapter
{
    public class SQLServerDatabaseFactory
    {
        private static string MakeConnectionString(bool useDataSource, string dataSource, string host, int port, bool useWindowsAuthentication, string userName, string password, string dbName)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            if (useDataSource)
            {
                sqlConnectionStringBuilder.DataSource = dataSource;
            }
            else
            {
                sqlConnectionStringBuilder.DataSource = host + "," + port;
            }

            if (useWindowsAuthentication)
            {
                sqlConnectionStringBuilder.IntegratedSecurity = true;
            }
            else
            {
                sqlConnectionStringBuilder.UserID = userName;
                sqlConnectionStringBuilder.Password = password;
            }

            sqlConnectionStringBuilder.InitialCatalog = dbName;
            return sqlConnectionStringBuilder.ToString();
        }

        private static void OpenDatabase(out PetaPoco.NetCore.Database database, bool useDataSource, string dataSource, string host, int port, bool useWindowsAuthentication, string userName, string password, string dbName)
        {
            string connectionString = MakeConnectionString(useDataSource, dataSource, host, port, useWindowsAuthentication, userName, password, dbName);
            database = new PetaPoco.NetCore.Database(connectionString, "System.Data.SqlClient");
            database.OpenSharedConnection();
        }

        public static void OpenDatabase(out MycoKeys.Library.Database.IDatabase iDatabase, bool useDataSource, string dataSource, string host, int port, bool useWindowsAuthentication, string userName, string password, string dbName)
        {
            OpenDatabase(out PetaPoco.NetCore.Database database, useDataSource, dataSource, host, port, useWindowsAuthentication, userName, password, dbName);
            iDatabase = new DatabaseWrapper(database);
            iDatabase.ISqlQueryBuilder = new SqlQueryBuilders.SqlServerQueryBuilder();
        }

        public static void CreateDatabase(out MycoKeys.Library.Database.IDatabase iDatabase, string dataSource, bool useWindowsAuthentication, string userName, string password, string folder, string dbName)
        {
            // Connect to the master DB to create the requested database

            OpenDatabase(out PetaPoco.NetCore.Database database, true, dataSource, null, -1, useWindowsAuthentication, userName, password, "master");

            SqlQueryBuilders.SqlServerQueryBuilder iSqlQueryBuilder = new SqlQueryBuilders.SqlServerQueryBuilder();
            database.Execute(iSqlQueryBuilder.CreateDatabase(folder, dbName));
            database.CloseSharedConnection();

            // Connect to the new database

            OpenDatabase(out database, true, dataSource, null, -1, useWindowsAuthentication, userName, password, dbName);

            iDatabase = new DatabaseWrapper(database);
            iDatabase.ISqlQueryBuilder = iSqlQueryBuilder;

            // Create each table

            iDatabase.CreateTables();
        }
    }
}
