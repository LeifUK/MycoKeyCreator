namespace MycoKeys.WebApplication.Model
{
    public class KeyItem
    {
        public int Number { get; set; }
        public int ParentNumber { get; set; }
        public KeyItem ParentKeyItem { get; set; }
        public string Attribute { get; set; }
        public string Species { get; set; }
        public KeyItem ChildKeyItem { get; set; }
    }
}
