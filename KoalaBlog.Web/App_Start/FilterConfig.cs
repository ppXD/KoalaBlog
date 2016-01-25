using KoalaBlog.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KoalaBlog.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new KoalaBlogAuthenticationAttribute());
            filters.Add(new KoalaBlogAuthorizeAttribute());
            filters.Add(new KoalaBlogExceptionHandlerAttribute());
            filters.Add(new KoalaBlogJsonNetActionFilter());
        }
    }
}