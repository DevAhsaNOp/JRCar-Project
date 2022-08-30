using System;
using JRCar.BOL;
using System.Linq;
using System.Configuration;
using System.Data.SqlClient;
using JRCar.WebApp.Controllers;
using System.Collections.Generic;
using System.IO;

namespace JRCar.WebApp
{
    public class NotificationComponent
    {
        public void RegisterNotification(DateTime currentTime)
        {
            PortalController pc = new PortalController();
            string constring = ConfigurationManager.ConnectionStrings["jrcarNotification"].ConnectionString;
            string SqlCmd = String.Empty;
            //([CreatedOn] > @CreatedOn or IsRead <> 1)
            SqlCmd = @"SELECT [ID] ,[Title] ,[Description] ,[AdURL] ,[FromUserID] ,[FromShowroomID] ,[IsShowroomInterested] ,[IsRead] ,[CreatedOn] FROM [dbo].[tblNotification] WHERE ([CreatedOn] > @CreatedOn or IsRead <> 1)";

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
            if (e.Info == SqlNotificationInfo.Update)
            {
                SqlDependency sql = sender as SqlDependency;
                sql.OnChange -= sqlDep_OnChange;
                NotificationHub.Show();
                RegisterNotification(DateTime.Now);
            }
            else if (e.Info == SqlNotificationInfo.Insert)
            {
                SqlDependency sql = sender as SqlDependency;
                sql.OnChange -= sqlDep_OnChange;
                NotificationHub.Show();
                RegisterNotification(DateTime.Now);
            }
        }

        public List<NotiShow> GetNotifications(DateTime afterDate, int ShowroomID)
        {
            using (jrcarEntities jrcar = new jrcarEntities())
            {
                var reas = jrcar.tblNotifications.Where(a => (a.CreatedOn > afterDate || a.IsRead == false) && (a.FromShowroomID == ShowroomID)).OrderByDescending(a => a.CreatedOn).Select(x => new NotiShow() { Title = x.Title, Description = x.Description }).ToList();
                return reas;
            }
        }

        public int GetNotificationsCount(DateTime afterDate, int ShowroomID)
        {
            using (jrcarEntities jrcar = new jrcarEntities())
            {
                var reas = jrcar.tblNotifications.Where(a => a.IsRead == false && a.FromShowroomID == ShowroomID).Select(x => new NotiShow() { Title = x.Title, Description = x.Description }).ToList().Count;
                return reas;
            }
        }

        public bool ChangeNotificationToAsRead(int ShowroomID)
        {
            using (jrcarEntities jrcar = new jrcarEntities())
            {
                var reas = jrcar.tblNotifications.Where(a => a.IsRead == false && a.FromShowroomID == ShowroomID).ToList();
                if (reas.Count > 0)
                {
                    foreach (var notification in reas)
                    {
                        try
                        {
                            notification.ID = notification.ID;
                            notification.Title = notification.Title;
                            notification.Description = notification.Description;
                            notification.AdURL = notification.AdURL;
                            notification.FromUserID = notification.FromUserID;
                            notification.FromShowroomID = notification.FromShowroomID;
                            notification.IsShowroomInterested = notification.IsShowroomInterested;
                            notification.IsRead = true;
                            notification.CreatedOn = notification.CreatedOn;
                            jrcar.Entry(notification).State = System.Data.Entity.EntityState.Modified;
                            jrcar.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            return false;
                            throw ex;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    public class NotiShow
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? IsRead { get; set; }
    }
}