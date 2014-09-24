using System;
using System.Globalization;

namespace VerySimpleDashboard.Importer
{
    internal static class ExcelWorkSheetExtensions
    {
        public static bool CompareWithPrecision(this decimal a, decimal b, int precision)
        {
            return Math.Round(a, precision) == Math.Round(b, precision);
        }

        public static string CellAsString(this IExcelReaderProxy reader, string workSheetName, int row, int column, bool allowEmpty = true, bool trimWhitespace = true,
            Action<ExcelImportError> onFailure = null, Func<string, bool> onValidate = null, Action<ExcelImportError> onValidationFailure = null)
        {
            var value = (reader.GetValue(workSheetName, row, column) ?? string.Empty).ToString().Trim();
            if (!allowEmpty && string.IsNullOrWhiteSpace(value) && onFailure != null)
            {
                onFailure(new ExcelImportError() { WorkSheet = workSheetName, Column = column, Row = row, Value = value });
                return null;
            }
            if (onValidate != null && onFailure != null && !onValidate(value))
            {
                if (onValidationFailure != null)
                    onValidationFailure(new ExcelImportError() { WorkSheet = workSheetName, Column = column, Row = row, Value = value });
                return null;
            }
            return value;
        }

        public static decimal CellAsDecimal(this IExcelReaderProxy reader, string workSheetName, int row, int column,
            Action<ExcelImportError> onFailure = null, Func<decimal, bool> onValidate = null, Action<ExcelImportError> onValidationFailure = null)
        {
            var value = (reader.GetValue(workSheetName, row, column) ?? string.Empty).ToString();
            var result = default(decimal);
            if (!Decimal.TryParse(value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out result) && onFailure != null)
            {
                onFailure(new ExcelImportError() { WorkSheet = workSheetName, Column = column, Row = row, Value = value });
                return result;
            }
            if (onValidate != null && onFailure != null && !onValidate(result))
            {
                if (onValidationFailure != null)
                    onValidationFailure(new ExcelImportError() { WorkSheet = workSheetName, Column = column, Row = row, Value = value });
                return result;
            }
            return result;
        }

        public static int CellAsInt(this IExcelReaderProxy reader, string workSheetName, int row, int column,
            Action<ExcelImportError> onFailure = null, Func<int, bool> onValidate = null, Action<ExcelImportError> onValidationFailure = null)
        {
            var value = (reader.GetValue(workSheetName, row, column) ?? string.Empty).ToString();
            var result = default(int);
            if (!Int32.TryParse(value, NumberStyles.Integer, CultureInfo.CurrentCulture, out result) && onFailure != null)
            {
                onFailure(new ExcelImportError() { WorkSheet = workSheetName, Column = column, Row = row, Value = value });
                return result;
            }
            if (onValidate != null && onFailure != null && !onValidate(result))
            {
                if (onValidationFailure != null)
                    onValidationFailure(new ExcelImportError() { WorkSheet = workSheetName, Column = column, Row = row, Value = value });
                return result;
            }
            return result;
        }

        public static bool CellAsBool(this IExcelReaderProxy reader, string workSheetName, int row, int column,
            Action<ExcelImportError> onFailure = null, Func<bool, bool> onValidate = null, Action<ExcelImportError> onValidationFailure = null)
        {
            var value = (reader.GetValue(workSheetName, row, column) ?? string.Empty).ToString();
            var result = default(bool);
            if (!Boolean.TryParse(value, out result) && onFailure != null)
            {
                onFailure(new ExcelImportError() { WorkSheet = workSheetName, Column = column, Row = row, Value = value });
                return result;
            }
            if (onValidate != null && onFailure != null && !onValidate(result))
            {
                if (onValidationFailure != null)
                    onValidationFailure(new ExcelImportError() { WorkSheet = workSheetName, Column = column, Row = row, Value = value });
                return result;
            }
            return result;
        }
    }
}