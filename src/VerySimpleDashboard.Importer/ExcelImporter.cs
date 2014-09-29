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

        public int MaxEmptycount { get; set; }
        public int BufferRowLength { get; set; }
        public int ColumnGuessScanLength { get; set; }

        private const int DefaultMaxEmptycount = 10;
        private const int DefaultBufferRowLength = 3;
        private const int DefaultColumnGuessScanLength = 100;

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

            MaxEmptycount = DefaultMaxEmptycount;
            BufferRowLength = DefaultBufferRowLength;
            ColumnGuessScanLength = DefaultColumnGuessScanLength;
        }

        private void EnsureReaderIsOpen()
        {
            if (_excelReaderProxy.State != ExcelReaderState.Open)
            {
                Log.Error("ExcelReader must be in state open before calling Import methods");
                throw ExcelImporterException.ReaderNotOpen();
            }
        }

        public Project ImportDocumentStructure()
        {
            Log.Info("ImportDocumentStructure initiated");

            _errors.Clear();
            EnsureReaderIsOpen();

            var project = new Project();
            foreach (var workSheetName in _excelReaderProxy.GetWorkSheetNames())
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
                    columnHeader = _excelReaderProxy.CellAsString(workSheetName, row: 0, column: columnIndex, allowEmpty: true, trimWhitespace: true);

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


            Log.Info("ImportDocumentStructure finished");
            return project;
        }

        public Project ImportDocumentContent(Project project)
        {
            Log.Info("ImportDocumentContent initiated");

            _errors.Clear();
            EnsureReaderIsOpen();

            foreach (var table in project.Tables)
            {
                var workSheetName = table.Name;
                var emptyCount = 0;
                var rowIndex = 1;
                var importedRows = new List<DataRow>();

                var bufferPage = -1;
                var rowOffset = rowIndex;
                var rowValues = (object[,])null;
                while (emptyCount <= DefaultMaxEmptycount)
                {
                    if ((rowIndex >= ((bufferPage + 1) * BufferRowLength + rowOffset)) || rowValues == null)
                    {
                        bufferPage += 1;
                        rowValues = _excelReaderProxy.GetRangeValues(workSheetName, 
                            startRow: rowOffset + bufferPage * BufferRowLength, rowCount: BufferRowLength,
                            startColumn: 0, columnCount: table.Columns.Count);
                    }

                    var columnIndex = 0;
                    var isEmpty = true;
                    var dataRow = new DataRow()
                    {
                        Data = new object[table.Columns.Count]
                    };
                    foreach (var column in table.Columns)
                    {
                        // ReSharper disable once PossibleNullReferenceException - First run rowIndex will always be 1 and rowOffset will be 0 so a new array will be read.
                        var rowValue = DataTypeParser.ParseValue(rowValues[columnIndex, rowIndex - (bufferPage * BufferRowLength) - rowOffset], column.DataType, _cultureInfo);
                        isEmpty = isEmpty & ((rowValue == null) || string.IsNullOrWhiteSpace(rowValue.ToString()));
                        dataRow.Data[columnIndex] = rowValue;

                        columnIndex++;
                    }
                    if (isEmpty) emptyCount++;
                    else
                    {
                        importedRows.Add(dataRow);
                        emptyCount = 0;
                    }

                    rowIndex++;
                }
                foreach (var dataRow in importedRows)
                {
                    table.Rows.Add(dataRow);
                }
            }
            Log.Info("ImportDocumentContent finished");
            return project;
        }

        public Project ImportDocumentContent(System.IO.Stream fileStream, Project project)
        {
            Log.Info("ImportDocumentContent initiated");

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

                ImportDocumentContent(project);
            }

            Log.Info("ImportDocumentContent finished");
            return project;
        }

        private DataType GuessDataType(string workSheetName, int columnIndex, int maxEmptyCount = 10)
        {
            var emptyCount = 0;
            var possibleDataTypes = new List<DataType> {DataType.Integer, DataType.Double, DataType.Boolean, DataType.DateTime, DataType.String};

            foreach (var rowValue in _excelReaderProxy.GetRangeValues(
                workSheetName, startRow:1, rowCount: ColumnGuessScanLength, startColumn: columnIndex, columnCount:1))
            {
                if (rowValue == null || string.IsNullOrWhiteSpace(rowValue.ToString()))
                {
                    emptyCount++;
                }
                else
                {
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
                if (emptyCount > maxEmptyCount)
                {
                    break;
                }
            }

            return possibleDataTypes.First();
        }

        
    }
}
