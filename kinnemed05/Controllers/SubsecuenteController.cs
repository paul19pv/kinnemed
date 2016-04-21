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
    [CustomAuthorize(UserRoles.medico)]
    public class SubsecuenteController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Subsecuente/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Index()
        {
            var subsecuente = db.subsecuente.Include(s => s.historia);
            return View(subsecuente.ToList());
        }
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Historia(lista lista)
        {
            var historia = db.historia.Include(h => h.paciente);
            if (lista.paciente != 0)
                historia = historia.Where(h => h.his_paciente == lista.paciente);
            if (!String.IsNullOrEmpty(lista.fecha))
                historia = historia.Where(h => h.his_fecha == lista.fecha);
            if (lista.tipo != 0)
                historia = historia.Where(h => h.his_tipo == lista.tipo);
            ViewBag.paciente = lista.paciente +" "+lista.fecha+" "+lista.tipo;
            return PartialView(historia.ToList());
        }

        //
        // GET: /Subsecuente/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Details(int id = 0)
        {
            subsecuente subsecuente = db.subsecuente.Find(id);
            if (subsecuente == null)
            {
                return HttpNotFound();
            }
            return View(subsecuente);
        }
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Lista() {
            ViewBag.tipo = his_tipo();
            return View();
        }
        //
        // GET: /Subsecuente/Create
        [CustomAuthorize(UserRoles.medico)]
        public ActionResult Create(int id)
        {
            ViewBag.sub_historia = id;
            DateTime dd = DateTime.Now;
            ViewBag.fecha = dd.Date.ToString("d");
            ViewBag.hora = dd.ToString("T");
            return View();
        }

        //
        // POST: /Subsecuente/Create
        [CustomAuthorize(UserRoles.medico)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(subsecuente subsecuente)
        {
            if (ModelState.IsValid)
            {
                db.subsecuente.Add(subsecuente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.sub_historia = subsecuente.sub_historia;
            DateTime dd = DateTime.Now;
            ViewBag.fecha = dd.Date.ToString("d");
            ViewBag.hora = dd.ToString("T");
            return View(subsecuente);
        }

        //
        // GET: /Subsecuente/Edit/5
        [CustomAuthorize(UserRoles.medico)]
        public ActionResult Edit(int id = 0)
        {
            subsecuente subsecuente = db.subsecuente.Find(id);
            if (subsecuente == null)
            {
                return HttpNotFound();
            }
            return View(subsecuente);
        }

        //
        // POST: /Subsecuente/Edit/5
        [CustomAuthorize(UserRoles.medico)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(subsecuente subsecuente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subsecuente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subsecuente);
        }

        //
        // GET: /Subsecuente/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin)]
        public ActionResult Delete(int id = 0)
        {
            subsecuente subsecuente = db.subsecuente.Find(id);
            if (subsecuente == null)
            {
                return HttpNotFound();
            }
            return View(subsecuente);
        }

        //
        // POST: /Subsecuente/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            subsecuente subsecuente = db.subsecuente.Find(id);
            db.subsecuente.Remove(subsecuente);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public SelectList his_tipo(int? tipo = 0)
        {
            List<SelectListItem> list_tipo = new List<SelectListItem>();
            list_tipo.Add(new SelectListItem { Text = "General", Value = "1" });
            list_tipo.Add(new SelectListItem { Text = "Preocupacional", Value = "2" });
            list_tipo.Add(new SelectListItem { Text = "Periodica", Value = "3" });
            list_tipo.Add(new SelectListItem { Text = "Retiro", Value = "4" });
            SelectList tipos;
            if (tipo == 0)
                tipos = new SelectList(list_tipo, "Value", "Text");
            else
                tipos = new SelectList(list_tipo, "Value", "Text", tipo.ToString());
            return tipos;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}