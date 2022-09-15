using JRCar.BOL;
using JRCar.BLL.Repositories;
using JRCar.BOL.Validation_Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList.Mvc;
using PagedList;
using JRCar.WebApp.ViewModels;
using System.Security.Policy;
using System.Collections;
using System.Drawing;
using System.Xml.Linq;

namespace JRCar.WebApp.Controllers
{
    public class WebsiteController : Controller
    {
        private UserRepo RepoObj;
        private UserAdsRepo RepoObj1;
        private AddressAutofillRepo AddressRepoObj;
        private ShowroomAdsRepo adsRepo;

        public WebsiteController()
        {
            adsRepo = new ShowroomAdsRepo();
            RepoObj = new UserRepo();
            RepoObj1 = new UserAdsRepo();
            AddressRepoObj = new AddressAutofillRepo();
        }

        public ActionResult Index()
        {
            var reas = adsRepo.GetAllActiveAdsForTabs();
            return View(reas);
        }

        #region **Post & Edit Vehicle**
        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "User")]
        [Route("Ad/PostNewVehicle")]
        public ActionResult PostNewVehicles()
        {
            var AllStates = AddressRepoObj.GetAllState();
            var states = new List<SelectListItem>();
            foreach (var item in AllStates)
            {
                states.Add(new SelectListItem() { Text = item.StateName, Value = item.StateId.ToString() });
            }
            ViewBag.State = states;

            var year = new List<SelectListItem>();
            year.Add(new SelectListItem() { Text = "---Select Model Year---", Value = "0", Disabled = true, Selected = true });
            for (int i = -50; i <= 0; ++i)
            {
                year.Add(new SelectListItem() { Text = DateTime.Now.AddYears(i).ToString("yyyy"), Value = DateTime.Now.AddYears(i).ToString("yyyy") });
            }
            ViewBag.Years = year;

            var condition = new List<SelectListItem>();
            condition.Add(new SelectListItem() { Text = "---Select Condition---", Value = "0", Disabled = true, Selected = true });
            condition.Add(new SelectListItem() { Text = "Used", Value = "1" });
            condition.Add(new SelectListItem() { Text = "New", Value = "2" });
            ViewBag.Conditions = condition;

            var AllCategory = RepoObj1.GetAllCategory();
            var categories = new List<SelectListItem>();
            categories.Add(new SelectListItem() { Text = "---Select Category---", Value = "0", Disabled = true, Selected = true });
            foreach (var item in AllCategory)
            {
                categories.Add(new SelectListItem() { Text = item.CategoryName, Value = item.CategoryID.ToString() });
            }
            ViewBag.Category = categories;

            var AllMake = RepoObj1.GetAllMakes();
            var makes = new List<SelectListItem>();
            makes.Add(new SelectListItem() { Text = "---Select Model---", Value = "0", Disabled = true, Selected = true });
            foreach (var item in AllMake)
            {
                makes.Add(new SelectListItem() { Text = item.Manufacturer_Name, Value = item.Manufacturer_Id.ToString() });
            }
            ViewBag.Make = makes;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "User")]
        [Route("Ads/EditVehicle/{AdID}")]
        public ActionResult EditVehicle(int AdID)
        {
            var AllStates = AddressRepoObj.GetAllState();
            var states = new List<SelectListItem>();
            states.Add(new SelectListItem() { Text = "---Select State---", Value = "0", Disabled = true, Selected = true });
            foreach (var item in AllStates)
            {
                states.Add(new SelectListItem() { Text = item.StateName, Value = item.StateId.ToString() });
            }
            ViewBag.State = states;

            var year = new List<SelectListItem>();
            year.Add(new SelectListItem() { Text = "---Select Model Year---", Value = "0", Disabled = true, Selected = true });
            for (int i = -50; i <= 0; ++i)
            {
                year.Add(new SelectListItem() { Text = DateTime.Now.AddYears(i).ToString("yyyy"), Value = DateTime.Now.AddYears(i).ToString("yyyy") });
            }
            ViewBag.Years = year;

            var condition = new List<SelectListItem>();
            condition.Add(new SelectListItem() { Text = "---Select Condition---", Value = "0", Disabled = true, Selected = true });
            condition.Add(new SelectListItem() { Text = "Used", Value = "1" });
            condition.Add(new SelectListItem() { Text = "New", Value = "2" });
            ViewBag.Conditions = condition;

            var AllCategory = RepoObj1.GetAllCategory();
            var categories = new List<SelectListItem>();
            categories.Add(new SelectListItem() { Text = "---Select Category---", Value = "0", Disabled = true, Selected = true });
            foreach (var item in AllCategory)
            {
                categories.Add(new SelectListItem() { Text = item.CategoryName, Value = item.CategoryID.ToString() });
            }
            ViewBag.Category = categories;



            var AllMake = RepoObj1.GetAllMakes();
            var makes = new List<SelectListItem>();
            makes.Add(new SelectListItem() { Text = "---Select Make---", Value = "0", Disabled = true, Selected = true });
            foreach (var item in AllMake)
            {
                makes.Add(new SelectListItem() { Text = item.Manufacturer_Name, Value = item.Manufacturer_Id.ToString() });
            }
            ViewBag.Make = makes;

            var reas = RepoObj1.GetUserAdsDetailOnlyForUpdate(AdID);

            Session["AddressID"] = reas.AddressId;
            Session["AdID"] = reas.AdID;

            var AllSubCategory = RepoObj1.GetSubCategoriesByCategory(reas.CategoryId.Value);
            var Subcategories = new List<SelectListItem>();
            Subcategories.Add(new SelectListItem() { Text = "---Select SubCategory---", Value = "0", Disabled = true, Selected = true });
            foreach (var item in AllSubCategory)
            {
                Subcategories.Add(new SelectListItem() { Text = item.SubCategoryName, Value = item.SubCategoryId.ToString() });
            }
            ViewBag.SubCategory = Subcategories;

            var AllCarModels = RepoObj1.GetModelsByMake(reas.ManufacturerId.Value);
            var Carmodels = new List<SelectListItem>();
            Carmodels.Add(new SelectListItem() { Text = "---Select Car Model---", Value = "0", Disabled = true });
            foreach (var item in AllCarModels)
            {
                if (item.ManufacturerCarModel_Id == reas.ManufacturerCarModelID)
                    Carmodels.Add(new SelectListItem() { Text = item.Manufacturer_CarModelName, Value = item.ManufacturerCarModel_Id.ToString(), Selected = true });
                else
                    Carmodels.Add(new SelectListItem() { Text = item.Manufacturer_CarModelName, Value = item.ManufacturerCarModel_Id.ToString() });
            }
            ViewBag.CarModel = Carmodels;

            var AllCities = AddressRepoObj.GetCitiesByState(reas.StateID);
            var ACities = new List<SelectListItem>();
            ACities.Add(new SelectListItem() { Text = "---Select City---", Value = "0", Disabled = true });
            foreach (var item in AllCities)
            {
                if (item.CityId == reas.tblAddress.City)
                    ACities.Add(new SelectListItem() { Text = item.CityName, Value = item.CityId.ToString(), Selected = true });
                else
                    ACities.Add(new SelectListItem() { Text = item.CityName, Value = item.CityId.ToString() });
            }
            ViewBag.ACities = ACities;

            var AllZones = AddressRepoObj.GetZoneByCity(reas.CityID);
            var AZones = new List<SelectListItem>();
            AZones.Add(new SelectListItem() { Text = "---Select Area---", Value = "0", Disabled = true });
            foreach (var item in AllZones)
            {
                if (item.ZoneId == reas.tblAddress.Area)
                    AZones.Add(new SelectListItem() { Text = item.ZoneName, Value = item.ZoneId.ToString(), Selected = true });
                else
                    AZones.Add(new SelectListItem() { Text = item.ZoneName, Value = item.ZoneId.ToString() });
            }
            ViewBag.AZones = AZones;

            string path = Server.MapPath("" + reas.CarImage + "");
            string[] FolderName = reas.CarImage.Split('/');
            string[] imageFiles = Directory.GetFiles(path);
            List<string> images = new List<string>();
            foreach (var item in imageFiles)
            {
                images.Add(FolderName[2] + "/" + Path.GetFileName(item));
            }
            ViewBag.Images = images;

            return View(reas);
        }

        public ActionResult GetCityList(int StateId)
        {
            var city = AddressRepoObj.GetCitiesByState(StateId);
            ViewBag.City = new SelectList(city, "CityId", "CityName");
            return PartialView("DisplayCity");
        }

        public ActionResult GetZoneList(int CityId)
        {
            var zone = AddressRepoObj.GetZoneByCity(CityId);
            ViewBag.Zone = new SelectList(zone, "ZoneId", "ZoneName");
            return PartialView("DisplayZone");
        }

        public ActionResult GetModelsList(int MakeId)
        {
            var carModels = RepoObj1.GetModelsByMake(MakeId);
            ViewBag.carModels = new SelectList(carModels, "ManufacturerCarModel_Id", "Manufacturer_CarModelName");
            return PartialView("DisplayModels");
        }

        public ActionResult GetSubCategoryList(int CategoryId)
        {
            var subCategories = RepoObj1.GetSubCategoriesByCategory(CategoryId);
            ViewBag.subCategories = new SelectList(subCategories, "SubCategoryId", "SubCategoryName");
            return PartialView("DisplaySubCategory");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "User")]
        [Route("Ad/PostNewVehicle")]
        public ActionResult PostNewVehicles(ImageFile objImage, ValidationUserAds userAds)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var name = string.Format("~/uploads/{0}{1}", RandomString(), DateTime.Now.Millisecond);
                    string createFolder = Server.MapPath(name);
                    if (!Directory.Exists(createFolder))
                    {
                        Directory.CreateDirectory(createFolder);
                        foreach (var file in objImage.files)
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                file.SaveAs(Path.Combine(Server.MapPath(name), Guid.NewGuid() + Path.GetExtension(file.FileName)));
                            }
                        }
                        if (userAds != null)
                        {
                            if (userAds.State != null && userAds.City != null && userAds.Area != null)
                            {
                                userAds.Condition = (userAds.Condition == "1") ? "Used" : "New";
                                var area = AddressRepoObj.GetZoneLatLong(Convert.ToInt32(userAds.Area));
                                userAds.UserID = Convert.ToInt32(Session["Id"]);
                                userAds.CarImage = name;
                                userAds.State = userAds.State;
                                userAds.City = userAds.City;
                                userAds.Area = userAds.Area;
                                userAds.CompleteAddress = userAds.Address;
                                userAds.Latitude = area.Item1.ToString();
                                userAds.Longitude = area.Item2.ToString();
                                userAds.CategoryId = Convert.ToInt32(userAds.CategoryName);
                                userAds.SubCategoryId = Convert.ToInt32(userAds.SubCategoryName);
                                userAds.ManufacturerId = Convert.ToInt32(userAds.Manufacturer_Name);
                                userAds.ManufacturerCarModelID = Convert.ToInt32(userAds.Manufacturer_CarModelName);
                                var AdsPublish = RepoObj1.InsertUserAds(userAds);
                                if (AdsPublish)
                                {
                                    TempData["SuccessMsg"] = "Ad Publish Successfully!";
                                    return RedirectToAction("PostNewVehicles");
                                }
                                else
                                {
                                    TempData["ErrorMsg"] = "Error on Ads Publishing please try again!";
                                    return RedirectToAction("PostNewVehicles");
                                }
                            }
                        }
                        else
                        {
                            ViewBag.Error = "Error please try again!";
                            return View("PostNewVehicles");
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Error on uploading Image!";
                        return View("PostNewVehicles");
                    }
                }
                else
                {
                    var err = (int)HttpStatusCode.BadRequest;
                    return Json(new { error = err + " Bad Request Error " + "Invalid Request!!" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("PostNewVehicles");
        }

        [HttpPost]
        public ActionResult SetSession(string[] RemovedImages)
        {
            Session["DeletedFiles"] = RemovedImages;
            return Json("Request!!", JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Route("EditVehicle")]
        public ActionResult EditVehicle(ImageFile objImage, ValidationUserAds userAds)
        {
            try
            {
                string[] FolderName = null;
                string[] arr = null;
                var name = string.Empty;
                var createFolder = string.Empty;
                var valerr = Session["DeletedFiles"];
                if (valerr != null)
                {
                    arr = ((IEnumerable)valerr).Cast<object>()
                             .Select(x => x.ToString())
                             .ToArray();
                }

                if (User.Identity.IsAuthenticated)
                {
                    if (arr != null && valerr != null)
                    {
                        FolderName = arr[0].Split('/');
                        foreach (var item in arr)
                        {
                            string path = Server.MapPath("~" + item);
                            FileInfo Afile = new FileInfo(path);
                            if (Afile.Exists)//check Afile exsit or not  
                            {
                                Afile.Delete();
                            }
                        }
                        name = string.Format("~/uploads/{0}", FolderName[2]);
                        createFolder = Server.MapPath(name);
                    }
                    if (Directory.Exists(createFolder))
                    {
                        foreach (var file in objImage.files)
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                file.SaveAs(Path.Combine(Server.MapPath(name), Guid.NewGuid() + Path.GetExtension(file.FileName)));
                            }
                        }
                        var reas = UpdateAd(userAds);
                        if (reas)
                        {
                            return RedirectToRoute("EditVehicle", new { userAds.AdID });
                        }
                        else
                        {
                            return RedirectToRoute("EditVehicle", new { userAds.AdID });
                        }
                    }
                    else
                    {
                        var reas = UpdateAd(userAds);
                        if (reas)
                        {
                            return RedirectToRoute("EditVehicle", new { userAds.AdID });
                        }
                        else
                        {
                            return RedirectToRoute("EditVehicle", new { userAds.AdID });
                        }
                    }
                }
                else
                {
                    var err = (int)HttpStatusCode.BadRequest;
                    return Json(new { error = err + " Bad Request Error " + "Invalid Request!!" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateAd(ValidationUserAds userAds)
        {
            if (userAds != null)
            {
                if (userAds.StateID > 0 && userAds.CityID > 0 && userAds.AreaID > 0)
                {
                    userAds.Condition = (userAds.Condition == "1") ? "Used" : "New";
                    var area = AddressRepoObj.GetZoneLatLong(userAds.AreaID);
                    userAds.UserID = Convert.ToInt32(Session["Id"]);
                    userAds.AddressId = Convert.ToInt32(Session["AddressID"]);
                    userAds.AdID = Convert.ToInt32(Session["AdID"]);
                    userAds.StateID = userAds.StateID;
                    userAds.CityID = userAds.CityID;
                    userAds.AreaID = userAds.AreaID;
                    userAds.CompleteAddress = userAds.CompleteAddress;
                    userAds.Latitude = area.Item1.ToString();
                    userAds.Longitude = area.Item2.ToString();
                    userAds.CategoryId = userAds.CategoryId;
                    userAds.SubCategoryId = userAds.SubCategoryId;
                    userAds.ManufacturerId = userAds.ManufacturerId;
                    userAds.ManufacturerCarModelID = userAds.ManufacturerCarModelID;
                    var AdsPublish = RepoObj1.UpdateUserAds(userAds);
                    if (AdsPublish)
                    {
                        TempData["SuccessMsg"] = "Ad Update Successfully!";
                        return true;
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Error on Ads Updating please try again!";
                        return false;
                    }
                }
            }
            else
            {
                ViewBag.Error = "Error please try again!";
                return false;
            }
            return false;
        }

        #endregion

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Shortlisted()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Shortlisted(ValidationUserAds userAds)
        {

            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShowroomProfile(string AdId)
        {
            if (AdId != null)
            {
                var reas = adsRepo.ShowroomProfileView(AdId);
                if (reas == null)
                {
                    TempData["ErrorMsg"] = "Showroom you trying to view is not exists!";
                    return RedirectToAction("AllVehicles");

                }
                else
                {
                    return View(reas);
                }
            }
            else
            {
                return View();
            }
        }

        #region **Car Detail**
        [AcceptVerbs(HttpVerbs.Get)]
        [Route("Ad/{AdID}")]
        public ActionResult CarDetail(string AdID)
        {
            var carDetail = RepoObj1.GetUserAdsDetail(AdID);
            if (carDetail == null)
            {
                TempData["ErrorMsg"] = "Car you trying to view is not exists!";
                return RedirectToAction("AllVehicles");
            }
            else
            {
                string path = Server.MapPath("" + carDetail.CarImage + "");
                string[] FolderName = carDetail.CarImage.Split('/');
                string[] imageFiles = Directory.GetFiles(path);
                List<string> images = new List<string>();
                foreach (var item in imageFiles)
                {
                    images.Add(FolderName[2] + "/" + Path.GetFileName(item));
                }
                ViewBag.Images = images;
                return View(carDetail);
            }
        }
        #endregion

        #region **User Car Removed**
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CarRemoved(int AdID)
        {
            var carDetail = RepoObj1.InActiveUserAds(AdID);
            if (carDetail)
            {
                TempData["SuccessMsg"] = "Ad Removed Successfully!";
                var id = Convert.ToInt32(Session["Id"]);
                var reas = RepoObj1.GetAllUserAds(id);
                return PartialView("_LoadVehicle", reas);
            }
            else
            {
                TempData["ErrorMsg"] = "Something went wrong. Please try again!";
                var id = Convert.ToInt32(Session["Id"]);
                var reas = RepoObj1.GetAllUserAds(id);
                return PartialView("_LoadVehicle", reas);
            }
        }
        #endregion

        #region **User Vehicle**
        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "User")]
        [Route("Myvehicles")]
        public ActionResult Myvehicles()
        {
            var id = Convert.ToInt32(Session["Id"]);
            int rows = 2;
            Session["rows"] = rows;
            var reas = RepoObj1.GetAllUserAds(id).Take(rows);
            return View(reas);
        }

        [Authorize(Roles = "User")]
        public ActionResult LoadVehicle()
        {
            var id = Convert.ToInt32(Session["Id"]);
            var rows = Convert.ToInt32(Session["rows"]) + 2;
            var reas = RepoObj1.GetAllUserAds(id).Take(rows);
            Session["rows"] = rows;
            return PartialView("_LoadVehicle", reas);
        }
        #endregion

        #region **User Profile Setting**
        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "User")]
        public ActionResult ProfileSettings()
        {
            var id = Convert.ToInt32(Session["Id"]);
            var reas = RepoObj.GetUserDetailById(id);
            return View(reas);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ProfileSettings(ImageFile objImage, ValidationUserAds userAds)
        {
            return View();
        }
        #endregion

        #region **Search Page**
        [AcceptVerbs(HttpVerbs.Get)]
        [Route("Ads")]
        public ActionResult AllVehicles(string searchTerm, int? minimumPrice, int? maximumPrice, int? sortBy, int? Condition, int? StartYear, int? EndYear, int? page, int?[] MakeId, int?[] ModelId, string[] ColorSelected, string[] TransSelected)
        {
            //ValidationUserAds adsView = new ValidationUserAds();
            //var AllStates = AddressRepoObj.GetAllState();
            //var states = new List<SelectListItem>();
            //foreach (var item in AllStates)
            //{
            //    states.Add(new SelectListItem() { Text = item.StateName, Value = item.StateId.ToString() });
            //}

            //ViewBag.State = AllStates;

            var year = new List<string>();
            for (int i = -50; i <= 0; ++i)
            {
                year.Add(DateTime.Now.AddYears(i).ToString("yyyy"));
            }
            ViewBag.CARYears = year;

            var Color = new List<SelectListItem>
            {
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
                new SelectListItem() { Text = "Unlisted", Value = "Unlisted" }
            };
            ViewBag.CarColors = Color;

            var Transm = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Automatic", Value = "Automatic" },
                new SelectListItem() { Text = "Manual", Value = "Manual" },
                new SelectListItem() { Text = "CVT Transmission", Value = "CVT Transmission" },
                new SelectListItem() { Text = "Semi Automatic", Value = "Semi Automatic" },
                new SelectListItem() { Text = "Dual Clutch", Value = "Dual Clutch" }
            };
            ViewBag.Trans = Transm;


            ShowroomAdsRepo AdsRepo = new ShowroomAdsRepo();
            var AllMakes = AdsRepo.GetAllMakes(); AddressRepoObj.GetAllState();
            var makes = new List<SelectListItem>();
            foreach (var item in AllMakes)
            {
                makes.Add(new SelectListItem() { Text = item.Manufacturer_Name, Value = item.Manufacturer_Id.ToString() });
            }

            ViewBag.Make = AllMakes;

            sortBy = sortBy.HasValue ? sortBy.Value : 1;
            ViewBag.MaximumPrice = RepoObj1.GetAllActiveAds().Select(x => Convert.ToInt32(x.Price)).Max();
            ViewBag.MinimumPrice = RepoObj1.GetAllActiveAds().Select(x => Convert.ToInt32(x.Price)).Min();

            ViewBag.SortBy = (sortBy.HasValue ? sortBy.Value : 1);

            /***Number of Records you want per Page***/
            int pagesize = 2, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = AdsRepo.GetAllActiveAdsFilter(searchTerm, minimumPrice, maximumPrice, sortBy, Condition, StartYear, EndYear, MakeId, ModelId, ColorSelected, TransSelected);
            IPagedList<ValidateShowroomAds> reas = list.ToPagedList(pageindex, pagesize);
            return View(reas);
        }

        public ActionResult GetCityListCheckBox(int StateId)
        {
            var city = AddressRepoObj.GetCitiesByState(StateId);
            ViewBag.City = new SelectList(city, "CityId", "CityName");
            return PartialView("DisplayCityCheckBox");
        }

        public ActionResult GetModelListCheckBox(int MakeId)
        {
            var carModels = RepoObj1.GetModelsByMake(MakeId);
            ViewBag.carModelsChk = new SelectList(carModels, "ManufacturerCarModel_Id", "Manufacturer_CarModelName");
            return PartialView("DisplayModelsCheckBox");
        }

        public ActionResult GetZoneListCheckBox(int[] CityId)
        {
            var list = new List<Data>();
            var zoneList = new List<Data>();
            foreach (var item in CityId)
            {
                var val = item;
                list = AddressRepoObj.GetZoneByCity(item).Select(x => new Data()
                {
                    ZoneId = x.ZoneId,
                    ZoneName = x.ZoneName
                }).ToList();
                zoneList.AddRange(list);
            }
            ViewBag.Zone = new SelectList(zoneList, "ZoneId", "ZoneName");
            return PartialView("DisplayZoneCheckBox");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetAds(string searchTerm, int? minimumPrice, int? maximumPrice, int? sortBy, int? Condition, int? StartYear, int? EndYear, int? page, int?[] MakeId, int?[] ModelId, string[] ColorSelected, string[] TransSelected)
        {
            AdsViewModel adsView = new AdsViewModel();
            ShowroomAdsRepo AdsRepo = new ShowroomAdsRepo();
            adsView.SearchTerm = searchTerm;
            sortBy = sortBy.HasValue ? sortBy.Value : 1;
            adsView.MaximumPrice = Convert.ToInt32(AdsRepo.GetAllActiveAdsFilter(searchTerm, minimumPrice, maximumPrice, sortBy, Condition, StartYear, EndYear, MakeId, ModelId, ColorSelected, TransSelected).Max(x => x.Price));
            ViewBag.SortBy = (sortBy.HasValue ? sortBy.Value : 1);
            ViewBag.MaximumPrice = (maximumPrice.HasValue ? maximumPrice.Value : AdsRepo.GetAllActiveAds().Select(x => Convert.ToInt32(x.Price)).Max());
            ViewBag.MinimumPrice = (minimumPrice.HasValue ? minimumPrice.Value : AdsRepo.GetAllActiveAds().Select(x => Convert.ToInt32(x.Price)).Min());

            /***Number of Records you want per Page***/
            int pagesize = 2, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = AdsRepo.GetAllActiveAdsFilter(searchTerm, minimumPrice, maximumPrice, sortBy, Condition, StartYear, EndYear, MakeId, ModelId, ColorSelected, TransSelected);
            IPagedList<ValidateShowroomAds> reas = list.ToPagedList(pageindex, pagesize);
            return PartialView("_LoadAdsOn", reas);
        }
        #endregion

        #region **Usabale Functions & Classes**
        private static Random random = new Random();

        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 12)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public class ImageFile
        {
            public List<HttpPostedFileBase> files { get; set; }
        }

        public class Data
        {
            public int ZoneId { get; set; }
            public string ZoneName { get; set; }
        }
        #endregion

    }
}