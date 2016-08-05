using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kinnemed05.Models;

namespace kinnemed05.Controllers
{
    public class EnfermedadController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Enfermedad/

        public ActionResult Index()
        {
            var enfermedad = db.enfermedad.Include(e => e.paciente);
            return View(enfermedad.ToList());
        }

        //
        // GET: /Enfermedad/Details/5

        public ActionResult Details(int id = 0)
        {
            enfermedad enfermedad = db.enfermedad.Find(id);
            if (enfermedad == null)
            {
                return HttpNotFound();
            }
            return View(enfermedad);
        }

        //
        // GET: /Enfermedad/Create

        public ActionResult Create()
        {
            ViewBag.enf_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula");
            return View();
        }

        //
        // POST: /Enfermedad/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(enfermedad enfermedad)
        {
            if (ModelState.IsValid)
            {
                db.enfermedad.Add(enfermedad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.enf_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula", enfermedad.enf_paciente);
            return View(enfermedad);
        }

        //
        // GET: /Enfermedad/Edit/5

        public ActionResult Edit(int id = 0)
        {
            enfermedad enfermedad = db.enfermedad.Find(id);
            if (enfermedad == null)
            {
                return HttpNotFound();
            }
            ViewBag.enf_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula", enfermedad.enf_paciente);
            return View(enfermedad);
        }

        //
        // POST: /Enfermedad/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(enfermedad enfermedad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enfermedad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.enf_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula", enfermedad.enf_paciente);
            return View(enfermedad);
        }

        //
        // GET: /Enfermedad/Delete/5

        public ActionResult Delete(int id = 0)
        {
            enfermedad enfermedad = db.enfermedad.Find(id);
            if (enfermedad == null)
            {
                return HttpNotFound();
            }
            return View(enfermedad);
        }

        //
        // POST: /Enfermedad/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            enfermedad enfermedad = db.enfermedad.Find(id);
            db.enfermedad.Remove(enfermedad);
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