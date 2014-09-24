using System;
using System.Collections.Generic;
using System.Linq;

namespace VerySimpleDashboard.Importer
{
    public class ExcelImporter
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IEnumerable<ExcelImportError> Errors
        {
            get { return _errors; }
        }

        private readonly IExcelReaderProxy _excelReaderProxy;
        private readonly IList<ExcelImportError> _errors = new List<ExcelImportError>();
        private bool _hasRowErrors;

        public ExcelImporter(
            IExcelReaderProxy excelReaderProxy)
        {
            _excelReaderProxy = excelReaderProxy;
        }
    }
}
