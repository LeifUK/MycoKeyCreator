namespace MycoKeys.Library.PetaPocoAdapter
{
    public class SQLiterDatabaseFactory
    {
        private static void OpenDatabase(out PetaPoco.NetCore.Database database, string path)
        {
            database = new PetaPoco.NetCore.Database("Data Source=" + path + ";Version=3;", "System.Data.SQLite");
            database.OpenSharedConnection();
        }

        public static void OpenDatabase(out Database.IDatabase iDatabase, string path)
        {
            OpenDatabase(out PetaPoco.NetCore.Database database, path);
            iDatabase = new DatabaseWrapper(database);
            iDatabase.ISqlQueryBuilder = new SqlQueryBuilders.SQLiteQueryBuilder();
        }

        public static void CreateDatabase(out MycoKeys.Library.Database.IDatabase iDatabase, string folder, string dbName)
        {
            string path = System.IO.Path.Combine(folder, dbName + ".sqlite");
            OpenDatabase(out iDatabase, path);
            PetaPoco.NetCore.Database database = (iDatabase as DatabaseWrapper).Database;
            SqlQueryBuilders.ISqlQueryBuilder iSqlQueryBuilder = (iDatabase as DatabaseWrapper).ISqlQueryBuilder;

            // Create each table

            iDatabase.CreateTables();
        }
    }
}
