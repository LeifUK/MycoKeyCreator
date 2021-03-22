using System;

namespace MycoKeyMaker.Application.Model
{
    public class SpeciesAttributeChoice : SpeciesAttributeValue
    {
        public Library.DBObject.AttributeChoice AttributeChoice { get; set; }
        public Library.DBObject.SpeciesAttributeChoice SpeciesAttributeChoiceValue { get; set; }
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
