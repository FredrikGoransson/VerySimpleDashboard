namespace VerySimpleDashboard.WebAPI.Common.Http
{
    public class HttpContextHtmlDecoder : IHtmlDecoder
    {
        public string Decode(string encodedString)
        {
            var htmlDecodedString = System.Web.HttpContext.Current.Server.HtmlDecode(encodedString);
            return htmlDecodedString;
        }
    }
}