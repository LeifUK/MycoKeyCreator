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
        Library.DBObject.SpeciesAttribute Select(Library.DBObject.SpeciesAttribute speciesDescription);
        bool Insert(Library.DBObject.SpeciesAttribute speciesAttribute);
        bool Update(Library.DBObject.SpeciesAttribute speciesAttribute);
        bool Delete(Library.DBObject.SpeciesAttribute speciesAttribute);
        IEnumerable<DBObject.SpeciesAttribute> GetSpeciesAttributeEnumerator(Int64 species_id);
        IEnumerable<DBObject.Species> GetKeySpeciesEnumerator(Int64 key_id);
        bool Insert(DBObject.Attribute attribute);
        bool Update(DBObject.Attribute attribute);
        bool Delete(DBObject.Attribute attribute);
        IEnumerable<DBObject.Attribute> GetKeyAttributeEnumerator(Int64 key_id);
        bool Insert(DBObject.Species species);
        bool Update(DBObject.Species species);
        bool Delete(DBObject.Species species);
    }
}
