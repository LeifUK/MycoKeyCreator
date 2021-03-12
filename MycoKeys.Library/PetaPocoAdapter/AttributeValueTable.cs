using System;
using System.Collections.Generic;

namespace MycoKeys.Library.PetaPocoAdapter
{
    public class AttributeValueTable : Table<DBObject.AttributeChoice>, Database.IAttributeChoiceTable
    {
        public AttributeValueTable(SqlQueryBuilders.ISqlQueryBuilder iSqlQueryBuilder, PetaPoco.NetCore.Database database) : base(iSqlQueryBuilder, database, Database.TableNames.AttributeChoice)
        {
        }
        
        public IEnumerable<DBObject.AttributeChoice> GetEnumeratorForAttribute(Int64 attribute_id)
        {
            return GetEnumeratorForColumn("attribute_id", attribute_id);
        }

        public void DeleteByKey(Int64 key_id)
        {
            _database.Delete<DBObject.AttributeChoice>("WHERE key_id=@0", key_id);
        }
    }
}
