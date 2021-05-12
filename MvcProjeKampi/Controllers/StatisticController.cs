using DataAccessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProjeKampi.Controllers
{
    public class StatisticController : Controller
    {
        Context context = new Context();
        // GET: Statistic
        public ActionResult Index()
        {
            var totalCategory = context.Categories.Count().ToString();
            ViewBag.TotalCategory = totalCategory;

            var softwareTitles = context.Headings.Count(x=>x.CategoryId==12).ToString();
            ViewBag.SoftwareTitles = softwareTitles;

            var thoseWithCharacter = context.Writers.Where(x => x.WriterName.Contains("a")).Count();
            ViewBag.ThoseWithCharacter = thoseWithCharacter;

            var categoryMostTitles = context.Headings.GroupBy(x => x.Category.CategoryName)
                .OrderByDescending(z => z.Count()).Select(y => y.Key).FirstOrDefault();
            ViewBag.CategoryMostTitles = categoryMostTitles;

            var trueFalse = context.Categories.Count(x => x.CategoryStatus == true) - context.Categories.Count(x => x.CategoryStatus == false);
            ViewBag.TrueFalse = trueFalse;



            return View();
        }


    }
}