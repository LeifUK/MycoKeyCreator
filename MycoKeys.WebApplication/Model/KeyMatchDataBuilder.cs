using System;
using System.Collections.Generic;
using System.Linq;

namespace MycoKeys.WebApplication.Model
{
    public class KeyMatchViewDataBuilder
    {
        private static Dictionary<Int64, Library.DBObject.Attribute> GetSpeciesAttributeMap(Int64 parentId, Dictionary<Int64, KeyValuePair<Library.DBObject.Attribute, bool>> attributeMap)
        {
            Dictionary<Int64, Library.DBObject.Attribute> map = new Dictionary<long, Library.DBObject.Attribute>();
            while (attributeMap.ContainsKey(parentId))
            {
                Library.DBObject.Attribute attribute = attributeMap[parentId].Key;
                map.Add(attribute.id, attribute);
                parentId = attribute.parent_id;
            }

            return map;
        }

        private static void ProcessKeyItem(List<KeyItem> keyItems, KeyNode keyNode, int parentNumber, int number, ref int nextFreeNumber, Dictionary<Int64, Library.DBObject.Species> speciesParentMap)
        {
            int index = keyItems.Count;
            KeyItem keyItem = new KeyItem()
            {
                Attribute = (keyNode.Attribute != null) ? keyNode.Attribute.description : null,
                Number = number,
                ParentNumber = parentNumber
            };
            keyItems.Add(keyItem);

            parentNumber = number;
            ++number;

            if (number <= nextFreeNumber)
            {
                number = nextFreeNumber;
            }

            if (keyNode.Items.Count > 0)
            {
                ++nextFreeNumber;
                foreach (var childKeyNode in keyNode.Items)
                {
                    ProcessKeyItem(keyItems, childKeyNode, keyItem.Number, number, ref nextFreeNumber, speciesParentMap);
                }
            }
            else
            {
                if (keyNode.Attribute != null)
                {
                    if (speciesParentMap.ContainsKey(keyNode.Attribute.id))
                    {
                        keyItem.Species = speciesParentMap[keyNode.Attribute.id].name;
                    }
                }
            }

            if (keyItems.Count > (index + 1))
            {
                keyItems[index].ChildKeyItem = keyItems[index + 1];
            }
        }

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


            /*
             * Build the advanced key
             */

            /*
             * Key:   Attribute id
             * Value:  
             *        Key: Attribute
             *        Value: True if the attribute is selected and false otherwise
             */
            Dictionary<Int64, KeyValuePair<Library.DBObject.Attribute, bool>> attributeMap =
                iKeyManager.GetKeyAttributeEnumerator(selectedKey.id).ToDictionary(
                    n => n.id,
                    n => new KeyValuePair<Library.DBObject.Attribute, bool>(n, attributeChoices != null ? attributeChoices[n.id] : false));

            Dictionary<Int64, Library.DBObject.Species> speciesParentMap = 
                iKeyManager.GetKeySpeciesEnumerator(selectedKey.id).ToDictionary(n => n.parent_id, n => n);

            Model.KeyMatchViewData keyMatchData = new Model.KeyMatchViewData();
            keyMatchData.KeyName = selectedKey.name;
            keyMatchData.KeyTitle = selectedKey.title;
            keyMatchData.AttributeSelections = attributeMap.ToDictionary(n => n.Value.Key, n => n.Value.Value);
            keyMatchData.Species = new List<Model.SpeciesMatchData>();
            foreach (KeyValuePair<long, Library.DBObject.Species> item in speciesParentMap)
            {
                Model.SpeciesMatchData speciesMatchData = new Model.SpeciesMatchData() { Species = item.Value };
                if (attributeChoices != null)
                {
                    Dictionary<Int64, Library.DBObject.Attribute> speciesAttributeMap = GetSpeciesAttributeMap(item.Value.parent_id, attributeMap);

                    speciesMatchData.AttributeCount = speciesAttributeMap.Count;

                    foreach (var temp in attributeMap)
                    {
                        if (temp.Value.Value)
                        {
                            if (speciesAttributeMap.ContainsKey(temp.Key))
                            {
                                speciesMatchData.Matches++;
                            }
                            else
                            {
                                speciesMatchData.Mismatches++;
                            }
                        }
                    }
                }

                keyMatchData.Species.Add(speciesMatchData);
            }

            keyMatchData.Species = keyMatchData.Species.OrderBy(
                n => ( n.AttributeCount > 0 ) ? (100 * (n.AttributeCount - n.Matches)) / n.AttributeCount : 100 + 50 * n.Mismatches).ToList();

            /*
             * Build the traditional key
             */

            Dictionary<Int64, Model.KeyNode> nodeMap = attributeMap.ToDictionary(n => n.Key, n => new Model.KeyNode() { Attribute = n.Value.Key });
            KeyNode rootNode = new KeyNode();
            nodeMap.Add(0, rootNode);

            foreach (var node in attributeMap.OrderBy(n => n.Value.Key.position))
            {
                Library.DBObject.Attribute attribute = node.Value.Key;
                nodeMap[attribute.parent_id].Items.Add(nodeMap[attribute.id]);
            }

            keyMatchData.KeyItems = new List<KeyItem>();
            int number = 0;
            int nextFreeNumber = 1;
            ProcessKeyItem(keyMatchData.KeyItems, rootNode, 0, number, ref nextFreeNumber, speciesParentMap);
            keyMatchData.KeyItems.RemoveAt(0);
            keyMatchData.KeyItems = keyMatchData.KeyItems.OrderBy(n => n.Number).ToList();

            return keyMatchData;
        }
    }
}
