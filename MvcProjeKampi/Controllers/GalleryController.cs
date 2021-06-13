using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProjeKampi.Controllers
{
    public class GalleryController : Controller
    {
        ImageFileManager ımagefileManager = new ImageFileManager(new EfImageFileDal());
        
        
        // GET: Gallery
        public ActionResult Index()
        {
            var files = ımagefileManager.GetList();
            
            return View(files);
        }
    }
}