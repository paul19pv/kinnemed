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
    public class PersonalController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Personal/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Index()
        {
            var personal = db.personal.Include(p => p.paciente);
            return View(personal.ToList());
        }

        //
        // GET: /Personal/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin, UserRoles.doctor)]
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
        [CustomAuthorize(UserRoles.medico, UserRoles.doctor)]
        public ActionResult Create(int id)
        {
            //ViewBag.per_id = new SelectList(db.historia, "his_id", "his_motivo");
            personal personal = db.personal.Find(id);
            if (personal != null)
            {
                return RedirectToAction("Edit", new { id = id });
            }
            ViewBag.per_id = id;
            ViewBag.per_lateralidad = per_lateralidad();
            return PartialView();
        }

        //
        // POST: /Personal/Create
        [CustomAuthorize(UserRoles.medico, UserRoles.doctor)]
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
                    return RedirectToAction("Create", "Habitos", new { id = personal.per_id });
                }
                
            }

            ViewBag.per_id = personal.per_id;
            ViewBag.per_lateralidad = per_lateralidad(personal.per_lateralidad);
            return PartialView(personal);
        }

        //
        // GET: /Personal/Edit/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Edit(int id)
        {
            personal personal = db.personal.Find(id);
            
            if (personal == null)
            {
                return RedirectToAction("Create", new { id = id });
            }
            ViewBag.per_id = id;
            ViewBag.per_lateralidad = per_lateralidad(personal.per_lateralidad);
            return PartialView(personal);
        }

        //
        // POST: /Personal/Edit/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
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
                    return RedirectToAction("Edit", "Habitos", new { id = personal.per_id });
                }
                //return RedirectToAction("Edit", "Ginecologico", new { id = personal.per_id });
            }
            ViewBag.per_id = personal.per_id;
            ViewBag.per_lateralidad = per_lateralidad(personal.per_lateralidad);
            return PartialView(personal);
        }

        //
        // GET: /Personal/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
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
        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            personal personal = db.personal.Find(id);
            db.personal.Remove(personal);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private SelectList per_lateralidad(string jornada = "")
        {
            List<SelectListItem> list_jornada = new List<SelectListItem>();
            list_jornada.Add(new SelectListItem { Text = "Diestro", Value = "Diestro" });
            list_jornada.Add(new SelectListItem { Text = "Zurdo", Value = "Zurdo" });
            list_jornada.Add(new SelectListItem { Text = "Ambidiestro", Value = "Ambidiestro" });
            SelectList jornadas;
            if (jornada == "")
                jornadas = new SelectList(list_jornada, "Value", "Text");
            else
                jornadas = new SelectList(list_jornada, "Value", "Text", jornada);
            return jornadas;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}