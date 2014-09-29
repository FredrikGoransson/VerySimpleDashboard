using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using VerySimpleDashboard.Data;
using VerySimpleDashboard.Importer;

namespace VerySimpleDashboard.Tests
{
    // ReSharper disable once InconsistentNaming
    public class ExcelImporterTests_ImportDocument
    {
        [Test]
        public void Should_return_project_from_document()
        {
            //Act
            var reader = new ExcelReaderProxy();
            var importer = new ExcelImporter(reader, System.Globalization.CultureInfo.InvariantCulture);
            var fileStream = System.IO.File.Open(@"Excel Importer/Test Document 1.xlsx", FileMode.Open, FileAccess.Read,
                FileShare.Read);

            //Arrange
            var importedProject = importer.ImportDocumentStructure(fileStream);
            fileStream.Close();
            fileStream.Dispose();

            //Assert
            importedProject.Should().NotBeNull();
            importedProject.Id.Should().NotBe(Guid.Empty);
            importedProject.Id.Should().NotBe(default(Guid));
        }

        [Test]
        public void Should_return_project_with_one_table_per_worksheet_in_document()
        {
            //Act
            var reader = new ExcelReaderProxy();
            var importer = new ExcelImporter(reader, System.Globalization.CultureInfo.InvariantCulture);
            var fileStream = System.IO.File.Open(@"Excel Importer/Test Document 1.xlsx", FileMode.Open, FileAccess.Read,
                FileShare.Read);

            //Arrange
            var importedProject = importer.ImportDocumentStructure(fileStream);
            fileStream.Close();
            fileStream.Dispose();

            //Assert
            importedProject.Tables.Should().HaveCount(2);

            var tableInternetUsers = importedProject.Tables.FirstOrDefault(a => a.Name == "Internet users");
            var tableInternetConnectionSpeed =
                importedProject.Tables.FirstOrDefault(a => a.Name == "Internet connection speed");

            tableInternetUsers.Should().NotBeNull();
            tableInternetUsers.Id.Should().NotBe(Guid.Empty);
            tableInternetUsers.Id.Should().NotBe(default(Guid));

            tableInternetConnectionSpeed.Id.Should().NotBe(Guid.Empty);
            tableInternetConnectionSpeed.Id.Should().NotBe(default(Guid));
        }

        [Test]
        public void Should_return_table_columns_for_each_worksheet_column()
        {
            //Act
            var reader = new ExcelReaderProxy();
            var importer = new ExcelImporter(reader, System.Globalization.CultureInfo.InvariantCulture);
            var fileStream = System.IO.File.Open(@"Excel Importer/Test Document 1.xlsx", FileMode.Open, FileAccess.Read,
                FileShare.Read);

            //Arrange
            var importedProject = importer.ImportDocumentStructure(fileStream);
            fileStream.Close();
            fileStream.Dispose();

            //Assert
            var tableInternetUsers = importedProject.Tables.First(a => a.Name == "Internet users");
            var tableInternetConnectionSpeed = importedProject.Tables.First(a => a.Name == "Internet connection speed");

            tableInternetUsers.Columns.Should().HaveCount(6);
            tableInternetConnectionSpeed.Columns.Should().HaveCount(2);

            var expectedColumns = new[]
            {"Country or area", "Internet users", "Rank", "Penetration", "Is big", "Check date"};
            tableInternetUsers.Columns.Select(c => c.Name).ShouldAllBeEquivalentTo(expectedColumns);

            expectedColumns = new[] {"Country/Territory", "Avg. connection speed (Mbit/s)"};
            tableInternetConnectionSpeed.Columns.Select(c => c.Name).ShouldAllBeEquivalentTo(expectedColumns);
        }

        [Test]
        public void Should_return_table_columns_with_best_guessed_datatype_string_for_textual_data()
        {
            //Act
            var reader = new ExcelReaderProxy();
            var importer = new ExcelImporter(reader, System.Globalization.CultureInfo.InvariantCulture);
            var fileStream = System.IO.File.Open(@"Excel Importer/Test Document 1.xlsx", FileMode.Open, FileAccess.Read,
                FileShare.Read);

            //Arrange
            var importedProject = importer.ImportDocumentStructure(fileStream);
            fileStream.Close();
            fileStream.Dispose();

            //Assert
            var tableInternetUsers = importedProject.Tables.First(a => a.Name == "Internet users");
            var countryOrAreaColumn = tableInternetUsers.Columns.First(c => c.Name == "Country or area");
            countryOrAreaColumn.DataType.Should().Be(DataType.String);
        }

        [Test]
        public void Should_return_table_columns_with_best_guessed_datatype_integer_for_interger_data()
        {
            //Act
            var reader = new ExcelReaderProxy();
            var importer = new ExcelImporter(reader, System.Globalization.CultureInfo.InvariantCulture);
            var fileStream = System.IO.File.Open(@"Excel Importer/Test Document 1.xlsx", FileMode.Open, FileAccess.Read,
                FileShare.Read);

            //Arrange
            var importedProject = importer.ImportDocumentStructure(fileStream);
            fileStream.Close();
            fileStream.Dispose();

            //Assert
            var tableInternetUsers = importedProject.Tables.First(a => a.Name == "Internet users");
            var internetUsersColumn = tableInternetUsers.Columns.First(c => c.Name == "Internet users");
            internetUsersColumn.DataType.Should().Be(DataType.Integer);
        }

        [Test]
        public void Should_return_table_columns_with_best_guessed_datatype_double_for_double_data()
        {
            //Act
            var reader = new ExcelReaderProxy();
            var importer = new ExcelImporter(reader, System.Globalization.CultureInfo.InvariantCulture);
            var fileStream = System.IO.File.Open(@"Excel Importer/Test Document 1.xlsx", FileMode.Open, FileAccess.Read,
                FileShare.Read);

            //Arrange
            var importedProject = importer.ImportDocumentStructure(fileStream);
            fileStream.Close();
            fileStream.Dispose();

            //Assert
            var tableInternetUsers = importedProject.Tables.First(a => a.Name == "Internet users");
            var internetUsersColumn = tableInternetUsers.Columns.First(c => c.Name == "Penetration");
            internetUsersColumn.DataType.Should().Be(DataType.Double);
        }

        [Test]
        public void Should_return_table_columns_with_best_guessed_datatype_boolean_for_boolean_data()
        {
            //Act
            var reader = new ExcelReaderProxy();
            var importer = new ExcelImporter(reader, System.Globalization.CultureInfo.InvariantCulture);
            var fileStream = System.IO.File.Open(@"Excel Importer/Test Document 1.xlsx", FileMode.Open, FileAccess.Read,
                FileShare.Read);

            //Arrange
            var importedProject = importer.ImportDocumentStructure(fileStream);
            fileStream.Close();
            fileStream.Dispose();

            //Assert
            var tableInternetUsers = importedProject.Tables.First(a => a.Name == "Internet users");
            var isBigColumn = tableInternetUsers.Columns.First(c => c.Name == "Is big");
            isBigColumn.DataType.Should().Be(DataType.Boolean);
        }

        [Test]
        public void Should_return_table_columns_with_best_guessed_datatype_datetime_for_boolean_datetime()
        {
            //Act
            var reader = new ExcelReaderProxy();
            var importer = new ExcelImporter(reader, System.Globalization.CultureInfo.InvariantCulture);
            var fileStream = System.IO.File.Open(@"Excel Importer/Test Document 1.xlsx", FileMode.Open, FileAccess.Read,
                FileShare.Read);

            //Arrange
            var importedProject = importer.ImportDocumentStructure(fileStream);
            fileStream.Close();
            fileStream.Dispose();

            //Assert
            var tableInternetUsers = importedProject.Tables.First(a => a.Name == "Internet users");
            var checkDateColumn = tableInternetUsers.Columns.First(c => c.Name == "Check date");
            checkDateColumn.DataType.Should().Be(DataType.DateTime);
        }

        [Test]
        public void Should_return_table_columns_with_datatype_for_each_worksheet_column()
        {
            //Act
            var reader = new ExcelReaderProxy();
            var importer = new ExcelImporter(reader, System.Globalization.CultureInfo.InvariantCulture);
            var fileStream = System.IO.File.Open(@"Excel Importer/Test Document 1.xlsx", FileMode.Open, FileAccess.Read,
                FileShare.Read);

            //Arrange
            var importedProject = importer.ImportDocumentStructure(fileStream);
            fileStream.Close();
            fileStream.Dispose();

            //Assert
            var tableInternetUsers = importedProject.Tables.First(a => a.Name == "Internet users");
            var tableInternetConnectionSpeed = importedProject.Tables.First(a => a.Name == "Internet connection speed");

            tableInternetUsers.Columns.Should().HaveCount(6);
            tableInternetConnectionSpeed.Columns.Should().HaveCount(2);

            var expectedColumns = new[]
            {
                new {Name = "Country or area", DataType = DataType.String},
                new {Name = "Internet users", DataType = DataType.Integer},
                new {Name = "Rank", DataType = DataType.Integer},
                new {Name = "Penetration", DataType = DataType.Double},
                new {Name = "Is big", DataType = DataType.Boolean},
                new {Name = "Check date", DataType = DataType.DateTime}
            };

            for (var i = 0; i < expectedColumns.Length; i++)
            {
                var expectedColumn = expectedColumns[i];
                var actualColumn = tableInternetUsers.Columns[i];

                actualColumn.Name.Should().Be(expectedColumn.Name);
                actualColumn.DataType.Should().Be(expectedColumn.DataType);
            }

            expectedColumns = new[]
            {
                new {Name = "Country/Territory", DataType = DataType.String},
                new {Name = "Avg. connection speed (Mbit/s)", DataType = DataType.Double}
            };


            for (var i = 0; i < expectedColumns.Length; i++)
            {
                var expectedColumn = expectedColumns[i];
                var actualColumn = tableInternetConnectionSpeed.Columns[i];

                actualColumn.Name.Should().Be(expectedColumn.Name);
                actualColumn.DataType.Should().Be(expectedColumn.DataType);
            }
        }
    }
}
