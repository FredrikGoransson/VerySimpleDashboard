using System;
using System.IO;

namespace VerySimpleDashboard.Importer
{
    public interface IExcelReaderProxy : IDisposable
    {
        bool Open(Stream fileStream);
        void Close();
        object GetValue(string workSheetName, int row, int column);
        int GetRowCount(string workSheetName);
        bool HasWorkSheet(string workSheetName);
    }
}