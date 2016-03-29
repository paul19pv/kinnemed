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
    public class RevisionController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Revision/

        public ActionResult Index()
        {
            var revision = db.revision.Include(r => r.historia);
            return View(revision.ToList());
        }

        //
        // GET: /Revision/Details/5

        public ActionResult Details(int id = 0)
        {
            revision revision = db.revision.Find(id);
            if (revision == null)
            {
                return HttpNotFound();
            }
            return PartialView(revision);
        }

        //
        // GET: /Revision/Create

        public ActionResult Create(int id)
        {
            ViewBag.rev_id = id;
            return PartialView();
        }

        //
        // POST: /Revision/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(revision revision)
        {
            if (ModelState.IsValid)
            {
                db.revision.Add(revision);
                db.SaveChanges();
                return RedirectToAction("Create", "Signos", new {id=revision.rev_id });
            }

            ViewBag.rev_id = revision.rev_id;
            return PartialView(revision);
        }

        //
        // GET: /Revision/Edit/5

        public ActionResult Edit(int id = 0)
        {
            revision revision = db.revision.Find(id);
            if (revision == null)
            {
                return RedirectToAction("Create", new {id=id });
            }
            ViewBag.rev_id = id;
            return PartialView(revision);
        }

        //
        // POST: /Revision/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(revision revision)
        {
            if (ModelState.IsValid)
            {
                db.Entry(revision).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Signos", new { id = revision.rev_id });
            }
            ViewBag.rev_id = revision.rev_id;
            return PartialView(revision);
        }

        //
        // GET: /Revision/Delete/5

        public ActionResult Delete(int id = 0)
        {
            revision revision = db.revision.Find(id);
            if (revision == null)
            {
                return HttpNotFound();
            }
            return View(revision);
        }

        //
        // POST: /Revision/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            revision revision = db.revision.Find(id);
            db.revision.Remove(revision);
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