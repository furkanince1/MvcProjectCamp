using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProjeKampi.Controllers
{
    public class AboutController : Controller
    {
        AboutManager aboutManager = new AboutManager(new EfAboutDal());


        public ActionResult Index()
        {
            var aboutvalues = aboutManager.GetList();

            return View(aboutvalues);
        }
        [HttpPost]
        public ActionResult AddAbout(About about)
        {
            aboutManager.AboutAdd(about);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateAbout(int id)
        {
            var result = aboutManager.GetById(id);
            if (result.AboutStatus == true)
            {
                result.AboutStatus = false;
            }
            else
            {
                result.AboutStatus = true;
            }
            aboutManager.AboutUpdate(result);
            return RedirectToAction("Index");


        }

        public PartialViewResult AboutPartial()
        {
            return PartialView();
        }
    }
}