using System;

namespace MycoKeys.Application.Model
{
    public abstract class SpeciesAttributeValue: ISpeciesAttributeValue
    {
        public string Title 
        { 
            get 
            {
                return Attribute.description;
            } 
        }
        public Library.DBObject.Attribute Attribute { get; set; }
        private bool _isUsed;
        public bool IsUsed
        {
            get
            {
                return _isUsed;
            }
            set
            {
                _isUsed = value;
                OnChanged?.Invoke(this);
            }
        }
        public abstract object Value { get; set; }

        public event OnChangedHandler OnChanged;
    }
}
