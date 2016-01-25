using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace KoalaBlog.Framework.MVC
{
    public class KoalaBlogJsonResult : JsonResult
    {
        public KoalaBlogJsonResult()
        {
            this.Settings = new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Error
                            };
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if(context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if(!string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            if(this.Data == null)
            {
                return;
            }

            var scriptSerializer = JsonSerializer.Create(this.Settings);
            // Serialize the data to the Output stream of the response
            scriptSerializer.Serialize(response.Output, this.Data);
        }

        public JsonSerializerSettings Settings { get; private set; }
    }
}
