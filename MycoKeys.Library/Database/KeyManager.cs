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
            Database.ILiteratureTable iLiteratureTable,
            Database.ISpeciesTable iSpeciesTable,
            Database.IAttributeTable iAttributeTable,
            Database.IAttributeValueTable iAttributeValueTable,
            Database.ISpeciesAttributeValueTable iSpeciesAttributeTable,
            Database.ISpeciesSizeAttributeValueTable iSpeciesSizeAttributeValueTable)
        {
            _iDatabase = iDatabase;
            _iKeyTable = iKeyTable;
            _iLiteratureTable = iLiteratureTable;
            _iSpeciesTable = iSpeciesTable;
            _iAttributeTable = iAttributeTable;
            _iAttributeValueTable = iAttributeValueTable;
            _iSpeciesAttributeValueTable = iSpeciesAttributeTable;
            _iSpeciesSizeAttributeValueTable = iSpeciesSizeAttributeValueTable;

            // Upgrades 

            var literatureEnumerator = GetLiteratureEnumerator().GetEnumerator();
            try
            {
                // Throws an exception if the table does not exist
                literatureEnumerator.MoveNext();
            }
            catch
            {
                _iDatabase.CreateLiteratureTable();
            }
            literatureEnumerator.Dispose();

            if (!_iSpeciesSizeAttributeValueTable.Exists())
            {
                _iDatabase.CreateSpeciesSizeAttributeValueTable();
            }
        }

        private readonly Database.IDatabase _iDatabase;
        private readonly Database.IKeyTable _iKeyTable;
        private readonly Database.ILiteratureTable _iLiteratureTable;
        private readonly Database.ISpeciesTable _iSpeciesTable;
        private readonly Database.IAttributeTable _iAttributeTable;
        private readonly Database.IAttributeValueTable _iAttributeValueTable;
        private readonly Database.ISpeciesAttributeValueTable _iSpeciesAttributeValueTable;
        private readonly Database.ISpeciesSizeAttributeValueTable _iSpeciesSizeAttributeValueTable;

        public IEnumerable<DBObject.Key> GetKeyEnumerator()
        {
            return _iKeyTable.Enumerator;
        }

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
                _iAttributeValueTable.DeleteByKey(key.id);
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

        public IEnumerable<DBObject.Literature> GetLiteratureEnumerator()
        {
            return _iLiteratureTable.Enumerator;
        }

        public IEnumerable<DBObject.Literature> GetKeyLiteratureEnumerator(Int64 key_id)
        {
            return _iLiteratureTable.GetEnumeratorForKey(key_id);
        }

        public void Insert(DBObject.Literature literature)
        {
            _iLiteratureTable.Insert(literature);
        }

        public void Update(DBObject.Literature literature)
        {
            _iLiteratureTable.Update(literature);
        }

        public bool Delete(DBObject.Literature literature)
        {
            bool success = true;
            try
            {
                _iLiteratureTable.Delete(literature);
            }
            // Warning warning => error logging
            catch
            {
                success = false;
            }

            return success;
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

        private IEnumerable<DBObject.SpeciesAttributeValue> GetSpeciesAttributeValueEnumerator()
        {
            return _iSpeciesAttributeValueTable.Enumerator;
        }

        public IEnumerable<DBObject.SpeciesAttributeValue> GetSpeciesAttributeEnumerator(Int64 species_id)
        {
            return _iSpeciesAttributeValueTable.GetEnumeratorForSpecies(species_id);
        }

        public IEnumerable<DBObject.SpeciesAttributeValue> GetKeySpeciesAttributeValueEnumerator(Int64 key_id)
        {
            return _iSpeciesAttributeValueTable.GetEnumeratorForKey(key_id);
        }

        public IEnumerable<DBObject.SpeciesAttributeValue> GetSpeciesAttributeValueEnumerator(Int64 species_id)
        {
            return _iSpeciesAttributeValueTable.GetEnumeratorForSpecies(species_id);
        }

        public bool Insert(Library.DBObject.SpeciesSizeAttributeValue speciesSizeAttributeValue)
        {
            bool success = true;
            try
            {
                _iSpeciesSizeAttributeValueTable.Insert(speciesSizeAttributeValue);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public bool Update(Library.DBObject.SpeciesSizeAttributeValue speciesSizeAttributeValue)
        {
            bool success = true;
            try
            {
                _iSpeciesSizeAttributeValueTable.Update(speciesSizeAttributeValue);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public bool Delete(Library.DBObject.SpeciesSizeAttributeValue speciesSizeAttributeValue)
        {
            bool success = true;
            try
            {
                _iSpeciesSizeAttributeValueTable.Delete(speciesSizeAttributeValue);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public IEnumerable<DBObject.SpeciesSizeAttributeValue> GetSpeciesSizeAttributeValueEnumerator(Int64 species_id)
        {
            return _iSpeciesSizeAttributeValueTable.GetEnumeratorForSpecies(species_id);
        }

        private IEnumerable<DBObject.Attribute> GetAttributeEnumerator()
        {
            return _iAttributeTable.Enumerator;
        }

        public IEnumerable<DBObject.Attribute> GetKeyAttributeEnumerator(Int64 key_id)
        {
            return _iAttributeTable.GetEnumeratorForKey(key_id);
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

        public bool Insert(DBObject.Attribute attribute, List<DBObject.AttributeValue> attributeValues)
        {
            bool success = true;
            try
            {
                _iDatabase.BeginTransaction();
                _iAttributeTable.Insert(attribute);

                foreach (var attributeValue in attributeValues)
                {
                    attributeValue.key_id = attribute.key_id;
                    attributeValue.attribute_id = attribute.id;
                    _iAttributeValueTable.Insert(attributeValue);
                }
                _iDatabase.CommitTransaction();
            }
            catch
            {
                success = false;
                _iDatabase.RollbackTransaction();
            }

            return success;
        }

        public bool Update(DBObject.Attribute attribute, List<DBObject.AttributeValue> attributeValues)
        {
            bool success = true;
            try
            {
                _iDatabase.BeginTransaction();
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
                _iDatabase.CommitTransaction();
            }
            catch
            {
                success = false;
                _iDatabase.RollbackTransaction();
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

        public IEnumerable<DBObject.AttributeValue> GetAttributeValueEnumerator()
        {
            return _iAttributeValueTable.Enumerator;
        }

        public IEnumerable<DBObject.AttributeValue> GetAttributeValueEnumerator(Int64 attribute_id)
        {
            return _iAttributeValueTable.GetEnumeratorForAttribute(attribute_id);
        }

        public IEnumerable<DBObject.AttributeValue> GetKeyAttributeValueEnumerator(Int64 key_id)
        {
            return _iAttributeValueTable.GetEnumeratorForKey(key_id);
        }

        public bool Insert(DBObject.AttributeValue attributeValue)
        {
            bool success = true;
            try
            {
                _iAttributeValueTable.Insert(attributeValue);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public bool Update(DBObject.AttributeValue attributeValue)
        {
            bool success = true;
            try
            {
                _iAttributeValueTable.Update(attributeValue);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        // Warning warning => no delete
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

        private IEnumerable<DBObject.Species> GetSpeciesEnumerator()
        {
            return _iSpeciesTable.Enumerator;
        }

        public IEnumerable<DBObject.Species> GetKeySpeciesEnumerator(Int64 key_id)
        {
            return _iSpeciesTable.GetEnumeratorForKey(key_id);
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
                _iDatabase.BeginTransaction();
                _iSpeciesAttributeValueTable.DeleteBySpecies(species.id);
                _iSpeciesTable.Delete(species);
                _iDatabase.CommitTransaction();
            }
            catch
            {
                success = false;
                _iDatabase.RollbackTransaction();
            }

            return success;
        }

        public static void Export(
            IKeyManager iSourceKeyManager,
            IKeyManager iTargetKeyManager)
        {
            KeyManager sourceKeyManager = iSourceKeyManager as KeyManager;
            KeyManager targetKeyManager = iTargetKeyManager as KeyManager;

            Dictionary<long, long> keyIdMap = new Dictionary<long, long>();
            foreach (DBObject.Key key in sourceKeyManager.GetKeyEnumerator())
            {
                Int64 key_id = key.id;
                key.id = 0;
                targetKeyManager.Insert(key);
                keyIdMap.Add(key_id, key.id);
            }

            Dictionary<long, long> speciesIdMap = new Dictionary<long, long>();
            foreach (DBObject.Species species in sourceKeyManager.GetSpeciesEnumerator())
            {
                Int64 species_id = species.id;
                species.id = 0;
                species.key_id = keyIdMap[species.key_id];
                targetKeyManager.Insert(species);
                speciesIdMap.Add(species_id, species.id);
            }

            Dictionary<long, long> attributeIdMap = new Dictionary<long, long>();
            foreach (DBObject.Attribute attribute in sourceKeyManager.GetAttributeEnumerator())
            {
                long attribute_id = attribute.id;
                attribute.id = 0;
                attribute.key_id = keyIdMap[attribute.key_id];
                targetKeyManager.Insert(attribute);
                attributeIdMap.Add(attribute_id, attribute.id);
            }

            Dictionary<long, long> attributeValueIdMap = new Dictionary<long, long>();
            foreach (DBObject.AttributeValue attributeValue in sourceKeyManager.GetAttributeValueEnumerator())
            {
                long attributevalue_id = attributeValue.id;
                attributeValue.id = 0;
                attributeValue.key_id = keyIdMap[attributeValue.key_id];
                attributeValue.attribute_id = attributeIdMap[attributeValue.attribute_id];
                targetKeyManager.Insert(attributeValue);
                attributeValueIdMap.Add(attributevalue_id, attributeValue.id);
            }

            Dictionary<long, long> speciesAttributeValueIdMap = new Dictionary<long, long>();
            foreach (DBObject.SpeciesAttributeValue speciesAttributeValue in sourceKeyManager.GetSpeciesAttributeValueEnumerator())
            {
                long speciesAttributevalue_id = speciesAttributeValue.id;
                speciesAttributeValue.id = 0;
                speciesAttributeValue.key_id = keyIdMap[speciesAttributeValue.key_id];
                speciesAttributeValue.species_id = speciesIdMap[speciesAttributeValue.species_id];
                speciesAttributeValue.attributevalue_id = attributeValueIdMap[speciesAttributeValue.attributevalue_id];
                targetKeyManager.Insert(speciesAttributeValue);
                speciesAttributeValueIdMap.Add(speciesAttributevalue_id, speciesAttributeValue.id);
            }
        }
    }
}
