using JRCar.BOL;
using JRCar.BOL.Validation_Classes;
using JRCar.DAL.DBLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace JRCar.BLL.Repositories
{
    public class UserAdsRepo
    {
        private UserAds dbObj;
        private NotificationRepo dbObjNoti;
        private AddressAutofillRepo AddressRepoObj;

        public UserAdsRepo()
        {
            dbObj = new UserAds();
            AddressRepoObj = new AddressAutofillRepo();
            dbObjNoti = new NotificationRepo();
        }

        public IEnumerable<ValidationUserAds> GetAllActiveAdsFilter(string searchTerm, int? minimumPrice, int? maximumPrice, int? sortBy, int? StateId, int?[] CityId, int?[] ZoneId)
        {
            return dbObj.GetAllActiveAdsFilter(searchTerm, minimumPrice, maximumPrice, sortBy, StateId, CityId, ZoneId);
        }

        public IEnumerable<ValidationUserAds> GetAllActiveAds()
        {
            return dbObj.GetAllActiveAds();
        }

        public IEnumerable<ValidationUserAds> GetAllInActiveAds()
        {
            return dbObj.GetAllInActiveAds();
        }

        public ValidationUserAds GetUserAdsDetail(string AdsId)
        {
            var user = dbObj.GetUserAdsDetail(AdsId);
            if (user != null)
            {
                return user;
            }
            else
                return null;
        }
        
        public ValidationUserAds GetUserAdsDetailOnlyForUpdate(int AdId)
        {
            if (AdId > 0)
            {
                var reas = dbObj.GetUserAdsDetailOnlyForUpdate(AdId);
                if (reas != null)
                {
                    return reas;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public ValidationUserAds GetUserAdURL(int AdsId)
        {
            if (AdsId > 0)
            {
                var AdUrl = dbObj.GetUserAdURL(AdsId);
                if (AdUrl != null)
                {
                    return AdUrl;
                }
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<ValidationUserAds> GetAllUserActiveAds(int UserID)
        {
            if (UserID > 0)
            {
                var reas = dbObj.GetAllUserActiveAds(UserID);
                if (reas != null)
                {
                    return reas;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public IEnumerable<ValidationUserAds> GetAllUserInActiveAds(int UserID)
        {
            if (UserID > 0)
            {
                var reas = dbObj.GetAllUserInActiveAds(UserID);
                if (reas != null)
                {
                    return reas;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public IEnumerable<ValidationUserAds> GetAllUserAds(int UserID)
        {
            if (UserID > 0)
            {
                var reas = dbObj.GetAllUserAds(UserID);
                if (reas != null)
                {
                    return reas;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public bool InsertUserAds(ValidationUserAds model)
        {
            try
            {
                if (model != null)
                {
                    tblAddress addrs = new tblAddress()
                    {
                        State = Convert.ToInt32(model.State),
                        City = Convert.ToInt32(model.City),
                        Area = Convert.ToInt32(model.Area),
                        CompleteAddress = (model.CompleteAddress == null) ? "" : model.CompleteAddress,
                    };
                    var addrsID = AddressRepoObj.InsertAddress(addrs);
                    if (addrsID > 0)
                    {
                        tblUserAdd userAds = new tblUserAdd()
                        {
                            UserID = model.UserID,
                            Model = model.Model,
                            Year = model.Year,
                            Condition = model.Condition,
                            Title = model.Title,
                            Description = model.Description,
                            Price = model.Price,
                            AddressId = addrsID,
                            Latitude = model.Latitude,
                            Longitude = model.Longitude,
                            CategoryId = model.CategoryId,
                            SubCategoryId = model.SubCategoryId,
                            ManufacturerId = model.ManufacturerId,
                            ManufacturerCarModelID = model.ManufacturerCarModelID
                        };
                        var cityName = AddressRepoObj.GetStateandCity(Convert.ToInt32(model.City));
                        var user = dbObj.InsertUserAds(userAds, cityName.Item2);
                        if (user > 0)
                        {
                            ValidateUserAdsImage userAdImagesObj = new ValidateUserAdsImage()
                            {
                                Image = model.CarImage,
                                tblUserAddID = user
                            };
                            var userAdImages = InsertUserAdsImages(userAdImagesObj);
                            if (userAdImages)
                            {
                                var UserAdURL = GetUserAdURL(user).AdURL;
                                tblNotification notification = new tblNotification()
                                {
                                    Title = model.Title,
                                    Description = WithMaxLength(model.Description, 15),
                                    AdURL = UserAdURL,
                                    FromUserID = model.UserID
                                };
                                dbObjNoti.InsertNotification(notification);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string WithMaxLength(string value, int maxLength)
        {
            return value?.Substring(0, Math.Min(value.Length, maxLength));
        }

        public bool UpdateUserAds(ValidationUserAds model)
        {
            try
            {
                if (model != null)
                {
                    tblAddress addrs = new tblAddress()
                    {
                        ID = model.AddressId.Value,
                        State = model.StateID,
                        City = model.CityID,
                        Area = model.AreaID,
                        CompleteAddress = (model.CompleteAddress == null) ? "" : model.CompleteAddress,
                    };
                    var addrsID = AddressRepoObj.UpdateAddress(addrs);
                    if (addrsID > 0)
                    {
                        tblUserAdd userAds = new tblUserAdd()
                        {
                            ID = model.AdID,
                            UserID = model.UserID,
                            Model = model.Model,
                            Year = model.Year,
                            Condition = model.Condition,
                            Title = model.Title,
                            Description = model.Description,
                            Price = model.Price,
                            AddressId = addrsID,
                            Latitude = model.Latitude,
                            Longitude = model.Longitude,
                            CategoryId = model.CategoryId,
                            SubCategoryId = model.SubCategoryId,
                            ManufacturerId = model.ManufacturerId,
                            ManufacturerCarModelID = model.ManufacturerCarModelID
                        };
                        var cityName = AddressRepoObj.GetStateandCity(model.CityID);
                        var user = dbObj.UpdateUserAds(userAds, cityName.Item2);
                        if (user)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertUserAdsImages(ValidateUserAdsImage model)
        {
            try
            {
                if (model != null)
                {
                    tblUserAddImage userAddImage = new tblUserAddImage()
                    {
                        Image = model.Image,
                        tblUserAddID = model.tblUserAddID
                    };
                    var user = dbObj.InsertUserAdsImages(userAddImage);
                    if (user == true)
                        return user;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
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
                    var user = dbObj.UpdateUserAdsImages(model);
                    if (user == true)
                        return user;
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

        public bool InActiveUserAds(int AdID)
        {
            try
            {
                if (AdID > 0)
                {
                    var user = dbObj.InActiveUserAds(AdID);
                    if (user == true)
                        return user;
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

        public bool ReActiveUserAds(int AdID)
        {
            try
            {
                if (AdID > 0)
                {
                    var user = dbObj.ReActiveUserAds(AdID);
                    if (user == true)
                        return user;
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

        public bool CarShortlistedActive(int CarID, int UserID)
        {
            if (CarID > 0 && UserID > 0)
            {
                tblFavAdd favAdd = new tblFavAdd()
                {
                    CarID = CarID,
                    UserID = UserID,
                    Isactive = true,
                    Isarchived = false                    
                };
                var reas = dbObj.CarShortlistedActive(favAdd);
                return reas;
            }
            else
                return false;
        }
        public bool CarShortlistedInActive(int favAdID)
        {
            if (favAdID > 0 )
            {
                tblFavAdd favAdd = new tblFavAdd()
                {
                    ID = favAdID,
                };
                var reas = dbObj.CarShortlistedInActive(favAdd);
                return reas;
            }
            else
                return false;
        }

        public IEnumerable<tblFavAdd> AllCarShortlisted(int UserID)
        {
            var user = dbObj.AllCarShortlisted(UserID);
            if (user != null)
            {
                return user;
            }
            else
                return null;
        }

        public IEnumerable<tblManufacturer> GetAllMakes()
        {
            return dbObj.GetAllMakes();
        }

        public IEnumerable<tblManfacturerCarModel> GetAllModels()
        {
            return dbObj.GetAllModels();
        }

        public IEnumerable<tblCategory> GetAllCategory()
        {
            return dbObj.GetAllCategory();
        }

        public IEnumerable<tblSubCategory> GetAllSubCategory()
        {
            return dbObj.GetAllSubCategory();
        }

        public IEnumerable<tblManfacturerCarModel> GetModelsByMake(int ManufacturerId)
        {
            return dbObj.GetModelsByMake(ManufacturerId);
        }

        public IEnumerable<tblSubCategory> GetSubCategoriesByCategory(int CategoryId)
        {
            return dbObj.GetSubCategoriesByCategory(CategoryId);
        }
    }
}