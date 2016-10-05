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
    public class ActividadController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Actividad/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Index()
        {
            var actividad = db.actividad.Include(a => a.paciente);
            return View(actividad.ToList());
        }

        //
        // GET: /Actividad/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Details(int id = 0)
        {
            actividad actividad = db.actividad.Find(id);
            if (actividad == null)
            {
                return HttpNotFound();
            }
            return View(actividad);
        }

        //
        // GET: /Actividad/Create
        [CustomAuthorize(UserRoles.medico, UserRoles.doctor)]
        public ActionResult Create(int id)
        {
            actividad actividad = db.actividad.Find(id);
            if (actividad != null)
            {
                return RedirectToAction("Edit",new {id=id});
            }
            ViewBag.act_id = id;
            ViewBag.act_enf_estado = opcion();
            ViewBag.act_acc_estado = opcion();
            return PartialView();
        }

        //
        // POST: /Actividad/Create
        [CustomAuthorize(UserRoles.medico, UserRoles.doctor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(actividad actividad)
        {
            if (ModelState.IsValid)
            {
                db.actividad.Add(actividad);
                db.SaveChanges();
                return RedirectToAction("Create", "Personal", new { id = actividad.act_id });
            }

            ViewBag.act_id = actividad.act_id;
            ViewBag.act_enf_estado = opcion(actividad.act_enf_estado);
            ViewBag.act_acc_estado = opcion(actividad.act_acc_estado);
            return PartialView(actividad);
        }

        //
        // GET: /Actividad/Edit/5
        [CustomAuthorize(UserRoles.medico, UserRoles.doctor)]
        public ActionResult Edit(int id = 0)
        {
            actividad actividad = db.actividad.Find(id);
            if (actividad == null)
            {
                return HttpNotFound();
            }
            ViewBag.act_id = id;
            ViewBag.act_enf_estado = opcion(actividad.act_enf_estado);
            ViewBag.act_acc_estado = opcion(actividad.act_acc_estado);
            return PartialView(actividad);
        }

        //
        // POST: /Actividad/Edit/5
        [CustomAuthorize(UserRoles.medico, UserRoles.doctor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(actividad actividad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actividad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Personal", new { id = actividad.act_id });
            }
            ViewBag.act_id = actividad.act_id;
            ViewBag.act_enf_estado = opcion(actividad.act_enf_estado);
            ViewBag.act_acc_estado = opcion(actividad.act_acc_estado);
            return PartialView(actividad);
        }

        //
        // GET: /Actividad/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.doctor)]
        public ActionResult Delete(int id = 0)
        {
            actividad actividad = db.actividad.Find(id);
            if (actividad == null)
            {
                return HttpNotFound();
            }
            return View(actividad);
        }

        //
        // POST: /Actividad/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.doctor)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            actividad actividad = db.actividad.Find(id);
            db.actividad.Remove(actividad);
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