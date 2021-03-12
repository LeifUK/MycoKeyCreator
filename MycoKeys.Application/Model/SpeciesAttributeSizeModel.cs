using System;

namespace MycoKeys.Application.Model
{
    public class SpeciesAttributeSizeModel : SpeciesAttributeValueModel
    {
        public Library.DBObject.SpeciesAttributeSize SpeciesAttributeSize { get; set; }
        public override object Value
        {
            get
            {
                if (SpeciesAttributeSize != null)
                {
                    return SpeciesAttributeSize.value;
                }
                return null;
            }
            set
            {
                if (SpeciesAttributeSize != null)
                {
                    Int16 i = 0;
                    if (Int16.TryParse((string)value, out i) && (i >= 0))
                    {
                        SpeciesAttributeSize.value = i;
                        IsUsed = true;
                    }
                }
            }
        }
    }
}
