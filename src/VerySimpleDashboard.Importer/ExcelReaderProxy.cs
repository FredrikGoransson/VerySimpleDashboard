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
            var workSheet = _package.Workbook.Worksheets[workSheetName];
            return workSheet.Cells[row, column].Value;
        }

        public int GetRowCount(string workSheetName)
        {
            var workSheet = _package.Workbook.Worksheets[workSheetName];
            return workSheet.Dimension.End.Row;
        }

        public bool HasWorkSheet(string workSheetName)
        {
            return _workSheetNames.Any(workSheet => workSheet == workSheetName);
        }

        public void Dispose()
        {
            if (_package != null)
                _package.Dispose();
        }
    }
}