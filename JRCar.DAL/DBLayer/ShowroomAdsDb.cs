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
    public class ShowroomAdsDb
    {
        private jrcarEntities _context;

        public ShowroomAdsDb()
        {
            _context = new jrcarEntities();
        }

        public IEnumerable<ValidateShowroomAds> GetAllActiveAdsFilter(string searchTerm, int? minimumPrice, int? maximumPrice, int? sortBy, int? StateId, int?[] CityId, int?[] ZoneId)
        {
            var reas = _context.tblCars.Where(x => x.Isactive == true).Select(s => new ValidateShowroomAds()
            {
                Title = s.Title,
                Price = s.Price,
                Year = s.tblCarModel.Year,
                Manufacturer_Name = s.tblManufacturer.Manufacturer_Name,
                Manufacturer_CarModelName = s.tblManfacturerCarModel.Manufacturer_CarModelName,
                Condition = s.Condition,
                tblCarCreatedOn = s.CreatedOn,
                CurrentLocation = s.CurrentLocation,
                tblAddress = s.tblAddress,
                CarsURL = s.CarsURL,
                CarImage = s.tblCarImages.Select(a => a.Image).FirstOrDefault(),
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
                        reas = reas.OrderByDescending(Ad => Ad.tblCarCreatedOn).ToList();
                        break;
                    case 2:
                        reas = reas.OrderBy(x => Convert.ToInt32(x.Price)).ToList();
                        break;
                    case 3:
                        reas = reas.OrderByDescending(x => Convert.ToInt32(x.Price)).ToList();
                        break;
                    default:
                        reas = reas.OrderBy(Ad => Ad.tblCarCreatedOn).ToList();
                        break;
                }
            }
            if (StateId > 0 && CityId == null && ZoneId == null)
            {
                reas = reas.Where(x => x.tblAddress.State == StateId).ToList();
            }
            if (StateId > 0 && CityId != null && ZoneId == null)
            {
                List<ValidateShowroomAds> AdsList = new List<ValidateShowroomAds>();
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
                List<ValidateShowroomAds> AdsList = new List<ValidateShowroomAds>();
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

        public IEnumerable<ValidateShowroomAds> GetAllActiveAds()
        {
            var reas = _context.tblCars.OrderBy(Ad => Ad.CreatedOn).Where(x => x.Isactive == true).Select(s => new ValidateShowroomAds()
            {
                tblCarID = s.ID,
                Title = s.Title,
                Price = s.Price,
                Year = s.tblCarModel.Year,
                Manufacturer_Name = s.tblManufacturer.Manufacturer_Name,
                Manufacturer_CarModelName = s.tblManfacturerCarModel.Manufacturer_CarModelName,
                Condition = s.Condition,
                tblCarCreatedOn = s.CreatedOn,
                CurrentLocation = s.CurrentLocation,
                CarsURL = s.CarsURL,
                CarImage = s.tblCarImages.Select(a => a.Image).FirstOrDefault(),
            }).ToList();
            return reas;
        }

        public IEnumerable<ValidateShowroomAds> GetAllInActiveAds()
        {
            return _context.tblCars.Where(x => x.Isactive == false).Select(s => new ValidateShowroomAds()
            {
                tblCarID = s.ID,
                Title = s.Title,
                Price = s.Price,
                Year = s.tblCarModel.Year,
                Manufacturer_Name = s.tblManufacturer.Manufacturer_Name,
                Manufacturer_CarModelName = s.tblManfacturerCarModel.Manufacturer_CarModelName,
                Condition = s.Condition,
                tblCarCreatedOn = s.CreatedOn,
                CurrentLocation = s.CurrentLocation,
                CarsURL = s.CarsURL,
                CarImage = s.tblCarImages.Select(a => a.Image).FirstOrDefault(),
            }).ToList();
        }

        public IEnumerable<ValidateShowroomAds> GetAllShowroomActiveAds(int ShowroomAdID)
        {
            return _context.tblCars.Where(x => x.Isactive == true && x.ID == ShowroomAdID).Select(s => new ValidateShowroomAds()
            {
                tblCarID = s.ID,
                Title = s.Title,
                Price = s.Price,
                Year = s.tblCarModel.Year,
                Manufacturer_Name = s.tblManufacturer.Manufacturer_Name,
                Manufacturer_CarModelName = s.tblManfacturerCarModel.Manufacturer_CarModelName,
                Condition = s.Condition,
                tblCarIsactive = s.Isactive,
                tblCarCreatedOn = s.CreatedOn,
                CurrentLocation = s.CurrentLocation,
                CarsURL = s.CarsURL,
                CarImage = s.tblCarImages.Select(a => a.Image).FirstOrDefault(),
            }).ToList();
        }

        public IEnumerable<ValidateShowroomAds> GetAllShowroomInActiveAds(int ShowroomAdID)
        {
            return _context.tblCars.Where(x => x.Isactive == false && x.ID == ShowroomAdID).Select(s => new ValidateShowroomAds()
            {
                tblCarID = s.ID,
                Title = s.Title,
                Price = s.Price,
                Year = s.tblCarModel.Year,
                Manufacturer_Name = s.tblManufacturer.Manufacturer_Name,
                Manufacturer_CarModelName = s.tblManfacturerCarModel.Manufacturer_CarModelName,
                Condition = s.Condition,
                tblCarIsactive = s.Isactive,
                tblCarCreatedOn = s.CreatedOn,
                CurrentLocation = s.CurrentLocation,
                CarsURL = s.CarsURL,
                CarImage = s.tblCarImages.Select(a => a.Image).FirstOrDefault(),
            }).ToList();
        }

        public IEnumerable<ValidateShowroomAds> GetAllShowroomAds(int ShowroomAdID)
        {
            return _context.tblCars.Where(x => x.ID == ShowroomAdID).Select(s => new ValidateShowroomAds()
            {
                tblCarID = s.ID,
                Title = s.Title,
                Price = s.Price,
                Year = s.tblCarModel.Year,
                Manufacturer_Name = s.tblManufacturer.Manufacturer_Name,
                Manufacturer_CarModelName = s.tblManfacturerCarModel.Manufacturer_CarModelName,
                Condition = s.Condition,
                tblCarIsactive = s.Isactive,
                tblCarCreatedOn = s.CreatedOn,
                CurrentLocation = s.CurrentLocation,
                CarsURL = s.CarsURL,
                CarImage = s.tblCarImages.Select(a => a.Image).FirstOrDefault(),
            }).ToList();
        }

        public ValidateShowroomAds GetShowroomAdsDetail(int AdsId)
        {
            var Showroom = _context.tblCars.Where(x => x.ID == AdsId).Select(s => new ValidateShowroomAds()
            {
                /*---Showroom Car Details---*/
                tblCarID = s.ID,
                tblCarModelID = s.CarModelID,
                tblShowroomID = s.tblShowroomID,
                RegNo = s.RegNo,
                RegLocation = s.RegLocation,
                Condition = s.Condition,
                MaxSpeed = s.MaxSpeed,
                Color = s.Color,
                Price = s.Price,
                tblCarCreatedBy = s.CreatedBy,
                tblCarCreatedOn = s.CreatedOn,
                tblCarUpdatedOn = s.UpdatedOn,
                tblCarUpdatedBy = s.UpdatedBy,
                tblCarIsactive = s.Isactive,
                tblCarIsarchive = s.Isarchive,
                GearType = s.GearType,
                CurrentLocation = s.CurrentLocation,
                Mileage = s.Mileage,
                Title = s.Title,
                Description = ((s.Description == null) ? "" : s.Description),
                Transmission = s.Transmission,
                AddressId = s.AddressId,
                CarsURL = s.CarsURL,
                ManufacturerId = s.ManufacturerId,
                Manufacturer_Name = s.tblManufacturer.Manufacturer_Name,
                ManufacturerCarModelID = s.ManufacturerCarModelID,
                Manufacturer_CarModelName = s.tblManfacturerCarModel.Manufacturer_CarModelName,
                CategoryId = s.CategoryId,
                CategoryName = s.tblCategory.CategoryName,
                SubCategoryId = s.SubCategoryId,
                SubCategoryName = s.tblSubCategory.SubCategoryName,

                /*---Showroom Car Model Details---*/
                CarModelID = s.CarModelID,
                Year = s.tblCarModel.Year,
                BodyType = s.tblCarModel.BodyType,
                Seater = s.tblCarModel.Seater,
                Assembly = s.tblCarModel.Assembly,
                EngineCapacity = s.tblCarModel.EngineCapacity,
                CarFeatureID = s.tblCarModel.CarFeatureID,
                CarModelCreatedBy = s.tblCarModel.CreatedBy,
                CarModelUpdatedBy = s.tblCarModel.UpdatedBy,
                CarModelCreatedOn = s.tblCarModel.CreatedOn,
                CarModelUpdatedOn = s.tblCarModel.UpdatedOn,
                CarModelIsactive = s.tblCarModel.Isactive,
                CarModelIsarchive = s.tblCarModel.Isarchive,
                EngineType = s.tblCarModel.EngineType,

                /*---Showroom Car Features Details---*/
                tblCarFeatureID = s.tblCarModel.tblCarFeature.ID,
                FuelType = s.tblCarModel.tblCarFeature.FuelType,
                AirConditioned = s.tblCarModel.tblCarFeature.AirConditioned,
                ABS = s.tblCarModel.tblCarFeature.ABS,
                AirBag = s.tblCarModel.tblCarFeature.AirBag,
                PowerWindows = s.tblCarModel.tblCarFeature.PowerWindows,
                PowerMirrors = s.tblCarModel.tblCarFeature.PowerMirrors,
                PowerLocks = s.tblCarModel.tblCarFeature.PowerLocks,
                PowerSteering = s.tblCarModel.tblCarFeature.PowerSteering,
                ImmobilizerKey = s.tblCarModel.tblCarFeature.ImmobilizerKey,
                Radio = s.tblCarModel.tblCarFeature.Radio,
                KeyLessEntry = s.tblCarModel.tblCarFeature.KeyLessEntry,
                AlloyRims = s.tblCarModel.tblCarFeature.AlloyRims,
                CoolBox = s.tblCarModel.tblCarFeature.CoolBox,
                CruiseControl = s.tblCarModel.tblCarFeature.CruiseControl,
                SunRoof = s.tblCarModel.tblCarFeature.SunRoof,
                NavigationSystem = s.tblCarModel.tblCarFeature.NavigationSystem,
                CarFeatureCreatedBy = s.tblCarModel.tblCarFeature.CreatedBy,
                CarFeatureCreatedOn = s.tblCarModel.tblCarFeature.CreatedOn,
                CarFeatureUpdatedOn = s.tblCarModel.tblCarFeature.UpdatedOn,
                CarFeatureUpdatedBy = s.tblCarModel.tblCarFeature.UpdatedBy,
                CarFeatureIsactive = s.tblCarModel.tblCarFeature.Isactive,
                CarFeatureIsarchive = s.tblCarModel.tblCarFeature.Isarchive,
                RearAcVents = s.tblCarModel.tblCarFeature.RearAcVents,
                FrontCam = s.tblCarModel.tblCarFeature.FrontCam,
                CassetPlayer = s.tblCarModel.tblCarFeature.CassetPlayer,
                DvdPlayer = s.tblCarModel.tblCarFeature.DvdPlayer,
                SteeringSwitch = s.tblCarModel.tblCarFeature.SteeringSwitch,
                CdPlayer = s.tblCarModel.tblCarFeature.CdPlayer,
                ClimateControl = s.tblCarModel.tblCarFeature.ClimateControl,
                FrontSpeaker = s.tblCarModel.tblCarFeature.FrontSpeaker,
                HeatedSeat = s.tblCarModel.tblCarFeature.HeatedSeat,
                RearCamera = s.tblCarModel.tblCarFeature.RearCamera,
                RearSeatEntertain = s.tblCarModel.tblCarFeature.RearSeatEntertain,
                RearSpeaker = s.tblCarModel.tblCarFeature.RearSpeaker,
                BoxUsbAux = s.tblCarModel.tblCarFeature.BoxUsbAux,

                /*---Extra Things To Be Required---*/
                Latitude = ((s.tblShowroom.Latitude == null) ? "" : s.tblShowroom.Latitude.ToString()),
                Longitude = ((s.tblShowroom.Longitude == null) ? "" : s.tblShowroom.Longitude.ToString()),
                State = s.tblAddress.tblState.StateName,
                City = s.tblAddress.tblCity.CityName,
                Area = s.tblAddress.tblZone.ZoneName,
                CompleteAddress = ((s.tblAddress.CompleteAddress == null) ? "Not Available" : s.tblAddress.CompleteAddress),
                CarImage = s.tblCarImages.Select(a => a.Image).FirstOrDefault(),

                /*---Showroom Details---*/
                ShowroomImage = s.tblShowroom.Image,
                ShowroomName = s.tblShowroom.FullName,
                ShowroomEmail = s.tblShowroom.Email,
                ShowroomRole = s.tblShowroom.tblRole.Role,
                ShowroomNumber = s.tblShowroom.Contact,
                ShowroomCreatedOn = s.tblShowroom.CreatedOn,
                ShowroomURL = s.tblShowroom.ShowroomURL

            }).FirstOrDefault();

            if (Showroom != null)
            {
                return Showroom;
            }
            else
                return null;
        }

        public ValidateShowroomAds GetShowroomAdsDetailOnlyForUpdate(int AdId)
        {
            var user = _context.tblCars.Where(x => x.ID == AdId).Select(s => new ValidateShowroomAds()
            {
                /*---Showroom Car Details---*/
                tblCarID = s.ID,
                tblCarModelID = s.CarModelID,
                tblShowroomID = s.tblShowroomID,
                RegNo = s.RegNo,
                RegLocation = s.RegLocation,
                Condition = s.Condition,
                MaxSpeed = s.MaxSpeed,
                Color = s.Color,
                Price = s.Price,
                tblCarCreatedBy = s.CreatedBy,
                tblCarCreatedOn = s.CreatedOn,
                tblCarUpdatedOn = s.UpdatedOn,
                tblCarUpdatedBy = s.UpdatedBy,
                tblCarIsactive = s.Isactive,
                tblCarIsarchive = s.Isarchive,
                GearType = s.GearType,
                CurrentLocation = s.CurrentLocation,
                Mileage = s.Mileage,
                Title = s.Title,
                Description = ((s.Description == null) ? "" : s.Description),
                Transmission = s.Transmission,
                AddressId = s.AddressId,
                CarsURL = s.CarsURL,
                ManufacturerId = s.ManufacturerId,
                Manufacturer_Name = s.tblManufacturer.Manufacturer_Name,
                ManufacturerCarModelID = s.ManufacturerCarModelID,
                Manufacturer_CarModelName = s.tblManfacturerCarModel.Manufacturer_CarModelName,
                CategoryId = s.CategoryId,
                CategoryName = s.tblCategory.CategoryName,
                SubCategoryId = s.SubCategoryId,
                SubCategoryName = s.tblSubCategory.SubCategoryName,

                /*---Showroom Car Model Details---*/
                CarModelID = s.CarModelID,
                Year = s.tblCarModel.Year,
                BodyType = s.tblCarModel.BodyType,
                Seater = s.tblCarModel.Seater,
                Assembly = s.tblCarModel.Assembly,
                EngineCapacity = s.tblCarModel.EngineCapacity,
                CarFeatureID = s.tblCarModel.CarFeatureID,
                CarModelCreatedBy = s.tblCarModel.CreatedBy,
                CarModelUpdatedBy = s.tblCarModel.UpdatedBy,
                CarModelCreatedOn = s.tblCarModel.CreatedOn,
                CarModelUpdatedOn = s.tblCarModel.UpdatedOn,
                CarModelIsactive = s.tblCarModel.Isactive,
                CarModelIsarchive = s.tblCarModel.Isarchive,
                EngineType = s.tblCarModel.EngineType,

                /*---Showroom Car Features Details---*/
                tblCarFeatureID = s.tblCarModel.tblCarFeature.ID,
                FuelType = s.tblCarModel.tblCarFeature.FuelType,
                AirConditioned = s.tblCarModel.tblCarFeature.AirConditioned,
                ABS = s.tblCarModel.tblCarFeature.ABS,
                AirBag = s.tblCarModel.tblCarFeature.AirBag,
                PowerWindows = s.tblCarModel.tblCarFeature.PowerWindows,
                PowerMirrors = s.tblCarModel.tblCarFeature.PowerMirrors,
                PowerLocks = s.tblCarModel.tblCarFeature.PowerLocks,
                PowerSteering = s.tblCarModel.tblCarFeature.PowerSteering,
                ImmobilizerKey = s.tblCarModel.tblCarFeature.ImmobilizerKey,
                Radio = s.tblCarModel.tblCarFeature.Radio,
                KeyLessEntry = s.tblCarModel.tblCarFeature.KeyLessEntry,
                AlloyRims = s.tblCarModel.tblCarFeature.AlloyRims,
                CoolBox = s.tblCarModel.tblCarFeature.CoolBox,
                CruiseControl = s.tblCarModel.tblCarFeature.CruiseControl,
                SunRoof = s.tblCarModel.tblCarFeature.SunRoof,
                NavigationSystem = s.tblCarModel.tblCarFeature.NavigationSystem,
                CarFeatureCreatedBy = s.tblCarModel.tblCarFeature.CreatedBy,
                CarFeatureCreatedOn = s.tblCarModel.tblCarFeature.CreatedOn,
                CarFeatureUpdatedOn = s.tblCarModel.tblCarFeature.UpdatedOn,
                CarFeatureUpdatedBy = s.tblCarModel.tblCarFeature.UpdatedBy,
                CarFeatureIsactive = s.tblCarModel.tblCarFeature.Isactive,
                CarFeatureIsarchive = s.tblCarModel.tblCarFeature.Isarchive,
                RearAcVents = s.tblCarModel.tblCarFeature.RearAcVents,
                FrontCam = s.tblCarModel.tblCarFeature.FrontCam,
                CassetPlayer = s.tblCarModel.tblCarFeature.CassetPlayer,
                DvdPlayer = s.tblCarModel.tblCarFeature.DvdPlayer,
                SteeringSwitch = s.tblCarModel.tblCarFeature.SteeringSwitch,
                CdPlayer = s.tblCarModel.tblCarFeature.CdPlayer,
                ClimateControl = s.tblCarModel.tblCarFeature.ClimateControl,
                FrontSpeaker = s.tblCarModel.tblCarFeature.FrontSpeaker,
                HeatedSeat = s.tblCarModel.tblCarFeature.HeatedSeat,
                RearCamera = s.tblCarModel.tblCarFeature.RearCamera,
                RearSeatEntertain = s.tblCarModel.tblCarFeature.RearSeatEntertain,
                RearSpeaker = s.tblCarModel.tblCarFeature.RearSpeaker,
                BoxUsbAux = s.tblCarModel.tblCarFeature.BoxUsbAux,

                /*---Extra Things To Be Required---*/
                Latitude = ((s.tblShowroom.Latitude == null) ? "" : s.tblShowroom.Latitude.ToString()),
                Longitude = ((s.tblShowroom.Longitude == null) ? "" : s.tblShowroom.Longitude.ToString()),
                State = s.tblAddress.tblState.StateName,
                City = s.tblAddress.tblCity.CityName,
                Area = s.tblAddress.tblZone.ZoneName,
                CompleteAddress = ((s.tblAddress.CompleteAddress == null) ? "Not Available" : s.tblAddress.CompleteAddress),
                CarImage = s.tblCarImages.Select(a => a.Image).FirstOrDefault(),

                /*---Showroom Details---*/
                ShowroomImage = s.tblShowroom.Image,
                ShowroomName = s.tblShowroom.FullName,
                ShowroomEmail = s.tblShowroom.Email,
                ShowroomRole = s.tblShowroom.tblRole.Role,
                ShowroomNumber = s.tblShowroom.Contact,
                ShowroomCreatedOn = s.tblShowroom.CreatedOn,
                ShowroomURL = s.tblShowroom.ShowroomURL

            }).FirstOrDefault();

            if (user != null)
            {
                return user;
            }
            else
                return null;
        }

        public ValidateShowroomAds GetShowroomAdURL(int AdsId)
        {
            var user = _context.tblCars.Where(x => x.ID == AdsId).Select(s => new ValidateShowroomAds()
            {
                CarsURL = s.CarsURL
            }).FirstOrDefault();

            if (user != null)
            {
                return user;
            }
            else
                return null;
        }

        public ValidateShowroomAds GetShowroomAdsDetail(string AdsId)
        {
            var user = _context.tblCars.Where(x => x.CarsURL == AdsId).Select(s => new ValidateShowroomAds()
            {
                /*---Showroom Car Details---*/
                tblCarID = s.ID,
                tblCarModelID = s.CarModelID,
                tblShowroomID = s.tblShowroomID,
                RegNo = s.RegNo,
                RegLocation = s.RegLocation,
                Condition = s.Condition,
                MaxSpeed = s.MaxSpeed,
                Color = s.Color,
                Price = s.Price,
                tblCarCreatedBy = s.CreatedBy,
                tblCarCreatedOn = s.CreatedOn,
                tblCarUpdatedOn = s.UpdatedOn,
                tblCarUpdatedBy = s.UpdatedBy,
                tblCarIsactive = s.Isactive,
                tblCarIsarchive = s.Isarchive,
                GearType = s.GearType,
                CurrentLocation = s.CurrentLocation,
                Mileage = s.Mileage,
                Title = s.Title,
                Description = ((s.Description == null) ? "" : s.Description),
                Transmission = s.Transmission,
                AddressId = s.AddressId,
                CarsURL = s.CarsURL,
                ManufacturerId = s.ManufacturerId,
                Manufacturer_Name = s.tblManufacturer.Manufacturer_Name,
                ManufacturerCarModelID = s.ManufacturerCarModelID,
                Manufacturer_CarModelName = s.tblManfacturerCarModel.Manufacturer_CarModelName,
                CategoryId = s.CategoryId,
                CategoryName = s.tblCategory.CategoryName,
                SubCategoryId = s.SubCategoryId,
                SubCategoryName = s.tblSubCategory.SubCategoryName,

                /*---Showroom Car Model Details---*/
                CarModelID = s.CarModelID,
                Year = s.tblCarModel.Year,
                BodyType = s.tblCarModel.BodyType,
                Seater = s.tblCarModel.Seater,
                Assembly = s.tblCarModel.Assembly,
                EngineCapacity = s.tblCarModel.EngineCapacity,
                CarFeatureID = s.tblCarModel.CarFeatureID,
                CarModelCreatedBy = s.tblCarModel.CreatedBy,
                CarModelUpdatedBy = s.tblCarModel.UpdatedBy,
                CarModelCreatedOn = s.tblCarModel.CreatedOn,
                CarModelUpdatedOn = s.tblCarModel.UpdatedOn,
                CarModelIsactive = s.tblCarModel.Isactive,
                CarModelIsarchive = s.tblCarModel.Isarchive,
                EngineType = s.tblCarModel.EngineType,

                /*---Showroom Car Features Details---*/
                tblCarFeatureID = s.tblCarModel.tblCarFeature.ID,
                FuelType = s.tblCarModel.tblCarFeature.FuelType,
                AirConditioned = s.tblCarModel.tblCarFeature.AirConditioned,
                ABS = s.tblCarModel.tblCarFeature.ABS,
                AirBag = s.tblCarModel.tblCarFeature.AirBag,
                PowerWindows = s.tblCarModel.tblCarFeature.PowerWindows,
                PowerMirrors = s.tblCarModel.tblCarFeature.PowerMirrors,
                PowerLocks = s.tblCarModel.tblCarFeature.PowerLocks,
                PowerSteering = s.tblCarModel.tblCarFeature.PowerSteering,
                ImmobilizerKey = s.tblCarModel.tblCarFeature.ImmobilizerKey,
                Radio = s.tblCarModel.tblCarFeature.Radio,
                KeyLessEntry = s.tblCarModel.tblCarFeature.KeyLessEntry,
                AlloyRims = s.tblCarModel.tblCarFeature.AlloyRims,
                CoolBox = s.tblCarModel.tblCarFeature.CoolBox,
                CruiseControl = s.tblCarModel.tblCarFeature.CruiseControl,
                SunRoof = s.tblCarModel.tblCarFeature.SunRoof,
                NavigationSystem = s.tblCarModel.tblCarFeature.NavigationSystem,
                CarFeatureCreatedBy = s.tblCarModel.tblCarFeature.CreatedBy,
                CarFeatureCreatedOn = s.tblCarModel.tblCarFeature.CreatedOn,
                CarFeatureUpdatedOn = s.tblCarModel.tblCarFeature.UpdatedOn,
                CarFeatureUpdatedBy = s.tblCarModel.tblCarFeature.UpdatedBy,
                CarFeatureIsactive = s.tblCarModel.tblCarFeature.Isactive,
                CarFeatureIsarchive = s.tblCarModel.tblCarFeature.Isarchive,
                RearAcVents = s.tblCarModel.tblCarFeature.RearAcVents,
                FrontCam = s.tblCarModel.tblCarFeature.FrontCam,
                CassetPlayer = s.tblCarModel.tblCarFeature.CassetPlayer,
                DvdPlayer = s.tblCarModel.tblCarFeature.DvdPlayer,
                SteeringSwitch = s.tblCarModel.tblCarFeature.SteeringSwitch,
                CdPlayer = s.tblCarModel.tblCarFeature.CdPlayer,
                ClimateControl = s.tblCarModel.tblCarFeature.ClimateControl,
                FrontSpeaker = s.tblCarModel.tblCarFeature.FrontSpeaker,
                HeatedSeat = s.tblCarModel.tblCarFeature.HeatedSeat,
                RearCamera = s.tblCarModel.tblCarFeature.RearCamera,
                RearSeatEntertain = s.tblCarModel.tblCarFeature.RearSeatEntertain,
                RearSpeaker = s.tblCarModel.tblCarFeature.RearSpeaker,
                BoxUsbAux = s.tblCarModel.tblCarFeature.BoxUsbAux,

                /*---Extra Things To Be Required---*/
                Latitude = ((s.tblShowroom.Latitude == null) ? "" : s.tblShowroom.Latitude.ToString()),
                Longitude = ((s.tblShowroom.Longitude == null) ? "" : s.tblShowroom.Longitude.ToString()),
                State = s.tblAddress.tblState.StateName,
                City = s.tblAddress.tblCity.CityName,
                Area = s.tblAddress.tblZone.ZoneName,
                CompleteAddress = ((s.tblAddress.CompleteAddress == null) ? "Not Available" : s.tblAddress.CompleteAddress),
                CarImage = s.tblCarImages.Select(a => a.Image).FirstOrDefault(),

                /*---Showroom Details---*/
                ShowroomImage = s.tblShowroom.Image,
                ShowroomName = s.tblShowroom.FullName,
                ShowroomEmail = s.tblShowroom.Email,
                ShowroomRole = s.tblShowroom.tblRole.Role,
                ShowroomNumber = s.tblShowroom.Contact,
                ShowroomCreatedOn = s.tblShowroom.CreatedOn,
                ShowroomURL = s.tblShowroom.ShowroomURL

            }).FirstOrDefault();

            if (user != null)
            {
                return user;
            }
            else
                return null;
        }

        public int InsertShowroomAds(tblCar model, string city)
        {
            try
            {
                if (model != null)
                {
                    model.CarsURL = ShowroomAdsURLGenerate(model.Title, model.tblCarModel.Year, city);
                    if (model.CarsURL != null)
                    {
                        model.Isactive = true;
                        model.Isarchive = false;
                        model.CreatedOn = DateTime.Now.ToString();
                        model.CreatedBy = model.tblShowroomID;
                        model.UpdatedOn = null;
                        model.UpdatedBy = null;
                        _context.tblCars.Add(model);
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

        public int InsertShowroomCarModels(tblCarModel model)
        {
            try
            {
                if (model != null)
                {
                    model.Isactive = true;
                    model.Isarchive = false;
                    model.CreatedOn = DateTime.Now.ToString();
                    model.CreatedBy = model.CreatedBy;
                    model.UpdatedOn = null;
                    model.UpdatedBy = 0;
                    _context.tblCarModels.Add(model);
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

        public int InsertShowroomCarFeatures(tblCarFeature model)
        {
            try
            {
                if (model != null)
                {
                    model.Isactive = true;
                    model.Isarchive = false;
                    model.CreatedBy = model.CreatedBy;
                    model.UpdatedBy = 0;
                    model.CreatedOn = DateTime.Now.ToString();
                    model.UpdatedOn = null;
                    _context.tblCarFeatures.Add(model);
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

        public int UpdateShowroomCarModels(tblCarModel model)
        {
            try
            {
                if (model != null)
                {
                    model.Isactive = true;
                    model.Isarchive = false;
                    model.CreatedOn = GetShowroomAdsDetail(model.ID).CarModelCreatedOn;
                    model.CreatedBy = GetShowroomAdsDetail(model.ID).CarModelCreatedBy;
                    model.UpdatedOn = DateTime.Now.ToString();
                    model.UpdatedBy = model.UpdatedBy;
                    _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
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

        public int UpdateShowroomCarFeatures(tblCarFeature model)
        {
            try
            {
                if (model != null)
                {
                    model.Isactive = true;
                    model.Isarchive = false;
                    model.CreatedOn = GetShowroomAdsDetail(model.ID).CarFeatureCreatedOn;
                    model.CreatedBy = GetShowroomAdsDetail(model.ID).CarFeatureCreatedBy;
                    model.UpdatedOn = DateTime.Now.ToString();
                    model.UpdatedBy = model.UpdatedBy;
                    _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
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

        public bool UpdateShowroomAds(tblCar model, string city)
        {
            try
            {
                if (model != null)
                {
                    model.CarsURL = ShowroomAdsURLGenerate(model.Title, model.tblCarModel.Year, city);
                    if (model.CarsURL != null)
                    {
                        model.Isactive = true;
                        model.Isarchive = false;
                        model.CreatedOn = GetShowroomAdsDetail(model.ID).tblCarCreatedOn;
                        model.CreatedBy = GetShowroomAdsDetail(model.ID).tblCarCreatedBy;
                        model.UpdatedOn = DateTime.Now.ToString();
                        model.UpdatedBy = model.UpdatedBy;
                        _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        Save();
                        return true;
                    }
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
                    var reas = _context.tblCars.Where(x => x.ID == AdID).FirstOrDefault();
                    reas.Isactive = false;
                    reas.Isarchive = true;
                    reas.CreatedOn = GetShowroomAdsDetail(reas.ID).tblCarCreatedOn;
                    reas.UpdatedBy = GetShowroomAdsDetail(reas.ID).tblCarUpdatedBy;
                    reas.UpdatedOn = DateTime.Now.ToString();
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

        public bool ReActiveShowroomAds(int AdID)
        {
            try
            {
                if (AdID > 0)
                {
                    var reas = _context.tblCars.Where(x => x.ID == AdID).FirstOrDefault();
                    reas.Isactive = true;
                    reas.Isarchive = false;
                    reas.CreatedOn = GetShowroomAdsDetail(reas.ID).tblCarCreatedOn;
                    reas.UpdatedBy = GetShowroomAdsDetail(reas.ID).tblCarUpdatedBy;
                    reas.UpdatedOn = DateTime.Now.ToString();
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

        public bool InsertShowroomAdsImages(tblCarImage model)
        {
            try
            {
                if (model != null)
                {
                    _context.tblCarImages.Add(model);
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

        public bool UpdateShowroomAdsImages(tblCarImage model)
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

        public string ShowroomAdsURLGenerate(string AdTitle, string Year, string CityLocation)
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
    }
}
