namespace VerySimpleDashboard.Importer
{
    public class ExcelImportError
    {
        public string WorkSheet { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}