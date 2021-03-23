using System.Collections.Generic;

namespace MycoKeyCreator.WebApplication.Model
{
    public class SpeciesMatchData
    {
        public MycoKeyCreator.Library.DBObject.Species Species { get; set; }
        public int AttributeCount { get; set; }
        public int Matches { get; set; }
        public int Mismatches { get; set; }
        public List<string> MismatchedFeatures { get; set; }
    }
}
