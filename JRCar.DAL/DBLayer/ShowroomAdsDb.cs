﻿using JRCar.BOL;
using JRCar.BOL.Validation_Classes;
using JRCar.DAL.UserDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

        public IEnumerable<ValidateShowroomAds> GetAllActiveAdsFilter(string searchTerm, int? minimumPrice, int? maximumPrice, int? sortBy, int? Condition, int? StartYear, int? EndYear, int?[] MakeId, int?[] ModelId, string[] ColorSelected, string[] TransSelected)
        {
            var reas = _context.tblCars.Where(x => x.Isactive == true).Select(s => new ValidateShowroomAds()
            {
                tblCarID = s.ID,
                Title = s.Title,
                Price = s.Price,
                Year = s.tblCarModel.Year,
                Manufacturer_Name = s.tblManufacturer.Manufacturer_Name,
                Manufacturer_CarModelName = s.tblManfacturerCarModel.Manufacturer_CarModelName,
                ManufacturerId = s.tblManufacturer.Manufacturer_Id,
                ManufacturerCarModelID = s.tblManfacturerCarModel.ManufacturerCarModel_Id,
                Condition = s.Condition,
                tblCarCreatedOn = s.CreatedOn,
                CurrentLocation = s.CurrentLocation,
                tblAddress = s.tblAddress,
                CarsURL = s.CarsURL,
                CarImage = s.tblCarImages.Select(a => a.Image).FirstOrDefault(),
                Color = s.Color,
                Transmission = s.Transmission,
                tblFavAdds = s.tblFavAdds,
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
            if (Condition == 1)
            {
                reas = reas.Where(x => x.Condition.Contains("Used")).ToList();
            }
            if (Condition == 2)
            {
                reas = reas.Where(x => x.Condition.Contains("New")).ToList();
            }
            if (ColorSelected != null)
            {
                List<ValidateShowroomAds> AdsList = new List<ValidateShowroomAds>();
                foreach (var item in ColorSelected)
                {
                    var val = item;
                    var ads = reas.Where(x => x.Color.ToLower().Contains(item.ToLower())).ToList();
                    AdsList.AddRange(ads);
                }
                reas = AdsList;
            }
            if (TransSelected != null)
            {
                List<ValidateShowroomAds> AdsList = new List<ValidateShowroomAds>();
                foreach (var item in TransSelected)
                {
                    var val = item;
                    var ads = reas.Where(x => x.Transmission.ToLower().Contains(item.ToLower())).ToList();
                    AdsList.AddRange(ads);
                }
                reas = AdsList;
            }
            if (StartYear.HasValue && EndYear.HasValue)
            {
                reas = reas.Where(x => Convert.ToInt32(x.Year) >= StartYear && Convert.ToInt32(x.Year) <= EndYear).ToList();
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
            if (MakeId != null && ModelId == null)
            {
                List<ValidateShowroomAds> AdsList = new List<ValidateShowroomAds>();
                foreach (var item in MakeId)
                {
                    var val = item;
                    var ads = reas.Where(x => x.ManufacturerId == item).ToList();
                    AdsList.AddRange(ads);
                }
                reas = AdsList;
            }
            if (MakeId != null && ModelId != null)
            {
                List<ValidateShowroomAds> AdsList = new List<ValidateShowroomAds>();
                foreach (var makes in MakeId)
                {
                    var val = makes;
                    foreach (var models in ModelId)
                    {
                        var val1 = makes;
                        var ads = reas.Where(x => x.ManufacturerId == makes && x.ManufacturerCarModelID == models).ToList();
                        AdsList.AddRange(ads);
                    }
                }
                reas = AdsList;
            }
            return reas;
        }

        public IEnumerable<ValidateShowroomAds> GetAllAds()
        {
            var reas = _context.tblCars.OrderBy(Ad => Ad.CreatedOn).Select(s => new ValidateShowroomAds()
            {
                tblCarID = s.ID,
                Title = s.Title,
                Price = s.Price,
                Year = s.tblCarModel.Year,
                Manufacturer_Name = s.tblManufacturer.Manufacturer_Name,
                Manufacturer_CarModelName = s.tblManfacturerCarModel.Manufacturer_CarModelName,
                Condition = s.Condition,
                tblCarCreatedOn = s.CreatedOn,
                tblCarIsactive = s.Isactive,
                CurrentLocation = s.CurrentLocation,
                CarsURL = s.CarsURL,
                CarImage = s.tblCarImages.Select(a => a.Image).FirstOrDefault(),
                ShowroomName = s.tblShowroom.FullName
            }).ToList();
            return reas;
        }

        public bool IncreaseShowroomAdViewCount(int CarID)
        {
            if (CarID > 0)
            {
                var reas = _context.tblCars.Find(CarID);
                if (reas.ViewsCount == null)
                {
                    reas.ViewsCount = 1.ToString();
                }
                else
                {
                    var count = Convert.ToInt32(reas.ViewsCount) + 1;
                    reas.ViewsCount = count.ToString();
                }
                _context.Entry(reas).State = System.Data.Entity.EntityState.Modified;
                Save();
                return true;
            }
            else
                return false;
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

        public IEnumerable<ValidateShowroomAds> GetAllActiveAdsForTabs()
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
                ShowroomName = s.tblShowroom.FullName,
                ShowroomURL = s.tblShowroom.ShowroomURL
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

        public IEnumerable<ValidateShowroomAds> GetAllShowroomActiveAds(int ShowroomID)
        {
            return _context.tblCars.Where(x => x.Isactive == true && x.tblShowroomID == ShowroomID).Select(s => new ValidateShowroomAds()
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

        public IEnumerable<ValidateShowroomAds> GetAllShowroomActiveAdsFD(int ShowroomID)
        {
            return _context.tblCars.Where(x => x.Isactive == true && x.tblShowroomID == ShowroomID).Select(s => new ValidateShowroomAds()
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
            }).OrderByDescending(x => x.tblCarCreatedOn).ToList();
        }

        public int GetShowroomAdsViewCount(int ShowroomID)
        {
            if (ShowroomID > 0)
            {
                var Viewscount = 0;
                var reas = _context.tblCars.Where(x => x.tblShowroomID == ShowroomID).ToList();
                if (reas != null)
                {
                    foreach (var item in reas)
                    {
                        if (item.ViewsCount != null)
                        {
                            Viewscount += Convert.ToInt32(item.ViewsCount);
                        }
                        else
                        {
                            Viewscount += 0;
                        }
                    }
                }
                return Viewscount;
            }
            else
                return 0;
        }

        public IEnumerable<ValidateShowroomAds> GetAllShowroomInActiveAds(int ShowroomID)
        {
            return _context.tblCars.Where(x => x.Isactive == false && x.tblShowroomID == ShowroomID).Select(s => new ValidateShowroomAds()
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

        public IEnumerable<ValidateShowroom> GetAllShowroomAdsDetailsFD()
        {
            var query = _context.tblCars.AsEnumerable()
                               .GroupBy(p => p.Title)
                               .Select(g => new ValidateShowroom { FullName = g.Key, count = g.Sum(w => Int32.Parse(w.ViewsCount)) }).ToList();
            return query;
        }
        
        public IEnumerable<ValidateShowroom> GetAllUserAdsDetailsFD()
        {
            var query = _context.tblUserAdds.AsEnumerable()
                               .GroupBy(p => p.Title)
                               .Select(g => new ValidateShowroom { FullName = g.Key, count = g.Sum(w => Int32.Parse(w.ViewsCount)) }).ToList();
            return query;
        }

        public IEnumerable<ValidateShowroomAds> GetAllShowroomAds(int ShowroomAdID)
        {
            return _context.tblCars.Where(x => x.tblShowroomID == ShowroomAdID).Select(s => new ValidateShowroomAds()
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
                IsSold = (s.Issold == true) ? true : false,
            }).ToList();
        }

        public int GetShowroomID(int ShowroomAdID)
        {
            return _context.tblCars.Where(x => x.ID == ShowroomAdID).Select(s => s.tblShowroomID).FirstOrDefault();
        }

        public int GetShowroomFavAdsCount(int ShowroomID)
        {
            return _context.tblFavAdds.Where(x => x.tblCar.tblShowroomID == ShowroomID).Count();
        }

        public decimal GetShowroomTotalPaidAmnt(int ShowroomID)
        {
            var reas = _context.tblPayments.Where(x => x.ShowroomID == ShowroomID).ToList();
            decimal amount = 0;
            foreach (var item in reas)
            {
                if (item.Recieved != null)
                {
                    amount += item.Recieved.Value;
                }
                else
                {
                    amount += 0;
                }
            }
            return amount;
        }

        public IEnumerable<ValidateShowroomAds> GetAllShowroomAdsForReport(int ShowroomAdID)
        {
            return _context.tblCars.Where(x => x.tblShowroomID == ShowroomAdID).Select(s => new ValidateShowroomAds()
            {
                tblCarID = s.ID,
                Title = s.Title,
                Price = s.Price,
                Year = s.tblCarModel.Year,
                Manufacturer_Name = s.tblManufacturer.Manufacturer_Name,
                Manufacturer_CarModelName = s.tblManfacturerCarModel.Manufacturer_CarModelName,
                Condition = s.Condition,
                CarIsActive = ((s.Isactive == true) ? "Active" : "Inactive"),
                tblCarIsactive = s.Isactive,
                tblCarCreatedOn = s.CreatedOn,
                CurrentLocation = s.CurrentLocation,
                CarsURL = s.CarsURL,
                RegNo = s.RegNo,
                RegLocation = s.RegLocation,
                Color = s.Color,
                MaxSpeed = s.MaxSpeed,
                GearType = s.GearType,
                Description = s.Description,
                Transmission = s.Transmission,
                Mileage = s.Mileage,
                ShowroomName = s.tblShowroom.FullName,
                ShowroomImage = s.tblShowroom.Image,
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

        public ValidateShowroomAds ShowroomProfileView(string AdId)
        {
            if (AdId != null)
            {
                var reas = _context.tblShowrooms.Where(x => x.ShowroomURL.ToLower().Contains(AdId.ToLower())).Select(s => new ValidateShowroomAds()
                {
                    tblShowroomID = s.ID,
                    ShowroomName = s.FullName,
                    ShowroomEmail = s.Email,
                    ShowroomNumber = s.Contact,
                    CurrentLocation = s.ShopNumber,
                    ShowroomImage = s.Image,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    Description = s.Description,
                    tblAddress = s.tblAddress,
                    tblCars = s.tblCars,
                    ShowroomActive = s.Isactive
                }).FirstOrDefault();
                return reas;
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

        public bool IsCarShortlist(int CarID, int UserID)
        {
            var IsShortlist = _context.tblFavAdds.Where(x => x.CarID == CarID && x.UserID == UserID && x.Isactive == true).FirstOrDefault();
            if (IsShortlist != null)
                return true;
            else
                return false;
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

        public int InsertShowroomAds(tblCar model, string Year, string city)
        {
            try
            {
                if (model != null)
                {
                    model.CarsURL = ShowroomAdsURLGenerate(model.Title, Year, city);
                    if (model.CarsURL != null)
                    {
                        model.Isactive = true;
                        model.Isarchive = false;
                        model.Issold = false;
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
                    model.UpdatedOn = DateTime.Now.ToString();
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
                    model.UpdatedOn = DateTime.Now.ToString();
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
                    if (model.CarsURL.Length > 1)
                    {
                        model.Isactive = true;
                        model.Isarchive = false;
                        model.Issold = false;
                        model.CreatedOn = GetShowroomAdsDetail(model.ID).tblCarCreatedOn;
                        model.CreatedBy = GetShowroomAdsDetail(model.ID).tblCarCreatedBy;
                        model.UpdatedOn = DateTime.Now.ToString();
                        model.UpdatedBy = model.UpdatedBy;
                        _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        Save();
                        return true;
                    }
                    else if (model.CarsURL.Length < 1)
                    {
                        model.CarsURL = ShowroomAdsURLGenerate(model.Title, model.tblCarModel.Year, city);
                        model.Isactive = true;
                        model.Isarchive = false;
                        model.Issold = false;
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

        public bool MarkSoldShowroomAds(int AdID)
        {
            try
            {
                if (AdID > 0)
                {
                    var reas = _context.tblCars.Where(x => x.ID == AdID).FirstOrDefault();
                    reas.Isactive = false;
                    reas.Issold = true;
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
