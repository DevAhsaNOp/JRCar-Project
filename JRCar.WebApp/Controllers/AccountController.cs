using JRCar.BLL.Repositories;
using JRCar.BOL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace JRCar.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private UserRepo RepoObj;

        public AccountController()
        {
            RepoObj = new UserRepo();
        }

        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Register()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Register(HttpPostedFileBase file, tblUser user)
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
                    if (file.ContentLength <= 1000000)
                    {
                        RepoObj.InsertModel(user);
                        file.SaveAs(path);
                        TempData["SuccessMsg"] = "Account Created Successfully!";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.msg = "Image size is very large";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error occured on creating Account!" + ex.Message;
                return RedirectToAction("Register");
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
    }
}