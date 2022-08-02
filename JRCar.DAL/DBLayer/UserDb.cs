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
using JRCar.BOL.Validation_Classes;

namespace JRCar.DAL.DBLayer
{
    public class UserDb
    {
        private jrcarEntities _context;

        public UserDb()
        {
            _context = new jrcarEntities();
        }

        public bool IsEmailExist(string Email)
        {
            var reas = GetUserDetail(Email);
            if (reas != null)
                return true;
            else
                return false;
        }

        public IEnumerable<tblUser> GetAllUsers()
        {
            return _context.tblUsers.ToList();
        }

        public IEnumerable<tblAdmin> GetAllAdmin()
        {
            return _context.tblAdmins.ToList();
        }

        public IEnumerable<tblUnion> GetAllUnion()
        {
            return _context.tblUnions.ToList();
        }

        public IEnumerable<tblShowroom> GetAllShowRoom()
        {
            return _context.tblShowrooms.ToList();
        }

        public tblUser GetUserByID(int modelId)
        {
            return _context.tblUsers.Find(modelId);
        }

        public tblAdmin GetAdminByID(int modelId)
        {
            return _context.tblAdmins.Find(modelId);
        }

        public tblUnion GetUnionByID(int modelId)
        {
            return _context.tblUnions.Find(modelId);
        }

        public tblShowroom GetShowRoomByID(int modelId)
        {
            return _context.tblShowrooms.Find(modelId);
        }

        //public UserViewDetail GetUserByEmail(string emailtext)
        //{
        //    var user = _context.tblUsers.Where(x => x.Email == emailtext).FirstOrDefault();
        //    var admin = _context.tblAdmins.Where(x => x.Email == emailtext).FirstOrDefault();
        //    var showroom = _context.tblUnions.Where(x => x.Email == emailtext).FirstOrDefault();
        //    var union = _context.tblShowrooms.Where(x => x.Email == emailtext).FirstOrDefault();

        //    if (user != null)
        //    {
        //        return user;
        //    }
        //    else if (admin != null)
        //    {
        //        return admin;
        //    }
        //    else if (showroom != null)
        //    {
        //        return showroom;
        //    }
        //    else if (union != null)
        //    {
        //        return union;
        //    }
        //    else
        //        return null;
        //}

        //public tblAdmin GetAdminByEmail(string emailtext)
        //{
        //    if (entity != null)
        //    {
        //        return entity;
        //    }
        //    else
        //        return null;
        //}

        //public tblUnion GetUnionByEmail(string emailtext)
        //{
        //    if (entity != null)
        //    {
        //        return entity;
        //    }
        //    else
        //        return null;
        //}

        //public tblShowroom GetShownroomByEmail(string emailtext)
        //{
        //    if (entity != null)
        //    {
        //        return entity;
        //    }
        //    else
        //        return null;
        //}

        public void SendEmail(string otp, string emailtext)
        {
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
        }

        public bool ForgotPassword(string emailtext)
        {
            var user = GetUserDetail(emailtext);
            if (user != null)
            {
                var userData = GetUserByID(user.ID);
                var adminData = GetAdminByID(user.ID);
                var unionData = GetUnionByID(user.ID);
                var showroomData = GetShowRoomByID(user.ID);
                var otp = OTPGenerator.GenerateRandomOTP();
                if (userData != null)
                {
                    userData.OTP = otp;
                    UpdateUser(userData);
                }
                else if (adminData != null)
                {
                    adminData.OTP = otp;
                    UpdateAdmin(adminData);
                }
                else if (unionData != null)
                {
                    unionData.OTP = otp;
                    UpdateUnion(unionData);
                }
                else if (showroomData != null)
                {
                    showroomData.OTP = otp;
                    UpdateShowroom(showroomData);
                }
                SendEmail(otp, emailtext);
                return true;
            }
            else
                return false;
        }

        public bool CheckOTP(string emailtext, string OTP)
        {
            var user = _context.tblUsers.Where(x => x.Email == emailtext && x.OTP == OTP).FirstOrDefault();
            var admin = _context.tblAdmins.Where(x => x.Email == emailtext && x.OTP == OTP).FirstOrDefault();
            var showroom = _context.tblShowrooms.Where(x => x.Email == emailtext && x.OTP == OTP).FirstOrDefault();
            var union = _context.tblUnions.Where(x => x.Email == emailtext && x.OTP == OTP).FirstOrDefault();
            if ((admin != null) || (union != null) || (showroom != null) || (user != null))
                return true;
            else
                return false;
        }

        public UserDefine.UserViewDetail GetUserDetail(string emailtext)
        {
            var user = _context.tblUsers.Where(x => x.Email == emailtext).Select(s => new UserDefine.UserViewDetail()
            {
                ID = s.ID,
                Name = s.Name,
                Email = s.Email,
                Image = s.Image,
                Password = s.Password,
                OTP = s.OTP,
                Role = s.tblRole.Role
            }).FirstOrDefault();

            var admin = _context.tblAdmins.Where(x => x.Email == emailtext).Select(s => new UserDefine.UserViewDetail()
            {
                ID = s.ID,
                Name = s.Name,
                Email = s.Email,
                Image = s.Image,
                Password = s.Password,
                OTP = s.OTP,
                Role = s.tblRole.Role
            }).FirstOrDefault();

            var union = _context.tblUnions.Where(x => x.Email == emailtext).Select(s => new UserDefine.UserViewDetail()
            {
                ID = s.ID,
                Name = s.Name,
                Email = s.Email,
                Image = s.Image,
                Password = s.Password,
                OTP = s.OTP,
                Role = s.tblRole.Role
            }).FirstOrDefault();

            var showroom = _context.tblShowrooms.Where(x => x.Email == emailtext).Select(s => new UserDefine.UserViewDetail()
            {
                ID = s.ID,
                Name = s.FullName,
                Email = s.Email,
                Image = s.Image,
                Password = s.Password,
                OTP = s.OTP,
                Role = s.tblUser.tblRole.Role
            }).FirstOrDefault();

            if (user != null)
            {
                return user;
            }
            else if (admin != null)
            {
                return admin;
            }
            else if (showroom != null)
            {
                return showroom;
            }
            else if (union != null)
            {
                return union;
            }
            else
                return null;
        }

        public ValidateUser GetUserDetailById(int Id)
        {
            var user = _context.tblUsers.Where(x => x.ID == Id).Select(s => new ValidateUser()
            {
                ID = s.ID,
                Name = s.Name,
                Email = s.Email,
                Address = s.Address,
                Number = s.Number,
                Image = s.Image,
                Password = s.Password,
                tblRoleID = s.tblRoleID
            }).FirstOrDefault();

            var admin = _context.tblAdmins.Where(x => x.ID == Id).Select(s => new ValidateUser()
            {
                ID = s.ID,
                Name = s.Name,
                Email = s.Email,
                Address = s.Address,
                Number = s.Number,
                Image = s.Image,
                Password = s.Password,
                tblRoleID = s.tblRoleID
            }).FirstOrDefault();

            var union = _context.tblUnions.Where(x => x.ID == Id).Select(s => new ValidateUser()
            {
                ID = s.ID,
                Name = s.Name,
                Email = s.Email,
                Address = s.Address,
                Number = s.Number,
                Image = s.Image,
                Password = s.Password,
                tblRoleID = s.tblRoleID
            }).FirstOrDefault();

            if (user != null)
            {
                return user;
            }
            else if (admin != null)
            {
                return admin;
            }
            else if (union != null)
            {
                return union;
            }
            else
                return null;
        }

        public void InsertUser(tblUser model)
        {
            try
            {
                model.Active = true;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = 2;
                model.UpdatedOn = null;
                model.UpdatedBy = null;
                model.tblRoleID = 2;
                _context.tblUsers.Add(model);
                Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertAdmin(tblAdmin model)
        {
            try
            {
                model.Active = true;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = 1;
                model.UpdatedOn = null;
                model.UpdatedBy = null;
                model.tblRoleID = 1;
                model.Verified = false;
                _context.tblAdmins.Add(model);
                Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertUnion(tblUnion model)
        {
            try
            {
                model.Active = true;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = 4;
                model.UpdatedOn = null;
                model.UpdatedBy = null;
                model.tblRoleID = 4;
                _context.tblUnions.Add(model);
                Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertShowroom(tblShowroom model)
        {
            try
            {
                model.Isactive = true;
                model.Isarchive = false;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = 2;
                model.UpdatedOn = DateTime.Now;
                model.UpdatedBy = 4;
                _context.tblShowrooms.Add(model);
                Save();
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

        public void UpdateUser(tblUser model)
        {
            try
            {
                model.Active = true;
                model.CreatedOn = GetUserByID(model.ID).CreatedOn;
                model.CreatedBy = GetUserByID(model.ID).CreatedBy;
                model.UpdatedOn = DateTime.Now;
                model.tblRoleID = 2;
                _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdateAdmin(tblAdmin model)
        {
            try
            {
                model.Active = true;
                model.Verified = false;
                model.CreatedOn = GetAdminByID(model.ID).CreatedOn;
                model.CreatedBy = GetAdminByID(model.ID).CreatedBy;
                model.UpdatedOn = DateTime.Now;
                model.tblRoleID = 1;
                _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateUnion(tblUnion model)
        {
            try
            {
                model.Active = true;
                model.CreatedOn = GetUnionByID(model.ID).CreatedOn;
                model.CreatedBy = GetUnionByID(model.ID).CreatedBy;
                model.UpdatedOn = DateTime.Now;
                model.tblRoleID = 4;
                _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateShowroom(tblShowroom model)
        {
            try
            {
                model.Isactive = true;
                model.Isarchive = false;
                model.CreatedOn = GetShowRoomByID(model.ID).CreatedOn;
                model.CreatedBy = GetShowRoomByID(model.ID).CreatedBy;
                model.UpdatedOn = DateTime.Now;
                _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InActiveUser(tblUser model)
        {
            try
            {
                model.Active = false;
                model.CreatedOn = GetUserByID(model.ID).CreatedOn;
                model.CreatedBy = GetUserByID(model.ID).CreatedBy;
                model.UpdatedOn = DateTime.Now;
                model.tblRoleID = 2;
                _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InActiveAdmin(tblAdmin model)
        {
            try
            {
                model.Active = false;
                model.CreatedOn = GetAdminByID(model.ID).CreatedOn;
                model.CreatedBy = GetAdminByID(model.ID).CreatedBy;
                model.UpdatedOn = DateTime.Now;
                model.tblRoleID = 1;
                _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InActiveUnion(tblUnion model)
        {
            try
            {
                model.Active = false;
                model.CreatedOn = GetUnionByID(model.ID).CreatedOn;
                model.CreatedBy = GetUnionByID(model.ID).CreatedBy;
                model.UpdatedOn = DateTime.Now;
                model.tblRoleID = 4;
                _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InActiveShowroom(tblShowroom model)
        {
            try
            {
                model.Isactive = true;
                model.Isarchive = false;
                model.CreatedOn = GetShowRoomByID(model.ID).CreatedOn;
                model.CreatedBy = GetShowRoomByID(model.ID).CreatedBy;
                model.UpdatedOn = DateTime.Now;
                _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
