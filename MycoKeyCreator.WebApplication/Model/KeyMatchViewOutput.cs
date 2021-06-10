using System;
using System.Collections.Generic;

namespace MycoKeyCreator.WebApplication.Model
{
    public class KeyMatchViewOutput
    {
        public KeyMatchViewOutput()
        {
            AttributeSelections = new List<Selection>();
        }

        public class Selection
        {
            public bool IsSelected { get; set; }
            public Int64 AttributeId { get; set; }
            public Int16 AttributeType { get; set; }
            public float AttributeValue { get; set; }
        }

        public string KeyName { get; set; }
        public List<Selection> AttributeSelections { get; set; }
    }
}
