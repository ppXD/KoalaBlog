using KoalaBlog.ApiClient;
using KoalaBlog.ApiClient.Extensions;
using KoalaBlog.Framework.Common;
using KoalaBlog.Framework.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KoalaBlog.Web.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class KoalaBlogAuthorizeAttribute : AuthorizeAttribute
    {
        private string[] rolesOrPermissionsName = null;

        private Dictionary<string, string[]> cacheDic = new Dictionary<string, string[]>();

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("HttpContext");
            }
            if (rolesOrPermissionsName != null)
            {
                bool isUserInRoleOrHasPermission = ClientContext.Clients.CreateSecurityClient().IsUserInRole(httpContext.User.Identity.Name, rolesOrPermissionsName);

                if(!isUserInRoleOrHasPermission)
                {
                    return false;
                }
            }
            return true;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            var controllerDescriptor = filterContext.ActionDescriptor.ControllerDescriptor;
            var actionDescriptor = filterContext.ActionDescriptor;

            var cacheKey = controllerDescriptor.ControllerName + "." + actionDescriptor.ActionName;

            //persistent property
            rolesOrPermissionsName = null;

            if (!cacheDic.ContainsKey(cacheKey))
            {
                var attrs = actionDescriptor.GetCustomAttributes(typeof(RequireRolesOrPermissionsAttribute), false);
                if (attrs.Length == 1)
                {
                    rolesOrPermissionsName = ((RequireRolesOrPermissionsAttribute)attrs[0]).RolesOrPermissionsName;
                }
                else
                {
                    attrs = controllerDescriptor.GetCustomAttributes(typeof(RequireRolesOrPermissionsAttribute), false);
                    if (attrs.Length == 1)
                    {
                        rolesOrPermissionsName = ((RequireRolesOrPermissionsAttribute)attrs[0]).RolesOrPermissionsName;
                    }
                }
                if (rolesOrPermissionsName != null)
                {
                    cacheDic[cacheKey] = rolesOrPermissionsName;
                }
            }
            else
            {
                rolesOrPermissionsName = cacheDic[cacheKey];
            }
            
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            ViewResult result = new ViewResult() { ViewName = "Error" }; 
            result.ViewBag.ErrorMsg = "AccessDenied";

            filterContext.Result = result;           
        }
    }
}
