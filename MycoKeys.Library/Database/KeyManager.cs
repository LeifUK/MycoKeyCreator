using System;
using System.Collections.Generic;

namespace MycoKeys.Library.Database
{
    public class KeyManager : IKeyManager
    {
        public KeyManager(
            Database.IDatabase iDatabase,
            Database.IKeyTable iKeyTable,
            Database.ISpeciesTable iSpeciesTable,
            Database.IAttributeTable iAttributeTable,
            Database.ISpeciesAttributeTable iSpeciesAttributeTable)
        {
            _database = iDatabase;
            _iKeyTable = iKeyTable;
            _iSpeciesTable = iSpeciesTable;
            _iAttributeTable = iAttributeTable;
            _iSpeciesAttributeTable = iSpeciesAttributeTable;
        }

        private readonly Database.IDatabase _database;
        private readonly Database.IKeyTable _iKeyTable;
        private readonly Database.ISpeciesTable _iSpeciesTable;
        private readonly Database.IAttributeTable _iAttributeTable;
        private readonly Database.ISpeciesAttributeTable _iSpeciesAttributeTable;
        
        public void Insert(DBObject.Key key)
        {
            _iKeyTable.Insert(key);
        }

        public void Update(DBObject.Key key)
        {
            _iKeyTable.Update(key);
        }

        public bool Delete(DBObject.Key key)
        {
            bool success = true;
            try
            {
                _database.BeginTransaction();
                _iSpeciesAttributeTable.DeleteByKey(key.id);
                _iSpeciesTable.DeleteByKey(key.id);
                _iAttributeTable.DeleteByKey(key.id);
                _iKeyTable.Delete(key);
                _database.CommitTransaction();
            }
            // Warning warning => error logging
            catch
            {
                _database.RollbackTransaction();
                success = false;
            }

            return success;
        }

        public IEnumerable<DBObject.Key> GetKeyEnumerator()
        {
            return _iKeyTable.Enumerator;
        }

        public Library.DBObject.SpeciesAttribute Select(Library.DBObject.SpeciesAttribute speciesAttribute)
        {
            if (speciesAttribute.id == 0)
            {
                return null;
            }

            Library.DBObject.SpeciesAttribute newSpeciesAttribute = null;
            try
            {
                newSpeciesAttribute = _iSpeciesAttributeTable.Query(speciesAttribute.id);
            }
            catch
            {
            }

            return newSpeciesAttribute;
        }

        public bool Insert(Library.DBObject.SpeciesAttribute speciesAttribute)
        {
            bool success = true;
            try
            {
                _iSpeciesAttributeTable.Insert(speciesAttribute);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public bool Update(Library.DBObject.SpeciesAttribute speciesAttribute)
        {
            bool success = true;
            try
            {
                _iSpeciesAttributeTable.Update(speciesAttribute);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public bool Delete(Library.DBObject.SpeciesAttribute speciesAttribute)
        {
            bool success = true;
            try
            {
                _iSpeciesAttributeTable.Delete(speciesAttribute);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public IEnumerable<DBObject.SpeciesAttribute> GetSpeciesAttributeEnumerator(Int64 species_id)
        {
            return _iSpeciesAttributeTable.GetEnumeratorForSpecies(species_id);
        }

        public IEnumerable<DBObject.Species> GetKeySpeciesEnumerator(Int64 key_id)
        {
            return _iSpeciesTable.GetEnumeratorForKey(key_id);
        }

        public bool Insert(DBObject.Attribute attribute)
        {
            bool success = true;
            try
            {
                _iAttributeTable.Insert(attribute);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public bool Update(DBObject.Attribute attribute)
        {
            bool success = true;
            try
            {
                _iAttributeTable.Update(attribute);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public bool Delete(DBObject.Attribute attribute)
        {
            bool success = true;
            try
            {
                _iSpeciesAttributeTable.DeleteByAttribute(attribute.id);
                _iAttributeTable.Delete(attribute);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public IEnumerable<DBObject.Attribute> GetKeyAttributeEnumerator(Int64 key_id)
        {
            return _iAttributeTable.GetEnumeratorForKey(key_id);
        }

        public bool Insert(DBObject.Species species)
        {
            bool success = true;
            try
            {
                _iSpeciesTable.Insert(species);
            }
            catch 
            {
                success = false;
            }

            return success;
        }

        public bool Update(DBObject.Species species)
        {
            bool success = true;
            try
            {
                _iSpeciesTable.Update(species);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public bool Delete(DBObject.Species species)
        {
            bool success = true;
            try
            {
                _iSpeciesAttributeTable.DeleteBySpecies(species.id);
                _iSpeciesTable.Delete(species);
            }
            catch
            {
                success = false;
            }

            return success;
        }
    }
}
