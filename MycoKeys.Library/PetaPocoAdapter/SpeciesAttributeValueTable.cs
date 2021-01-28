using System;
using System.Collections.Generic;

namespace MycoKeys.Library.PetaPocoAdapter
{
    public class SpeciesAttributeValueTable : Table<DBObject.SpeciesAttributeValue>, Database.ISpeciesAttributeValueTable
    {
        public SpeciesAttributeValueTable(SqlQueryBuilders.ISqlQueryBuilder iSqlQueryBuilder, PetaPoco.NetCore.Database database) : base(iSqlQueryBuilder, database, Database.TableNames.SpeciesAttributeValue)
        {

        }
        
        public IEnumerable<DBObject.SpeciesAttributeValue> GetEnumeratorForSpecies(Int64 species_id)
        {
            return _database.Query<DBObject.SpeciesAttributeValue>(_iSqlQueryBuilder.SelectByColumn(_tableName, "species_id"), species_id);
        }

        public void DeleteByKey(Int64 key_id)
        {
            _database.Delete<DBObject.SpeciesAttributeValue>("WHERE key_id=@0", key_id);
        }

        public void DeleteBySpecies(Int64 species_id)
        {
            _database.Delete<DBObject.SpeciesAttributeValue>("WHERE species_id=@0", species_id);
        }

        public void DeleteByAttributeValue(Int64 attributevalue_id)
        {
            _database.Delete<DBObject.SpeciesAttributeValue>("WHERE attributevalue_id=@0", attributevalue_id);
        }
    }
}
