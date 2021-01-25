namespace MycoKeys.WebApplication.Model
{
    public class SpeciesMatchData
    {
        public MycoKeys.Library.DBObject.Species Species { get; set; }
        public int AttributeCount { get; set; }
        public int Matches { get; set; }
        public int Mismatches { get; set; }
    }
}
