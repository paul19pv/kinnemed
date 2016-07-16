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
    public class ExamenController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Examen/

        public ActionResult Index()
        {
            var examen = db.examen.Include(e => e.area);
            return View(examen.ToList());
        }

        //
        // GET: /Examen/Details/5

        public ActionResult Details(int id = 0)
        {
            examen examen = db.examen.Find(id);
            if (examen == null)
            {
                return HttpNotFound();
            }
            return View(examen);
        }

        //
        // GET: /Examen/Create

        public ActionResult Create()
        {
            ViewBag.exa_area = new SelectList(db.area, "are_id", "are_nombre");
            return View();
        }

        //
        // POST: /Examen/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(examen examen)
        {
            if (ModelState.IsValid)
            {
                db.examen.Add(examen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.exa_area = new SelectList(db.area, "are_id", "are_nombre", examen.exa_area);
            return View(examen);
        }

        //
        // GET: /Examen/Edit/5

        public ActionResult Edit(int id = 0)
        {
            examen examen = db.examen.Find(id);
            if (examen == null)
            {
                return HttpNotFound();
            }
            ViewBag.exa_area = new SelectList(db.area, "are_id", "are_nombre", examen.exa_area);
            return View(examen);
        }

        //
        // POST: /Examen/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(examen examen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(examen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.exa_area = new SelectList(db.area, "are_id", "are_nombre", examen.exa_area);
            return View(examen);
        }

        //
        // GET: /Examen/Delete/5

        public ActionResult Delete(int id = 0)
        {
            examen examen = db.examen.Find(id);
            if (examen == null)
            {
                return HttpNotFound();
            }
            return View(examen);
        }

        //
        // POST: /Examen/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            examen examen = db.examen.Find(id);
            db.examen.Remove(examen);
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