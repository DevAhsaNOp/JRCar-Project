using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using JRCar.BLL.Repositories;
using JRCar.BOL.Validation_Classes;
using System.Text.RegularExpressions;
using System.Linq;

namespace JRCar.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private UserRepo RepoObj;
        private AddressAutofillRepo AddressRepoObj;

        public AccountController()
        {
            RepoObj = new UserRepo();
            AddressRepoObj = new AddressAutofillRepo();
        }

        public JsonResult IsEmailExist(string SignUpEmail)
        {
            return Json(!RepoObj.IsEmailExist(SignUpEmail), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsUpdateEmailExist(string SignUpUpdateEmail)
        {
            string UserCurrentEmail;
            if (Session["UserEditEmail"] != null)
                UserCurrentEmail = Session["UserEditEmail"].ToString();
            else
                UserCurrentEmail = Session["Email"].ToString();
            return Json(!RepoObj.IsUpdateEmailExist(SignUpUpdateEmail, UserCurrentEmail), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "Admin,Showroom,Union")]
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SetTempData(string value)
        {
            TempData["SuccessMsg"] = value;
            return new EmptyResult();
        }

        #region **Account Configuration**

        #region **User SignUp**

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
                if (file == null)
                {
                    user.Image = Session["ImageAvatar"].ToString();
                    user.Email = user.SignUpEmail;
                    RepoObj.InsertUser(user);
                    TempData["SuccessMsg"] = "Account Created Successfully!";
                    ModelState.Clear();
                    if (Session["UnionUserSignUp"] != null)
                    {
                        if (Session["UnionUserSignUp"].ToString() == "1")
                        {
                            Session["UnionUserSignUp"] = null;
                            return RedirectToAction("AddUser", "Portal");
                        }
                        else
                        {
                            Session["UnionUserSignUp"] = null;
                            TempData["ErrorMsg"] = "Error occured on creating Account!";
                            return RedirectToAction("AddUser", "Portal");
                        }
                    }
                    else
                    {
                        return RedirectToAction("SignIn");
                    }
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
                            RepoObj.InsertUser(user);
                            file.SaveAs(path);
                            TempData["SuccessMsg"] = "Account Created Successfully!";
                            ModelState.Clear();
                            if (Session["UnionUserSignUp"] != null)
                            {
                                if (Session["UnionUserSignUp"].ToString() == "1")
                                {
                                    Session["UnionUserSignUp"] = null;
                                    return RedirectToAction("AddUser", "Portal");
                                }
                                else
                                {
                                    Session["UnionUserSignUp"] = null;
                                    TempData["ErrorMsg"] = "Error occured on creating Account!";
                                    return RedirectToAction("AddUser", "Portal");
                                }
                            }
                            else
                            {
                                return RedirectToAction("SignIn");
                            }
                        }
                        else
                        {
                            if (Session["UnionUserSignUp"] != null)
                            {
                                if (Session["UnionUserSignUp"].ToString() == "1")
                                {
                                    Session["UnionUserSignUp"] = null;
                                    TempData["ErrorMsg"] = "Image size is very large";
                                    return RedirectToAction("AddUser", "Portal");
                                }
                                else
                                {
                                    Session["UnionUserSignUp"] = null;
                                    TempData["ErrorMsg"] = "Image size is very large";
                                    return RedirectToAction("AddUser", "Portal");
                                }
                            }
                            else
                            {
                                TempData["ErrorMsg"] = "Image size is very large";
                                return RedirectToAction("SignUp");
                            }
                        }
                    }
                    else
                    {
                        if (Session["UnionUserSignUp"] != null)
                        {
                            if (Session["UnionUserSignUp"].ToString() == "1")
                            {
                                Session["UnionUserSignUp"] = null;
                                TempData["ErrorMsg"] = "Image is not in correct format kindly choose jpg/jpeg/png files!";
                                return RedirectToAction("AddUser", "Portal");
                            }
                            else
                            {
                                Session["UnionUserSignUp"] = null;
                                TempData["ErrorMsg"] = "Image is not in correct format kindly choose jpg/jpeg/png files!";
                                return RedirectToAction("AddUser", "Portal");
                            }
                        }
                        else
                        {
                            TempData["ErrorMsg"] = "Image is not in correct format kindly choose jpg/jpeg/png files!";
                            return RedirectToAction("SignUp");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error occured on creating Account!" + ex.Message;
                return RedirectToAction("SignUp");
            }
        }

        #endregion

        #region **Showroom SignUp**

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShowroomSignUp()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ShowroomSignUp(HttpPostedFileBase file, ValidateShowroom user)
        {
            try
            {
                if (file == null)
                {
                    user.Image = Session["ImageAvatar"].ToString();
                    user.Email = user.SignUpEmail;
                    /*These are hard coded values change it when project scope is changes to multiple Union*/
                    user.UnionId = 1;
                    user.AddressId = 2009;
                    var area = AddressRepoObj.GetZoneLatLong(284);
                    user.Latitude = area.Item1.ToString();
                    user.Longitude = area.Item2.ToString();
                    /*-------------------------------------THE-----END-------------------------------------*/
                    var ShowroomMaxId = RepoObj.GetAllShowRoom().OrderByDescending(u => u.ID).FirstOrDefault().ID;
                    user.CreatedBy = ((Convert.ToInt32(Session["Id"]) == 0) ? ShowroomMaxId + 1 : Convert.ToInt32(Session["Id"]));
                    RepoObj.InsertShowroom(user);
                    TempData["SuccessMsg"] = "Account Created Successfully!";
                    ModelState.Clear();
                    if (Session["UnionShowroomSignUp"] != null)
                    {
                        if (Session["UnionShowroomSignUp"].ToString() == "1")
                        {
                            Session["UnionShowroomSignUp"] = null;
                            return RedirectToAction("AddShowroom", "Portal");
                        }
                        else
                        {
                            Session["UnionShowroomSignUp"] = null;
                            TempData["ErrorMsg"] = "Error occured on creating Account!";
                            return RedirectToAction("AddShowroom", "Portal");
                        }
                    }
                    else
                    {
                        return RedirectToAction("SignIn");
                    }
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
                            /*These are hard coded values change it when project scope is changes to multiple Union*/
                            user.UnionId = 1;
                            user.AddressId = 2009;
                            var area = AddressRepoObj.GetZoneLatLong(284);
                            user.Latitude = area.Item1.ToString();
                            user.Longitude = area.Item2.ToString();
                            /*-------------------------------------THE-----END-------------------------------------*/
                            var ShowroomMaxId = RepoObj.GetAllShowRoom().OrderByDescending(u => u.ID).FirstOrDefault().ID;
                            user.CreatedBy = ((Convert.ToInt32(Session["Id"]) == 0) ? ShowroomMaxId + 1 : Convert.ToInt32(Session["Id"]));
                            RepoObj.InsertShowroom(user);
                            file.SaveAs(path);
                            TempData["SuccessMsg"] = "Account Created Successfully!";
                            ModelState.Clear();
                            if (Session["UnionShowroomSignUp"] != null)
                            {
                                if (Session["UnionShowroomSignUp"].ToString() == "1")
                                {
                                    Session["UnionShowroomSignUp"] = null;
                                    return RedirectToAction("AddShowroom", "Portal");
                                }
                                else
                                {
                                    Session["UnionShowroomSignUp"] = null;
                                    TempData["ErrorMsg"] = "Error occured on creating Account!";
                                    return RedirectToAction("AddShowroom", "Portal");
                                }
                            }
                            else
                            {
                                return RedirectToAction("SignIn");
                            }
                        }
                        else
                        {
                            if (Session["UnionShowroomSignUp"] != null)
                            {
                                if (Session["UnionShowroomSignUp"].ToString() == "1")
                                {
                                    Session["UnionShowroomSignUp"] = null;
                                    TempData["ErrorMsg"] = "Image size is very large";
                                    return RedirectToAction("AddShowroom", "Portal");
                                }
                                else
                                {
                                    Session["UnionShowroomSignUp"] = null;
                                    TempData["ErrorMsg"] = "Image size is very large";
                                    return RedirectToAction("AddShowroom", "Portal");
                                }
                            }
                            else
                            {
                                TempData["ErrorMsg"] = "Image size is very large";
                                return RedirectToAction("ShowroomSignUp");
                            }
                        }
                    }
                    else
                    {
                        if (Session["UnionShowroomSignUp"] != null)
                        {
                            if (Session["UnionShowroomSignUp"].ToString() == "1")
                            {
                                Session["UnionShowroomSignUp"] = null;
                                TempData["ErrorMsg"] = "Image is not in correct format kindly choose jpg/jpeg/png files!";
                                return RedirectToAction("AddShowroom", "Portal");
                            }
                            else
                            {
                                Session["UnionShowroomSignUp"] = null;
                                TempData["ErrorMsg"] = "Image is not in correct format kindly choose jpg/jpeg/png files!";
                                return RedirectToAction("AddShowroom", "Portal");
                            }
                        }
                        else
                        {
                            TempData["ErrorMsg"] = "Image is not in correct format kindly choose jpg/jpeg/png files!";
                            return RedirectToAction("ShowroomSignUp");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error occured on creating Account!" + ex.Message;
                return RedirectToAction("ShowroomSignUp");
            }
        }

        #endregion

        #region **SignIn For Everyone**

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
                var IsSuccess = RepoObj.CheckLoginDetails(user.Email, user.Password);
                if (IsSuccess != null && IsSuccess.Active == true)
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    //TempData["SuccessMsg"] = "Account Login Successfully!";
                    Session["Id"] = IsSuccess.ID;
                    Session["Name"] = Regex.Replace(IsSuccess.Name.ToUpper().Split()[0], @"[^0-9a-zA-Z\ ]+", "");
                    Session["Email"] = IsSuccess.Email;
                    Session["Image"] = IsSuccess.Image;
                    Session["Role"] = IsSuccess.Role;
                    var role = Session["Role"].ToString();
                    if (role == "Admin" || role == "Union" || role == "Showroom")
                    {
                        return RedirectToAction("Index");
                    }
                    else if (role == "User")
                    {
                        return RedirectToAction("Index", "Website");
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Error occured on login Account!";
                        return RedirectToAction("SignIn");
                    }
                }
                else if (IsSuccess != null && IsSuccess.Active == false)
                {
                    TempData["ErrorMsg"] = "<b>Your account has been Deactivated by Admin. Contact with support for further guidance!</b>";
                    return View("SignIn");
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

        #endregion

        #endregion

        #region **Password Configuration**
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
                var IsSuccess = RepoObj.CheckOTP(Email, user.OTP);
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
                var Role = RepoObj.GetUserRole(Email).Role;
                var IsSuccess = RepoObj.UpdateUser(user, Role);
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
        #endregion

        [Authorize(Roles = "Admin,Union")]
        public ActionResult Details(int? id)
        {
            id = 2042;
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
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            this.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            this.Response.Cache.SetNoStore();
            return View("SignIn");
        }
    }
}