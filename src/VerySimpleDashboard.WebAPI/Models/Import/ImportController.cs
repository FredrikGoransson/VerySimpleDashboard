using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using VerySimpleDashboard.Importer;

namespace VerySimpleDashboard.WebAPI.Models.Import
{
    public class Upload
    {
        public string Name { get; set; }
        public IEnumerable<UploadTablePreview> Tables { get; set; }
    }

    public class UploadTablePreview
    {
        public string Name { get; set; }
        public IEnumerable<UploadTableColumnPreview> Columns { get; set; }
    }

    public class UploadTableColumnPreview
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}