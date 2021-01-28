using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MycoKeys.Application.ViewModel
{
    public class SpeciesAttributeValue
    {
        public Library.DBObject.Attribute Attribute { get; set; }
        public Library.DBObject.AttributeValue AttributeValue { get; set; }
        public bool IsUsed { get; set; }
    }
}
