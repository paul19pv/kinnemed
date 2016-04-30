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
    //[InitializeSimpleMembership]
    //[CustomAuthorize(UserRoles.laboratorista,UserRoles.admin)]
    public class PerfilController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Perfil/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Index()
        {
            return View(db.perfil.ToList());
        }

        //
        // GET: /Perfil/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
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
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        public ActionResult Create()
        {
            var riesgo = db.riesgo.ToList();
            return View();
        }

        //
        // POST: /Perfil/Create
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
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
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
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
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
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
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
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
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
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