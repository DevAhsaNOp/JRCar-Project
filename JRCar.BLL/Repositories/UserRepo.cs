using JRCar.BOL;
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

        public void InsertModel(tblUser model)
        {
            if (model.Password != null)
            {
                model.Password = EncDec.Encrypt(model.Password);
                dbObj.InsertModel(model);
            }
        }

        public void UpdateModel(tblUser model)
        {
            dbObj.UpdateModel(model);
        }
    }
}
