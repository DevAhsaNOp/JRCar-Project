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
using JRCar.BOL.Validation_Classes;
using System.Security.Policy;
using DocumentFormat.OpenXml.Spreadsheet;

namespace JRCar.WebApp
{
    public class NotificationComponent
    {
        private NotificationRepo repo = new NotificationRepo();
        private AppointmentRepo appointmentrepo = new AppointmentRepo();

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

        public void RegisterAppointment(DateTime currentTime)
        {
            PortalController pc = new PortalController();
            string constring = ConfigurationManager.ConnectionStrings["jrcarNotification"].ConnectionString;
            SqlDependency.Start(constring);
            string SqlCmd = String.Empty;
            SqlCmd = @"SELECT [ID] ,[UserInterestedID] ,[ShowroomInterestedID] ,[UserCarID] ,[ShowroomCarID] ,[Isactive] ,[CreatedBy] ,[CreatedOn] ,[UpdatedOn] ,[UpdatedBy] ,[IsAccepted] ,[IsRead] FROM [dbo].[tblAppointments] WHERE ([CreatedOn] > @CreatedOn or IsRead <> 1)";

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
                sql.OnChange += new OnChangeEventHandler(sqlDep_OnChangeAppointment);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                }
            }
        }

        public void RegisterAnnouncement(DateTime currentTime)
        {
            PortalController pc = new PortalController();
            string constring = ConfigurationManager.ConnectionStrings["jrcarNotification"].ConnectionString;
            SqlDependency.Start(constring);
            string SqlCmd = String.Empty;
            SqlCmd = @"SELECT [ID] ,[ShowroomID] ,[Title] ,[Description] ,[CreatedOn] ,[IsRead] FROM [dbo].[tblAnnouncements] WHERE ([CreatedOn] > @CreatedOn or IsRead <> 1)";

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
                sql.OnChange += new OnChangeEventHandler(sqlDep_OnChangeAnnouncement);
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

        void sqlDep_OnChangeAnnouncement(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info == SqlNotificationInfo.Update)
            {
                NotificationHub.Show();
                RegisterAnnouncement(DateTime.Now);
            }
            else if (e.Info == SqlNotificationInfo.Insert)
            {
                NotificationHub.Show();
                RegisterAnnouncement(DateTime.Now);
            }
        }

        void sqlDep_OnChangeAppointment(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                NotificationHub.Show();
                RegisterAppointment(DateTime.Now);
            }
            else if (e.Info == SqlNotificationInfo.Insert)
            {
                NotificationHub.Show();
                RegisterAppointment(DateTime.Now);
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

        public int GetUserTodaysAppointmentsCount(DateTime afterDate, int UserID)
        {
            var reas = appointmentrepo.GetUserTodaysAppointmentsCount(afterDate, UserID);
            return reas;
        }

        public int GetShowroomTodaysAppointmentsCount(DateTime afterDate, int ShowroomID)
        {
            var reas = appointmentrepo.GetShowroomTodaysAppointmentsCount(afterDate, ShowroomID);
            return reas;
        }

        public IEnumerable<NotiShow> GetShowroomAppointments(DateTime afterDate, int ShowroomID)
        {
            //var reas = appointmentrepo.GetShowroomAppointments(afterDate, ShowroomID).Select(x => new NotiShow() { Title = x.tblUser.Name.ToString(), Description = x.tblCar.tblManufacturer.Manufacturer_Name + " " + x.tblCar.tblManfacturerCarModel.Manufacturer_CarModelName, AdURL = x.ID.ToString() });
            var reas = appointmentrepo.GetShowroomAppointments(afterDate, ShowroomID);
            List<NotiShow> notis = new List<NotiShow>();
            if (reas != null)
            {
                foreach (var item in reas)
                {
                    if (item.tblCar != null)
                    {
                        NotiShow noti = new NotiShow();
                        noti.Title = "New Appointment Requested By" + item.tblUser.Name.ToString();
                        noti.Description = " of " + item.tblCar.tblManufacturer.Manufacturer_Name + " " + item.tblCar.tblManfacturerCarModel.Manufacturer_CarModelName;
                        noti.AdURL = item.ID.ToString();
                        notis.Add(noti);
                    }
                    else if (item.tblUserAdd != null)
                    {
                        NotiShow noti = new NotiShow();
                        if (item.IsAccepted.Value == false)
                        {
                            noti.Title = " Appointment Reject By " + item.tblUserAdd.tblUser.Name.ToString();
                        }
                        if (item.IsAccepted.Value == true)
                        {
                            noti.Title = " Appointment Appected By " + item.tblUserAdd.tblUser.Name.ToString();
                        }
                        noti.Description = " of " + item.tblUserAdd.tblManufacturer.Manufacturer_Name + " " + item.tblUserAdd.tblManfacturerCarModel.Manufacturer_CarModelName;
                        noti.AdURL = "false";
                        noti.IsAccpeted = item.IsAccepted.Value;
                        notis.Add(noti);
                    }
                }
                return notis;
            }
            else
                return null;
        }

        public IEnumerable<NotiShow> GetUserAppointments(DateTime afterDate, int UserID)
        {
            var reas = appointmentrepo.GetUserAppointments(afterDate, UserID);
            List<NotiShow> notis = new List<NotiShow>();
            if (reas != null)
            {
                foreach (var item in reas)
                {
                    if (item.tblCar != null)
                    {
                        NotiShow noti = new NotiShow();
                        if (item.IsAccepted.Value == false)
                        {
                            noti.Title = " Appointment Reject By " + item.tblCar.tblShowroom.FullName.ToString();
                        }
                        if (item.IsAccepted.Value == true)
                        {
                            noti.Title = " Appointment Appected By " + item.tblCar.tblShowroom.FullName.ToString();
                        }
                        noti.Description = " of " + item.tblCar.tblManufacturer.Manufacturer_Name + " " + item.tblCar.tblManfacturerCarModel.Manufacturer_CarModelName;
                        noti.AdURL = "false";
                        noti.IsAccpeted = item.IsAccepted.Value;
                        notis.Add(noti);
                    }
                    else if (item.tblUserAdd != null)
                    {
                        NotiShow noti = new NotiShow();
                        noti.Title = "New Appointment Requested By" + item.tblShowroom.FullName.ToString();
                        noti.Description = " of " + item.tblUserAdd.tblManufacturer.Manufacturer_Name + " " + item.tblUserAdd.tblManfacturerCarModel.Manufacturer_CarModelName;
                        noti.AdURL = item.ID.ToString();
                        noti.IsAccpeted = item.IsAccepted.Value;
                        notis.Add(noti);
                    }
                }
                return notis;
            }
            else
                return null;
        }

        public IEnumerable<ValidateAppointment> GetShowroomAppointmentsById(int ShowroomID)
        {
            var reas = appointmentrepo.GetShowroomAppointmentsById(ShowroomID);
            return reas;
        }

        public ValidateAppointment GetAppointmentById(int id)
        {
            try
            {
                if (id > 0)
                {
                    var appointment = appointmentrepo.GetAppointmentById(id);
                    if (appointment != null)
                        return appointment;
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NotiShow GetShowroomCurrAppointmentById(int id)
        {
            try
            {
                if (id > 0)
                {
                    var appnt = appointmentrepo.GetShowroomCurrAppointmentById(id);
                    NotiShow appointment = new NotiShow()
                    {
                        FromUserName = appnt.tblUser.Name,
                        Title = appnt.tblCar.tblManufacturer.Manufacturer_Name + " " + appnt.tblCar.tblManfacturerCarModel.Manufacturer_CarModelName,
                        Time = appnt.Datetime.ToString(),
                        Description = appnt.Purpose,
                        Date = appnt.CreatedOn.ToString(),
                        PhoneNumber = appnt.Number,
                        AdURL = appnt.tblCar.CarsURL,
                        CardID = appnt.ID.ToString()
                    };
                    if (appointment != null)
                        return appointment;
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NotiShow GetUserCurrAppointmentById(int id)
        {
            try
            {
                if (id > 0)
                {
                    var appnt = appointmentrepo.GetUserCurrAppointmentById(id);
                    NotiShow appointment = new NotiShow()
                    {
                        FromUserName = appnt.tblShowroom.FullName,
                        Title = appnt.tblUserAdd.tblManufacturer.Manufacturer_Name + " " + appnt.tblUserAdd.tblManfacturerCarModel.Manufacturer_CarModelName,
                        Time = appnt.Datetime.ToString(),
                        Description = appnt.Purpose,
                        Date = appnt.CreatedOn.ToString(),
                        PhoneNumber = appnt.Number,
                        AdURL = appnt.tblUserAdd.UserAdsURL,
                        CardID = appnt.ID.ToString()
                    };
                    if (appointment != null)
                        return appointment;
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ChangeShowroomAppointmentToAsRead(int ShowroomID)
        {
            if (ShowroomID > 0)
            {
                var reas = appointmentrepo.ChangeShowroomAppointmentToAsRead(ShowroomID);
                return reas;
            }
            else
            {
                return false;
            }
        }

        public bool ChangeUserAppointmentToAsRead(int UserID)
        {
            if (UserID > 0)
            {
                var reas = appointmentrepo.ChangeUserAppointmentToAsRead(UserID);
                return reas;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<ValidateAppointment> GetUserAppointmentsById(int UserID)
        {
            var reas = appointmentrepo.GetUserAppointmentsById(UserID);
            return reas;
        }

        public List<NotiShow> GetAllNotifications(int ShowroomID)
        {
            var reas1 = repo.GetAllNotifications(ShowroomID);
            var reas2 = repo.GetAllAnnouncements(ShowroomID);
            var reas = reas1.Concat(reas2).OrderByDescending(x => x.CreatedOn).ToList();
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

        public List<NotiShow> GetAnnouncements(DateTime afterDate, int ShowroomID)
        {
            var reas = repo.GetAnnouncements(afterDate, ShowroomID);
            return reas;
        }

        public int GetAnnouncementsCount(DateTime afterDate, int ShowroomID)
        {
            var reas = repo.GetAnnouncementsCount(afterDate, ShowroomID);
            return reas;
        }

        public List<NotiShow> GetAllAnnouncements(int ShowroomID)
        {
            var reas = repo.GetAllAnnouncements(ShowroomID);
            return reas;
        }

        public List<NotiShow> GetAllAnnouncement()
        {
            var reas = repo.GetAllAnnouncement();
            return reas;
        }

        public bool ChangeAnnouncementsToAsRead(int ShowroomID)
        {

            if (ShowroomID > 0)
            {
                var reas = repo.ChangeAnnouncementsToAsRead(ShowroomID);
                return reas;
            }
            else
            {
                return false;
            }
        }
    }
}