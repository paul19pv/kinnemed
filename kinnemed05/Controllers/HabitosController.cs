using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kinnemed05.Models;
using kinnemed05.Security;

namespace kinnemed05.Controllers
{
    public class HabitosController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Habitos/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Index()
        {
            var habitos = db.habitos.Include(h => h.paciente);
            return View(habitos.ToList());
        }

        //
        // GET: /Habitos/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Details(int id = 0)
        {
            habitos habitos = db.habitos.Find(id);
            if (habitos == null)
            {
                return HttpNotFound();
            }
            return PartialView(habitos);
        }

        //
        // GET: /Habitos/Create
        [CustomAuthorize(UserRoles.medico)]
        public ActionResult Create(int id)
        {
            habitos habitos = db.habitos.Find(id);
            if (habitos != null)
                return RedirectToAction("Edit", new { id = id });
            ViewBag.hab_id = id;
            ViewBag.hab_fumo = opcion();
            ViewBag.hab_fuma = opcion();
            ViewBag.hab_alcohol = opcion();
            ViewBag.hab_drogas = opcion();
            ViewBag.hab_ejercicio = opcion();
            return PartialView();
        }

        //
        // POST: /Habitos/Create
        [CustomAuthorize(UserRoles.medico)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(habitos habitos)
        {
            if (ModelState.IsValid)
            {
                db.habitos.Add(habitos);
                db.SaveChanges();
                return RedirectToAction("Create", "Familiar", new { id = habitos.hab_id });
            }

            ViewBag.hab_id = habitos.hab_id;
            ViewBag.hab_fumo = opcion(habitos.hab_fumo);
            ViewBag.hab_fuma = opcion(habitos.hab_fuma);
            ViewBag.hab_alcohol = opcion(habitos.hab_alcohol);
            ViewBag.hab_drogas = opcion(habitos.hab_drogas);
            ViewBag.hab_ejercicio = opcion(habitos.hab_ejercicio);
            return PartialView(habitos);
        }

        //
        // GET: /Habitos/Edit/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin)]
        public ActionResult Edit(int id = 0)
        {
            habitos habitos = db.habitos.Find(id);
            if (habitos == null)
            {
                return RedirectToAction("Create", new { id = id });
            }

            ViewBag.hab_fumo = opcion(habitos.hab_fumo);
            ViewBag.hab_fuma = opcion(habitos.hab_fuma);
            ViewBag.hab_alcohol = opcion(habitos.hab_alcohol);
            ViewBag.hab_drogas = opcion(habitos.hab_drogas);
            ViewBag.hab_ejercicio = opcion(habitos.hab_ejercicio);
            return PartialView(habitos);
        }

        //
        // POST: /Habitos/Edit/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(habitos habitos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(habitos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create", "Familiar", new { id = habitos.hab_id });
            }
            ViewBag.hab_fumo = opcion(habitos.hab_fumo);
            ViewBag.hab_fuma = opcion(habitos.hab_fuma);
            ViewBag.hab_alcohol = opcion(habitos.hab_alcohol);
            ViewBag.hab_drogas = opcion(habitos.hab_drogas);
            ViewBag.hab_ejercicio = opcion(habitos.hab_ejercicio);
            return PartialView(habitos);
        }

        //
        // GET: /Habitos/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin)]
        public ActionResult Delete(int id = 0)
        {
            habitos habitos = db.habitos.Find(id);
            if (habitos == null)
            {
                return HttpNotFound();
            }
            return View(habitos);
        }

        //
        // POST: /Habitos/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            habitos habitos = db.habitos.Find(id);
            db.habitos.Remove(habitos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private SelectList opcion(string valor = "")
        {
            List<SelectListItem> list_opcion = new List<SelectListItem>();
            list_opcion.Add(new SelectListItem { Text = "SI", Value = "SI" });
            list_opcion.Add(new SelectListItem { Text = "NO", Value = "NO" });
            SelectList opciones;
            if (valor == "")
                opciones = new SelectList(list_opcion, "Value", "Text");
            else
                opciones = new SelectList(list_opcion, "Value", "Text", valor);
            return opciones;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}