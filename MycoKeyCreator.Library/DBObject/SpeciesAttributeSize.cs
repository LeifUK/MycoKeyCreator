using System;
using PetaPoco.NetCore;

namespace MycoKeyCreator.Library.DBObject
{
    [TableName(Database.TableNames.SpeciesAttributeSize)]
    public class SpeciesAttributeSize : IObject
    {
        public Int64 id { get; set; }
        public Int64 key_id { get; set; }
        public Int64 species_id { get; set; }
        public Int64 attribute_id { get; set; }
        public Int16 value { get; set; }
    }
}
