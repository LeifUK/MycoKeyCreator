using System.Collections.Generic;
using System;

namespace MycoKeys.WebApplication.Model
{
    public class KeyMatchViewModel
    {
        public class AttributeValueSelection
        {
            public MycoKeys.Library.DBObject.AttributeValue AttributeValue { get; set; }
            public bool IsSelected { get; set; }
        }

        public class AttributeSelection
        {
            public bool IsSelected { get; set; }
            public MycoKeys.Library.DBObject.Attribute Attribute { get; set; }
            public Int64 SelectedAttributeValueId { get; set; }
            public List<AttributeValueSelection> AttributeValueSelections { get; set; }
        }

        public string KeyName { get; set; }
        public string KeyTitle { get; set; }
        public string KeyDescription { get; set; }
        public string Copyright { get; set; }
        public List<AttributeSelection> AttributeSelections { get; set; }
        public List<SpeciesMatchData> Species { get; set; }
    }
}
