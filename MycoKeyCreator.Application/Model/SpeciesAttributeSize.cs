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
                    return SpeciesAttributeSizeValue.value;
                }
                return null;
            }
            set
            {
                if (SpeciesAttributeSizeValue != null)
                {
                    Int16 i = 0;
                    if (Int16.TryParse((string)value, out i) && (i >= 0))
                    {
                        SpeciesAttributeSizeValue.value = i;
                        IsUsed = true;
                    }
                }
            }
        }
    }
}
