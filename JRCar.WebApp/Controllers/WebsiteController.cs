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

namespace JRCar.WebApp.Controllers
{
    public class WebsiteController : Controller
    {
        private UserRepo RepoObj;
        private UserAdsRepo RepoObj1;
        private AddressAutofillRepo AddressRepoObj;

        public WebsiteController()
        {
            RepoObj = new UserRepo();
            RepoObj1 = new UserAdsRepo();
            AddressRepoObj = new AddressAutofillRepo();
        }

        public ActionResult Index()
        {
            return View();
        }

        #region **Post New Vehicle**
        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "User")]
        [Route("Ads/PostNewVehicle")]
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
        [Route("Ads/PostNewVehicle")]
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
                        ViewBag.Error = "Error on uploading file!";
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

        #region **Car Detail**
        [AcceptVerbs(HttpVerbs.Get)]
        [Route("Ad/{AdID}")]
        public ActionResult CarDetail(string AdID)
        {
            var carDetail = RepoObj1.GetUserAdsDetail(AdID);
            if (carDetail == null)
            {
                TempData["ErrorMsg"] = "Car you trying to view is not exists!";
                return RedirectToAction("Myvehicles");
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
        public ActionResult AllVehicles(string searchTerm, int? minimumPrice, int? maximumPrice, int? sortBy, int? page, int? StateId, int?[] CityId, int?[] ZoneId)
        {
            ValidationUserAds adsView = new ValidationUserAds();
            var AllStates = AddressRepoObj.GetAllState();
            var states = new List<SelectListItem>();
            foreach (var item in AllStates)
            {
                states.Add(new SelectListItem() { Text = item.StateName, Value = item.StateId.ToString() });
            }

            ViewBag.State = AllStates;

            sortBy = sortBy.HasValue ? sortBy.Value : 1;
            ViewBag.MaximumPrice = RepoObj1.GetAllActiveAds().Select(x => Convert.ToInt32(x.Price)).Max();
            ViewBag.MinimumPrice = RepoObj1.GetAllActiveAds().Select(x => Convert.ToInt32(x.Price)).Min();

            ViewBag.SortBy = (sortBy.HasValue ? sortBy.Value : 1);

            /***Number of Records you want per Page***/
            int pagesize = 2, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = RepoObj1.GetAllActiveAdsFilter(searchTerm, minimumPrice, maximumPrice, sortBy, StateId, CityId, ZoneId);
            IPagedList<ValidationUserAds> reas = list.ToPagedList(pageindex, pagesize);
            return View(reas);
        }

        public ActionResult GetCityListCheckBox(int StateId)
        {
            var city = AddressRepoObj.GetCitiesByState(StateId);
            ViewBag.City = new SelectList(city, "CityId", "CityName");
            return PartialView("DisplayCityCheckBox");
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
        public ActionResult GetAds(string searchTerm, int? minimumPrice, int? maximumPrice, int? sortBy, int? page, int? StateId, int?[] CityId, int?[] ZoneId)
        {
            AdsViewModel adsView = new AdsViewModel();
            adsView.SearchTerm = searchTerm;
            sortBy = sortBy.HasValue ? sortBy.Value : 1;
            adsView.MaximumPrice = Convert.ToInt32(RepoObj1.GetAllActiveAdsFilter(searchTerm, minimumPrice, maximumPrice, sortBy, StateId, CityId, ZoneId).Max(x => x.Price));
            ViewBag.SortBy = (sortBy.HasValue ? sortBy.Value : 1);
            ViewBag.MaximumPrice = (maximumPrice.HasValue ? maximumPrice.Value : RepoObj1.GetAllActiveAds().Select(x => Convert.ToInt32(x.Price)).Max());
            ViewBag.MinimumPrice = (minimumPrice.HasValue ? minimumPrice.Value : RepoObj1.GetAllActiveAds().Select(x => Convert.ToInt32(x.Price)).Min());

            /***Number of Records you want per Page***/
            int pagesize = 2, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = RepoObj1.GetAllActiveAdsFilter(searchTerm, minimumPrice, maximumPrice, sortBy, StateId, CityId, ZoneId);
            IPagedList<ValidationUserAds> reas = list.ToPagedList(pageindex, pagesize);
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