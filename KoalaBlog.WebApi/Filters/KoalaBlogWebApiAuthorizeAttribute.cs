using KoalaBlog.Framework.Filters;
using KoalaBlog.WebApi.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Filters;

namespace KoalaBlog.WebApi.Filters
{
    public class KoalaBlogWebApiAuthorizeAttribute : IAuthorizationFilter
    {
        private string[] rolesOrPermissionsName;

        private Dictionary<string, string[]> cacheDic = new Dictionary<string, string[]>();

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> ExecuteAuthorizationFilterAsync(System.Web.Http.Controllers.HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken, Func<System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage>> continuation)
        {
            if(actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }
            if(continuation == null)
            {
                throw new ArgumentNullException("continuation");
            }

            var controllerDescriptor = actionContext.ActionDescriptor.ControllerDescriptor;
            var actionDescriptor = actionContext.ActionDescriptor;

            var cacheKey = controllerDescriptor.ControllerName + "." + actionDescriptor.ActionName;

            //persistent property
            rolesOrPermissionsName = null;

            if(!cacheDic.ContainsKey(cacheKey))
            {
                var attrs = actionDescriptor.GetCustomAttributes<RequireRolesOrPermissionsAttribute>(false);
                if (attrs.Count == 1)
                {
                    rolesOrPermissionsName = ((RequireRolesOrPermissionsAttribute)attrs[0]).RolesOrPermissionsName;
                }
                else
                {
                    attrs = controllerDescriptor.GetCustomAttributes<RequireRolesOrPermissionsAttribute>(false);
                    if (attrs.Count == 1)
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

            if (rolesOrPermissionsName != null)
            {
                bool isAuthorized = await new SecurityManager().IsUserInRoleAsync("admin", rolesOrPermissionsName);

                if(!isAuthorized)
                {
                    string controller = actionDescriptor.ControllerDescriptor.ControllerName;
                    string action = actionDescriptor.ActionName;

                    string sourceObj = String.Format("KoalaBlog.WebApi.Controllers.{0}Controller.{1}", controller, action);

                    System.Threading.Tasks.Task logTask = new SecurityManager().WriteLogAsync(Entity.Models.Enums.LogLevel.WARNING, sourceObj, "Forbidden");

                    //create forbidden response
                    System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.Forbidden,
                        RequestMessage = actionContext.Request
                    };

                    await logTask;

                    return response;
                }
            }

            return await continuation();
        }

        public bool AllowMultiple
        {
            get { return true; }
        }
    }
}