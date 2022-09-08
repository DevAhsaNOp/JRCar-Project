using JRCar.BOL;
using JRCar.BLL.Repositories;
using JRCar.BOL.Validation_Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static JRCar.WebApp.Controllers.WebsiteController;
using System.Collections;
using System.Linq;
using JRCar.DAL.DBLayer;
using System.Xml.Linq;

namespace JRCar.WebApp.Controllers
{
    public class PortalController : Controller
    {
        private UserRepo RepoObj;
        private ShowroomAdsRepo RepoObj1;
        private AddressAutofillRepo AddressRepoObj;

        public PortalController()
        {
            RepoObj = new UserRepo();
            RepoObj1 = new ShowroomAdsRepo();
            AddressRepoObj = new AddressAutofillRepo();
        }

        public ActionResult Index()
        {
            return View();
        }

        #region **Any Profile Update**
        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "Admin,Showroom,Union")]
        public ActionResult UpdateProfile()
        {
            var id = Convert.ToInt32(Session["Id"]);
            var reas = RepoObj.GetUserDetailById(id);
            return View(reas);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateProfile(HttpPostedFileBase file, ValidateUser user)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (file != null)
                    {
                        string filename = Path.GetFileName(file.FileName);
                        string _filename = DateTime.Now.ToString("yymmssfff") + filename;
                        string extension = Path.GetExtension(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/Images/"), _filename);
                        user.Image = "~/Images/" + _filename;
                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                        {
                            var role = Session["Role"].ToString();
                            user.tblRoleName = role;
                            if (file.ContentLength <= 10000000)
                            {
                                user.ID = (int)Session["Id"];
                                user.UpdatedBy = (int)Session["Id"];
                                var IsUpdated = RepoObj.UpdateUser(user);
                                string oldImgPath = Request.MapPath(Session["Image"].ToString());
                                if (IsUpdated)
                                {
                                    file.SaveAs(path);
                                    if (System.IO.File.Exists(oldImgPath))
                                    {
                                        System.IO.File.Delete(oldImgPath);
                                        TempData["SuccessMsg"] = "Account Updated Successfully!";
                                        Session["Image"] = user.Image;
                                        if (role == "Admin" || role == "Union" || role == "Showroom")
                                        {
                                            return View("UpdateProfile");
                                        }
                                        else if (role == "User")
                                        {
                                            return RedirectToAction("ProfileSettings", "Website");
                                        }
                                        else
                                        {
                                            TempData["ErrorMsg"] = "Error occured on login Account!";
                                            return RedirectToAction("SignIn");
                                        }
                                    }
                                }
                                else
                                {
                                    if (role == "Admin" || role == "Union" || role == "Showroom")
                                    {
                                        TempData["ErrorMsg"] = "Error occured on login Account!";
                                        return View("UpdateProfile");
                                    }
                                    else if (role == "User")
                                    {
                                        TempData["ErrorMsg"] = "Error occured on login Account!";
                                        return RedirectToAction("ProfileSettings", "Website");
                                    }
                                    else
                                    {
                                        TempData["ErrorMsg"] = "Error occured on login Account!";
                                        return RedirectToAction("SignIn");
                                    }
                                }
                            }
                            else
                            {
                                if (role == "Admin" || role == "Union" || role == "Showroom")
                                {
                                    TempData["ErrorMsg"] = "Image size is very large";
                                    return View("UpdateProfile");
                                }
                                else if (role == "User")
                                {
                                    TempData["ErrorMsg"] = "Image size is very large";
                                    return RedirectToAction("ProfileSettings", "Website");
                                }
                                else
                                {
                                    TempData["ErrorMsg"] = "Error occured on login Account!";
                                    return RedirectToAction("SignIn");
                                }
                            }
                        }
                    }
                    else
                    {
                        user.Image = Session["Image"].ToString();
                        user.ID = (int)Session["Id"];
                        var role = Session["Role"].ToString();
                        user.tblRoleName = role;
                        user.UpdatedBy = (int)Session["Id"];
                        var IsUpdated = RepoObj.UpdateUser(user);
                        if (IsUpdated)
                        {
                            TempData["SuccessMsg"] = "Account Updated Successfully!";
                            FormsAuthentication.SetAuthCookie(user.Email, false);
                            if (role == "Admin" || role == "Union" || role == "Showroom")
                            {
                                return View("UpdateProfile");
                            }
                            else if (role == "User")
                            {
                                return RedirectToAction("ProfileSettings", "Website");
                            }
                            else
                            {
                                TempData["ErrorMsg"] = "Error occured on login Account!";
                                return RedirectToAction("SignIn");
                            }
                        }
                        else
                        {
                            if (role == "Admin" || role == "Union" || role == "Showroom")
                            {
                                TempData["ErrorMsg"] = "Error occured on login Account!";
                                return View("UpdateProfile");
                            }
                            else if (role == "User")
                            {
                                TempData["ErrorMsg"] = "Error occured on login Account!";
                                return RedirectToAction("ProfileSettings", "Website");
                            }
                            else
                            {
                                TempData["ErrorMsg"] = "Error occured on login Account!";
                                return RedirectToAction("SignIn");
                            }
                        }
                    }
                    return View();
                }
                else
                {
                    var err = (int)HttpStatusCode.BadRequest;
                    return Json(new { error = err + " Bad Request Error " + "Invalid Request!!" });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error occured on updating Account!" + ex.Message;
                return View("UpdateProfile");
            }
        }
        #endregion

        #region **Notification For Showroom About New Ads**
        [HttpGet]
        public JsonResult GetNotificationsList()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var ShowroomID = Convert.ToInt32(Session["Id"]);
                    var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
                    NotificationComponent NC = new NotificationComponent();
                    var list = NC.GetNotifications(notificationRegisterTime, ShowroomID);
                    //Update session here for get only new added (Notifications)
                    Session["LastUpdate"] = DateTime.Now;
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
                TempData["ErrorMsg"] = "Error occured on updating Notification!" + ex.Message;
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult GetNotificationsListCount()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var ShowroomID = Convert.ToInt32(Session["Id"]);
                    var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
                    NotificationComponent NC = new NotificationComponent();
                    var list = NC.GetNotificationsCount(notificationRegisterTime, ShowroomID);
                    //Update session here for get only new added (Notifications)
                    Session["LastUpdate"] = DateTime.Now;
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
                TempData["ErrorMsg"] = "Error occured on updating Notification Count!" + ex.Message;
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult ChangeNotificationToAsRead()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var ShowroomID = Convert.ToInt32(Session["Id"]);
                    NotificationComponent NC = new NotificationComponent();
                    var list = NC.ChangeNotificationToAsRead(ShowroomID);
                    //Update session here for get only new added (Notifications)
                    Session["LastUpdate"] = DateTime.Now;
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
                TempData["ErrorMsg"] = "Error occured on updating Notification As Read!" + ex.Message;
                throw ex;
            }
        }

        #endregion

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "Showroom")]
        [Route("Showroom/PostNewAd")]
        public ActionResult PostNewAd()
        {
            ViewBag.Years = RepoObj1.ListOfYears();

            ViewBag.Conditions = RepoObj1.Conditions();

            ViewBag.Transmissions = RepoObj1.Transmissions();

            ViewBag.Assemblys = RepoObj1.Assemblys();

            ViewBag.Colors = RepoObj1.Colors();

            ViewBag.BodyTypes = RepoObj1.BodyTypes();

            ViewBag.Category = RepoObj1.GetAllCategories();

            ViewBag.Make = RepoObj1.GetAllMake();

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Showroom")]
        [Route("Showroom/PostNewAd")]
        public ActionResult PostNewAd(ImageFile objImage, ValidateShowroomAds showroomAds)
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
                        if (showroomAds != null)
                        {
                            var ShowroomID = Convert.ToInt32(Session["Id"]);
                            showroomAds.Condition = (showroomAds.Condition == "1") ? "Used" : "New";
                            var showroom = RepoObj.GetShowRoomByID(ShowroomID);
                            showroomAds.tblShowroomID = Convert.ToInt32(Session["Id"]);
                            showroomAds.CarImage = name;
                            showroomAds.CurrentLocation = showroom.ShopNumber + " " + showroom.tblAddress.CompleteAddress;

                            var AdsPublish = RepoObj1.InsertShowroomAds(showroomAds);
                            if (AdsPublish)
                            {
                                TempData["SuccessMsg"] = "Ad Publish Successfully!";
                                return RedirectToAction("PostNewAd");
                            }
                            else
                            {
                                TempData["ErrorMsg"] = "Error on Ads Publishing please try again!";
                                return RedirectToAction("PostNewAd");
                            }
                        }
                        else
                        {
                            ViewBag.Error = "Error please try again!";
                            return View("PostNewAd");
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Error on uploading Image!";
                        return View("PostNewAd");
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

        [HttpPost]
        public ActionResult SetSession(string[] RemovedImages)
        {
            Session["DeletedFiles"] = RemovedImages;
            return Json("Request!!", JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "Showroom")]
        [Route("ShowroomAd/EditAd/{AdID}")]
        public ActionResult EditAd(int AdID)
        {
            ViewBag.Years = RepoObj1.ListOfYears();

            ViewBag.Conditions = RepoObj1.Conditions();

            ViewBag.Transmissions = RepoObj1.Transmissions();

            ViewBag.Assemblys = RepoObj1.Assemblys();

            ViewBag.Colors = RepoObj1.Colors();

            ViewBag.BodyTypes = RepoObj1.BodyTypes();

            ViewBag.Category = RepoObj1.GetAllCategories();

            ViewBag.Make = RepoObj1.GetAllMake();

            var reas = RepoObj1.GetShowroomAdsDetailOnlyForUpdate(AdID);

            ViewBag.SubCategory = RepoObj1.GetSubCategoriesByCategoryForDropdown(reas.CategoryId.Value);

            ViewBag.CarModel = RepoObj1.GetModelsByMakeForDropdown(reas.ManufacturerId.Value);

            Session["AdID"] = reas.tblCarID;
            Session["AddressID"] = reas.AddressId;
            Session["CarFeatureID"] = reas.CarFeatureID;
            Session["CarModelID"] = reas.CarModelID;

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

        [AcceptVerbs(HttpVerbs.Post)]
        [Route("ShowroomAd/EditAd")]
        public ActionResult EditAd(ImageFile objImage, ValidateShowroomAds showroomAds)
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
                        var reas = UpdateAd(showroomAds);
                        var AdID = showroomAds.tblCarID;
                        if (reas)
                        {
                            return RedirectToRoute("EditAd", new { AdID });
                        }
                        else
                        {
                            return RedirectToRoute("EditAd", new { AdID });
                        }
                    }
                    else
                    {
                        var reas = UpdateAd(showroomAds);
                        var AdID = showroomAds.tblCarID;
                        if (reas)
                        {
                            return RedirectToRoute("EditAd", new { AdID });
                        }
                        else
                        {
                            return RedirectToRoute("EditAd", new { AdID });
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

        public bool UpdateAd(ValidateShowroomAds showroomAds)
        {
            if (showroomAds != null)
            {
                var ShowroomID = Convert.ToInt32(Session["Id"]);
                showroomAds.tblCarID = Convert.ToInt32(Session["AdID"]);
                showroomAds.AddressId = Convert.ToInt32(Session["AddressID"]);
                showroomAds.CarFeatureID = Convert.ToInt32(Session["CarFeatureID"]);
                showroomAds.CarModelID = Convert.ToInt32(Session["CarModelID"]);
                showroomAds.Condition = (showroomAds.Condition == "1") ? "Used" : "New";
                var showroom = RepoObj.GetShowRoomByID(ShowroomID);
                showroomAds.tblShowroomID = ShowroomID;
                showroomAds.CurrentLocation = showroom.ShopNumber + " " + showroom.tblAddress.CompleteAddress;
                var AdsPublish = RepoObj1.UpdateShowroomAds(showroomAds);
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
            else
            {
                ViewBag.Error = "Error please try again!";
                return false;
            }
        }
    }
}