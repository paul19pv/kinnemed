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
    public class FisicoController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Fisico/

        public ActionResult Index()
        {
            var fisico = db.fisico.Include(f => f.historia);
            return PartialView(fisico.ToList());
        }

        //
        // GET: /Fisico/Details/5

        public ActionResult Details(int id = 0)
        {
            fisico fisico = db.fisico.Find(id);
            if (fisico == null)
            {
                return HttpNotFound();
            }
            return PartialView(fisico);
        }

        //
        // GET: /Fisico/Create

        public ActionResult Create(int id)
        {
            ViewBag.fis_id = id;
            return PartialView();
        }

        //
        // POST: /Fisico/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(fisico fisico)
        {
            if (ModelState.IsValid)
            {
                db.fisico.Add(fisico);
                db.SaveChanges();
                return RedirectToAction("Create", "Diagnostico", new { id=fisico.fis_id});
            }

            ViewBag.fis_id = fisico.fis_id;
            return PartialView(fisico);
        }

        //
        // GET: /Fisico/Edit/5

        public ActionResult Edit(int id = 0)
        {
            fisico fisico = db.fisico.Find(id);
            if (fisico == null)
            {
                return RedirectToAction("Create", new { id=id});
            }
            ViewBag.fis_id = id;
            return PartialView(fisico);
        }

        //
        // POST: /Fisico/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(fisico fisico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fisico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create", "Diagnostico", new { id = fisico.fis_id });
            }
            ViewBag.fis_id = fisico.fis_id;
            return PartialView(fisico);
        }

        //
        // GET: /Fisico/Delete/5

        public ActionResult Delete(int id = 0)
        {
            fisico fisico = db.fisico.Find(id);
            if (fisico == null)
            {
                return HttpNotFound();
            }
            return View(fisico);
        }

        //
        // POST: /Fisico/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            fisico fisico = db.fisico.Find(id);
            db.fisico.Remove(fisico);
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