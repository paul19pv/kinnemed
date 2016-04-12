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
    public class InmunizacionController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Inmunizacion/

        public ActionResult Index(int id)
        {
            var inmunizacion = db.inmunizacion.Include(i => i.paciente).Include(i => i.vacuna);
            inmunizacion = inmunizacion.Where(i => i.inm_paciente == id);
            return PartialView(inmunizacion.ToList());
        }

        //
        // GET: /Inmunizacion/Details/5

        public ActionResult Details(int id = 0)
        {
            inmunizacion inmunizacion = db.inmunizacion.Find(id);
            if (inmunizacion == null)
            {
                return HttpNotFound();
            }
            return View(inmunizacion);
        }

        //
        // GET: /Inmunizacion/Create

        public ActionResult Create()
        {
            ViewBag.inm_paciente = Session["pac_id"];
            ViewBag.inm_vacuna = new SelectList(db.vacuna, "vac_id", "vac_nombre");
            return PartialView();
        }

        //
        // POST: /Inmunizacion/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(inmunizacion inmunizacion)
        {
            if (ModelState.IsValid)
            {
                db.inmunizacion.Add(inmunizacion);
                db.SaveChanges();
                return RedirectToAction("Index", new {id=inmunizacion.inm_paciente });
            }

            ViewBag.inm_paciente = inmunizacion.inm_paciente;
            ViewBag.inm_vacuna = new SelectList(db.vacuna, "vac_id", "vac_nombre", inmunizacion.inm_vacuna);
            return PartialView(inmunizacion);
        }

        //
        // GET: /Inmunizacion/Edit/5

        public ActionResult Edit(int id = 0)
        {
            inmunizacion inmunizacion = db.inmunizacion.Find(id);
            if (inmunizacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.inm_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula", inmunizacion.inm_paciente);
            ViewBag.inm_vacuna = new SelectList(db.vacuna, "vac_id", "vac_nombre", inmunizacion.inm_vacuna);
            return View(inmunizacion);
        }

        //
        // POST: /Inmunizacion/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(inmunizacion inmunizacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inmunizacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.inm_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula", inmunizacion.inm_paciente);
            ViewBag.inm_vacuna = new SelectList(db.vacuna, "vac_id", "vac_nombre", inmunizacion.inm_vacuna);
            return View(inmunizacion);
        }

        //
        // GET: /Inmunizacion/Delete/5

        public ActionResult Delete(int id = 0)
        {
            inmunizacion inmunizacion = db.inmunizacion.Find(id);
            if (inmunizacion == null)
            {
                return HttpNotFound();
            }
            return View(inmunizacion);
        }

        //
        // POST: /Inmunizacion/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            inmunizacion inmunizacion = db.inmunizacion.Find(id);
            db.inmunizacion.Remove(inmunizacion);
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