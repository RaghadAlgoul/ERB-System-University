using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using University_Registration.Models;

namespace University_Registration.Controllers
{
    public class MajorsAdminController : Controller
    {
        private ERP_SystemEntities1 db = new ERP_SystemEntities1();
        [Authorize(Roles = "Admin")]

        // GET: MajorsAdmin
        public ActionResult Index()
        {

            var majors = db.Majors.Include(m => m.Facility);
            return View(majors.ToList());
        }
        [ActionName("Index")]
        [HttpPost]
        public ActionResult Index2(string search)
        {
            var majors = db.Majors.Where(p=>p.Name.Contains(search)).Include(m => m.Facility);
            return View(majors);
        }

        // GET: MajorsAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Major major = db.Majors.Find(id);
            if (major == null)
            {
                return HttpNotFound();
            }
            return View(major);
        }

        // GET: MajorsAdmin/Create
        public ActionResult Create()
        {
            ViewBag.Facility_ID = new SelectList(db.Facilities, "Facility_ID", "Name");
            return View();
        }

        // POST: MajorsAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Major_ID,Name,Description,Major_Image,Facility_ID,price")] Major major, HttpPostedFileBase picture)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    try
                    {
                        string fileName = Path.GetFileName(picture.FileName);
                        string path = "~/Images/" + Path.GetFileName(picture.FileName);
                        string path2 = Path.GetFileName(picture.FileName);
                        picture.SaveAs(Server.MapPath(path));
                        major.Major_Image = path2.ToString();
                        Session["image"] = path2.ToString();
                        int flag = 0;
                        foreach (var item in db.Majors)
                        {

                            if (major.Name == item.Name)
                            {
                                ViewBag.alert = "You can not add the Major is alerady exist";
                                flag = 1;

                            }

                        }
                        if (flag == 0)
                        {
                            db.Majors.Add(major);
                            db.SaveChanges();
                            return RedirectToAction("Index");

                        }
                    }
                    catch (Exception ex)
                    {

                        major.Major_Image = Session["image"].ToString();
                        db.Majors.Add(major);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                }
                ViewBag.Facility_ID = new SelectList(db.Facilities, "Facility_ID", "Name", major.Facility_ID);
                return View(major);
            }
            catch(Exception ex) {
                ViewBag.alert = "You Must Upload the image";
                ViewBag.Facility_ID = new SelectList(db.Facilities, "Facility_ID", "Name", major.Facility_ID);
                return View(major);


            }
        }

        // GET: MajorsAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Major major = db.Majors.Find(id);
            if (major == null)
            {
                return HttpNotFound();
            }
            ViewBag.Facility_ID = new SelectList(db.Facilities, "Facility_ID", "Name", major.Facility_ID);
            return View(major);
        }

        // POST: MajorsAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,[Bind(Include = "Major_ID,Name,Description,Major_Image,Facility_ID,price")] Major major, HttpPostedFileBase picture)
        {

            if (ModelState.IsValid)
            {
                var existingModel = db.Majors.AsNoTracking().FirstOrDefault(x => x.Major_ID == id);

                if (picture != null)
                {
                    string path = "~/Images/" + Path.GetFileName(picture.FileName);
                    picture.SaveAs(Server.MapPath(path));
                    major.Major_Image = picture.FileName;


                }
                else
                {
                    major.Major_Image= existingModel.Major_Image;
                }
                db.Entry(major).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Facility_ID = new SelectList(db.Facilities, "Facility_ID", "Name", major.Facility_ID);
            return View(major);
        }

        // GET: MajorsAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Major major = db.Majors.Find(id);
            if (major == null)
            {
                return HttpNotFound();
            }
            return View(major);
        }

        // POST: MajorsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Major major = db.Majors.Find(id);
                db.Majors.Remove(major);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception err) {
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
