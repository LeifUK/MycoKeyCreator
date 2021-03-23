namespace MycoKeyCreator.Library.PetaPocoAdapter
{
    public class KeyTable : Table<DBObject.Key>, Database.IKeyTable
    {
        public KeyTable(SqlQueryBuilders.ISqlQueryBuilder iSqlQueryBuilder, PetaPoco.NetCore.Database database) : base(iSqlQueryBuilder, database, Database.TableNames.Key)
        {
        }
    }
}
