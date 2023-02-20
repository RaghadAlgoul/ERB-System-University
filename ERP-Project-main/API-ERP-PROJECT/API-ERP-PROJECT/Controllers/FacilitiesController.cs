using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using API_ERP_PROJECT.Models;

namespace API_ERP_PROJECT.Controllers
{
    public class FacilitiesController : ApiController
    {
        private ERP_SystemEntities db = new ERP_SystemEntities();

        // GET: api/Facilities
        public IQueryable<Facility> GetFacilities()
        {
            return db.Facilities;
        }

        // GET: api/Facilities/5
        [ResponseType(typeof(Facility))]
        public IHttpActionResult GetFacility(int id)
        {
            Facility facility = db.Facilities.Find(id);
            if (facility == null)
            {
                return NotFound();
            }

            return Ok(facility);
        }

        // PUT: api/Facilities/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFacility(int id, Facility facility)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != facility.Facility_ID)
            {
                return BadRequest();
            }

            db.Entry(facility).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacilityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Facilities
        [ResponseType(typeof(Facility))]
        public IHttpActionResult PostFacility(Facility facility)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Facilities.Add(facility);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = facility.Facility_ID }, facility);
        }

        // DELETE: api/Facilities/5
        [ResponseType(typeof(Facility))]
        public IHttpActionResult DeleteFacility(int id)
        {
            Facility facility = db.Facilities.Find(id);
            if (facility == null)
            {
                return NotFound();
            }

            db.Facilities.Remove(facility);
            db.SaveChanges();

            return Ok(facility);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FacilityExists(int id)
        {
            return db.Facilities.Count(e => e.Facility_ID == id) > 0;
        }
    }
}