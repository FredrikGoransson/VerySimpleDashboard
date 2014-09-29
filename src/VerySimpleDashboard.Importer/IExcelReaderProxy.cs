using System;
using System.Collections.Generic;
using System.IO;

namespace VerySimpleDashboard.Importer
{
    public interface IExcelReaderProxy : IDisposable
    {
        ExcelReaderState State { get; }
        bool Open(Stream fileStream);
        void Close();
        object GetValue(string workSheetName, int row, int column);
        object[,] GetRangeValues(string workSheetName, int startRow, int rowCount, int startColumn, int columnCount);
        int GetRowCount(string workSheetName);
        IEnumerable<string> GetWorkSheetNames();
        bool HasWorkSheet(string workSheetName);
    }

    public enum ExcelReaderState
    {
        Initial,
        Open,
        Closed,
        Error,
    }
}