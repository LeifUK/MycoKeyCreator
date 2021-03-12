using System;
using System.Collections.Generic;

namespace MycoKeys.Library.Database
{
    public interface IKeyManager
    {
        IEnumerable<DBObject.Key> GetKeyEnumerator();
        void Insert(DBObject.Key key);
        void Update(DBObject.Key key);
        bool Delete(DBObject.Key key);

        IEnumerable<DBObject.Literature> GetLiteratureEnumerator();
        IEnumerable<DBObject.Literature> GetKeyLiteratureEnumerator(Int64 key_id);
        void Insert(DBObject.Literature literature);
        void Update(DBObject.Literature literature);
        bool Delete(DBObject.Literature literature);

        Library.DBObject.SpeciesAttributeChoice Select(Library.DBObject.SpeciesAttributeChoice speciesAttributeChoice);
        IEnumerable<DBObject.SpeciesAttributeChoice> GetKeySpeciesAttributeChoiceEnumerator(Int64 key_id);
        IEnumerable<DBObject.SpeciesAttributeChoice> GetSpeciesAttributeChoiceEnumerator(Int64 species_id);
        bool Insert(Library.DBObject.SpeciesAttributeChoice speciesAttributeChoice);
        bool Update(Library.DBObject.SpeciesAttributeChoice speciesAttributeChoice);
        bool Delete(Library.DBObject.SpeciesAttributeChoice speciesAttributeChoice);

        IEnumerable<DBObject.SpeciesAttributeSize> GetSpeciesSizeAttributeEnumerator(Int64 species_id);
        bool Insert(Library.DBObject.SpeciesAttributeSize speciesAttributeSize);
        bool Update(Library.DBObject.SpeciesAttributeSize speciesAttributeSize);
        bool Delete(Library.DBObject.SpeciesAttributeSize speciesAttributeSize);

        IEnumerable<DBObject.Species> GetKeySpeciesEnumerator(Int64 key_id);
        bool Insert(DBObject.Attribute attribute);
        bool Insert(DBObject.Attribute attribute, List<DBObject.AttributeChoice> attributeChoices);
        bool Update(DBObject.Attribute attribute, List<DBObject.AttributeChoice> attributeChoices);
        bool Update(DBObject.Attribute attribute);
        bool Delete(DBObject.Attribute attribute);
        IEnumerable<DBObject.Attribute> GetKeyAttributeEnumerator(Int64 key_id);
        bool Insert(DBObject.AttributeChoice attributeChoice);
        bool Update(DBObject.AttributeChoice attributeChoice);
        bool Delete(DBObject.AttributeChoice attributeChoice);
        IEnumerable<DBObject.AttributeChoice> GetKeyAttributeChoiceEnumerator(Int64 key_id);
        IEnumerable<DBObject.AttributeChoice> GetAttributeChoiceEnumerator(Int64 attribute_id);
        bool Insert(DBObject.Species species);
        bool Update(DBObject.Species species);
        bool Delete(DBObject.Species species);
    }
}
