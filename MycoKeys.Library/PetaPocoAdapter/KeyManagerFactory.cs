using System;

namespace MycoKeys.Library.PetaPocoAdapter
{
    public class KeyManagerFactory
    {
        public static MycoKeys.Library.Database.KeyManager BuildKeyManager(Database.IDatabase iDatabase)
        {
            System.Diagnostics.Trace.Assert(iDatabase is DatabaseWrapper);
            var database = (iDatabase as DatabaseWrapper).Database;
            MycoKeys.Library.Database.KeyManager keyManager = new MycoKeys.Library.Database.KeyManager(
                iDatabase,
                new PetaPocoAdapter.KeyTable(iDatabase.ISqlQueryBuilder, database),
                new PetaPocoAdapter.LiteratureTable(iDatabase.ISqlQueryBuilder, database),
                new PetaPocoAdapter.SpeciesTable(iDatabase.ISqlQueryBuilder, database),
                new PetaPocoAdapter.AttributeTable(iDatabase.ISqlQueryBuilder, database),
                new PetaPocoAdapter.AttributeValueTable(iDatabase.ISqlQueryBuilder, database),
                new PetaPocoAdapter.SpeciesAttributeValueTable(iDatabase.ISqlQueryBuilder, database),
                new PetaPocoAdapter.SpeciesSizeAttributeValueTable(iDatabase.ISqlQueryBuilder, database));

            return keyManager;
        }
    }
}
