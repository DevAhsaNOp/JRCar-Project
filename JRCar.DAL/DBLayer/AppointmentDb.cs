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
                        Purpose = a.Purpose,
                        Datetime = a.Date,
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
                    var appointment = _context.tblAppointments.Where(x => x.ShowroomInterestedID == ShowroomID).Select(a => new ValidateAppointment()
                    {
                        tblShowroom = a.tblShowroom,
                        Purpose = a.Purpose,
                        Datetime = a.Date,
                        UserCarID = a.UserCarID,
                        Isactive = a.Isactive,
                        CreatedBy = a.CreatedBy,
                        CreatedOn = a.CreatedOn,
                        UpdatedOn = a.UpdatedOn,
                        UpdatedBy = a.UpdatedBy
                    }).ToList();
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
                    var appointment = _context.tblAppointments.Where(x => x.UserInterestedID == UserID).Select(a => new ValidateAppointment()
                    {
                        tblUser = a.tblUser,
                        Purpose = a.Purpose,
                        Datetime = a.Date,
                        ShowroomCarID = a.ShowroomCarID,
                        Isactive = a.Isactive,
                        CreatedBy = a.CreatedBy,
                        CreatedOn = a.CreatedOn,
                        UpdatedOn = a.UpdatedOn,
                        UpdatedBy = a.UpdatedBy
                    }).ToList();
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
            var reas = _context.tblAppointments.Where(x => x.UserInterestedID == UserID && x.Date.Date.ToString("dd/MM/yyyy") == currentDate.ToString("dd/MM/yyyy")).ToList().Count;
            return reas;
        }

        public int GetShowroomTodaysAppointmentsCount(DateTime currentDate, int ShowroomID)
        {
            var reas = _context.tblAppointments.Where(x => x.ShowroomInterestedID == ShowroomID && x.Date.Date.ToString("dd/MM/yyyy") == currentDate.ToString("dd/MM/yyyy")).ToList().Count;
            return reas;
        }
    }
}
