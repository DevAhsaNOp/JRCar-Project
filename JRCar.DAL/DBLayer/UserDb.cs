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
using System.Web.Security;

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

        public bool IsPhoneNumberExist(string PhoneNumber)
        {
            var reas = GetUserPhone(PhoneNumber);
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

        public IEnumerable<tblRolePermission> GetAllRolePermission()
        {
            return _context.tblRolePermissions.ToList();
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

        public tblRolePermission GetRolePermissionByID(int modelId)
        {
            return _context.tblRolePermissions.Find(modelId);
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
                    NetworkCredential NetworkCred = new NetworkCredential(email, "vcwonsjwxtsbyajf");
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

        public UserDefine.UserViewDetail GetUserPhone(string phone)
        {
            var user = _context.tblUsers.Where(x => x.Number == phone).Select(s => new UserDefine.UserViewDetail()
            {
                PhoneNumber = s.Number
            }).FirstOrDefault();

            var admin = _context.tblAdmins.Where(x => x.Number == phone).Select(s => new UserDefine.UserViewDetail()
            {
                PhoneNumber = s.Number
            }).FirstOrDefault();

            var union = _context.tblUnions.Where(x => x.Number == phone).Select(s => new UserDefine.UserViewDetail()
            {
                PhoneNumber = s.Number
            }).FirstOrDefault();

            var showroom = _context.tblShowrooms.Where(x => x.Contact == phone).Select(s => new UserDefine.UserViewDetail()
            {
                PhoneNumber = s.Contact
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
                Role = s.tblRole.Role,
                Active = s.Active,
                PhoneNumber = s.Number
            }).FirstOrDefault();

            var admin = _context.tblAdmins.Where(x => x.Email == emailtext).Select(s => new UserDefine.UserViewDetail()
            {
                ID = s.ID,
                Name = s.Name,
                Email = s.Email,
                Image = s.Image,
                Password = s.Password,
                OTP = s.OTP,
                Role = s.tblRole.Role,
                Active = s.Active,
                PhoneNumber = s.Number
            }).FirstOrDefault();

            var union = _context.tblUnions.Where(x => x.Email == emailtext).Select(s => new UserDefine.UserViewDetail()
            {
                ID = s.ID,
                Name = s.Name,
                Email = s.Email,
                Image = s.Image,
                Password = s.Password,
                OTP = s.OTP,
                Role = s.tblRole.Role,
                Active = s.Active,
                PhoneNumber = s.Number,
                CRole = s.tblRole1 != null ? s.tblRole1.ID.ToString() : null
            }).FirstOrDefault();

            var showroom = _context.tblShowrooms.Where(x => x.Email == emailtext).Select(s => new UserDefine.UserViewDetail()
            {
                ID = s.ID,
                Name = s.FullName,
                Email = s.Email,
                Image = s.Image,
                Password = s.Password,
                OTP = s.OTP,
                Role = s.tblRole.Role,
                Active = s.Isactive,
                PhoneNumber = s.Contact
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

        public ValidateUser GetUserDetailById(int Id, string Role)
        {
            var user = _context.tblUsers.Where(x => x.ID == Id && x.tblRole.Role.ToLower().Contains(Role.ToLower())).Select(s => new ValidateUser()
            {
                ID = s.ID,
                Name = s.Name,
                Email = s.Email,
                SignUpEmail = s.Email,
                SignUpUpdateEmail = s.Email,
                SignUpUpdateNumber = s.Number,
                Address = s.Address,
                Number = s.Number,
                Image = s.Image,
                Password = s.Password,
                tblRoleID = s.tblRoleID,
                Active = ((s.Active == true) ? "1" : "0"),
            }).FirstOrDefault();

            var admin = _context.tblAdmins.Where(x => x.ID == Id && x.tblRole.Role.ToLower().Contains(Role.ToLower())).Select(s => new ValidateUser()
            {
                ID = s.ID,
                Name = s.Name,
                Email = s.Email,
                SignUpEmail = s.Email,
                SignUpUpdateEmail = s.Email,
                SignUpUpdateNumber = s.Number,
                Address = s.Address,
                Number = s.Number,
                Image = s.Image,
                Password = s.Password,
                Active = ((s.Active == true) ? "1" : "0"),
                tblRoleID = s.tblRoleID
            }).FirstOrDefault();

            var union = _context.tblUnions.Where(x => x.ID == Id && x.tblRole.Role.ToLower().Contains(Role.ToLower())).Select(s => new ValidateUser()
            {
                ID = s.ID,
                Name = s.Name,
                Email = s.Email,
                SignUpEmail = s.Email,
                SignUpUpdateEmail = s.Email,
                SignUpUpdateNumber = s.Number,
                Address = s.Address,
                Number = s.Number,
                Image = s.Image,
                Password = s.Password,
                Active = ((s.Active == true) ? "1" : "0"),
                tblRoleID = s.tblRoleID,
                tblCRoleID = s.tblCRoleID,
                tblRole = s.tblRole1,
            }).FirstOrDefault();

            var showroom = _context.tblShowrooms.Where(x => x.ID == Id && x.tblRole.Role.ToLower().Contains(Role.ToLower())).Select(s => new ValidateUser()
            {
                ID = s.ID,
                Name = s.FullName,
                Email = s.Email,
                SignUpEmail = s.Email,
                SignUpUpdateEmail = s.Email,
                SignUpUpdateNumber = s.Contact,
                Address = s.ShopNumber,
                Number = s.Contact,
                Image = s.Image,
                Password = s.Password,
                CNIC = s.CNIC,
                tblRoleID = s.RoleId,
                ShowroomDescription = s.Description,
                ShowroomURL = s.ShowroomURL,
                Active = ((s.Isactive == true) ? "1" : "0"),
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
            else if (showroom != null)
            {
                return showroom;
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

        public bool InsertRolePermission(tblRolePermission model)
        {
            try
            {
                _context.tblRolePermissions.Add(model);
                Save();
                if (model.ID > 0)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool EditRolePermission(tblRolePermission model)
        {
            try
            {
                var obj = _context.tblRolePermissions.Find(model.ID);
                obj.ID = model.ID;
                obj.AddShowroom = model.AddShowroom;
                obj.AddUnionMember = model.AddUnionMember;
                obj.AddUser = model.AddUser;
                obj.DeleteShowroom = model.DeleteShowroom;
                obj.DeleteUnionMember = model.DeleteUnionMember;
                obj.DeleteUser = model.DeleteUser;
                obj.EditProfile = model.EditProfile;
                obj.EditShowroom = model.EditShowroom;
                obj.EditUnionMember = model.EditUnionMember;
                obj.EditUser = model.EditUser;
                obj.MakeAnnoucment = model.MakeAnnoucment;
                obj.MakePayments = model.MakePayments;
                obj.ManagShowroomAds = model.ManagShowroomAds;
                obj.ManagUserAds = model.ManagUserAds;
                obj.RoleID = model.RoleID;
                obj.ShowAnnoucment = model.ShowAnnoucment;
                obj.ShowPayments = model.ShowPayments;
                obj.ShowShowroom = model.ShowShowroom;
                obj.ShowUnionMember = model.ShowUnionMember;
                obj.ShowUsers = model.ShowUsers;
                _context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                Save();
                if (model.ID > 0)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertRole(tblRole model)
        {
            try
            {
                model.Isactive = true;
                model.Isarchive = false;
                model.CreatedOn = DateTime.Now;
                model.UpdatedBy = null;
                model.UpdatedOn = null;
                _context.tblRoles.Add(model);
                Save();
                if (model.ID > 0)
                    return model.ID;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public tblRole GetRoleByID(int ID)
        {
            return _context.tblRoles.Find(ID);
        }

        public int EditRole(tblRole model)
        {
            try
            {
                var obj = _context.tblRoles.Find(model.ID);
                obj.Role = model.Role;
                obj.UpdatedOn = DateTime.Now;
                obj.UpdatedBy = model.UpdatedBy;
                obj.Isactive = true;
                obj.Isarchive = false;
                _context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                Save();
                if (model.ID > 0)
                    return model.ID;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InactiveRole(int RoleID)
        {
            try
            {
                if (RoleID > 0)
                {
                    var model = _context.tblRoles.Find(RoleID);
                    model.Isactive = false;
                    model.Isarchive = true;
                    model.UpdatedOn = DateTime.Now;
                    _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    Save();
                    if (model.ID > 0)
                        return model.ID;
                    else
                        return 0;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ActiveRole(int RoleID)
        {
            try
            {
                if (RoleID > 0)
                {
                    var model = _context.tblRoles.Find(RoleID);
                    model.Isactive = true;
                    model.Isarchive = false;
                    model.UpdatedOn = DateTime.Now;
                    _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    Save();
                    if (model.ID > 0)
                        return model.ID;
                    else
                        return 0;
                }
                else
                    return 0;
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
                var zone = _context.tblAddresses.Where(x => x.ID == model.AddressId).FirstOrDefault().tblZone.ZoneName;
                var city = _context.tblAddresses.Where(x => x.ID == model.AddressId).FirstOrDefault().tblCity.CityName;
                model.ShowroomURL = ShowroomURLGenerate(model.FullName, zone, city);
                model.Isactive = true;
                model.Isarchive = false;
                model.CreatedOn = DateTime.Now;
                model.UpdatedBy = null;
                model.UpdatedOn = null;
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
                var reas = _context.tblUnions.Where(x => x.ID == model.ID).FirstOrDefault();
                model.tblCRoleID = reas.tblCRoleID;
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

        public void ActiveUnion(tblUnion model)
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

        public void InActiveShowroom(tblShowroom model)
        {
            try
            {
                model.Isactive = false;
                model.Isarchive = true;
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

        public string ShowroomURLGenerate(string ShowroomName, string Zone, string CityLocation)
        {
            if (ShowroomName != null && Zone != null && CityLocation != null)
            {
                string URL = ShowroomName + "-Dealer-In-" + Zone + "-" + CityLocation + "-" + OTPGenerator.GenerateRandomOTP() + DateTime.Now.ToString("ddMMyy") + DateTime.Now.Millisecond.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                var rURL = URL.Replace(" ", "-");
                return rURL;
            }
            else
            {
                return null;
            }
        }
    }
}
