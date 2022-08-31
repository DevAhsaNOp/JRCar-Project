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

        public int GetNotificationsCount(DateTime afterDate, int ShowroomID)
        {
            var reas = DbObj.GetNotificationsCount(afterDate, ShowroomID);
            return reas;
        }
    }
}
