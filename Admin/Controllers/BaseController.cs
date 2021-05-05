using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session[Shared.Session_Admin] == null)
                filterContext.Result = new RedirectResult(UrlHandler.ResolveUrl("/Account/Login"));
            base.OnActionExecuting(filterContext);
        }
    }
}