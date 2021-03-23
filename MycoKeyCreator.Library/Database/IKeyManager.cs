using System;
using System.Collections.Generic;

namespace MycoKeyCreator.Library.Database
{
    public interface IKeyManager
    {
        IEnumerable<DBObject.Key> GetKeyEnumerator();
        void Insert(DBObject.Key key);
        void Update(DBObject.Key key);
        bool Delete(DBObject.Key key);

        IEnumerable<DBObject.Literature> GetLiteratureEnumerator();
        IEnumerable<DBObject.Literature> GetLiteratureEnumeratorForKey(Int64 key_id);
        void Insert(DBObject.Literature literature);
        void Update(DBObject.Literature literature);
        bool Delete(DBObject.Literature literature);

        Library.DBObject.SpeciesAttributeChoice Select(Library.DBObject.SpeciesAttributeChoice speciesAttributeChoice);
        IEnumerable<DBObject.SpeciesAttributeChoice> GetSpeciesAttributeChoiceEnumeratorForKey(Int64 key_id);
        IEnumerable<DBObject.SpeciesAttributeChoice> GetSpeciesAttributeChoiceEnumeratorForSpecies(Int64 species_id);
        bool Insert(Library.DBObject.SpeciesAttributeChoice speciesAttributeChoice);
        bool Update(Library.DBObject.SpeciesAttributeChoice speciesAttributeChoice);
        bool Delete(Library.DBObject.SpeciesAttributeChoice speciesAttributeChoice);

        IEnumerable<DBObject.SpeciesAttributeSize> GetSpeciesSizeAttributeEnumeratorForSpecies(Int64 species_id);
        bool Insert(Library.DBObject.SpeciesAttributeSize speciesAttributeSize);
        bool Update(Library.DBObject.SpeciesAttributeSize speciesAttributeSize);
        bool Delete(Library.DBObject.SpeciesAttributeSize speciesAttributeSize);

        IEnumerable<DBObject.Attribute> GetAttributeEnumeratorForKey(Int64 key_id);
        bool Insert(DBObject.Attribute attribute);
        bool Insert(DBObject.Attribute attribute, List<DBObject.AttributeChoice> attributeChoices);
        bool Update(DBObject.Attribute attribute, List<DBObject.AttributeChoice> attributeChoices);
        bool Update(DBObject.Attribute attribute);
        bool Delete(DBObject.Attribute attribute);

        IEnumerable<DBObject.AttributeChoice> GetAttributeChoiceEnumeratorForKey(Int64 key_id);
        IEnumerable<DBObject.AttributeChoice> GetAttributeChoiceEnumeratorForAttribute(Int64 attribute_id);
        bool Insert(DBObject.AttributeChoice attributeChoice);
        bool Update(DBObject.AttributeChoice attributeChoice);
        bool Delete(DBObject.AttributeChoice attributeChoice);

        IEnumerable<DBObject.Species> GetKeySpeciesEnumerator(Int64 key_id);
        IEnumerable<Library.DBObject.Species> GetSpeciesEnumeratorForAttributeChoice(Int64 key_id, Int64 attributeChoiceId);
        bool Insert(DBObject.Species species);
        bool Update(DBObject.Species species);
        bool Delete(DBObject.Species species);
    }
}
