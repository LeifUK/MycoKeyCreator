using System;
using System.Collections.Generic;

namespace MycoKeys.Library.Database
{
    public interface IKeyManager
    {
        void Insert(DBObject.Key key);
        void Update(DBObject.Key key);
        bool Delete(DBObject.Key key);
        IEnumerable<DBObject.Key> GetKeyEnumerator();
        Library.DBObject.SpeciesAttributeValue Select(Library.DBObject.SpeciesAttributeValue speciesDescription);
        bool Insert(Library.DBObject.SpeciesAttributeValue speciesAttribute);
        bool Update(Library.DBObject.SpeciesAttributeValue speciesAttribute);
        bool Delete(Library.DBObject.SpeciesAttributeValue speciesAttribute);
        IEnumerable<DBObject.SpeciesAttributeValue> GetSpeciesAttributeEnumerator(Int64 species_id);
        IEnumerable<DBObject.Species> GetKeySpeciesEnumerator(Int64 key_id);
        bool Insert(DBObject.Attribute attribute, List<DBObject.AttributeValue> attributeValues);
        bool Update(DBObject.Attribute attribute, List<DBObject.AttributeValue> attributeValues);
        bool Update(DBObject.Attribute attribute);
        bool Delete(DBObject.Attribute attribute);
        bool Delete(DBObject.AttributeValue attributeValue);
        IEnumerable<DBObject.Attribute> GetKeyAttributeEnumerator(Int64 key_id);
        IEnumerable<DBObject.AttributeValue> GetAttributeValueEnumerator(Int64 attribute_id);
        bool Insert(DBObject.Species species);
        bool Update(DBObject.Species species);
        bool Delete(DBObject.Species species);
    }
}
