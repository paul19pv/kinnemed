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
using kinnemed05.Reports.dataset;
using System.Configuration;
using System.Data.SqlClient;
using kinnemed05.Reports;

namespace kinnemed05.Controllers
{
    [InitializeSimpleMembership]
    //[CustomAuthorize(UserRoles.laboratorista)]
    public class RayosController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();
        private UsersContext db_users = new UsersContext();

        //
        // GET: /Rayos/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Index(int? id, int? paciente)
        {
            var rayos = db.rayos.Include(r => r.paciente);
            if (id != null)
                rayos = rayos.Where(r => r.ray_paciente == id);
            if (paciente != null)
                rayos = rayos.Where(r => r.ray_paciente == paciente);
            
            if (Request.IsAjaxRequest())
                return PartialView("Index_historia", rayos.ToList());
            if (User.IsInRole("paciente"))
            {
                string cedula = Convert.ToString(User.Identity.Name);
                paciente paciente_ = db.paciente.Where(p => p.pac_cedula == cedula).First();
                rayos = rayos.Where(r => r.ray_paciente == paciente_.pac_id);
            }
            if (User.IsInRole("empresa"))
            {
                string cedula = Convert.ToString(User.Identity.Name);
                empresa empresa = db.empresa.Where(e => e.emp_cedula == cedula).First();
                rayos = rayos.Where(r => r.paciente.pac_empresa == empresa.emp_id);
            }


            return View(rayos.ToList());
        }

        //
        // GET: /Rayos/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Details(int id = 0)
        {
            rayos rayos = db.rayos.Find(id);
            if (rayos == null)
            {
                return HttpNotFound();
            }
            paciente paciente = db.paciente.Find(rayos.ray_paciente);
            ViewBag.paciente = paciente.pac_nombres + " " + paciente.pac_apellidos;
            
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
                    DateTime dd = DateTime.Now;
                    rayos.ray_fecha = dd.Date.ToString("d");
                    rayos.ray_laboratorista = get_user();
                    rayos.ray_orden = get_orden(rayos.ray_fecha);
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
            
            return View(rayos);
        }

        //
        // POST: /Rayos/Edit/5

        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(rayos rayos)
        {
            //string nom_pac;
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
                        UserManager usermanager = new UserManager();
                        rayos.ray_responsable = usermanager.get_user_id(User);
                        rayos.ray_perfil = usermanager.get_perfil(User);
                        db.Entry(rayos).State = EntityState.Modified;
                        db.SaveChanges();
                        if (rayos.ray_observacion != "")
                            notificar(rayos.ray_paciente);
                        return RedirectToAction("Index");
                    }
                }


            }

            paciente paciente = db.paciente.Find(rayos.ray_paciente);
            ViewBag.paciente = paciente.pac_nombres + " " + paciente.pac_apellidos;
            
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
        public ActionResult Descargar(int id) {
            try
            {
                dsRayos dsRayos = new dsRayos();
                string conn = ConfigurationManager.AppSettings["conexion"];
                int ray_id = id;
                string strRayos = "Select * from view_rayos where ray_id="+ray_id;
                SqlConnection sqlcon = new SqlConnection(conn);
                SqlDataAdapter daRayos = new SqlDataAdapter(strRayos, sqlcon);
                daRayos.Fill(dsRayos, "view_rayos");
                RptRayos_ rp = new RptRayos_();
                string reportPath = Server.MapPath("~/Reports/RptRayos_.rpt");
                rp.Load(reportPath);
                rp.SetDataSource(dsRayos);

                Stream stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", ray_id + ".pdf");
            }
            catch (Exception ex) {
                ViewBag.mensaje = ex.Message;
                //return View("Message");
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message });
            }
        }
        private void notificar(int pac_id)
        {
            string resultado = String.Empty;
            //ModelState.AddModelError("msn", "Llego");
            paciente paciente = db.paciente.Find(pac_id);
            string celular = paciente.pac_celular;
            string correo = paciente.pac_correo;
            Mensaje mensaje = new Mensaje();
            if (!string.IsNullOrEmpty(celular))
            {

                resultado = mensaje.enviar(celular, "Sr.(a) Paciente sus exámenes realizados en el Centro Médico Kinnmed están listos reviselos en kinnemed.com con cédula para usuario y clave");


            }
            if (!string.IsNullOrEmpty(correo))
            {
                resultado = " " + resultado + mensaje.mail(correo, "Sr.(a) Paciente sus exámenes realizados en el Centro Médico Kinnmed están listos reviselos en kinnemed.com con cédula para usuario y clave");
            }
            ModelState.AddModelError("notificacion", resultado);


        }
        private int get_user()
        {
            int user_id = 0;
            if (Request.IsAuthenticated)
            {
                string user_name = String.Empty;
                user_name = User.Identity.Name;
                UserProfile userprofile = db_users.UserProfiles.Where(u => u.UserName == user_name).First();
                user_id = userprofile.UserLaboratorista.GetValueOrDefault();
            }
            return user_id;
        }

        private int get_orden(string fecha)
        {
            string orden = String.Empty;
            int num = 0;
            int num_exa = 0;
            var consulta = db.rayos.Where(r => r.ray_fecha == fecha);
            if (consulta.Any())
                num_exa = db.rayos.Where(r => r.ray_fecha == fecha).OrderByDescending(r => r.ray_orden).First().ray_orden.GetValueOrDefault();
            else
                num_exa = 0;
            num = num_exa + 1;
            return num;
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}