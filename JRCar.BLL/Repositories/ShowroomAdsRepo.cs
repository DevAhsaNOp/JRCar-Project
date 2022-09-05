using JRCar.BOL;
using JRCar.BOL.Validation_Classes;
using JRCar.DAL.DBLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRCar.BLL.Repositories
{
    public class ShowroomAdsRepo
    {
        private ShowroomAdsDb dbObj;
        private NotificationRepo dbObjNoti;
        private AddressAutofillRepo AddressRepoObj;

        public ShowroomAdsRepo()
        {
            dbObj = new ShowroomAdsDb();
            AddressRepoObj = new AddressAutofillRepo();
            dbObjNoti = new NotificationRepo();
        }

        public IEnumerable<ValidateShowroomAds> GetAllActiveAdsFilter(string searchTerm, int? minimumPrice, int? maximumPrice, int? sortBy, int? StateId, int?[] CityId, int?[] ZoneId)
        {
            return dbObj.GetAllActiveAdsFilter(searchTerm, minimumPrice, maximumPrice, sortBy, StateId, CityId, ZoneId);
        }

        public IEnumerable<ValidateShowroomAds> GetAllActiveAds()
        {
            return dbObj.GetAllActiveAds();
        }

        public IEnumerable<ValidateShowroomAds> GetAllInActiveAds()
        {
            return dbObj.GetAllInActiveAds();
        }

        public ValidateShowroomAds GetShowroomAdsDetail(string AdsId)
        {
            var user = dbObj.GetShowroomAdsDetail(AdsId);
            if (user != null)
            {
                return user;
            }
            else
                return null;
        }

        public ValidateShowroomAds GetShowroomAdsDetailOnlyForUpdate(int AdId)
        {
            if (AdId > 0)
            {
                var reas = dbObj.GetShowroomAdsDetailOnlyForUpdate(AdId);
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

        public ValidateShowroomAds GetShowroomAdURL(int AdsId)
        {
            if (AdsId > 0)
            {
                var AdUrl = dbObj.GetShowroomAdURL(AdsId);
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

        public IEnumerable<ValidateShowroomAds> GetAllShowroomActiveAds(int UserID)
        {
            if (UserID > 0)
            {
                var reas = dbObj.GetAllShowroomActiveAds(UserID);
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

        public IEnumerable<ValidateShowroomAds> GetAllShowroomInActiveAds(int UserID)
        {
            if (UserID > 0)
            {
                var reas = dbObj.GetAllShowroomInActiveAds(UserID);
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

        public IEnumerable<ValidateShowroomAds> GetAllShowroomAds(int UserID)
        {
            if (UserID > 0)
            {
                var reas = dbObj.GetAllShowroomAds(UserID);
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

        public bool InsertShowroomAds(ValidateShowroomAds model)
        {
            try
            {
                if (model != null)
                {
                    tblCarFeature carFeature = new tblCarFeature()
                    {
                        FuelType = model.FuelType,
                        AirConditioned = model.AirConditioned,
                        ABS = model.ABS,
                        AirBag = model.AirBag,
                        PowerWindows = model.PowerWindows,
                        PowerMirrors = model.PowerMirrors,
                        PowerLocks = model.PowerLocks,
                        PowerSteering = model.PowerSteering,
                        ImmobilizerKey = model.ImmobilizerKey,
                        Radio = model.Radio,
                        KeyLessEntry = model.KeyLessEntry,
                        AlloyRims = model.AlloyRims,
                        CoolBox = model.CoolBox,
                        CruiseControl = model.CruiseControl,
                        SunRoof = model.SunRoof,
                        NavigationSystem = model.NavigationSystem,
                        CreatedBy = model.tblShowroomID,
                        RearAcVents = model.RearAcVents,
                        FrontCam = model.FrontCam,
                        CassetPlayer = model.CassetPlayer,
                        DvdPlayer = model.DvdPlayer,
                        SteeringSwitch = model.SteeringSwitch,
                        CdPlayer = model.CdPlayer,
                        ClimateControl = model.ClimateControl,
                        FrontSpeaker = model.FrontSpeaker,
                        HeatedSeat = model.HeatedSeat,
                        RearCamera = model.RearCamera,
                        RearSeatEntertain = model.RearSeatEntertain,
                        RearSpeaker = model.RearSpeaker,
                        BoxUsbAux = model.BoxUsbAux
                    };
                    var CarfeatureID = dbObj.InsertShowroomCarFeatures(carFeature);

                    if (CarfeatureID > 0)
                    {
                        tblCarModel carModel = new tblCarModel()
                        {
                            Year = model.Year,
                            BodyType = model.BodyType,
                            Seater = model.Seater,
                            Assembly = model.Assembly,
                            EngineCapacity = model.EngineCapacity,
                            CarFeatureID = CarfeatureID,
                            EngineType = model.EngineType,
                            CreatedBy = model.tblShowroomID                             
                        };
                        var carModelId = dbObj.InsertShowroomCarModels(carModel);
                        if (carModelId > 0)
                        {
                            tblCar car = new tblCar()
                            {
                                CarModelID = carModelId,
                                tblShowroomID = model.tblShowroomID,
                                RegNo = model.RegNo,
                                RegLocation = model.RegLocation,
                                Condition = model.Condition,
                                MaxSpeed = model.MaxSpeed,
                                Color = model.Color,
                                Price = model.Price,
                                CreatedBy = model.tblShowroomID,
                                GearType = model.GearType,
                                CurrentLocation = model.CurrentLocation,
                                Mileage = model.Mileage,
                                Title = model.Title,
                                Description = model.Description,
                                Transmission = model.Transmission,
                                AddressId = model.AddressId,
                                ManufacturerId = model.ManufacturerId,
                                ManufacturerCarModelID = model.ManufacturerCarModelID,
                                CategoryId = model.CategoryId,
                                SubCategoryId = model.SubCategoryId
                            };
                            var cityName = AddressRepoObj.GetStateandCity(Convert.ToInt32(model.tblAddress.City));
                            var CarId = dbObj.InsertShowroomAds(car, cityName.Item2);
                            if (CarId > 0)
                            {
                                tblCarImage carImage = new tblCarImage()
                                {
                                    CarID = CarId,
                                    Image = model.CarImage
                                };
                                var carImages = InsertShowroomAdsImages(carImage);
                                if (carImages)
                                {
                                    return true;
                                }
                                else
                                    return false;
                            }
                            else
                                return false;
                        }
                        else
                            return false;
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

        public bool UpdateShowroomAds(ValidateShowroomAds model)
        {
            try
            {
                if (model != null)
                {
                    tblCarFeature carFeature = new tblCarFeature()
                    {
                        FuelType = model.FuelType,
                        AirConditioned = model.AirConditioned,
                        ABS = model.ABS,
                        AirBag = model.AirBag,
                        PowerWindows = model.PowerWindows,
                        PowerMirrors = model.PowerMirrors,
                        PowerLocks = model.PowerLocks,
                        PowerSteering = model.PowerSteering,
                        ImmobilizerKey = model.ImmobilizerKey,
                        Radio = model.Radio,
                        KeyLessEntry = model.KeyLessEntry,
                        AlloyRims = model.AlloyRims,
                        CoolBox = model.CoolBox,
                        CruiseControl = model.CruiseControl,
                        SunRoof = model.SunRoof,
                        NavigationSystem = model.NavigationSystem,
                        CreatedBy = model.tblShowroomID,
                        RearAcVents = model.RearAcVents,
                        FrontCam = model.FrontCam,
                        CassetPlayer = model.CassetPlayer,
                        DvdPlayer = model.DvdPlayer,
                        SteeringSwitch = model.SteeringSwitch,
                        CdPlayer = model.CdPlayer,
                        ClimateControl = model.ClimateControl,
                        FrontSpeaker = model.FrontSpeaker,
                        HeatedSeat = model.HeatedSeat,
                        RearCamera = model.RearCamera,
                        RearSeatEntertain = model.RearSeatEntertain,
                        RearSpeaker = model.RearSpeaker,
                        BoxUsbAux = model.BoxUsbAux
                    };
                    var CarfeatureID = dbObj.UpdateShowroomCarFeatures(carFeature);

                    if (CarfeatureID > 0)
                    {
                        tblCarModel carModel = new tblCarModel()
                        {
                            Year = model.Year,
                            BodyType = model.BodyType,
                            Seater = model.Seater,
                            Assembly = model.Assembly,
                            EngineCapacity = model.EngineCapacity,
                            CarFeatureID = CarfeatureID,
                            EngineType = model.EngineType,
                            CreatedBy = model.tblShowroomID
                        };
                        var carModelId = dbObj.UpdateShowroomCarModels(carModel);

                        if (carModelId > 0)
                        {
                            tblCar car = new tblCar()
                            {
                                CarModelID = carModelId,
                                tblShowroomID = model.tblShowroomID,
                                RegNo = model.RegNo,
                                RegLocation = model.RegLocation,
                                Condition = model.Condition,
                                MaxSpeed = model.MaxSpeed,
                                Color = model.Color,
                                Price = model.Price,
                                CreatedBy = model.tblShowroomID,
                                GearType = model.GearType,
                                CurrentLocation = model.CurrentLocation,
                                Mileage = model.Mileage,
                                Title = model.Title,
                                Description = model.Description,
                                Transmission = model.Transmission,
                                AddressId = model.AddressId,
                                ManufacturerId = model.ManufacturerId,
                                ManufacturerCarModelID = model.ManufacturerCarModelID,
                                CategoryId = model.CategoryId,
                                SubCategoryId = model.SubCategoryId
                            };
                            var cityName = AddressRepoObj.GetStateandCity(Convert.ToInt32(model.tblAddress.City));
                            var CarId = dbObj.UpdateShowroomAds(car, cityName.Item2);
                            
                            if (CarId)
                                return true;
                            else
                                return false;
                        }
                        else
                            return false;
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

        public bool InsertShowroomAdsImages(tblCarImage model)
        {
            try
            {
                if (model != null)
                {
                    var user = dbObj.InsertShowroomAdsImages(model);
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

        public bool UpdateShowroomAdsImages(tblCarImage model)
        {
            try
            {
                if (model != null)
                {
                    var user = dbObj.UpdateShowroomAdsImages(model);
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

        public bool InActiveShowroomAds(int AdID)
        {
            try
            {
                if (AdID > 0)
                {
                    var user = dbObj.InActiveShowroomAds(AdID);
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

        public bool ReActiveShowroomAds(int AdID)
        {
            try
            {
                if (AdID > 0)
                {
                    var user = dbObj.ReActiveShowroomAds(AdID);
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
