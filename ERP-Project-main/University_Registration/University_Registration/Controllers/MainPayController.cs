using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University_Registration.Models;

namespace University_Registration.Controllers
{
    public class MainPayController : Controller
    {
        private ERP_SystemEntities1 db = new ERP_SystemEntities1();

        // GET: MainPay

        public ActionResult Mainpayy()
        {
            string ASPid = User.Identity.GetUserId();
            var Mainid = db.Students.FirstOrDefault(c => c.Id == ASPid).Student_ID;
            ViewBag.studentwallet = db.Students.FirstOrDefault(c => c.Student_ID == Mainid).Wallet;
            return View();
        }

        //Action Button
        [HttpPost]
        public ActionResult MainFillWallet([Bind(Include = "Student_ID,Id,Name,Email,Password,NationalNum,Grad,Pic,Status,PhoneNumber,BirthFile,PersonalIdFile,CertificateFile,Gender,Major_ID")] Student student, string search)
        {
            try
            {
                double money = Convert.ToDouble(search);

                string ASPid = User.Identity.GetUserId();
                var Mainid = db.Students.FirstOrDefault(c => c.Id == ASPid).Student_ID;

                ViewBag.Mainid = Mainid;
                double fill = Convert.ToDouble(db.Students.FirstOrDefault(a => a.Student_ID == Mainid).Wallet);
                double total = fill + money;
                db.Students.FirstOrDefault(a => a.Student_ID == Mainid).Wallet = total;
                db.SaveChanges();
            }

            catch (FormatException ex)
            {
                string ASPid = User.Identity.GetUserId();
                var Mainid = db.Students.FirstOrDefault(c => c.Id == ASPid).Student_ID;
                ViewBag.Mainid = Mainid;
                ViewBag.studentwallet = db.Students.FirstOrDefault(c => c.Student_ID == Mainid).Wallet;
                ViewBag.errormessage = ex.Message;
                return View("Mainpayy");

            }
            return RedirectToAction("Mainpayment");
        }



        public ActionResult Mainpayment()
        {
            int t = 0;
            string ASPid = User.Identity.GetUserId();
            var Mainid = db.Students.FirstOrDefault(c => c.Id == ASPid).Student_ID;
            var walletmain = db.Students.FirstOrDefault(c => c.Student_ID == Mainid).Wallet;
            ViewBag.walletmain = walletmain;
            ViewBag.Mainid = Mainid;
            var info = db.SubjectRegistrations.Where(c => c.Student_ID == Mainid && c.PaymentStatus == false).ToList();
            var info1 = db.SubjectRegistrations.Where(c => c.Student_ID == Mainid).ToList();
            foreach (var tootalprice in info)
            {

                t += (int)tootalprice.Price;



            }
            ViewBag.totalprice = t;

            return View(info1);
        }
        //ActionEdit

        public ActionResult MaincourseP()
        {

            int t = 0;
            string ASPid = User.Identity.GetUserId();
            var Mainid = db.Students.FirstOrDefault(c => c.Id == ASPid).Student_ID;
            var walletmain = db.Students.FirstOrDefault(c => c.Student_ID == Mainid).Wallet;
            
            ViewBag.Mainid = Mainid;
            var d = db.Students.FirstOrDefault(c => c.Student_ID == Mainid);
            var sr = db.SubjectRegistrations.Where(a => a.Student_ID == Mainid && a.PaymentStatus == false).ToList();


            foreach (var tootalprice in sr)
            {

                t += (int)tootalprice.Price;



            }
            



            if (d.Wallet >= t)
            {
                foreach (var item in sr)
                {

                    item.PaymentStatus = true;

                }
                d.Wallet = d.Wallet - t;
                ViewBag.walletmain = d.Wallet;

                db.SaveChanges();





                return View("Mainpayment", sr);

            }
            else
            {

                ViewBag.notenogh = "Unfortunately, your account balance is currently insufficient to complete this transaction. ";
                ViewBag.walletmain = walletmain;

                return View("Mainpayment", sr);
            }


        }
    }
}