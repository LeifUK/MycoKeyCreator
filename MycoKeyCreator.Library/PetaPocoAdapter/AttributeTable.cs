using System;

namespace MycoKeyCreator.Library.PetaPocoAdapter
{
    public class AttributeTable : Table<DBObject.Attribute>, Database.IAttributeTable
    {
        public AttributeTable(SqlQueryBuilders.ISqlQueryBuilder iSqlQueryBuilder, PetaPoco.NetCore.Database database) : base(iSqlQueryBuilder, database, Database.TableNames.Attribute)
        {
        }

        public void DeleteByKey(Int64 key_id)
        {
            _database.Delete<DBObject.Attribute>("WHERE key_id=@0", key_id);
        }
    }
}
