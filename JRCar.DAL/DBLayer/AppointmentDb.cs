﻿using JRCar.BOL.Validation_Classes;
using JRCar.BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JRCar.DAL.UserDefine;

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

        public int InsertUserAppointment(tblAppointment model)
        {
            try
            {
                if (model != null)
                {
                    model.IsAccepted = false;
                    model.IsRead = null;
                    model.IsUserRead = false;
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

        public int RejectUserAppointment(int AppntID, int Usr)
        {
            try
            {
                if (AppntID > 0)
                {
                    var model = _context.tblAppointments.Where(a => a.ID == AppntID).FirstOrDefault();
                    model.IsAccepted = false;
                    model.Isactive = false;
                    model.IsRead = false;
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

        public bool AcceptUserAppointment(int AppntID, int Usr, tblAppointmentDetail AppntDetailmodel)
        {
            try
            {
                if (AppntID > 0 && Usr > 0)
                {
                    var model = _context.tblAppointments.Where(a => a.ID == AppntID).FirstOrDefault();
                    model.IsAccepted = true;
                    model.IsRead = false;
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

        public string ShowroomContact(tblQuery model)
        {
            try
            {
                if (model != null)
                {
                    string QueryNo = "QNO" + DateTime.Now.ToString("ddMMyy") + "-" + DateTime.Now.Millisecond.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                    model.QueryNo = QueryNo;
                    model.CreatedOn = DateTime.Now;
                    model.UpdatedOn = null;
                    model.UpdatedBy = null;
                    model.Isactive = true;
                    model.Isarchive = false;
                    model.IsUserRead = null;
                    model.IsShowroomRead = false;
                    model.IsUnionRead = null;
                    _context.tblQueries.Add(model);
                    Save();
                    if (model.ID > 0)
                        return QueryNo;
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

        public bool IsShowroomRequestThisCarAppointment(int ShowroomID, int CarID)
        {
            try
            {
                if (ShowroomID > 0 && CarID > 0)
                {
                    var reas = _context.tblAppointments.Where(x => x.ShowroomInterestedID == ShowroomID && x.UserCarID == CarID).FirstOrDefault();
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
            var reas = _context.tblAppointments.Where(a => (a.IsRead == false && a.tblCar.tblShowroomID == ShowroomID) || (a.IsRead == false && a.ShowroomInterestedID == ShowroomID)).ToList();
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
            var reas = _context.tblAppointments.Where(a => (a.IsUserRead == false && a.UserInterestedID == UserID) || (a.tblUserAdd.UserID == UserID && a.IsUserRead == false)).ToList();
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

        public bool ChangeShowroomMessageToAsRead(int ShowroomID)
        {
            var reas = _context.tblQueries.Where(a => a.IsShowroomRead == false && a.ShowroomID == ShowroomID).ToList();
            if (reas.Count > 0)
            {
                foreach (var message in reas)
                {
                    try
                    {
                        message.IsShowroomRead = true;
                        message.UpdatedOn = DateTime.Now;
                        message.UpdatedBy = ShowroomID;
                        _context.Entry(message).State = System.Data.Entity.EntityState.Modified;
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
                        tblUserAdd = a.tblUserAdd,
                        tblUser = ((a.tblUserAdd == null) ? a.tblUser : a.tblUserAdd.tblUser),
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
                        ID = a.ID,
                        tblCar = a.tblCar,
                        tblUserAdd = a.tblUserAdd,
                        tblShowroom = ((a.tblCar == null) ? a.tblShowroom : a.tblCar.tblShowroom),
                        Purpose = a.tblAppointmentDetails.FirstOrDefault().Purpose,
                        Datetime = a.tblAppointmentDetails.FirstOrDefault().Date,
                        UserCarID = a.UserCarID,
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
                    //var appointment = _context.tblAppointments.Where(x => (x.tblCar.tblShowroomID == ShowroomID && x.IsAccepted == true) || (x.ShowroomInterestedID == ShowroomID)).Select(a => new ValidateAppointment()
                    //{
                    //    tblShowroom = a.tblShowroom,
                    //    Purpose = a.tblAppointmentDetails.FirstOrDefault().Purpose,
                    //    Datetime = a.ConfirmDatetime.Value,
                    //    tblCar = a.tblCar,
                    //    tblUser = a.tblUser,
                    //    CreatedOn = a.CreatedOn,
                    //}).OrderByDescending(x => x.Datetime).ToList();
                    var appointment = _context.tblAppointments.Where(x => (x.tblCar.tblShowroomID == ShowroomID) || (x.ShowroomInterestedID == ShowroomID)).ToList();
                    List<ValidateAppointment> appnt = new List<ValidateAppointment>();
                    foreach (var item in appointment)
                    {
                        if (item.tblCar != null)
                        {
                            ValidateAppointment validate = new ValidateAppointment();
                            validate.ID = item.ID;
                            validate.tblShowroom = item.tblCar.tblShowroom;
                            validate.Purpose = item.tblAppointmentDetails.FirstOrDefault().Purpose;
                            validate.Datetime = item.ConfirmDatetime.Value;
                            validate.tblCar = item.tblCar;
                            validate.tblUser = item.tblUser;
                            validate.CreatedOn = item.CreatedOn;
                            validate.Isactive = item.Isactive;
                            validate.IsAccepted = item.IsAccepted.Value;
                            appnt.Add(validate);
                        }
                        else if (item.tblUserAdd != null)
                        {
                            ValidateAppointment validate = new ValidateAppointment();
                            validate.ID = item.ID;
                            validate.tblShowroom = item.tblShowroom;
                            validate.Purpose = item.tblAppointmentDetails.FirstOrDefault().Purpose;
                            validate.Datetime = item.ConfirmDatetime.Value;
                            validate.tblUserAdd = item.tblUserAdd;
                            validate.tblUser = item.tblUserAdd.tblUser;
                            validate.CreatedOn = item.CreatedOn;
                            validate.Isactive = item.Isactive;
                            validate.IsAccepted = item.IsAccepted.Value;
                            appnt.Add(validate);
                        }
                    }
                    if (appointment != null)
                        return appnt.OrderByDescending(x => x.Datetime);
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
                    var appointment = _context.tblAppointments.Where(x => x.UserInterestedID == UserID || x.tblUserAdd.UserID == UserID).ToList();
                    List<ValidateAppointment> appnt = new List<ValidateAppointment>();
                    foreach (var item in appointment)
                    {
                        if (item.tblCar != null)
                        {
                            ValidateAppointment validate = new ValidateAppointment();
                            validate.ID = item.ID;
                            validate.tblShowroom = item.tblCar.tblShowroom;
                            validate.Purpose = item.tblAppointmentDetails.FirstOrDefault().Purpose;
                            validate.Datetime = item.ConfirmDatetime.Value;
                            validate.tblCar = item.tblCar;
                            validate.tblUser = item.tblUser;
                            validate.CreatedOn = item.CreatedOn;
                            validate.Isactive = item.Isactive;
                            validate.IsAccepted = item.IsAccepted.Value;
                            appnt.Add(validate);
                        }
                        else if (item.tblUserAdd != null)
                        {
                            ValidateAppointment validate = new ValidateAppointment();
                            validate.ID = item.ID;
                            validate.tblShowroom = item.tblShowroom;
                            validate.Purpose = item.tblAppointmentDetails.FirstOrDefault().Purpose;
                            validate.Datetime = item.ConfirmDatetime.Value;
                            validate.tblUserAdd = item.tblUserAdd;
                            validate.tblUser = item.tblUserAdd.tblUser;
                            validate.CreatedOn = item.CreatedOn;
                            validate.Isactive = item.Isactive;
                            validate.IsAccepted = item.IsAccepted.Value;
                            appnt.Add(validate);
                        }
                    }
                    if (appointment != null)
                        return appnt.OrderByDescending(x => x.Datetime);
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
                    var messges = _context.tblQueries.Where(x => x.ShowroomID == ShowroomID).ToList();
                    if (messges != null)
                        return messges.OrderByDescending(x => x.CreatedOn);
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

        public int GetShowroomMessagesCount(int ShowroomID)
        {
            var reas = _context.tblQueries.Where(x => x.ShowroomID == ShowroomID && x.IsShowroomRead == false).OrderByDescending(x => x.CreatedOn).ToList().Count;
            return reas;
        }

        public IEnumerable<tblQuery> GetShowroomMessages(int ShowroomID)
        {
            var reas = _context.tblQueries.Where(x => x.ShowroomID == ShowroomID && x.IsShowroomRead == false).OrderByDescending(x => x.CreatedOn).ToList();
            return reas;
        }

        public int GetUserTodaysAppointmentsCount(DateTime currentDate, int UserID)
        {
            var reas = _context.tblAppointments.Where(x => (x.UserInterestedID == UserID && x.IsUserRead == false) || (x.tblUserAdd.UserID == UserID && x.IsUserRead == false)).OrderByDescending(x => x.CreatedOn).ToList().Count;
            return reas;
        }

        public int GetShowroomTodaysAppointmentsCount(DateTime currentDate, int ShowroomID)
        {
            var reas = _context.tblAppointments.Where(x => (x.tblCar.tblShowroomID == ShowroomID && x.IsRead == false) || (x.ShowroomInterestedID == ShowroomID && x.IsRead == false)).ToList().Count;
            return reas;
        }

        public IEnumerable<tblAppointment> GetUserAppointments(DateTime currentDate, int UserID)
        {
            var reas = _context.tblAppointments.Where(x => (x.UserInterestedID == UserID && x.IsUserRead == false) || (x.tblUserAdd.UserID == UserID && x.IsUserRead == false)).ToList();
            return reas;
        }

        public IEnumerable<tblAppointment> GetShowroomAppointments(DateTime currentDate, int ShowroomID)
        {
            var reas = _context.tblAppointments.Where(x => (x.tblCar.tblShowroomID == ShowroomID && x.IsRead == false) || (x.ShowroomInterestedID == ShowroomID && x.IsRead == false)).OrderByDescending(x => x.CreatedOn).ToList();
            return reas;
        }
    }
}
