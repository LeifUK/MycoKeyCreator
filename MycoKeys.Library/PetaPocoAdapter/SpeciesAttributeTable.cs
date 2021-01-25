using System;
using System.Collections.Generic;

namespace MycoKeys.Library.PetaPocoAdapter
{
    public class SpeciesAttributeTable : Table<DBObject.SpeciesAttribute>, Database.ISpeciesAttributeTable
    {
        public SpeciesAttributeTable(SqlQueryBuilders.ISqlQueryBuilder iSqlQueryBuilder, PetaPoco.NetCore.Database database) : base(iSqlQueryBuilder, database, Database.TableNames.SpeciesAttribute)
        {

        }
        
        public IEnumerable<DBObject.SpeciesAttribute> GetEnumeratorForSpecies(Int64 species_id)
        {
            return _database.Query<DBObject.SpeciesAttribute>(_iSqlQueryBuilder.SelectByColumn(_tableName, "species_id"), species_id);
        }

        public void DeleteBySpecies(Int64 species_id)
        {
            _database.Delete<DBObject.SpeciesAttribute>("WHERE species_id=@0", species_id);
        }

        public void DeleteByAttribute(Int64 attribute_id)
        {
            _database.Delete<DBObject.SpeciesAttribute>("WHERE attribute_id=@0", attribute_id);
        }
    }
}
