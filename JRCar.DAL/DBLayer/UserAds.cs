using JRCar.BOL;
using JRCar.BOL.Validation_Classes;
using JRCar.DAL.UserDefine;
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

        public IEnumerable<ValidationUserAds> GetAllActiveAdsFilter(string searchTerm, int? minimumPrice, int? maximumPrice, int? sortBy)
        {
            var reas = _context.tblUserAdds.Where(x => x.Isactive == true).Select(s => new ValidationUserAds()
            {
                Title = s.Title,
                Price = s.Price,
                Year = s.Year,
                Model = s.Model,
                Condition = s.Condition,
                CreatedOn = s.CreatedOn,
                City = s.tblAddress.tblCity.CityName,
                AdURL = s.UserAdsURL,
                CarImage = s.tblUserAddImages.Select(a => a.Image).FirstOrDefault(),
            }).ToList();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                reas = reas.Where(x => x.Title.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
            if (maximumPrice.HasValue)
            {
                reas = reas.Where(x => Convert.ToInt32(x.Price) <= maximumPrice.Value).ToList();
            }
            if (minimumPrice.HasValue)
            {
                reas = reas.Where(x => Convert.ToInt32(x.Price) >= minimumPrice.Value).ToList();
            }
            if (sortBy.HasValue)
            {
                switch (sortBy.Value)
                {
                    case 1:
                        reas = reas.OrderByDescending(Ad => Ad.CreatedOn).ToList();
                        break;
                    case 2:
                        reas = reas.OrderBy(x => Convert.ToInt32(x.Price)).ToList();
                        break;
                    case 3:
                        reas = reas.OrderByDescending(x => Convert.ToInt32(x.Price)).ToList();
                        break;
                    default:
                        reas = reas.OrderBy(Ad => Ad.CreatedOn).ToList();
                        break;
                }
            }
            return reas;
        }
        
        public IEnumerable<ValidationUserAds> GetAllActiveAds()
        {
            var reas = _context.tblUserAdds.OrderBy(Ad => Ad.CreatedOn).Where(x => x.Isactive == true).Select(s => new ValidationUserAds()
            {
                Title = s.Title,
                Price = s.Price,
                Year = s.Year,
                Model = s.Model,
                Condition = s.Condition,
                CreatedOn = s.CreatedOn,
                City = s.tblAddress.tblCity.CityName,
                AdURL = s.UserAdsURL,
                CarImage = s.tblUserAddImages.Select(a => a.Image).FirstOrDefault(),
            }).ToList();
            return reas;
        }

        public IEnumerable<ValidationUserAds> GetAllInActiveAds()
        {
            return _context.tblUserAdds.Where(x => x.Isactive == false).Select(s => new ValidationUserAds()
            {
                Title = s.Title,
                Price = s.Price,
                Year = s.Year,
                Model = s.Model,
                Condition = s.Condition,
                CreatedOn = s.CreatedOn,
                City = s.tblAddress.tblCity.CityName,
                AdURL = s.UserAdsURL,
                CarImage = s.tblUserAddImages.Select(a => a.Image).FirstOrDefault(),
            }).ToList();
        }

        public IEnumerable<ValidationUserAds> GetAllUserActiveAds(int UserID)
        {
            return _context.tblUserAdds.Where(x => x.Isactive == true && x.UserID == UserID).Select(s => new ValidationUserAds()
            {
                Title = s.Title,
                Isactive = s.Isactive,
                ExpiryDate = s.ExpiryDate,
                AdURL = s.UserAdsURL,
                CarImage = s.tblUserAddImages.Select(a => a.Image).FirstOrDefault(),
            }).ToList();
        }

        public IEnumerable<ValidationUserAds> GetAllUserInActiveAds(int UserID)
        {
            return _context.tblUserAdds.Where(x => x.Isactive == false && x.UserID == UserID).Select(s => new ValidationUserAds()
            {
                Title = s.Title,
                Isactive = s.Isactive,
                ExpiryDate = s.ExpiryDate,
                AdURL = s.UserAdsURL,
                CarImage = s.tblUserAddImages.Select(a => a.Image).FirstOrDefault(),
            }).ToList();
        }

        public IEnumerable<ValidationUserAds> GetAllUserAds(int UserID)
        {
            return _context.tblUserAdds.Where(x => x.UserID == UserID).Select(s => new ValidationUserAds()
            {
                Title = s.Title,
                Isactive = s.Isactive,
                ExpiryDate = s.ExpiryDate,
                AdURL = s.UserAdsURL,
                CarImage = s.tblUserAddImages.Select(a => a.Image).FirstOrDefault(),
            }).ToList();
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
                State = s.tblAddress.tblState.StateName,
                City = s.tblAddress.tblCity.CityName,
                Area = s.tblAddress.tblZone.ZoneName,
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
                Number = s.tblUser.Number,
                UserCreatedOn = s.tblUser.CreatedOn
            }).FirstOrDefault();

            if (user != null)
            {
                return user;
            }
            else
                return null;
        }

        public ValidationUserAds GetUserAdsDetail(string AdsId)
        {
            var user = _context.tblUserAdds.Where(x => x.UserAdsURL == AdsId).Select(s => new ValidationUserAds()
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
                State = s.tblAddress.tblState.StateName,
                City = s.tblAddress.tblCity.CityName,
                Area = s.tblAddress.tblZone.ZoneName,
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
                Number = s.tblUser.Number,
                UserCreatedOn = s.tblUser.CreatedOn
            }).FirstOrDefault();

            if (user != null)
            {
                return user;
            }
            else
                return null;
        }

        public int InsertUserAds(tblUserAdd model, string city)
        {
            try
            {
                if (model != null)
                {
                    model.UserAdsURL = UserAdsURLGenerate(model.Title, model.Year, city);
                    if (model.UserAdsURL != null)
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

        public string UserAdsURLGenerate(string AdTitle, string Year, string CityLocation)
        {
            if (AdTitle != null && Year != null && CityLocation != null)
            {
                string URL = AdTitle + "-" + Year + "-for-sale-in-" + CityLocation + "-" + OTPGenerator.GenerateRandomOTP() + DateTime.Now.ToString("ddMMyy") + DateTime.Now.Millisecond.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                var rURL = URL.Replace(" ", "-");
                return rURL;
            }
            else
            {
                return null;
            }
        }
    }
}
