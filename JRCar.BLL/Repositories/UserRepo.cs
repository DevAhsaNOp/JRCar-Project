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
            return dbObj.GetModelByID(modelId);
        }

        public void InsertModel(tblUser model)
        {
            dbObj.InsertModel(model);
        }

        public void UpdateModel(tblUser model)
        {
            dbObj.UpdateModel(model);
        }
    }
}
