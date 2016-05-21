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
    public class ReposoController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Reposo/

        public ActionResult Index()
        {
            var reposo = db.reposo.Include(r => r.historia);
            return View(reposo.ToList());
        }

        //
        // GET: /Reposo/Details/5

        public ActionResult Details(int id = 0)
        {
            reposo reposo = db.reposo.Find(id);
            if (reposo == null)
            {
                return HttpNotFound();
            }
            return View(reposo);
        }

        //
        // GET: /Reposo/Create

        public ActionResult Create(int id)
        {
            reposo reposo = db.reposo.Find(id);
            if (reposo != null)
            {
                return RedirectToAction("Edit", new { id = id });
            }
            ViewBag.rep_id = id;
            return PartialView();
        }

        //
        // POST: /Reposo/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(reposo reposo)
        {
            if (ModelState.IsValid)
            {
                db.reposo.Add(reposo);
                db.SaveChanges();
                return RedirectToAction("Message", "Home", new { mensaje="Datos Guardados"});
            }

            ViewBag.rep_id = reposo.rep_id;
            return PartialView(reposo);
        }

        //
        // GET: /Reposo/Edit/5

        public ActionResult Edit(int id = 0)
        {
            reposo reposo = db.reposo.Find(id);
            if (reposo == null)
            {
                return RedirectToAction("Create", new { id = id });
            }
            ViewBag.rep_id = reposo.rep_id;
            return PartialView(reposo);
        }

        //
        // POST: /Reposo/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(reposo reposo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reposo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Message", "Home", new { mensaje = "Datos Guardados" });
            }
            ViewBag.rep_id = reposo.rep_id;
            return PartialView(reposo);
        }

        //
        // GET: /Reposo/Delete/5

        public ActionResult Delete(int id = 0)
        {
            reposo reposo = db.reposo.Find(id);
            if (reposo == null)
            {
                return HttpNotFound();
            }
            return View(reposo);
        }

        //
        // POST: /Reposo/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            reposo reposo = db.reposo.Find(id);
            db.reposo.Remove(reposo);
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