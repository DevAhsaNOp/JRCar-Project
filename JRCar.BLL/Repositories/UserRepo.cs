using JRCar.BOL;
using JRCar.BOL.Validation_Classes;
using JRCar.DAL.DBLayer;
using JRCar.DAL.UserDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRCar.BLL.Repositories
{
    public class UserRepo
    {
        private UserDb dbObj;

        public UserRepo()
        {
            dbObj = new UserDb();
        }
        public void InActiveModel(tblUser model)
        {
            dbObj.InActiveUser(model);
        }

        public bool IsEmailExist(string Email) 
        {
            if (Email != null)
            {
                var reas = dbObj.IsEmailExist(Email);
                if (reas)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<tblUser> GetAllUsers()
        {
            return dbObj.GetAllUsers();
        }

        public bool ForgotPassword(string emailtext)
        {
            try
            {
                var isTrue = dbObj.ForgotPassword(emailtext);
                if (isTrue)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserViewDetail GetUserRole(string email)
        {
            if (email != null)
            {
                var reas = dbObj.GetUserDetail(email);
                return reas;
            }
            else return null;
        }

        public bool CheckOTP(string emailtext, string OTP)
        {
            var IsTrue = dbObj.CheckOTP(emailtext, OTP);
            if (IsTrue)
                return true;
            else
                return false;
        }

        public tblUser GetModelByID(int modelId)
        {
            try
            {
                var reas = dbObj.GetUserByID(modelId);
                if (reas != null)
                {
                    reas.Password = EncDec.Decrypt(reas.Password);
                    return reas;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DAL.UserDefine.UserViewDetail CheckLoginDetails(string emailtext, string password)
        {
            try
            {
                var reas = dbObj.GetUserDetail(emailtext);
                if (reas != null)
                {
                    if (EncDec.Decrypt(reas.Password).Equals(password))
                    {
                        var entity = dbObj.GetUserDetail(emailtext);
                        return entity;
                    }
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertUser(ValidateUser model)
        {
            if (model != null)
            {
                tblUser obj = new tblUser()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Number = model.Number,
                    Address = model.Address,
                    Password = EncDec.Encrypt(model.Password),
                    Image = model.Image
                };
                dbObj.InsertUser(obj);
            }
        }

        public void InsertAdmin(ValidateUser model)
        {
            if (model != null)
            {
                tblAdmin obj = new tblAdmin()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Number = model.Number,
                    Address = model.Address,
                    Password = EncDec.Encrypt(model.Password),
                    Image = model.Image
                };
                dbObj.InsertAdmin(obj);
            }
        }

        public void InsertUnion(ValidateUser model)
        {
            if (model != null)
            {
                tblUnion obj = new tblUnion()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Number = model.Number,
                    Address = model.Address,
                    Password = EncDec.Encrypt(model.Password),
                    Image = model.Image
                };
                dbObj.InsertUnion(obj);
            }
        }

        public void InsertShowroom(ValidateShowroom model)
        {
            if (model != null)
            {
                tblShowroom obj = new tblShowroom()
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = EncDec.Encrypt(model.Password),
                    CNIC = model.CNIC,
                    Contact = model.Contact,
                    ShopNumber = model.ShopNumber,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    Image = model.Image,
                    RoleId = 3,
                    AddressId = model.AddressId,
                    UnionId = model.UnionId
                };
                dbObj.InsertShowroom(obj);
            }
        }

        public bool UpdateUser(ValidateUser model)
        {
            if (model != null)
            {
                var admin = dbObj.GetAdminByID(model.ID);
                var ShowroomData = dbObj.GetShowRoomByID(model.ID);
                var UnionData = dbObj.GetUnionByID(model.ID);
                var UserData = dbObj.GetUserByID(model.ID);
                var reas1 = dbObj.GetUserDetail(model.Email);
                if (admin != null)
                {
                    admin.Name = model.Name;
                    admin.Email = model.Email;
                    admin.Number = model.Number;
                    admin.Address = model.Address;
                    admin.Password = EncDec.Encrypt(model.Password);
                    admin.Image = model.Image;
                    admin.UpdatedBy = model.UpdatedBy;
                    dbObj.UpdateAdmin(admin);
                    return true;
                }
                else if (UserData != null)
                {
                    UserData.Name = model.Name;
                    UserData.Email = model.Email;
                    UserData.Number = model.Number;
                    UserData.Address = model.Address;
                    UserData.Password = EncDec.Encrypt(model.Password);
                    UserData.Image = model.Image;
                    UserData.UpdatedBy = model.UpdatedBy;
                    dbObj.UpdateUser(UserData);
                    return true;
                }
                else if (UnionData != null)
                {
                    UnionData.Name = model.Name;
                    UnionData.Email = model.Email;
                    UnionData.Number = model.Number;
                    UnionData.Address = model.Address;
                    UnionData.Password = EncDec.Encrypt(model.Password);
                    UnionData.Image = model.Image;
                    UnionData.UpdatedBy = model.UpdatedBy;
                    dbObj.UpdateUnion(UnionData);
                    return true;
                }
                else if (ShowroomData != null)
                {
                    ShowroomData.FullName = model.Name;
                    ShowroomData.Email = model.Email;
                    ShowroomData.Contact = model.Number;
                    ShowroomData.ShopNumber = model.Address;
                    ShowroomData.Password = EncDec.Encrypt(model.Password);
                    ShowroomData.Image = model.Image;
                    ShowroomData.CNIC = model.CNIC;
                    ShowroomData.UpdatedBy = model.UpdatedBy;
                    dbObj.UpdateShowroom(ShowroomData);
                    return true;
                }
                else if (reas1 != null)
                {
                    if (reas1.Role == "Admin")
                    {
                        var adminData = dbObj.GetAdminByID(reas1.ID);
                        adminData.Password = EncDec.Encrypt(model.Password);
                        dbObj.UpdateAdmin(adminData);
                    }
                    else if (reas1.Role == "User")
                    {
                        var userData = dbObj.GetUserByID(reas1.ID);
                        userData.Password = EncDec.Encrypt(model.Password);
                        dbObj.UpdateUser(userData);
                    }
                    else if (reas1.Role == "Showroom")
                    {
                        var showroomData = dbObj.GetShowRoomByID(reas1.ID);
                        showroomData.Password = EncDec.Encrypt(model.Password);
                        dbObj.UpdateShowroom(showroomData);
                    }
                    else
                    {
                        var unionData = dbObj.GetUnionByID(reas1.ID);
                        unionData.Password = EncDec.Encrypt(model.Password);
                        dbObj.UpdateUnion(unionData);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

        public tblShowroom GetShowRoomByID(int modelId)
        {
            return dbObj.GetShowRoomByID(modelId);
        }

        public ValidateUser GetUserDetailById(int Id)
        {
            try
            {
                var reas = dbObj.GetUserDetailById(Id);
                if (reas != null)
                {
                    reas.Password = EncDec.Decrypt(reas.Password);
                    return reas;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
