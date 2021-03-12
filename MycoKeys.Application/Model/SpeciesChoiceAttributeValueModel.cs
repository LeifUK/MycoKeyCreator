using System;

namespace MycoKeys.Application.Model
{
    public class SpeciesChoiceAttributeValueModel : SpeciesAttributeValueModel
    {
        public Library.DBObject.AttributeValue AttributeValue { get; set; }
        public Library.DBObject.SpeciesAttributeValue SpeciesAttributeValue { get; set; }
        public override object Value
        {
            get
            {
                return AttributeValue.description;
            }
            set
            {
                // Not editable
            }
        }
    }
}
