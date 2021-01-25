using System;
using System.Collections.Generic;

namespace MycoKeys.Library.PetaPocoAdapter
{
    public class Table<T> where T : DBObject.IObject
    {
        public Table(SqlQueryBuilders.ISqlQueryBuilder iSqlQueryBuilder, PetaPoco.NetCore.Database database, string tableName)
        {
            _iSqlQueryBuilder = iSqlQueryBuilder;
            _database = database;
            _tableName = tableName;
        }

        protected readonly SqlQueryBuilders.ISqlQueryBuilder _iSqlQueryBuilder;
        protected readonly PetaPoco.NetCore.Database _database;
        protected readonly string _tableName;

        public T Query(Int64 id)
        {
            T dbObject = default(T);

            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("SELECT * FROM ");
            stringBuilder.Append(_tableName);
            stringBuilder.Append(" WHERE id=@0;");
            string queryText = stringBuilder.ToString();
            var iterator = _database.Query<T>(queryText, id);
            if (iterator.GetEnumerator().MoveNext())
            {
                dbObject = iterator.GetEnumerator().Current;
            }

            return dbObject;
        }

        public void Insert(T dbObject)
        {
            dbObject.id = System.Convert.ToInt64(_database.Insert(_tableName, "id", dbObject));
        }

        public void Update(T dbObject)
        {
            _database.Update(_tableName, "id", dbObject);
        }

        public void Delete(T dbObject)
        {
            _database.Delete(_tableName, "id", dbObject);
        }

        public IEnumerable<T> Enumerator
        {
            get
            {
                return _database.Query<T>("SELECT * FROM " + _tableName);
            }
        }

        // This won't work for all tables!
        public IEnumerable<T> GetEnumeratorForKey(Int64 key_id)
        {
            return _database.Query<T>(_iSqlQueryBuilder.SelectByKey(_tableName), key_id);
        }
    }
}
