using System.Collections.Generic;
using System.Linq;

namespace MycoKeys.Application.ViewModel
{
    class SpeciesAttributesViewModel : OpenControls.Wpf.Utilities.ViewModel.BaseViewModel
    {
        public class SpeciesAttributeItem
        {
            public Library.DBObject.Attribute Attribute { get; set; }
            public Library.DBObject.SpeciesAttribute SpeciesAttribute;
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
            SpeciesAttributeItems = new List<SpeciesAttributeItem>();
            var map = IKeyManager.GetSpeciesAttributeEnumerator(Species.id).ToDictionary(n => n.attribute_id, n => n);
            foreach (var item in IKeyManager.GetKeyAttributeEnumerator(Key.id).OrderBy(n => n.position)) 
            {
                bool applied = map.ContainsKey(item.id);
                SpeciesAttributeItem speciesAttributeItem = new SpeciesAttributeItem() { Attribute = item, Applied = applied, SpeciesAttribute = applied ? map[item.id] : null };
                speciesAttributeItem.OnCheck += SpeciesAttributeItem_OnCheck;
                SpeciesAttributeItems.Add(speciesAttributeItem);
            }
        }

        private void SpeciesAttributeItem_OnCheck(SpeciesAttributeItem sender)
        {
            if (sender.Applied)
            {
                Library.DBObject.SpeciesAttribute speciesAttribute = new Library.DBObject.SpeciesAttribute();
                speciesAttribute.species_id = Species.id;
                speciesAttribute.attribute_id = sender.Attribute.id;
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
                return "Species Attributes Editor: " + Name;
            }
        }

        public string Name
        {
            get
            {
                return Species.name;
            }
        }

        private List<SpeciesAttributeItem> _speciesAttributeItems;
        public List<SpeciesAttributeItem> SpeciesAttributeItems
        {
            get
            {
                return _speciesAttributeItems;
            }
            set
            {
                _speciesAttributeItems = value;
                NotifyPropertyChanged("SpeciesAttributeItems");
            }
        }
    }
}
