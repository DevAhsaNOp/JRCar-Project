using JRCar.BLL.Repositories;
using JRCar.BOL;
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
    public class AccountController : Controller
    {
        private UserRepo RepoObj;

        public AccountController()
        {
            RepoObj = new UserRepo();
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SignUp()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SignUp(HttpPostedFileBase file, ValidateUser user)
        {
            try
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
                        RepoObj.InsertModel(user);
                        file.SaveAs(path);
                        TempData["SuccessMsg"] = "Account Created Successfully!";
                        ModelState.Clear();
                        return RedirectToAction("SignIn");
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Image size is very large";
                        return RedirectToAction("SignUp");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error occured on creating Account!" + ex.Message;
                return RedirectToAction("SignUp");
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SignIn()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SignIn(ValidateUser user)
        {
            try
            {
                var IsSuccess = RepoObj.GetModelByID(user.Email, user.Password);
                if (IsSuccess != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Name, false);
                    TempData["SuccessMsg"] = "Account Login Successfully!";
                    Session["Id"] = IsSuccess.ID;
                    Session["Name"] = IsSuccess.Name;
                    Session["Email"] = IsSuccess.Email;
                    Session["Image"] = IsSuccess.Image;
                    var str = (string)@Session["Image"];
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMsg"] = "<b>Invalid Email or Passowrd!</b>";
                    return View("SignIn");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error occured on login Account!" + ex.Message;
                return RedirectToAction("SignIn");
            }
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ForgotPassword(ValidateUser user)
        {
            try
            {
                var IsSuccess = RepoObj.ForgotPassword(user.Email);
                if (IsSuccess)
                {
                    TempData["SuccessMsg"] = "Please check your <b>email</b> for a message with your code. Your code is 6 numbers long!";
                    Session["Email"] = user.Email;
                    return View("_PasswordRecover");
                }
                else
                {
                    TempData["ErrorMsg"] = "No Account found with this Email Address!";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error occured on Account!" + ex.Message;
                return RedirectToAction("ForgotPassword");
            }
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CheckOTP(ValidateUser user)
        {
            try
            {
                var Email = @Session["Email"].ToString();
                TempData["Email"] = Email;
                var IsSuccess = RepoObj.CheckOTP(Email,user.OTP);
                if (IsSuccess)
                {
                    TempData["SuccessMsg"] = "OTP Confirmed!";
                    Session["Email"] = user.Email;
                    return View("_ResetPassword");
                }
                else
                {
                    TempData["ErrorMsg"] = "Incorrect OTP please recheck your email!";
                    return View("_PasswordRecover");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error occured on Account!" + ex.Message;
                return RedirectToAction("SignIn");
            }
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ResetPass(ValidateUser user)
        {
            try
            {
                var Email = @TempData["Email"].ToString();
                user.Email = Email;
                var IsSuccess = RepoObj.UpdateModel(user);
                if (IsSuccess)
                {
                    TempData["SuccessMsg"] = "<b>Hurry!</b> your Password is Reset...";
                    return View("SignIn");
                }
                else
                {
                    TempData["ErrorMsg"] = "No Account found with this Email Address!";
                    return RedirectToAction("SignUp");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error occured on Account!" + ex.Message;
                return RedirectToAction("ResetPass");
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tbl_img = RepoObj.GetModelByID(id.GetValueOrDefault());

            if (tbl_img == null)
            {
                return HttpNotFound();
            }
            return View(tbl_img);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("SignIn");
        }
    }
}