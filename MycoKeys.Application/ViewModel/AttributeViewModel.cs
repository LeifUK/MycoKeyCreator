using System.Linq;

namespace MycoKeys.Application.ViewModel
{
    internal class AttributeViewModel : OpenControls.Wpf.Utilities.ViewModel.BaseViewModel
    {
        public AttributeViewModel(MycoKeys.Library.DBObject.Attribute attribute)
        {
            _attribute = attribute;
            Text = attribute.description;
        }

        private MycoKeys.Library.DBObject.Attribute _attribute;

        public void Save()
        {
            _attribute.description = Text;
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
    }
}
