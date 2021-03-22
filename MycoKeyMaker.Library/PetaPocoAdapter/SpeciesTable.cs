using System;

namespace MycoKeys.Library.PetaPocoAdapter
{
    public class SpeciesTable : Table<DBObject.Species>, Database.ISpeciesTable
    {
        public SpeciesTable(SqlQueryBuilders.ISqlQueryBuilder iSqlQueryBuilder, PetaPoco.NetCore.Database database) : base(iSqlQueryBuilder, database, Database.TableNames.Species)
        {

        }

        public void DeleteByKey(Int64 key_id)
        {
            _database.Delete<DBObject.Species>("WHERE key_id=@0", key_id);
        }
    }
}
