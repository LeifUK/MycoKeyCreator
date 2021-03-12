using System;
using PetaPoco.NetCore;

namespace MycoKeys.Library.DBObject
{
    [TableName(Database.TableNames.AttributeChoice)]
    public class AttributeChoice : IObject
    {
        public Int64 id { get; set; }
        public Int64 key_id { get; set; }
        public Int64 attribute_id { get; set; }
        public string description { get; set; }
        public Int16 position { get; set; }
    }
}
