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
            SpeciesAttributes = new List<Model.ISpeciesAttributeValue>();

            List<Library.DBObject.Attribute> attributes = new List<Library.DBObject.Attribute>(
                IKeyManager.GetAttributeEnumeratorForKey(Species.key_id).OrderBy(n => n.position));

            Dictionary<Int64, Library.DBObject.SpeciesAttributeChoice> speciesAttributeChoicesMap =
                IKeyManager.GetSpeciesAttributeChoiceEnumeratorForSpecies(Species.id).ToDictionary(n => n.attributechoice_id, n => n);

            Dictionary<Int64, Library.DBObject.SpeciesAttributeSize> speciesAttributeSizesMap =
                IKeyManager.GetSpeciesSizeAttributeEnumeratorForSpecies(Species.id).ToDictionary(n => n.attribute_id, n => n);

            foreach (var attribute in attributes)
            {
                Library.Database.AttributeType attributeType = (Library.Database.AttributeType)attribute.type;
                switch (attributeType)
                {
                    case Library.Database.AttributeType.Choice:
                        {
                            List<Library.DBObject.AttributeChoice> attributeChoices = IKeyManager.GetAttributeChoiceEnumeratorForAttribute(attribute.id).OrderBy(n => n.position).ToList();
                            foreach (var attributeChoice in attributeChoices)
                            {
                                Model.SpeciesAttributeChoice speciesAttributeChoice = new Model.SpeciesAttributeChoice();
                                speciesAttributeChoice.Attribute = attribute;
                                speciesAttributeChoice.AttributeChoice = attributeChoice;
                                if (speciesAttributeChoicesMap.ContainsKey(attributeChoice.id))
                                {
                                    speciesAttributeChoice.IsUsed = true;
                                    speciesAttributeChoice.SpeciesAttributeChoiceValue = speciesAttributeChoicesMap[attributeChoice.id];
                                }
                                SpeciesAttributes.Add(speciesAttributeChoice);
                                speciesAttributeChoice.OnChanged += SpeciesAttribute_OnChanged;
                            }
                        }
                        break;
                    case Library.Database.AttributeType.MaximumSize:
                    case Library.Database.AttributeType.MinimumSize:
                        Model.SpeciesAttributeSize speciesAttributeSize = new Model.SpeciesAttributeSize();
                        speciesAttributeSize.Attribute = attribute;
                        if (speciesAttributeSizesMap.ContainsKey(attribute.id))
                        {
                            speciesAttributeSize.IsUsed = true;
                            speciesAttributeSize.SpeciesAttributeSizeValue = speciesAttributeSizesMap[attribute.id];
                        }
                        SpeciesAttributes.Add(speciesAttributeSize);
                        speciesAttributeSize.OnChanged += SpeciesAttribute_OnChanged;
                        break;
                }
            }
        }

        private void SpeciesAttribute_OnChanged(Model.ISpeciesAttributeValue sender)
        {
            if (sender is Model.SpeciesAttributeChoice)
            {
                Model.SpeciesAttributeChoice speciesAttributeChoice = sender as Model.SpeciesAttributeChoice;
                if (sender.IsUsed)
                {
                    speciesAttributeChoice.SpeciesAttributeChoiceValue = new Library.DBObject.SpeciesAttributeChoice();
                    speciesAttributeChoice.SpeciesAttributeChoiceValue.key_id = Key.id;
                    speciesAttributeChoice.SpeciesAttributeChoiceValue.species_id = Species.id;
                    speciesAttributeChoice.SpeciesAttributeChoiceValue.attributechoice_id = speciesAttributeChoice.AttributeChoice.id;
                    IKeyManager.Insert(speciesAttributeChoice.SpeciesAttributeChoiceValue);
                }
                else
                {
                    if (speciesAttributeChoice.SpeciesAttributeChoiceValue != null)
                    {
                        IKeyManager.Delete(speciesAttributeChoice.SpeciesAttributeChoiceValue);
                        speciesAttributeChoice.SpeciesAttributeChoiceValue = null;
                    }
                }
            }
            else if (sender is Model.SpeciesAttributeSize)
            {
                Model.SpeciesAttributeSize speciesAttributeSize = sender as Model.SpeciesAttributeSize;
                if (sender.IsUsed)
                {
                    if (speciesAttributeSize.SpeciesAttributeSizeValue != null)
                    {
                        IKeyManager.Update(speciesAttributeSize.SpeciesAttributeSizeValue);
                    }
                    else
                    {
                        speciesAttributeSize.SpeciesAttributeSizeValue = new Library.DBObject.SpeciesAttributeSize();
                        speciesAttributeSize.SpeciesAttributeSizeValue.key_id = Key.id;
                        speciesAttributeSize.SpeciesAttributeSizeValue.species_id = Species.id;
                        speciesAttributeSize.SpeciesAttributeSizeValue.attribute_id = speciesAttributeSize.Attribute.id;
                        speciesAttributeSize.SpeciesAttributeSizeValue.value = 1;
                        IKeyManager.Insert(speciesAttributeSize.SpeciesAttributeSizeValue);
                    }
                }
                else
                {
                    if (speciesAttributeSize.SpeciesAttributeSizeValue != null)
                    {
                        IKeyManager.Delete(speciesAttributeSize.SpeciesAttributeSizeValue);
                        speciesAttributeSize.SpeciesAttributeSizeValue = null;
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

        private List<Model.ISpeciesAttributeValue> _speciesAttributes;
        public List<Model.ISpeciesAttributeValue> SpeciesAttributes
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
