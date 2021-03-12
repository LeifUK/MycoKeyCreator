using System;
using System.Collections.Generic;

namespace MycoKeys.Library.Database
{
    public interface ISpeciesSizeAttributeValueTable : ITable<DBObject.SpeciesSizeAttributeValue>
    {
        IEnumerable<DBObject.SpeciesSizeAttributeValue> GetEnumeratorForKey(Int64 key_id);
        IEnumerable<DBObject.SpeciesSizeAttributeValue> GetEnumeratorForSpecies(Int64 species_id);
        void DeleteByKey(Int64 key_id);
        void DeleteBySpecies(Int64 species_id);
    }
}
