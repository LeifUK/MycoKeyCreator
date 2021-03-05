using System;
using System.Collections.Generic;
using System.Linq;

namespace MycoKeys.WebApplication.Model
{
    public class KeyMatchViewDataBuilder
    {
        /*
         * Arguments: 
         * 
         *  keyName: The name of the current key
         *  
         *  attributeChoices: 
         *  
         *      Key:   Attribute value id
         *      Value: True if the attribute value is selected and False otherwise. 
         */
        public static Model.KeyMatchViewModel Build(Library.Database.IKeyManager iKeyManager, string keyName, Model.KeyMatchViewOutput keyMatchViewOutput)
        {
            /*
             * Locate the key
             */
            Library.DBObject.Key selectedKey = iKeyManager.GetKeyEnumerator().Where(n => n.name == keyName).FirstOrDefault();
            if (selectedKey == null)
            {
                return null;
            }

            Model.KeyMatchViewModel keyMatchViewModel = new Model.KeyMatchViewModel();
            keyMatchViewModel.Species = new List<Model.SpeciesMatchData>();
            keyMatchViewModel.KeyName = selectedKey.name;
            keyMatchViewModel.KeyTitle = selectedKey.title;
            keyMatchViewModel.KeyDescription = selectedKey.description;
            keyMatchViewModel.Literature = new List<Library.DBObject.Literature>();
            foreach (var literature in iKeyManager.GetKeyLiteratureEnumerator(selectedKey.id))
            {
                keyMatchViewModel.Literature.Add(literature);
            }
            keyMatchViewModel.Copyright = selectedKey.copyright;
            keyMatchViewModel.AttributeSelections = new List<KeyMatchViewModel.AttributeSelection>();

            /*
             * Key: The id of the selected attribute value
             * Value: The Selection object
             */
            Dictionary<Int64, KeyMatchViewOutput.Selection> attributeSelections = 
                keyMatchViewOutput != null ? 
                keyMatchViewOutput.AttributeSelections.ToDictionary(n => n.AttributeValueId, n => n) : 
                null;

            Dictionary<Int64, Library.DBObject.Attribute> attributesMap = iKeyManager.GetKeyAttributeEnumerator(selectedKey.id).OrderBy(n => n.position).ToDictionary(n => n.id, n => n);

            foreach (KeyValuePair<Int64, Library.DBObject.Attribute> item in attributesMap)
            {
                KeyMatchViewModel.AttributeSelection attributeSelection = new KeyMatchViewModel.AttributeSelection();
                attributeSelection.Attribute = item.Value;
                attributeSelection.AttributeValues = new List<Library.DBObject.AttributeValue>();
                foreach (Library.DBObject.AttributeValue attributeValue in iKeyManager.GetAttributeValueEnumerator(item.Value.id).OrderBy(n => n.position))
                {
                    bool isSelected = ((attributeSelections == null) || !attributeSelections.ContainsKey(attributeValue.id)) ?
                        false :
                        attributeSelections[attributeValue.id].IsSelected;

                    attributeSelection.AttributeValues.Add(attributeValue);
                    if (isSelected)
                    {
                        attributeSelection.IsSelected = isSelected;
                        attributeSelection.SelectedAttributeValueId = attributeValue.id;
                    }
                }

                keyMatchViewModel.AttributeSelections.Add(attributeSelection);
            }

            Dictionary<Int64, Library.DBObject.AttributeValue> attributeValuesMap = iKeyManager.GetKeyAttributeValueEnumerator(selectedKey.id).ToDictionary(n => n.id, n => n);
            List<Library.DBObject.SpeciesAttributeValue> speciesAttributeValues = iKeyManager.GetKeySpeciesAttributeValueEnumerator(selectedKey.id).ToList();

            var allSpecies = iKeyManager.GetKeySpeciesEnumerator(selectedKey.id).ToList();
            foreach (Library.DBObject.Species species in allSpecies)
            {
                List<Library.DBObject.SpeciesAttributeValue> currentSpeciesAttributeValues = speciesAttributeValues.Where(n => n.species_id == species.id).ToList();

                Dictionary<Int64, List<Int64>> currentSpeciesAttributeIdToAttributeValueIdMap = new Dictionary<Int64, List<Int64>>();
                foreach (var item in currentSpeciesAttributeValues)
                {
                    Int64 attributeId = attributeValuesMap[item.attributevalue_id].attribute_id;
                    if (!currentSpeciesAttributeIdToAttributeValueIdMap.ContainsKey(attributeId))
                    {
                        currentSpeciesAttributeIdToAttributeValueIdMap.Add(attributeId, new List<long>());
                    }
                    // Do globally and create a list of attribute ids here! 
                    currentSpeciesAttributeIdToAttributeValueIdMap[attributeId].Add(item.attributevalue_id);
                }

                int matches = 0;
                int mismatches = 0;
                List<string> mismatchedFeatures = new List<string>();
                List<Int64> currentSpeciesAttributeValueIds = currentSpeciesAttributeValues.Select(n => n.attributevalue_id).ToList();
                if (attributeSelections != null)
                {
                    foreach (var item in attributeSelections)
                    {
                        if (item.Value.IsSelected)
                        {
                            if (currentSpeciesAttributeValueIds.Contains(item.Key))
                            {
                                ++matches;
                            }
                            else
                            {
                                Int64 attributeId = attributeValuesMap[item.Key].attribute_id;
                                string text = attributesMap[attributeId].description + " ";
                                
                                if (currentSpeciesAttributeIdToAttributeValueIdMap.ContainsKey(attributeId))
                                {
                                    foreach (var attributeValueId in currentSpeciesAttributeIdToAttributeValueIdMap[attributeId])
                                    {
                                        mismatchedFeatures.Add(text + attributeValuesMap[attributeValueId].description.ToLower());
                                    }
                                }
                                ++mismatches;
                            }
                        }
                    }
                }

                int numberOfAttributes = attributeValuesMap.Where(n => currentSpeciesAttributeValueIds.Contains(n.Key)).
                    Select(n => n.Value.attribute_id).Distinct().Count();

                keyMatchViewModel.Species.Add(new SpeciesMatchData()
                {
                    Species = species,
                    AttributeCount = numberOfAttributes,
                    Matches = matches,
                    Mismatches = mismatches,
                    MismatchedFeatures = mismatchedFeatures
                });
            }

            keyMatchViewModel.Species = keyMatchViewModel.Species.OrderBy(
                n => (n.AttributeCount > 0) ? (100 * (-n.AttributeCount - n.Matches + n.Mismatches)) / n.AttributeCount : 1000).ToList();

            return keyMatchViewModel;
        }
    }
}
