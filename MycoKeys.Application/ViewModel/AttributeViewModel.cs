using System.Linq;
using System.Collections.ObjectModel;

namespace MycoKeys.Application.ViewModel
{
    internal class AttributeViewModel : OpenControls.Wpf.Utilities.ViewModel.BaseViewModel
    {
        public AttributeViewModel(Library.Database.IKeyManager keyManager, Library.DBObject.Key key, Library.DBObject.Attribute attribute)
        {
            _keyManager = keyManager;
            _key = key;
            _attribute = attribute;
            Text = attribute.description;
            LoadValues();
        }

        public void LoadValues()
        {
            AttributeValues = new ObservableCollection<Library.DBObject.AttributeValue>(
                _keyManager.GetAttributeValueEnumerator(_attribute.id).OrderBy(n => n.position));
            AssignPositions();
            if (AttributeValues.Count > 0)
            {
                SelectedAttributeValue = AttributeValues[0];
            }
        }

        public void Refresh()
        {
            int index = (SelectedAttributeValue != null) ? SelectedAttributeValue.position : -1;
            AttributeValues = new ObservableCollection<Library.DBObject.AttributeValue>(AttributeValues.OrderBy(n => n.position));
            if (index != -1)
            {
                SelectedAttributeValue = AttributeValues[index];
            }
        }

        private Library.Database.IKeyManager _keyManager;
        private Library.DBObject.Key _key;
        private Library.DBObject.Attribute _attribute;

        public void Save()
        {
            _attribute.description = Text;
            if (_attribute.id == 0)
            {
                _attribute.key_id = _key.id;
                _keyManager.Insert(_attribute, AttributeValues.ToList());
            }
            else
            {
                _keyManager.Update(_attribute, AttributeValues.ToList());
            }
        }

        private ObservableCollection<Library.DBObject.AttributeValue> _attributeValues;
        public ObservableCollection<Library.DBObject.AttributeValue> AttributeValues
        {
            get
            {
                return _attributeValues;
            }
            set
            {
                _attributeValues = value;
                NotifyPropertyChanged("AttributeValues");
            }
        }

        private Library.DBObject.AttributeValue _selectedAttributeValue;
        public Library.DBObject.AttributeValue SelectedAttributeValue
        {
            get
            {
                return _selectedAttributeValue;
            }
            set
            {
                _selectedAttributeValue = value;
                NotifyPropertyChanged("SelectedAttributeValue");
                NotifyPropertyChanged("CanMoveValueUp");
                NotifyPropertyChanged("CanMoveValueDown");
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
            for (int i = 0; i < AttributeValues.Count; ++i)
            {
                AttributeValues[i].position = (short)i;
            }
        }

        public void Add(Library.DBObject.AttributeValue attributeValue)
        {
            int index = SelectedAttributeValue != null ? SelectedAttributeValue.position : 0;
            if (index < AttributeValues.Count)
            {
                ++index;
            }
            _keyManager.Insert(attributeValue);
            AttributeValues.Insert(index, attributeValue);
            AssignPositions();
            SelectedAttributeValue = AttributeValues[index];
        }

        public void DeleteSelectedValue()
        {
            if (SelectedAttributeValue != null)
            {
                _keyManager.Delete(SelectedAttributeValue);
                AttributeValues.Remove(SelectedAttributeValue);
                AssignPositions();
            }
        }

        public bool CanMoveValueUp
        {
            get
            {
                if (SelectedAttributeValue == null)
                {
                    return false;
                }

                int index = AttributeValues.IndexOf(SelectedAttributeValue);
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
                if (SelectedAttributeValue == null)
                {
                    return false;
                }

                int index = AttributeValues.IndexOf(SelectedAttributeValue);
                return index < (AttributeValues.Count - 1);
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

            int index = AttributeValues.IndexOf(SelectedAttributeValue);
            AttributeValues[index].position = (short)(index - 1);
            AttributeValues[index - 1].position = (short)index;
            _keyManager.Update(AttributeValues[index - 1]);
            _keyManager.Update(AttributeValues[index]);
            AttributeValues = new ObservableCollection<Library.DBObject.AttributeValue>(AttributeValues.OrderBy(n => n.position));
            SelectedAttributeValue = AttributeValues[index - 1];
        }

        public void MoveSelectedAttributeDown()
        {
            if (!CanMoveValueDown)
            {
                return;
            }

            int index = AttributeValues.IndexOf(SelectedAttributeValue);
            AttributeValues[index].position = (short)(index + 1);
            AttributeValues[index + 1].position = (short)index;
            _keyManager.Update(AttributeValues[index]);
            _keyManager.Update(AttributeValues[index + 1]);
            AttributeValues = new ObservableCollection<Library.DBObject.AttributeValue>(AttributeValues.OrderBy(n => n.position));
            SelectedAttributeValue = AttributeValues[index + 1];
        }
    }
}
