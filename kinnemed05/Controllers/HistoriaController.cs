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
    public class HistoriaController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Historia/

        public ActionResult Index()
        {
            return View(db.historia.ToList());
        }

        //
        // GET: /Historia/Details/5

        public ActionResult Details(int id = 0)
        {
            historia historia = db.historia.Find(id);
            if (historia == null)
            {
                return HttpNotFound();
            }
            return View(historia);
        }

        //
        // GET: /Historia/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Historia/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(historia historia)
        {
            DateTime dd = DateTime.Now;
            historia.his_fecha = dd.Date.ToString("d");
            historia.his_numero = numero_historia(historia);
            if (ModelState.IsValid)
            {
                db.historia.Add(historia);
                db.SaveChanges();
                Session["his_id"] = historia.his_id;
                return RedirectToAction("Create", "Personal", new { id = historia.his_paciente});
            }

            return PartialView(historia);
        }

        //Historias preocupacionales, ocupaciones y retiro
        public ActionResult Create01()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create01(historia historia)
        {
            DateTime dd = DateTime.Now;
            historia.his_fecha = dd.Date.ToString("d");
            historia.his_numero = numero_historia(historia);
            if (ModelState.IsValid)
            {
                db.historia.Add(historia);
                db.SaveChanges();
                Session["his_id"] = historia.his_id;
                return RedirectToAction("Create", "Ocupacional", new { id = historia.his_paciente });
            }

            return PartialView(historia);
        }


        //
        // GET: /Historia/Edit/5
        
        public ActionResult Edit(int id = 0)
        {
            historia historia = db.historia.Find(id);
            if (historia == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(historia);
            }
            return View("Edit01",historia);
        }



        //
        // POST: /Historia/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(historia historia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(historia).State = EntityState.Modified;
                db.SaveChanges();
                Session["his_id"] = historia.his_id;
                return RedirectToAction("Edit", "Personal", new { id = historia.his_paciente });
            }
            return PartialView(historia);
        }


        public ActionResult Edit02(int id = 0)
        {
            historia historia = db.historia.Find(id);
            if (historia == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(historia);
            }
            return View("Edit03", historia);
        }



        //
        // POST: /Historia/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit02(historia historia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(historia).State = EntityState.Modified;
                db.SaveChanges();
                Session["his_id"] = historia.his_id;
                return RedirectToAction("Edit", "Ocupacional", new { id = historia.his_paciente });
            }
            return PartialView(historia);
        }

        public ActionResult Problema(int id)
        {
            historia historia = db.historia.Find(id);
            if (historia == null)
            {
                return HttpNotFound();
            }
            return PartialView(historia);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Problema(historia historia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(historia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Revision", new { id = historia.his_id });
            }
            return PartialView(historia);
        }
        //
        // GET: /Historia/Delete/5

        public ActionResult Delete(int id = 0)
        {
            historia historia = db.historia.Find(id);
            if (historia == null)
            {
                return HttpNotFound();
            }
            return View(historia);
        }

        //
        // POST: /Historia/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            historia historia = db.historia.Find(id);
            db.historia.Remove(historia);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public JsonResult AutocompletePaciente(string search)
        {
            search = search.ToUpper();
            var result = (from p in db.paciente
                          where p.pac_cedula.ToUpper().Contains(search) ||
                              p.pac_nombres.ToUpper().Contains(search) ||
                              p.pac_apellidos.ToUpper().Contains(search)
                          select new { p.pac_id, p.pac_nombres, p.pac_apellidos, p.pac_edad }).Distinct();
            if (result.Count() == 0)
            {
                return new JsonResult() { Data = new { Data = new { pac_id = 0, pac_nombres = "Sin Datos", pac_apellidos = "",pac_edad="" } } };
            }
            return new JsonResult() { Data = result };
        }
        public int numero_historia(historia historia) {
            int num = 0;
            num = db.historia.Where(h => h.his_tipo == historia.his_tipo && h.his_paciente == historia.his_paciente).Count();
            num++;
            return num;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}