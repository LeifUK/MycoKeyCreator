using System;

namespace MycoKeys.Library.PetaPocoAdapter
{
    public class LiteratureTable : Table<DBObject.Literature>, Database.ILiteratureTable
    {
        public LiteratureTable(SqlQueryBuilders.ISqlQueryBuilder iSqlQueryBuilder, PetaPoco.NetCore.Database database) : base(iSqlQueryBuilder, database, Database.TableNames.Literature)
        {
        }

        public void DeleteByKey(Int64 key_id)
        {
            _database.Delete<DBObject.Literature>("WHERE key_id=@0", key_id);
        }
    }
}
