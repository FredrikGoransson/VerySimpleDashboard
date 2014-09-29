using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VerySimpleDashboard.Data;

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
        private readonly CultureInfo _cultureInfo;
        private readonly IList<ExcelImportError> _errors = new List<ExcelImportError>();
        private bool _hasRowErrors;
        
        private void AddError(ExcelImportError error)
        {
            _hasRowErrors = true;
            _errors.Add(error);
        }


        public ExcelImporter(
            IExcelReaderProxy excelReaderProxy, 
            System.Globalization.CultureInfo cultureInfo)
        {
            _excelReaderProxy = excelReaderProxy;
            _cultureInfo = cultureInfo;
        }

        public Project ImportDocumentStructure(System.IO.Stream fileStream)
        {
            Log.Info("ImportDocumentStructure initiated");

            var project = new Project();
            
            _errors.Clear();

            using (var reader = _excelReaderProxy)
            {
                var isValid = reader.Open(fileStream);

                if (!isValid)
                {
                    AddError(new ExcelImportError().WithDescription("The file to import is not a valid Excel file"));
                    Log.Debug("Import file was not valid, exiting");
                    return null;
                }

                foreach (var workSheetName in reader.GetWorkSheetNames())
                {
                    Log.DebugFormat("Importing worksheet {0} initiated", workSheetName);
                    var table = new Table()
                    {
                        Name = workSheetName
                    };
                    project.Tables.Add(table);

                    var columnIndex = 0;
                    var columnHeader = (string)null;
                    do
                    {
                        columnHeader = reader.CellAsString(workSheetName, row: 0, column: columnIndex, allowEmpty: true, trimWhitespace: true);

                        if (!string.IsNullOrWhiteSpace(columnHeader))
                        {
                            var column = new Column()
                            {
                                Name = columnHeader
                            };
                            table.Columns.Add(column);

                            column.DataType = GuessDataType(workSheetName, columnIndex: columnIndex, maxEmptyCount: 10);
                        }
                        columnIndex++;
                    } while (!string.IsNullOrWhiteSpace(columnHeader));

                    Log.DebugFormat("Importing worksheet {0} finished", workSheetName);
                }
            }

            Log.Info("ImportDocumentStructure finished");
            return project;
        }

        public Project ImportDocumentContent(Project project)
        {
            throw new NotImplementedException();
        }

        private DataType GuessDataType(string workSheetName, int columnIndex, int maxEmptyCount = 10)
        {
            var emptyCount = 0;
            var possibleDataTypes = new List<DataType> {DataType.Integer, DataType.Double, DataType.Boolean, DataType.DateTime, DataType.String};
            foreach (var rowValue in _excelReaderProxy.GetRowValues(workSheetName, 1, 100, columnIndex))
            {
                
                if (string.IsNullOrWhiteSpace(rowValue as string))
                {
                    emptyCount++;
                }
                if (emptyCount > maxEmptyCount)
                {
                    break;
                }

                var dataTypesToTest = new List<DataType>(possibleDataTypes);
                foreach (var dataTypeToTest in dataTypesToTest)
                {
                    if (!DataTypeParser.TestValue(rowValue, dataTypeToTest, _cultureInfo))
                    {
                        possibleDataTypes.Remove(dataTypeToTest);
                    }                    
                }
                if (!possibleDataTypes.Any())
                {
                    return DataType.Unknown;
                }
            }

            return possibleDataTypes.First();
        }
    }
}
