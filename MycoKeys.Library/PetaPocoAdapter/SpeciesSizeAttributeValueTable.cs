using System;
using System.Collections.Generic;

namespace MycoKeys.Library.PetaPocoAdapter
{
    public class SpeciesSizeAttributeValueTable : Table<DBObject.SpeciesSizeAttributeValue>, Database.ISpeciesSizeAttributeValueTable
    {
        public SpeciesSizeAttributeValueTable(SqlQueryBuilders.ISqlQueryBuilder iSqlQueryBuilder, PetaPoco.NetCore.Database database) : base(iSqlQueryBuilder, database, Database.TableNames.SpeciesSizeAttributeValue)
        {

        }
        
        public IEnumerable<DBObject.SpeciesSizeAttributeValue> GetEnumeratorForSpecies(Int64 species_id)
        {
            return _database.Query<DBObject.SpeciesSizeAttributeValue>(_iSqlQueryBuilder.SelectByColumn(_tableName, "species_id"), species_id);
        }

        public void DeleteByKey(Int64 key_id)
        {
            _database.Delete<DBObject.SpeciesSizeAttributeValue>("WHERE key_id=@0", key_id);
        }

        public void DeleteBySpecies(Int64 species_id)
        {
            _database.Delete<DBObject.SpeciesSizeAttributeValue>("WHERE species_id=@0", species_id);
        }
    }
}
