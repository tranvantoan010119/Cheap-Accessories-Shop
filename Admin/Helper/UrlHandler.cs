using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    public class UrlHandler
    {
        public static string ResolveUrl(string contentPath)
        {
            var request = HttpContext.Current.Request;
            return string.Format("{0}://{1}{2}{3}", request.Url.Scheme, request.Url.Authority, (new System.Web.Mvc.UrlHelper(request.RequestContext)).Content("~"), contentPath);
        }
    }
}