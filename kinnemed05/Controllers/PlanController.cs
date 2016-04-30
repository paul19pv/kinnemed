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
    public class PlanController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Plan/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Index()
        {
            var plan = db.plan.Include(p => p.historia);
            return View(plan.ToList());
        }

        //
        // GET: /Plan/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Details(int id = 0)
        {
            plan plan = db.plan.Find(id);
            if (plan == null)
            {
                return HttpNotFound();
            }
            return View(plan);
        }

        //
        // GET: /Plan/Create

        [CustomAuthorize(UserRoles.medico)]
        public ActionResult Create(int id)
        {
            plan plan = db.plan.Find(id);

            if (plan != null)
            {
                return RedirectToAction("Edit", new { id = id });
            }
            historia historia = db.historia.Find(id);
            ViewBag.pla_id = id;
            ViewBag.pac_id = historia.his_paciente;
            return PartialView();
        }

        //
        // POST: /Plan/Create

        [CustomAuthorize(UserRoles.medico)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(plan plan)
        {
            if (ModelState.IsValid)
            {
                db.plan.Add(plan);
                db.SaveChanges();
                //return RedirectToAction("Message", "Home", new { mensaje="Datos Guardados. El proceso ha finalizado."});
                return RedirectToAction("Create", "Inmunizacion");
            }

            ViewBag.pla_id = plan.pla_id;
            return PartialView(plan);
        }

        //
        // GET: /Plan/Edit/5

        [CustomAuthorize(UserRoles.medico, UserRoles.admin)]
        public ActionResult Edit(int id = 0)
        {
            plan plan = db.plan.Find(id);
            historia historia = db.historia.Find(id);

            if (plan == null)
            {
                return RedirectToAction("Create", new { id = id });
            }
            ViewBag.pla_id = plan.pla_id;
            ViewBag.pac_id = historia.his_paciente;
            return PartialView(plan);
        }

        //
        // POST: /Plan/Edit/5

        [CustomAuthorize(UserRoles.medico, UserRoles.admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(plan plan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(plan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create", "Inmunizacion");
            }
            ViewBag.pla_id = plan.pla_id;
            return PartialView(plan);
        }

        //
        // GET: /Plan/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin)]
        public ActionResult Delete(int id = 0)
        {
            plan plan = db.plan.Find(id);
            if (plan == null)
            {
                return HttpNotFound();
            }
            return View(plan);
        }

        //
        // POST: /Plan/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            plan plan = db.plan.Find(id);
            db.plan.Remove(plan);
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