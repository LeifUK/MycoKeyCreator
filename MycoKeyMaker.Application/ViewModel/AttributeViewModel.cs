using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MycoKeys.Application.ViewModel
{
    internal class AttributeViewModel : OpenControls.Wpf.Utilities.ViewModel.BaseViewModel
    {
        public AttributeViewModel(Library.Database.IKeyManager keyManager, Library.DBObject.Key key, Library.DBObject.Attribute attribute)
        {
            _iKeyManager = keyManager;
            _key = key;
            _attribute = attribute;
            Text = attribute.description;
            LoadValues();
        }

        public void LoadValues()
        {
            AttributeChoices = new ObservableCollection<Library.DBObject.AttributeChoice>(
                _iKeyManager.GetAttributeChoiceEnumeratorForAttribute(_attribute.id).OrderBy(n => n.position));
            AssignPositions();
            if (AttributeChoices.Count > 0)
            {
                SelectedAttributeChoice = AttributeChoices[0];
            }
        }

        public void Refresh()
        {
            int index = (SelectedAttributeChoice != null) ? SelectedAttributeChoice.position : -1;
            AttributeChoices = new ObservableCollection<Library.DBObject.AttributeChoice>(AttributeChoices.OrderBy(n => n.position));
            if (index != -1)
            {
                SelectedAttributeChoice = AttributeChoices[index];
            }
        }

        private Library.Database.IKeyManager _iKeyManager;
        private Library.DBObject.Key _key;
        private Library.DBObject.Attribute _attribute;

        public void Save()
        {
            _attribute.description = Text;
            if (_attribute.id == 0)
            {
                _attribute.key_id = _key.id;
                _iKeyManager.Insert(_attribute, AttributeChoices.ToList());
            }
            else
            {
                _iKeyManager.Update(_attribute, AttributeChoices.ToList());
            }
        }

        private ObservableCollection<Library.DBObject.AttributeChoice> _attributeChoices;
        public ObservableCollection<Library.DBObject.AttributeChoice> AttributeChoices
        {
            get
            {
                return _attributeChoices;
            }
            set
            {
                _attributeChoices = value;
                NotifyPropertyChanged("AttributeChoices");
            }
        }

        private Library.DBObject.AttributeChoice _selectedAttributeChoice;
        public Library.DBObject.AttributeChoice SelectedAttributeChoice
        {
            get
            {
                return _selectedAttributeChoice;
            }
            set
            {
                _selectedAttributeChoice = value;
                NotifyPropertyChanged("SelectedAttributeChoice");
                NotifyPropertyChanged("CanMoveValueUp");
                NotifyPropertyChanged("CanMoveValueDown");

                if (_selectedAttributeChoice == null)
                {
                    AssociatedSpecies = null;
                }
                else
                {
                    AssociatedSpecies = _iKeyManager.GetSpeciesEnumeratorForAttributeChoice(_key.id, value.id).Select(n => n.name).ToList();
                }
            }
        }

        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                NotifyPropertyChanged("Text");
            }
        }

        private void AssignPositions()
        {
            for (int i = 0; i < AttributeChoices.Count; ++i)
            {
                AttributeChoices[i].position = (short)i;
            }
        }

        public void Add(Library.DBObject.AttributeChoice attributeValue)
        {
            int index = SelectedAttributeChoice != null ? SelectedAttributeChoice.position : 0;
            if (index < AttributeChoices.Count)
            {
                ++index;
            }
            _iKeyManager.Insert(attributeValue);
            AttributeChoices.Insert(index, attributeValue);
            AssignPositions();
            SelectedAttributeChoice = AttributeChoices[index];
        }

        public void DeleteSelectedValue()
        {
            if (SelectedAttributeChoice != null)
            {
                _iKeyManager.Delete(SelectedAttributeChoice);
                AttributeChoices.Remove(SelectedAttributeChoice);
                AssignPositions();
            }
        }

        public bool CanMoveValueUp
        {
            get
            {
                if (SelectedAttributeChoice == null)
                {
                    return false;
                }

                int index = AttributeChoices.IndexOf(SelectedAttributeChoice);
                return index > 0;
            }
            set
            {
                NotifyPropertyChanged("CanMoveValueUp");
            }
        }

        public bool CanMoveValueDown
        {
            get
            {
                if (SelectedAttributeChoice == null)
                {
                    return false;
                }

                int index = AttributeChoices.IndexOf(SelectedAttributeChoice);
                return index < (AttributeChoices.Count - 1);
            }
            set
            {
                NotifyPropertyChanged("CanMoveValueDown");
            }
        }

        public void MoveSelectedAttributeUp()
        {
            if (!CanMoveValueUp)
            {
                return;
            }

            int index = AttributeChoices.IndexOf(SelectedAttributeChoice);
            AttributeChoices[index].position = (short)(index - 1);
            AttributeChoices[index - 1].position = (short)index;
            _iKeyManager.Update(AttributeChoices[index - 1]);
            _iKeyManager.Update(AttributeChoices[index]);
            AttributeChoices = new ObservableCollection<Library.DBObject.AttributeChoice>(AttributeChoices.OrderBy(n => n.position));
            SelectedAttributeChoice = AttributeChoices[index - 1];
        }

        public void MoveSelectedAttributeDown()
        {
            if (!CanMoveValueDown)
            {
                return;
            }

            int index = AttributeChoices.IndexOf(SelectedAttributeChoice);
            AttributeChoices[index].position = (short)(index + 1);
            AttributeChoices[index + 1].position = (short)index;
            _iKeyManager.Update(AttributeChoices[index]);
            _iKeyManager.Update(AttributeChoices[index + 1]);
            AttributeChoices = new ObservableCollection<Library.DBObject.AttributeChoice>(AttributeChoices.OrderBy(n => n.position));
            SelectedAttributeChoice = AttributeChoices[index + 1];
        }

        List<string> _associatedSpecies;
        public List<string> AssociatedSpecies
        {
            get
            {
                return _associatedSpecies;
            }
            set
            {
                _associatedSpecies = value;
                NotifyPropertyChanged("AssociatedSpecies");
            }
        }
    }
}
