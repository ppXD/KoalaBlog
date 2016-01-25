using KoalaBlog.Framework.Exceptions;
using KoalaBlog.Framework.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KoalaBlog.Web.Filters
{
    public class KoalaBlogExceptionHandlerAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            //判断当前请求是否AJAX请求
            bool isAjaxRequest = filterContext.RequestContext.HttpContext.Request.IsAjaxRequest();

            if (filterContext.Exception is HttpRequestValidationException)
            {
                if(isAjaxRequest)
                {
                    filterContext.Result = new JsonResult()
                    {
                        Data = new
                        {
                            result = new ActionResultData
                            {
                                IsSucceed = false,
                                IsException = true,
                                Title = "System Error",
                                Detail = "你当前输入内容可能包含非法字符，请重新输入"
                            }
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    ViewResult result = new ViewResult() { ViewName = "Error" }; result.ViewBag.ErrorMsg = "你当前输入内容可能包含非法字符，请重新输入";
                    filterContext.Result = result;
                }
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }

            else if(filterContext.Exception is AssertException)
            {
                AssertException assertException = filterContext.Exception as AssertException;

                if (isAjaxRequest)
                {
                    filterContext.Result = new JsonResult()
                    {
                        Data = new
                        {
                            result = new ActionResultData
                            {
                                IsSucceed = false,
                                Title = "操作失败",
                                Detail = string.Join("\r\n", assertException.ExceptionMessageList ?? new List<string> { "" })
                            }
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    ViewResult result = new ViewResult() { ViewName = "Error" }; result.ViewBag.ErrorMsg = string.Join("\r\n", assertException.ExceptionMessageList);
                    filterContext.Result = result;
                }
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }

            else if(filterContext.Exception is DisplayableException)
            {
                DisplayableException dispalyableException = filterContext.Exception as DisplayableException;
               
                if (isAjaxRequest)
                {
                    filterContext.Result = new JsonResult()
                    {
                        Data = new ActionResultData
                              {
                                IsSucceed = false,
                                Title = "操作失败",
                                Detail = dispalyableException.Message
                              },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    ViewResult result = new ViewResult() { ViewName = "Error" }; result.ViewBag.ErrorMsg = dispalyableException.Message;
                    filterContext.Result = result;
                }
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = 400;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }

            else if(filterContext.Exception is HttpAntiForgeryException)
            {
                ViewResult result = new ViewResult() { ViewName = "Error" }; result.ViewBag.ErrorMsg = "发现恶意请求，请重试";
                filterContext.Result = result;

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }

            else if (filterContext.Exception is UnauthorizedAccessException)
            {
                filterContext.Result = new AuthenticationFailureResult();

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }

            else
            {
                ViewResult result = new ViewResult() { ViewName = "Error" }; result.ViewBag.ErrorMsg = "系统出错";
                filterContext.Result = result;

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }
        }
    }
}