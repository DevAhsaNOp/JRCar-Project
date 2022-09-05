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
using Microsoft.AspNet.Identity;
using static JRCar.WebApp.Controllers.WebsiteController;
using System.Security.Claims;
using System.Threading;

namespace JRCar.WebApp.Controllers
{
    public class PortalController : Controller
    {
        private UserRepo RepoObj;
        private ShowroomAdsRepo RepoObj1;

        public PortalController()
        {
            RepoObj = new UserRepo();
            RepoObj1 = new ShowroomAdsRepo();
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
        public ActionResult PostNewAd()
        {
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
    }
}