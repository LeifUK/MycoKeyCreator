using System;
using System.Linq;
using System.Collections.Generic;

namespace MycoKeys.Application.ViewModel
{
    public class KeyViewModel : OpenControls.Wpf.Utilities.ViewModel.BaseViewModel
    {
        public KeyViewModel(MycoKeys.Library.Database.IKeyManager iKeyManager, MycoKeys.Library.DBObject.Key key)
        {
            IKeyManager = iKeyManager;
            Key = key;
            LoadHeader();
            LoadSpecies();
            LoadAttributes();
        }

        public void LoadHeader()
        {
            Name = Key.name;
            Title = Key.title;
            Description = Key.description;
            Copyright = Key.copyright;
            Publish = Key.Publish;
        }

        public void LoadSpecies()
        {
            Species = new System.Collections.ObjectModel.ObservableCollection<Library.DBObject.Species>(
                IKeyManager.GetKeySpeciesEnumerator(Key.id).OrderBy(n => n.name));
            SelectedSpecies = Species.Count > 0 ? Species[0] : null;
        }

        public void LoadAttributes()
        {
            Attributes = new System.Collections.ObjectModel.ObservableCollection<Library.DBObject.Attribute>(
                IKeyManager.GetKeyAttributeEnumerator(Key.id).OrderBy(n => n.position));
            SelectedAttribute = Attributes.Count > 0 ? Attributes[0] : null;
        }

        public void Save()
        {
            Key.name = Name;
            Key.title = Title;
            Key.description = Description;
            Key.copyright = Copyright;
            Key.Publish = Publish;
            IKeyManager.Update(Key);
        }

        public readonly MycoKeys.Library.Database.IKeyManager IKeyManager;

        public readonly MycoKeys.Library.DBObject.Key Key;

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                NotifyPropertyChanged("Description");
            }
        }

        private string _copyright;
        public string Copyright
        {
            get
            {
                return _copyright;
            }
            set
            {
                _copyright = value;
                NotifyPropertyChanged("Copyright");
            }
        }

        private bool _publish;
        public bool Publish
        {
            get
            {
                return _publish;
            }
            set
            {
                _publish = value;
                NotifyPropertyChanged("Publish");
            }
        }

        private System.Collections.ObjectModel.ObservableCollection<MycoKeys.Library.DBObject.Species> _species;
        public System.Collections.ObjectModel.ObservableCollection<MycoKeys.Library.DBObject.Species> Species
        {
            get
            {
                return _species;
            }
            set
            {
                _species = value;
                NotifyPropertyChanged("Species");
            }
        }

        private MycoKeys.Library.DBObject.Species _selectedSpecies;
        public MycoKeys.Library.DBObject.Species SelectedSpecies
        {
            get
            {
                return _selectedSpecies;
            }
            set
            {
                _selectedSpecies = value;
                NotifyPropertyChanged("SelectedSpecies");
            }
        }

        public void DeleteSelectedSpecies()
        {
            if ((SelectedSpecies != null) && IKeyManager.Delete(SelectedSpecies))
            {
                Species.Remove(SelectedSpecies);
            }
        }

        private System.Collections.ObjectModel.ObservableCollection<MycoKeys.Library.DBObject.Attribute> _attributes;
        public System.Collections.ObjectModel.ObservableCollection<MycoKeys.Library.DBObject.Attribute> Attributes
        {
            get
            {
                return _attributes;
            }
            set
            {
                _attributes = value;
                NotifyPropertyChanged("Attributes");
            }
        }

        private MycoKeys.Library.DBObject.Attribute _selectedAttribute;
        public MycoKeys.Library.DBObject.Attribute SelectedAttribute
        {
            get
            {
                return _selectedAttribute;
            }
            set
            {
                _selectedAttribute = value;
                NotifyPropertyChanged("SelectedAttribute");
                NotifyPropertyChanged("CanMoveAttributeUp");
                NotifyPropertyChanged("CanMoveAttributeDown");
            }
        }

        public void Insert(Library.DBObject.Attribute attribute)
        {
            attribute.key_id = Key.id;
            attribute.position = (short)Attributes.Count;
            IKeyManager.Insert(attribute);
            Attributes.Add(attribute);
            SelectedAttribute = attribute;
        }

        public void Update(Library.DBObject.Attribute attribute)
        {
            IKeyManager.Update(attribute);
            SelectedAttribute = attribute;
            Int64 id = attribute.id;
            LoadAttributes();
            SelectedAttribute = Attributes.Where(n => n.id == id).FirstOrDefault();
        }

        public void DeleteSelectedAttribute()
        {
            if ((SelectedAttribute != null) && IKeyManager.Delete(SelectedAttribute))
            {
                Attributes.Remove(SelectedAttribute);
            }
        }

        public bool CanMoveAttributeUp
        {
            get
            {
                if (SelectedAttribute == null)
                {
                    return false;
                }

                int index = Attributes.IndexOf(SelectedAttribute);
                return index > 0;
            }
            set
            {
                NotifyPropertyChanged("CanMoveAttributeUp");
            }
        }

        public bool CanMoveAttributeDown
        {
            get
            {
                if (SelectedAttribute == null)
                {
                    return false;
                }

                int index = Attributes.IndexOf(SelectedAttribute);
                return index < (Attributes.Count - 1);
            }
            set
            {
                NotifyPropertyChanged("CanMoveAttributeDown");
            }
        }

        public void MoveSelectedAttributeUp()
        {
            if (!CanMoveAttributeUp)
            {
                return;
            }

            int index = Attributes.IndexOf(SelectedAttribute);
            SelectedAttribute.position = (short)(index - 1);
            Attributes[index - 1].position = (short)index;
            IKeyManager.Update(Attributes[index - 1]);
            IKeyManager.Update(Attributes[index]);
            LoadAttributes();
            SelectedAttribute = Attributes[index - 1];
        }

        public void MoveSelectedAttributeDown()
        {
            if (!CanMoveAttributeDown)
            {
                return;
            }

            int index = Attributes.IndexOf(SelectedAttribute);
            SelectedAttribute.position = (short)(index + 1);
            Attributes[index + 1].position = (short)index;
            IKeyManager.Update(Attributes[index]);
            IKeyManager.Update(Attributes[index + 1]);
            LoadAttributes();
            SelectedAttribute = Attributes[index + 1];
        }
    }
}
