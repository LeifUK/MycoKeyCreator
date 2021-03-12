using System.Collections.Generic;
using System;

namespace MycoKeys.WebApplication.Model
{
    public class KeyMatchViewModel
    {
        public KeyMatchViewModel()
        {
            Literature = new List<Library.DBObject.Literature>();
            AttributeSelections = new List<AttributeSelection>();
            Species = new List<SpeciesMatchData>();
        }

        public class AttributeSelection
        {
            public bool IsSelected { get; set; }
            public MycoKeys.Library.DBObject.Attribute Attribute { get; set; }
            public Int64 SelectedAttributeValueId { get; set; }
            public List<MycoKeys.Library.DBObject.AttributeChoice> AttributeChoices { get; set; }
        }

        public string KeyName { get; set; }
        public string KeyTitle { get; set; }
        public string KeyDescription { get; set; }
        public List<Library.DBObject.Literature> Literature { get; private set; }
        public string Copyright { get; set; }
        public List<AttributeSelection> AttributeSelections { get; private set; }
        public List<SpeciesMatchData> Species { get; set; }
    }
}
