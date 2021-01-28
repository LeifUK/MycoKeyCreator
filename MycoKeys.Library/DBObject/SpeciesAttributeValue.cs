using System;
using PetaPoco.NetCore;

namespace MycoKeys.Library.DBObject
{
    [TableName(Database.TableNames.SpeciesAttributeValue)]
    public class SpeciesAttributeValue : IObject
    {
        public Int64 id { get; set; }
        public Int64 key_id { get; set; }
        public Int64 attributevalue_id { get; set; }
        public Int64 species_id { get; set; }
    }
}
