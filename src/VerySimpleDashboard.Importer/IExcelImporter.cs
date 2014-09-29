using VerySimpleDashboard.Data;

namespace VerySimpleDashboard.Importer
{
    public interface IExcelImporter
    {
        int MaxEmptycount { get; set; }
        int BufferRowLength { get; set; }
        int ColumnGuessScanLength { get; set; }
        Project ImportDocumentStructure();
        Project ImportDocumentContent(Project project);
    }
}