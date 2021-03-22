
using System.Text;

namespace MycoKeyMaker.Library.SqlQueryBuilders
{
    public class MySqlQueryBuilder : SQLQueryBuilder
    {
        public override string CreateDatabase(string folder, string dbName)
        {
            return (@"CREATE DATABASE " + dbName);
        }

        public override void AppendIdentityColumn(StringBuilder stringBuilder, string columnName)
        {
            stringBuilder.Append(columnName);
            stringBuilder.Append(" INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY");
        }
    }
}
