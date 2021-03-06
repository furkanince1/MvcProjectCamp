using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.Utilities.Hashing;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using EntityLayer.Dto;
using MvcProjeKampi.Roles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace MvcProjeKampi.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        AdminManager adminManager = new AdminManager(new EfAdminDal());
        IAuthService authService = new AuthManager(new AdminManager(new EfAdminDal()), new WriterManager(new EfWriterDal()));
        WriterManager writerManager = new WriterManager(new EfWriterDal());

        // GET: Login

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(LoginDto loginDto)
        {



            var response = Request["g-recaptcha-response"];
            const string secret = "6Lfw6T8bAAAAAItuIShiVWQ5-4K-WhNS-m51WOtD";
            var client = new WebClient();
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResult>(reply);
            if (captchaResponse.Success && authService.Login(loginDto))
            {
                foreach (Admin item in adminManager.GetList())
                {
                    bool result = HashingHelper.VerifyAdminHash(loginDto.AdminUserName, loginDto.AdminPassword, item.AdminUserName, item.AdminPasswordHash, item.AdminPasswordSalt);
                    if (result == true)
                    {
                        FormsAuthentication.SetAuthCookie(loginDto.AdminUserName, false);
                        Session["AdminUserName"] = loginDto.AdminUserName;
                        return RedirectToAction("Index", "AdminCategory");
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = "Kullanıcı adı veya Parola yanlış";
                        return View();
                    }
                }
                return View();
            }
            else
            {
                ViewData["ErrorMessage"] = "Kullanıcı adı veya Parola yanlış";
                return View();
            }
        }

        

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public ActionResult WriterLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult WriterLogin(WriterLoginDto writerLoginDto)
        {
            var response = Request["g-recaptcha-response"];
            const string secret = "6Lfw6T8bAAAAAItuIShiVWQ5-4K-WhNS-m51WOtD";
            var client = new WebClient();
            var reply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResult>(reply);

            if (authService.WriterLogin(writerLoginDto) && captchaResponse.Success)
            {
                var writerMail = writerManager.GetList().Where(x => x.WriterMail == writerLoginDto.WriterMail).FirstOrDefault();
                if (writerMail != null)
                {
                    bool result = HashingHelper.WriterVerifyPasswordHash(writerLoginDto.WriterPassword, writerMail.WriterPasswordHash, writerMail.WriterPasswordSalt);
                    if (result == true)
                    {
                        FormsAuthentication.SetAuthCookie(writerLoginDto.WriterMail, false);
                        Session["WriterEmail"] = writerLoginDto.WriterMail;
                        return RedirectToAction("MyContent", "WriterPanelContent");
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = "Kullanıcı adı veya Parola yanlış";
                        return View();
                    }

                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Kullanıcı adı veya Parola yanlış";
                return RedirectToAction("WriterLogin");
            }
            return View();
        }
        public ActionResult WriterLogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Headings", "Default");
        }

    }
}