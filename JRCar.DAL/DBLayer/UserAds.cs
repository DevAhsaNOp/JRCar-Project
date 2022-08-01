﻿using JRCar.BOL;
using JRCar.BOL.Validation_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRCar.DAL.DBLayer
{
    public class UserAds
    {
        private jrcarEntities _context;

        public UserAds()
        {
            _context = new jrcarEntities();
        }

        public IEnumerable<tblUserAdd> GetAllUserActiveAds()
        {
            return _context.tblUserAdds.Where(x => x.Isactive == true).ToList();
        }
        
        public IEnumerable<tblUserAdd> GetAllUserInActiveAds()
        {
            return _context.tblUserAdds.Where(x => x.Isactive == false).ToList();
        }

        public ValidationUserAds GetUserAdsDetail(int AdsId)
        {
            var user = _context.tblUserAdds.Where(x => x.ID == AdsId).Select(s => new ValidationUserAds()
            {
                /*---Car Details---*/
                Model = s.Model,
                Year = s.Year,
                Condition = s.Condition,
                Title = s.Title,
                Description = ((s.Description == null) ? "" : s.Description),
                Price = s.Price,
                Latitude = ((s.Latitude == null) ? "" : s.Latitude.ToString()),
                Longitude = ((s.Longitude == null) ? "" : s.Longitude.ToString()),
                State = s.tblAddress.State,
                City = s.tblAddress.City,
                Area = s.tblAddress.Area,
                CompleteAddress = ((s.tblAddress.CompleteAddress == null) ? "Not Available" : s.tblAddress.CompleteAddress),
                Isactive = s.Isactive,
                CreatedOn = s.CreatedOn,
                ExpiryDate = s.ExpiryDate,
                CarImage = s.tblUserAddImages.Select(a => a.Image).FirstOrDefault(),

                /*---User Details---*/
                UserImage = s.tblUser.Image,
                UserName = s.tblUser.Name,
                Email = s.tblUser.Email,
                UserRole = s.tblUser.tblRole.Role,
                Number = s.tblUser.Number
            }).FirstOrDefault();

            if (user != null)
            {
                return user;
            }
            else
                return null;
        }

        public int InsertUserAds(tblUserAdd model)
        {
            try
            {
                if (model != null)
                {
                    model.Isactive = true;
                    model.Isarchive = false;
                    model.CreatedOn = DateTime.Now;
                    model.ExpiryDate = DateTime.Now.AddMonths(2);
                    _context.tblUserAdds.Add(model);
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

        public void Save()
        {
            _context.SaveChanges();
        }

        public bool UpdateUserAds(tblUserAdd model)
        {
            try
            {
                if (model != null)
                {
                    model.Isactive = true;
                    model.Isarchive = false;
                    model.CreatedOn = GetUserAdsDetail(model.ID).CreatedOn;
                    model.ExpiryDate = GetUserAdsDetail(model.ID).ExpiryDate;
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
        
        public bool InActiveUserAds(int AdID)
        {
            try
            {
                if (AdID > 0)
                {
                    var reas = _context.tblUserAdds.Where(x => x.ID == AdID).FirstOrDefault(); 
                    reas.Isactive = false;
                    reas.Isarchive = true;
                    reas.CreatedOn = GetUserAdsDetail(reas.ID).CreatedOn;
                    reas.ExpiryDate = GetUserAdsDetail(reas.ID).ExpiryDate;
                    _context.Entry(reas).State = System.Data.Entity.EntityState.Modified;
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
        
        public bool ReActiveUserAds(int AdID)
        {
            try
            {
                if (AdID > 0)
                {
                    var reas = _context.tblUserAdds.Where(x => x.ID == AdID).FirstOrDefault(); 
                    reas.Isactive = true;
                    reas.Isarchive = false;
                    reas.CreatedOn = GetUserAdsDetail(reas.ID).CreatedOn;
                    reas.ExpiryDate = DateTime.Now.AddMonths(2);
                    _context.Entry(reas).State = System.Data.Entity.EntityState.Modified;
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

        public bool InsertUserAdsImages(tblUserAddImage model)
        {
            try
            {
                if (model != null)
                {
                    _context.tblUserAddImages.Add(model);
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

        public bool UpdateUserAdsImages(tblUserAddImage model)
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
    }
}
