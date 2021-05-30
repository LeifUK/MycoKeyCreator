namespace MycoKeyCreator.WebApplication.Services
{
    public class KeyManagerFactory : IKeyManagerFactory
    {
        public KeyManagerFactory()
        {

        }

        private static Library.Database.IDatabase OpenConnection()
        {
#if DEBUG
            MycoKeyCreator.Library.PetaPocoAdapter.SQLServerDatabaseFactory.OpenDatabase(
                 out Library.Database.IDatabase iDatabase, 
                 true,
                "DESKTOP-6RPCGOV\\SQLEXPRESS",
                "127.0.0.1",
                1433,
                true,
                null,
                null,
                "LeifGoodwin_MycologyKeys");
#else
            MycoKeyCreator.Library.PetaPocoAdapter.SQLServerDatabaseFactory.OpenDatabase(
                 out Library.Database.IDatabase iDatabase, 
                true,
                "mssql4.websitelive.net",
                null,
                0,
                false,
                "LeifGoodwin_Admin", 
                "$WebWiz1625",
                "LeifGoodwin_MycologyKeys");
#endif        
            return iDatabase;
        }

        public MycoKeyCreator.Library.Database.IKeyManager GetKeyManager()
        {
            return MycoKeyCreator.Library.PetaPocoAdapter.KeyManagerFactory.BuildKeyManager(OpenConnection());
        }
    }
}
