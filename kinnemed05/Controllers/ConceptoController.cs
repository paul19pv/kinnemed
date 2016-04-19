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
    public class ConceptoController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Concepto/

        public ActionResult Index()
        {
            var concepto = db.concepto.Include(c => c.historia);
            return View(concepto.ToList());
        }

        //
        // GET: /Concepto/Details/5

        public ActionResult Details(int id = 0)
        {
            concepto concepto = db.concepto.Find(id);
            if (concepto == null)
            {
                return HttpNotFound();
            }
            return PartialView(concepto);
        }

        //
        // GET: /Concepto/Create

        public ActionResult Create(int id)
        {
            concepto concepto = db.concepto.Find(id);
            if (concepto != null)
            {
                return RedirectToAction("Edit", new { id = id });
            }
            ViewBag.con_resultado = con_resultado();
            ViewBag.con_id = id;
            return PartialView();
        }

        //
        // POST: /Concepto/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(concepto concepto)
        {
            if (ModelState.IsValid)
            {
                db.concepto.Add(concepto);
                db.SaveChanges();
                return RedirectToAction("Message", "Home", new {mensaje="Proceso Finalizado" });
            }
            ViewBag.con_resultado = con_resultado(concepto.con_resultado);
            ViewBag.con_id = new SelectList(db.historia, "his_id", "his_motivo", concepto.con_id);
            return PartialView(concepto);
        }

        //
        // GET: /Concepto/Edit/5

        public ActionResult Edit(int id = 0)
        {
            concepto concepto = db.concepto.Find(id);
            if (concepto == null)
            {
                return RedirectToAction("Create", new {id=id });
            }
            ViewBag.con_resultado = con_resultado(concepto.con_resultado);
            ViewBag.con_id = id;
            return PartialView(concepto);
        }

        //
        // POST: /Concepto/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(concepto concepto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(concepto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Message", "Home", new { mensaje = "Proceso Finalizado" });
            }
            ViewBag.con_resultado = con_resultado(concepto.con_resultado);
            ViewBag.con_id = concepto.con_id;
            return PartialView(concepto);
        }

        //
        // GET: /Concepto/Delete/5

        public ActionResult Delete(int id = 0)
        {
            concepto concepto = db.concepto.Find(id);
            if (concepto == null)
            {
                return HttpNotFound();
            }
            return View(concepto);
        }

        //
        // POST: /Concepto/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            concepto concepto = db.concepto.Find(id);
            db.concepto.Remove(concepto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private SelectList con_resultado(string resultado = "")
        {
            List<SelectListItem> list_resultado = new List<SelectListItem>();
            list_resultado.Add(new SelectListItem { Text = "APTO", Value = "APTO" });
            list_resultado.Add(new SelectListItem { Text = "APTO CON RESTRICCIONES PERSONALES", Value = "APTO CON RESTRICCIONES PERSONALES" });
            list_resultado.Add(new SelectListItem { Text = "APTO CON RESTRICCIONES LABORALES", Value = "APTO CON RESTRICCIONES LABORALES" });
            list_resultado.Add(new SelectListItem { Text = "NO APTO", Value = "NO APTO" });
            SelectList resultados;
            if (resultado == "")
                resultados = new SelectList(list_resultado, "Value", "Text");
            else
                resultados = new SelectList(list_resultado, "Value", "Text", resultado);
            return resultados;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}