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
    public class AccidenteController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Accidente/

        public ActionResult Index()
        {
            var accidente = db.accidente.Include(a => a.paciente);
            return View(accidente.ToList());
        }

        //
        // GET: /Accidente/Details/5

        public ActionResult Details(int id = 0)
        {
            accidente accidente = db.accidente.Find(id);
            if (accidente == null)
            {
                return HttpNotFound();
            }
            return View(accidente);
        }

        //
        // GET: /Accidente/Create

        public ActionResult Create()
        {
            ViewBag.acc_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula");
            return View();
        }

        //
        // POST: /Accidente/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(accidente accidente)
        {
            if (ModelState.IsValid)
            {
                db.accidente.Add(accidente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.acc_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula", accidente.acc_paciente);
            return View(accidente);
        }

        //
        // GET: /Accidente/Edit/5

        public ActionResult Edit(int id = 0)
        {
            accidente accidente = db.accidente.Find(id);
            if (accidente == null)
            {
                return HttpNotFound();
            }
            ViewBag.acc_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula", accidente.acc_paciente);
            return View(accidente);
        }

        //
        // POST: /Accidente/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(accidente accidente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accidente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.acc_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula", accidente.acc_paciente);
            return View(accidente);
        }

        //
        // GET: /Accidente/Delete/5

        public ActionResult Delete(int id = 0)
        {
            accidente accidente = db.accidente.Find(id);
            if (accidente == null)
            {
                return HttpNotFound();
            }
            return View(accidente);
        }

        //
        // POST: /Accidente/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            accidente accidente = db.accidente.Find(id);
            db.accidente.Remove(accidente);
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