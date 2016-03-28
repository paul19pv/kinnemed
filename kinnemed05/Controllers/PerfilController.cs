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
    public class PerfilController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Perfil/

        public ActionResult Index()
        {
            return View(db.perfil.ToList());
        }

        //
        // GET: /Perfil/Details/5

        public ActionResult Details(int id = 0)
        {
            perfil perfil = db.perfil.Find(id);
            if (perfil == null)
            {
                return HttpNotFound();
            }
            return View(perfil);
        }

        //
        // GET: /Perfil/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Perfil/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(perfil perfil)
        {
            if (ModelState.IsValid)
            {
                db.perfil.Add(perfil);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Create", "Control", new { id = perfil.per_id });
            }

            return View(perfil);
        }

        //
        // GET: /Perfil/Edit/5

        public ActionResult Edit(int id = 0)
        {
            perfil perfil = db.perfil.Find(id);
            if (perfil == null)
            {
                return HttpNotFound();
            }
            return View(perfil);
        }

        //
        // POST: /Perfil/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(perfil perfil)
        {
            if (ModelState.IsValid)
            {
                db.Entry(perfil).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Message", "Home", new { mensaje = "Datos Guardados" });
            }
            return View(perfil);
        }

        //
        // GET: /Perfil/Delete/5

        public ActionResult Delete(int id = 0)
        {
            perfil perfil = db.perfil.Find(id);
            if (perfil == null)
            {
                return HttpNotFound();
            }
            return View(perfil);
        }

        //
        // POST: /Perfil/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            perfil perfil = db.perfil.Find(id);
            db.perfil.Remove(perfil);
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