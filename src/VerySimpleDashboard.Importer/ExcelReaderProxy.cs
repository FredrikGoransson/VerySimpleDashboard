using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace VerySimpleDashboard.Importer
{
    public class ExcelReaderProxy : IExcelReaderProxy
    {
        private ExcelPackage _package;
        private string[] _workSheetNames;
        public bool Open(Stream fileStream)
        {
            try
            {
                _package = new ExcelPackage(fileStream);
                
                _workSheetNames = _package.Workbook.Worksheets.Select(workSheet => workSheet.Name).ToArray();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void Close()
        {
            _package.Dispose();
        }

        public object GetValue(string workSheetName, int row, int column)
        {
            if (_package == null) throw new PackageNotOpenException();

            var workSheet = _package.Workbook.Worksheets[workSheetName];
            return workSheet.Cells[row+1, column+1].Value;
        }

        public IEnumerable<object> GetRowValues(string workSheetName, int startRow, int rowCount, int column)
        {
            if (_package == null) throw new PackageNotOpenException();

            var workSheet = _package.Workbook.Worksheets[workSheetName];

            return workSheet.Cells[startRow + 1, column + 1, startRow + rowCount + 1, column + 1].Select(c => c.Value);
        }

        public int GetRowCount(string workSheetName)
        {
            if (_package == null) throw new PackageNotOpenException();

            var workSheet = _package.Workbook.Worksheets[workSheetName];
            return workSheet.Dimension.End.Row;
        }

        public IEnumerable<string> GetWorkSheetNames()
        {
            return _workSheetNames;
        }

        public bool HasWorkSheet(string workSheetName)
        {
            if (_package == null) throw new PackageNotOpenException();

            return _workSheetNames.Any(workSheet => workSheet == workSheetName);
        }

        public void Dispose()
        {
            if (_package != null)
                _package.Dispose();
        }
    }

    public class PackageNotOpenException : Exception
    {
        public PackageNotOpenException()
            : base(
                "Call Open(...) to open an Excel Package before calling any methods that tries to access worksheet data"
                ) { }
    }
}