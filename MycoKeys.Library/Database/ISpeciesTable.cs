using System;
using System.Collections.Generic;

namespace MycoKeys.Library.Database
{
    public interface ISpeciesTable : ITable<DBObject.Species>
    {
        IEnumerable<DBObject.Species> GetEnumeratorForKey(Int64 key_id);
    }
}
