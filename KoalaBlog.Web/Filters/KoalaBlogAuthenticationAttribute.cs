using KoalaBlog.ApiClient;
using KoalaBlog.ApiClient.Extensions;
using KoalaBlog.Framework.Common;
using KoalaBlog.Framework.Extensions;
using KoalaBlog.Framework.MVC;
using KoalaBlog.Principal;
using KoalaBlog.Security;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace KoalaBlog.Web.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class KoalaBlogAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if(filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            //1. If there are define allow Anonymous attribute, do nothing.
            if (IsDefinedAllowAnonymous(filterContext))
            {
                return;
            }

            //2. If there are no credentials, set the error result.
            if(string.IsNullOrEmpty(KoalaBlogSecurityManager.GetAuthCookie()))
            {
                filterContext.Result = new AuthenticationFailureResult();
            }
            else
            {
                //3. Check the credentials.
                KoalaBlogIdentityObject identityObj = ClientContext.Clients.CreateSecurityClient().GetIdentityObj();

                //4. If the credentials are bad, set the error result.
                if(identityObj == null)
                {
                    filterContext.Result = new AuthenticationFailureResult();
                }
                else
                {
                    KoalaBlogIdentity identity = new KoalaBlogIdentity(identityObj);
                    KoalaBlogPrincipal principal = new KoalaBlogPrincipal(identity);
                    filterContext.Principal = principal;
                }
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            
        }

        private bool IsDefinedAllowAnonymous(AuthenticationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            return filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
        }
    }
}