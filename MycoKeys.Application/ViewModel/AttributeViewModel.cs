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
            AttributeValues = new ObservableCollection<Library.DBObject.AttributeValue>(_keyManager.GetAttributeValueEnumerator(_attribute.id));
            if (AttributeValues.Count > 0)
            {
                SelectedAttributeValue = AttributeValues[0];
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
                NotifyPropertyChanged("Values");
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

        public void DeleteSelectedValue()
        {
            if (SelectedAttributeValue != null)
            {
                AttributeValues.Remove(SelectedAttributeValue);
            }
        }
    }
}
