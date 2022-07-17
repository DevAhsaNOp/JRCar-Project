using JRCar.BOL;
using JRCar.BOL.Validation_Classes;
using JRCar.DAL.DBLayer;
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
            dbObj.InActiveModel(model);
        }

        public IEnumerable<tblUser> GetModel()
        {
            return dbObj.GetModel();
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

        public tblUser GetUserRole(string email)
        {
            if (email != null)
            {
                var reas = dbObj.GetModelByID(email);
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
                var reas = dbObj.GetModelByID(modelId);
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

        public DAL.UserDefine.UserViewDetail GetModelByID(string emailtext, string password)
        {
            try
            {
                var reas = dbObj.GetModelByID(emailtext);
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

        public void InsertModel(ValidateUser model)
        {
            if (model != null)
            {
                tblUser obj = new tblUser()
                {
                    Name = model.Name,
                    Email = model.Username,
                    Number = model.Number,
                    Address = model.Address,
                    Password = EncDec.Encrypt(model.Password),
                    Image = model.Image
                };
                dbObj.InsertModel(obj);
            }
        }

        public bool UpdateModel(ValidateUser model)
        {
            if (model != null)
            {
                var reas = dbObj.GetModelByID(model.Username);
                reas.Password = EncDec.Encrypt(model.Password);
                dbObj.UpdateModel(reas);
                return true;
            }
            else
                return false;
        }
    }
}
