namespace MycoKeys.Library.PetaPocoAdapter
{
    public class SpeciesTable : Table<DBObject.Species>, Database.ISpeciesTable
    {
        public SpeciesTable(SqlQueryBuilders.ISqlQueryBuilder iSqlQueryBuilder, PetaPoco.NetCore.Database database) : base(iSqlQueryBuilder, database, Database.TableNames.Species)
        {

        }
    }
}
