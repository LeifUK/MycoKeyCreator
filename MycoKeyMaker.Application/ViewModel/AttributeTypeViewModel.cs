using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MycoKeyMaker.Application.ViewModel
{
    internal class AttributeTypeViewModel : OpenControls.Wpf.Utilities.ViewModel.BaseViewModel
    {
        // Warning warning => improve
        public AttributeTypeViewModel()
        {
            AttributeTypes = new ObservableCollection<KeyValuePair<string, Library.Database.AttributeType>>();
            AttributeTypes.Add(new KeyValuePair<string, Library.Database.AttributeType>("Choice", Library.Database.AttributeType.Choice));
            AttributeTypes.Add(new KeyValuePair<string, Library.Database.AttributeType>("Minimum Size", Library.Database.AttributeType.MinimumSize));
            AttributeTypes.Add(new KeyValuePair<string, Library.Database.AttributeType>("Maximum Size", Library.Database.AttributeType.MaximumSize));
            SelectedAttributeType = AttributeTypes[0];
        }

        private ObservableCollection<KeyValuePair<string, Library.Database.AttributeType>> _attributeTypes;
        public ObservableCollection<KeyValuePair<string, Library.Database.AttributeType>> AttributeTypes
        {
            get
            {
                return _attributeTypes;
            }
            set
            {
                _attributeTypes = value;
                NotifyPropertyChanged("AttributeTypes");
            }
        }

        private KeyValuePair<string, Library.Database.AttributeType> _selectedAttributeType;
        public KeyValuePair<string, Library.Database.AttributeType> SelectedAttributeType
        {
            get
            {
                return _selectedAttributeType;
            }
            set
            {
                _selectedAttributeType = value;
                NotifyPropertyChanged("SelectedAttributeType");
            }
        }
    }
}
