using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kinnemed05.Models;
using System.IO;
using kinnemed05.Filters;
using kinnemed05.Security;

namespace kinnemed05.Controllers
{
    [InitializeSimpleMembership]
    //[CustomAuthorize(UserRoles.laboratorista,UserRoles.medico)]
    public class EspirometriaController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Espirometria/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Index(int? id)
        {
            var espirometria = db.espirometria.Include(e => e.paciente);
            if (id != null)
                espirometria = espirometria.Where(e => e.esp_paciente == id);
            if (Request.IsAjaxRequest())
                return PartialView("Index_historia", espirometria.ToList());
            return View(espirometria.ToList());
        }

        //
        // GET: /Espirometria/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Details(int id = 0)
        {
            espirometria espirometria = db.espirometria.Find(id);
            if (espirometria == null)
            {
                return HttpNotFound();
            }
            string nom_pac = String.Empty;
            paciente paciente = db.paciente.Find(espirometria.esp_paciente);
            if (paciente != null)
                nom_pac = paciente.pac_nombres + " " + paciente.pac_apellidos;
            else
                nom_pac = "";
            @ViewBag.paciente = nom_pac;
            return View(espirometria);
        }

        //
        // GET: /Espirometria/Create
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico)]
        public ActionResult Create()
        {
            //ViewBag.esp_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula");
            return View();
        }

        //
        // POST: /Espirometria/Create
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create(HttpPostedFileBase FileUpload,espirometria espirometria)
        public ActionResult Create(espirometria espirometria)
        {
            string nom_pac;
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                string fileName = Path.GetFileName(file.FileName);
                string ext = Path.GetExtension(fileName);
                if (fileName != "") {
                    espirometria.esp_archivo = fileName;
                    if (ModelState.IsValid && ext == ".pdf")
                    {
                        string path = Path.Combine(Server.MapPath("~/Content/espirometria"), fileName);
                        file.SaveAs(path);
                        db.espirometria.Add(espirometria);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("ext", "Extensión no Válida");
                    }
                }
                else
                {
                    ModelState.AddModelError("ext", "Debe seleccionar un archivo");
                }
                

            }

            paciente paciente = db.paciente.Find(espirometria.esp_paciente);
            if (paciente != null)
                nom_pac = paciente.pac_nombres + " " + paciente.pac_apellidos;
            else
                nom_pac = "";
            @ViewBag.paciente = nom_pac;
            return View(espirometria);
        }

        //
        // GET: /Espirometria/Edit/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico)]
        public ActionResult Edit(int id = 0)
        {
            espirometria espirometria = db.espirometria.Find(id);
            if (espirometria == null)
            {
                return HttpNotFound();
            }
            paciente paciente = db.paciente.Find(espirometria.esp_paciente);
            string nom_pac = String.Empty;
            if (paciente != null)
                nom_pac = paciente.pac_nombres + " " + paciente.pac_apellidos;
            else
                nom_pac = "";
            @ViewBag.paciente = nom_pac;
            return View(espirometria);
        }

        //
        // POST: /Espirometria/Edit/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(espirometria espirometria)
        {
            string nom_pac;
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                string fileName = Path.GetFileName(file.FileName);
                string ext = Path.GetExtension(fileName);
                if (fileName != "")
                {
                    if (ModelState.IsValid && ext == ".pdf")
                    {
                        if (fileName != espirometria.esp_archivo)
                        {
                            string path = Path.Combine(Server.MapPath("~/Content/espirometria"), fileName);
                            file.SaveAs(path);
                        }
                        espirometria.esp_archivo = fileName;
                        db.Entry(espirometria).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("ext", "Extensión no Válida");
                    }
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(espirometria).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }


            }

            paciente paciente = db.paciente.Find(espirometria.esp_paciente);
            if (paciente != null)
                nom_pac = paciente.pac_nombres + " " + paciente.pac_apellidos;
            else
                nom_pac = "";
            @ViewBag.paciente = nom_pac;
            return View(espirometria);
        }

        //
        // GET: /Espirometria/Delete/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        public ActionResult Delete(int id = 0)
        {
            espirometria espirometria = db.espirometria.Find(id);
            if (espirometria == null)
            {
                return HttpNotFound();
            }
            paciente paciente = db.paciente.Find(espirometria.esp_paciente);
            string nom_pac = String.Empty;
            if (paciente != null)
                nom_pac = paciente.pac_nombres + " " + paciente.pac_apellidos;
            else
                nom_pac = "";
            @ViewBag.paciente = nom_pac;
            return View(espirometria);
        }
       
        //
        // POST: /Espirometria/Delete/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            espirometria espirometria = db.espirometria.Find(id);
            db.espirometria.Remove(espirometria);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Download(int id = 0)
        {
            string contentType = "application/pdf";
            espirometria espirometria = db.espirometria.Find(id);
            if (espirometria == null)
            {
                return HttpNotFound();
            }
            return File(Server.MapPath("~/Content/espirometria/") + espirometria.esp_archivo, contentType, espirometria.esp_archivo);

        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}