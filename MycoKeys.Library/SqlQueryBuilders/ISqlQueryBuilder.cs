namespace MycoKeys.Library.SqlQueryBuilders
{
    public interface ISqlQueryBuilder
    {
        string CreateDatabase(string folder, string dbName);
        string CreateKeyTable();
        string CreateLiteratureTable();
        string CreateAttributeTable();
        string CreateAttributeValueTable();
        string CreateSpeciesTable();
        string CreateSpeciesAttributeValueTable();
        string SelectByKey(string tableName);
        string SelectByColumn(string tableName, string columnName);
    }
}
