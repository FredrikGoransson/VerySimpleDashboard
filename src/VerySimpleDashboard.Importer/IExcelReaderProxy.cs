using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace VerySimpleDashboard.Importer
{
    public interface IExcelReaderProxy : IDisposable
    {
        bool Open(Stream fileStream);
        void Close();
        object GetValue(string workSheetName, int row, int column);
        IEnumerable<object> GetRowValues(string workSheetName, int startRow, int rowCount, int column);
        int GetRowCount(string workSheetName);
        IEnumerable<string> GetWorkSheetNames();
        bool HasWorkSheet(string workSheetName);
    }
}