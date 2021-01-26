using System;
using System.Collections.Generic;

namespace MycoKeys.Library.Database
{
    public interface ISpeciesAttributeTable : ITable<DBObject.SpeciesAttribute>
    {
        IEnumerable<DBObject.SpeciesAttribute> GetEnumeratorForKey(Int64 key_id);
        IEnumerable<DBObject.SpeciesAttribute> GetEnumeratorForSpecies(Int64 species_id);
        void DeleteByKey(Int64 key_id);
        void DeleteBySpecies(Int64 species_id);
        void DeleteByAttribute(Int64 attribute_id);
    }
}
