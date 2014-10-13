using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        public object[,] GetRangeValues(string workSheetName, int startRow, int rowCount, int startColumn, int columnCount)
        {
            if (_package == null) throw new PackageNotOpenException();

            var workSheet = _package.Workbook.Worksheets[workSheetName];
            var range = workSheet.Cells[startRow + 1, startColumn + 1, startRow + (rowCount - 1) + 1, startColumn + (columnCount - 1) + 1]
                .Select(c => new { Value = c.Value, Address = new CellAddress(c.Address) })
                .ToArray();
            var values = new object[columnCount, rowCount];
            foreach (var cell in range)
            {
                values[cell.Address.Column - startColumn, cell.Address.Row - startRow] = cell.Value;
            }

            return values;
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

        public class CellAddress
        {
            private static readonly Regex AddressParsingRegEx;
            private static readonly int CharAValue;
            private static readonly int CharAtoZOffset;
            
            
            static CellAddress()
            {
                CharAValue = (int) 'A';
                CharAtoZOffset = (int)'Z' - CharAValue + 1;
                const string regEx = @"\b([A-Z]{1,}?)(\d{1,})\b";
                AddressParsingRegEx = new System.Text.RegularExpressions.Regex(regEx, RegexOptions.Compiled);
            }

            public CellAddress(string address)
            {
                var groups = AddressParsingRegEx.Match(address).Groups;
                var columnValue = groups[1].Value.ToUpper();
                var column = 0;
                for (var i = 0; i < columnValue.Length; i++)
                {
                    var columnChar = (int) columnValue[i];
                    column += (columnChar - CharAValue) + i*CharAtoZOffset;
                }
                Column = column; 

                var row = -1;
                if (int.TryParse(groups[2].Value, NumberStyles.Integer,
                    System.Globalization.CultureInfo.InvariantCulture, out row))
                {
                    Row = row - 1;
                }
            }

            public int Row { get; private set; }
            public int Column { get; private set; }
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