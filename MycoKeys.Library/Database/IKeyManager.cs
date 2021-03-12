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

        Library.DBObject.SpeciesAttributeValue Select(Library.DBObject.SpeciesAttributeValue speciesDescription);
        IEnumerable<DBObject.SpeciesAttributeValue> GetKeySpeciesAttributeValueEnumerator(Int64 key_id);
        IEnumerable<DBObject.SpeciesAttributeValue> GetSpeciesAttributeValueEnumerator(Int64 species_id);
        bool Insert(Library.DBObject.SpeciesAttributeValue speciesAttributeValue);
        bool Update(Library.DBObject.SpeciesAttributeValue speciesAttributeValue);
        bool Delete(Library.DBObject.SpeciesAttributeValue speciesAttributeValue);
        IEnumerable<DBObject.SpeciesAttributeValue> GetSpeciesAttributeEnumerator(Int64 species_id);

        bool Insert(Library.DBObject.SpeciesSizeAttributeValue speciesSizeAttributeValue);
        bool Update(Library.DBObject.SpeciesSizeAttributeValue speciesSizeAttributeValue);
        bool Delete(Library.DBObject.SpeciesSizeAttributeValue speciesSizeAttributeValue);
        IEnumerable<DBObject.SpeciesSizeAttributeValue> GetSpeciesSizeAttributeValueEnumerator(Int64 species_id);

        IEnumerable<DBObject.Species> GetKeySpeciesEnumerator(Int64 key_id);
        bool Insert(DBObject.Attribute attribute);
        bool Insert(DBObject.Attribute attribute, List<DBObject.AttributeValue> attributeValues);
        bool Update(DBObject.Attribute attribute, List<DBObject.AttributeValue> attributeValues);
        bool Update(DBObject.Attribute attribute);
        bool Delete(DBObject.Attribute attribute);
        IEnumerable<DBObject.Attribute> GetKeyAttributeEnumerator(Int64 key_id);
        bool Insert(DBObject.AttributeValue attributeValue);
        bool Update(DBObject.AttributeValue attributeValue);
        bool Delete(DBObject.AttributeValue attributeValue);
        IEnumerable<DBObject.AttributeValue> GetKeyAttributeValueEnumerator(Int64 key_id);
        IEnumerable<DBObject.AttributeValue> GetAttributeValueEnumerator(Int64 attribute_id);
        bool Insert(DBObject.Species species);
        bool Update(DBObject.Species species);
        bool Delete(DBObject.Species species);
    }
}
