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
    //[CustomAuthorize(UserRoles.laboratorista)]
    public class RayosController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Rayos/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Index(int? id)
        {
            var rayos = db.rayos.Include(r => r.paciente);
            if (id != null)
                rayos = rayos.Where(r => r.ray_paciente == id);
            if (Request.IsAjaxRequest())
                return PartialView("Index_historia", rayos.ToList());
            
            UserManager usermanager = new UserManager();
            string perfil = usermanager.get_perfil(User);
            if (perfil == "paciente")
            {
                string cedula = Convert.ToString(User.Identity.Name);
                paciente paciente_ = db.paciente.Where(p => p.pac_cedula == cedula).First();
                rayos = rayos.Where(a => a.ray_paciente == paciente_.pac_id);
            }


            return View(rayos.ToList());
        }

        //
        // GET: /Rayos/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Details(int id = 0)
        {
            rayos rayos = db.rayos.Find(id);
            if (rayos == null)
            {
                return HttpNotFound();
            }
            paciente paciente = db.paciente.Find(rayos.ray_paciente);
            ViewBag.paciente = paciente.pac_nombres + " " + paciente.pac_apellidos;
            medico medico = db.medico.Find(rayos.ray_medico);
            ViewBag.medico = medico.med_nombres + " " + medico.med_apellidos;
            return View(rayos);
        }

        //
        // GET: /Rayos/Create

        [CustomAuthorize(UserRoles.laboratorista)]
        public ActionResult Create()
        {
            //ViewBag.ray_paciente = new SelectList(db.paciente, "pac_id", "pac_cedula");
            return View();
        }

        //
        // POST: /Rayos/Create

        [CustomAuthorize(UserRoles.laboratorista)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(rayos rayos)
        {
            string nom_pac;
            string nom_med;
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                string fileName = Path.GetFileName(file.FileName);
                string ext = Path.GetExtension(fileName);
                string[] formatos = new string[] { ".jpg", ".jpeg", ".bmp", ".png", ".gif", ".JPG", ".JPEG", ".BMP", ".PNG", ".GIF" };
                if (fileName != "")
                {
                    rayos.ray_imagen = fileName;
                    if (ModelState.IsValid && (Array.IndexOf(formatos, ext) >= 0))
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
                        ModelState.AddModelError("ext", "Extensión no Válida "+ext);
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
            ViewBag.paciente = nom_pac;
            medico medico = db.medico.Find(rayos.ray_paciente);
            if (medico != null)
                nom_med = medico.med_nombres + " " + medico.med_apellidos;
            else
                nom_med = "";
            ViewBag.medico = nom_med;
            return View(rayos);
        }

        //
        // GET: /Rayos/Edit/5

        [CustomAuthorize(UserRoles.laboratorista,UserRoles.medico,UserRoles.admin)]
        public ActionResult Edit(int id = 0)
        {
            rayos rayos = db.rayos.Find(id);
            if (rayos == null)
            {
                return HttpNotFound();
            }
            paciente paciente = db.paciente.Find(rayos.ray_paciente);
            ViewBag.paciente = paciente.pac_nombres + " " + paciente.pac_apellidos;
            medico medico = db.medico.Find(rayos.ray_medico);
            ViewBag.medico = medico.med_nombres + " " + medico.med_apellidos;
            return View(rayos);
        }

        //
        // POST: /Rayos/Edit/5

        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.admin)]
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
                string[] formatos = new string[] { ".jpg", ".jpeg", ".bmp", ".png", ".gif", ".JPG", ".JPEG", ".BMP", ".PNG", ".GIF" };
                if (fileName != "")
                {
                    if (ModelState.IsValid && (Array.IndexOf(formatos, ext) >= 0))
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
                        ModelState.AddModelError("ext", "Extensión no Válida");
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
            ViewBag.paciente = paciente.pac_nombres + " " + paciente.pac_apellidos;
            medico medico = db.medico.Find(rayos.ray_medico);
            ViewBag.medico = medico.med_nombres + " " + medico.med_apellidos;
            return View(rayos);
        }

        //
        // GET: /Rayos/Delete/5

        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
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



        //
        // POST: /Rayos/Delete/5

        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            rayos rayos = db.rayos.Find(id);
            db.rayos.Remove(rayos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Download(int id = 0)
        {
            rayos rayos = db.rayos.Find(id);
            string[] filename = rayos.ray_imagen.Split('.');
            string contentType = "application/" + filename[1];
            if (rayos == null)
            {
                return HttpNotFound();
            }
            return File(Server.MapPath("~/Content/rayos/") + rayos.ray_imagen, contentType, rayos.ray_imagen);

        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Reporte(int id)
        {
            try
            {
                //var consulta = db.registro.Where(r => r.reg_paciente == registro.reg_paciente && r.reg_fecha == registro.reg_fecha && r.reg_estado == true);
                //if (!consulta.Any())
                //    return RedirectToAction("Message", "Home", new { mensaje = "El paciente no tiene exámenes para esta fecha" });

                Session["ray_id"] = id;
                ReportViewerViewModel model = new ReportViewerViewModel();
                string content = Url.Content("~/Reports/Viewer/ViewRayos.aspx");
                model.ReportPath = content;
                return View("ReportViewer", model);
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
                //return View("Message");
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message });
            }
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}