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
    public class SignosController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Signos/

        public ActionResult Index()
        {
            var signos = db.signos.Include(s => s.historia);
            return View(signos.ToList());
        }

        //
        // GET: /Signos/Details/5

        public ActionResult Details(int id = 0)
        {
            signos signos = db.signos.Find(id);
            if (signos == null)
            {
                return HttpNotFound();
            }
            return PartialView(signos);
        }

        //
        // GET: /Signos/Create

        public ActionResult Create(int id)
        {
            ViewBag.sig_id = id;
            return PartialView();
        }

        //
        // POST: /Signos/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(signos signos)
        {
            if (ModelState.IsValid)
            {
                db.signos.Add(signos);
                db.SaveChanges();
                return RedirectToAction("Create", "Fisico", new { id=signos.sig_id});
            }

            ViewBag.sig_id = signos.sig_id;
            return PartialView(signos);
        }

        //
        // GET: /Signos/Edit/5

        public ActionResult Edit(int id = 0)
        {
            signos signos = db.signos.Find(id);
            if (signos == null)
            {
                return RedirectToAction("Create", new{ id=id});
            }
            ViewBag.sig_id = id;
            return PartialView(signos);
        }

        //
        // POST: /Signos/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(signos signos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(signos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Fisico", new { id = signos.sig_id });
            }
            ViewBag.sig_id = signos.sig_id;
            return PartialView(signos);
        }

        //
        // GET: /Signos/Delete/5

        public ActionResult Delete(int id = 0)
        {
            signos signos = db.signos.Find(id);
            if (signos == null)
            {
                return HttpNotFound();
            }
            return PartialView(signos);
        }

        //
        // POST: /Signos/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            signos signos = db.signos.Find(id);
            db.signos.Remove(signos);
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