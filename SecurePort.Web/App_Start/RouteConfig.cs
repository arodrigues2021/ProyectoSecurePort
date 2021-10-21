using System.Web.Mvc;
using System.Web.Routing;

namespace SecurePort.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Login",
               url: "Login",
               defaults: new { controller = "Login", action = "Index" });

            routes.MapRoute(
               name: "SecurePort",
               url: "Secureport",
               defaults: new { controller = "Home", action = "Index"});

            routes.MapRoute(
               name: "Menu",
               url: "Menu",
               defaults: new { controller = "Menu", action = "Index" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }

    }
}
