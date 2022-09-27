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
                    tblAppointment appointment = new tblAppointment()
                    {
                        CreatedBy = model.CreatedBy,
                        UserCarID = (model.UserCarID != null) ? model.UserCarID : null,
                        ShowroomCarID = (model.ShowroomCarID != null) ? model.ShowroomCarID : null,
                        ShowroomInterestedID = (model.ShowroomInterestedID != null) ? model.ShowroomInterestedID : null,
                        UserInterestedID = (model.UserInterestedID != null) ? model.UserInterestedID : null
                    };

                    var reas = appointmentDb.InsertAppointment(appointment);
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

        public bool ChangeAppointmentToAsRead(int ShowroomID)
        {
            if (ShowroomID > 0)
            {
                var reas = appointmentDb.ChangeAppointmentToAsRead(ShowroomID);
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
