using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace JRCar.WebApp
{
    public class NotificationComponent
    {
        public void RegisterNotification(DateTime currentTime)
        {
            string constring = ConfigurationManager.ConnectionStrings["jrcarNotification"].ConnectionString;
            string SqlCommand = @"";
        }
    }
}