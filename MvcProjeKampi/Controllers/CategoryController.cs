using BusinessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProjeKampi.Controllers
{
    public class CategoryController : Controller
    {

        CategoryManager cm = new CategoryManager();
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCategoryList()
        {
            var CategoryValues = cm.GetAllBL();
            return View(CategoryValues);
        }


        
    }
}