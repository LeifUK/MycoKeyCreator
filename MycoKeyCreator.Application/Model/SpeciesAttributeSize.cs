using System;

namespace MycoKeyCreator.Application.Model
{
    public class SpeciesAttributeSize : SpeciesAttributeValue
    {
        public Library.DBObject.SpeciesAttributeSize SpeciesAttributeSizeValue { get; set; }
        public override object Value
        {
            get
            {
                if (SpeciesAttributeSizeValue != null)
                {
                    return (_previousValue != null) ? (object)_previousValue : SpeciesAttributeSizeValue.value;
                }
                return null;
            }
            set
            {
                if (SpeciesAttributeSizeValue != null)
                {
                    float i = 0;
                    if (float.TryParse((string)value, out i) && (i >= 0))
                    {
                        SpeciesAttributeSizeValue.value = i;
                        IsUsed = true;
                        _previousValue = (string)value;
                        if (i == SpeciesAttributeSizeValue.value)
                        {
                            _previousValue = (string)value;
                        }
                        else
                        {
                            _previousValue = i.ToString();
                        }
                    }
                }
            }
        }

        // This allows the user to enter a decimal at the end without it being removed eg "35."
        private string _previousValue = null;
    }
}
