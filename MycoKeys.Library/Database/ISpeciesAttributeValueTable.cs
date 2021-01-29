using System;
using System.Collections.Generic;

namespace MycoKeys.Library.Database
{
    public interface ISpeciesAttributeValueTable : ITable<DBObject.SpeciesAttributeValue>
    {
        IEnumerable<DBObject.SpeciesAttributeValue> GetEnumeratorForKey(Int64 key_id);
        IEnumerable<DBObject.SpeciesAttributeValue> GetEnumeratorForSpecies(Int64 species_id);
        IEnumerable<DBObject.SpeciesAttributeValue> GetEnumeratorForAttributeValue(Int64 attributevalue_id);
        void DeleteByKey(Int64 key_id);
        void DeleteBySpecies(Int64 species_id);
        void DeleteByAttributeValue(Int64 attributevalue_id);
    }
}
