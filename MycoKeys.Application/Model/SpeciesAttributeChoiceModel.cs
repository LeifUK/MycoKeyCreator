using System;

namespace MycoKeys.Application.Model
{
    public class SpeciesAttributeChoiceModel : SpeciesAttributeValueModel
    {
        public Library.DBObject.AttributeChoice AttributeChoice { get; set; }
        public Library.DBObject.SpeciesAttributeChoice SpeciesAttributeChoice { get; set; }
        public override object Value
        {
            get
            {
                return AttributeChoice.description;
            }
            set
            {
                // Not editable
            }
        }
    }
}
