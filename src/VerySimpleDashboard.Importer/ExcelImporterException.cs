using System;

namespace VerySimpleDashboard.Importer
{
    public class ExcelImporterException : Exception
    {
        private ExcelImporterException(string message) : base(message)
        {
            
        }

        public static ExcelImporterException ReaderNotOpen()
        {
            return new ExcelImporterException("ExcelReader must be in state Open before calling Import methods");
        }
    }
}