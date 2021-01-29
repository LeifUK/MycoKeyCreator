namespace MycoKeys.Application.ViewModel
{
    public class SpeciesAttributeValueModel
    {
        public Library.DBObject.Attribute Attribute { get; set; }
        public Library.DBObject.AttributeValue AttributeValue { get; set; }
        public Library.DBObject.SpeciesAttributeValue SpeciesAttributeValue { get; set; }
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
                OnCheck?.Invoke(this);
            }
        }

        public delegate void OnCheckHandler(SpeciesAttributeValueModel sender);
        public event OnCheckHandler OnCheck;
    }
}
