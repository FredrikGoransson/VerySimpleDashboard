namespace VerySimpleDashboard.Importer
{
    public static class ExcelImportErrorExtensions
    {
        public static ExcelImportError WithDescription(this ExcelImportError error, string description, params object[] args)
        {
            error.Description = string.Format(description, args);
            return error;
        }
    }
}