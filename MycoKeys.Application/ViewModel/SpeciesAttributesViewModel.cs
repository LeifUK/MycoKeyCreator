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
            SpeciesAttributes = new List<Model.ISpeciesAttributeValueModel>();

            List<Library.DBObject.Attribute> attributes = new List<Library.DBObject.Attribute>(
                IKeyManager.GetKeyAttributeEnumerator(Species.key_id).OrderBy(n => n.position));

            Dictionary<Int64, Library.DBObject.SpeciesAttributeValue> speciesAttributeValuesMap =
                IKeyManager.GetSpeciesAttributeValueEnumerator(Species.id).ToDictionary(n => n.attributevalue_id, n => n);

            Dictionary<Int64, Library.DBObject.SpeciesSizeAttributeValue> speciesSizeAttributeValuesMap =
                IKeyManager.GetSpeciesSizeAttributeValueEnumerator(Species.id).ToDictionary(n => n.attribute_id, n => n);

            foreach (var attribute in attributes)
            {
                Library.Database.AttributeType attributeType = (Library.Database.AttributeType)attribute.type;
                switch (attributeType)
                {
                    case Library.Database.AttributeType.Choice:
                        {
                            List<Library.DBObject.AttributeValue> attributeValues = IKeyManager.GetAttributeValueEnumerator(attribute.id).OrderBy(n => n.position).ToList();
                            foreach (var attributeValue in attributeValues)
                            {
                                Model.SpeciesChoiceAttributeValueModel speciesChoiceAttributeValueModel = new Model.SpeciesChoiceAttributeValueModel();
                                speciesChoiceAttributeValueModel.Attribute = attribute;
                                speciesChoiceAttributeValueModel.AttributeValue = attributeValue;
                                if (speciesAttributeValuesMap.ContainsKey(attributeValue.id))
                                {
                                    speciesChoiceAttributeValueModel.IsUsed = true;
                                    speciesChoiceAttributeValueModel.SpeciesAttributeValue = speciesAttributeValuesMap[attributeValue.id];
                                }
                                SpeciesAttributes.Add(speciesChoiceAttributeValueModel);
                                speciesChoiceAttributeValueModel.OnChanged += SpeciesAttribute_OnChanged;
                            }
                        }
                        break;
                    case Library.Database.AttributeType.MaximumSize:
                    case Library.Database.AttributeType.MinimumSize:
                        Model.SpeciesSizeAttributeValueModel speciesSizeAttributeValueModel = new Model.SpeciesSizeAttributeValueModel();
                        speciesSizeAttributeValueModel.Attribute = attribute;
                        if (speciesSizeAttributeValuesMap.ContainsKey(attribute.id))
                        {
                            speciesSizeAttributeValueModel.IsUsed = true;
                            speciesSizeAttributeValueModel.SpeciesSizeAttributeValue = speciesSizeAttributeValuesMap[attribute.id];
                        }
                        SpeciesAttributes.Add(speciesSizeAttributeValueModel);
                        speciesSizeAttributeValueModel.OnChanged += SpeciesAttribute_OnChanged;
                        break;
                }
            }
        }

        private void SpeciesAttribute_OnChanged(Model.ISpeciesAttributeValueModel sender)
        {
            if (sender is Model.SpeciesChoiceAttributeValueModel)
            {
                Model.SpeciesChoiceAttributeValueModel speciesChoiceAttributeValueModel = sender as Model.SpeciesChoiceAttributeValueModel;
                if (sender.IsUsed)
                {
                    speciesChoiceAttributeValueModel.SpeciesAttributeValue = new Library.DBObject.SpeciesAttributeValue();
                    speciesChoiceAttributeValueModel.SpeciesAttributeValue.key_id = Key.id;
                    speciesChoiceAttributeValueModel.SpeciesAttributeValue.species_id = Species.id;
                    speciesChoiceAttributeValueModel.SpeciesAttributeValue.attributevalue_id = speciesChoiceAttributeValueModel.AttributeValue.id;
                    IKeyManager.Insert(speciesChoiceAttributeValueModel.SpeciesAttributeValue);
                }
                else
                {
                    if (speciesChoiceAttributeValueModel.SpeciesAttributeValue != null)
                    {
                        IKeyManager.Delete(speciesChoiceAttributeValueModel.SpeciesAttributeValue);
                        speciesChoiceAttributeValueModel.SpeciesAttributeValue = null;
                    }
                }
            }
            else if (sender is Model.SpeciesSizeAttributeValueModel)
            {
                Model.SpeciesSizeAttributeValueModel speciesSizeAttribute = sender as Model.SpeciesSizeAttributeValueModel;
                if (sender.IsUsed)
                {
                    if (speciesSizeAttribute.SpeciesSizeAttributeValue != null)
                    {
                        IKeyManager.Update(speciesSizeAttribute.SpeciesSizeAttributeValue);
                    }
                    else
                    {
                        speciesSizeAttribute.SpeciesSizeAttributeValue = new Library.DBObject.SpeciesSizeAttributeValue();
                        speciesSizeAttribute.SpeciesSizeAttributeValue.key_id = Key.id;
                        speciesSizeAttribute.SpeciesSizeAttributeValue.species_id = Species.id;
                        speciesSizeAttribute.SpeciesSizeAttributeValue.attribute_id = speciesSizeAttribute.Attribute.id;
                        speciesSizeAttribute.SpeciesSizeAttributeValue.value = 1;
                        IKeyManager.Insert(speciesSizeAttribute.SpeciesSizeAttributeValue);
                    }
                }
                else
                {
                    if (speciesSizeAttribute.SpeciesSizeAttributeValue != null)
                    {
                        IKeyManager.Delete(speciesSizeAttribute.SpeciesSizeAttributeValue);
                        speciesSizeAttribute.SpeciesSizeAttributeValue = null;
                    }
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

        private List<Model.ISpeciesAttributeValueModel> _speciesAttributes;
        public List<Model.ISpeciesAttributeValueModel> SpeciesAttributes
        {
            get
            {
                return _speciesAttributes;
            }
            set
            {
                _speciesAttributes = value;
                NotifyPropertyChanged("SpeciesAttributes");
            }
        }
    }
}
