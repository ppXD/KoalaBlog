using System;
using System.Web.Mvc;

namespace KoalaBlog.Framework.MVC
{
    public class AuthenticationFailureResult : ActionResult
    {
        private const string DefaultRedirectUrl = "/login";

        public AuthenticationFailureResult()
            : this(DefaultRedirectUrl)
        {
        }

        public AuthenticationFailureResult(string url)
            : this(url, false)
        {
        }

        public AuthenticationFailureResult(string url, bool permanent)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            this.Permanent = permanent;
            this.Url = url;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (context.IsChildAction)
            {
                throw new InvalidOperationException("RedirectAction_CannotRedirectInChildAction");
            }
            string url = UrlHelper.GenerateContentUrl(this.Url, context.HttpContext);
            context.Controller.TempData.Keep();
            if (this.Permanent)
            {
                context.HttpContext.Response.RedirectPermanent(url, false);
            }
            else
            {
                context.HttpContext.Response.Redirect(url, false);
            }
        }

        public bool Permanent { get; private set; }

        public string Url { get; private set; }
    }
}
