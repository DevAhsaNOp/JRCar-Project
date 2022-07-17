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

        public bool UpdateUser(ValidateUser model)
        {
            if (model != null)
            {
                var reas = dbObj.GetUserDetail(model.Email);
                if (reas != null)
                {
                    if (reas.Role == "Admin")
                    {
                        var adminData=dbObj.GetAdminByID(reas.ID);
                        adminData.Password = EncDec.Encrypt(model.Password);
                        dbObj.UpdateAdmin(adminData);
                    }
                    else if (reas.Role == "User")
                    {
                       var userData = dbObj.GetUserByID(reas.ID);
                       userData.Password = EncDec.Encrypt(model.Password);
                       dbObj.UpdateUser(userData);
                    }
                    else if (reas.Role == "Showroom")
                    {
                        var showroomData = dbObj.GetShowRoomByID(reas.ID);
                        showroomData.Password = EncDec.Encrypt(model.Password);
                        dbObj.UpdateShowroom(showroomData);
                    }
                    else
                    {
                        var unionData = dbObj.GetUnionByID(reas.ID);
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
    }
}
