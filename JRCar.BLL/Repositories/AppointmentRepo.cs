using JRCar.BOL.Validation_Classes;
using JRCar.BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using JRCar.DAL.DBLayer;

namespace JRCar.BLL.Repositories
{
    public class AppointmentRepo
    {
        private AppointmentDb appointmentDb;

        public AppointmentRepo()
        {
            appointmentDb = new AppointmentDb();
        }

        public bool InsertAppointment(ValidateAppointment model)
        {
            try
            {
                if (model != null)
                {
                    int reas = 0;
                    tblAppointment appointment = new tblAppointment()
                    {
                        CreatedBy = model.CreatedBy,
                        UserCarID = (model.UserCarID != null) ? model.UserCarID : null,
                        ShowroomCarID = (model.ShowroomCarID != null) ? model.ShowroomCarID : null,
                        ShowroomInterestedID = (model.ShowroomInterestedID != null) ? model.ShowroomInterestedID : null,
                        UserInterestedID = (model.UserInterestedID != null) ? model.UserInterestedID : null,
                        ConfirmDatetime = model.Datetime
                    };
                    if (model.UserInterestedID != null)
                    {
                        reas = appointmentDb.InsertAppointment(appointment);
                    }
                    else
                    {
                        reas = appointmentDb.InsertUserAppointment(appointment);
                    }
                    if (reas > 0)
                    {
                        var appntreas = InsertAppointmentDetails(model, reas);
                        if (appntreas)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ShowroomContact(string FullName, string Email, string PhoneNumber, string Message, int UserID, int ShowroomID)
        {
            try
            {
                if (FullName.Length > 1 && PhoneNumber.Length > 1 && Message.Length > 1 && UserID > 0 && ShowroomID > 0)
                {
                    
                    tblQuery query = new tblQuery()
                    {
                        UserID = UserID,
                        ShowroomID = ShowroomID,
                        FullName = FullName,
                        Email = Email,
                        PhoneNumber = PhoneNumber,
                        Message = Message,
                        CreatedBy = UserID,
                    };
                    var reas = appointmentDb.ShowroomContact(query);
                    if (reas.Length > 0)
                    {
                        return reas;
                    }
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

        public bool InsertAppointmentDetails(ValidateAppointment model, int AppntID)
        {
            try
            {
                if (model != null)
                {
                    var ShowroomID = 0;
                    var UserID = 0;
                    if (model.ShowroomInterestedID == null)
                    {
                        ShowroomAdsRepo adsRepo = new ShowroomAdsRepo();
                        ShowroomID = adsRepo.GetShowroomID(model.ShowroomCarID.Value);
                    }

                    if (model.UserInterestedID == null)
                    {
                        UserAdsRepo adsRepo = new UserAdsRepo();
                        UserID = adsRepo.GetUserID(model.UserCarID.Value);
                    }

                    ShowroomID = (model.ShowroomInterestedID == null) ? ShowroomID : model.ShowroomInterestedID.Value;
                    UserID = (model.UserInterestedID == null) ? UserID : model.UserInterestedID.Value;

                    tblAppointmentDetail appointmentDetail = new tblAppointmentDetail()
                    {
                        AppointmentID = (model.ID > 0) ? model.ID : AppntID,
                        ShowroomID = ShowroomID,
                        UserID = UserID,
                        Email = model.Email,
                        Number = model.Number,
                        Purpose = model.Purpose,
                        Date = model.Datetime,
                        CreatedBy = model.CreatedBy,
                    };
                    var reas = appointmentDb.InsertAppointmentDetails(appointmentDetail);

                    if (reas > 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AcceptAppointment(int AppntID, int Usr, string Purpose, string Date, bool IsAppntDel)
        {
            try
            {
                if (AppntID > 0 && Usr > 0 && IsAppntDel == false)
                {
                    tblAppointmentDetail AppntDetail = new tblAppointmentDetail();
                    AppntDetail = null;
                    var reas = appointmentDb.AcceptAppointment(AppntID, Usr, AppntDetail);
                    if (reas)
                        return true;
                    else
                        return false;
                }
                else if (AppntID > 0 && Usr > 0 && Purpose.Length > 0 && Date.Length > 0 && IsAppntDel == true)
                {
                    var Appnt = appointmentDb.GetShowById(AppntID);
                    tblAppointmentDetail AppntDetail = new tblAppointmentDetail()
                    {
                        AppointmentID = AppntID,
                        ShowroomID = Appnt.ShowroomID,
                        UserID = Appnt.UserID,
                        CreatedBy = Usr,
                        Date = Convert.ToDateTime(Date),
                        Purpose = Purpose,
                        Number = Appnt.ShowroomContact,
                        Email = Appnt.ShowroomEmail,
                    };
                    var reas = appointmentDb.AcceptAppointment(AppntID, Usr, AppntDetail);
                    if (reas)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AcceptUserAppointment(int AppntID, int Usr, string Purpose, string Date, bool IsAppntDel)
        {
            try
            {
                if (AppntID > 0 && Usr > 0 && IsAppntDel == false)
                {
                    tblAppointmentDetail AppntDetail = new tblAppointmentDetail();
                    AppntDetail = null;
                    var reas = appointmentDb.AcceptUserAppointment(AppntID, Usr, AppntDetail);
                    if (reas)
                        return true;
                    else
                        return false;
                }
                else if (AppntID > 0 && Usr > 0 && Purpose.Length > 0 && Date.Length > 0 && IsAppntDel == true)
                {
                    var Appnt = appointmentDb.GetUserById(AppntID);
                    tblAppointmentDetail AppntDetail = new tblAppointmentDetail()
                    {
                        AppointmentID = AppntID,
                        ShowroomID = Appnt.ShowroomID,
                        UserID = Appnt.UserID,
                        CreatedBy = Usr,
                        Date = Convert.ToDateTime(Date),
                        Purpose = Purpose,
                        Number = Appnt.UserContact,
                        Email = Appnt.UserEmail,
                    };
                    var reas = appointmentDb.AcceptUserAppointment(AppntID, Usr, AppntDetail);
                    if (reas)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsUserRequestThisCarAppointment(int UserID, int CarID)
        {
            try
            {
                if (UserID > 0 && CarID > 0)
                {
                    var reas = appointmentDb.IsUserRequestThisCarAppointment(UserID, CarID);
                    if (reas)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
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
                var reas = appointmentDb.ChangeShowroomAppointmentToAsRead(ShowroomID);
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
                var reas = appointmentDb.ChangeUserAppointmentToAsRead(UserID);
                return reas;
            }
            else
            {
                return false;
            }
        }
        
        public bool ChangeShowroomMessageToAsRead(int ShowroomID)
        {
            if (ShowroomID > 0)
            {
                var reas = appointmentDb.ChangeShowroomMessageToAsRead(ShowroomID);
                return reas;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateAppointment(tblAppointment model)
        {
            try
            {
                if (model != null)
                {
                    tblAppointment appointment = new tblAppointment()
                    {
                        //Date = model.Date,
                        //Email = model.Email,
                        //Number = model.Number,
                        //Purpose = model.Purpose,
                        UpdatedBy = model.UpdatedBy,
                        UserCarID = (model.UserCarID != null) ? model.UserCarID : null,
                        ShowroomCarID = (model.ShowroomCarID != null) ? model.ShowroomCarID : null,
                        ShowroomInterestedID = (model.ShowroomInterestedID != null) ? model.ShowroomInterestedID : null,
                        UserInterestedID = (model.UserInterestedID != null) ? model.UserInterestedID : null
                    };

                    var reas = appointmentDb.UpdateAppointment(appointment);
                    if (reas > 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool RejectAppointment(int AppntID, int Usr)
        {
            try
            {
                if (AppntID > 0 && Usr > 0)
                {
                    var reas = appointmentDb.RejectAppointment(AppntID, Usr);
                    if (reas > 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool RejectUserAppointment(int AppntID, int Usr)
        {
            try
            {
                if (AppntID > 0 && Usr > 0)
                {
                    var reas = appointmentDb.RejectUserAppointment(AppntID, Usr);
                    if (reas > 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ValidateAppointment GetAppointmentById(int id)
        {
            try
            {
                if (id > 0)
                {
                    var appointment = appointmentDb.GetAppointmentById(id);
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

        public ValidateAppointment GetShowroomCurrAppointmentById(int id)
        {
            try
            {
                if (id > 0)
                {
                    var appointment = appointmentDb.GetShowroomCurrAppointmentById(id);
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

        public ValidateAppointment GetUserCurrAppointmentById(int id)
        {
            try
            {
                if (id > 0)
                {
                    var appointment = appointmentDb.GetUserCurrAppointmentById(id);
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

        public IEnumerable<ValidateAppointment> GetShowroomAppointmentsById(int ShowroomID)
        {
            try
            {
                if (ShowroomID > 0)
                {
                    var appointment = appointmentDb.GetShowroomAppointmentsById(ShowroomID);
                    if (appointment != null && appointment.Count() > 0)
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

        public IEnumerable<ValidateAppointment> GetUserAppointmentsById(int UserID)
        {
            try
            {
                if (UserID > 0)
                {
                    var appointment = appointmentDb.GetUserAppointmentsById(UserID);
                    if (appointment != null && appointment.Count() > 0)
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
        
        public IEnumerable<tblQuery> GetShowroomMessagesById(int ShowroomID)
        {
            try
            {
                if (ShowroomID > 0)
                {
                    var messages = appointmentDb.GetShowroomMessagesById(ShowroomID);
                    if (messages != null && messages.Count() > 0)
                        return messages;
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

        public int GetUserTodaysAppointmentsCount(DateTime currentDate, int UserID)
        {
            if (UserID > 0)
            {
                var reas = appointmentDb.GetUserTodaysAppointmentsCount(currentDate, UserID);
                return reas;
            }
            else
                return 0;
        }
        
        public int GetShowroomMessagesCount(int ShowroomID)
        {
            if (ShowroomID > 0)
            {
                var reas = appointmentDb.GetShowroomMessagesCount(ShowroomID);
                return reas;
            }
            else
                return 0;
        }

        public int GetShowroomTodaysAppointmentsCount(DateTime currentDate, int ShowroomID)
        {
            if (ShowroomID > 0)
            {
                var reas = appointmentDb.GetShowroomTodaysAppointmentsCount(currentDate, ShowroomID);
                return reas;
            }
            else
                return 0;
        }

        public IEnumerable<tblAppointment> GetUserAppointments(DateTime currentDate, int UserID)
        {
            if (UserID > 0)
            {
                var reas = appointmentDb.GetUserAppointments(currentDate, UserID);
                return reas;
            }
            else
                return null;
        }
        
        public IEnumerable<tblQuery> GetShowroomMessages(int ShowroomID)
        {
            if (ShowroomID > 0)
            {
                var reas = appointmentDb.GetShowroomMessages(ShowroomID);
                return reas;
            }
            else
                return null;
        }

        public IEnumerable<tblAppointment> GetShowroomAppointments(DateTime currentDate, int ShowroomID)
        {
            if (ShowroomID > 0)
            {
                var reas = appointmentDb.GetShowroomAppointments(currentDate, ShowroomID);
                return reas;
            }
            else
                return null;
        }
    }
}
