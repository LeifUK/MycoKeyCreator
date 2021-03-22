using System;
using System.Collections.Generic;

namespace MycoKeyMaker.WebApplication.Model
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
            public Int64 AttributeValue { get; set; }
        }

        public string KeyName { get; set; }
        public List<Selection> AttributeSelections { get; set; }
    }
}
