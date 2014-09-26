using System.Collections.Generic;

namespace VerySimpleDashboard.Data
{
    public interface IEntityRepository<T> where T : class
    {
        IEnumerable<string> GetIds();
        IEnumerable<T> Load(params string[] ids);
        int Insert(IEnumerable<T> rowsToInsert);
        int Update(IEnumerable<T> rowsToUpdate);
        void DeleteAllRows();
    }
}