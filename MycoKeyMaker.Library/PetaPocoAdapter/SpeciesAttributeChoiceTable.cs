using System;
using System.Collections.Generic;

namespace MycoKeyMaker.Library.PetaPocoAdapter
{
    public class SpeciesAttributeChoiceTable : Table<DBObject.SpeciesAttributeChoice>, Database.ISpeciesAttributeChoiceTable
    {
        public SpeciesAttributeChoiceTable(SqlQueryBuilders.ISqlQueryBuilder iSqlQueryBuilder, PetaPoco.NetCore.Database database) : base(iSqlQueryBuilder, database, Database.TableNames.SpeciesAttributeChoice)
        {

        }
        
        public IEnumerable<DBObject.SpeciesAttributeChoice> GetEnumeratorForSpecies(Int64 species_id)
        {
            return _database.Query<DBObject.SpeciesAttributeChoice>(_iSqlQueryBuilder.SelectByColumn(_tableName, "species_id"), species_id);
        }

        public IEnumerable<DBObject.SpeciesAttributeChoice> GetEnumeratorForAttributeChoice(Int64 attributechoice_id)
        {
            return _database.Query<DBObject.SpeciesAttributeChoice>(_iSqlQueryBuilder.SelectByColumn(_tableName, "attributechoice_id"), attributechoice_id);
        }

        public void DeleteByKey(Int64 key_id)
        {
            _database.Delete<DBObject.SpeciesAttributeChoice>("WHERE key_id=@0", key_id);
        }

        public void DeleteBySpecies(Int64 species_id)
        {
            _database.Delete<DBObject.SpeciesAttributeChoice>("WHERE species_id=@0", species_id);
        }

        public void DeleteByAttributeChoice(Int64 attributechoice_id)
        {
            _database.Delete<DBObject.SpeciesAttributeChoice>("WHERE attributechoice_id=@0", attributechoice_id);
        }
    }
}
