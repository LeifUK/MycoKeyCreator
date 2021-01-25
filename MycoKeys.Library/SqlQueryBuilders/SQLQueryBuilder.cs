using System.Text;

namespace MycoKeys.Library.SqlQueryBuilders
{
    public abstract class SQLQueryBuilder : ISqlQueryBuilder
    {
        public abstract void AppendIdentityColumn(StringBuilder stringBuilder, string columnName);

        public void AppendIntegerColumn(StringBuilder stringBuilder, string columnName)
        {
            stringBuilder.Append(columnName);
            stringBuilder.Append(" INTEGER");
        }

        public void AppendStringColumn(StringBuilder stringBuilder, string columnName, int sizeInBytes, bool notNull)
        {
            stringBuilder.Append(columnName);
            stringBuilder.Append(" VARCHAR(");
            stringBuilder.Append(sizeInBytes);
            stringBuilder.Append(")");
            if (notNull)
            {
                stringBuilder.Append(" NOT NULL");
            }
        }

        public void AppendStringColumnNull(StringBuilder stringBuilder, string columnName, int sizeInBytes)
        {
            AppendStringColumn(stringBuilder, columnName, sizeInBytes, false);
        }

        public void AppendStringColumnNotNull(StringBuilder stringBuilder, string columnName, int sizeInBytes)
        {
            AppendStringColumn(stringBuilder, columnName, sizeInBytes, true);
        }

        public void AppendForeignKeyConstraint(StringBuilder stringBuilder, string constraintName, string columnName, string foreignTableName, string foreignColumnName)
        {
            stringBuilder.Append("CONSTRAINT ");
            stringBuilder.Append(constraintName);
            stringBuilder.Append(" FOREIGN KEY (");
            stringBuilder.Append(columnName);
            stringBuilder.Append(") REFERENCES ");
            stringBuilder.Append(foreignTableName);
            stringBuilder.Append("(");
            stringBuilder.Append(foreignColumnName);
            stringBuilder.Append(")");
        }
        
        #region ISqlQueryBuilder

        public abstract string CreateDatabase(string folder, string dbName);

        public string CreateKeyTable()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("CREATE TABLE ");
            stringBuilder.Append(Database.TableNames.Key);
            stringBuilder.Append(" (");
            AppendIdentityColumn(stringBuilder, "id");
            stringBuilder.Append(", ");
            AppendStringColumnNotNull(stringBuilder, "name", 100);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "title", 200);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "description", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "copyright", 200);
            stringBuilder.Append(");");
            return stringBuilder.ToString();
        }

        public string CreateAttributeTable()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("CREATE TABLE ");
            stringBuilder.Append(Database.TableNames.Attribute);
            stringBuilder.Append(" (");
            AppendIdentityColumn(stringBuilder, "id");
            stringBuilder.Append(", ");
            AppendIntegerColumn(stringBuilder, "key_id");
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "description", 8000);
            stringBuilder.Append(", ");
            AppendForeignKeyConstraint(stringBuilder, "FK_attribute_key_id", "key_id", Database.TableNames.Key, "id");
            stringBuilder.Append(");");
            return stringBuilder.ToString();
        }

        public string CreateSpeciesAttributeTable()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("CREATE TABLE ");
            stringBuilder.Append(Database.TableNames.SpeciesAttribute);
            stringBuilder.Append(" (");
            AppendIdentityColumn(stringBuilder, "id");
            stringBuilder.Append(", ");
            AppendIntegerColumn(stringBuilder, "key_id");
            stringBuilder.Append(", ");
            AppendIntegerColumn(stringBuilder, "species_id");
            stringBuilder.Append(", ");
            AppendIntegerColumn(stringBuilder, "attribute_id");
            stringBuilder.Append(", ");
            AppendForeignKeyConstraint(stringBuilder, "FK_speciesattribute_key_id", "key_id", Database.TableNames.Key, "id");
            stringBuilder.Append(", ");
            AppendForeignKeyConstraint(stringBuilder, "FK_speciesattribute_species_id", "species_id", Database.TableNames.Species, "id");
            stringBuilder.Append(", ");
            AppendForeignKeyConstraint(stringBuilder, "FK_speciesattribute_attribute_id", "attribute_id", Database.TableNames.Attribute, "id");
            stringBuilder.Append(");");
            return stringBuilder.ToString();
        }

        public string CreateSpeciesTable()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("CREATE TABLE ");
            stringBuilder.Append(Database.TableNames.Species);
            stringBuilder.Append(" (");
            AppendIdentityColumn(stringBuilder, "id");
            stringBuilder.Append(", ");
            AppendIntegerColumn(stringBuilder, "key_id");
            stringBuilder.Append(", ");
            AppendStringColumnNotNull(stringBuilder, "name", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "qualified_name", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "synonyms", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "common_name", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "fruiting_body", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "cap", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "hymenium", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "gills", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "pores", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "spines", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "stem", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "flesh", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "smell", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "taste", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "season", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "distribution", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "habitat", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "spore_print", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "microscopic_features", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "edibility", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "notes", 1000);
            stringBuilder.Append(", ");
            AppendForeignKeyConstraint(stringBuilder, "FK_species_key_id", "key_id", Database.TableNames.Key, "id");
            stringBuilder.Append(");");
            return stringBuilder.ToString();
        }

        public string SelectByColumn(string tableName, string columnName)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("SELECT * FROM ");
            stringBuilder.Append(tableName);
            stringBuilder.Append(" WHERE ");
            stringBuilder.Append(columnName);
            stringBuilder.Append("=@0;");
            return stringBuilder.ToString();
        }

        public string SelectByKey(string tableName)
        {
            return SelectByColumn(tableName, "key_id");
        }

        #endregion
    }
}
