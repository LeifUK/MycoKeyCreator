namespace MycoKeyCreator.WebApplication.Services
{
    public class KeyManagerFactory : IKeyManagerFactory
    {
        public KeyManagerFactory()
        {

        }

        private static Library.Database.IDatabase OpenConnection()
        {
            MycoKeyCreator.Library.PetaPocoAdapter.SQLServerDatabaseFactory.OpenDatabase(
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

        public MycoKeyCreator.Library.Database.IKeyManager GetKeyManager()
        {
            return MycoKeyCreator.Library.PetaPocoAdapter.KeyManagerFactory.BuildKeyManager(OpenConnection());
        }
    }
}
