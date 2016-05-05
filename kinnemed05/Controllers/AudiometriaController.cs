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
            UserManager usermanager = new UserManager();
            string perfil = usermanager.get_perfil(User);
            if (perfil == "paciente")
            {
                string cedula = Convert.ToString(User.Identity.Name);
                paciente paciente_ = db.paciente.Where(p => p.pac_cedula == cedula).First();
                audiometria = audiometria.Where(a => a.aud_paciente == paciente_.pac_id);
            }


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
            paciente paciente = db.paciente.Find(audiometria.aud_paciente);
            ViewBag.paciente = paciente.pac_nombres + " " + paciente.pac_apellidos;
            medico medico = db.medico.Find(audiometria.aud_medico);
            ViewBag.medico = medico.med_nombres + " " + medico.med_apellidos;
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
            string nom_med;
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
            ViewBag.paciente = nom_pac;
            medico medico = db.medico.Find(audiometria.aud_medico);
            if (medico != null)
                nom_med = medico.med_nombres + " " + medico.med_apellidos;
            else
                nom_med = "";
            ViewBag.medico = nom_med;
            return View(audiometria);
        }

        //
        // GET: /Audiometria/Edit/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico,UserRoles.admin)]
        public ActionResult Edit(int id = 0)
        {
            audiometria audiometria = db.audiometria.Find(id);
            if (audiometria == null)
            {
                return HttpNotFound();
            }
            
            paciente paciente = db.paciente.Find(audiometria.aud_paciente);
            ViewBag.paciente = paciente.pac_nombres + " " + paciente.pac_apellidos;
            medico medico = db.medico.Find(audiometria.aud_medico);
            ViewBag.medico = medico.med_nombres + " " + medico.med_apellidos;
            
            
            return View(audiometria);
        }

        //
        // POST: /Audiometria/Edit/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.admin)]
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
            ViewBag.paciente = paciente.pac_nombres + " " + paciente.pac_apellidos;
            medico medico = db.medico.Find(audiometria.aud_medico);
            ViewBag.medico = medico.med_nombres + " " + medico.med_apellidos;
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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Reporte(int id)
        {
            try
            {
                //var consulta = db.registro.Where(r => r.reg_paciente == registro.reg_paciente && r.reg_fecha == registro.reg_fecha && r.reg_estado == true);
                //if (!consulta.Any())
                //    return RedirectToAction("Message", "Home", new { mensaje = "El paciente no tiene exámenes para esta fecha" });
                
                Session["aud_id"] = id;
                ReportViewerViewModel model = new ReportViewerViewModel();
                string content = Url.Content("~/Reports/Viewer/ViewAudiometria.aspx");
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