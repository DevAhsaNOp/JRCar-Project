using System;
using JRCar.BOL;
using System.Linq;
using System.Configuration;
using System.Data.SqlClient;
using JRCar.WebApp.Controllers;
using System.Collections.Generic;
using System.IO;
using JRCar.BLL.Repositories;
using JRCar.DAL.DBLayer;

namespace JRCar.WebApp
{
    public class NotificationComponent
    {
        private NotificationRepo repo = new NotificationRepo();

        public void RegisterNotification(DateTime currentTime)
        {
            PortalController pc = new PortalController();
            string constring = ConfigurationManager.ConnectionStrings["jrcarNotification"].ConnectionString;
            SqlDependency.Start(constring);
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
                sql.OnChange += new OnChangeEventHandler(sqlDep_OnChange);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                }
            }
        }

        void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info == SqlNotificationInfo.Update)
            {
                //SqlDependency sql = sender as SqlDependency;
                //sql.OnChange -= sqlDep_OnChange;
                NotificationHub.Show();
                RegisterNotification(DateTime.Now);
            }
            else if (e.Info == SqlNotificationInfo.Insert)
            {
                //SqlDependency sql = sender as SqlDependency;
                //sql.OnChange -= sqlDep_OnChange;
                NotificationHub.Show();
                RegisterNotification(DateTime.Now);
            }
        }

        public List<NotiShow> GetNotifications(DateTime afterDate, int ShowroomID)
        {
            var reas = repo.GetNotifications(afterDate, ShowroomID);
            return reas;
        }

        public int GetNotificationsCount(DateTime afterDate, int ShowroomID)
        {
            var reas = repo.GetNotificationsCount(afterDate, ShowroomID);
            return reas;
        }

        public bool ChangeNotificationToAsRead(int ShowroomID)
        {

            if (ShowroomID > 0)
            {
                var reas = repo.ChangeNotificationToAsRead(ShowroomID);
                return reas;
            }
            else
            {
                return false;
            }
        }
    }
}