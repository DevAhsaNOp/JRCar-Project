using JRCar.BOL;
using JRCar.DAL.DBLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRCar.BLL.Repositories
{
    public class NotificationRepo
    {
        private NotificationDb DbObj;

        public NotificationRepo()
        {
            DbObj = new NotificationDb();
        }

        public bool InsertNotification(tblNotification model)
        {
            try
            {
                if (model != null)
                {
                    var reas = DbObj.InsertNotification(model);
                    return reas;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ChangeNotificationToAsRead(int ShowroomID)
        {
            if (ShowroomID > 0)
            {
                var reas = DbObj.ChangeNotificationToAsRead(ShowroomID);
                return reas;
            }
            else
            {
                return false;
            }
        }

        public List<NotiShow> GetNotifications(DateTime afterDate, int ShowroomID)
        {
            var reas = DbObj.GetNotifications(afterDate, ShowroomID);
            return reas;
        }

        public List<NotiShow> GetAllNotifications(int ShowroomID)
        {
            var reas = DbObj.GetAllNotifications(ShowroomID);
            return reas;
        }

        public int GetNotificationsCount(DateTime afterDate, int ShowroomID)
        {
            var reas = DbObj.GetNotificationsCount(afterDate, ShowroomID);
            return reas;
        }
        
        public bool InsertAnnouncements(tblAnnouncement model)
        {
            try
            {
                if (model != null)
                {
                    var reas = DbObj.InsertAnnouncements(model);
                    return reas;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ChangeAnnouncementsToAsRead(int ShowroomID)
        {
            if (ShowroomID > 0)
            {
                var reas = DbObj.ChangeAnnouncementsToAsRead(ShowroomID);
                return reas;
            }
            else
            {
                return false;
            }
        }

        public List<NotiShow> GetAnnouncements(DateTime afterDate, int ShowroomID)
        {
            var reas = DbObj.GetAnnouncements(afterDate, ShowroomID);
            return reas;
        }

        public List<NotiShow> GetAllAnnouncements(int ShowroomID)
        {
            var reas = DbObj.GetAllAnnouncements(ShowroomID);
            return reas;
        }

        public List<NotiShow> GetAllAnnouncement()
        {
            var reas = DbObj.GetAllAnnouncement();
            return reas;
        }

        public int GetAnnouncementsCount(DateTime afterDate, int ShowroomID)
        {
            var reas = DbObj.GetAnnouncementsCount(afterDate, ShowroomID);
            return reas;
        }
    }
}
