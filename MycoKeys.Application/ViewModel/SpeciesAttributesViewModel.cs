using System.Collections.Generic;

namespace MycoKeys.Application.ViewModel
{
    class SpeciesAttributesViewModel : OpenControls.Wpf.Utilities.ViewModel.BaseViewModel
    {
        public class SpeciesAttributeItem
        {
            public Library.DBObject.Attribute Attribute { get; set; }
            public Library.DBObject.SpeciesAttributeValue SpeciesAttribute;
            private bool _applied;
            public bool Applied 
            { 
                get
                {
                    return _applied;
                }
                set
                {
                    _applied = value;
                    OnCheck?.Invoke(this);
                }
            }

            public delegate void OnCheckHandler(SpeciesAttributeItem sender);

            public event OnCheckHandler OnCheck;
        }

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
            SpeciesAttributeValues = new List<SpeciesAttributeValue>();

            List<Library.DBObject.Attribute> attributes = new List<Library.DBObject.Attribute>(
                IKeyManager.GetKeyAttributeEnumerator(Species.key_id));

            foreach (var attribute in attributes)
            {
                foreach (var item in IKeyManager.GetAttributeValueEnumerator(attribute.id))
                {
                    SpeciesAttributeValue speciesAttributeValue = new SpeciesAttributeValue();
                    speciesAttributeValue.Attribute = attribute;
                    speciesAttributeValue.AttributeValue = item;
                    SpeciesAttributeValues.Add(speciesAttributeValue);
                }
            }
        }

        private void SpeciesAttributeItem_OnCheck(SpeciesAttributeItem sender)
        {
            if (sender.Applied)
            {
                Library.DBObject.SpeciesAttributeValue speciesAttribute = new Library.DBObject.SpeciesAttributeValue();
                speciesAttribute.key_id = Key.id;
                speciesAttribute.species_id = Species.id;
                speciesAttribute.attributevalue_id = sender.Attribute.id;
                IKeyManager.Insert(speciesAttribute);
            }
            else
            {
                if (sender.SpeciesAttribute != null)
                {
                    IKeyManager.Delete(sender.SpeciesAttribute);
                    sender.SpeciesAttribute = null;
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

        private List<SpeciesAttributeValue> _speciesAttributeValues;
        public List<SpeciesAttributeValue> SpeciesAttributeValues
        {
            get
            {
                return _speciesAttributeValues;
            }
            set
            {
                _speciesAttributeValues = value;
                NotifyPropertyChanged("SpeciesAttributeValues");
            }
        }
    }
}
