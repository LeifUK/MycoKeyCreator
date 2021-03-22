using System;
using System.Collections.Generic;

namespace MycoKeyMaker.Library.PetaPocoAdapter
{
    public class SpeciesAttributeSizeTable : Table<DBObject.SpeciesAttributeSize>, Database.ISpeciesAttributeSizeTable
    {
        public SpeciesAttributeSizeTable(SqlQueryBuilders.ISqlQueryBuilder iSqlQueryBuilder, PetaPoco.NetCore.Database database) : base(iSqlQueryBuilder, database, Database.TableNames.SpeciesAttributeSize)
        {

        }
        
        public IEnumerable<DBObject.SpeciesAttributeSize> GetEnumeratorForSpecies(Int64 species_id)
        {
            return _database.Query<DBObject.SpeciesAttributeSize>(_iSqlQueryBuilder.SelectByColumn(_tableName, "species_id"), species_id);
        }

        public void DeleteByKey(Int64 key_id)
        {
            _database.Delete<DBObject.SpeciesAttributeSize>("WHERE key_id=@0", key_id);
        }

        public void DeleteBySpecies(Int64 species_id)
        {
            _database.Delete<DBObject.SpeciesAttributeSize>("WHERE species_id=@0", species_id);
        }
        
        public void DeleteByAttribute(Int64 attribute_id)
        {
            _database.Delete<DBObject.SpeciesAttributeSize>("WHERE attribute_id=@0", attribute_id);
        }

    }
}
