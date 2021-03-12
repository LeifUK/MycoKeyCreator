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

            Dictionary<Int64, Library.DBObject.SpeciesAttributeChoice> speciesAttributeChoicesMap =
                IKeyManager.GetSpeciesAttributeChoiceEnumerator(Species.id).ToDictionary(n => n.attributechoice_id, n => n);

            Dictionary<Int64, Library.DBObject.SpeciesAttributeSize> speciesAttributeSizesMap =
                IKeyManager.GetSpeciesSizeAttributeEnumerator(Species.id).ToDictionary(n => n.attribute_id, n => n);

            foreach (var attribute in attributes)
            {
                Library.Database.AttributeType attributeType = (Library.Database.AttributeType)attribute.type;
                switch (attributeType)
                {
                    case Library.Database.AttributeType.Choice:
                        {
                            List<Library.DBObject.AttributeChoice> attributeChoices = IKeyManager.GetAttributeChoiceEnumerator(attribute.id).OrderBy(n => n.position).ToList();
                            foreach (var attributeChoice in attributeChoices)
                            {
                                Model.SpeciesAttributeChoiceModel speciesAttributeChoiceModel = new Model.SpeciesAttributeChoiceModel();
                                speciesAttributeChoiceModel.Attribute = attribute;
                                speciesAttributeChoiceModel.AttributeChoice = attributeChoice;
                                if (speciesAttributeChoicesMap.ContainsKey(attributeChoice.id))
                                {
                                    speciesAttributeChoiceModel.IsUsed = true;
                                    speciesAttributeChoiceModel.SpeciesAttributeChoice = speciesAttributeChoicesMap[attributeChoice.id];
                                }
                                SpeciesAttributes.Add(speciesAttributeChoiceModel);
                                speciesAttributeChoiceModel.OnChanged += SpeciesAttribute_OnChanged;
                            }
                        }
                        break;
                    case Library.Database.AttributeType.MaximumSize:
                    case Library.Database.AttributeType.MinimumSize:
                        Model.SpeciesAttributeSizeModel speciesAttributeSizeModel = new Model.SpeciesAttributeSizeModel();
                        speciesAttributeSizeModel.Attribute = attribute;
                        if (speciesAttributeSizesMap.ContainsKey(attribute.id))
                        {
                            speciesAttributeSizeModel.IsUsed = true;
                            speciesAttributeSizeModel.SpeciesAttributeSize = speciesAttributeSizesMap[attribute.id];
                        }
                        SpeciesAttributes.Add(speciesAttributeSizeModel);
                        speciesAttributeSizeModel.OnChanged += SpeciesAttribute_OnChanged;
                        break;
                }
            }
        }

        private void SpeciesAttribute_OnChanged(Model.ISpeciesAttributeValueModel sender)
        {
            if (sender is Model.SpeciesAttributeChoiceModel)
            {
                Model.SpeciesAttributeChoiceModel speciesAttributeChoiceModel = sender as Model.SpeciesAttributeChoiceModel;
                if (sender.IsUsed)
                {
                    speciesAttributeChoiceModel.SpeciesAttributeChoice = new Library.DBObject.SpeciesAttributeChoice();
                    speciesAttributeChoiceModel.SpeciesAttributeChoice.key_id = Key.id;
                    speciesAttributeChoiceModel.SpeciesAttributeChoice.species_id = Species.id;
                    speciesAttributeChoiceModel.SpeciesAttributeChoice.attributechoice_id = speciesAttributeChoiceModel.AttributeChoice.id;
                    IKeyManager.Insert(speciesAttributeChoiceModel.SpeciesAttributeChoice);
                }
                else
                {
                    if (speciesAttributeChoiceModel.SpeciesAttributeChoice != null)
                    {
                        IKeyManager.Delete(speciesAttributeChoiceModel.SpeciesAttributeChoice);
                        speciesAttributeChoiceModel.SpeciesAttributeChoice = null;
                    }
                }
            }
            else if (sender is Model.SpeciesAttributeSizeModel)
            {
                Model.SpeciesAttributeSizeModel speciesAttributeSizeModel = sender as Model.SpeciesAttributeSizeModel;
                if (sender.IsUsed)
                {
                    if (speciesAttributeSizeModel.SpeciesAttributeSize != null)
                    {
                        IKeyManager.Update(speciesAttributeSizeModel.SpeciesAttributeSize);
                    }
                    else
                    {
                        speciesAttributeSizeModel.SpeciesAttributeSize = new Library.DBObject.SpeciesAttributeSize();
                        speciesAttributeSizeModel.SpeciesAttributeSize.key_id = Key.id;
                        speciesAttributeSizeModel.SpeciesAttributeSize.species_id = Species.id;
                        speciesAttributeSizeModel.SpeciesAttributeSize.attribute_id = speciesAttributeSizeModel.Attribute.id;
                        speciesAttributeSizeModel.SpeciesAttributeSize.value = 1;
                        IKeyManager.Insert(speciesAttributeSizeModel.SpeciesAttributeSize);
                    }
                }
                else
                {
                    if (speciesAttributeSizeModel.SpeciesAttributeSize != null)
                    {
                        IKeyManager.Delete(speciesAttributeSizeModel.SpeciesAttributeSize);
                        speciesAttributeSizeModel.SpeciesAttributeSize = null;
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
