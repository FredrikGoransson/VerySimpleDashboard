using System.Globalization;
using System.Linq;
using System.Web;

namespace VerySimpleDashboard.WebAPI.Common.Http
{
    public class HttpRequestCultureInfoProvider : ICultureInfoProvider
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            // Get Browser languages.
            var userLanguages = HttpContext.Current.Request.UserLanguages;
            CultureInfo currentCultureInfo;
            if (userLanguages != null && userLanguages.Any())
            {
                try
                {
                    currentCultureInfo = new CultureInfo(userLanguages[0]);
                }
                catch (CultureNotFoundException)
                {
                    currentCultureInfo = CultureInfo.InvariantCulture;
                }
            }
            else
            {
                currentCultureInfo = CultureInfo.InvariantCulture;
            }

            return currentCultureInfo;
        }
    }
}