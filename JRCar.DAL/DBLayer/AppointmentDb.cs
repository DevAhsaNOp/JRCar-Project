using JRCar.BOL.Validation_Classes;
using JRCar.BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRCar.DAL.DBLayer
{
    public class AppointmentDb
    {
        private jrcarEntities _context;

        public AppointmentDb()
        {
            _context = new jrcarEntities();
        }

        public int InsertAppointment(tblAppointment model)
        {
            try
            {
                if (model != null)
                {
                    model.IsAccepted = false;
                    model.IsRead = false;
                    model.IsUserRead = null;
                    model.Isactive = true;
                    model.CreatedOn = DateTime.Now;
                    model.UpdatedOn = null;
                    model.UpdatedBy = null;
                    _context.tblAppointments.Add(model);
                    Save();
                    return model.ID;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertAppointmentDetails(tblAppointmentDetail model)
        {
            try
            {
                if (model != null)
                {
                    model.CreatedOn = DateTime.Now;
                    model.UpdatedOn = null;
                    model.UpdatedBy = null;
                    _context.tblAppointmentDetails.Add(model);
                    Save();
                    return model.ID;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateAppointment(tblAppointment model)
        {
            try
            {
                if (model != null)
                {
                    model.CreatedOn = GetAppointmentById(model.ID).CreatedOn;
                    model.CreatedBy = GetAppointmentById(model.ID).CreatedBy;
                    model.UpdatedOn = DateTime.Now;
                    _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    Save();
                    return model.ID;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RejectAppointment(int AppntID, int Usr)
        {
            try
            {
                if (AppntID > 0)
                {
                    var model = _context.tblAppointments.Where(a => a.ID == AppntID).FirstOrDefault();
                    model.IsAccepted = false;
                    model.Isactive = false;
                    model.IsUserRead = false;
                    model.UpdatedBy = Usr;
                    model.UpdatedOn = DateTime.Now;
                    _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    Save();
                    return model.ID;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AcceptAppointment(int AppntID, int Usr, tblAppointmentDetail AppntDetailmodel)
        {
            try
            {
                if (AppntID > 0 && Usr > 0)
                {
                    var model = _context.tblAppointments.Where(a => a.ID == AppntID).FirstOrDefault();
                    model.IsAccepted = true;
                    model.IsUserRead = false;
                    model.Isactive = true;
                    model.UpdatedBy = Usr;
                    model.UpdatedOn = DateTime.Now;
                    model.ConfirmDatetime = ((AppntDetailmodel == null) ? model.tblAppointmentDetails.Where(x => x.AppointmentID == AppntID).FirstOrDefault().Date : AppntDetailmodel.Date);
                    _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    Save();
                    if (model.ID > 0 && AppntDetailmodel != null)
                    {
                        var reas = InsertAppointmentDetails(AppntDetailmodel);
                        if (reas > 0)
                            return true;
                        else
                            return false;
                    }
                    else if (model.ID > 0)
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
                    var reas = _context.tblAppointments.Where(x => x.UserInterestedID == UserID && x.ShowroomCarID == CarID).FirstOrDefault();
                    if (reas != null)
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
            var reas = _context.tblAppointments.Where(a => a.IsRead == false && a.tblCar.tblShowroomID == ShowroomID).ToList();
            if (reas.Count > 0)
            {
                foreach (var appointment in reas)
                {
                    try
                    {
                        appointment.IsRead = true;
                        appointment.UpdatedOn = DateTime.Now;
                        appointment.UpdatedBy = ShowroomID;
                        _context.Entry(appointment).State = System.Data.Entity.EntityState.Modified;
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

        public bool ChangeUserAppointmentToAsRead(int UserID)
        {
            var reas = _context.tblAppointments.Where(a => a.IsUserRead == false && a.UserInterestedID == UserID).ToList();
            if (reas.Count > 0)
            {
                foreach (var appointment in reas)
                {
                    try
                    {
                        appointment.IsUserRead = true;
                        appointment.UpdatedOn = DateTime.Now;
                        appointment.UpdatedBy = UserID;
                        _context.Entry(appointment).State = System.Data.Entity.EntityState.Modified;
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

        public ValidateAppointment GetAppointmentById(int id)
        {
            try
            {
                if (id > 0)
                {
                    var appointment = _context.tblAppointments.Where(x => x.ID == id).Select(a => new ValidateAppointment()
                    {
                        tblShowroom = a.tblShowroom,
                        tblUser = a.tblUser,
                        Purpose = a.tblAppointmentDetails.FirstOrDefault().Purpose,
                        Datetime = a.tblAppointmentDetails.FirstOrDefault().Date,
                        UserCarID = a.UserCarID,
                        ShowroomCarID = a.ShowroomCarID,
                        CreatedBy = a.CreatedBy,
                        CreatedOn = a.CreatedOn,
                        UpdatedOn = a.UpdatedOn,
                        UpdatedBy = a.UpdatedBy
                    }).FirstOrDefault();
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

        public ValidateAppointment GetShowById(int id)
        {
            try
            {
                if (id > 0)
                {
                    var appointment = _context.tblAppointments.Where(x => x.ID == id).Select(a => new ValidateAppointment()
                    {
                        ShowroomID = a.tblCar.tblShowroomID,
                        UserID = a.UserInterestedID, 
                        ShowroomContact = a.tblCar.tblShowroom.Contact, 
                        ShowroomEmail = a.tblCar.tblShowroom.Email
                    }).FirstOrDefault();
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
        
        public ValidateAppointment GetUserById(int id)
        {
            try
            {
                if (id > 0)
                {
                    var appointment = _context.tblAppointments.Where(x => x.ID == id).Select(a => new ValidateAppointment()
                    {
                        ShowroomID = a.ShowroomInterestedID.Value,
                        UserID = a.tblUserAdd.UserID, 
                        UserContact = a.tblUserAdd.tblUser.Number, 
                        UserEmail = a.tblUserAdd.tblUser.Email
                    }).FirstOrDefault();
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
                    var appointment = _context.tblAppointments.Where(x => x.ID == id).Select(a => new ValidateAppointment()
                    {
                        ID = a.ID,
                        tblCar = a.tblCar,
                        tblUser = a.tblUser,
                        Purpose = a.tblAppointmentDetails.FirstOrDefault().Purpose,
                        Datetime = a.tblAppointmentDetails.FirstOrDefault().Date,
                        ShowroomCarID = a.ShowroomCarID,
                        CreatedOn = a.CreatedOn,
                        Number = a.tblAppointmentDetails.FirstOrDefault().Number,
                    }).FirstOrDefault();
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
                    var appointment = _context.tblAppointments.Where(x => x.ID == id).Select(a => new ValidateAppointment()
                    {
                        tblUserAdd = a.tblUserAdd,
                        tblUser = a.tblUser,
                        Purpose = a.tblAppointmentDetails.FirstOrDefault().Purpose,
                        Datetime = a.tblAppointmentDetails.FirstOrDefault().Date,
                        ShowroomCarID = a.ShowroomCarID,
                        CreatedOn = a.CreatedOn,
                    }).FirstOrDefault();
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

        public void Save()
        {
            _context.SaveChanges();
        }

        public IEnumerable<ValidateAppointment> GetShowroomAppointmentsById(int ShowroomID)
        {
            try
            {
                if (ShowroomID > 0)
                {
                    var appointment = _context.tblAppointments.Where(x => x.tblCar.tblShowroomID == ShowroomID && x.IsAccepted == true).Select(a => new ValidateAppointment()
                    {
                        tblShowroom = a.tblShowroom,
                        Purpose = a.tblAppointmentDetails.FirstOrDefault().Purpose,
                        Datetime = a.ConfirmDatetime.Value,
                        tblCar = a.tblCar,
                        tblUser = a.tblUser,
                        CreatedOn = a.CreatedOn,
                    }).OrderByDescending(x => x.Datetime).ToList();
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

        public IEnumerable<ValidateAppointment> GetUserAppointmentsById(int UserID)
        {
            try
            {
                if (UserID > 0)
                {
                    var appointment = _context.tblAppointments.Where(x => x.UserInterestedID == UserID && x.IsAccepted == true).Select(a => new ValidateAppointment()
                    {
                        tblShowroom = a.tblShowroom,
                        Purpose = a.tblAppointmentDetails.FirstOrDefault().Purpose,
                        Datetime = a.ConfirmDatetime.Value,
                        tblCar = a.tblCar,
                        tblUser = a.tblUser,
                        CreatedOn = a.CreatedOn,
                    }).OrderByDescending(x => x.Datetime).ToList();

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

        public int GetUserTodaysAppointmentsCount(DateTime currentDate, int UserID)
        {
            var reas = _context.tblAppointments.Where(x => x.UserInterestedID == UserID && (x.UpdatedOn > currentDate || x.IsUserRead == false)).OrderByDescending(x => x.UpdatedOn).ToList().Count;
            return reas;
        }

        public int GetShowroomTodaysAppointmentsCount(DateTime currentDate, int ShowroomID)
        {
            var reas = _context.tblAppointments.Where(x => x.tblCar.tblShowroomID == ShowroomID && x.IsAccepted == false && x.Isactive == true && (x.CreatedOn > currentDate || x.IsRead == false)).ToList().Count;
            return reas;
        }

        public IEnumerable<tblAppointment> GetUserAppointments(DateTime currentDate, int UserID)
        {
            var reas = _context.tblAppointments.Where(x => x.UserInterestedID == UserID && (x.UpdatedOn > currentDate || x.IsUserRead == false)).OrderByDescending(x => x.UpdatedOn).ToList();
            return reas;
        }

        public IEnumerable<tblAppointment> GetShowroomAppointments(DateTime currentDate, int ShowroomID)
        {
            var reas = _context.tblAppointments.Where(x => x.tblCar.tblShowroomID == ShowroomID && x.IsAccepted == false && x.Isactive == true && (x.CreatedOn > currentDate || x.IsRead == false)).OrderByDescending(x => x.CreatedOn).ToList();
            return reas;
        }
    }
}
