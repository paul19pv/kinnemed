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

        public ActionResult Index(int tipo)
        {
            var historia = db.historia.Where(h => h.his_tipo == tipo);
            ViewBag.tipo = tipo;
            ViewBag.titulo = titulo(tipo);
            return View(historia.ToList());
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

        public ActionResult Create(int tipo)
        {
            ViewBag.tipo = tipo;
            ViewBag.his_tipo = his_tipo(tipo);
            if (tipo != 1) {
                return View("Create01");
            }
                
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
            historia.his_tipo = 1;
            if (ModelState.IsValid)
            {
                db.historia.Add(historia);
                db.SaveChanges();
                Session["his_id"] = historia.his_id;
                return RedirectToAction("Create", "Personal", new { id = historia.his_paciente});
            }
            ViewBag.numero = numero_historia(historia);
            return PartialView(historia);
        }

        //Historias preocupacionales, ocupaciones y retiro
        public ActionResult Create01()
        {
            ViewBag.his_tipo = his_tipo();
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
                if (historia.his_tipo == 2)
                    change_tipo(historia.his_paciente);
                Session["his_id"] = historia.his_id;
                return RedirectToAction("Create", "Ocupacional", new { id = historia.his_paciente });
            }
            ViewBag.his_tipo = his_tipo(historia.his_tipo);
            return PartialView(historia);
        }


        //
        // GET: /Historia/Edit/5
        
        public ActionResult Edit(int id = 0)
        {
            historia historia = db.historia.Find(id);
            ViewBag.tipo = historia.his_tipo;
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
            ViewBag.his_tipo = his_tipo(historia.his_tipo);
            ViewBag.tipo = historia.his_tipo;
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
            ViewBag.his_tipo = his_tipo(historia.his_tipo);
            ViewBag.tipo = historia.his_tipo;
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
            ViewBag.tipo = historia.his_tipo;
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
            return RedirectToAction("Index", new {tipo=historia.his_tipo });
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

        public SelectList his_tipo(int? tipo=0)
        {
            List<SelectListItem> list_tipo = new List<SelectListItem>();
            list_tipo.Add(new SelectListItem { Text = "Preocupacional", Value = "2" });
            list_tipo.Add(new SelectListItem { Text = "Ocupacional", Value = "3" });
            list_tipo.Add(new SelectListItem { Text = "Retiro", Value = "4" });
            SelectList tipos;
            if(tipo==0)
                tipos= new SelectList(list_tipo, "Value", "Text");
            else
                tipos = new SelectList(list_tipo, "Value", "Text",tipo.ToString());
            return tipos;
        }

        public string change_tipo(int pac_id) {
            string mensaje = String.Empty;
            var consulta = db.ocupacional.Where(o => o.ocu_paciente == pac_id && o.ocu_tipo == "actual");
            if (!consulta.Any())
            {
                mensaje = "El paciente no registra un trabajo actual";
            }
            else {
                ocupacional ocupacional = db.ocupacional.Where(o => o.ocu_paciente == pac_id && o.ocu_tipo == "actual").First();
                ocupacional.ocu_tipo = "registro";
                ocupacional.ocu_estado = false;
                db.Entry(ocupacional).State = EntityState.Modified;
                db.SaveChanges();
                mensaje = "El trabajo actual anterior quedara como historico";
            }
            
            return mensaje;
        }
        public string titulo(int tipo) {
            string titulo = String.Empty;
            switch (tipo) { 
                case 1:
                    titulo = "Generales";
                    break;
                case 2:
                    titulo = "Preocupacionales";
                    break;
                case 3:
                    titulo = "Periodicas";
                    break;
                case 4:
                    titulo = "de Retiro";
                    break;
            }
            return titulo;
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}