using System;
using PetaPoco.NetCore;

namespace FungiKeys.Library.DBObject
{
    [TableName("attributechoices")]
    public class AttributeChoice : IObject
    {
        public Int64 id { get; set; }
        public Int64 attribute_id { get; set; }
        public string description { get; set; }
    }
}
