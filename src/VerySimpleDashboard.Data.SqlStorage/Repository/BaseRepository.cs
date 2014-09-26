using System.Collections.Generic;
using System.Linq;

namespace VerySimpleDashboard.Data.SqlStorage.Repository
{
    public static class RepositoryExtension
    {
        public static string CreateWhereInList<T>
            (this IEntityRepository<T> repository, IEnumerable<string> ids)
            where T : class
        {
            return ids.Select(id => "'" + id + "'").Aggregate((result, id) => result + ", " + id);
        }
    }
}