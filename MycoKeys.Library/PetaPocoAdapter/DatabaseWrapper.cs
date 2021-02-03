namespace MycoKeys.Library.PetaPocoAdapter
{
    public class DatabaseWrapper : MycoKeys.Library.Database.IDatabase
    {
        public DatabaseWrapper(PetaPoco.NetCore.Database database)
        {
            Database = database;
        }

        public SqlQueryBuilders.ISqlQueryBuilder ISqlQueryBuilder { get; set; }

        public readonly PetaPoco.NetCore.Database Database;

        public string Name
        {
            get
            {
                return Database.Connection.Database;
            }
        }

        public void CloseConnection()
        {
            Database.CloseSharedConnection();
        }

        public void BeginTransaction()
        {
            Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            Database.CompleteTransaction();
        }

        public void RollbackTransaction()
        {
            Database.AbortTransaction();
        }
    }
}
