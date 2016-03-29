using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kinnemed05.Models;
using System.IO;

namespace kinnemed05.Controllers
{
    public class EspirometriaController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Espirometria/

        public ActionResult Index()
        {
            var espirometria = db.espirometria.Include(e => e.paciente);
            return View(espirometria.ToList());
        }

        //
        // GET: /Espirometria/Details/5

        public ActionResult Details(int id = 0)
        {
            espirometria espirometria = db.espirometria.Find(id);
            if (espirometria == null)
            {
                return HttpNotFound();
            }
            return View(espirometria);
        }

        //
        // GET: /Espirometria/Create

        public ActionResult Create()
        {
            //ViewBag.esp_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula");
            return View();
        }

        //
        // POST: /Espirometria/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create(HttpPostedFileBase FileUpload,espirometria espirometria)
        public ActionResult Create(espirometria espirometria)
        {
            
            
            if (Request.Files.Count>0) {
                var file = Request.Files[0];
                string fileName = Path.GetFileName(file.FileName);
                espirometria.esp_archivo = fileName;
                string ext = Path.GetExtension(fileName);
                if (ext == ".pdf")
                {
                    if (ModelState.IsValid)
                    {
                        string path = Path.Combine(Server.MapPath("~/Content/espirometria"), fileName);

                        file.SaveAs(path);
                        db.espirometria.Add(espirometria);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            
            
            
            //ViewBag.esp_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula", espirometria.esp_paciente);
            return View(espirometria);
        }

        //
        // GET: /Espirometria/Edit/5

        public ActionResult Edit(int id = 0)
        {
            espirometria espirometria = db.espirometria.Find(id);
            if (espirometria == null)
            {
                return HttpNotFound();
            }
            ViewBag.esp_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula", espirometria.esp_paciente);
            return View(espirometria);
        }

        //
        // POST: /Espirometria/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(espirometria espirometria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(espirometria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.esp_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula", espirometria.esp_paciente);
            return View(espirometria);
        }

        //
        // GET: /Espirometria/Delete/5

        public ActionResult Delete(int id = 0)
        {
            espirometria espirometria = db.espirometria.Find(id);
            if (espirometria == null)
            {
                return HttpNotFound();
            }
            return View(espirometria);
        }
        public ActionResult Download(int id=0) {
            string contentType = "application/pdf";
            espirometria espirometria = db.espirometria.Find(id);
            if (espirometria == null)
            {
                return HttpNotFound();
            }
            return File(Server.MapPath("~/Content/espirometria/")+espirometria.esp_archivo, contentType, "Report.pdf");

        }
        //
        // POST: /Espirometria/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            espirometria espirometria = db.espirometria.Find(id);
            db.espirometria.Remove(espirometria);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}