using System.IO;
using NUnit.Framework;
using VerySimpleDashboard.Importer;

namespace VerySimpleDashboard.Tests
{
    // ReSharper disable once InconsistentNaming
    public class ExcelImporterTests_ImportDocumentStructure_With_well_formed_document : ExcelImporterTests_base_tests
    {
        private FileStream _fileStream;

        [SetUp]
        public void Setup()
        {
            var reader = new ExcelReaderProxy();
            _fileStream = File.Open(@"Excel Importer/Test Document 1.xlsx",
                FileMode.Open, FileAccess.Read, FileShare.Read);
            reader.Open(_fileStream);
            ExcelReaderProxy = reader;
        }

        [TearDown]
        public void Teardown()
        {
            _fileStream.Close();
            _fileStream.Dispose();
            ExcelReaderProxy.Dispose();
        }
    }
}
