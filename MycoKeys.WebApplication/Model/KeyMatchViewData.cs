using System.Collections.Generic;

namespace MycoKeys.WebApplication.Model
{
    public class KeyMatchViewData
    {
        public string KeyName { get; set; }
        public string KeyTitle { get; set; }
        public string KeyDescription { get; set; }
        public string Copyright { get; set; }
        public Dictionary<MycoKeys.Library.DBObject.Attribute, bool> AttributeSelections { get; set; }
        public List<SpeciesMatchData> Species { get; set; }
    }
}
