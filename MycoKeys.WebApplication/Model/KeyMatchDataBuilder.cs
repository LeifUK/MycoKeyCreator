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
            keyMatchViewModel.Copyright = selectedKey.copyright;
            keyMatchViewModel.AttributeSelections = new List<KeyMatchViewModel.AttributeSelection>();

            var attributeValueSelections = keyMatchViewOutput != null ? keyMatchViewOutput.AttributeSelections.ToDictionary(n => n.AttributeValueId, n => n.IsSelected) : null;

            /*
             * Key:   Attribute value id
             * Value: Attribute ids
             */
            Dictionary<Int64, List<Int64>> attributeValueToAttributeMap = new Dictionary<long, List<long>>();
            Dictionary<Int64, bool> attributeTicked = new Dictionary<long, bool>();

            List<Library.DBObject.Attribute> attributes = iKeyManager.GetKeyAttributeEnumerator(selectedKey.id).OrderBy(n => n.position).ToList();
            foreach (Library.DBObject.Attribute attribute in attributes)
            {
                var list = new List<Int64>();
                attributeValueToAttributeMap.Add(attribute.id, list);

                KeyMatchViewModel.AttributeSelection attributeSelection = new KeyMatchViewModel.AttributeSelection();
                attributeSelection.Attribute = attribute;
                attributeSelection.AttributeValues = new List<Library.DBObject.AttributeValue>();
                foreach (Library.DBObject.AttributeValue attributeValue in iKeyManager.GetAttributeValueEnumerator(attribute.id).OrderBy(n => n.position))
                {
                    list.Add(attributeValue.id);

                    bool isSelected = ((attributeValueSelections == null) || !attributeValueSelections.ContainsKey(attributeValue.id)) ?
                        false :
                        attributeValueSelections[attributeValue.id];

                    attributeSelection.AttributeValues.Add(attributeValue);
                    if (isSelected)
                    {
                        attributeSelection.IsSelected = isSelected;
                        attributeSelection.SelectedAttributeValueId = attributeValue.id;
                    }
                }

                keyMatchViewModel.AttributeSelections.Add(attributeSelection);
            }

            List<Library.DBObject.SpeciesAttributeValue> speciesAttributeValues = iKeyManager.GetKeySpeciesAttributeValueEnumerator(selectedKey.id).ToList();
            List<Library.DBObject.AttributeValue> attributeValues = iKeyManager.GetKeyAttributeValueEnumerator(selectedKey.id).ToList();
            var species = iKeyManager.GetKeySpeciesEnumerator(selectedKey.id).ToList();
            for (int i = 0; i < species.Count; ++i)
            {
                List<Int64> speciesAttributeValueIds = speciesAttributeValues.Where(n => n.species_id == species[i].id).
                    Select(n => n.attributevalue_id).ToList();
                int numberOfAttributes = attributeValues.Where(n => speciesAttributeValueIds.Contains(n.id)).
                    Select(n => n.attribute_id).Distinct().Count();
                int matches = 0;
                int mismatches = 0;

                if (attributeValueSelections != null)
                {
                    foreach (var item in attributeValueSelections)
                    {
                        if (item.Value)
                        {
                            if (speciesAttributeValueIds.Contains(item.Key))
                            {
                                ++matches;
                            }
                            else
                            {
                                ++mismatches;
                            }
                        }
                    }
                }

                keyMatchViewModel.Species.Add(new SpeciesMatchData()
                {
                    Species = species[i],
                    AttributeCount = numberOfAttributes,
                    Matches = matches,
                    Mismatches = mismatches
                });

            }

            keyMatchViewModel.Species = keyMatchViewModel.Species.OrderBy(
                n => (n.AttributeCount > 0) ? (100 * (-n.AttributeCount - n.Matches + n.Mismatches)) / n.AttributeCount : 1000).ToList();

            return keyMatchViewModel;
        }
    }
}
