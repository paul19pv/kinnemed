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
    public class RevisionController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Revision/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Index()
        {
            var revision = db.revision.Include(r => r.historia);
            return View(revision.ToList());
        }

        //
        // GET: /Revision/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin, UserRoles.doctor)]
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
        [CustomAuthorize(UserRoles.medico, UserRoles.doctor)]
        public ActionResult Create(int id)
        {
            revision revision = db.revision.Find(id);
            if (revision != null)
            {
                return RedirectToAction("Edit", new { id = id });
            }
            ViewBag.rev_id = id;
            return PartialView();
        }

        //
        // POST: /Revision/Create
        [CustomAuthorize(UserRoles.medico, UserRoles.doctor)]
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
        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
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
        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
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
        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
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
        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
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