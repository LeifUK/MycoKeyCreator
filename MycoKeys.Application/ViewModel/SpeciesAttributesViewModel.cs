using System;
using System.Collections.Generic;
using System.Linq;

namespace MycoKeys.Application.ViewModel
{
    class SpeciesAttributesViewModel : OpenControls.Wpf.Utilities.ViewModel.BaseViewModel
    {
        public SpeciesAttributesViewModel(MycoKeys.Library.Database.IKeyManager iKeyManager, MycoKeys.Library.DBObject.Key key, MycoKeys.Library.DBObject.Species species)
        {
            Species = species;
            IKeyManager = iKeyManager;
            Key = key;
            Load();
        }

        public readonly MycoKeys.Library.Database.IKeyManager IKeyManager;
        public readonly MycoKeys.Library.DBObject.Key Key;
        private readonly MycoKeys.Library.DBObject.Species Species;

        public void Load()
        {
            SpeciesAttributeValueModels = new List<SpeciesAttributeValueModel>();

            List<Library.DBObject.Attribute> attributes = new List<Library.DBObject.Attribute>(
                IKeyManager.GetKeyAttributeEnumerator(Species.key_id).OrderBy(n => n.position));

            Dictionary<Int64, Library.DBObject.SpeciesAttributeValue> speciesAttributeValuesMap = 
                IKeyManager.GetSpeciesAttributeValueEnumerator(Species.id).ToDictionary(n=> n.attributevalue_id, n => n);

            foreach (var attribute in attributes)
            {
                List<Library.DBObject.AttributeValue> attributeValues = IKeyManager.GetAttributeValueEnumerator(attribute.id).OrderBy(n => n.position).ToList();
                foreach (var attributeValue in attributeValues)
                {
                    SpeciesAttributeValueModel speciesAttributeValueModel = new SpeciesAttributeValueModel();
                    speciesAttributeValueModel.Attribute = attribute;
                    speciesAttributeValueModel.AttributeValue = attributeValue;
                    if (speciesAttributeValuesMap.ContainsKey(attributeValue.id))
                    {
                        speciesAttributeValueModel.IsUsed = true;
                        speciesAttributeValueModel.SpeciesAttributeValue = speciesAttributeValuesMap[attributeValue.id];
                    }

                    speciesAttributeValueModel.OnCheck += SpeciesAttributeValueModel_OnCheck;
                    SpeciesAttributeValueModels.Add(speciesAttributeValueModel);
                }
            }
        }

        private void SpeciesAttributeValueModel_OnCheck(SpeciesAttributeValueModel sender)
        {
            if (sender.IsUsed)
            {
                Library.DBObject.SpeciesAttributeValue speciesAttributeValue = new Library.DBObject.SpeciesAttributeValue();
                speciesAttributeValue.key_id = Key.id;
                speciesAttributeValue.species_id = Species.id;
                speciesAttributeValue.attributevalue_id = sender.AttributeValue.id;
                IKeyManager.Insert(speciesAttributeValue);
            }
            else
            {
                if (sender.SpeciesAttributeValue != null)
                {
                    IKeyManager.Delete(sender.SpeciesAttributeValue);
                    sender.SpeciesAttributeValue = null;
                }
            }
        }

        public void Save()
        {
            Species.name = Name;
        }

        public string Title
        {
            get
            {
                return "Attributes: " + Name;
            }
        }

        public string Name
        {
            get
            {
                return Species.name;
            }
        }

        private List<SpeciesAttributeValueModel> _speciesAttributeValueModels;
        public List<SpeciesAttributeValueModel> SpeciesAttributeValueModels
        {
            get
            {
                return _speciesAttributeValueModels;
            }
            set
            {
                _speciesAttributeValueModels = value;
                NotifyPropertyChanged("SpeciesAttributeValueModels");
            }
        }
    }
}
