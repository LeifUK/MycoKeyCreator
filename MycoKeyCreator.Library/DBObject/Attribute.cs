using System;
using PetaPoco.NetCore;

namespace MycoKeyCreator.Library.DBObject
{
    [TableName(Database.TableNames.Attribute)]
    public class Attribute : IObject
    {
        public Int64 id { get; set; }
        public Int64 key_id { get; set; }
        public string description { get; set; }
        public Int16 position { get; set; }
        public Int16 type { get; set; }
    }
}
