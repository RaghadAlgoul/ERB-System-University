using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University_Registration.Models;

namespace University_Registration.Controllers
{

    public class StudentController : Controller
    {
        ERP_SystemEntities1 university = new ERP_SystemEntities1();

        // GET: Student
        [Authorize(Roles="User")]
        public ActionResult StudentDashBoard()
        {
            string ASPid=User.Identity.GetUserId();
            Session["id"] = User.Identity.GetUserId();
            var Mainid = university.Students.FirstOrDefault(c => c.Id == ASPid).Student_ID;
            ViewBag.MainId= university.Students.FirstOrDefault(c => c.Id == ASPid).Student_ID;
            var MainMajor = university.Students.FirstOrDefault(c => c.Student_ID == Mainid).Major_ID;
            ViewBag.id=User.Identity.GetUserId();   
            var courses = university.Subjects.Where(c=>c.Major_ID== MainMajor).ToList();
            var info = university.SubjectRegistrations.Where(c => c.Student_ID== Mainid).ToList();
            
            
            return View(Tuple.Create(courses, info));
        }
        public ActionResult table()
        {
            return View();
        }

        public ActionResult sections()
        { Subject sub1 = new Subject();
            string ASPid = User.Identity.GetUserId();
            var Mainid = university.Students.FirstOrDefault(c => c.Id == ASPid).Student_ID;

            var x = university.Students.FirstOrDefault(c => c.Student_ID == Mainid).Major_ID;
            var courses = university.Subjects.Where(c => c.Major_ID == x).ToList();
            //var sub = university.Majors.Where(c => c.Major_ID == sub1.Major_ID);
            var section = university.Sections.Where(c => c.Subject.Major_ID==x ).ToList();
            return View(Tuple.Create(section, courses));
        }
        
        public ActionResult Add(int ADD)
        {
            SubjectRegistration subject = new SubjectRegistration();


            string ASPid = User.Identity.GetUserId();
            var Mainid = university.Students.FirstOrDefault(c => c.Id == ASPid).Student_ID;

            int subjectID =Convert.ToInt32(university.Sections.Find(ADD).Subject_ID);
            string day =university.Sections.Find(ADD).SectionDay;
            int majorPrice = Convert.ToInt32(university.Students.FirstOrDefault(c => c.Student_ID == Mainid).Major.price);
            int TotalHours = Convert.ToInt32(university.Sections.FirstOrDefault(c => c.Section_ID == ADD).Subject.NumHour);
            int TotalPrice = majorPrice * TotalHours; 

            int sectionTime = Convert.ToInt32(university.Sections.Find(ADD).SectionTime);
            var sss = university.SubjectRegistrations.Find(ADD);
            bool f = true;
            if (ModelState.IsValid)
            {
                foreach (var x in university.SubjectRegistrations.ToList())
                {
                  
                    if (Mainid == x.Student.Student_ID && subjectID == x.Subject_ID)
                    {
                        

                        f = false;
                      
                        break;
                    }
                    else if (Mainid == x.Student.Student_ID  && sectionTime == x.Section.SectionTime && day== x.Section.SectionDay)
                    {
                       
                        Console.WriteLine("Hello, world22!");
                        f = false;
                        break;
                    }
                }
                if (f == true) {
                    subject.Price = TotalPrice;
                subject.PaymentStatus = false;
                subject.Subject_ID = subjectID;
                subject.Student_ID = Mainid;
                subject.Section_ID = ADD;
                university.SubjectRegistrations.Add(subject);
                university.SaveChanges();
                }

                return RedirectToAction("StudentDashBoard");

            }
            return View(subject);


        }

        public ActionResult del(int ID)
        {
            if (ID != null)
            {
                SubjectRegistration registration = university.SubjectRegistrations.Find(ID);
                university.SubjectRegistrations.Remove(registration);
                university.SaveChanges();
                return RedirectToAction("StudentDashBoard");
            }
            return RedirectToAction("StudentDashBoard");

        }
        [HttpPost]
        public ActionResult EditOne(string v1,string v2,string v3 , HttpPostedFileBase pic)
        {
            HttpPostedFileBase pic1 = Request.Files["pic"];
            string ASPid = User.Identity.GetUserId();
            var Mainid = university.Students.FirstOrDefault(c => c.Id == ASPid).Student_ID;
            var studentEdit = university.Students.Where(b => b.Student_ID == Mainid);
            foreach(var item in studentEdit)
            {
                if(v1 !="" ||  v1 != string.Empty)
                {
                    item.Name = v1;
                }
                
                if (v3 != "" || v3 != string.Empty)
                {
                    item.PhoneNumber = v3;
                }
                if (pic != null && pic.ContentLength > 0)
                {
                    string path = "../Content/img/" + Path.GetFileName(pic.FileName);
                    pic.SaveAs(Server.MapPath(path));
                    item.Pic = "../Content/img/" + Path.GetFileName(pic.FileName);
                    // do something with the filename
                }



                break;

            }
            university.SaveChanges();
            return RedirectToAction("StudentDashBoard");

        }


        public PartialViewResult _Oprofileuser()
        {
            string ASPid = User.Identity.GetUserId();
            var Mainid = university.Students.FirstOrDefault(c => c.Id == ASPid).Student_ID;

            var x = university.Students.FirstOrDefault(c => c.Student_ID == Mainid).Major_ID;
            var info = university.Students.Where(c => c.Major_ID == x).ToList();
            return PartialView("_Oprofileuser", info);
        }

        public PartialViewResult _showSubject()
        {
            ViewBag.STUDENTID = User.Identity.GetUserId();
            string ASPid = User.Identity.GetUserId();
            var Mainid = university.Students.FirstOrDefault(c => c.Id == ASPid).Student_ID;

            var x = university.Students.FirstOrDefault(c => c.Student_ID == Mainid).Major_ID;
            var showSubject = university.Sections.Where(c => c.Subject.Major_ID == x).ToList();
            return PartialView("_showSubject", showSubject);
        }
        public ActionResult STDform()
        {
            ViewBag.Id = new SelectList(university.AspNetUsers, "Id", "Email");
            ViewBag.Major_ID = new SelectList(university.Majors, "Major_ID", "Name");
            //string pass = Session["UserPassword"].ToString();
            return View();
        }
        [HttpPost]
        public ActionResult STDform([Bind(Include = "Student_ID,Id,Name,Email,Password,NationalNum,Grad,Pic,Status,PhoneNumber,BirthFile,PersonalIdFile,CertificateFile,Gender,Major_ID,Wallet")] Student student, HttpPostedFileBase Pic1, HttpPostedFileBase PersonalIdFile1, HttpPostedFileBase CertificateFile1, HttpPostedFileBase BirthFile1)
        {

            if (ModelState.IsValid)
            {
                if (Pic1 != null)
                {
                    //string fileName = Path.GetFileName(image.FileName);
                    string path = Server.MapPath("~/Images/") + Pic1.FileName;
                    Pic1.SaveAs(path);
                    student.Pic = Pic1.FileName;
                }

                if (PersonalIdFile1 != null)
                {
                    //string fileName = Path.GetFileName(cv.FileName);
                    string path = Server.MapPath("~/FormalFile/") + PersonalIdFile1.FileName;
                    PersonalIdFile1.SaveAs(path);
                    student.PersonalIdFile = PersonalIdFile1.FileName;
                }
                if (CertificateFile1 != null)
                {
                    //string fileName = Path.GetFileName(cv.FileName);
                    string path = Server.MapPath("~/FormalFile/") + CertificateFile1.FileName;
                    CertificateFile1.SaveAs(path);
                    student.CertificateFile = CertificateFile1.FileName;
                }

                if (BirthFile1 != null)
                {
                    //string fileName = Path.GetFileName(cv.FileName);
                    string path = Server.MapPath("~/FormalFile/") + BirthFile1.FileName;
                    BirthFile1.SaveAs(path);
                    student.BirthFile = BirthFile1.FileName;
                }
                student.Password = Session["UserPassword"].ToString();
                student.Email = Session["UserEmail"].ToString();
                student.Id = Session["UserID"].ToString();
                student.Wallet = 0;
                university.Students.Add(student);
                university.SaveChanges();
                return RedirectToAction("Index", "mainHome");
            }

            ViewBag.Id = new SelectList(university.AspNetUsers, "Id", "Email", student.Id);
            ViewBag.Major_ID = new SelectList(university.Majors, "Major_ID", "Name", student.Major_ID);
            return View("STDform");
        }
        // GET: StudentForm
        public ActionResult Index()
        {
            return View();
        }
    }
}
