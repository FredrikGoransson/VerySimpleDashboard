using System.Collections.Generic;

namespace VerySimpleDashboard.Data.SqlStorage.DataAccess
{
    public interface ISqlDatabaseProxy
    {
        IEnumerable<T> Query<T>(string sql);
        IEnumerable<T> Query<T>(string sql, object param);
        int Execute<T>(string sql, params T[] items);
        int Execute(string sql);
        int Insert<T>(string sql, T item);
    }
}