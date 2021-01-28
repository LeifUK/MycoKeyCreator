using System;
using System.Linq;
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
            Database.IAttributeValueTable iAttributeValueTable,
        Database.ISpeciesAttributeValueTable iSpeciesAttributeTable)
        {
            _iDatabase = iDatabase;
            _iKeyTable = iKeyTable;
            _iSpeciesTable = iSpeciesTable;
            _iAttributeTable = iAttributeTable;
            _iAttributeValueTable = iAttributeValueTable;
            _iSpeciesAttributeValueTable = iSpeciesAttributeTable;
        }

        private readonly Database.IDatabase _iDatabase;
        private readonly Database.IKeyTable _iKeyTable;
        private readonly Database.ISpeciesTable _iSpeciesTable;
        private readonly Database.IAttributeTable _iAttributeTable;
        private readonly Database.IAttributeValueTable _iAttributeValueTable;
        private readonly Database.ISpeciesAttributeValueTable _iSpeciesAttributeValueTable;
        
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
                _iDatabase.BeginTransaction();
                _iSpeciesAttributeValueTable.DeleteByKey(key.id);
                _iSpeciesTable.DeleteByKey(key.id);
                _iAttributeTable.DeleteByKey(key.id);
                _iKeyTable.Delete(key);
                _iDatabase.CommitTransaction();
            }
            // Warning warning => error logging
            catch
            {
                _iDatabase.RollbackTransaction();
                success = false;
            }

            return success;
        }

        public IEnumerable<DBObject.Key> GetKeyEnumerator()
        {
            return _iKeyTable.Enumerator;
        }

        public Library.DBObject.SpeciesAttributeValue Select(Library.DBObject.SpeciesAttributeValue speciesAttribute)
        {
            if (speciesAttribute.id == 0)
            {
                return null;
            }

            Library.DBObject.SpeciesAttributeValue newSpeciesAttribute = null;
            try
            {
                newSpeciesAttribute = _iSpeciesAttributeValueTable.Query(speciesAttribute.id);
            }
            catch
            {
            }

            return newSpeciesAttribute;
        }

        public bool Insert(Library.DBObject.SpeciesAttributeValue speciesAttribute)
        {
            bool success = true;
            try
            {
                _iSpeciesAttributeValueTable.Insert(speciesAttribute);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public bool Update(Library.DBObject.SpeciesAttributeValue speciesAttribute)
        {
            bool success = true;
            try
            {
                _iSpeciesAttributeValueTable.Update(speciesAttribute);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public bool Delete(Library.DBObject.SpeciesAttributeValue speciesAttribute)
        {
            bool success = true;
            try
            {
                _iSpeciesAttributeValueTable.Delete(speciesAttribute);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public IEnumerable<DBObject.SpeciesAttributeValue> GetSpeciesAttributeEnumerator(Int64 species_id)
        {
            return _iSpeciesAttributeValueTable.GetEnumeratorForSpecies(species_id);
        }

        public IEnumerable<DBObject.Species> GetKeySpeciesEnumerator(Int64 key_id)
        {
            return _iSpeciesTable.GetEnumeratorForKey(key_id);
        }

        public IEnumerable<DBObject.AttributeValue> GetAttributeValueEnumerator(Int64 attribute_id)
        {
            return _iAttributeValueTable.GetEnumeratorForAttribute(attribute_id);
        }

        public bool Insert(DBObject.Attribute attribute, List<DBObject.AttributeValue> attributeValues)
        {
            bool success = true;
            try
            {
                _iAttributeTable.Insert(attribute);

                foreach (var attributeValue in attributeValues)
                {
                    attributeValue.key_id = attribute.key_id;
                    attributeValue.attribute_id = attribute.id;
                    _iAttributeValueTable.Insert(attributeValue);
                }
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public bool Update(DBObject.Attribute attribute, List<DBObject.AttributeValue> attributeValues)
        {
            bool success = true;
            try
            {
                _iAttributeTable.Update(attribute);

                Dictionary<Int64, Library.DBObject.AttributeValue> attributeValueMap = _iAttributeValueTable.GetEnumeratorForAttribute(attribute.id).ToDictionary(n => n.id, n => n);
                if (attributeValues != null)
                {
                    foreach (var attributeValue in attributeValues)
                    {
                        attributeValue.key_id = attribute.key_id;
                        attributeValue.attribute_id = attribute.id;
                        if (attributeValue.id == 0)
                        {
                            _iAttributeValueTable.Insert(attributeValue);
                        }
                        else
                        {
                            attributeValueMap.Remove(attributeValue.id);
                            _iAttributeValueTable.Update(attributeValue);
                        }
                    }
                }

                foreach (var attributeValue in attributeValueMap.Values)
                {
                    _iSpeciesAttributeValueTable.DeleteByAttributeValue(attributeValue.id);
                    _iAttributeValueTable.Delete(attributeValue);
                }
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
                var attributeValues = _iAttributeValueTable.GetEnumeratorForAttribute(attribute.id).ToList();
                foreach (var attributeValue in attributeValues)
                {
                    _iSpeciesAttributeValueTable.DeleteByAttributeValue(attributeValue.id);
                    _iAttributeValueTable.Delete(attributeValue);
                }
                _iAttributeTable.Delete(attribute);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public bool Delete(DBObject.AttributeValue attributeValue)
        {
            bool success = true;
            try
            {
                _iSpeciesAttributeValueTable.DeleteByAttributeValue(attributeValue.id);
                _iAttributeValueTable.Delete(attributeValue);
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
                _iSpeciesAttributeValueTable.DeleteBySpecies(species.id);
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
