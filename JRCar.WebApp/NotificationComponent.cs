using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.AspNet.SignalR;
using JRCar.BOL;

namespace JRCar.WebApp
{
    public class NotificationComponent
    {
        public void RegisterNotification(DateTime currentTime)
        {
            string constring = ConfigurationManager.ConnectionStrings["jrcarNotification"].ConnectionString;
            string SqlCmd = @"SELECT [ID] ,[Title] ,[Description] ,[FromUserID] ,[FromShowroomID] ,[IsShowroomInterested] ,[IsRead] ,[CreatedOn] FROM [dbo].[tblNotification] WHERE [CreatedOn] > @CreatedOn";

            using (SqlConnection con = new SqlConnection(constring))
            {
                SqlCommand cmd = new SqlCommand(SqlCmd, con);
                cmd.Parameters.AddWithValue("@CreatedOn", currentTime);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                SqlDependency sql = new SqlDependency(cmd);
                sql.OnChange += sqlDep_OnChange;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                }
            }
        }

        void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sql = sender as SqlDependency;
                sql.OnChange -= sqlDep_OnChange;

                var notificationhub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notificationhub.Clients.All.notify("added");
                RegisterNotification(DateTime.Now);
            }
        }

        public List<NotiShow> GetNotifications(DateTime afterDate)
        {
            using (jrcarEntities jrcar = new jrcarEntities())
            {
                return jrcar.tblNotifications.Where(a => a.CreatedOn > afterDate).OrderByDescending(a => a.CreatedOn).Select(x => new NotiShow() { Title = x.Title, Description=x.Description}).ToList();
            }
        }
    }

    public  class NotiShow
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}