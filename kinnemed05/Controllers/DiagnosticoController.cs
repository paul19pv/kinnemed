using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kinnemed05.Models;
using kinnemed05.Security;
using kinnemed05.Filters;

namespace kinnemed05.Controllers
{
    [InitializeSimpleMembership]
    //[CustomAuthorize(UserRoles.medico)]
    public class DiagnosticoController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Diagnostico/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Index(int id)
        {
            var diagnostico = db.diagnostico.Include(d => d.historia).Include(d => d.sub_cie10);
            diagnostico = diagnostico.Where(d => d.dia_historia == id);
            return PartialView(diagnostico.ToList());
        }
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Consulta(int id)
        {
            var diagnostico = db.diagnostico.Include(d => d.historia).Include(d => d.sub_cie10);
            diagnostico = diagnostico.Where(d => d.dia_historia == id);
            return PartialView(diagnostico.ToList());
        }

        //
        // GET: /Diagnostico/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Details(int id = 0)
        {
            diagnostico diagnostico = db.diagnostico.Find(id);
            if (diagnostico == null)
            {
                return HttpNotFound();
            }
            return View(diagnostico);
        }

        //
        // GET: /Diagnostico/Create
        [CustomAuthorize(UserRoles.medico)]
        public ActionResult Create(int id)
        {
            ViewBag.dia_historia = id;
            //ViewBag.dia_subcie10 = new SelectList(db.sub_cie10, "sub_id", "sub_codigo");
            ViewBag.dia_tipo = tipo();
            return PartialView();
        }

        //
        // POST: /Diagnostico/Create
        [CustomAuthorize(UserRoles.medico)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(diagnostico diagnostico)
        {
            if (ModelState.IsValid)
            {
                db.diagnostico.Add(diagnostico);
                db.SaveChanges();
                return RedirectToAction("Index", new { id=diagnostico.dia_historia});
            }

            ViewBag.dia_historia = diagnostico.dia_historia;
            //ViewBag.dia_subcie10 = new SelectList(db.sub_cie10, "sub_id", "sub_codigo", diagnostico.dia_subcie10);
            ViewBag.dia_tipo = tipo(diagnostico);
            return PartialView(diagnostico);
        }

        //
        // GET: /Diagnostico/Edit/5
        [CustomAuthorize(UserRoles.medico)]
        public ActionResult Edit(int id = 0)
        {
            diagnostico diagnostico = db.diagnostico.Find(id);
            if (diagnostico == null)
            {
                return RedirectToAction("Create", new { id=id});
            }
            ViewBag.dia_historia = diagnostico.dia_historia;
            //ViewBag.dia_subcie10 = new SelectList(db.sub_cie10, "sub_id", "sub_codigo", diagnostico.dia_subcie10);
            ViewBag.dia_tipo = tipo(diagnostico);
            return PartialView(diagnostico);
        }

        //
        // POST: /Diagnostico/Edit/5
        [CustomAuthorize(UserRoles.medico)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(diagnostico diagnostico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(diagnostico).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
            }
            ViewBag.dia_historia = diagnostico.dia_historia;
            //ViewBag.dia_subcie10 = new SelectList(db.sub_cie10, "sub_id", "sub_codigo", diagnostico.dia_subcie10);
            ViewBag.dia_tipo = tipo(diagnostico);
            return PartialView(diagnostico);
        }

        //
        // GET: /Diagnostico/Delete/5
        [CustomAuthorize(UserRoles.medico,UserRoles.admin)]
        public ActionResult Delete(int id = 0)
        {
            diagnostico diagnostico = db.diagnostico.Find(id);
            db.diagnostico.Remove(diagnostico);
            db.SaveChanges();
            return RedirectToAction("Index", new { id=diagnostico.dia_historia });
        }

        //
        // POST: /Diagnostico/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            diagnostico diagnostico = db.diagnostico.Find(id);
            db.diagnostico.Remove(diagnostico);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult AutocompleteCategoria(string search)
        {
            search = search.ToUpper();
            var result = (from p in db.cie10
                          where p.cie_codigo.ToUpper().Contains(search) ||
                          p.cie_descripcion.ToUpper().Contains(search)
                          select new { p.cie_id, p.cie_codigo, p.cie_descripcion }).Distinct();
            if (result.Count() == 0)
            {
                return new JsonResult() { Data = new { Data = new { cie_id=0,cie_codigo = "Sin Datos", cie_descripcion = "" } } };
            }
            return new JsonResult() { Data = result };
        }
        public JsonResult AutocompleteSubcategoria(string search)
        {
            search = search.ToUpper();
            var result = (from p in db.sub_cie10
                          where p.sub_codigo.ToUpper().Contains(search) ||
                          p.sub_descripcion.ToUpper().Contains(search) 
                          select new { p.sub_id, p.sub_codigo, p.sub_descripcion }).Distinct();
            if (result.Count() == 0)
            {
                return new JsonResult() { Data = new { Data = new { sub_id = 0, sub_codigo = "Sin Datos", sub_descripcion = "" } } };
            }
            return new JsonResult() { Data = result };
        }

        public JsonResult GetSubPorCat(int id)
        {
            var lista = from c in db.sub_cie10 where c.sub_cie101 == id select c;
            var cantones = new SelectList(lista, "sub_id", "sub_descripcion");
            return new JsonResult() { Data = cantones };
        }
        public SelectList tipo()
        {
            List<SelectListItem> list_tipo = new List<SelectListItem>();
            list_tipo.Add(new SelectListItem { Text = "PRESUNTIVO", Value = "PRE" });
            list_tipo.Add(new SelectListItem { Text = "DEFINITIVO", Value = "DEF" });
            SelectList tipos = new SelectList(list_tipo, "Value", "Text");
            return tipos;
        }
        public SelectList tipo(diagnostico diagnostico)
        {
            List<SelectListItem> list_tipo = new List<SelectListItem>();
            list_tipo.Add(new SelectListItem { Text = "PRESUNTIVO", Value = "PRE" });
            list_tipo.Add(new SelectListItem { Text = "DEFINITIVO", Value = "DEF" });
            SelectList tipos = new SelectList(list_tipo, "Value", "Text", diagnostico.dia_tipo);
            return tipos;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}