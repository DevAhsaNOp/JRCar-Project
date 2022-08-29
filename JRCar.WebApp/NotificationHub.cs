using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace JRCar.WebApp
{
    public class NotificationHub : Hub
    {
        public static void Show()
        {
            var notificationhub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            notificationhub.Clients.All.DisplayNoti("added");
        }
    }
}