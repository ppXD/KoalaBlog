using KoalaBlog.Framework.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KoalaBlog.Framework.Filters;

namespace KoalaBlog.Web.Controllers
{
    public class HomeController : BaseController
    {
        //[RequireRolesOrPermissions(SecurityPermissionStore.System_User_Group, SecurityPermissionStore.System_User_Member)]
        public ActionResult Index()
        {
            return View();
        }
    }
}