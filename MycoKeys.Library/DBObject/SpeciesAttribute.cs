using System;
using PetaPoco.NetCore;

namespace MycoKeys.Library.DBObject
{
    [TableName(Database.TableNames.SpeciesAttribute)]
    public class SpeciesAttribute : IObject
    {
        public Int64 id { get; set; }
        public Int64 attribute_id { get; set; }
        public Int64 species_id { get; set; }
    }
}
