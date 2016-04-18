using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kinnemed05.Models;
using kinnemed05.Security;
using kinnemed05.Filters;

namespace kinnemed05.Controllers
{
    [InitializeSimpleMembership]
    //[CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
    public class ControlController : Controller
    {
        
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Control/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Index(int id)
        {
            var control = db.control.Include(c => c.examen).Include(c => c.perfil).Where(c=>c.con_perfil==id);
            return PartialView(control.ToList());
        }

        //
        // GET: /Control/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Details(int id = 0)
        {
            control control = db.control.Find(id);
            if (control == null)
            {
                return HttpNotFound();
            }
            return View(control);
        }

        //
        // GET: /Control/Create
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        public ActionResult Create(int id)
        {
            //ViewBag.con_examen = new SelectList(db.examen, "exa_id", "exa_nombre");
            ViewBag.per_id = id;
            //ViewBag.con_perfil = new SelectList(db.perfil, "per_id", "per_nombre");
            return PartialView();
        }

        //
        // POST: /Control/Create
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(control control)
        {
            if (ModelState.IsValid)
            {
                db.control.Add(control);
                db.SaveChanges();
                return RedirectToAction("Index", new { id=control.con_perfil });
            }

            ViewBag.per_id = control.con_perfil;
            return PartialView(control);
        }

        //
        // GET: /Control/Edit/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        public ActionResult Edit(int id = 0)
        {
            control control = db.control.Find(id);
            if (control == null)
            {
                return HttpNotFound();
            }
            ViewBag.con_examen = new SelectList(db.examen, "exa_id", "exa_nombre", control.con_examen);
            ViewBag.con_perfil = new SelectList(db.perfil, "per_id", "per_nombre", control.con_perfil);
            return View(control);
        }

        //
        // POST: /Control/Edit/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(control control)
        {
            if (ModelState.IsValid)
            {
                db.Entry(control).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.con_examen = new SelectList(db.examen, "exa_id", "exa_nombre", control.con_examen);
            ViewBag.con_perfil = new SelectList(db.perfil, "per_id", "per_nombre", control.con_perfil);
            return View(control);
        }

        //
        // GET: /Control/Delete/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        public ActionResult Delete(int id = 0)
        {
            control control = db.control.Find(id);
            db.control.Remove(control);
            db.SaveChanges();
            return RedirectToAction("Index", new { id=control.con_perfil });
        }

        //
        // POST: /Control/Delete/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            control control = db.control.Find(id);
            db.control.Remove(control);
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