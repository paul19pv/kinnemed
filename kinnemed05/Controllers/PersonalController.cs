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
    public class PersonalController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Personal/

        public ActionResult Index()
        {
            var personal = db.personal.Include(p => p.paciente);
            return View(personal.ToList());
        }

        //
        // GET: /Personal/Details/5

        public ActionResult Details(int id = 0)
        {
            personal personal = db.personal.Find(id);
            if (personal == null)
            {
                return HttpNotFound();
            }
            return PartialView(personal);
        }

        //
        // GET: /Personal/Create

        public ActionResult Create(int id)
        {
            //ViewBag.per_id = new SelectList(db.historia, "his_id", "his_motivo");
            personal personal = db.personal.Find(id);
            if (personal != null)
            {
                return RedirectToAction("Edit", new { id = id });
            }
            ViewBag.per_id = id;
            return PartialView();
        }

        //
        // POST: /Personal/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(personal personal)
        {
            if (ModelState.IsValid)
            {
                db.personal.Add(personal);
                db.SaveChanges();
                paciente paciente = db.paciente.Find(personal.per_id);
                if (paciente.pac_genero == "Femenino")
                {
                    return RedirectToAction("Create", "Ginecologico", new { id = personal.per_id });
                }
                else {
                    return RedirectToAction("Create", "Familiar", new { id = personal.per_id });
                }
                
            }

            ViewBag.per_id = personal.per_id;
            return PartialView(personal);
        }

        //
        // GET: /Personal/Edit/5
        public ActionResult Edit(int id)
        {
            personal personal = db.personal.Find(id);
            
            if (personal == null)
            {
                return RedirectToAction("Create", new { id = id });
            }
            ViewBag.per_id = id;
            return PartialView(personal);
        }

        //
        // POST: /Personal/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(personal personal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personal).State = EntityState.Modified;
                db.SaveChanges();
                paciente paciente = db.paciente.Find(personal.per_id);
                if (paciente.pac_genero == "Femenino")
                {
                    return RedirectToAction("Edit", "Ginecologico", new { id = personal.per_id });
                }
                else
                {
                    return RedirectToAction("Edit", "Familiar", new { id = personal.per_id });
                }
                //return RedirectToAction("Edit", "Ginecologico", new { id = personal.per_id });
            }
            ViewBag.per_id = personal.per_id;
            return PartialView(personal);
        }

        //
        // GET: /Personal/Delete/5

        public ActionResult Delete(int id = 0)
        {
            personal personal = db.personal.Find(id);
            if (personal == null)
            {
                return HttpNotFound();
            }
            return PartialView(personal);
        }

        //
        // POST: /Personal/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            personal personal = db.personal.Find(id);
            db.personal.Remove(personal);
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