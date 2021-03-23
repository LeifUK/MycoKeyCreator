using System;
using PetaPoco.NetCore;

namespace MycoKeyCreator.Library.DBObject
{
    [TableName(Database.TableNames.Literature)]
    public class Literature : IObject
    {
        public Int64 id { get; set; }
        public Int64 key_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }
}