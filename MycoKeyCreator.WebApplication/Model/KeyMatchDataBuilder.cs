using System;
using System.Collections.Generic;
using System.Linq;

namespace MycoKeyCreator.WebApplication.Model
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
            keyMatchViewModel.KeyNotes = selectedKey.notes;
            foreach (var literature in iKeyManager.GetLiteratureEnumeratorForKey(selectedKey.id).OrderBy(n => n.position))
            {
                keyMatchViewModel.Literature.Add(literature);
            }
            keyMatchViewModel.Copyright = selectedKey.copyright;

            /*
             * Add the attributes and their selection status
             */

            Dictionary<Int64, Library.DBObject.Attribute> attributesMap = 
                iKeyManager.GetAttributeEnumeratorForKey(selectedKey.id).OrderBy(n => n.position).ToDictionary(n => n.id, n => n);
            Dictionary<Int64, KeyMatchViewOutput.Selection> choicesMap = 
                keyMatchViewOutput != null ? 
                keyMatchViewOutput.AttributeSelections.Where(n => n.AttributeType == (Int16)Library.Database.AttributeType.Choice).
                ToDictionary(n => (Int64)n.AttributeValue, n => n) : 
                null;

            Dictionary<Int64, KeyMatchViewOutput.Selection> sizesMap =
                keyMatchViewOutput != null ?
                keyMatchViewOutput.AttributeSelections.Where(n => n.AttributeType != (Int16)Library.Database.AttributeType.Choice).
                ToDictionary(n => n.AttributeId, n => n) :
                null;

            foreach (KeyValuePair<Int64, Library.DBObject.Attribute> item in attributesMap)
            {
                if (item.Value.type == (Int16)Library.Database.AttributeType.Choice)
                {
                    KeyMatchViewModel.AttributeChoice attribute = new KeyMatchViewModel.AttributeChoice();
                    attribute.Attribute = item.Value;
                    attribute.AttributeChoices = new List<Library.DBObject.AttributeChoice>();
                    foreach (Library.DBObject.AttributeChoice attributeChoice in iKeyManager.GetAttributeChoiceEnumeratorForAttribute(item.Value.id).OrderBy(n => n.position))
                    {
                        attribute.AttributeChoices.Add(attributeChoice);
                        if ((choicesMap != null) && choicesMap.ContainsKey(attributeChoice.id))
                        {
                            attribute.IsSelected = choicesMap[attributeChoice.id].IsSelected;
                            attribute.SelectedAttributeChoiceId = attributeChoice.id;
                        }
                    }
                    keyMatchViewModel.AttributeSelections.Add(attribute);
                }
                else
                {
                    KeyMatchViewModel.AttributeSize attribute = new KeyMatchViewModel.AttributeSize();
                    attribute.Attribute = item.Value;
                    if ((sizesMap != null) && sizesMap.ContainsKey(item.Value.id))
                    {
                        attribute.IsSelected = sizesMap[item.Value.id].IsSelected;
                        attribute.Value = sizesMap[item.Value.id].AttributeValue;
                    }
                    keyMatchViewModel.AttributeSelections.Add(attribute);
                }
            }

            /*
             * Work out the matches/mismatches for each species
             */

            Dictionary<Int64, Library.DBObject.AttributeChoice> attributeChoicesMap = iKeyManager.GetAttributeChoiceEnumeratorForKey(selectedKey.id).ToDictionary(n => n.id, n => n);
            List<Library.DBObject.SpeciesAttributeChoice> speciesAttributeChoices = iKeyManager.GetSpeciesAttributeChoiceEnumeratorForKey(selectedKey.id).ToList();

            IEnumerable<Library.DBObject.Species> speciesEnumerator = iKeyManager.GetKeySpeciesEnumerator(selectedKey.id).ToList();
            foreach (Library.DBObject.Species species in speciesEnumerator)
            {
                List<Int64> speciesAttributeChoiceIds = speciesAttributeChoices.Where(n => n.species_id == species.id).Select(n => n.attributechoice_id).ToList();

                /*
                 * For the current species => get the attribute values for each attribute
                 */
                Dictionary<Int64, List<Int64>> speciesAttributeIdAttributeChoiceIdMap = new Dictionary<Int64, List<Int64>>();
                foreach (var attributeChoiceId in speciesAttributeChoiceIds)
                {
                    if (attributeChoicesMap.ContainsKey(attributeChoiceId))
                    {
                        Int64 attributeId = attributeChoicesMap[attributeChoiceId].attribute_id;
                        if (!speciesAttributeIdAttributeChoiceIdMap.ContainsKey(attributeId))
                        {
                            speciesAttributeIdAttributeChoiceIdMap.Add(attributeId, new List<long>());
                        }
                        speciesAttributeIdAttributeChoiceIdMap[attributeId].Add(attributeChoiceId);
                    }
                }

                int matches = 0;
                int mismatches = 0;
                List<string> mismatchedFeatures = new List<string>();
                int numberOfAttributes = 0;

                if (sizesMap != null)
                {
                    Dictionary<Int64, Library.DBObject.SpeciesAttributeSize> attributeSizesMap = iKeyManager.GetSpeciesSizeAttributeEnumeratorForSpecies(species.id).ToDictionary(n => n.attribute_id, n => n);

                    foreach (var item in sizesMap)
                    {
                        if (item.Value.IsSelected)
                        {
                            if (attributeSizesMap.ContainsKey(item.Value.AttributeId))
                            {
                                ++numberOfAttributes;

                                Library.DBObject.Attribute attribute = attributesMap[item.Value.AttributeId];
                                Library.Database.AttributeType attributeType = (Library.Database.AttributeType)attribute.type;
                                Library.DBObject.SpeciesAttributeSize speciesAttributeSize = attributeSizesMap[item.Value.AttributeId];
                                if (
                                     (
                                       (attributeType == Library.Database.AttributeType.MaximumSize) &&
                                       (item.Value.AttributeValue > speciesAttributeSize.value)
                                     ) ||
                                     (
                                       (attributeType == Library.Database.AttributeType.MinimumSize) &&
                                       (item.Value.AttributeValue < speciesAttributeSize.value)
                                     )
                                   )
                                {
                                    ++mismatches;
                                    mismatchedFeatures.Add(attribute.description + " " + speciesAttributeSize.value);
                                }
                                else
                                {
                                    ++matches;
                                }
                            }
                        }
                    }
                }

                if (choicesMap != null)
                {
                    foreach (var item in choicesMap)
                    {
                        if (item.Value.IsSelected)
                        {
                            ++numberOfAttributes;

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
