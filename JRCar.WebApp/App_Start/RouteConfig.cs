using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JRCar.WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Website", action = "Index", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "EditVehicle",
                url: "Ads/{action}/{AdID}",
                defaults: new { controller = "Website", action = "EditVehicle", AdID = UrlParameter.Optional }
            );

        }
    }
}
