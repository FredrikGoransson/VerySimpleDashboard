using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerySimpleDashboard.Importer;
using VerySimpleDashboard.WebAPI.Models.Import;

namespace VerySimpleDashboard.WebAPI.Controllers
{
    public class ImportController : Controller
    {
        private readonly IExcelImporter _importer;
        private readonly IExcelReaderProxy _excelReader;

        public ImportController(IExcelImporter importer, IExcelReaderProxy excelReader)
        {
            _importer = importer;
            _excelReader = excelReader;
        }

        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload(HttpPostedFileBase uploadFile)
        {
            Debug.WriteLine("Uploading file with {0} bytes", uploadFile.ContentLength);

            _excelReader.Open(uploadFile.InputStream);
            var importDocumentStructure = _importer.ImportDocumentStructure();

            var model = new Models.Import.Upload
            {
                Name = uploadFile.FileName,
                Tables = importDocumentStructure.Tables.Select(
                    table =>
                        new UploadTablePreview()
                        {
                            Name = table.Name,
                            Columns =
                                table.Columns.Select(
                                    column =>
                                        new UploadTableColumnPreview()
                                        {
                                            Name = column.Name,
                                            Type = column.DataType.ToString()
                                        })
                        }).ToArray()
            };
            _excelReader.Close();

            return View(model);
        }
    }
}