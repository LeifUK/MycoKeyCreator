using System;
using System.Collections.Generic;

namespace MycoKeyCreator.Library.Database
{
    public interface IAttributeChoiceTable : ITable<DBObject.AttributeChoice>
    {
        IEnumerable<DBObject.AttributeChoice> GetEnumeratorForKey(Int64 key_id);
        IEnumerable<DBObject.AttributeChoice> GetEnumeratorForAttribute(Int64 attribute_id);
        void DeleteByKey(Int64 key_id);
    }
}
