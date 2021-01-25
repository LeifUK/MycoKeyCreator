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
    }
}
