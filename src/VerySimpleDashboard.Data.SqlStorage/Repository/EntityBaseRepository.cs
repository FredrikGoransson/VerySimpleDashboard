using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using VerySimpleDashboard.Data.SqlStorage.DataAccess;

namespace VerySimpleDashboard.Data.SqlStorage.Repository
{
    public abstract class EntityRepositoryBase<T> : IEntityRepository<T> where T : class
    {
        protected ISqlDatabaseProxy DatabaseProxy { get; set; }
        private readonly DateTime _minDate;
        private readonly string _table;
        private readonly string _insertSql;
        private readonly string _updateSql;

        protected EntityRepositoryBase(ISqlDatabaseProxy databaseProxy, string table, string insertSql, string updateSql)
        {
            _table = table;
            _insertSql = insertSql;
            _updateSql = updateSql;
            DatabaseProxy = databaseProxy;
            _minDate = DateTime.ParseExact("1753-01-01", "yyyy-MM-dd", CultureInfo.CurrentCulture);
        }

        public IEnumerable<string> GetIds()
        {
            try
            {
                var ids = DatabaseProxy.Query<string>(string.Format("select Id from {0}", _table)).ToList();
                return ids;
            }
            catch (SqlException sqlException)
            {
                throw RepositoryException.DatabaseError(sqlException);
            }
            catch (Exception exception)
            {
                throw RepositoryException.GeneralError(exception);
            }
        }

        public IEnumerable<T> Load(params string[] ids)
        {
            try
            {
                if (ids == null || !ids.Any()) return DatabaseProxy.Query<T>(string.Format("select * from {0}", _table));

                var idList = this.CreateWhereInList(ids);
                return DatabaseProxy.Query<T>(string.Format("select * from {0} where Id in ({1})", _table, idList));
            }
            catch (SqlException sqlException)
            {
                throw RepositoryException.DatabaseError(sqlException);
            }
            catch (Exception exception)
            {
                throw RepositoryException.GeneralError(exception);
            }
        }

        public int Insert(IEnumerable<T> rowsToInsert)
        {
            if (rowsToInsert == null) throw new ArgumentNullException("rowsToInsert");
            try
            {
                var array = rowsToInsert.ToArray();
                var count = DatabaseProxy.Execute(_insertSql, array);
                return count;
            }
            catch (SqlException sqlException)
            {
                throw RepositoryException.DatabaseError(sqlException);
            }
            catch (Exception exception)
            {
                throw RepositoryException.GeneralError(exception);
            }
        }

        public int Update(IEnumerable<T> rowsToUpdate)
        {
            if (rowsToUpdate == null) throw new ArgumentNullException("rowsToUpdate");
            try
            {
                var count = DatabaseProxy.Execute(_updateSql, rowsToUpdate.ToArray());
                return count;
            }
            catch (SqlException sqlException)
            {
                throw RepositoryException.DatabaseError(sqlException);
            }
            catch (Exception exception)
            {
                throw RepositoryException.GeneralError(exception);
            }
        }

        public void DeleteAllRows()
        {
            try
            {
                DatabaseProxy.Execute(string.Format("delete from {0}", _table));
            }
            catch (SqlException sqlException)
            {
                throw RepositoryException.DatabaseError(sqlException);
            }
            catch (Exception exception)
            {
                throw RepositoryException.GeneralError(exception);
            }
        }
    }    
}