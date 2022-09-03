using JRCar.BOL;
using JRCar.BOL.Validation_Classes;
using JRCar.DAL.UserDefine;
using System;
using System.Collections;
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

        public IEnumerable<ValidationUserAds> GetAllActiveAdsFilter(string searchTerm, int? minimumPrice, int? maximumPrice, int? sortBy, int? StateId, int?[] CityId, int?[] ZoneId)
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
                tblAddress = s.tblAddress,
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
            if (StateId > 0 && CityId == null && ZoneId == null)
            {
                reas = reas.Where(x => x.tblAddress.State == StateId).ToList();
            }
            if (StateId > 0 && CityId != null && ZoneId == null)
            {
                List<ValidationUserAds> AdsList = new List<ValidationUserAds>();
                foreach (var item in CityId)
                {
                    var val = item;
                    var ads = reas.Where(x => x.tblAddress.City == item).ToList();
                    AdsList.AddRange(ads);
                }
                reas = AdsList;
            }
            if (StateId > 0 && CityId != null && ZoneId != null)
            {
                List<ValidationUserAds> AdsList = new List<ValidationUserAds>();
                foreach (var cities in CityId)
                {
                    var val = cities;
                    foreach (var zones in ZoneId)
                    {
                        var val1 = cities;
                        var ads = reas.Where(x => x.tblAddress.City == cities && x.tblAddress.Area == zones).ToList();
                        AdsList.AddRange(ads);
                    }
                }
                reas = AdsList;
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

        public ValidationUserAds GetUserAdsDetailOnlyForUpdate(int AdId)
        {
            var user = _context.tblUserAdds.Where(x => x.ID == AdId).Select(s => new ValidationUserAds()
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
                ManufacturerId = s.ManufacturerId,
                Manufacturer_Name = s.tblManufacturer.Manufacturer_Name,
                ManufacturerCarModelID = s.ManufacturerCarModelID,
                Manufacturer_CarModelName = s.tblManfacturerCarModel.Manufacturer_CarModelName,
                CategoryId = s.CategoryId,
                CategoryName = s.tblCategory.CategoryName,
                SubCategoryId = s.SubCategoryId,
                SubCategoryName = s.tblSubCategory.SubCategoryName,
                State = s.tblAddress.tblState.StateName,
                City = s.tblAddress.tblCity.CityName,
                Area = s.tblAddress.tblZone.ZoneName,
                AddressId = s.AddressId,
                CompleteAddress = ((s.tblAddress.CompleteAddress == null) ? "Not Available" : s.tblAddress.CompleteAddress),
                Isactive = s.Isactive,
                Isarchive = s.Isarchive,
                CreatedOn = s.CreatedOn,
                ExpiryDate = s.ExpiryDate,
                tblAddress = s.tblAddress,
                tblCategory = s.tblCategory,
                tblManfacturerCarModel = s.tblManfacturerCarModel,
                tblManufacturer = s.tblManufacturer,
                tblSubCategory = s.tblSubCategory,
                tblUserAddImages = s.tblUserAddImages,

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

        public ValidationUserAds GetUserAdURL(int AdsId)
        {
            var user = _context.tblUserAdds.Where(x => x.ID == AdsId).Select(s => new ValidationUserAds()
            {
                AdURL = s.UserAdsURL
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

        public IEnumerable<tblManufacturer> GetAllMakes()
        {
            return _context.tblManufacturers.ToList();
        }

        public IEnumerable<tblManfacturerCarModel> GetAllModels()
        {
            return _context.tblManfacturerCarModels.ToList();
        }

        public IEnumerable<tblCategory> GetAllCategory()
        {
            return _context.tblCategories.ToList();
        }

        public IEnumerable<tblSubCategory> GetAllSubCategory()
        {
            return _context.tblSubCategories.ToList();
        }

        public IEnumerable<tblManfacturerCarModel> GetModelsByMake(int ManufacturerId)
        {
            return _context.tblManfacturerCarModels.Where(x => x.Manufacturer_Id == ManufacturerId).ToList();
        }

        public IEnumerable<tblSubCategory> GetSubCategoriesByCategory(int CategoryId)
        {
            return _context.tblSubCategories.Where(x => x.CategoryId == CategoryId).ToList();
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
