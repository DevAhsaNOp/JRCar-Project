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
        public static void Insert(string writeText)
        {
            File.WriteAllText("E:\\JRCar-Project\\JRCar.WebApp\\filename.txt", writeText);
        }

        public static void InsertZero()
        {
            File.WriteAllText("E:\\JRCar-Project\\JRCar.WebApp\\filename.txt", "0");
        }

        public void RegisterNotification(DateTime currentTime)
        {
            var ID = File.ReadAllText("E:\\JRCar-Project\\JRCar.WebApp\\filename.txt"); // Read the contents of the file
            PortalController pc = new PortalController();
            string constring = ConfigurationManager.ConnectionStrings["jrcarNotification"].ConnectionString;
            string SqlCmd = String.Empty;
            int ShowroomID = Convert.ToInt32(ID);
            if (ShowroomID <= 0)
            {
                //(([CreatedOn] > @CreatedOn or IsRead <> 1)
                SqlCmd = @"SELECT [ID] ,[Title] ,[Description] ,[AdURL] ,[FromUserID] ,[FromShowroomID] ,[IsShowroomInterested] ,[IsRead] ,[CreatedOn] FROM [dbo].[tblNotification] WHERE IsRead <> 1";
            }
            else
            {
                SqlCmd = @"SELECT [ID] ,[Title] ,[Description] ,[AdURL] ,[FromUserID] ,[FromShowroomID] ,[IsShowroomInterested] ,[IsRead] ,[CreatedOn] FROM [dbo].[tblNotification] WHERE (IsRead <> 1 and FromShowroomID = @FromShowroomID)";
            }

            using (SqlConnection con = new SqlConnection(constring))
            {
                SqlCommand cmd = new SqlCommand(SqlCmd, con);
                //cmd.Parameters.AddWithValue("@CreatedOn", currentTime);
                if (ShowroomID > 0)
                {
                    cmd.Parameters.AddWithValue("@FromShowroomID", ShowroomID);
                }
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
    }

    public class NotiShow
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}