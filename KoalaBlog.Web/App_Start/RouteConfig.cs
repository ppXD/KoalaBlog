using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KoalaBlog.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            //Default
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { id = UrlParameter.Optional }
            );

            #region Account

            //Login
            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { controller = "Account", action = "Login" }
            );

            //LogOff
            routes.MapRoute(
                name: "LogOff",
                url: "logoff",
                defaults: new { controller = "Account", action = "LogOff" }
            );

            routes.MapRoute(
                name: "Register",
                url: "register",
                defaults: new { controller = "Account", action = "Register" }
            );

            routes.MapRoute(
                name: "LockOut",
                url: "lockout",
                defaults: new { controller = "Account", action = "LockedOut" }
            );

            #endregion

            #region Home

            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );

            #endregion
        }
    }
}
