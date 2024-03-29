﻿using JRCar.BOL;
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
//using Microsoft.Reporting.WebForms;
using System.Data;
using ClosedXML.Excel;
using System.Text.RegularExpressions;
//using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System.Web.Helpers;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.EMMA;
using System.Web.Routing;

namespace JRCar.WebApp.Controllers
{
    public class PortalController : Controller
    {
        private UserRepo RepoObj;
        private UserAdsRepo userAdsRepo;
        private PaymentRepo PayRepoObj;
        private ShowroomAdsRepo RepoObj1;
        private AddressAutofillRepo AddressRepoObj;

        public PortalController()
        {
            RepoObj = new UserRepo();
            PayRepoObj = new PaymentRepo();
            userAdsRepo = new UserAdsRepo();
            RepoObj1 = new ShowroomAdsRepo();
            AddressRepoObj = new AddressAutofillRepo();
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult IsMonthExist(int ShowroomID, List<string> RecievedDate)
        {
            return Json(PayRepoObj.IsMonthPaymentRecieved(ShowroomID, RecievedDate), JsonRequestBehavior.AllowGet);
        }

        #region **Showroom Action Methods**

        #region **Showroom Reporting**
        [Authorize(Roles = "Showroom")]
        public ActionResult ShowroomReport()
        {
            var id = Convert.ToInt32(Session["Id"]);
            var reas = RepoObj1.GetAllShowroomAdsForReport(id);
            return View(reas);
        }

        public FileResult DownloadExcel()
        {
            var id = Convert.ToInt32(Session["Id"]);
            var reas = RepoObj1.GetAllShowroomAdsForReport(id);
            var date = DateTime.Now.ToString("dd/MMM/yyyy");
            DataTable dataTable = new DataTable("ShowroomCars");
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("Manufacturer_Name", typeof(string));
            dataTable.Columns.Add("Manufacturer_CarModelName", typeof(string));
            dataTable.Columns.Add("Year", typeof(string));
            dataTable.Columns.Add("Condition", typeof(string));
            dataTable.Columns.Add("RegNo", typeof(string));
            dataTable.Columns.Add("RegLocation", typeof(string));
            dataTable.Columns.Add("Color", typeof(string));
            dataTable.Columns.Add("MaxSpeed", typeof(string));
            dataTable.Columns.Add("GearType", typeof(string));
            dataTable.Columns.Add("Transmission", typeof(string));
            dataTable.Columns.Add("Mileage", typeof(string));
            dataTable.Columns.Add("Price", typeof(string));
            dataTable.Columns.Add("CarIsActive", typeof(string));

            foreach (var item in reas)
            {
                DataRow row = dataTable.NewRow();
                row["Title"] = item.Title;
                row["Description"] = item.Description;
                row["Manufacturer_Name"] = item.Manufacturer_Name;
                row["Manufacturer_CarModelName"] = item.Manufacturer_CarModelName;
                row["Year"] = item.Year;
                row["Condition"] = item.Condition;
                row["RegNo"] = item.RegNo;
                row["RegLocation"] = item.RegLocation;
                row["Color"] = item.Color;
                row["MaxSpeed"] = item.MaxSpeed;
                row["GearType"] = item.GearType;
                row["Transmission"] = item.Transmission;
                row["Mileage"] = item.Mileage;
                row["Price"] = item.Price;
                row["CarIsActive"] = item.CarIsActive;

                dataTable.Rows.Add(row);
            }

            string filename = reas.FirstOrDefault().ShowroomName + "_" + date + "_" + "report";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename + ".xlsx");
                }
            }

        }

        [Authorize(Roles = "Showroom")]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShowroomPaymentReport()
        {
            var ShowroomID = Convert.ToInt32(Session["Id"]);
            var reas = PayRepoObj.GetShowroomDetailsById(ShowroomID);
            return View(reas);
        }

        #endregion

        #region **Any Account Profile Update**
        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "Admin,Showroom,Union")]
        public ActionResult UpdateProfile()
        {
            Session["UserEditEmail"] = null;
            Session["UserEditPhoneNumber"] = null;
            var id = Convert.ToInt32(Session["Id"]);
            var Role = Session["Role"].ToString();
            var reas = RepoObj.GetUserDetailById(id, Role);
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
                                user.Email = user.SignUpUpdateEmail;
                                user.Number = user.SignUpUpdateNumber;
                                Session["PhoneNumber"] = user.Number;
                                Session["Email"] = user.Email;
                                var Role = Session["Role"].ToString();
                                var IsUpdated = RepoObj.UpdateUser(user, role);
                                //string oldImgPath = Request.MapPath(Session["Image"].ToString());
                                if (IsUpdated)
                                {
                                    Session["Name"] = Regex.Replace(user.Name.ToUpper().Split()[0], @"[^0-9a-zA-Z\ ]+", "");
                                    file.SaveAs(path);
                                    TempData["SuccessMsg"] = "Account Updated Successfully!";
                                    Session["Image"] = user.Image;
                                    if (role == "Admin" || role == "Union" || role == "Showroom")
                                    {
                                        return RedirectToAction("UpdateProfile");
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
                                    //if (System.IO.File.Exists(oldImgPath))
                                    //{
                                    //    System.IO.File.Delete(oldImgPath);
                                    //}
                                }
                                else
                                {
                                    if (role == "Admin" || role == "Union" || role == "Showroom")
                                    {
                                        TempData["ErrorMsg"] = "Error occured on login Account!";
                                        return RedirectToAction("UpdateProfile");
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
                        user.Email = user.SignUpUpdateEmail;
                        user.Number = user.SignUpUpdateNumber;
                        Session["PhoneNumber"] = user.Number;
                        Session["Email"] = user.Email;
                        user.UpdatedBy = (int)Session["Id"];
                        var IsUpdated = RepoObj.UpdateUser(user, role);
                        Session["Name"] = Regex.Replace(user.Name.ToUpper().Split()[0], @"[^0-9a-zA-Z\ ]+", "");
                        if (IsUpdated)
                        {
                            TempData["SuccessMsg"] = "Account Updated Successfully!";
                            if (User.Identity.IsAuthenticated)
                            {
                                FormsAuthentication.SetAuthCookie(user.Email, false);
                                if (role == "Admin" || role == "Union" || role == "Showroom")
                                {
                                    return RedirectToAction("UpdateProfile");
                                }
                                else if (role == "User")
                                {
                                    return RedirectToAction("ProfileSettings", "Website");
                                }
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
                                return RedirectToAction("UpdateProfile");
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
                return RedirectToAction("UpdateProfile");
            }
        }
        #endregion

        #region **Notification For Showroom About New Ads, Union Announcements and User Appointments**

        [Authorize(Roles = "Showroom")]
        public ActionResult NotificationList()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var ShowroomID = Convert.ToInt32(Session["Id"]);
                    NotificationComponent NC = new NotificationComponent();
                    var list = NC.GetAllNotifications(ShowroomID);
                    return View(list);
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

        [Authorize(Roles = "Showroom")]
        public ActionResult AppointmentList()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var ShowroomID = Convert.ToInt32(Session["Id"]);
                    NotificationComponent NC = new NotificationComponent();
                    var list = NC.GetShowroomAppointmentsById(ShowroomID);
                    return View(list);
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
                    Session["LastUpdated"] = DateTime.Now;
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
                TempData["ErrorMsg"] = "Error occured on loading Notification!" + ex.Message;
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
                    Session["LastUpdated"] = DateTime.Now;
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
                    Session["LastUpdated"] = DateTime.Now;
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

        [HttpGet]
        public JsonResult GetAnnouncementsList()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var ShowroomID = Convert.ToInt32(Session["Id"]);
                    var notificationRegisterTime = Session["AnnLastUpdated"] != null ? Convert.ToDateTime(Session["AnnLastUpdated"]) : DateTime.Now;
                    NotificationComponent NC = new NotificationComponent();
                    var list = NC.GetAnnouncements(notificationRegisterTime, ShowroomID);
                    //Update session here for get only new added (Announcements)
                    Session["AnnLastUpdated"] = DateTime.Now;
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
                TempData["ErrorMsg"] = "Error occured on loading Announcements!" + ex.Message;
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult GetAnnouncementsListCount()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var ShowroomID = Convert.ToInt32(Session["Id"]);
                    var notificationRegisterTime = Session["AnnLastUpdated"] != null ? Convert.ToDateTime(Session["AnnLastUpdated"]) : DateTime.Now;
                    NotificationComponent NC = new NotificationComponent();
                    var list = NC.GetAnnouncementsCount(notificationRegisterTime, ShowroomID);
                    //Update session here for get only new added (Announcements)
                    Session["AnnLastUpdated"] = DateTime.Now;
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
                TempData["ErrorMsg"] = "Error occured on updating Announcements Count!" + ex.Message;
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult ChangeAnnouncementsToAsRead()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var ShowroomID = Convert.ToInt32(Session["Id"]);
                    NotificationComponent NC = new NotificationComponent();
                    var list = NC.ChangeAnnouncementsToAsRead(ShowroomID);
                    //Update session here for get only new added (Announcements)
                    Session["AnnLastUpdated"] = DateTime.Now;
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

        [HttpGet]
        public JsonResult GetShowroomAppointmentsList()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (Session["Id"] != null)
                    {
                        var ShowroomID = Convert.ToInt32(Session["Id"]);
                        var notificationRegisterTime = Session["AppLastUpdated"] != null ? Convert.ToDateTime(Session["AppLastUpdated"]) : DateTime.Now;
                        NotificationComponent NC = new NotificationComponent();
                        var list = NC.GetShowroomAppointments(notificationRegisterTime, ShowroomID);
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
        public JsonResult GetShowroomAppointmentById(int id)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    TempData["AppntID"] = id;
                    var AppntID = id;
                    NotificationComponent NC = new NotificationComponent();
                    var list = NC.GetShowroomCurrAppointmentById(AppntID);
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

        [HttpPost]
        public JsonResult AcceptShowroomAppointment(string Purpose, string Date, bool IsAppntDel)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var AppntID = Convert.ToInt32(TempData["AppntID"]);
                    var Usr = Convert.ToInt32(Session["Id"]);
                    AppointmentRepo appointmentRepo = new AppointmentRepo();
                    var reas = appointmentRepo.AcceptAppointment(AppntID, Usr, Purpose, Date, IsAppntDel);
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
        public JsonResult ShowroomAppointmentReject()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var AppntID = Convert.ToInt32(TempData["AppntID"]);
                    var Usr = Convert.ToInt32(Session["Id"]);
                    AppointmentRepo appointmentRepo = new AppointmentRepo();
                    var reas = appointmentRepo.RejectAppointment(AppntID, Usr);
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

        [HttpGet]
        public JsonResult GetShowroomAppointmentsListCount()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var ShowroomID = Convert.ToInt32(Session["Id"]);
                    var notificationRegisterTime = Session["AppLastUpdated"] != null ? Convert.ToDateTime(Session["AppLastUpdated"]) : DateTime.Now;
                    NotificationComponent NC = new NotificationComponent();
                    var list = NC.GetShowroomTodaysAppointmentsCount(notificationRegisterTime, ShowroomID);
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
        public JsonResult ChangeShowroomAppointmentsToAsRead()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var ShowroomID = Convert.ToInt32(Session["Id"]);
                    NotificationComponent NC = new NotificationComponent();
                    var list = NC.ChangeShowroomAppointmentToAsRead(ShowroomID);
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

        [HttpGet]
        [Route("MyMessages")]
        public ActionResult ShowroomMessagesList()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (Session["Id"] != null)
                    {
                        var ShowroomID = Convert.ToInt32(Session["Id"]);
                        NotificationComponent NC = new NotificationComponent();
                        var list = NC.GetShowroomMessagesById(ShowroomID);
                        return View(list);
                    }
                    else
                    {
                        var err = (int)HttpStatusCode.BadRequest;
                        return Json(new { error = err + " Bad Request Error " + "Invalid Request!!" });
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
                TempData["ErrorMsg"] = "Error occured on loading Messages!" + ex.Message;
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult GetShowroomMessagesList()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (Session["Id"] != null)
                    {
                        var ShowroomID = Convert.ToInt32(Session["Id"]);
                        NotificationComponent NC = new NotificationComponent();
                        var list = NC.GetShowroomMessages(ShowroomID);
                        return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                    else
                    {
                        var err = (int)HttpStatusCode.BadRequest;
                        return Json(new { error = err + " Bad Request Error " + "Invalid Request!!" });
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
                TempData["ErrorMsg"] = "Error occured on loading Messages!" + ex.Message;
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult GetShowroomMessagesListCount()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var ShowroomID = Convert.ToInt32(Session["Id"]);
                    NotificationComponent NC = new NotificationComponent();
                    var list = NC.GetShowroomMessagesCount(ShowroomID);
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
                TempData["ErrorMsg"] = "Error occured on updating Messages Count!" + ex.Message;
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult ChangeShowroomMessagesToAsRead()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var ShowroomID = Convert.ToInt32(Session["Id"]);
                    NotificationComponent NC = new NotificationComponent();
                    var list = NC.ChangeShowroomMessageToAsRead(ShowroomID);
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
                TempData["ErrorMsg"] = "Error occured on updating Messages As Read!" + ex.Message;
                throw ex;
            }
        }

        #endregion

        #region **Showroom Post and Edit Ad**
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
                            showroomAds.CategoryId = 1;
                            showroomAds.SubCategoryId = 1;
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
        #endregion

        #region **Showroom Ads**

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "Showroom")]
        [Route("ShowroomAd/MyAds")]
        public ActionResult MyAds()
        {
            var id = Convert.ToInt32(Session["Id"]);
            var reas = RepoObj1.GetAllShowroomAds(id);
            return View(reas);
        }

        [Authorize(Roles = "Showroom")]
        public ActionResult ShowroomLoadVehicle()
        {
            var id = Convert.ToInt32(Session["Id"]);
            var rows = Convert.ToInt32(Session["showroomrows"]) + 2;
            var reas = RepoObj1.GetAllShowroomAds(id).Take(rows);
            Session["showroomrows"] = rows;
            return PartialView("_LoadVehicle", reas);
        }

        #endregion

        #region **Showroom Car Remove & Mark As Sold**

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Showroom")]
        public ActionResult ShowroomCarRemoved(int AdID)
        {
            var carDetail = RepoObj1.InActiveShowroomAds(AdID);
            if (carDetail)
            {
                TempData["SuccessMsg"] = "Ad Removed Successfully!";
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["ErrorMsg"] = "Something went wrong. Please try again!";
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Showroom")]
        public ActionResult ShowroomCarSold(int AdID)
        {
            var carDetail = RepoObj1.MarkSoldShowroomAds(AdID);
            if (carDetail)
            {
                TempData["SuccessMsg"] = "Ad Mark As Sold and Deactivated Successfully!";
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["ErrorMsg"] = "Something went wrong. Please try again!";
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region **Showroom Ads Details**
        [AcceptVerbs(HttpVerbs.Get)]
        [Route("Ads/{AdID}")]
        public ActionResult CarAds(string AdID)
        {
            var carDetail = RepoObj1.GetShowroomAdsDetail(AdID);
            var ShowroomID = Convert.ToInt32(Session["Id"]);
            Session["IsAppntShow"] = carDetail.tblShowroomID == ShowroomID ? "true" : "false";
            if (carDetail == null || carDetail.tblCarIsactive == false)
            {
                Session["ShowCarID"] = null;
                TempData["ErrorMsg"] = "Car you trying to view is not exists!";
                return RedirectToAction("AllVehicles", "Website");
            }
            else
            {
                RepoObj1.IncreaseShowroomAdViewCount(carDetail.tblCarID);
                string path = Server.MapPath("" + carDetail.CarImage + "");
                string[] FolderName = carDetail.CarImage.Split('/');
                string[] imageFiles = Directory.GetFiles(path);
                List<string> images = new List<string>();
                foreach (var item in imageFiles)
                {
                    images.Add(FolderName[2] + "/" + Path.GetFileName(item));
                }
                ViewBag.Images = images;
                Session["ShowCarID"] = carDetail.tblCarID;
                if (User.Identity.IsAuthenticated)
                {
                    var UserID = Convert.ToInt32(Session["Id"]);
                    var IsShortlist = RepoObj1.IsCarShortlist(carDetail.tblCarID, UserID);
                    if (IsShortlist)
                    {
                        Session["IsShortlistCC"] = "true";
                    }
                    else
                    {
                        Session["IsShortlistCC"] = "false";
                    }
                    AppointmentRepo repo = new AppointmentRepo();
                    var reas = repo.IsUserRequestThisCarAppointment((int)Session["Id"], carDetail.tblCarID);
                    if (reas)
                    {
                        Session["IsSchedule"] = "true";
                    }
                    else
                    {
                        Session["IsSchedule"] = "false";
                    }
                }
                return View(carDetail);
            }
        }
        #endregion

        #region **Car Ads Appointment**

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SetTime(string Time)
        {
            Session["Apptime"] = Time;
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "User")]
        public ActionResult ScheduleAppointment(string selecteddatetime, string purpose)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (selecteddatetime != null && purpose != null)
                    {
                        if (purpose.Length > 1 && selecteddatetime.Length > 1)
                        {
                            var email = Session["Email"].ToString();
                            var UserID = Convert.ToInt32(Session["Id"]);
                            var CarID = Convert.ToInt32(Session["ShowCarID"]);
                            string userphone = RepoObj.GetUserByID(UserID).Number;
                            AppointmentRepo repo = new AppointmentRepo();

                            ValidateAppointment appointment = new ValidateAppointment()
                            {
                                Email = email,
                                UserInterestedID = UserID,
                                ShowroomCarID = CarID,
                                Number = userphone,
                                Purpose = purpose,
                                Datetime = Convert.ToDateTime(selecteddatetime),
                                CreatedBy = UserID,
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

        #region **Showroom Payment**
        [Authorize(Roles = "Showroom")]
        [AcceptVerbs(HttpVerbs.Get)]
        [Route("Payments")]
        public ActionResult ShowroomPaymentInfo()
        {
            var ShowroomID = Convert.ToInt32(Session["Id"]);
            var reas = PayRepoObj.GetShowroomDetailsById(ShowroomID);
            return View(reas);
        }
        #endregion

        #endregion

        #region **Unions/Admin Action Methods**

        #region **Make New Announcements**

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult MakeAnnouncment()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    NotificationComponent NC = new NotificationComponent();
                    var list = NC.GetAllAnnouncement();
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
                TempData["ErrorMsg"] = "Error occured on updating Announcement!" + ex.Message;
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult MakeAnnouncment(string Title, string Message)
        {
            if (Title != null && Message != null)
            {
                if (Title.Length > 1 && Message.Length > 1)
                {
                    NotificationRepo notiRepo = new NotificationRepo();
                    tblAnnouncement tbl = new tblAnnouncement()
                    {
                        Title = Title,
                        Description = Message
                    };
                    var reas = notiRepo.InsertAnnouncements(tbl);
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

        #endregion

        #region **Manage User**

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult AddUser()
        {
            Session["ImageAvatar"] = "~/Images/user.png";
            Session["UnionUserSignUp"] = "1";
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult ListOfUser()
        {
            var reas = RepoObj.GetAllUsers();
            return View(reas);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult UserEdit(int UserID)
        {
            ViewBag.UserStatus = RepoObj1.UserStatus();
            var id = UserID;
            var Role = RepoObj.GetAllUsers().Where(x => x.ID == UserID).FirstOrDefault().tblRole.Role;
            var Email = RepoObj.GetAllUsers().Where(x => x.ID == UserID).FirstOrDefault().Email;
            var PhoneN = RepoObj.GetAllUsers().Where(x => x.ID == UserID).FirstOrDefault().Number;
            var reas = RepoObj.GetUserDetailById(id, Role);
            Session["CurrentUserAvatar"] = reas.Image;
            Session["UserEditID"] = id;
            Session["UserEditEmail"] = Email;
            Session["UserEditPhoneNumber"] = PhoneN;
            return View(reas);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult UserEdit(HttpPostedFileBase file, ValidateUser user)
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
                        user.ID = (int)Session["UserEditID"];
                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                        {
                            var role = RepoObj.GetUserRole(Session["UserEditEmail"].ToString()).Role;
                            user.tblRoleName = role;
                            if (file.ContentLength <= 10000000)
                            {
                                user.UpdatedBy = (int)Session["Id"];
                                user.Email = user.SignUpUpdateEmail;
                                user.Number = user.SignUpUpdateNumber;
                                var IsUpdated = RepoObj.UpdateUser(user, role);
                                string oldImgPath = Request.MapPath(Session["Image"].ToString());
                                Session["UserEditID"] = null;
                                Session["UserEditEmail"] = null;
                                Session["UserEditPhoneNumber"] = null;
                                if (IsUpdated)
                                {
                                    file.SaveAs(path);
                                    if (System.IO.File.Exists(oldImgPath))
                                    {
                                        if (Path.GetFileNameWithoutExtension(oldImgPath) == "user")
                                        {
                                        }
                                        else
                                        {
                                            System.IO.File.Delete(oldImgPath);
                                        }
                                        Session["CurrentUserAvatar"] = user.Image;
                                        TempData["SuccessMsg"] = "Account Updated Successfully!";
                                        if (user.Active == "1")
                                        {
                                            return RedirectToAction("UserEdit", new { UserID = user.ID });
                                        }
                                        else
                                        {
                                            var IsInactive = RepoObj.InActiveModel(user.ID, role);
                                            return RedirectToAction("UserEdit", new { UserID = user.ID });
                                        }
                                    }
                                }
                                else
                                {
                                    TempData["ErrorMsg"] = "Error occured on updating Account!";
                                    return RedirectToAction("UserEdit", new { UserID = user.ID });
                                }
                            }
                            else
                            {
                                Session["UserEditID"] = null;
                                Session["UserEditEmail"] = null;
                                Session["UserEditPhoneNumber"] = null;
                                TempData["ErrorMsg"] = "Image size is very large";
                                return RedirectToAction("UserEdit", new { UserID = user.ID });
                            }
                        }
                        else
                        {
                            Session["UserEditID"] = null;
                            Session["UserEditEmail"] = null;
                            Session["UserEditPhoneNumber"] = null;
                            TempData["ErrorMsg"] = "Image is not in correct format kindly choose jpg/jpeg/png files";
                            return RedirectToAction("UserEdit", new { UserID = user.ID });
                        }
                    }
                    else
                    {
                        user.Image = Session["CurrentUserAvatar"].ToString();
                        user.ID = (int)Session["UserEditID"];
                        var role = RepoObj.GetUserRole(Session["UserEditEmail"].ToString()).Role;
                        user.tblRoleName = role;
                        user.Email = user.SignUpUpdateEmail;
                        user.Number = user.SignUpUpdateNumber;
                        user.UpdatedBy = (int)Session["Id"];
                        var IsUpdated = RepoObj.UpdateUser(user, role);
                        Session["UserEditID"] = null;
                        Session["UserEditEmail"] = null;
                        Session["UserEditPhoneNumber"] = null;
                        if (IsUpdated)
                        {
                            TempData["SuccessMsg"] = "Account Updated Successfully!";
                            if (user.Active == "1")
                            {
                                return RedirectToAction("UserEdit", new { UserID = user.ID });
                            }
                            else
                            {
                                var IsInactive = RepoObj.InActiveModel(user.ID, role);
                                return RedirectToAction("UserEdit", new { UserID = user.ID });
                            }
                        }
                        else
                        {
                            TempData["ErrorMsg"] = "Error occured on updating Account!";
                            return RedirectToAction("UserEdit", new { UserID = user.ID });
                        }
                    }
                    return RedirectToAction("UserEdit", new { UserID = (int)Session["UserEditID"] });
                }
                else
                {
                    Session["UserEditID"] = null;
                    Session["UserEditEmail"] = null;
                    Session["UserEditPhoneNumber"] = null;
                    var err = (int)HttpStatusCode.BadRequest;
                    return Json(new { error = err + " Bad Request Error " + "Invalid Request!!" });
                }
            }
            catch (Exception ex)
            {
                Session["UserEditID"] = null;
                Session["UserEditEmail"] = null;
                Session["UserEditPhoneNumber"] = null;
                TempData["ErrorMsg"] = "Error occured on updating Account!" + ex.Message;
                return RedirectToAction("UserEdit", new { UserID = (int)Session["UserEditID"] });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult UserInActive(int ID)
        {
            if (ID > 0)
            {
                var role = "User";
                var IsInactive = RepoObj.InActiveModel(ID, role);
                if (IsInactive)
                {
                    TempData["SuccessMsg"] = "Account Deactivated Successfully!";
                    return Json("True", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["ErrorMsg"] = "Error Occured On Account Deactivation!";
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Error Occured On Account Deactivation!";
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region **Manage Showroom**

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult AddShowroom()
        {
            Session["ImageAvatar"] = "~/Images/user.png";
            Session["UnionShowroomSignUp"] = "1";
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult ListOfShowroom()
        {
            var reas = RepoObj.GetAllShowRoom();
            return View(reas);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult ShowroomEdit(int UserID)
        {
            ViewBag.UserStatus = RepoObj1.UserStatus();
            var id = UserID;
            var Role = RepoObj.GetAllShowRoom().Where(x => x.ID == UserID).FirstOrDefault().tblRole.Role;
            var Email = RepoObj.GetAllShowRoom().Where(x => x.ID == UserID).FirstOrDefault().Email;
            var PhoneN = RepoObj.GetAllShowRoom().Where(x => x.ID == UserID).FirstOrDefault().Contact;
            var reas = RepoObj.GetUserDetailById(id, Role);
            Session["CurrentUserAvatar"] = reas.Image;
            Session["UserEditID"] = id;
            Session["UserEditEmail"] = Email;
            Session["UserEditPhoneNumber"] = PhoneN;
            return View(reas);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult ShowroomEdit(HttpPostedFileBase file, ValidateUser user)
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
                        user.ID = (int)Session["UserEditID"];
                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                        {
                            var role = RepoObj.GetUserRole(Session["UserEditEmail"].ToString()).Role;
                            user.tblRoleName = role;
                            if (file.ContentLength <= 10000000)
                            {
                                user.UpdatedBy = (int)Session["Id"];
                                user.Email = user.SignUpUpdateEmail;
                                user.Number = user.SignUpUpdateNumber;
                                var IsUpdated = RepoObj.UpdateUser(user, role);
                                //string oldImgPath = Request.MapPath(Session["Image"].ToString());
                                Session["UserEditID"] = null;
                                Session["UserEditEmail"] = null;
                                Session["UserEditPhoneNumber"] = null;
                                if (IsUpdated)
                                {
                                    file.SaveAs(path);
                                    Session["CurrentUserAvatar"] = user.Image;
                                    TempData["SuccessMsg"] = "Account Updated Successfully!";
                                    if (user.Active == "1")
                                    {
                                        return RedirectToAction("ShowroomEdit", new { UserID = user.ID });
                                    }
                                    else
                                    {
                                        var IsInactive = RepoObj.InActiveModel(user.ID, role);
                                        return RedirectToAction("ShowroomEdit", new { UserID = user.ID });
                                    }
                                    //if (System.IO.File.Exists(oldImgPath))
                                    //{
                                    //    if (Path.GetFileNameWithoutExtension(oldImgPath) == "user")
                                    //    {
                                    //    }
                                    //    else
                                    //    {
                                    //        System.IO.File.Delete(oldImgPath);
                                    //    }
                                    //}
                                }
                                else
                                {
                                    TempData["ErrorMsg"] = "Error occured on updating Account!";
                                    return RedirectToAction("ShowroomEdit", new { UserID = user.ID });
                                }
                            }
                            else
                            {
                                Session["UserEditID"] = null;
                                Session["UserEditEmail"] = null;
                                Session["UserEditPhoneNumber"] = null;
                                TempData["ErrorMsg"] = "Image size is very large";
                                return RedirectToAction("ShowroomEdit", new { UserID = user.ID });
                            }
                        }
                        else
                        {
                            Session["UserEditID"] = null;
                            Session["UserEditEmail"] = null;
                            Session["UserEditPhoneNumber"] = null;
                            TempData["ErrorMsg"] = "Image size is not in correct format kindly choose jpg/jpeg/png files";
                            return RedirectToAction("ShowroomEdit", new { UserID = user.ID });
                        }
                    }
                    else
                    {
                        user.Image = Session["CurrentUserAvatar"].ToString();
                        user.ID = (int)Session["UserEditID"];
                        var role = RepoObj.GetUserRole(Session["UserEditEmail"].ToString()).Role;
                        user.tblRoleName = role;
                        user.Email = user.SignUpUpdateEmail;
                        user.Number = user.SignUpUpdateNumber;
                        user.UpdatedBy = (int)Session["Id"];
                        var IsUpdated = RepoObj.UpdateUser(user, role);
                        Session["UserEditID"] = null;
                        Session["UserEditEmail"] = null;
                        Session["UserEditPhoneNumber"] = null;
                        if (IsUpdated)
                        {
                            TempData["SuccessMsg"] = "Account Updated Successfully!";
                            if (user.Active == "1")
                            {
                                return RedirectToAction("ShowroomEdit", new { UserID = user.ID });
                            }
                            else
                            {
                                var IsInactive = RepoObj.InActiveModel(user.ID, role);
                                return RedirectToAction("ShowroomEdit", new { UserID = user.ID });
                            }
                        }
                        else
                        {
                            TempData["ErrorMsg"] = "Error occured on updating Account!";
                            return RedirectToAction("ShowroomEdit", new { UserID = user.ID });
                        }
                    }
                    //return RedirectToAction("ShowroomEdit", new { UserID = (int)Session["UserEditID"] });
                }
                else
                {
                    Session["UserEditID"] = null;
                    Session["UserEditEmail"] = null;
                    Session["UserEditPhoneNumber"] = null;
                    var err = (int)HttpStatusCode.BadRequest;
                    return Json(new { error = err + " Bad Request Error " + "Invalid Request!!" });
                }
            }
            catch (Exception ex)
            {
                Session["UserEditID"] = null;
                Session["UserEditEmail"] = null;
                Session["UserEditPhoneNumber"] = null;
                TempData["ErrorMsg"] = "Error occured on updating Account!" + ex.Message;
                return RedirectToAction("ShowroomEdit", new { UserID = (int)Session["UserEditID"] });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult ShowroomInActive(int ID)
        {
            if (ID > 0)
            {
                var role = "Showroom";
                var IsInactive = RepoObj.InActiveModel(ID, role);
                if (IsInactive)
                {
                    TempData["SuccessMsg"] = "Account Deactivated Successfully!";
                    return Json("True", JsonRequestBehavior.AllowGet);

                }
                else
                {
                    TempData["ErrorMsg"] = "Error Occured On Account Deactivation!";
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Error Occured On Account Deactivation!";
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region **Manage Ads**

        [Authorize(Roles = "Admin,Union")]
        public ActionResult ListofUserAds()
        {
            var reas = userAdsRepo.GetAllAds();
            return View(reas);
        }

        [Authorize(Roles = "Admin,Union")]
        public ActionResult UserAdsActive(int AdID)
        {
            if (AdID > 0)
            {
                var IsInactive = userAdsRepo.ReActiveUserAds(AdID);
                if (IsInactive)
                {
                    TempData["SuccessMsg"] = "Ad Activated Successfully!";
                    return Json("True", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["ErrorMsg"] = "Error Occured On Ad Activation!";
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Error Occured On Ad Activation!";
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Admin,Union")]
        public ActionResult UserAdsInActive(int AdID)
        {
            if (AdID > 0)
            {
                var IsInactive = userAdsRepo.InActiveUserAds(AdID);
                if (IsInactive)
                {
                    TempData["SuccessMsg"] = "Ad Deactivated Successfully!";
                    return Json("True", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["ErrorMsg"] = "Error Occured On Ad Deactivation!";
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Error Occured On Ad Deactivation!";
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Admin,Union")]
        public ActionResult ListofShowroomAds()
        {
            var reas = RepoObj1.GetAllAds();
            return View(reas);
        }

        [Authorize(Roles = "Admin,Union")]
        public ActionResult ShowroomAdsActive(int AdID)
        {
            if (AdID > 0)
            {
                var IsInactive = RepoObj1.ReActiveShowroomAds(AdID);
                if (IsInactive)
                {
                    TempData["SuccessMsg"] = "Ad Activated Successfully!";
                    return Json("True", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["ErrorMsg"] = "Error Occured On Ad Activation!";
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Error Occured On Ad Activation!";
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Admin,Union")]
        public ActionResult ShowroomAdsInActive(int AdID)
        {
            if (AdID > 0)
            {
                var IsInactive = RepoObj1.InActiveShowroomAds(AdID);
                if (IsInactive)
                {
                    TempData["SuccessMsg"] = "Ad Deactivated Successfully!";
                    return Json("True", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["ErrorMsg"] = "Error Occured On Ad Deactivation!";
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Error Occured On Ad Deactivation!";
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region **Manage Union For Admin Only**

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "Admin")]
        public ActionResult ListOfUnion()
        {
            var reas = RepoObj.GetAllUnion().Where(x => x.tblCRoleID==null);
            return View(reas);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Admin")]
        public ActionResult UnionInActive(int ID)
        {
            if (ID > 0)
            {
                var role = "Union";
                var IsInactive = RepoObj.InActiveModel(ID, role);
                if (IsInactive)
                {
                    TempData["SuccessMsg"] = "Account Deactivated Successfully!";
                    return Json("True", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["ErrorMsg"] = "Error Occured On Account Deactivation!";
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Error Occured On Account Deactivation!";
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Admin")]
        public ActionResult UnionActive(int ID)
        {
            if (ID > 0)
            {
                var role = "Union";
                var IsInactive = RepoObj.ActiveModel(ID, role);
                if (IsInactive)
                {
                    TempData["SuccessMsg"] = "Account Activated Successfully!";
                    return Json("True", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["ErrorMsg"] = "Error Occured On Account Activation!";
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Error Occured On Account Activation!";
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region **Manage Payment**

        [Authorize(Roles = "Admin,Union")]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult MakePayment()
        {
            ViewBag.Dealers = RepoObj1.GetAllDealers();
            ViewBag.Months = RepoObj1.GetAllMonths();

            return View();
        }

        [Authorize(Roles = "Admin,Union")]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShowroomPayment()
        {
            var reas = PayRepoObj.GetAllShowRoom();
            return View(reas);
        }

        [Authorize(Roles = "Admin,Union")]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShowroomInfo(int ShowroomID)
        {
            var reas = PayRepoObj.GetShowroomDetailsById(ShowroomID);
            return View(reas);
        }

        [Authorize(Roles = "Admin,Union")]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetShowroomData(int ShowroomID)
        {
            var reas = PayRepoObj.GetShowroomDetailById(ShowroomID);
            if (reas != null)
            {
                return Json(reas, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Admin,Union")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MakePayment(ValidationPayment payment)
        {
            try
            {
                IEnumerable<string> RecievableMonth = null;
                var receivedDates = payment.RecievedDate;
                var Months = PayRepoObj.GetRecievableMonths(payment.ShowroomID.Value);

                if (Months != null)
                {
                    RecievableMonth = Months.Except(receivedDates);
                }

                var CurrentMonth = DateTime.Now.Month;

                DateTime NextRecievablemonthStart = DateTime.Now;
                DateTime NextRecievablemonthEnd = DateTime.Now;

                var receivedFirstMonth = Convert.ToDateTime(receivedDates.First());
                var receivedLastMonth = Convert.ToDateTime(receivedDates.Last());
                var receivedfirstmonth = Convert.ToDateTime(receivedFirstMonth.Month + " " + receivedFirstMonth.Year);
                var receivedlastmonth = Convert.ToDateTime(receivedLastMonth.Month + " " + receivedLastMonth.Year);
                var receivedfirstmonthStart = new DateTime(receivedfirstmonth.Year, receivedfirstmonth.Month, 1);
                var receivedlastmonthStart = new DateTime(receivedlastmonth.Year, receivedlastmonth.Month, 1);
                var receivedlastmonthEnd = receivedlastmonthStart.AddMonths(1).AddDays(-1);
                NextRecievablemonthStart = receivedlastmonthStart.AddMonths(1).AddDays(0);

                if (receivedlastmonthEnd.Month < CurrentMonth)
                {
                    DateTime now = DateTime.Now;
                    var startDate = new DateTime(now.Year, now.Month, 1);
                    NextRecievablemonthEnd = startDate.AddMonths(1).AddDays(-1);
                    //NextRecievablemonthEnd = receivedlastmonthStart.AddMonths(2).AddDays(-1);
                }
                else if (receivedlastmonthEnd.Month >= CurrentMonth)
                {
                    NextRecievablemonthEnd = receivedlastmonthStart.AddMonths(2).AddDays(-1);
                }

                ValidationPayment Payment = new ValidationPayment()
                {
                    ShowroomID = payment.ShowroomID,
                    Recievable = payment.Recievable,
                    Recieved = payment.Recieved,
                    Discount = payment.Discount,
                    Balance = payment.Balance,
                    RecievedFromDate = receivedfirstmonthStart,
                    RecievedToDate = receivedlastmonthEnd,
                    RecievableFromDate = NextRecievablemonthStart,
                    RecievableToDate = NextRecievablemonthEnd,
                    CreatedBy = (int)Session["Id"],
                };

                var reas = PayRepoObj.InsertPayment(Payment);
                if (reas)
                {
                    TempData["SuccessMsg"] = "Payment Added Successfully!";
                }
                else
                {
                    TempData["ErrorMsg"] = "Error occured on Payment of Showroom!";
                }
                return RedirectToAction("MakePayment");
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error occured on Payment of Showroom!";
                throw ex;
            }

        }

        #endregion

        #region **Manage Union Roles**

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult AddNewUnionRole()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult AddNewUnionRole(ValidateRolePermission model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (model != null)
                {
                    model.CreatedBy = (int)Session["Id"];
                    var reas = RepoObj.InsertRoleWithPermission(model);
                    ModelState.Clear();
                    if (reas)
                    {
                        TempData["SuccessMsg"] = "Role Added Successfully!";
                        return View();
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Error occured on adding Role!";
                        return View();
                    }
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Error occured on adding Role!";
                return View();
            }
            return View();
        }
        
        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult EditUnionRole(int ID)
        {
            var reas = RepoObj.GetRolePermissionByID(ID);
            TempData["RoleID"] = reas.RoleID;
            TempData["ID"] = reas.ID;
            return View(reas);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult EditUnionRole(ValidateRolePermission model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (model != null)
                {
                    model.UpdatedBy = (int)Session["Id"];
                    model.ID = Convert.ToInt32(TempData["ID"]);
                    model.RoleID = Convert.ToInt32(TempData["RoleID"]);
                    var reas = RepoObj.EditRoleWithPermission(model);
                    ModelState.Clear();
                    if (reas)
                    {
                        TempData["SuccessMsg"] = "Role Updated Successfully!";
                        return RedirectToAction("ListUnionRole");
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Error occured on Updating Role!";
                        return RedirectToAction("ListUnionRole");
                    }
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Error occured on Updating Role!";
                return RedirectToAction("ListUnionRole");
            }
            return View();
        }
        
        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult ListUnionRole()
        {
            var reas = RepoObj.GetRolePermission();
            return View(reas);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult RoleInActive(int ID)
        {
            if (ID > 0)
            {
                var IsInactive = RepoObj.InactiveRole(ID);
                if (IsInactive)
                {
                    TempData["SuccessMsg"] = "Role Deactivated Successfully!";
                    return Json("True", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["ErrorMsg"] = "Error Occured On Role Deactivation!";
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Error Occured On Role Deactivation!";
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult RoleActive(int ID)
        {
            if (ID > 0)
            {
                var IsInactive = RepoObj.ActiveRole(ID);
                if (IsInactive)
                {
                    TempData["SuccessMsg"] = "Role Activated Successfully!";
                    return Json("True", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["ErrorMsg"] = "Error Occured On Role Activation!";
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Error Occured On Role Activation!";
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region **Manage Union Member**

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult AddUnionMember()
        {
            ViewBag.UnionMemberRoles = RepoObj.GetAllUnionRole();
            Session["ImageAvatar"] = "~/Images/user.png";
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult AddUnionMember(HttpPostedFileBase file, ValidateUser user)
        {
            try
            {
                if (file == null)
                {
                    user.Image = Session["ImageAvatar"].ToString();
                    user.Email = user.SignUpEmail;
                    user.CreatedBy = (int)Session["Id"];
                    user.tblRoleID = Convert.ToInt32(user.UnionRoleName);
                    RepoObj.InsertUnion(user);
                    TempData["SuccessMsg"] = "Account Created Successfully!";
                    ModelState.Clear();
                    return RedirectToAction("AddUnionMember");
                }
                else
                {
                    string filename = Path.GetFileName(file.FileName);
                    string _filename = DateTime.Now.ToString("yymmssfff") + filename;
                    string extension = Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images/"), _filename);
                    user.Image = "~/Images/" + _filename;

                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (file.ContentLength <= 10000000)
                        {
                            user.Email = user.SignUpEmail;
                            user.CreatedBy = (int)Session["Id"];
                            user.tblRoleID = Convert.ToInt32(user.UnionRoleName);
                            RepoObj.InsertUnion(user);
                            file.SaveAs(path);
                            TempData["SuccessMsg"] = "Account Created Successfully!";
                            ModelState.Clear();
                            return RedirectToAction("AddUnionMember");
                        }
                        else
                        {
                            TempData["ErrorMsg"] = "Image size is very large";
                            return RedirectToAction("AddUnionMember");
                        }
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Image is not in correct format kindly choose jpg/jpeg/png files!";
                        return RedirectToAction("AddUnionMember");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error occured on creating Account!" + ex.Message;
                return RedirectToAction("AddUnionMember");
            }
        }
        
        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult EditUnionMember(int ID)
        {
            ViewBag.UnionMemberRoles = RepoObj.GetAllUnionRole();
            Session["UserUnionMemID"] = ID;
            var Role = "Union";
            var reas = RepoObj.GetUserDetailById(ID, Role);
            Session["UserEditEmail"] = reas.Email;
            Session["UserEditPhoneNumber"] = reas.Number;
            Session["UserUnionMemImage"] = reas.Image;
            Session["UserUnionMemName"] = reas.Name;
            return View(reas);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult EditUnionMember(HttpPostedFileBase file, ValidateUser user)
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
                            var role ="Union";
                            user.tblRoleName = role;
                            if (file.ContentLength <= 10000000)
                            {
                                user.ID = (int)Session["UserUnionMemID"];
                                user.UpdatedBy = (int)Session["Id"];
                                user.Email = user.SignUpUpdateEmail;
                                user.Number = user.SignUpUpdateNumber;
                                user.tblCRoleID = Convert.ToInt32(user.UnionRoleName);
                                var IsUpdated = RepoObj.UpdateUser(user, role);
                                if (IsUpdated)
                                {
                                    Session["UserEditEmail"] = null;
                                    Session["UserEditPhoneNumber"] = null;
                                    file.SaveAs(path);
                                    TempData["SuccessMsg"] = "Account Updated Successfully!";
                                    return RedirectToAction("ListUnionMember");
                                }
                                else
                                {
                                    Session["UserEditEmail"] = null;
                                    Session["UserEditPhoneNumber"] = null;
                                    TempData["ErrorMsg"] = "Error occured on Updating Account!";
                                     return RedirectToAction("ListUnionMember");
                                }
                            }
                            else
                            {
                                Session["UserEditEmail"] = null;
                                Session["UserEditPhoneNumber"] = null;
                                TempData["ErrorMsg"] = "Image size is very large";
                                return RedirectToAction("EditUnionMember", new { ID = (int)Session["UserUnionMemID"] });
                            }
                        }
                    }
                    else
                    {
                        user.ID = (int)Session["UserUnionMemID"];
                        var role = "Union";
                        user.Image = Session["UserUnionMemImage"].ToString();
                        user.tblRoleName = role;
                        user.Email = user.SignUpUpdateEmail;
                        user.Number = user.SignUpUpdateNumber;
                        user.UpdatedBy = (int)Session["Id"];
                        user.tblCRoleID = Convert.ToInt32(user.tblRole.ID);
                        var IsUpdated = RepoObj.UpdateUser(user, role);
                        if (IsUpdated)
                        {
                            Session["UserEditEmail"] = null;
                            Session["UserEditPhoneNumber"] = null;
                            TempData["SuccessMsg"] = "Account Updated Successfully!";
                            return RedirectToAction("ListUnionMember");
                        }
                        else
                        {
                            Session["UserEditEmail"] = null;
                            Session["UserEditPhoneNumber"] = null;
                            TempData["ErrorMsg"] = "Error occured on Updating Account!";
                            return RedirectToAction("ListUnionMember");
                        }
                    }
                    return View();
                }
                else
                {
                    Session["UserEditEmail"] = null;
                    Session["UserEditPhoneNumber"] = null;
                    var err = (int)HttpStatusCode.BadRequest;
                    return Json(new { error = err + " Bad Request Error " + "Invalid Request!!" });
                }
            }
            catch (Exception ex)
            {
                Session["UserEditEmail"] = null;
                Session["UserEditPhoneNumber"] = null;
                TempData["ErrorMsg"] = "Error occured on updating Account!" + ex.Message;
                return RedirectToAction("ListUnionMember");
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "Admin,Union")]
        public ActionResult ListUnionMember()
        {
            var reas = RepoObj.GetAllUnion().Where(x => x.tblCRoleID != null);
            return View(reas);
        }

        #endregion

        #region **Union/Admin Reports**

        [Authorize(Roles = "Admin,Union")]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult UsersInfoList()
        {
            var reas = RepoObj.UsersInfoList();
            return View(reas);
        }

        [Authorize(Roles = "Admin,Union")]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShowroomInfoList()
        {
            var reas = RepoObj.ShowroomInfoList();
            return View(reas);
        }

        #endregion

        #endregion
    }
}