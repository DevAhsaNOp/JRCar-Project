using JRCar.BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRCar.DAL.DBLayer
{
    public class NotificationDb
    {
        private jrcarEntities _context;

        public NotificationDb()
        {
            _context = new jrcarEntities();
        }

        public bool InsertNotification(tblNotification model)
        {
            try
            {
                if (model != null)
                {
                    UserDb userDb = new UserDb();
                    var reas = userDb.GetAllShowRoom();
                    foreach (var item in reas)
                    {
                        model.FromShowroomID = item.ID;
                        model.IsShowroomInterested = false;
                        model.IsRead = false;
                        model.CreatedOn = DateTime.Now;
                        _context.tblNotifications.Add(model);
                        Save();
                    }
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public bool InsertAnnouncements(tblAnnouncement model)
        {
            try
            {
                if (model != null)
                {
                    UserDb userDb = new UserDb();
                    var reas = userDb.GetAllShowRoom();
                    foreach (var item in reas)
                    {
                        model.ShowroomID = item.ID;
                        model.IsRead = false;
                        model.CreatedOn = DateTime.Now;
                        _context.tblAnnouncements.Add(model);
                        Save();
                    }
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public bool ChangeNotificationToAsRead(int ShowroomID)
        {
            var reas = _context.tblNotifications.Where(a => a.IsRead == false && a.FromShowroomID == ShowroomID).ToList();
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
                        _context.Entry(notification).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();
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
        
        public bool ChangeAnnouncementsToAsRead(int ShowroomID)
        {
            var reas = _context.tblAnnouncements.Where(a => a.IsRead == false && a.ShowroomID == ShowroomID).ToList();
            if (reas.Count > 0)
            {
                foreach (var announcement in reas)
                {
                    try
                    {
                        announcement.ID = announcement.ID;
                        announcement.Title = announcement.Title;
                        announcement.Description = announcement.Description;
                        announcement.ShowroomID = announcement.ShowroomID;
                        announcement.IsRead = true;
                        announcement.CreatedOn = announcement.CreatedOn;
                        _context.Entry(announcement).State = System.Data.Entity.EntityState.Modified;
                        Save();
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

        public List<NotiShow> GetNotifications(DateTime afterDate, int ShowroomID)
        {
            var reas = _context.tblNotifications.Where(a => (a.CreatedOn > afterDate || a.IsRead == false) && (a.FromShowroomID == ShowroomID)).OrderByDescending(a => a.CreatedOn).Select(x => new NotiShow() { Title = x.Title, Description = x.Description, AdURL = x.AdURL }).ToList();
            return reas;
        }
        
        public List<NotiShow> GetAnnouncements(DateTime afterDate, int ShowroomID)
        {
            var reas = _context.tblAnnouncements.Where(a => (a.CreatedOn > afterDate || a.IsRead == false) && (a.ShowroomID == ShowroomID)).OrderByDescending(a => a.CreatedOn).Select(x => new NotiShow() { Title = x.Title, Description = x.Description, CreatedOn = x.CreatedOn }).ToList();
            return reas;
        }

        public List<NotiShow> GetAllNotifications(int ShowroomID)
        {
            var reas = _context.tblNotifications.Where(a => a.FromShowroomID == ShowroomID).OrderByDescending(a => a.CreatedOn).Select(x => new NotiShow() { Title = x.Title, Description = x.Description, AdURL = x.AdURL, CreatedOn = x.CreatedOn, FromUserName = x.tblUser.Name, IsNoti = true, IsAnno = false }).ToList();
            return reas;
        }
        
        public List<NotiShow> GetAllAnnouncements(int ShowroomID)
        {
            var reas = _context.tblAnnouncements.Where(a => a.ShowroomID == ShowroomID).OrderByDescending(a => a.CreatedOn).Select(x => new NotiShow() { Title = x.Title, Description = x.Description, AdURL = null, CreatedOn = x.CreatedOn, FromUserName = "JRCar Union", IsAnno = true, IsNoti = false }).ToList();
            return reas;
        }

        public List<NotiShow> GetAllAnnouncement()
        {
            var reas = _context.ANNOVIEWs.OrderByDescending(a => a.Time).Select(x => new NotiShow() { Title = x.Title, Description = x.Description, Time = x.Time, Date = x.Date, FromUserName = "JRCar Union" }).ToList();
            return reas;
        }

        public int GetNotificationsCount(DateTime afterDate, int ShowroomID)
        {
            var reas = _context.tblNotifications.Where(a => a.IsRead == false && a.FromShowroomID == ShowroomID).Select(x => new NotiShow() { Title = x.Title, Description = x.Description, AdURL = x.AdURL }).ToList().Count;
            return reas;
        }
        
        public int GetAnnouncementsCount(DateTime afterDate, int ShowroomID)
        {
            var reas = _context.tblAnnouncements.Where(a => a.IsRead == false && a.ShowroomID == ShowroomID).Select(x => new NotiShow() { Title = x.Title, Description = x.Description}).ToList().Count;
            return reas;
        }
    }

    public class NotiShow
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string AdURL { get; set; }
        public string CardID { get; set; }
        public bool? IsRead { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
        public string PhoneNumber { get; set; }
        public string FromUserName { get; set; }
        public bool IsNoti { get; set; }
        public bool IsAnno { get; set; }
        public bool IsAccpeted { get; set; }
        public virtual tblShowroom tblShowroom { get; set; }
        public virtual tblUser tblUser { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string IsUserAppnt { get; set; }
    }
}
