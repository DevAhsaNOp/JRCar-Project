using JRCar.DAL.DBLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JRCar.BOL;

namespace JRCar.BLL.Repositories
{
    public class AddressAutofillRepo
    {
        private AddressAutoFillDb dbObj;

        public AddressAutofillRepo()
        {
            dbObj = new AddressAutoFillDb();
        }

        public IEnumerable<tblState> GetAllState()
        {
            return dbObj.GetAllState();
        }

        public IEnumerable<tblCity> GetAllCity()
        {
            return dbObj.GetAllCity();
        }

        public IEnumerable<tblZone> GetAllZone()
        {
            return dbObj.GetAllZone();
        }

        public IEnumerable<tblCity> GetCitiesByState(int StateId)
        {
            return dbObj.GetCitiesByState(StateId);
        }

        public IEnumerable<tblZone> GetZoneByCity(int CityId)
        {
            return dbObj.GetZoneByCity(CityId);
        }
    }
}
