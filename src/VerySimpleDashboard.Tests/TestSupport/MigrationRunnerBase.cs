using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;

namespace VerySimpleDashboard.Tests.TestSupport
{
    /// <summary>
    /// Base code for test that runs migrations
    /// </summary>
    public class MigrationRunnerBase
    {
        // ReSharper disable InconsistentNaming

        // Runs all migrations in the assembly of specified type
        protected void RunMigrationsInAssembly(Assembly assembly, string connectionString)
        {
            var announcer = new NullAnnouncer();
            //var announcer = new TextWriterAnnouncer(s => System.Diagnostics.Debug.WriteLine(s));
            //var assembly = typeof(T).Assembly;

            var migrationContext = new RunnerContext(announcer);

            var options = new MigrationOptions { PreviewOnly = false, Timeout = 60 };
            var factory = new FluentMigrator.Runner.Processors.SqlServer.SqlServerCeProcessorFactory();
            var processor = factory.Create(connectionString, announcer, options);
            var runner = new MigrationRunner(assembly, migrationContext, processor);
            runner.MigrateUp(useAutomaticTransactionManagement: true);

            processor.Dispose();
        }
        // ReSharper restore InconsistentNaming
    }
}
