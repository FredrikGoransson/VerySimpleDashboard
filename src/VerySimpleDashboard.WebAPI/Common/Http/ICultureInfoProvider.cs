using System.Globalization;

namespace VerySimpleDashboard.WebAPI.Common.Http
{
    public interface ICultureInfoProvider
    {
        CultureInfo GetCurrentCultureInfo();
    }
}