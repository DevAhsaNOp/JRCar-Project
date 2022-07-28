using JRCar.BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRCar.DAL.DBLayer
{
    public class AddressAutoFillDb
    {
        private jrcarEntities _context;

        public AddressAutoFillDb()
        {
            _context = new jrcarEntities();
        }

        public IEnumerable<tblState> GetAllState()
        {
            return _context.tblStates.ToList();
        }

        public IEnumerable<tblCity> GetAllCity()
        {
            return _context.tblCities.ToList();
        }

        public IEnumerable<tblZone> GetAllZone()
        {
            return _context.tblZones.ToList();
        }

        public IEnumerable<tblCity> GetCitiesByState(int StateId)
        {
            return _context.tblCities.Where(x => x.StateId == StateId).ToList();
        }

        public IEnumerable<tblZone> GetZoneByCity(int CityId)
        {
            return _context.tblZones.Where(x => x.CityId == CityId).ToList();
        }
    }
}
