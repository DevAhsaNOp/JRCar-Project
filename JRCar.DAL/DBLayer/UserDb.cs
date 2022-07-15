using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JRCar.BOL;
using System.Data.Entity;
using JRCar.DAL.UserDefine;
using System.Net;
using System.Net.Mail;

namespace JRCar.DAL.DBLayer
{
    public class UserDb
    {
        private jrcarEntities _context;
        private IDbSet<tblUser> dbEntity;

        public UserDb()
        {
            _context = new jrcarEntities();
            dbEntity = _context.Set<tblUser>();
        }

        public IEnumerable<tblUser> GetModel()
        {
            return dbEntity.ToList();
        }

        public tblUser GetModelByID(int modelId)
        {
            return dbEntity.Find(modelId);
        }

        public tblUser GetModelByID(string emailtext)
        {
            var entity = dbEntity.Where(x => x.Email == emailtext).FirstOrDefault();
            if (entity != null)
            {
                return entity;
            }
            else
                return null;
        }

        public bool ForgotPassword(string emailtext)
        {
            var entity = dbEntity.Where(x => x.Email == emailtext).FirstOrDefault();
            if (entity != null)
            {
                var otp = OTPGenerator.GenerateRandomOTP();
                entity.OTP = otp;
                UpdateModel(entity);
                var email = "ahnkhan804@gmail.com";
                using (MailMessage mm = new MailMessage(email, emailtext))
                {
                    mm.Subject = "Password Reset OTP";
                    mm.Body = "<p>Your <b>OTP:" + otp + "</b><br/>Don't share it with anyone!</p>";
                    mm.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        mm.IsBodyHtml = true;
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential(email, "zbmkdbvsqhvnhmmw");
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);
                    }
                }
                return true;
            }
            else
                return false;
        }

        public UserDefine.UserViewDetail GetUserDetail(string emailtext)
        {
            var entity = dbEntity.Where(x => x.Email == emailtext).Select(s => new UserDefine.UserViewDetail()
            {
                ID = s.ID,
                Name = s.Name,
                Email = s.Email,
                Image = s.Image
            }).FirstOrDefault();
            if (entity != null)
            {
                return entity;
            }
            else
                return null;
        }

        public void InsertModel(tblUser model)
        {
            try
            {
                model.Active = true;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = 2;
                model.UpdatedOn = null;
                model.UpdatedBy = null;
                model.tblRoleID = 2;
                dbEntity.Add(model);
                Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateModel(tblUser model)
        {
            try
            {
                model.Active = true;
                model.CreatedOn = GetModelByID(model.ID).CreatedOn;
                model.CreatedBy = GetModelByID(model.ID).CreatedBy;
                model.UpdatedOn = DateTime.Now;
                model.tblRoleID = 2;
                _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                Save();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void InActiveModel(tblUser model)
        {
            try
            {
                model.Active = false;
                model.CreatedOn = GetModelByID(model.ID).CreatedOn;
                model.CreatedBy = GetModelByID(model.ID).CreatedBy;
                model.UpdatedOn = DateTime.Now;
                model.tblRoleID = 2;
                _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
