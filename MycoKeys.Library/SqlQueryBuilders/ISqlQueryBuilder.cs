namespace MycoKeys.Library.SqlQueryBuilders
{
    public interface ISqlQueryBuilder
    {
        string CreateDatabase(string folder, string dbName);
        string CreateKeyTable();
        string CreateAttributeTable();
        string CreateSpeciesTable();
        string CreateSpeciesAttributeTable();
        string SelectByKey(string tableName);
        string SelectByColumn(string tableName, string columnName);
    }
}
