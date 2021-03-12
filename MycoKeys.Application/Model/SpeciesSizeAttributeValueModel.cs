using System;

namespace MycoKeys.Application.Model
{
    public class SpeciesSizeAttributeValueModel : SpeciesAttributeValueModel
    {
        public Library.DBObject.SpeciesSizeAttributeValue SpeciesSizeAttributeValue { get; set; }
        public override object Value
        {
            get
            {
                if (SpeciesSizeAttributeValue != null)
                {
                    return SpeciesSizeAttributeValue.value;
                }
                return null;
            }
            set
            {
                if (SpeciesSizeAttributeValue != null)
                {
                    Int16 i = 0;
                    if (Int16.TryParse((string)value, out i) && (i >= 0))
                    {
                        SpeciesSizeAttributeValue.value = i;
                        IsUsed = true;
                    }
                }
            }
        }
    }
}
