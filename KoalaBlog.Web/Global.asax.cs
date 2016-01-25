using KoalaBlog.Entity.Models;
using KoalaBlog.Framework.Extensions;
using KoalaBlog.Framework.Common;
using KoalaBlog.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using KoalaBlog.Web.Controllers;

namespace KoalaBlog.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Fires when an global error occurs.
            //other error will handle by exception handler attribute.
            string errStrHeader = "============================= Website Error =========================== \r\n";
            string content = string.Empty;
            Exception ex = Server.GetLastError();

            content = errStrHeader + "WebSite Error: " + ex.Message + ex.StackTrace + "\r\n";
            while(ex.InnerException != null)
            {
                ex = ex.InnerException;
                content += "InnerException:" + "\r\n";
            }

            //log4net

        }

    }
}