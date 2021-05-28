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
    public class HeadingController : Controller
    {
        HeadingManager headingManager = new HeadingManager(new EfHeadingDal());
        CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());
        WriterManager writerManager = new WriterManager(new EfWriterDal());

        // GET: Heading
        public ActionResult Index()
        {
            var headingvalues = headingManager.GetList();

            return View(headingvalues);
        }
        [HttpGet]
        public ActionResult AddHeading()
        {
            List<SelectListItem> valuecategory = (from x in categoryManager.GetList() select new SelectListItem { Text = x.CategoryName, Value = x.CategoryId.ToString() }).ToList();
            ViewBag.vlc = valuecategory;

            List<SelectListItem> valuewriter = (from x in writerManager.GetList() select new SelectListItem { Text = x.WriterName + " " + x.WriterSurname, Value = x.WriterId.ToString() }).ToList();
            ViewBag.vlw = valuewriter;

            return View();
        }
        [HttpPost]
        public ActionResult AddHeading(Heading heading)
        {
            heading.HeadingDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            headingManager.HeadingAdd(heading);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult EditHeading(int id)
        {
            List<SelectListItem> valuecategory = (from x in categoryManager.GetList() select new SelectListItem { Text = x.CategoryName, Value = x.CategoryId.ToString() }).ToList();
            ViewBag.vlc = valuecategory;


            var headingvalue = headingManager.GetById(id);
            return View(headingvalue);

        }

        [HttpPost]
        public ActionResult EditHeading(Heading heading)
        {
            headingManager.HeadingUpdate(heading);
            return RedirectToAction("Index");
        }

        public ActionResult DeleteHeading(int id)
        {
            var headingvalue = headingManager.GetById(id);
            headingvalue.HeadingStatus = false;
            headingManager.HeadingDelete(headingvalue);
            
            
            return RedirectToAction("Index");
        }
        
        public ActionResult ActiveHeading(int id)
        {
            var headingvalue = headingManager.GetById(id);
            headingvalue.HeadingStatus = true;
            headingManager.ActiveHeading(headingvalue);
            return RedirectToAction("Index");

        }

    }
}