using System.Text;

namespace MycoKeyCreator.Library.SqlQueryBuilders
{
    public abstract class SQLQueryBuilder : ISqlQueryBuilder
    {
        public abstract void AppendIdentityColumn(StringBuilder stringBuilder, string columnName);

        public void AppendIntegerColumn(StringBuilder stringBuilder, string columnName)
        {
            stringBuilder.Append(columnName);
            stringBuilder.Append(" INTEGER");
        }

        public void AppendSmallIntColumn(StringBuilder stringBuilder, string columnName)
        {
            stringBuilder.Append(columnName);
            stringBuilder.Append(" SMALLINT");
        }

        public void AppendTinyIntColumn(StringBuilder stringBuilder, string columnName)
        {
            stringBuilder.Append(columnName);
            stringBuilder.Append(" TINYINT");
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
            stringBuilder.Append(", ");
            AppendSmallIntColumn(stringBuilder, "flags");
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "notes", 1000);
            stringBuilder.Append(");");
            return stringBuilder.ToString();
        }

        public string CreateLiteratureTable()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("CREATE TABLE ");
            stringBuilder.Append(Database.TableNames.Literature);
            stringBuilder.Append(" (");
            AppendIdentityColumn(stringBuilder, "id");
            stringBuilder.Append(", ");
            AppendIntegerColumn(stringBuilder, "key_id");
            stringBuilder.Append(", ");
            AppendStringColumnNotNull(stringBuilder, "title", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "description", 1000);
            stringBuilder.Append(", ");
            AppendSmallIntColumn(stringBuilder, "position");
            stringBuilder.Append(", ");
            AppendForeignKeyConstraint(stringBuilder, "FK_literature_key_id", "key_id", Database.TableNames.Key, "id");
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
            AppendStringColumnNull(stringBuilder, "description", 500);
            stringBuilder.Append(", ");
            AppendSmallIntColumn(stringBuilder, "position");
            stringBuilder.Append(", ");
            AppendSmallIntColumn(stringBuilder, "type");
            stringBuilder.Append(", ");
            AppendForeignKeyConstraint(stringBuilder, "FK_attribute_key_id", "key_id", Database.TableNames.Key, "id");
            stringBuilder.Append(");");
            return stringBuilder.ToString();
        }

        public string CreateAttributeChoiceTable()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("CREATE TABLE ");
            stringBuilder.Append(Database.TableNames.AttributeChoice);
            stringBuilder.Append(" (");
            AppendIdentityColumn(stringBuilder, "id");
            stringBuilder.Append(", ");
            AppendIntegerColumn(stringBuilder, "key_id");
            stringBuilder.Append(", ");
            AppendIntegerColumn(stringBuilder, "attribute_id");
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "description", 500);
            stringBuilder.Append(", ");
            AppendSmallIntColumn(stringBuilder, "position");
            stringBuilder.Append(", ");
            AppendForeignKeyConstraint(stringBuilder, "FK_attributevalue_key_id", "key_id", Database.TableNames.Key, "id");
            stringBuilder.Append(", ");
            AppendForeignKeyConstraint(stringBuilder, "FK_attributevalue_attribute_id", "attribute_id", Database.TableNames.Attribute, "id");
            stringBuilder.Append(");");
            return stringBuilder.ToString();
        }

        public string CreateSpeciesAttributeValueTable()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("CREATE TABLE ");
            stringBuilder.Append(Database.TableNames.SpeciesAttributeChoice);
            stringBuilder.Append(" (");
            AppendIdentityColumn(stringBuilder, "id");
            stringBuilder.Append(", ");
            AppendIntegerColumn(stringBuilder, "key_id");
            stringBuilder.Append(", ");
            AppendIntegerColumn(stringBuilder, "species_id");
            stringBuilder.Append(", ");
            AppendIntegerColumn(stringBuilder, "attributechoice_id");
            stringBuilder.Append(", ");
            AppendForeignKeyConstraint(stringBuilder, "FK_speciesattributechoice_key_id", "key_id", Database.TableNames.Key, "id");
            stringBuilder.Append(", ");
            AppendForeignKeyConstraint(stringBuilder, "FK_speciesattributechoice_species_id", "species_id", Database.TableNames.Species, "id");
            stringBuilder.Append(", ");
            AppendForeignKeyConstraint(stringBuilder, "FK_speciesattributechoice_attributechoice_id", "attributechoice_id", Database.TableNames.AttributeChoice, "id");
            stringBuilder.Append(");");
            return stringBuilder.ToString();
        }

        public string CreateSpeciesSizeAttributeValueTable()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("CREATE TABLE ");
            stringBuilder.Append(Database.TableNames.SpeciesAttributeSize);
            stringBuilder.Append(" (");
            AppendIdentityColumn(stringBuilder, "id");
            stringBuilder.Append(", ");
            AppendIntegerColumn(stringBuilder, "key_id");
            stringBuilder.Append(", ");
            AppendIntegerColumn(stringBuilder, "species_id");
            stringBuilder.Append(", ");
            AppendIntegerColumn(stringBuilder, "attribute_id");
            stringBuilder.Append(", ");
            AppendSmallIntColumn(stringBuilder, "value");
            stringBuilder.Append(", ");
            AppendForeignKeyConstraint(stringBuilder, "FK_speciessizeattributevalue_key_id", "key_id", Database.TableNames.Key, "id");
            stringBuilder.Append(", ");
            AppendForeignKeyConstraint(stringBuilder, "FK_speciessizeattributevalue_species_id", "species_id", Database.TableNames.Species, "id");
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
            AppendStringColumnNotNull(stringBuilder, "name", 500);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "qualified_name", 500);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "synonyms", 500);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "common_name", 200);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "fruiting_body", 1000);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "cap", 500);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "hymenium", 500);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "gills", 500);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "pores", 500);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "spines", 500);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "stem", 500);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "flesh", 500);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "smell", 500);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "taste", 500);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "season", 500);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "distribution", 500);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "habitat", 500);
            stringBuilder.Append(", ");
            AppendStringColumnNull(stringBuilder, "spore_print", 500);
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
