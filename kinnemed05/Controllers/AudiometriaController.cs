using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kinnemed05.Models;
using System.IO;
using kinnemed05.Security;
using kinnemed05.Filters;

namespace kinnemed05.Controllers
{
    [InitializeSimpleMembership]
    //[CustomAuthorize(UserRoles.laboratorista,UserRoles.medico)]
    public class AudiometriaController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Audiometria/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Index(int? id)
        {
            var audiometria = db.audiometria.Include(a => a.paciente);
            if (id != null)
                audiometria = audiometria.Where(a => a.aud_paciente == id);
            if (Request.IsAjaxRequest())
                return PartialView("Index_historia", audiometria.ToList());
            return View(audiometria.ToList());
        }

        //
        // GET: /Audiometria/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Details(int id = 0)
        {
            audiometria audiometria = db.audiometria.Find(id);
            if (audiometria == null)
            {
                return HttpNotFound();
            }
            string nom_pac = String.Empty;
            paciente paciente = db.paciente.Find(audiometria.aud_paciente);
            if (paciente != null)
                nom_pac = paciente.pac_nombres + " " + paciente.pac_apellidos;
            else
                nom_pac = "";
            @ViewBag.paciente = nom_pac;
            return View(audiometria);
        }

        //
        // GET: /Audiometria/Create
        [CustomAuthorize(UserRoles.laboratorista,UserRoles.medico)]
        public ActionResult Create()
        {
            //ViewBag.aud_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula");
            return View();
        }

        //
        // POST: /Audiometria/Create
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(audiometria audiometria)
        {
            string nom_pac;
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                string fileName = Path.GetFileName(file.FileName);
                string ext = Path.GetExtension(fileName);
                if (fileName != "")
                {
                    audiometria.aud_archivo = fileName;
                    if (ModelState.IsValid && ext == ".pdf")
                    {
                        string path = Path.Combine(Server.MapPath("~/Content/audiometria"), fileName);
                        file.SaveAs(path);
                        db.audiometria.Add(audiometria);
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

            paciente paciente = db.paciente.Find(audiometria.aud_paciente);
            if (paciente != null)
                nom_pac = paciente.pac_nombres + " " + paciente.pac_apellidos;
            else
                nom_pac = "";
            @ViewBag.paciente = nom_pac;
            return View(audiometria);
        }

        //
        // GET: /Audiometria/Edit/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico)]
        public ActionResult Edit(int id = 0)
        {
            audiometria audiometria = db.audiometria.Find(id);
            if (audiometria == null)
            {
                return HttpNotFound();
            }
            paciente paciente = db.paciente.Find(audiometria.aud_paciente);
            string nom_pac = String.Empty;
            if (paciente != null)
                nom_pac = paciente.pac_nombres + " " + paciente.pac_apellidos;
            else
                nom_pac = "";
            @ViewBag.paciente = nom_pac;
            return View(audiometria);
        }

        //
        // POST: /Audiometria/Edit/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(audiometria audiometria)
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
                        if (fileName != audiometria.aud_archivo)
                        {
                            string path = Path.Combine(Server.MapPath("~/Content/audiometria"), fileName);
                            file.SaveAs(path);
                        }
                        audiometria.aud_archivo = fileName;
                        db.Entry(audiometria).State = EntityState.Modified;
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
                        db.Entry(audiometria).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }

            paciente paciente = db.paciente.Find(audiometria.aud_paciente);
            if (paciente != null)
                nom_pac = paciente.pac_nombres + " " + paciente.pac_apellidos;
            else
                nom_pac = "";
            @ViewBag.paciente = nom_pac;
            return View(audiometria);
        }

        //
        // GET: /Audiometria/Delete/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        public ActionResult Delete(int id = 0)
        {
            audiometria audiometria = db.audiometria.Find(id);
            if (audiometria == null)
            {
                return HttpNotFound();
            }
            paciente paciente = db.paciente.Find(audiometria.aud_paciente);
            string nom_pac = String.Empty;
            if (paciente != null)
                nom_pac = paciente.pac_nombres + " " + paciente.pac_apellidos;
            else
                nom_pac = "";
            @ViewBag.paciente = nom_pac;
            return View(audiometria);
        }

        

        //
        // POST: /Audiometria/Delete/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            audiometria audiometria = db.audiometria.Find(id);
            db.audiometria.Remove(audiometria);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Download(int id = 0)
        {
            string contentType = "application/pdf";
            audiometria audiometria = db.audiometria.Find(id);
            if (audiometria == null)
            {
                return HttpNotFound();
            }
            return File(Server.MapPath("~/Content/audiometria/") + audiometria.aud_archivo, contentType, audiometria.aud_archivo);

        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}