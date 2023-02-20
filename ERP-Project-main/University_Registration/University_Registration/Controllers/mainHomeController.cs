using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using University_Registration.Models;

namespace University_Registration.Controllers
{
    public class mainHomeController : Controller
    {
        ERP_SystemEntities1 db = new ERP_SystemEntities1();

        // GET: mainHome
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EmailSubmit(string name,string email,string message,string subject)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add("obidatm68@gmail.com");
            mail.From = new MailAddress(email);

            mail.Subject = email;
            mail.Body = "message";

            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential("obidatm68@gmail.com", "zrwtqpllpsbviqws");
            smtp.Send(mail);
            return RedirectToAction("Contact");
        }

        public ActionResult coarses(int? id)
        {

            if (id == null)
            {
                var course = from m in db.Majors select m;
                return View(course);
            }
            else
            {
                var course = from m in db.Majors where m.Facility_ID == id select m;
                return View(course);
            }
            return View();
        }
        public ActionResult test()
        {
            return View();
        }
    }
}