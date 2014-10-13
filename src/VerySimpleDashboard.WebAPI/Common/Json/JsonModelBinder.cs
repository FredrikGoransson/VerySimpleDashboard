using System;
using System.IO;
using System.Web.Mvc;

namespace VerySimpleDashboard.WebAPI.Common.Json
{
    public class JsonModelBinder<T> : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var contentType = controllerContext.HttpContext.Request.ContentType;
            if (!contentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
                return (null); string bodyText;
            using (var stream = controllerContext.HttpContext.Request.InputStream)
            {
                stream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(stream))
                    bodyText = reader.ReadToEnd();
            }
            if (string.IsNullOrEmpty(bodyText))
                return (null);
            var command = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(bodyText);
            return command;
        }
    }
}