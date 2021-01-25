namespace MycoKeys.Library.PetaPocoAdapter
{
    public class AttributeTable : Table<DBObject.Attribute>, Database.IAttributeTable
    {
        public AttributeTable(SqlQueryBuilders.ISqlQueryBuilder iSqlQueryBuilder, PetaPoco.NetCore.Database database) : base(iSqlQueryBuilder, database, Database.TableNames.Attribute)
        {
        }
    }
}
