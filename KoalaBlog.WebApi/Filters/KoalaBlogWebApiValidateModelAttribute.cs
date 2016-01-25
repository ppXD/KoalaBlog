using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using KoalaBlog.WebApi.Core.Managers;

namespace KoalaBlog.WebApi.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class KoalaBlogWebApiValidateModelAttribute : ActionFilterAttribute
    {
        public override async System.Threading.Tasks.Task OnActionExecutingAsync(System.Web.Http.Controllers.HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            if(!actionContext.ModelState.IsValid)
            {                
                string logSourceObj = actionContext.ControllerContext.Controller.ToString() + "." +
                                      actionContext.ActionDescriptor.ActionName;

                string logMessage = "The request is invalid.";

                foreach (var key in actionContext.ModelState.Keys)
                {
                    if(actionContext.ModelState[key].Errors.Any())
                    {
                        logMessage += "\r\n" + actionContext.ModelState[key].Errors.First().ErrorMessage;
                    }
                }

                await new SecurityManager().WriteLogAsync(Entity.Models.Enums.LogLevel.WARNING, logSourceObj, logMessage);

                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }
}