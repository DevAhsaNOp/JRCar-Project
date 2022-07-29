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
            ViewBag.State = new SelectList(AddressRepoObj.GetAllState(), "StateId", "StateName");
            List<string> years = new List<string>();
            for (int i = -50; i <= 0; ++i)
            {
                years.Add(DateTime.Now.AddYears(i).ToString("yyyy"));
            }
            ViewBag.Years = years;
            List<string> consition = new List<string>() { "Used", "New" };
            ViewBag.Conditions = consition;
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
                    userAds.UserID = Convert.ToInt32(Session["Id"]);
                    userAds.CarImage = name;
                    if (userAds != null)
                    {
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
                    ViewBag.Error = "Error on uploading file!";
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