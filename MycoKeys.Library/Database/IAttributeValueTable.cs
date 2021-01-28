using System;
using System.Collections.Generic;

namespace MycoKeys.Library.Database
{
    public interface IAttributeValueTable : ITable<DBObject.AttributeValue>
    {
        IEnumerable<DBObject.AttributeValue> GetEnumeratorForKey(Int64 key_id);
        IEnumerable<DBObject.AttributeValue> GetEnumeratorForAttribute(Int64 attribute_id);
        void DeleteByKey(Int64 key_id);
    }
}
