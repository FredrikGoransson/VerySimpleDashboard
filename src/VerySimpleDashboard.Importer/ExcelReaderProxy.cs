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
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ExcelPackage _package;
        private string[] _workSheetNames;
        public ExcelReaderState State { get; private set; }

        public ExcelReaderProxy()
        {
            State = ExcelReaderState.Initial;
        }

        public bool Open(Stream fileStream)
        {
            try
            {
                _package = new ExcelPackage(fileStream);
                
                _workSheetNames = _package.Workbook.Worksheets.Select(workSheet => workSheet.Name).ToArray();
                State = ExcelReaderState.Open;
            }
            catch
            {
                State = ExcelReaderState.Error;
                Log.Error("Import file was not valid, exiting");
                return false;
            }
            return true;
        }

        public void Close()
        {
            State = ExcelReaderState.Closed;
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

        public IEnumerable<object> GetColumnValues(string workSheetName, int row, int startColumn, int columnCount)
        {
            if (_package == null) throw new PackageNotOpenException();

            var workSheet = _package.Workbook.Worksheets[workSheetName];

            return workSheet.Cells[row + 1, startColumn + 1, row + 1, startColumn + columnCount + 1].Select(c => c.Value);
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