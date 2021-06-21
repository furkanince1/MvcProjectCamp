using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProjeKampi.Controllers
{
    public class ContactController : Controller
    {
        ContactManager contactManager = new ContactManager(new EfContactDal());
        MessageManager messageManager = new MessageManager(new EfMessageDal());
        ContactValidator validationRules = new ContactValidator();

        public ActionResult Index()
        {
            var contactvalues = contactManager.GetList();
            return View(contactvalues);
        }

        public ActionResult GetContactDetails(int id)
        {
            var contactvalues = contactManager.GetById(id);
            
            return View(contactvalues);
        }

        public PartialViewResult MessageListMenu()
        {
            var contact = contactManager.GetList().Count();
            ViewBag.contact = contact;

            var sendMail = messageManager.GetListSendbox().Count();
            ViewBag.sendMail = sendMail;

            var receiverMail = messageManager.GetListInbox().Count();
            ViewBag.receiverMail = receiverMail;

            var draftMail = messageManager.GetListSendbox().Where(m => m.IsDraft == true).Count();
            ViewBag.draftMail = draftMail;

            var readMessage = messageManager.GetListInbox().Where(m => m.IsRead == true).Count();
            ViewBag.readMessage = readMessage;

            var unreadMessage = messageManager.GetAllRead().Count();
            ViewBag.unreadMessage = unreadMessage;


            return PartialView();
        }

    }
}