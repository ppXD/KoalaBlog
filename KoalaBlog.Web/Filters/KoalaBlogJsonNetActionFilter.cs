using KoalaBlog.Framework.MVC;
using System;
using System.Web;
using System.Web.Mvc;

namespace KoalaBlog.Web.Filters
{
    public class KoalaBlogJsonNetActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if(filterContext.Result.GetType() == typeof(JsonResult))
            {
                // Get the standard result object with unserialized data
                var result = filterContext.Result as JsonResult;

                // Replace it with our new result object and transfer settings
                filterContext.Result = new KoalaBlogJsonResult
                                       {
                                           ContentEncoding = result.ContentEncoding,
                                           ContentType = result.ContentType,
                                           Data = result.Data,
                                           JsonRequestBehavior = result.JsonRequestBehavior
                                       };

                // Later on when ExecuteResult will be called it will be the
                // function in JsonNetResult instead of in JsonResult
            }
            base.OnActionExecuted(filterContext);
        }
    }
}