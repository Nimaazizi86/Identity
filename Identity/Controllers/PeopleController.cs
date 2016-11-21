using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Identity.Models;
using System.Web.Security;

namespace Identity.Controllers
{
    public class PeopleController : Controller
    {
        private PersonContext db = new PersonContext();

        // GET: People
        public ActionResult Index()
        {

            var people = db.People.Include(p => p.city).Include(p => p.country);
            return View(people.ToList());
        }

        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            // prepopulat cities and countries for the view dropdown
            ViewBag.cityId = new SelectList(db.Cities, "ID", "Name");
            ViewBag.countryId = new SelectList(db.Countries, "ID", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        // bind the person information with the editor of the field(creater)
        public ActionResult Create([Bind(Include = "ID,Name,PersonalNumber,countryId,cityId,creater")] Person person)
        {
            if (ModelState.IsValid)
            {
                // save the name of the current user in creater
                person.creater = System.Web.HttpContext.Current.User.Identity.Name.ToString();
                db.People.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // prepopulat cities and countries for the view dropdown
            ViewBag.cityId = new SelectList(db.Cities, "ID", "Name", person.cityId);
            ViewBag.countryId = new SelectList(db.Countries, "ID", "Name", person.countryId);
            return View(person);
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            // prepopulat cities and countries for the view dropdown
            ViewBag.cityId = new SelectList(db.Cities, "ID", "Name", person.cityId);
            ViewBag.countryId = new SelectList(db.Countries, "ID", "Name", person.countryId);
            return View(person);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,PersonalNumber,countryId,cityId,creater")] Person person)
        {
            if (ModelState.IsValid)
            {
                // save the name of the current user in creater
                person.creater = System.Web.HttpContext.Current.User.Identity.Name.ToString();
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // prepopulat cities and countries for the view dropdown
            ViewBag.cityId = new SelectList(db.Cities, "ID", "Name", person.cityId);
            ViewBag.countryId = new SelectList(db.Countries, "ID", "Name", person.countryId);
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.People.Find(id);
            db.People.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index");
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
