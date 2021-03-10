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
        public static Model.KeyMatchViewModel Build(
            Library.Database.IKeyManager iKeyManager, 
            string keyName, 
            Model.KeyMatchViewOutput keyMatchViewOutput)
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

            /*
             * Add the header information
             */

            keyMatchViewModel.KeyName = selectedKey.name;
            keyMatchViewModel.KeyTitle = selectedKey.title;
            keyMatchViewModel.KeyDescription = selectedKey.description;
            foreach (var literature in iKeyManager.GetKeyLiteratureEnumerator(selectedKey.id))
            {
                keyMatchViewModel.Literature.Add(literature);
            }
            keyMatchViewModel.Copyright = selectedKey.copyright;

            /*
             * Add the attributes and their selection status
             */

            Dictionary<Int64, Library.DBObject.Attribute> attributesMap = 
                iKeyManager.GetKeyAttributeEnumerator(selectedKey.id).OrderBy(n => n.position).ToDictionary(n => n.id, n => n);
            Dictionary<Int64, KeyMatchViewOutput.Selection> attributeSelectionsMap = 
                keyMatchViewOutput != null ? 
                keyMatchViewOutput.AttributeSelections.ToDictionary(n => n.AttributeValueId, n => n) : 
                null;

            foreach (KeyValuePair<Int64, Library.DBObject.Attribute> item in attributesMap)
            {
                KeyMatchViewModel.AttributeSelection attributeSelection = new KeyMatchViewModel.AttributeSelection();
                attributeSelection.Attribute = item.Value;
                attributeSelection.AttributeValues = new List<Library.DBObject.AttributeValue>();
                foreach (Library.DBObject.AttributeValue attributeValue in iKeyManager.GetAttributeValueEnumerator(item.Value.id).OrderBy(n => n.position))
                {
                    attributeSelection.AttributeValues.Add(attributeValue);
                    if ((attributeSelectionsMap != null) && attributeSelectionsMap.ContainsKey(attributeValue.id)) 
                    {
                        attributeSelection.IsSelected = attributeSelectionsMap[attributeValue.id].IsSelected;
                        attributeSelection.SelectedAttributeValueId = attributeValue.id;
                    }
                }
                keyMatchViewModel.AttributeSelections.Add(attributeSelection);
            }

            /*
             * Work out the matches/mismatches for each species
             */

            Dictionary<Int64, Library.DBObject.AttributeValue> attributeValuesMap = iKeyManager.GetKeyAttributeValueEnumerator(selectedKey.id).ToDictionary(n => n.id, n => n);
            List<Library.DBObject.SpeciesAttributeValue> speciesAttributeValues = iKeyManager.GetKeySpeciesAttributeValueEnumerator(selectedKey.id).ToList();

            IEnumerable<Library.DBObject.Species> speciesEnumerator = iKeyManager.GetKeySpeciesEnumerator(selectedKey.id).ToList();
            foreach (Library.DBObject.Species species in speciesEnumerator)
            {
                List<Int64> speciesAttributeValueIds = speciesAttributeValues.Where(n => n.species_id == species.id).Select(n => n.attributevalue_id).ToList();

                /*
                 * For the current species => get the attribute values for each attribute
                 */
                Dictionary<Int64, List<Int64>> speciesAttributeIdAttributeValueIdMap = new Dictionary<Int64, List<Int64>>();
                foreach (var attributeValueId in speciesAttributeValueIds)
                {
                    Int64 attributeId = attributeValuesMap[attributeValueId].attribute_id;
                    if (!speciesAttributeIdAttributeValueIdMap.ContainsKey(attributeId))
                    {
                        speciesAttributeIdAttributeValueIdMap.Add(attributeId, new List<long>());
                    }
                    speciesAttributeIdAttributeValueIdMap[attributeId].Add(attributeValueId);
                }

                int matches = 0;
                int mismatches = 0;
                List<string> mismatchedFeatures = new List<string>();
                if (attributeSelectionsMap != null)
                {
                    foreach (var item in attributeSelectionsMap)
                    {
                        if (item.Value.IsSelected)
                        {
                            if (speciesAttributeValueIds.Contains(item.Key))
                            {
                                ++matches;
                            }
                            else
                            {
                                Int64 attributeId = attributeValuesMap[item.Key].attribute_id;
                                string text = attributesMap[attributeId].description + " ";
                                
                                if (speciesAttributeIdAttributeValueIdMap.ContainsKey(attributeId))
                                {
                                    foreach (var attributeValueId in speciesAttributeIdAttributeValueIdMap[attributeId])
                                    {
                                        mismatchedFeatures.Add(text + attributeValuesMap[attributeValueId].description.ToLower());
                                    }
                                }
                                ++mismatches;
                            }
                        }
                    }
                }

                int numberOfAttributes = attributeValuesMap.Where(n => speciesAttributeValueIds.Contains(n.Key)).
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
