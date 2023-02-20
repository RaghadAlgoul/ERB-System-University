using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using University_Registration.Models;

namespace University_Registration.Controllers
{
    public class SubjectRegistrationsAdminController : Controller
    {
        private ERP_SystemEntities1 db = new ERP_SystemEntities1();
        [Authorize(Roles = "Admin")]

        // GET: SubjectRegistrationsAdmin
        public ActionResult Index()
        {


            var SubjectRegistrations = db.SubjectRegistrations.Include(o => o.Section).Include(o => o.Student).Include(o => o.Subject).Where(o => o.PaymentStatus == true).ToList();
            db.SaveChanges();
            return View("Index", SubjectRegistrations);


        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index(string search)
        {


            var SubjectRegistrations = db.SubjectRegistrations.Include(o => o.Section).Include(o => o.Student).Include(o => o.Subject).Where(o => o.PaymentStatus == true&&o.Student.Name.Contains(search)).ToList();
            db.SaveChanges();
            return View("Index", SubjectRegistrations);


        }




        public ActionResult Index2()
        {

            var SubjectRegistrations = db.SubjectRegistrations.Include(o => o.Section).Include(o => o.Student).Include(o => o.Subject).Where(o => o.PaymentStatus == false);

            return View(SubjectRegistrations.ToList());

        }


        [HttpPost]
        [ActionName("Index2")]
        public ActionResult Index4(string search)
        {

        var SubjectRegistrations = db.SubjectRegistrations.Include(o => o.Section).Include(o => o.Student).Include(o => o.Subject).Where(o => o.PaymentStatus == false&&o.Student.Name.Contains(search));

          return View(SubjectRegistrations.ToList());

        }
        // GET: SubjectRegistrations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectRegistration subjectRegistration = db.SubjectRegistrations.Find(id);
            if (subjectRegistration == null)
            {
                return HttpNotFound();
            }
            return View(subjectRegistration);
        }

        // GET: SubjectRegistrations/Create
        public ActionResult Create()
        {
            ViewBag.Section_ID = new SelectList(db.Sections, "Section_ID", "SectionDay");
            ViewBag.Student_ID = new SelectList(db.Students, "Student_ID", "Id");
            ViewBag.Subject_ID = new SelectList(db.Subjects, "Subject_ID", "Name");
            return View();
        }

        // POST: SubjectRegistrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubjectRegistrations_ID,Student_ID,Subject_ID,PaymentStatus,Section_ID,Price")] SubjectRegistration subjectRegistration)
        {
            if (ModelState.IsValid)
            {
                db.SubjectRegistrations.Add(subjectRegistration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Section_ID = new SelectList(db.Sections, "Section_ID", "SectionDay", subjectRegistration.Section_ID);
            ViewBag.Student_ID = new SelectList(db.Students, "Student_ID", "Id", subjectRegistration.Student_ID);
            ViewBag.Subject_ID = new SelectList(db.Subjects, "Subject_ID", "Name", subjectRegistration.Subject_ID);
            return View(subjectRegistration);
        }

        // GET: SubjectRegistrations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectRegistration subjectRegistration = db.SubjectRegistrations.Find(id);
            if (subjectRegistration == null)
            {
                return HttpNotFound();
            }
            ViewBag.Section_ID = new SelectList(db.Sections, "Section_ID", "SectionDay", subjectRegistration.Section_ID);
            ViewBag.Student_ID = new SelectList(db.Students, "Student_ID", "Id", subjectRegistration.Student_ID);
            ViewBag.Subject_ID = new SelectList(db.Subjects, "Subject_ID", "Name", subjectRegistration.Subject_ID);
            return View(subjectRegistration);
        }

        // POST: SubjectRegistrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubjectRegistrations_ID,Student_ID,Subject_ID,PaymentStatus,Section_ID,Price")] SubjectRegistration subjectRegistration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subjectRegistration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Section_ID = new SelectList(db.Sections, "Section_ID", "SectionDay", subjectRegistration.Section_ID);
            ViewBag.Student_ID = new SelectList(db.Students, "Student_ID", "Id", subjectRegistration.Student_ID);
            ViewBag.Subject_ID = new SelectList(db.Subjects, "Subject_ID", "Name", subjectRegistration.Subject_ID);
            return View(subjectRegistration);
        }

        // GET: SubjectRegistrations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectRegistration subjectRegistration = db.SubjectRegistrations.Find(id);
            if (subjectRegistration == null)
            {
                return HttpNotFound();
            }
            return View(subjectRegistration);
        }

        // POST: SubjectRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                SubjectRegistration subjectRegistration = db.SubjectRegistrations.Find(id);
                db.SubjectRegistrations.Remove(subjectRegistration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex) { 

                return View("ErrorAdmin");
            }
        
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
