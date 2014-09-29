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
    public class ExcelReaderProxy_tests
    {
        [Test]
        public void GetRowValues_should_return_an_array_of_cell_values()
        {
            // Arrange
            var reader = new ExcelReaderProxy();
            var fileStream = File.Open(@"Excel Reader/Test Document 3.xlsx",
                FileMode.Open, FileAccess.Read, FileShare.Read);
            reader.Open(fileStream);

            // Act
            var values = reader.GetColumnValues("complete data", 0, 0, 3);

            // Assert
            values.ShouldAllBeEquivalentTo(new[] {"A1", "B1", "C1"});
        }

        [Test]
        public void GetRowValues_should_return_an_array_of_cell_values_when_cells_are_empty()
        {
            // Arrange
            var reader = new ExcelReaderProxy();
            var fileStream = File.Open(@"Excel Reader/Test Document 3.xlsx",
                FileMode.Open, FileAccess.Read, FileShare.Read);
            reader.Open(fileStream);

            // Act
            var values = reader.GetRangeValues("missing cells", startRow:1, rowCount: 3, startColumn: 0, columnCount: 3);

            // Assert
            values[0, 0].Should().Be(null);
            values[1, 0].Should().Be("B2");
            values[2, 0].Should().Be("C2");

            values[0, 1].Should().Be("A3");
            values[1, 1].Should().Be(null);
            values[2, 1].Should().Be("C3");

            values[0, 2].Should().Be("A4");
            values[1, 2].Should().Be("B4");
            values[2, 2].Should().Be("C4");
        }

        [Test]
        public void CellAddress_should_return_0_indexed_row_and_column_from_excel_style_address()
        {
            // Arrange
            var cells = new[] {"A1", "AA1", "AA11", "A11", "AQ123", "B23" };

            // Act
            var cellAddresses = cells.Select(c => new ExcelReaderProxy.CellAddress(c)).ToArray();

            // Assert
            var expectedColumns = new[] { 0, 26, 26, 0, 42, 1 };
            var expectedRows = new[] { 0, 0, 10, 10, 122, 22 };
            var actualColumns = cellAddresses.Select(c => c.Column).ToArray();
            var actualRows = cellAddresses.Select(c => c.Row).ToArray();

            for (var i = 0; i < expectedColumns.Length; i++)
            {
                actualColumns[i].Should().Be(expectedColumns[i]);
                actualRows[i].Should().Be(expectedRows[i]);
            }

        }
    }
}
