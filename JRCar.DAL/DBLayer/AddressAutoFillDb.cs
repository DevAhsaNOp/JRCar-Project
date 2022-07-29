﻿using JRCar.BOL;
using JRCar.BOL.Validation_Classes;
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

        public int InsertAddress(tblAddress model)
        {
            try
            {
                if (model != null)
                {
                    _context.tblAddresses.Add(model);
                    Save();
                    return model.ID;
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
                    _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    Save();
                    return true;
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
                    var address = _context.tblAddresses.Where(x => x.ID == id).Select(a => new ValidateAddress()
                    {
                        State = a.State,
                        City = a.City,
                        Area = ((a.Area == null) ? "" : a.Area),
                        CompleteAddress = ((a.CompleteAddress == null) ? "" : a.CompleteAddress)
                    }).FirstOrDefault();
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

        public void Save()
        {
            _context.SaveChanges();
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
