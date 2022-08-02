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

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "User")]
        public ActionResult PostNewVehicles()
        {
            var AllStates = AddressRepoObj.GetAllState();
            var states = new List<SelectListItem>();
            //states.Add(new SelectListItem() { Text = "---Select State---", Value = "0", Disabled = true, Selected = true });
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PostNewVehicles(ImageFile objImage, ValidationUserAds userAds)
        {
            try
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
                            var reas = AddressRepoObj.GetStateandCity(Convert.ToInt32(userAds.City));
                            var area = AddressRepoObj.GetZoneLatLong(Convert.ToInt32(userAds.Area));
                            userAds.UserID = Convert.ToInt32(Session["Id"]);
                            userAds.CarImage = name;
                            userAds.State = reas.Item1;
                            userAds.City = reas.Item2;
                            userAds.Area = area.Item3;
                            userAds.CompleteAddress = userAds.Address;
                            userAds.Latitude = area.Item1.ToString();
                            userAds.Longitude = area.Item2.ToString();
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
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("PostNewVehicles");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Shortlisted()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Shortlisted(ImageFile objImage, ValidationUserAds userAds)
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "User")]
        public ActionResult CarDetail(int AdID)
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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Myvehicles()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Myvehicles(ImageFile objImage, ValidationUserAds userAds)
        {
            return View();
        }

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

    }
}