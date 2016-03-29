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
    public class AudiometriaController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Audiometria/

        public ActionResult Index()
        {
            var audiometria = db.audiometria.Include(a => a.paciente);
            return View(audiometria.ToList());
        }

        //
        // GET: /Audiometria/Details/5

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

        public ActionResult Create()
        {
            //ViewBag.aud_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula");
            return View();
        }

        //
        // POST: /Audiometria/Create

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
                        ModelState.AddModelError("ext", "Extensión no Valida");
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
                        ModelState.AddModelError("ext", "Extensión no Valida");
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

        //
        // POST: /Audiometria/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            audiometria audiometria = db.audiometria.Find(id);
            db.audiometria.Remove(audiometria);
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