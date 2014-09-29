using System.Globalization;
using System.Linq;
using FluentAssertions;
using FluentMigrator;
using NUnit.Framework;
using VerySimpleDashboard.Data.SqlStorage.Migrations;

namespace VerySimpleDashboard.Tests
{
    public class MigrationsTests
    {
        [Test]
        // Makes sure that all Migrations are named after the migration number
        public void All_migrations_should_be_named_starting_with_the_version_number()
        {
            // Arrange
            var assembly = typeof(_20140925_001_InitialDatabase).Assembly;
            var migrationTypes = assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(Migration)))
                .Select(type => new
                {
                    Atrribute = type.GetCustomAttributes(typeof(MigrationAttribute), inherit: true).FirstOrDefault() as MigrationAttribute,
                    ClassName = type.Name
                })
                .ToList();

            // Act

            // Assert
            foreach (var migrationType in migrationTypes)
            {
                var migrationTypeClassNameParts = migrationType.ClassName.Trim('_').Split('_');
                var dateInClassName = migrationTypeClassNameParts[0];
                var versionInClassName = migrationTypeClassNameParts[1];
                var migrationVersionInClassName = string.Format("{0}{1}", dateInClassName, versionInClassName);
                migrationVersionInClassName.Should().BeEquivalentTo(
                    migrationType.Atrribute.Version.ToString(CultureInfo.InvariantCulture),
                    "Class for Migration should be named '_YYYYMMDD' + version + '_' + description. Migrations version: " +
                    migrationType.Atrribute.Version.ToString(CultureInfo.InvariantCulture));
            }
        }        
    }
}