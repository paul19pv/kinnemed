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
    public class OftalmologiaController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Oftalmologia/

        public ActionResult Index()
        {
            var oftalmologia = db.oftalmologia.Include(o => o.paciente);
            return View(oftalmologia.ToList());
        }

        //
        // GET: /Oftalmologia/Details/5

        public ActionResult Details(int id = 0)
        {
            oftalmologia oftalmologia = db.oftalmologia.Find(id);
            if (oftalmologia == null)
            {
                return HttpNotFound();
            }
            return View(oftalmologia);
        }

        //
        // GET: /Oftalmologia/Create

        public ActionResult Create()
        {
            ViewBag.oft_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula");
            return View();
        }

        //
        // POST: /Oftalmologia/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(oftalmologia oftalmologia)
        {
            if (ModelState.IsValid)
            {
                db.oftalmologia.Add(oftalmologia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.oft_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula", oftalmologia.oft_paciente);
            return View(oftalmologia);
        }

        //
        // GET: /Oftalmologia/Edit/5

        public ActionResult Edit(int id = 0)
        {
            oftalmologia oftalmologia = db.oftalmologia.Find(id);
            if (oftalmologia == null)
            {
                return HttpNotFound();
            }
            ViewBag.oft_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula", oftalmologia.oft_paciente);
            return View(oftalmologia);
        }

        //
        // POST: /Oftalmologia/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(oftalmologia oftalmologia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oftalmologia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.oft_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula", oftalmologia.oft_paciente);
            return View(oftalmologia);
        }

        //
        // GET: /Oftalmologia/Delete/5

        public ActionResult Delete(int id = 0)
        {
            oftalmologia oftalmologia = db.oftalmologia.Find(id);
            if (oftalmologia == null)
            {
                return HttpNotFound();
            }
            return View(oftalmologia);
        }

        //
        // POST: /Oftalmologia/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            oftalmologia oftalmologia = db.oftalmologia.Find(id);
            db.oftalmologia.Remove(oftalmologia);
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