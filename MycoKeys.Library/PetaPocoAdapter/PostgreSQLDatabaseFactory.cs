namespace MycoKeys.Library.PetaPocoAdapter
{
    public class PostgreSQLServerDatabaseFactory
    {
        private static string MakeConnectionString(string host, int port, bool useWindowsAuthentication, string userName, string password, string dbName)
        {
            Npgsql.NpgsqlConnectionStringBuilder npgsqlConnectionStringBuilder = new Npgsql.NpgsqlConnectionStringBuilder();
            npgsqlConnectionStringBuilder.Host = host;
            npgsqlConnectionStringBuilder.Port = port;
            npgsqlConnectionStringBuilder.IntegratedSecurity = useWindowsAuthentication;
            npgsqlConnectionStringBuilder.Username = userName;
            npgsqlConnectionStringBuilder.Password = password;
            npgsqlConnectionStringBuilder.Database = dbName;
            return npgsqlConnectionStringBuilder.ToString();
        }

        public static void OpenDatabase(out PetaPoco.NetCore.Database database, string host, int port, bool useWindowsAuthentication, string userName, string password, string dbName)
        {
            string connectionString = MakeConnectionString(host, port, useWindowsAuthentication, userName, password, dbName);
            database = new PetaPoco.NetCore.Database(connectionString, "Npgsql");
            database.OpenSharedConnection();
        }

        public static void OpenDatabase(out MycoKeys.Library.Database.IDatabase iDatabase, string host, int port, bool useWindowsAuthentication, string userName, string password, string dbName)
        {
            OpenDatabase(out PetaPoco.NetCore.Database database, host, port, useWindowsAuthentication, userName, password, dbName);
            iDatabase = new DatabaseWrapper(database);
            iDatabase.ISqlQueryBuilder = new SqlQueryBuilders.PostgreSQLQueryBuilder();
        }

        public static void CreateDatabase(out MycoKeys.Library.Database.IDatabase iDatabase, string host, int port, bool useWindowsAuthentication, string userName, string password, string dbName)
        {
            // Connect to the master DB to create the requested database

            OpenDatabase(out PetaPoco.NetCore.Database database, host, port, useWindowsAuthentication, userName, password, "postgres");

            SqlQueryBuilders.ISqlQueryBuilder iSqlQueryBuilder = new SqlQueryBuilders.PostgreSQLQueryBuilder();
            database.Execute(iSqlQueryBuilder.CreateDatabase(null, dbName));
            database.CloseSharedConnection();

            // Connect to the new database

            OpenDatabase(out database, host, port, useWindowsAuthentication, userName, password, dbName);

            iDatabase = new DatabaseWrapper(database);
            iDatabase.ISqlQueryBuilder = iSqlQueryBuilder;

            // Create each table

            iDatabase.CreateTables();
        }
    }
}
