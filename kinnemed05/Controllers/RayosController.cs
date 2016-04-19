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
    [CustomAuthorize(UserRoles.laboratorista)]
    public class RayosController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Rayos/

        public ActionResult Index()
        {
            var rayos = db.rayos.Include(r => r.paciente);
            return View(rayos.ToList());
        }

        //
        // GET: /Rayos/Details/5

        public ActionResult Details(int id = 0)
        {
            rayos rayos = db.rayos.Find(id);
            if (rayos == null)
            {
                return HttpNotFound();
            }
            string nom_pac = String.Empty;
            paciente paciente = db.paciente.Find(rayos.ray_paciente);
            if (paciente != null)
                nom_pac = paciente.pac_nombres + " " + paciente.pac_apellidos;
            else
                nom_pac = "";
            @ViewBag.paciente = nom_pac;
            return View(rayos);
        }

        //
        // GET: /Rayos/Create

        public ActionResult Create()
        {
            //ViewBag.ray_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula");
            return View();
        }

        //
        // POST: /Rayos/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(rayos rayos)
        {
            string nom_pac;
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                string fileName = Path.GetFileName(file.FileName);
                string ext = Path.GetExtension(fileName);
                string[] formatos = new string[] { ".jpg", ".jpeg", ".bmp", ".png", ".gif" };
                if (fileName != "")
                {
                    rayos.ray_imagen = fileName;
                    if (ModelState.IsValid && (Array.IndexOf(formatos, ext) > 0))
                    //if (ModelState.IsValid)
                    {
                        string path = Path.Combine(Server.MapPath("~/Content/rayos"), fileName);
                        file.SaveAs(path);
                        db.rayos.Add(rayos);
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

            paciente paciente = db.paciente.Find(rayos.ray_paciente);
            if (paciente != null)
                nom_pac = paciente.pac_nombres + " " + paciente.pac_apellidos;
            else
                nom_pac = "";
            @ViewBag.paciente = nom_pac;
            return View(rayos);
        }

        //
        // GET: /Rayos/Edit/5

        public ActionResult Edit(int id = 0)
        {
            rayos rayos = db.rayos.Find(id);
            if (rayos == null)
            {
                return HttpNotFound();
            }
            paciente paciente = db.paciente.Find(rayos.ray_paciente);
            string nom_pac = String.Empty;
            if (paciente != null)
                nom_pac = paciente.pac_nombres + " " + paciente.pac_apellidos;
            else
                nom_pac = "";
            @ViewBag.paciente = nom_pac;
            return View(rayos);
        }

        //
        // POST: /Rayos/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(rayos rayos)
        {
            string nom_pac;
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                string fileName = Path.GetFileName(file.FileName);
                string ext = Path.GetExtension(fileName);
                string[] formatos = new string[] { ".jpg", ".jpeg", ".bmp", ".png", ".gif" };
                if (fileName != "")
                {
                    if (ModelState.IsValid && (Array.IndexOf(formatos, ext) > 0))
                    {
                        if (fileName != rayos.ray_imagen)
                        {
                            string path = Path.Combine(Server.MapPath("~/Content/rayos"), fileName);
                            file.SaveAs(path);
                        }
                        rayos.ray_imagen = fileName;
                        db.Entry(rayos).State = EntityState.Modified;
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
                        db.Entry(rayos).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }


            }

            paciente paciente = db.paciente.Find(rayos.ray_paciente);
            if (paciente != null)
                nom_pac = paciente.pac_nombres + " " + paciente.pac_apellidos;
            else
                nom_pac = "";
            @ViewBag.paciente = nom_pac;
            return View(rayos);
        }

        //
        // GET: /Rayos/Delete/5

        public ActionResult Delete(int id = 0)
        {
            rayos rayos = db.rayos.Find(id);
            if (rayos == null)
            {
                return HttpNotFound();
            }
            paciente paciente = db.paciente.Find(rayos.ray_paciente);
            string nom_pac = String.Empty;
            if (paciente != null)
                nom_pac = paciente.pac_nombres + " " + paciente.pac_apellidos;
            else
                nom_pac = "";
            @ViewBag.paciente = nom_pac;
            return View(rayos);
        }

        public ActionResult Download(int id = 0)
        {
            rayos rayos = db.rayos.Find(id);
            string[] filename = rayos.ray_imagen.Split('.');
            string contentType = "application/"+filename[1];
            if (rayos == null)
            {
                return HttpNotFound();
            }
            return File(Server.MapPath("~/Content/rayos/") + rayos.ray_imagen, contentType, rayos.ray_imagen);

        }

        //
        // POST: /Rayos/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            rayos rayos = db.rayos.Find(id);
            db.rayos.Remove(rayos);
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