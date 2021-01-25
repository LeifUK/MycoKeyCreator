using System.Collections.Generic;

namespace MycoKeys.WebApplication.Model
{
    public class KeyNode
    {
        public KeyNode()
        {
            Items = new List<KeyNode>();
        }

        public Library.DBObject.Attribute Attribute { get; set; }
        public List<KeyNode> Items { get; set; }
    }
}
