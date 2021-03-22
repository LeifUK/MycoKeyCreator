using System;
using System.Collections.Generic;

namespace MycoKeyMaker.Library.Database
{
    public interface IAttributeTable : ITable<DBObject.Attribute>
    {
        IEnumerable<DBObject.Attribute> GetEnumeratorForKey(Int64 key_id);
        void DeleteByKey(Int64 key_id);
    }
}
