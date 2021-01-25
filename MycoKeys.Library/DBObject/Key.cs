using System;
using PetaPoco.NetCore;

namespace MycoKeys.Library.DBObject
{
    [TableName(Database.TableNames.Key)]
    public class Key : IObject
    {
        public Int64 id { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string copyright { get; set; }
    }
}
