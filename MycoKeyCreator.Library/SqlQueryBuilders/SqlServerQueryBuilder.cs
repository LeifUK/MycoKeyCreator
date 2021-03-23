using System.Text;

namespace MycoKeyCreator.Library.SqlQueryBuilders
{
    public class SqlServerQueryBuilder : SQLQueryBuilder
    {
        public override string CreateDatabase(string folder, string dbName)
        {
            string filename = System.IO.Path.Combine(folder, dbName);
            return "CREATE DATABASE " + dbName + " ON PRIMARY (Name=" + dbName + ", filename = \"" + filename + ".mdf\") LOG ON (name=" + dbName + "_log, filename=\"" + filename + ".ldf\")";
        }

        public override void AppendIdentityColumn(StringBuilder stringBuilder, string columnName)
        {
            stringBuilder.Append(columnName);
            stringBuilder.Append(" INTEGER NOT NULL IDENTITY PRIMARY KEY");
        }
    }
}
