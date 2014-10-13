using System.Web;
using VerySimpleDashboard.Importer;

namespace VerySimpleDashboard.WebAPI.Common.MVC
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AspNetDataPathBuilder : IFilePathBuilder
    {
        public string BuildPath(string documentName)
        {
            var path = HttpContext.Current.Server.MapPath(string.Format("~/App_Data/{0}", documentName.Replace("Data/", "")));
            return path;
        }
    }
}