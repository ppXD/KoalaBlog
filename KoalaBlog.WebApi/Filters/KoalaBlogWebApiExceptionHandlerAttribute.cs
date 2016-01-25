using KoalaBlog.Framework.Util;
using KoalaBlog.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using KoalaBlog.WebApi.Core.Managers;
using KoalaBlog.Framework.Exceptions;
using System.Data.Entity.Core;
using KoalaBlog.Framework.Net;

namespace KoalaBlog.WebApi.Filters
{
    public class KoalaBlogWebApiExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override async Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, System.Threading.CancellationToken cancellationToken)
        {
            //断言错误处理
            if(actionExecutedContext.Exception is AssertException)
            {
                AssertMessage assertMessage = new AssertMessage();
                assertMessage.AssertMessageList = (actionExecutedContext.Exception as AssertException).ExceptionMessageList;

                HttpResponseMessage response = actionExecutedContext.Request.CreateResponse<string>(HttpStatusCode.InternalServerError, assertMessage.ToJSON());
                actionExecutedContext.Response = response;
            }            
            else if(actionExecutedContext.Exception is DisplayableException)
            {
                string displayMessage = (actionExecutedContext.Exception as DisplayableException).Message;

                HttpResponseMessage response = actionExecutedContext.Request.CreateResponse<string>(HttpStatusCode.BadRequest, displayMessage);
                actionExecutedContext.Response = response;
            }
            //EntityFramework错误处理，此错误通常为Database Connect Fail，所以不能Log进数据库里，处理方式为发邮件
            else if (actionExecutedContext.Exception is EntityException)
            {
                Exception ex = actionExecutedContext.Exception;
                string msgSubject = "WebApi Error";
                string exMsg = ex.GetType() + "------" + ex.Message + "\r\n";
                string sourceObj = actionExecutedContext.ActionContext.ControllerContext.Controller.ToString() + "." +
                                   actionExecutedContext.ActionContext.ActionDescriptor.ActionName;

                exMsg += "Error Place ------ " + sourceObj + "\r\n";

                while(ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    exMsg += "InnerException" + "\r\n" + ex.Message + ex.StackTrace + "\r\n";
                }

                await SMTPMailClient.SendAsync(msgSubject, exMsg, "yourmail@xx.com", "", false);

                HttpResponseMessage response = actionExecutedContext.Request.CreateResponse<string>(HttpStatusCode.InternalServerError, "System Error");
                actionExecutedContext.Response = response;
            }
            //其它错误处理则Log进数据库
            else
            {
                //Log的错误信息
                Exception ex = actionExecutedContext.Exception;
                string exMsg = ex.GetType() + "------" + ex.Message;
                string sourceObj = actionExecutedContext.ActionContext.ControllerContext.Controller.ToString() + "." +
                                   actionExecutedContext.ActionContext.ActionDescriptor.ActionName;

                //Email的错误信息
                string emailSubject = "WebApi Error";
                string emailMsg = exMsg;
                emailMsg += "Error Place ------ " + sourceObj + "\r\n";

                while(ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    exMsg += "InnerException:" + ex.Message + ex.StackTrace;
                    emailMsg += "InnerException" + "\r\n" + ex.Message + ex.StackTrace + "\r\n";
                }

                SecurityManager securityManager = new SecurityManager();

                Task<bool> logTask = securityManager.WriteLogAsync(Entity.Models.Enums.LogLevel.ERROR, sourceObj, exMsg);                

                bool isSucceedLog = await logTask;

                //如果Log数据库失败，则改为发邮件。
                if(!isSucceedLog)
                {
                    await SMTPMailClient.SendAsync(emailSubject, exMsg, "yourmail@xx.com", "", false);
                }                

                HttpResponseMessage response = actionExecutedContext.Request.CreateResponse<string>(HttpStatusCode.InternalServerError, "System Error");
                actionExecutedContext.Response = response;
            }
        }
    }
}