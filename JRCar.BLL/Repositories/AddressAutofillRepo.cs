using JRCar.DAL.DBLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JRCar.BOL;
using JRCar.BOL.Validation_Classes;

namespace JRCar.BLL.Repositories
{
    public class AddressAutofillRepo
    {
        private AddressAutoFillDb dbObj;

        public AddressAutofillRepo()
        {
            dbObj = new AddressAutoFillDb();
        }

        public int InsertAddress(tblAddress model)
        {
            try
            {
                if (model != null)
                {
                    var addressId = dbObj.InsertAddress(model);
                    if(addressId > 0)
                        return addressId;
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

        public bool UpdateAddress(tblAddress model)
        {
            try
            {
                if (model != null)
                {
                    var addressId = dbObj.UpdateAddress(model);
                    if(addressId == true)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ValidateAddress GetAddressById(int id)
        {
            try
            {
                if (id > 0)
                {
                    var address = dbObj.GetAddressById(id);
                    if (address != null)
                        return address;
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
