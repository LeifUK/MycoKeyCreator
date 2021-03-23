using System;
using System.Collections.Generic;

namespace FungiKeys.Library.PetaPocoAdapter
{
    // Warning warning
    public class QueryManager : Database.IQueryManager
    {
        public QueryManager(PetaPoco.NetCore.Database database)
        {
            _database = database;
        }

        protected readonly PetaPoco.NetCore.Database _database;
    }
}
