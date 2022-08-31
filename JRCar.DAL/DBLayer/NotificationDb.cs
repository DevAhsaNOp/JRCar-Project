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

        public List<NotiShow> GetNotifications(DateTime afterDate, int ShowroomID)
        {
            var reas = _context.tblNotifications.Where(a => (a.CreatedOn > afterDate || a.IsRead == false) && (a.FromShowroomID == ShowroomID)).OrderByDescending(a => a.CreatedOn).Select(x => new NotiShow() { Title = x.Title, Description = x.Description }).ToList();
            return reas;
        }

        public int GetNotificationsCount(DateTime afterDate, int ShowroomID)
        {
            var reas = _context.tblNotifications.Where(a => a.IsRead == false && a.FromShowroomID == ShowroomID).Select(x => new NotiShow() { Title = x.Title, Description = x.Description }).ToList().Count;
            return reas;
        }
    }

    public class NotiShow
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? IsRead { get; set; }
    }
}
