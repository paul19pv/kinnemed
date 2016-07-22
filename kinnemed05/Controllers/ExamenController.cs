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
    public class ExamenController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Examen/
        [CustomAuthorize(UserRoles.admin)]
        public ActionResult Index(string nombre,string area)
        {
            var examen = db.examen.Include(e => e.area);
            ViewBag.nombre = nombre;
            ViewBag.area = new SelectList(db.area.Where(a => a.are_tipo == "GRUPO"), "are_id", "are_nombre",area);

            if (!String.IsNullOrEmpty(nombre))
            {
                nombre = nombre.ToUpper();
                examen = examen.Where(e => e.exa_nombre.ToUpper().Contains(nombre));
            }
            if (!String.IsNullOrEmpty(area)) {
                int area_id = Int32.Parse(area);
                if (area_id != 0)
                    examen = examen.Where(e => e.exa_area.Equals(area_id));
            }
            
            examen = examen.Where(e => e.exa_estado == "ACTIVO");
            return View(examen.ToList());
        }

        //
        // GET: /Examen/Details/5
        [CustomAuthorize(UserRoles.admin)]
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
        [CustomAuthorize(UserRoles.admin)]
        public ActionResult Create()
        {
            ViewBag.exa_area = new SelectList(db.area.Where(a=>a.are_tipo=="GRUPO"), "are_id", "are_nombre");
            ViewBag.exa_tipo = "CUANTITATIVO";
            ViewBag.exa_estado = "ACTIVO";
            return View();
        }

        //
        // POST: /Examen/Create
        [CustomAuthorize(UserRoles.admin)]
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

            ViewBag.exa_area = new SelectList(db.area.Where(a=>a.are_tipo=="GRUPO"), "are_id", "are_nombre",examen.exa_area);
            ViewBag.exa_tipo = "CUANTITATIVO";
            ViewBag.exa_estado = "ACTIVO";
            return View(examen);
        }

        //
        // GET: /Examen/Edit/5
        [CustomAuthorize(UserRoles.admin)]
        public ActionResult Edit(int id = 0)
        {
            examen examen = db.examen.Find(id);
            if (examen == null)
            {
                return HttpNotFound();
            }
            ViewBag.exa_area = new SelectList(db.area.Where(a => a.are_tipo == "GRUPO"), "are_id", "are_nombre", examen.exa_area);
            return View(examen);
        }

        //
        // POST: /Examen/Edit/5
        [CustomAuthorize(UserRoles.admin)]
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
            ViewBag.exa_area = new SelectList(db.area.Where(a => a.are_tipo == "GRUPO"), "are_id", "are_nombre", examen.exa_area);
            return View(examen);
        }

        //
        // GET: /Examen/Delete/5
        [CustomAuthorize(UserRoles.admin)]
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
        [CustomAuthorize(UserRoles.admin)]
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