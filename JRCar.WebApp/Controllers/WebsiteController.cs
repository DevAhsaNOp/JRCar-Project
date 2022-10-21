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

        [Route("about-us")]
        public ActionResult AboutUs()
        {
            return View();
        }

        [Route("contact-us")]
        public ActionResult ContactUs()
        {
            return View();
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
                                /*Hard Coded Values Change It When The Scenario Changes*/
                                userAds.CategoryId = 1; /*Convert.ToInt32(userAds.CategoryName);*/
                                userAds.SubCategoryId = 1; /*Convert.ToInt32(userAds.SubCategoryName);*/
                                /*******************************************************/
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
                    /*Hard Coded Values Change It When The Scenario Changes*/
                    userAds.CategoryId = 1; /*userAds.CategoryId;*/
                    userAds.SubCategoryId = 1; /*userAds.SubCategoryId;*/
                    /*******************************************************/
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

        #region **User Car Shortlist**
        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "User")]
        [Route("carshortlist")]
        public ActionResult Shortlisted()
        {
            var UserID = Convert.ToInt32(Session["Id"]);
            if (UserID > 0)
            {
                int rows = 1;
                Session["Shortrows"] = rows;
                var reas = RepoObj1.AllCarShortlisted(UserID).Take(rows);
                return View(reas);
            }
            else
            {
                return Json("An error occured please try again later!", JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "User")]
        public ActionResult LoadShortlist()
        {
            var id = Convert.ToInt32(Session["Id"]);
            var rows = Convert.ToInt32(Session["Shortrows"]) + 1;
            var reas = RepoObj1.AllCarShortlisted(id).Take(rows);
            Session["Shortrows"] = rows;
            return PartialView("_LoadShortlist", reas);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "User")]
        [Route("carshortlist")]
        public ActionResult Shortlisted(int CarID)
        {
            var UserID = Convert.ToInt32(Session["Id"]);
            var reas = RepoObj1.CarShortlistedActive(CarID, UserID);
            if (reas)
            {
                return Json("Ad has been shortlisted!", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "User")]
        public ActionResult DelShortlisted(int CarID)
        {
            var UserID = Convert.ToInt32(Session["Id"]);
            var reas = RepoObj1.CarShortlistedInActive(CarID);
            if (reas)
            {
                var rows = Convert.ToInt32(Session["Shortrows"]);
                rows = (rows == 1) ? 1 : rows - 1;
                var data = RepoObj1.AllCarShortlisted(UserID).Take(rows);
                Session["Shortrows"] = rows;
                return PartialView("_LoadShortlist", data);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region **Car Dealer Profile**
        [AcceptVerbs(HttpVerbs.Get)]
        [Route("cardealer/{Show}")]
        public ActionResult ShowroomProfile(string Show)
        {
            if (Show != null)
            {
                var reas = adsRepo.ShowroomProfileView(Show);
                Session["CShowroomID"] = reas.tblShowroomID;
                if (reas == null && reas.ShowroomActive == false)
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

        [AcceptVerbs(HttpVerbs.Get)]
        [Route("cardealer")]
        public ActionResult ShowroomDeal()
        {
            int? page = 1;
            int pagesize = 3, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = RepoObj.GetAllShowRoom().Where(x => x.Isactive == true);
            IPagedList<tblShowroom> reas = list.ToPagedList(pageindex, pagesize);
            return View(reas);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Dealer(int? page, string searchedDealer)
        {
            IEnumerable<tblShowroom> list;
            int pagesize = 3, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            if (searchedDealer != null)
            {
                ViewBag.search = searchedDealer;
                list = RepoObj.GetAllShowRoom().Where(s => s.FullName.ToLower().Contains(searchedDealer.ToLower()) && s.Isactive == true).ToList();
            }
            else
            {
                list = RepoObj.GetAllShowRoom().Where(x => x.Isactive == true);
            }
            IPagedList<tblShowroom> reas = list.ToPagedList(pageindex, pagesize);
            return PartialView("_cardealers", reas);
        }

        [HttpPost]
        public JsonResult ShowroomContact(string FullName, string Email, string PhoneNumber, string Message)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    Email = Email.Length < 1 ? Session["Email"].ToString() : Email;
                    var UserID = Convert.ToInt32(Session["Id"]);
                    var ShowroomID = Convert.ToInt32(Session["CShowroomID"]);
                    AppointmentRepo appointmentRepo = new AppointmentRepo();
                    var reas = appointmentRepo.ShowroomContact(FullName, Email, PhoneNumber, Message, UserID, ShowroomID);
                    if (reas.Length > 1)
                    {
                        return Json(reas, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
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
                TempData["ErrorMsg"] = "Error occured on loading Appointments!" + ex.Message;
                throw ex;
            }
        }

        #endregion

        #region **User Appointment**

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "User")]
        [Route("Myappoinments")]
        public ActionResult UserAppointmentsList()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var UserID = Convert.ToInt32(Session["Id"]);
                    NotificationComponent NC = new NotificationComponent();
                    var list = NC.GetUserAppointmentsById(UserID);
                    return View(list);
                }
                else
                {
                    var err = (int)HttpStatusCode.BadRequest;
                    return Json(new { error = err + " Bad Request Error " + "Invalid Request!!" });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error occured on loading Appointments!" + ex.Message;
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult GetUserAppointmentsList()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var UserID = Convert.ToInt32(Session["Id"]);
                    var notificationRegisterTime = Session["AppLastUpdated"] != null ? Convert.ToDateTime(Session["AppLastUpdated"]) : DateTime.Now;
                    NotificationComponent NC = new NotificationComponent();
                    var list = NC.GetUserAppointments(notificationRegisterTime, UserID);
                    //Update session here for get only new added (Announcements)
                    Session["AppLastUpdated"] = DateTime.Now;
                    return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    //insert into tblNotification values('Hey','Blablablabalb',null,2042,108,1,0,getdate())
                    var err = (int)HttpStatusCode.BadRequest;
                    return Json(new { error = err + " Bad Request Error " + "Invalid Request!!" });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error occured on loading Appointments!" + ex.Message;
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult GetUserAppointmentById(int id)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    TempData["AppntID"] = id;
                    var AppntID = id;
                    NotificationComponent NC = new NotificationComponent();
                    var list = NC.GetUserCurrAppointmentById(AppntID);
                    return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    var err = (int)HttpStatusCode.BadRequest;
                    return Json(new { error = err + " Bad Request Error " + "Invalid Request!!" });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error occured on loading Appointments!" + ex.Message;
                throw ex;
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public JsonResult GetUserAppointmentsListCount()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var UserID = Convert.ToInt32(Session["Id"]);
                    var notificationRegisterTime = Session["AppLastUpdated"] != null ? Convert.ToDateTime(Session["AppLastUpdated"]) : DateTime.Now;
                    NotificationComponent NC = new NotificationComponent();
                    var list = NC.GetUserTodaysAppointmentsCount(notificationRegisterTime, UserID);
                    //Update session here for get only new added (Announcements)
                    Session["AppLastUpdated"] = DateTime.Now;
                    return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    var err = (int)HttpStatusCode.BadRequest;
                    return Json(new { error = err + " Bad Request Error " + "Invalid Request!!" });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error occured on updating Appointments Count!" + ex.Message;
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult ChangeUserAppointmentsToAsRead()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var UserID = Convert.ToInt32(Session["Id"]);
                    NotificationComponent NC = new NotificationComponent();
                    var list = NC.ChangeUserAppointmentToAsRead(UserID);
                    //Update session here for get only new added (Announcements)
                    Session["AppLastUpdated"] = DateTime.Now;
                    return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    var err = (int)HttpStatusCode.BadRequest;
                    return Json(new { error = err + " Bad Request Error " + "Invalid Request!!" });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error occured on updating Announcements As Read!" + ex.Message;
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult AcceptUserAppointment(string Purpose, string Date, bool IsAppntDel)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var AppntID = Convert.ToInt32(TempData["AppntID"]);
                    var Usr = Convert.ToInt32(Session["Id"]);
                    AppointmentRepo appointmentRepo = new AppointmentRepo();
                    var reas = appointmentRepo.AcceptUserAppointment(AppntID, Usr, Purpose, Date, IsAppntDel);
                    if (reas)
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
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
                TempData["ErrorMsg"] = "Error occured on loading Appointments!" + ex.Message;
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult UserAppointmentReject()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var AppntID = Convert.ToInt32(TempData["AppntID"]);
                    var Usr = Convert.ToInt32(Session["Id"]);
                    AppointmentRepo appointmentRepo = new AppointmentRepo();
                    var reas = appointmentRepo.RejectUserAppointment(AppntID, Usr);
                    if (reas)
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
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
                TempData["ErrorMsg"] = "Error occured on Rejecting Appointments!" + ex.Message;
                throw ex;
            }
        }

        #endregion

        #region **Car Ads Appointment**

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Showroom")]
        public ActionResult ScheduleAppointment(string useremail, string userphone, string selecteddatetime, string purpose)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (userphone != null && selecteddatetime != null && purpose != null)
                    {
                        if (purpose.Length > 1 && userphone.Length > 1 && selecteddatetime.Length > 1)
                        {
                            var email = (useremail.Length > 1) ? useremail : Session["Email"].ToString();
                            var ShowroomID = Convert.ToInt32(Session["Id"]);
                            var CarID = Convert.ToInt32(Session["UserCarID"]);
                            AppointmentRepo repo = new AppointmentRepo();

                            ValidateAppointment appointment = new ValidateAppointment()
                            {
                                Email = email,
                                ShowroomInterestedID = ShowroomID,
                                UserCarID = CarID,
                                Number = userphone,
                                Purpose = purpose,
                                Datetime = Convert.ToDateTime(selecteddatetime),
                                CreatedBy = ShowroomID,
                            };

                            var reas = repo.InsertAppointment(appointment);
                            if (reas)
                            {
                                ModelState.Clear();
                                return Json(true, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                ModelState.Clear();
                                return Json(false, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            ModelState.Clear();
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        ModelState.Clear();
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ModelState.Clear();
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region **Car Detail**
        [AcceptVerbs(HttpVerbs.Get)]
        [Route("Ad/{AdID}")]
        [Authorize(Roles = "User,Showroom")]
        public ActionResult CarDetail(string AdID)
        {
            var carDetail = RepoObj1.GetUserAdsDetail(AdID);
            int IsUserCar = 0;
            var UserID = Convert.ToInt32(Session["Id"]);
            if (carDetail != null)
            {
                Session["IsAppntShow"] = carDetail.UserID == UserID ? "true" : "false";
                IsUserCar = carDetail.UserID;
                if (((carDetail == null || UserID != IsUserCar) && (RepoObj.IsShowroom(UserID) == false)) || carDetail.Isactive == false)
                {
                    Session["UserCarID"] = null;
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
                    Session["UserCarID"] = carDetail.AdID;
                    ViewBag.Images = images;
                    if (User.Identity.IsAuthenticated)
                    {
                        AppointmentRepo repo = new AppointmentRepo();
                        var reas = repo.IsShowroomRequestThisCarAppointment((int)Session["Id"], carDetail.AdID);
                        if (reas)
                        {
                            Session["IsAppntSchedule"] = "true";
                        }
                        else
                        {
                            Session["IsAppntSchedule"] = "false";
                        }
                    }
                    return View(carDetail);
                }
            }
            else
            {
                Session["UserCarID"] = null;
                TempData["ErrorMsg"] = "Car you trying to view is not exists!";
                return RedirectToAction("AllVehicles");
            }
        }
        #endregion

        #region **User Car Removed & Mark As Sold**
        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "User")]
        public ActionResult CarRemoved(int AdID)
        {
            var carDetail = RepoObj1.InActiveUserAds(AdID);
            if (carDetail)
            {
                TempData["SuccessMsg"] = "Ad Removed Successfully!";
                var id = Convert.ToInt32(Session["Id"]);
                var rows = Convert.ToInt32(Session["rows"]);
                var reas = RepoObj1.GetAllUserAds(id).Take(rows);
                return PartialView("_LoadVehicle", reas);
            }
            else
            {
                TempData["ErrorMsg"] = "Something went wrong. Please try again!";
                var id = Convert.ToInt32(Session["Id"]);
                var rows = Convert.ToInt32(Session["rows"]);
                var reas = RepoObj1.GetAllUserAds(id).Take(rows);
                return PartialView("_LoadVehicle", reas);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "User")]
        public ActionResult UserCarSold(int AdID)
        {
            var carDetail = RepoObj1.MarkSoldUserAds(AdID);
            if (carDetail)
            {
                TempData["SuccessMsg"] = "Ad Mark As Sold and Deactivated Successfully!";
                var id = Convert.ToInt32(Session["Id"]);
                var rows = Convert.ToInt32(Session["rows"]);
                var reas = RepoObj1.GetAllUserAds(id).Take(rows);
                return PartialView("_LoadVehicle", reas);
            }
            else
            {
                TempData["ErrorMsg"] = "Something went wrong. Please try again!";
                var id = Convert.ToInt32(Session["Id"]);
                var rows = Convert.ToInt32(Session["rows"]);
                var reas = RepoObj1.GetAllUserAds(id).Take(rows);
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
            var Role = Session["Role"].ToString();
            var reas = RepoObj.GetUserDetailById(id, Role);
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
            int pagesize = 7, pageindex = 1;
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

            if (adsRepo.GetAllActiveAds().Count() <= 0)
            {
                ViewBag.MaximumPrice = 0;
                ViewBag.MinimumPrice = -1;

                ViewBag.SortBy = 1;
                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
                IPagedList<ValidateShowroomAds> reas1 = null;
                return View(reas1);
            }
            else
            {
                ViewBag.MaximumPrice = adsRepo.GetAllActiveAds().Select(x => Convert.ToInt32(x.Price)).Max();
                ViewBag.MinimumPrice = adsRepo.GetAllActiveAds().Select(x => Convert.ToInt32(x.Price)).Min();

                ViewBag.SortBy = (sortBy.HasValue ? sortBy.Value : 1);

                /***Number of Records you want per Page***/
                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
                var list = AdsRepo.GetAllActiveAdsFilter(searchTerm, minimumPrice, maximumPrice, sortBy, Condition, StartYear, EndYear, MakeId, ModelId, ColorSelected, TransSelected);
                IPagedList<ValidateShowroomAds> reas = list.ToPagedList(pageindex, pagesize);
                return View(reas);
            }
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
            /***Number of Records you want per Page***/
            int pagesize = 7, pageindex = 1;
            AdsViewModel adsView = new AdsViewModel();
            ShowroomAdsRepo AdsRepo = new ShowroomAdsRepo();
            adsView.SearchTerm = searchTerm;
            sortBy = sortBy.HasValue ? sortBy.Value : 1;
            adsView.MaximumPrice = Convert.ToInt32(AdsRepo.GetAllActiveAdsFilter(searchTerm, minimumPrice, maximumPrice, sortBy, Condition, StartYear, EndYear, MakeId, ModelId, ColorSelected, TransSelected).Max(x => x.Price));
            if (adsRepo.GetAllActiveAds().Count() <= 0)
            {
                ViewBag.MaximumPrice = 0;
                ViewBag.MinimumPrice = -1;

                ViewBag.SortBy = 1;
                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
                IPagedList<ValidateShowroomAds> reas1 = null;
                return PartialView("_LoadAdsOn", reas1);
            }
            else
            {
                ViewBag.SortBy = (sortBy.HasValue ? sortBy.Value : 1);
                ViewBag.MaximumPrice = (maximumPrice ?? AdsRepo.GetAllActiveAds().Select(x => Convert.ToInt32(x.Price)).Max());
                ViewBag.MinimumPrice = (minimumPrice ?? AdsRepo.GetAllActiveAds().Select(x => Convert.ToInt32(x.Price)).Min());

                var maxval = AdsRepo.GetAllActiveAds().Select(x => Convert.ToInt32(x.Price)).Max();
                var minval = AdsRepo.GetAllActiveAds().Select(x => Convert.ToInt32(x.Price)).Min();

                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
                var list = AdsRepo.GetAllActiveAdsFilter(searchTerm, minimumPrice, maximumPrice, sortBy, Condition, StartYear, EndYear, MakeId, ModelId, ColorSelected, TransSelected);
                IPagedList<ValidateShowroomAds> reas = list.ToPagedList(pageindex, pagesize);

                if (searchTerm != "" || (minimumPrice.Value >= minval && maximumPrice.Value < maxval) || (maximumPrice.Value <= maxval && minimumPrice.Value > minval) || sortBy.Value > 1 || Condition != null || (StartYear.Value >= 1972 && EndYear.Value < 2022) || (EndYear.Value <= 2022 && StartYear.Value > 1972) || MakeId != null || ModelId != null || ColorSelected != null || TransSelected != null)
                {
                    TempData["ListReas"] = 1;
                }

                return PartialView("_LoadAdsOn", reas);
            }
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