using JRCar.BOL;
using JRCar.BOL.Validation_Classes;
using JRCar.DAL.DBLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

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

        public IEnumerable<ValidateShowroomAds> GetAllActiveAdsFilter(string searchTerm, int? minimumPrice, int? maximumPrice, int? sortBy, int? Condition, int? StartYear, int? EndYear, int?[] MakeId, int?[] ModelId, string[] ColorSelected, string[] TransSelected)
        {
            return dbObj.GetAllActiveAdsFilter(searchTerm, minimumPrice, maximumPrice, sortBy, Condition, StartYear, EndYear, MakeId, ModelId, ColorSelected, TransSelected);
        }

        public IEnumerable<ValidateShowroomAds> GetAllActiveAds()
        {
            return dbObj.GetAllActiveAds();
        }

        public IEnumerable<ValidateShowroomAds> GetAllActiveAdsForTabs()
        {
            return dbObj.GetAllActiveAdsForTabs();
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

        public ValidateShowroomAds ShowroomProfileView(string AdId)
        {
            if (AdId != null)
            {
                var reas = dbObj.ShowroomProfileView(AdId);
                return reas;
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
                            UserRepo userRepo = new UserRepo();
                            var showroom = userRepo.GetShowRoomByID(model.tblShowroomID);
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
                                AddressId = showroom.AddressId,
                                ManufacturerId = model.ManufacturerId,
                                ManufacturerCarModelID = model.ManufacturerCarModelID,
                                CategoryId = model.CategoryId,
                                SubCategoryId = model.SubCategoryId
                            };
                            var cityName = AddressRepoObj.GetStateandCity(Convert.ToInt32(showroom.tblAddress.City));
                            var CarId = dbObj.InsertShowroomAds(car, model.Year, cityName.Item2);
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
                        ID = model.CarFeatureID,
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
                        BoxUsbAux = model.BoxUsbAux,
                        UpdatedBy = model.tblShowroomID,
                        CreatedOn = GetShowroomAdsDetailOnlyForUpdate(model.tblCarID).CarFeatureCreatedOn,
                        CreatedBy = GetShowroomAdsDetailOnlyForUpdate(model.tblCarID).CarFeatureCreatedBy,
                    };
                    var CarfeatureID = dbObj.UpdateShowroomCarFeatures(carFeature);

                    if (CarfeatureID > 0)
                    {
                        tblCarModel carModel = new tblCarModel()
                        {
                            ID = model.CarModelID,
                            Year = model.Year,
                            BodyType = model.BodyType,
                            Seater = model.Seater,
                            Assembly = model.Assembly,
                            EngineCapacity = model.EngineCapacity,
                            CarFeatureID = CarfeatureID,
                            EngineType = model.EngineType,
                            UpdatedBy = model.tblShowroomID,
                            CreatedOn = GetShowroomAdsDetailOnlyForUpdate(model.tblCarID).CarModelCreatedOn,
                            CreatedBy = GetShowroomAdsDetailOnlyForUpdate(model.tblCarID).CarModelCreatedBy
                        };
                        var carModelId = dbObj.UpdateShowroomCarModels(carModel);

                        if (carModelId > 0)
                        {
                            UserRepo userRepo = new UserRepo();
                            var showroom = userRepo.GetShowRoomByID(model.tblShowroomID);
                            tblCar car = new tblCar()
                            {
                                ID = model.tblCarID,
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
                                SubCategoryId = model.SubCategoryId,
                                UpdatedBy = model.tblShowroomID,
                                tblCarModel = carModel
                            };
                            var cityName = AddressRepoObj.GetStateandCity(Convert.ToInt32(showroom.tblAddress.City));
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
        
        public List<SelectListItem> GetSubCategoriesByCategoryForDropdown(int CategoryId)
        {
            var AllSubCategory = GetSubCategoriesByCategory(CategoryId);
            List<SelectListItem> Subcategories = new List<SelectListItem>();
            Subcategories.Add(new SelectListItem() { Text = "---Select SubCategory---", Value = "0", Disabled = true, Selected = true });
            foreach (var item in AllSubCategory)
            {
                Subcategories.Add(new SelectListItem() { Text = item.SubCategoryName, Value = item.SubCategoryId.ToString() });
            }
            return Subcategories;
        }
        
        public List<SelectListItem> GetModelsByMakeForDropdown(int ManufacturerId)
        {
            var AllCarModels = GetModelsByMake(ManufacturerId);
            var Carmodels = new List<SelectListItem>();
            Carmodels.Add(new SelectListItem() { Text = "---Select Car Model---", Value = "0", Disabled = true });
            foreach (var item in AllCarModels)
            {
                //if (item.ManufacturerCarModel_Id == ManufacturerCarModelID)
                //    Carmodels.Add(new SelectListItem() { Text = item.Manufacturer_CarModelName, Value = item.ManufacturerCarModel_Id.ToString(), Selected = true });
                //else
                    Carmodels.Add(new SelectListItem() { Text = item.Manufacturer_CarModelName, Value = item.ManufacturerCarModel_Id.ToString() });
            }
            return Carmodels;
        }
        
        public List<SelectListItem> BodyTypes()
        {
            var BodyType = new List<SelectListItem>
            {
                new SelectListItem() { Text = "---Select Body Type---", Value = "0", Disabled = true, Selected = true },
                new SelectListItem() { Text = "Compact hatchback", Value = "Compact hatchback" },
                new SelectListItem() { Text = "Compact sedan", Value = "Compact sedan" },
                new SelectListItem() { Text = "Compact SUV", Value = "Compact SUV" },
                new SelectListItem() { Text = "Convertible", Value = "Convertible" },
                new SelectListItem() { Text = "Coupe", Value = "Coupe" },
                new SelectListItem() { Text = "Crossover", Value = "Crossover" },
                new SelectListItem() { Text = "Double Cabin", Value = "Double Cabin" },
                new SelectListItem() { Text = "Hatchback", Value = "Hatchback" },
                new SelectListItem() { Text = "High Roof", Value = "High Roof" },
                new SelectListItem() { Text = "Micro Van", Value = "Micro Van" },
                new SelectListItem() { Text = "Mini Van", Value = "Mini Van" },
                new SelectListItem() { Text = "Mini Vehicles", Value = "Mini Vehicles" },
                new SelectListItem() { Text = "MPV", Value = "MPV" },
                new SelectListItem() { Text = "Off Road Vehicles", Value = "Off Road Vehicles" },
                new SelectListItem() { Text = "Pick Up", Value = "Pick Up" },
                new SelectListItem() { Text = "Sedan", Value = "Sedan" },
                new SelectListItem() { Text = "Single Cabin", Value = "Single Cabin" },
                new SelectListItem() { Text = "Station Wagon", Value = "Station Wagon" },
                new SelectListItem() { Text = "Subcompact hatchback", Value = "Subcompact hatchback" },
                new SelectListItem() { Text = "SUV", Value = "SUV" },
                new SelectListItem() { Text = "Truck", Value = "Truck" },
                new SelectListItem() { Text = "Van", Value = "Van" }
            };
            return BodyType;
        }
        
        public List<SelectListItem> Colors()
        {
            var Color = new List<SelectListItem>
            {
                new SelectListItem() { Text = "---Select Color---", Value = "0", Disabled = true, Selected = true },
                new SelectListItem() { Text = "White", Value = "White" },
                new SelectListItem() { Text = "Silver", Value = "Silver" },
                new SelectListItem() { Text = "Black", Value = "Black" },
                new SelectListItem() { Text = "Grey", Value = "Grey" },
                new SelectListItem() { Text = "Blue", Value = "Blue" },
                new SelectListItem() { Text = "Green", Value = "Green" },
                new SelectListItem() { Text = "Red", Value = "Red" },
                new SelectListItem() { Text = "Gold", Value = "Gold" },
                new SelectListItem() { Text = "Maroon", Value = "Maroon" },
                new SelectListItem() { Text = "Beige", Value = "Beige" },
                new SelectListItem() { Text = "Pink", Value = "Pink" },
                new SelectListItem() { Text = "Brown", Value = "Brown" },
                new SelectListItem() { Text = "Burgundy", Value = "Burgundy" },
                new SelectListItem() { Text = "Yellow", Value = "Yellow" },
                new SelectListItem() { Text = "Bronze", Value = "Bronze" },
                new SelectListItem() { Text = "Purple", Value = "Purple" },
                new SelectListItem() { Text = "Turquoise", Value = "Turquoise" },
                new SelectListItem() { Text = "Orange", Value = "Orange" },
                new SelectListItem() { Text = "Indigo", Value = "Indigo" },
                new SelectListItem() { Text = "Magenta", Value = "Magenta" },
                new SelectListItem() { Text = "Navy", Value = "Navy" },
                new SelectListItem() { Text = "Unlisted", Value = "1" }
            }; 
            return Color;
        }
        
        public List<SelectListItem> Conditions()
        {
            var condition = new List<SelectListItem>
            {
                new SelectListItem() { Text = "---Select Condition---", Value = "0", Disabled = true, Selected = true },
                new SelectListItem() { Text = "Used", Value = "1" },
                new SelectListItem() { Text = "New", Value = "2" }
            };
            return condition;
        }
        
        public List<SelectListItem> Transmissions()
        {
            var Transmission = new List<SelectListItem>
            {
                new SelectListItem() { Text = "---Select Transmission---", Value = "0", Disabled = true, Selected = true },
                new SelectListItem() { Text = "Automatic", Value = "Automatic" },
                new SelectListItem() { Text = "Manual", Value = "Manual" },
                new SelectListItem() { Text = "CVT Transmission", Value = "CVT Transmission" },
                new SelectListItem() { Text = "Semi-Automatic", Value = "Semi-Automatic" },
                new SelectListItem() { Text = "Dual-Clutch", Value = "Dual-Clutch" }
            };
            return Transmission;
        }
        
        public List<SelectListItem> Assemblys()
        {
            var Assembly = new List<SelectListItem>
            {
                new SelectListItem() { Text = "---Select Assembly---", Value = "0", Disabled = true, Selected = true },
                new SelectListItem() { Text = "Local", Value = "Local" },
                new SelectListItem() { Text = "Imported", Value = "Imported" }
            };
            return Assembly;
        }
        
        public List<SelectListItem> ListOfYears()
        {
            var year = new List<SelectListItem>
            {
                new SelectListItem() { Text = "---Select Model Year---", Value = "0", Disabled = true, Selected = true }
            };
            for (int i = -50; i <= 0; ++i)
            {
                year.Add(new SelectListItem() { Text = DateTime.Now.AddYears(i).ToString("yyyy"), Value = DateTime.Now.AddYears(i).ToString("yyyy") });
            }
            return year;
        }
        
        public List<SelectListItem> GetAllCategories()
        {
            var AllCategory = GetAllCategory();
            var categories = new List<SelectListItem>
            {
                new SelectListItem() { Text = "---Select Category---", Value = "0", Disabled = true, Selected = true }
            };
            foreach (var item in AllCategory)
            {
                categories.Add(new SelectListItem() { Text = item.CategoryName, Value = item.CategoryID.ToString() });
            }
            return categories;
        }
        
        public List<SelectListItem> GetAllMake()
        {
            var AllMake = GetAllMakes();
            var makes = new List<SelectListItem>
            {
                new SelectListItem() { Text = "---Select Model---", Value = "0", Disabled = true, Selected = true }
            };
            foreach (var item in AllMake)
            {
                makes.Add(new SelectListItem() { Text = item.Manufacturer_Name, Value = item.Manufacturer_Id.ToString() });
            }
            return makes;
        }
    }
}
