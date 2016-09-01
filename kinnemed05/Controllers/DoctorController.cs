using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kinnemed05.Models;
using kinnemed05.Security;

namespace kinnemed05.Controllers
{
    [CustomAuthorize(UserRoles.admin)]
    public class DoctorController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();
        private UsersContext db_user = new UsersContext();
        //
        // GET: /Doctor/

        public ActionResult Index()
        {
            var doctor = db.doctor.Include(d => d.empresa);
            return View(doctor.ToList());
        }

        //
        // GET: /Doctor/Details/5

        public ActionResult Details(int id = 0)
        {
            doctor doctor = db.doctor.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        //
        // GET: /Doctor/Create

        public ActionResult Create()
        {
            ViewBag.doc_empresa = new SelectList(db.empresa, "emp_id", "emp_cedula");
            return View();
        }

        //
        // POST: /Doctor/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.doctor.Add(doctor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.doc_empresa = new SelectList(db.empresa, "emp_id", "emp_cedula", doctor.doc_empresa);
            return View(doctor);
        }

        //
        // GET: /Doctor/Edit/5

        public ActionResult Edit(int id = 0)
        {
            doctor doctor = db.doctor.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            ViewBag.doc_empresa = new SelectList(db.empresa, "emp_id", "emp_cedula", doctor.doc_empresa);
            return View(doctor);
        }

        //
        // POST: /Doctor/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.doc_empresa = new SelectList(db.empresa, "emp_id", "emp_cedula", doctor.doc_empresa);
            return View(doctor);
        }

        //
        // GET: /Doctor/Delete/5

        public ActionResult Delete(int id = 0)
        {
            doctor doctor = db.doctor.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        //
        // POST: /Doctor/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            doctor doctor = db.doctor.Find(id);
            db.doctor.Remove(doctor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}