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
         *      Key:   Attribute id
         *      Value: True if the attribute is selected and False otherwise. 
         */
        public static Model.KeyMatchViewData Build(Library.Database.IKeyManager iKeyManager, string keyName, Dictionary<Int64, bool> attributeChoices)
        {
            /*
             * Locate the key
             */
            Library.DBObject.Key selectedKey = iKeyManager.GetKeyEnumerator().Where(n => n.name == keyName).FirstOrDefault();
            if (selectedKey == null)
            {
                return null;
            }

            Model.KeyMatchViewData keyMatchData = new Model.KeyMatchViewData();
            keyMatchData.Species = new List<Model.SpeciesMatchData>();
            keyMatchData.KeyName = selectedKey.name;
            keyMatchData.KeyTitle = selectedKey.title;
            keyMatchData.KeyDescription = selectedKey.description;
            keyMatchData.Copyright = selectedKey.copyright;
            keyMatchData.AttributeSelections = iKeyManager.GetKeyAttributeEnumerator(selectedKey.id).Select(n => 
                new KeyValuePair<MycoKeys.Library.DBObject.Attribute, bool>(n, (attributeChoices != null ? attributeChoices[n.id] : false)))
                .OrderBy(n => n.Key.position).ToList();

            var selectedAttributeIds = keyMatchData.AttributeSelections.Where(n => n.Value).Select(n => n.Key.id).ToList();
            int totalNumberOfSelectedAttributes = selectedAttributeIds.Count();

            var species = iKeyManager.GetKeySpeciesEnumerator(selectedKey.id).ToList();
            for (int i = 0; i < species.Count; ++i)
            {
                var list = iKeyManager.GetSpeciesAttributeEnumerator(species[i].id).ToList();
                int matches = list.Where(n => selectedAttributeIds.Contains(n.attributevalue_id)).Count();

                keyMatchData.Species.Add(new SpeciesMatchData() 
                { 
                    Species = species[i],
                    AttributeCount = list.Count(),
                    Matches = matches,
                    Mismatches = totalNumberOfSelectedAttributes - matches
                });
            }

            keyMatchData.Species = keyMatchData.Species.OrderBy(
                n => ( n.AttributeCount > 0 ) ? (100 * (n.AttributeCount - n.Matches)) / n.AttributeCount : 100 + 50 * n.Mismatches).ToList();

            return keyMatchData;
        }
    }
}
