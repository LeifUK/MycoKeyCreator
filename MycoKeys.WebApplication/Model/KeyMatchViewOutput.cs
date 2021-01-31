using System;
using System.Collections.Generic;

namespace MycoKeys.WebApplication.Model
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
            public Int64 AttributeValueId { get; set; }
        }

        public string KeyName { get; set; }
        public List<Selection> AttributeSelections { get; set; }
    }
}
