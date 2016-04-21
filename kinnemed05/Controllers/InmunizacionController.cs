using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kinnemed05.Models;
using kinnemed05.Filters;
using kinnemed05.Security;

namespace kinnemed05.Controllers
{
    [InitializeSimpleMembership]
    //[CustomAuthorize(UserRoles.medico)]
    public class InmunizacionController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Inmunizacion/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Index(int id)
        {
            var inmunizacion = db.inmunizacion.Include(i => i.paciente).Include(i => i.vacuna);
            inmunizacion = inmunizacion.Where(i => i.inm_paciente == id);
            return PartialView(inmunizacion.ToList());
        }
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Consulta(int id)
        {
            var inmunizacion = db.inmunizacion.Include(i => i.paciente).Include(i => i.vacuna);
            inmunizacion = inmunizacion.Where(i => i.inm_paciente == id);
            return PartialView(inmunizacion.ToList());
        }

        //
        // GET: /Inmunizacion/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
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

        [CustomAuthorize(UserRoles.medico)]
        public ActionResult Create()
        {
            ViewBag.inm_paciente = Session["pac_id"];
            ViewBag.his_id = Session["his_id"];
            ViewBag.inm_vacuna = new SelectList(db.vacuna, "vac_id", "vac_nombre");
            int his_tipo = Convert.ToInt32(Session["his_tipo"]);
            if (his_tipo == 1)
                return RedirectToAction("Message", "Home", new { mensaje = "Proceso Finalizado" });
            return PartialView();
        }

        //
        // POST: /Inmunizacion/Create

        [CustomAuthorize(UserRoles.medico)]
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
        [CustomAuthorize(UserRoles.medico)]
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
        [CustomAuthorize(UserRoles.medico)]
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
        [CustomAuthorize(UserRoles.medico, UserRoles.admin)]
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
        [CustomAuthorize(UserRoles.medico, UserRoles.admin)]
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