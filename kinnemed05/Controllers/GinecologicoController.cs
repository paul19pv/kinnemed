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
    public class GinecologicoController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Ginecologico/

        public ActionResult Index()
        {
            var ginecologico = db.ginecologico.Include(g => g.paciente);
            return View(ginecologico.ToList());
        }

        //
        // GET: /Ginecologico/Details/5

        public ActionResult Details(int id = 0)
        {
            ginecologico ginecologico = db.ginecologico.Find(id);
            if (ginecologico == null)
            {
                return HttpNotFound();
            }
            return View(ginecologico);
        }

        //
        // GET: /Ginecologico/Create

        public ActionResult Create(int id)
        {
            ginecologico ginecologico = db.ginecologico.Find(id);
            if (ginecologico != null)
            {
                return RedirectToAction("Edit", new { id=id});
            }
            ViewBag.gin_id = id;
            return PartialView();
        }

        //
        // POST: /Ginecologico/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ginecologico ginecologico)
        {
            if (ModelState.IsValid)
            {
                db.ginecologico.Add(ginecologico);
                db.SaveChanges();
                return RedirectToAction("Create", "Familiar", new { id=ginecologico.gin_id});
            }

            ViewBag.gin_id = ginecologico.gin_id;
            return PartialView(ginecologico);
        }

        //
        // GET: /Ginecologico/Edit/5

        public ActionResult Edit(int id)
        {
            ginecologico ginecologico = db.ginecologico.Find(id);
            if (ginecologico == null)
            {
                paciente paciente = db.paciente.Find(id);
                if (paciente.pac_genero == "Femenino")
                {
                    return RedirectToAction("Create", new { id = id });
                }
                else {
                    return RedirectToAction("Edit","Personal", new { id = id });
                }
                
            }
            ViewBag.gin_id = ginecologico.gin_id;
            return PartialView(ginecologico);
        }

        //
        // POST: /Ginecologico/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ginecologico ginecologico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ginecologico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Familiar", new { id = ginecologico.gin_id });
            }
            ViewBag.gin_id = ginecologico.gin_id;
            return PartialView(ginecologico);
        }

        //
        // GET: /Ginecologico/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ginecologico ginecologico = db.ginecologico.Find(id);
            if (ginecologico == null)
            {
                return HttpNotFound();
            }
            return View(ginecologico);
        }

        //
        // POST: /Ginecologico/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ginecologico ginecologico = db.ginecologico.Find(id);
            db.ginecologico.Remove(ginecologico);
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