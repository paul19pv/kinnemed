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
            return View(ocupacional.ToList());
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
            int his_id = Convert.ToInt32(Session["his_id"]);
            historia historia = db.historia.Find(his_id);
            ViewBag.ocu_paciente = id;
            ViewBag.ocu_tipo = historia.his_tipo;
            return View();
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
                return RedirectToAction("Index");
            }

            ViewBag.ocu_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula", ocupacional.ocu_paciente);
            return View(ocupacional);
        }

        //
        // GET: /Ocupacional/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ocupacional ocupacional = db.ocupacional.Find(id);
            if (ocupacional == null)
            {
                return HttpNotFound();
            }
            ViewBag.ocu_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula", ocupacional.ocu_paciente);
            return View(ocupacional);
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
                return RedirectToAction("Index");
            }
            ViewBag.ocu_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula", ocupacional.ocu_paciente);
            return View(ocupacional);
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}