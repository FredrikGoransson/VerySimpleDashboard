using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

namespace VerySimpleDashboard.Data.SqlStorage.DataAccess
{
    public class SqlDatabaseProxy : ISqlDatabaseProxy
    {
        public IDbConnection Connection { get; set; }

        public SqlDatabaseProxy(IDbConnection connection)
        {
            Connection = connection;
        }

        private void EnsureConnection()
        {
            if (Connection.State == ConnectionState.Closed)
                Connection.Open();
        }

        public IEnumerable<T> Query<T>(string sql)
        {
            EnsureConnection();

            var query = Connection.Query<T>(sql);
            return query.ToList();
        }

        public IEnumerable<T> Query<T>(string sql, object param)
        {
            EnsureConnection();

            var query = Connection.Query<T>(sql, param);
            return query.ToList();
        }

        public int Insert<T>(string sql, T item)
        {
            EnsureConnection();
            var sqlWithIdentitySelect = sql + @"; 
                GO
                select cast(SCOPE_IDENTITY() as int);";
            return Connection.Query<int>(sqlWithIdentitySelect, item).First();
        }

        public int Execute<T>(string sql, params T[] items)
        {
            EnsureConnection();
            return Connection.Execute(sql, items);
        }

        public int Execute(string sql)
        {
            EnsureConnection();
            return Connection.Execute(sql);
        }

    }
}