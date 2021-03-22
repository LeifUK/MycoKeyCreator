using System;
using System.Collections.Generic;

namespace MycoKeys.Library.Database
{
    public interface ISpeciesAttributeChoiceTable : ITable<DBObject.SpeciesAttributeChoice>
    {
        IEnumerable<DBObject.SpeciesAttributeChoice> GetEnumeratorForKey(Int64 key_id);
        IEnumerable<DBObject.SpeciesAttributeChoice> GetEnumeratorForSpecies(Int64 species_id);
        IEnumerable<DBObject.SpeciesAttributeChoice> GetEnumeratorForAttributeChoice(Int64 attributechoice_id);
        void DeleteByKey(Int64 key_id);
        void DeleteBySpecies(Int64 species_id);
        void DeleteByAttributeChoice(Int64 attributechoice_id);
    }
}
