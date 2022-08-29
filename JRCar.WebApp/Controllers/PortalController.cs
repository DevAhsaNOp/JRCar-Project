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

namespace JRCar.WebApp.Controllers
{
    public class PortalController : Controller
    {
        private UserRepo RepoObj;

        public PortalController()
        {
            RepoObj = new UserRepo();
        }

        public ActionResult Index()
        {
            return View();
        }

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

        [HttpGet]
        public JsonResult GetNotificationsList()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
                    NotificationComponent NC = new NotificationComponent();
                    var list = NC.GetNotifications(notificationRegisterTime);
                    //Update session here for get only new added (Notifications)
                    Session["LastUpdate"] = DateTime.Now;
                    return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    //insert into tblNotification values('Hey','Blablablabalb',null,2042,108,1,1,getdate())
                    var err = (int)HttpStatusCode.BadRequest;
                    return Json(new { error = err + " Bad Request Error " + "Invalid Request!!" });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error occured on updating Account!" + ex.Message;
                throw ex;
            }
        }
    }
}