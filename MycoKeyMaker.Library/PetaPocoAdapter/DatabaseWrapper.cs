namespace MycoKeyMaker.Library.PetaPocoAdapter
{
    public class DatabaseWrapper : MycoKeyMaker.Library.Database.IDatabase
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

        public void CreateLiteratureTable()
        {
            Database.Execute(ISqlQueryBuilder.CreateLiteratureTable());
        }

        public void CreateSpeciesSizeAttributeValueTable()
        {
            Database.Execute(ISqlQueryBuilder.CreateSpeciesSizeAttributeValueTable());
        }

        public void CreateTables()
        {
            Database.Execute(ISqlQueryBuilder.CreateKeyTable());
            Database.Execute(ISqlQueryBuilder.CreateAttributeTable());
            Database.Execute(ISqlQueryBuilder.CreateAttributeChoiceTable());
            Database.Execute(ISqlQueryBuilder.CreateSpeciesTable());
            Database.Execute(ISqlQueryBuilder.CreateSpeciesAttributeValueTable());
            Database.Execute(ISqlQueryBuilder.CreateSpeciesSizeAttributeValueTable());
        }
    }
}
