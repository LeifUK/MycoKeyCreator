using System.Text;

namespace MycoKeyCreator.Library.SqlQueryBuilders
{
    public class SQLiteQueryBuilder : SQLQueryBuilder
    {
        public override string CreateDatabase(string folder, string dbName)
        {
            return (@"CREATE DATABASE " + dbName);
        }

        public override void AppendIdentityColumn(StringBuilder stringBuilder, string columnName)
        {
            stringBuilder.Append(columnName);
            stringBuilder.Append(" INTEGER NOT NULL PRIMARY KEY");
        }
    }
}
