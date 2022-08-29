using JRCar.WebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JRCar.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string con = ConfigurationManager.ConnectionStrings["jrcarNotification"].ConnectionString;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            SqlDependency.Start(con);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            NotificationComponent NC = new NotificationComponent();
            var currentTime = DateTime.Now;
            HttpContext.Current.Session["LastUpdated"] = currentTime;
            NC.RegisterNotification(currentTime);
        }


        protected void Application_End()
        {
            //here we will stop Sql Dependency
            SqlDependency.Stop(con);
        }

        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }
    }
}
