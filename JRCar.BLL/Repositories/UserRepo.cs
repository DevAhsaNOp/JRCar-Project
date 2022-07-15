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
            if (model.Password != null)
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
                dbObj.InsertModel(obj);
            }
        }

        public void UpdateModel(tblUser model)
        {
            dbObj.UpdateModel(model);
        }
    }
}
