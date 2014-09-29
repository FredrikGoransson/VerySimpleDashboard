using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using VerySimpleDashboard.Importer;

namespace VerySimpleDashboard.Tests
{
    // ReSharper disable once InconsistentNaming
    public class ExcelImporterTests_ImportDocumentContent
    {
        protected IExcelReaderProxy ExcelReaderProxy;
        private FileStream _fileStream;

        [SetUp]
        public void Setup()
        {
            var reader = new ExcelReaderProxy();
            ExcelReaderProxy = reader;
        }

        [TearDown]
        public void Teardown()
        {
            _fileStream.Close();
            _fileStream.Dispose();
            ExcelReaderProxy.Dispose();
        }

        [Test]
        public void Should_return_import_data_to_project_from_document()
        {
            //Act
            _fileStream = File.Open(@"Excel Importer/Test Document 1.xlsx",
                FileMode.Open, FileAccess.Read, FileShare.Read);
            ExcelReaderProxy.Open(_fileStream);
            var importer = new ExcelImporter(ExcelReaderProxy, System.Globalization.CultureInfo.InvariantCulture);
            var importedProject = importer.ImportDocumentStructure();

            //Arrange
            importedProject = importer.ImportDocumentContent(importedProject);

            //Assert
            var tableInternetUsers = importedProject.Tables.First(a => a.Name == "Internet users");
            tableInternetUsers.Rows.Should().HaveCount(212);

            var tableInternetConnectionSpeed = importedProject.Tables.First(a => a.Name == "Internet connection speed");
            tableInternetConnectionSpeed.Rows.Should().HaveCount(62);
        }

        [Test]
        public void When_document_contains_empty_rows_in_the_middle_should_return_import_data_to_project_from_document()
        {
            //Act
            _fileStream = File.Open(@"Excel Importer/Test Document 2.xlsx",
                FileMode.Open, FileAccess.Read, FileShare.Read);
            ExcelReaderProxy.Open(_fileStream);

            var importer = new ExcelImporter(ExcelReaderProxy, System.Globalization.CultureInfo.InvariantCulture);
            var importedProject = importer.ImportDocumentStructure();

            //Arrange
            importedProject = importer.ImportDocumentContent(importedProject);

            //Assert
            var tableInternetUsers = importedProject.Tables.First(a => a.Name == "Internet users");
            tableInternetUsers.Rows.Should().HaveCount(54);

            var tableInternetConnectionSpeed = importedProject.Tables.First(a => a.Name == "Internet connection speed");
            tableInternetConnectionSpeed.Rows.Should().HaveCount(61);
        }
    }
}