using System;
using System.Collections.Generic;

namespace MycoKeys.Library.Database
{
    public interface ITable<T> where T : class
    {
        bool Exists();
        T Query(Int64 id);
        void Insert(T item);
        void Update(T item);
        void Delete(T item);
        IEnumerable<T> Enumerator { get; }
    }
}
