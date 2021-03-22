namespace MycoKeyMaker.WebApplication.Services
{
    public class KeyManagerFactory : IKeyManagerFactory
    {
        public KeyManagerFactory()
        {

        }

        private static Library.Database.IDatabase OpenConnection()
        {
            MycoKeyMaker.Library.PetaPocoAdapter.SQLServerDatabaseFactory.OpenDatabase(
                out Library.Database.IDatabase iDatabase,
                true,
                "DESKTOP-6RPCGOV\\SQLEXPRESS",
                "127.0.0.1",
                1433,
                true,
                null,
                null,
                "MycoKeys");
            return iDatabase;
        }

        public MycoKeyMaker.Library.Database.IKeyManager GetKeyManager()
        {
            return MycoKeyMaker.Library.PetaPocoAdapter.KeyManagerFactory.BuildKeyManager(OpenConnection());
        }
    }
}
