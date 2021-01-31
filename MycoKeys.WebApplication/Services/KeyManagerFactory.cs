namespace MycoKeys.WebApplication.Services
{
    public class KeyManagerFactory : IKeyManagerFactory
    {
        public KeyManagerFactory()
        {

        }

        private static Library.Database.IDatabase OpenConnection()
        {
            MycoKeys.Library.PetaPocoAdapter.SQLServerDatabaseFactory.OpenDatabase(
                out Library.Database.IDatabase iDatabase,
                true,
                "DESKTOP-6RPCGOV\\SQLEXPRESS",
                "127.0.0.1",
                1433,
                true,
                null,
                null,
                "MycoKeys3");
            return iDatabase;
        }

        public MycoKeys.Library.Database.IKeyManager GetKeyManager()
        {
            return MycoKeys.Library.PetaPocoAdapter.KeyManagerFactory.BuildKeyManager(OpenConnection());
        }
    }
}
