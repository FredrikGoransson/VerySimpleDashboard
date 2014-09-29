using System;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace VerySimpleDashboard.Tests
{
    /// <summary>
    /// Base class for any integration test that uses a fully migrated file-based SQL CE database
    /// </summary>
    public class TestsWithDatabaseFromMigrations<TMigrationInMigrationAssembly> : MigrationRunnerBase
    {
        // ReSharper disable InconsistentNaming
        private string DatabaseFileName;
        protected string ConnectionString { get; private set; }
        private IDbConnection _connection;

        protected IDbConnection CreateConnection()
        {
            if (_connection == null)
            {
                _connection = new SqlCeConnection(ConnectionString);
                _connection.Open();
            }
            return _connection;
        }

        protected void CloseConnection()
        {
            if (_connection != null && _connection.State == ConnectionState.Open) _connection.Close();
            _connection = null;
        }

        [SetUp]
        public void Setup()
        {
            var databaseId = Guid.NewGuid();
            Console.WriteLine("Setup called " + databaseId);
            var executingPath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            executingPath = System.IO.Path.GetDirectoryName(executingPath);
            var fileName = string.Format("{0}.sdf", databaseId);
            DatabaseFileName = System.IO.Path.Combine(executingPath, fileName);

            ConnectionString = CreateDatabase(DatabaseFileName);
            if (ConnectionString != null)
            {
                RunMigrationsInAssembly(typeof(TMigrationInMigrationAssembly).Assembly, ConnectionString);
            }
        }

        [TearDown]
        public void TearDown()
        {
            Console.WriteLine("TearDown called");
            CloseConnection();
            DeleteDatabase(DatabaseFileName);
        }

        // Removes the database if it exists
        private static void DeleteDatabase(string databaseName)
        {
            // check if exists
            if (File.Exists(databaseName)) File.Delete(databaseName);
        }

        // Creates a new SQLCE database
        private static string CreateDatabase(string databaseFileName)
        {
            // Delete the database if it already exists
            DeleteDatabase(databaseFileName);

            var connectionString = @"Data Source = " + databaseFileName;

            // create Database
            var engine = new SqlCeEngine(connectionString);
            engine.CreateDatabase();
            engine.Dispose();

            return connectionString;
        }

        // ReSharper restore InconsistentNaming
    }
}