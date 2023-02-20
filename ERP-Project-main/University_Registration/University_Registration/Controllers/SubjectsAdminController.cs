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
    public class SubjectsAdminController : Controller
    {
        private ERP_SystemEntities1 db = new ERP_SystemEntities1();
        [Authorize(Roles = "Admin")]

        // GET: SubjectsAdmin
        public ActionResult Index()
        {
            var subjects = db.Subjects.Include(s => s.Major);
            return View(subjects.ToList());
        }

        [ActionName("Index")]
        [HttpPost]
        public ActionResult Index2(string search)
        {
            var subjects = db.Subjects.Where(n=>n.Name.Contains(search)).Include(s => s.Major);
            return View(subjects.ToList());
        }

        // GET: SubjectsAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // GET: SubjectsAdmin/Create
        public ActionResult Create()
        {
            ViewBag.Major_ID = new SelectList(db.Majors, "Major_ID", "Name");
            return View();
        }

        // POST: SubjectsAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Subject_ID,Name,Description,Major_ID,NumHour,SubjectView")] Subject subject, string Major_ID, string courseName)
        {
            bool flag = true;
            var mainMajorId = Convert.ToInt32(Major_ID);

            var existcourses = db.Subjects.Where(c => c.Major_ID == mainMajorId).ToList();
            if (ModelState.IsValid)
            {
                foreach (var item in existcourses)
                {
                    if (item.Name == courseName && item.Major_ID == mainMajorId)
                    {
                        flag = false;
                        ViewBag.courserepeat = "you have ths course in this major";
                        break;
                    }

                }
                if(flag == true)
                {
                    subject.Name= courseName;
                    subject.Major_ID= mainMajorId;
                    subject.Subject_ID = Convert.ToInt32(Session["id"]);
                    db.Subjects.Add(subject);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }

            ViewBag.Major_ID = new SelectList(db.Majors, "Major_ID", "Name", subject.Major_ID);
            return View(subject);
        }

        // GET: SubjectsAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            ViewBag.Major_ID = new SelectList(db.Majors, "Major_ID", "Name", subject.Major_ID);
            return View(subject);
        }

        // POST: SubjectsAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Subject_ID,Name,Description,Major_ID,NumHour,SubjectView")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Major_ID = new SelectList(db.Majors, "Major_ID", "Name", subject.Major_ID);
            return View(subject);
        }

        // GET: SubjectsAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // POST: SubjectsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {

                Subject subject = db.Subjects.Find(id);
                db.Subjects.Remove(subject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
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
