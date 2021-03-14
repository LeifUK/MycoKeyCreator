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
            Database.IAttributeChoiceTable iAttributeChoiceTable,
            Database.ISpeciesAttributeChoiceTable iSpeciesAttributeChoiceTable,
            Database.ISpeciesAttributeSizeTable iSpeciesAttributeTableSize)
        {
            _iDatabase = iDatabase;
            _iKeyTable = iKeyTable;
            _iLiteratureTable = iLiteratureTable;
            _iSpeciesTable = iSpeciesTable;
            _iAttributeTable = iAttributeTable;
            _iAttributeChoiceTable = iAttributeChoiceTable;
            _iSpeciesAttributeChoiceTable = iSpeciesAttributeChoiceTable;
            _iSpeciesAttributeSizeTable = iSpeciesAttributeTableSize;

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

            if (!_iSpeciesAttributeSizeTable.Exists())
            {
                _iDatabase.CreateSpeciesSizeAttributeValueTable();
            }
        }

        private readonly Database.IDatabase _iDatabase;
        private readonly Database.IKeyTable _iKeyTable;
        private readonly Database.ILiteratureTable _iLiteratureTable;
        private readonly Database.ISpeciesTable _iSpeciesTable;
        private readonly Database.IAttributeTable _iAttributeTable;
        private readonly Database.IAttributeChoiceTable _iAttributeChoiceTable;
        private readonly Database.ISpeciesAttributeChoiceTable _iSpeciesAttributeChoiceTable;
        private readonly Database.ISpeciesAttributeSizeTable _iSpeciesAttributeSizeTable;

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
                _iSpeciesAttributeChoiceTable.DeleteByKey(key.id);
                _iSpeciesAttributeSizeTable.DeleteByKey(key.id);
                _iSpeciesTable.DeleteByKey(key.id);
                _iAttributeChoiceTable.DeleteByKey(key.id);
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

        public Library.DBObject.SpeciesAttributeChoice Select(Library.DBObject.SpeciesAttributeChoice speciesAttributeChoice)
        {
            if (speciesAttributeChoice.id == 0)
            {
                return null;
            }

            Library.DBObject.SpeciesAttributeChoice newSpeciesAttributeChoice = null;
            try
            {
                newSpeciesAttributeChoice = _iSpeciesAttributeChoiceTable.Query(speciesAttributeChoice.id);
            }
            catch
            {
            }

            return newSpeciesAttributeChoice;
        }

        public bool Insert(Library.DBObject.SpeciesAttributeChoice speciesAttributeChoice)
        {
            bool success = true;
            try
            {
                _iSpeciesAttributeChoiceTable.Insert(speciesAttributeChoice);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public bool Update(Library.DBObject.SpeciesAttributeChoice speciesAttributeChoice)
        {
            bool success = true;
            try
            {
                _iSpeciesAttributeChoiceTable.Update(speciesAttributeChoice);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public bool Delete(Library.DBObject.SpeciesAttributeChoice speciesAttributeChoice)
        {
            bool success = true;
            try
            {
                _iSpeciesAttributeChoiceTable.Delete(speciesAttributeChoice);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        private IEnumerable<DBObject.SpeciesAttributeChoice> GetSpeciesAttributeChoiceEnumerator()
        {
            return _iSpeciesAttributeChoiceTable.Enumerator;
        }

        public IEnumerable<DBObject.SpeciesAttributeChoice> GetKeySpeciesAttributeChoiceEnumerator(Int64 key_id)
        {
            return _iSpeciesAttributeChoiceTable.GetEnumeratorForKey(key_id);
        }

        public IEnumerable<DBObject.SpeciesAttributeChoice> GetSpeciesAttributeChoiceEnumerator(Int64 species_id)
        {
            return _iSpeciesAttributeChoiceTable.GetEnumeratorForSpecies(species_id);
        }

        public bool Insert(Library.DBObject.SpeciesAttributeSize speciesAttributeSize)
        {
            bool success = true;
            try
            {
                _iSpeciesAttributeSizeTable.Insert(speciesAttributeSize);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public bool Update(Library.DBObject.SpeciesAttributeSize speciesAttributeSize)
        {
            bool success = true;
            try
            {
                _iSpeciesAttributeSizeTable.Update(speciesAttributeSize);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public bool Delete(Library.DBObject.SpeciesAttributeSize speciesAttributeSize)
        {
            bool success = true;
            try
            {
                _iSpeciesAttributeSizeTable.Delete(speciesAttributeSize);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        private IEnumerable<DBObject.SpeciesAttributeSize> GetSpeciesAttributeSizeEnumerator()
        {
            return _iSpeciesAttributeSizeTable.Enumerator;
        }

        public IEnumerable<DBObject.SpeciesAttributeSize> GetSpeciesSizeAttributeEnumerator(Int64 species_id)
        {
            return _iSpeciesAttributeSizeTable.GetEnumeratorForSpecies(species_id);
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

        public bool Insert(DBObject.Attribute attribute, List<DBObject.AttributeChoice> attributeChoices)
        {
            bool success = true;
            try
            {
                _iDatabase.BeginTransaction();
                _iAttributeTable.Insert(attribute);

                foreach (var attributeChoice in attributeChoices)
                {
                    attributeChoice.key_id = attribute.key_id;
                    attributeChoice.attribute_id = attribute.id;
                    _iAttributeChoiceTable.Insert(attributeChoice);
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

        public bool Update(DBObject.Attribute attribute, List<DBObject.AttributeChoice> attributeChoices)
        {
            bool success = true;
            try
            {
                _iDatabase.BeginTransaction();
                _iAttributeTable.Update(attribute);

                Dictionary<Int64, Library.DBObject.AttributeChoice> attributeChoiceMap = _iAttributeChoiceTable.GetEnumeratorForAttribute(attribute.id).ToDictionary(n => n.id, n => n);
                if (attributeChoices != null)
                {
                    foreach (var attributeChoice in attributeChoices)
                    {
                        attributeChoice.key_id = attribute.key_id;
                        attributeChoice.attribute_id = attribute.id;
                        if (attributeChoice.id == 0)
                        {
                            _iAttributeChoiceTable.Insert(attributeChoice);
                        }
                        else
                        {
                            attributeChoiceMap.Remove(attributeChoice.id);
                            _iAttributeChoiceTable.Update(attributeChoice);
                        }
                    }
                }

                foreach (var attributeChoice in attributeChoiceMap.Values)
                {
                    _iSpeciesAttributeChoiceTable.DeleteByAttributeChoice(attributeChoice.id);
                    _iAttributeChoiceTable.Delete(attributeChoice);
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
                var attributeChoices = _iAttributeChoiceTable.GetEnumeratorForAttribute(attribute.id).ToList();
                foreach (var attributeChoice in attributeChoices)
                {
                    _iSpeciesAttributeChoiceTable.DeleteByAttributeChoice(attributeChoice.id);
                    _iAttributeChoiceTable.Delete(attributeChoice);
                }
                _iSpeciesAttributeSizeTable.DeleteByAttribute(attribute.id);
                _iAttributeTable.Delete(attribute);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public IEnumerable<DBObject.AttributeChoice> GetAttributeChoiceEnumerator()
        {
            return _iAttributeChoiceTable.Enumerator;
        }

        public IEnumerable<DBObject.AttributeChoice> GetAttributeChoiceEnumerator(Int64 attribute_id)
        {
            return _iAttributeChoiceTable.GetEnumeratorForAttribute(attribute_id);
        }

        public IEnumerable<DBObject.AttributeChoice> GetKeyAttributeChoiceEnumerator(Int64 key_id)
        {
            return _iAttributeChoiceTable.GetEnumeratorForKey(key_id);
        }

        public bool Insert(DBObject.AttributeChoice attributeChoice)
        {
            bool success = true;
            try
            {
                _iAttributeChoiceTable.Insert(attributeChoice);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public bool Update(DBObject.AttributeChoice attributeChoice)
        {
            bool success = true;
            try
            {
                _iAttributeChoiceTable.Update(attributeChoice);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        // Warning warning => no delete
        public bool Delete(DBObject.AttributeChoice attributeChoice)
        {
            bool success = true;
            try
            {
                _iSpeciesAttributeChoiceTable.DeleteByAttributeChoice(attributeChoice.id);
                _iAttributeChoiceTable.Delete(attributeChoice);
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
                _iSpeciesAttributeChoiceTable.DeleteBySpecies(species.id);
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

            foreach(Library.DBObject.Literature literature in sourceKeyManager.GetLiteratureEnumerator())
            {
                literature.id = 0;
                literature.key_id = keyIdMap[literature.key_id];
                targetKeyManager.Insert(literature);
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

            Dictionary<long, long> attributeChoiceIdMap = new Dictionary<long, long>();
            foreach (DBObject.AttributeChoice attributeChoice in sourceKeyManager.GetAttributeChoiceEnumerator())
            {
                long attributechoice_id = attributeChoice.id;
                attributeChoice.id = 0;
                attributeChoice.key_id = keyIdMap[attributeChoice.key_id];
                attributeChoice.attribute_id = attributeIdMap[attributeChoice.attribute_id];
                targetKeyManager.Insert(attributeChoice);
                attributeChoiceIdMap.Add(attributechoice_id, attributeChoice.id);
            }

            foreach (DBObject.SpeciesAttributeChoice speciesAttributeChoice in sourceKeyManager.GetSpeciesAttributeChoiceEnumerator())
            {
                long speciesAttributevalue_id = speciesAttributeChoice.id;
                speciesAttributeChoice.id = 0;
                speciesAttributeChoice.key_id = keyIdMap[speciesAttributeChoice.key_id];
                speciesAttributeChoice.species_id = speciesIdMap[speciesAttributeChoice.species_id];
                speciesAttributeChoice.attributechoice_id = attributeChoiceIdMap[speciesAttributeChoice.attributechoice_id];
                targetKeyManager.Insert(speciesAttributeChoice);
            }

            foreach (DBObject.SpeciesAttributeSize speciesAttributeSize in sourceKeyManager.GetSpeciesAttributeSizeEnumerator())
            {
                long speciesAttributeSize_id = speciesAttributeSize.id;
                try
                {
                    speciesAttributeSize.id = 0;
                    speciesAttributeSize.key_id = keyIdMap[speciesAttributeSize.key_id];
                    speciesAttributeSize.species_id = speciesIdMap[speciesAttributeSize.species_id];
                    speciesAttributeSize.attribute_id = attributeIdMap[speciesAttributeSize.attribute_id];
                    speciesAttributeSize.value = speciesAttributeSize.value;
                    targetKeyManager.Insert(speciesAttributeSize);
                }
                catch (Exception exception)
                {

                }
            }
        }
    }
}
