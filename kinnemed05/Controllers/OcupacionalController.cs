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
    public class OcupacionalController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Ocupacional/

        public ActionResult Index()
        {
            var ocupacional = db.ocupacional.Include(o => o.paciente);
            return PartialView(ocupacional.ToList());
        }

        //
        // GET: /Ocupacional/Details/5

        public ActionResult Details(int id = 0)
        {
            ocupacional ocupacional = db.ocupacional.Find(id);
            if (ocupacional == null)
            {
                return HttpNotFound();
            }
            return View(ocupacional);
        }

        //
        // GET: /Ocupacional/Create

        public ActionResult Create(int id)
        {
            ViewBag.ocu_paciente = id;
            ViewBag.ocu_tipo = "actual";
            ViewBag.ocu_jornada = ocu_jornada();
            return PartialView();
        }

        //
        // POST: /Ocupacional/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ocupacional ocupacional)
        {
            ocupacional.ocu_estado = true;
            if (ModelState.IsValid)
            {
                db.ocupacional.Add(ocupacional);
                db.SaveChanges();
                return RedirectToAction("Create", "Laboral", new {id=ocupacional.ocu_id });
            }
            ViewBag.ocu_paciente = ocupacional.ocu_paciente;
            ViewBag.ocu_tipo = ocupacional.ocu_tipo;
            ViewBag.ocu_jornada = ocu_jornada(ocupacional.ocu_jornada);
            return PartialView(ocupacional);
        }



        //
        // GET: /Ocupacional/Edit/5

        public ActionResult Edit(int id)
        {
            ocupacional ocupacional = db.ocupacional.Where(o=>o.ocu_paciente==id).First();
            if (ocupacional == null)
            {
                return RedirectToAction("Create", new { id=id});
            }
            ocupacional = db.ocupacional.Find(id);
            ViewBag.ocu_jornada = ocu_jornada(ocupacional.ocu_jornada);
            return PartialView(ocupacional);
        }


        //
        // POST: /Ocupacional/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ocupacional ocupacional)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ocupacional).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Laboral", new { id = ocupacional.ocu_id });
            }
            ViewBag.ocu_jornada = ocu_jornada(ocupacional.ocu_jornada);
            return PartialView(ocupacional);
        }

        //
        // GET: /Ocupacional/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ocupacional ocupacional = db.ocupacional.Find(id);
            if (ocupacional == null)
            {
                return HttpNotFound();
            }
            return View(ocupacional);
        }

        //
        // POST: /Ocupacional/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ocupacional ocupacional = db.ocupacional.Find(id);
            db.ocupacional.Remove(ocupacional);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public string get_estado(int his_id) {
            string estado = String.Empty;
            return estado;
        }
        public SelectList ocu_jornada(string jornada = "")
        {
            List<SelectListItem> list_jornada = new List<SelectListItem>();
            list_jornada.Add(new SelectListItem { Text = "Diurno", Value = "Diurno" });
            list_jornada.Add(new SelectListItem { Text = "Nocturno", Value = "Nocturno" });
            list_jornada.Add(new SelectListItem { Text = "Rotativo", Value = "Rotativo" });
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