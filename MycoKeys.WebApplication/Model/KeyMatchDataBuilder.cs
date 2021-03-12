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
                attributeSelection.AttributeChoices = new List<Library.DBObject.AttributeChoice>();
                foreach (Library.DBObject.AttributeChoice attributeChoice in iKeyManager.GetAttributeChoiceEnumerator(item.Value.id).OrderBy(n => n.position))
                {
                    attributeSelection.AttributeChoices.Add(attributeChoice);
                    if ((attributeSelectionsMap != null) && attributeSelectionsMap.ContainsKey(attributeChoice.id)) 
                    {
                        attributeSelection.IsSelected = attributeSelectionsMap[attributeChoice.id].IsSelected;
                        attributeSelection.SelectedAttributeValueId = attributeChoice.id;
                    }
                }
                keyMatchViewModel.AttributeSelections.Add(attributeSelection);
            }

            /*
             * Work out the matches/mismatches for each species
             */

            Dictionary<Int64, Library.DBObject.AttributeChoice> attributeChoicesMap = iKeyManager.GetKeyAttributeChoiceEnumerator(selectedKey.id).ToDictionary(n => n.id, n => n);
            List<Library.DBObject.SpeciesAttributeChoice> speciesAttributeChoices = iKeyManager.GetKeySpeciesAttributeChoiceEnumerator(selectedKey.id).ToList();

            IEnumerable<Library.DBObject.Species> speciesEnumerator = iKeyManager.GetKeySpeciesEnumerator(selectedKey.id).ToList();
            foreach (Library.DBObject.Species species in speciesEnumerator)
            {
                List<Int64> speciesAttributeChoiceIds = speciesAttributeChoices.Where(n => n.species_id == species.id).Select(n => n.attributechoice_id).ToList();

                /*
                 * For the current species => get the attribute values for each attribute
                 */
                Dictionary<Int64, List<Int64>> speciesAttributeIdAttributeChoiceIdMap = new Dictionary<Int64, List<Int64>>();
                foreach (var attributeValueId in speciesAttributeChoiceIds)
                {
                    Int64 attributeId = attributeChoicesMap[attributeValueId].attribute_id;
                    if (!speciesAttributeIdAttributeChoiceIdMap.ContainsKey(attributeId))
                    {
                        speciesAttributeIdAttributeChoiceIdMap.Add(attributeId, new List<long>());
                    }
                    speciesAttributeIdAttributeChoiceIdMap[attributeId].Add(attributeValueId);
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
                            if (speciesAttributeChoiceIds.Contains(item.Key))
                            {
                                ++matches;
                            }
                            else
                            {
                                Int64 attributeId = attributeChoicesMap[item.Key].attribute_id;
                                string text = attributesMap[attributeId].description + " ";
                                
                                if (speciesAttributeIdAttributeChoiceIdMap.ContainsKey(attributeId))
                                {
                                    foreach (var attributeValueId in speciesAttributeIdAttributeChoiceIdMap[attributeId])
                                    {
                                        mismatchedFeatures.Add(text + attributeChoicesMap[attributeValueId].description.ToLower());
                                    }
                                }
                                ++mismatches;
                            }
                        }
                    }
                }

                int numberOfAttributes = attributeChoicesMap.Where(n => speciesAttributeChoiceIds.Contains(n.Key)).
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
