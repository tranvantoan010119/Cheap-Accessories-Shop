using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Customers
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Liên hệ",
                url: "lien-he",
                defaults: new { controller = "Home", action = "ContactUs" }
            );

            routes.MapRoute(
                name: "Giới thiệu",
                url: "gioi-thieu",
                defaults: new { controller = "Home", action = "AboutUs"}
            );

            routes.MapRoute(
                name: "Đăng nhập",
                url: "dang-nhap",
                defaults: new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                name: "Đăng ký",
                url: "dang-ky",
                defaults: new { controller = "Account", action = "Register" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
