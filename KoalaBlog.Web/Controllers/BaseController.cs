using KoalaBlog.Framework.MVC;
using KoalaBlog.Principal;
using KoalaBlog.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KoalaBlog.Web.Controllers
{
    public class BaseController : Controller
    {
        private KoalaService _service;

        protected KoalaBlogIdentityObject CurrentThreadIdentityObject
        {
            get
            {
                if (System.Threading.Thread.CurrentPrincipal.Identity is KoalaBlogIdentity)
                {
                    return (System.Threading.Thread.CurrentPrincipal.Identity as KoalaBlogIdentity).IdentityObject;
                }
                return null;
            }
        }

        protected KoalaService Services
        {
            get
            {
                return this._service ?? (this._service = KoalaService.Create());
            }
        }
    }
}