using kinnemed05.Models;
using kinnemed05.Reports;
using kinnemed05.Reports.dataset;
using kinnemed05.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace kinnemed05.Controllers
{
    [CustomAuthorize(UserRoles.admin)]
    public class MedicoController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Medico/

        public ActionResult Index(string sortOrder,string searchString,string exportar,string especialidad)
        {
            ViewBag.NombreSort = String.IsNullOrEmpty(sortOrder) ? "Nombre desc" : "";
            ViewBag.ApellidoSort = sortOrder=="Apellido"?"Apellido desc":"Apellido";
            ViewBag.SearchString = searchString;
            ViewBag.especialidad = new SelectList(db.especialidad, "esp_id", "esp_nombre");
            int cod_especialidad=0;
            //ViewBag.Reporte = exportar;
            var medico = db.medico.Include(m => m.especialidad);
            medico=medico.Where(m=>m.med_estado!=false);
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper();
                medico = medico.Where(s => s.med_nombres.ToUpper().Contains(searchString) || s.med_apellidos.ToUpper().Contains(searchString)|| s.med_cedula.Contains(searchString) );
              
            }
            if(!String.IsNullOrEmpty(especialidad))
                cod_especialidad = Int32.Parse(especialidad);
            if (cod_especialidad!=0)
            {
                medico = medico.Where(s => s.med_especialidad.Equals(cod_especialidad));
                //ViewBag.valor = especialidad;
            }
            switch (sortOrder) { 
                case "Nombre desc":
                    medico = medico.OrderByDescending(s => s.med_nombres);
                    break;
                case "Apellido":
                    medico = medico.OrderBy(s => s.med_apellidos);
                    break;
                case "Apellido desc":
                    medico = medico.OrderByDescending(s => s.med_apellidos);
                    break;
                default:
                    medico = medico.OrderBy(s => s.med_nombres);
                    break;
            }
            if (!String.IsNullOrEmpty(exportar)) {
                //List<medico> list=medico.to;
                //this.ExportReport();
                return RedirectToAction("ExportReport", new { filtro = searchString, especialidad = cod_especialidad });
                //return RedirectToAction("ExportReport",new RouteValueDictionary(new { controller = "Medico", action = "ExportReport", filtro= searchString }));
            }
            
            return View(medico.ToList());
        }

        //
        // GET: /Medico/Details/5

        public ActionResult Details(int id = 0)
        {
            medico medico = db.medico.Find(id);
            if (medico == null)
            {
                return HttpNotFound();
            }
            return View(medico);
        }

        //
        // GET: /Medico/Create

        public ActionResult Create()
        {
            ViewBag.med_especialidad = new SelectList(db.especialidad, "esp_id", "esp_nombre");
            return View();
        }

        //
        // POST: /Medico/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(medico medico)
        {

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                string fileName = Path.GetFileName(file.FileName);
                string ext = Path.GetExtension(fileName);
                string[] formatos = new string[] { ".jpg", ".jpeg", ".bmp", ".png", ".gif", ".JPG", ".JPEG", ".BMP", ".PNG", ".GIF" };
                if (!String.IsNullOrEmpty(fileName) && (Array.IndexOf(formatos, ext) > 0))
                {
                    Firma objfirma = new Firma();
                    medico.med_firma = fileName;
                    string path = Path.Combine(Server.MapPath("~/Content/firmas_"), fileName);
                    string path01 = Path.Combine(Server.MapPath("~/Content/firmas"), fileName);
                    file.SaveAs(path);
                    objfirma.ResizeImage(path, path01, 200, 120);
                }
                else
                {
                    if (!String.IsNullOrEmpty(ext))
                        if (Array.IndexOf(formatos, ext) <= 0)
                            ModelState.AddModelError("ext", "Extensión no Válida");
                }
            }
                
            if (ModelState.IsValid)
            {
                db.medico.Add(medico);
                db.SaveChanges();
                AccountController account = new AccountController();
                account.CreateUserProfile(medico.med_cedula, medico.med_cedula);
                UserManager userManager = new UserManager();
                int Userid = userManager.UpdateMedico(medico.med_cedula, medico.med_id);
                UsersInRoles usersinroles = new UsersInRoles();
                usersinroles.RoleId = 2;
                usersinroles.UserId = Userid;
                account.CreateUsersInRole(usersinroles);
                return RedirectToAction("Index");
            }

            ViewBag.med_especialidad = new SelectList(db.especialidad, "esp_id", "esp_nombre", medico.med_especialidad);
            return View(medico);
        }

        //
        // GET: /Medico/Edit/5

        public ActionResult Edit(int id = 0)
        {
            medico medico = db.medico.Find(id);
            if (medico == null)
            {
                return HttpNotFound();
            }
            ViewBag.med_especialidad = new SelectList(db.especialidad, "esp_id", "esp_nombre", medico.med_especialidad);
            return View(medico);
        }

        //
        // POST: /Medico/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(medico medico)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                string fileName = Path.GetFileName(file.FileName);
                string ext = Path.GetExtension(fileName);
                string[] formatos = new string[] { ".jpg", ".jpeg", ".bmp", ".png", ".gif", ".JPG", ".JPEG", ".BMP", ".PNG", ".GIF" };
                if (!String.IsNullOrEmpty(fileName) && (Array.IndexOf(formatos, ext) > 0))
                {
                    Firma objfirma = new Firma();
                    medico.med_firma = fileName;
                    string path = Path.Combine(Server.MapPath("~/Content/firmas_"), fileName);
                    string path01 = Path.Combine(Server.MapPath("~/Content/firmas"), fileName);
                    file.SaveAs(path);

                    objfirma.ResizeImage(path, path01, 200, 120);
                }
                else
                {
                    if (!String.IsNullOrEmpty(ext))
                        if (Array.IndexOf(formatos, ext) <= 0)
                            ModelState.AddModelError("ext", "Extensión no Válida");
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(medico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.med_especialidad = new SelectList(db.especialidad, "esp_id", "esp_nombre", medico.med_especialidad);
            return View(medico);
        }

        //
        // GET: /Medico/Delete/5

        public ActionResult Delete(int id = 0)
        {
            medico medico = db.medico.Find(id);
            if (medico == null)
            {
                return HttpNotFound();
            }
            return View(medico);
        }

        //
        // POST: /Medico/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            medico medico = db.medico.Find(id);
            db.medico.Remove(medico);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ExportReport(string filtro, int especialidad)
        {
            
            try
            {
                dsMedico dsMedico = new dsMedico();
                string conn = ConfigurationManager.AppSettings["conexion"];
                string strEspecialidad = "Select * from especialidad";
                string strMedico = "Select * from medico";
                if (!String.IsNullOrEmpty(filtro))
                {
                    strMedico += " where med_nombres like '%" + filtro + "%' or med_apellidos like '%" + filtro + "%'";
                }
                if (especialidad != 0 && !String.IsNullOrEmpty(filtro))
                {
                    strMedico += " and med_especialidad=" + especialidad;
                }
                else if (especialidad != 0 && String.IsNullOrEmpty(filtro))
                {
                    strMedico += " where med_especialidad=" + especialidad;
                }
                SqlConnection sqlcon = new SqlConnection(conn);
                SqlDataAdapter daEspecialidad = new SqlDataAdapter(strEspecialidad, sqlcon);
                SqlDataAdapter daMedico = new SqlDataAdapter(strMedico, sqlcon);
                daEspecialidad.Fill(dsMedico, "especialidad");
                daMedico.Fill(dsMedico, "medico");
                Session["ReportSource"] = dsMedico;
                Session["Titulo"] = "Médicos";

                ReportViewerViewModel model = new ReportViewerViewModel();
                string content = Url.Content("~/Reports/RptViewer.aspx");
                model.ReportPath = content;
                return View("ReportViewer", model);
                
                
                //RptMedico rp = new RptMedico();
                //rp.Load(Path.Combine(Server.MapPath("~/Reports"), "RptMedico.rpt"));
                //rp.SetDataSource(dsMedico);
                //rp.SetParameterValue("titulo", "General de Médicos");
                //if (especialidad != 0)
                //{
                //    rp.SetParameterValue("titulo", "Médicos por Especialidad");
                //}
                //Response.Buffer = false;
                //Response.ClearContent();
                //Response.ClearHeaders();
            
                //Stream stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                //stream.Seek(0, SeekOrigin.Begin);
                //return File(stream, "application/pdf", "ReporteMedicos.pdf");
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
                return View("Message");
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}