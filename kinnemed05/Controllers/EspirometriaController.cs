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
using kinnemed05.Reports.dataset;
using System.Configuration;
using System.Data.SqlClient;
using kinnemed05.Reports;
using iTextSharp.text;
using iTextSharp.text.pdf;

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

            UserManager usermanager = new UserManager();
            string perfil = usermanager.get_perfil(User);
            if (perfil == "paciente")
            {
                string cedula = Convert.ToString(User.Identity.Name);
                paciente paciente_ = db.paciente.Where(p => p.pac_cedula == cedula).First();
                espirometria = espirometria.Where(a => a.esp_paciente == paciente_.pac_id);
            }

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
            medico medico = db.medico.Find(espirometria.esp_medico);
            ViewBag.medico = medico.med_nombres + " " + medico.med_apellidos;
            paciente paciente = db.paciente.Find(espirometria.esp_paciente);
            ViewBag.paciente = paciente.pac_nombres + " " + paciente.pac_apellidos;
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
            string nom_med;
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
            ViewBag.paciente = nom_pac;
            medico medico = db.medico.Find(espirometria.esp_medico);
            if (medico != null)
                nom_med = medico.med_nombres + " " + medico.med_apellidos;
            else
                nom_med = "";
            ViewBag.medico = nom_med;
            return View(espirometria);
        }

        //
        // GET: /Espirometria/Edit/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico,UserRoles.admin)]
        public ActionResult Edit(int id = 0)
        {
            espirometria espirometria = db.espirometria.Find(id);
            if (espirometria == null)
            {
                return HttpNotFound();
            }
            
            medico medico = db.medico.Find(espirometria.esp_medico);
            ViewBag.medico = medico.med_nombres + " " + medico.med_apellidos;
            paciente paciente = db.paciente.Find(espirometria.esp_paciente);
            ViewBag.paciente = paciente.pac_nombres + " " + paciente.pac_apellidos;
            return View(espirometria);
        }

        //
        // POST: /Espirometria/Edit/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(espirometria espirometria)
        {
            
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
            medico medico = db.medico.Find(espirometria.esp_medico);
            ViewBag.medico = medico.med_nombres + " " + medico.med_apellidos;
            paciente paciente = db.paciente.Find(espirometria.esp_paciente);
            ViewBag.paciente = paciente.pac_nombres + " " + paciente.pac_apellidos;
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
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Reporte(int id)
        {
            try
            {
                //var consulta = db.registro.Where(r => r.reg_paciente == registro.reg_paciente && r.reg_fecha == registro.reg_fecha && r.reg_estado == true);
                //if (!consulta.Any())
                //    return RedirectToAction("Message", "Home", new { mensaje = "El paciente no tiene exámenes para esta fecha" });

                Session["esp_id"] = id;
                ReportViewerViewModel model = new ReportViewerViewModel();
                string content = Url.Content("~/Reports/Viewer/ViewEspirometria.aspx");
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



        public ActionResult Descargar(int id)
        {
            try
            {
                string contentType = "application/pdf";
                dsEspirometria dsPrueba = new dsEspirometria();
                string conn = ConfigurationManager.AppSettings["conexion"];
                espirometria espirometria = db.espirometria.Find(id);
                medico medico = db.medico.Find(espirometria.esp_medico);
                string fileName = String.Empty;
                //if (String.IsNullOrEmpty(fileName))
                //    fileName = "firma.png";
                //string path01 = Path.Combine(Server.MapPath("~/Content/firmas"), fileName);

                string strEspirometria = "Select * from espirometria where esp_id=" + id;
                string strPaciente = "Select * from paciente where pac_id=" + espirometria.esp_paciente;
                string strMedico = "Select * from medico where med_id=" + espirometria.esp_medico;


                SqlConnection sqlcon = new SqlConnection(conn);
                SqlDataAdapter daEspirometria = new SqlDataAdapter(strEspirometria, sqlcon);
                SqlDataAdapter daPaciente = new SqlDataAdapter(strPaciente, sqlcon);
                SqlDataAdapter daMedico = new SqlDataAdapter(strMedico, sqlcon);
                daEspirometria.Fill(dsPrueba, "espirometria");
                daPaciente.Fill(dsPrueba, "paciente");
                daMedico.Fill(dsPrueba, "medico");

                RptEspirometria_ rp = new RptEspirometria_();
                rp.Load(Path.Combine(Server.MapPath("~/Reports"), "RptEspirometria_.rpt"));
                rp.SetDataSource(dsPrueba);
                rp.SetParameterValue("hc", "");
                rp.SetParameterValue("orden", "");
                //rp.SetParameterValue("picturePath", path01);
                rp.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Path.Combine(Server.MapPath("~/Content/audiometria"), id + ".pdf"));

                //Stream stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                //stream.Seek(0, SeekOrigin.Begin);
                //return File(stream, "application/pdf", aud_id + ".pdf");

                var document = new Document();
                var ms = new MemoryStream();
                string archivo1 = Path.Combine(Server.MapPath("~/Content/audiometria"), id + ".pdf");
                string archivo2 = Path.Combine(Server.MapPath("~/Content/audiometria"), espirometria.esp_archivo);
                fileName = "Reporte" + id + ".pdf";
                string fileTarget = Path.Combine(Server.MapPath("~/Content/audiometria/") + fileName);
                string[] Lista = { archivo1, archivo2 };

                Merge(fileTarget, Lista);
                return File(fileTarget, contentType, fileName);

            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
                //return View("Message");
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message });
            }
        }

        internal static bool Merge(string strFileTarget, string[] arrStrFilesSource)
        {
            bool blnMerged = false;

            // Crea el PDF de salida
            try
            {
                using (System.IO.FileStream stmFile = new System.IO.FileStream(strFileTarget, System.IO.FileMode.Create))
                {
                    Document objDocument = null;
                    PdfWriter objWriter = null;

                    // Recorre los archivos
                    for (int intIndexFile = 0; intIndexFile < arrStrFilesSource.Length; intIndexFile++)
                    {
                        PdfReader objReader = new PdfReader(arrStrFilesSource[intIndexFile]);
                        int intNumberOfPages = objReader.NumberOfPages;

                        // La primera vez, inicializa el documento y el escritor
                        if (intIndexFile == 0)
                        { // Asigna el documento y el generador
                            objDocument = new Document(objReader.GetPageSizeWithRotation(1));
                            objWriter = PdfWriter.GetInstance(objDocument, stmFile);
                            // Abre el documento
                            objDocument.Open();
                        }
                        // Añade las páginas
                        for (int intPage = 0; intPage < intNumberOfPages; intPage++)
                        {
                            int intRotation = objReader.GetPageRotation(intPage + 1);
                            PdfImportedPage objPage = objWriter.GetImportedPage(objReader, intPage + 1);

                            // Asigna el tamaño de la página
                            objDocument.SetPageSize(objReader.GetPageSizeWithRotation(intPage + 1));
                            // Crea una nueva página
                            objDocument.NewPage();
                            // Añade la página leída
                            if (intRotation == 90 || intRotation == 270)
                                objWriter.DirectContent.AddTemplate(objPage, 0, -1f, 1f, 0, 0,
                                                                    objReader.GetPageSizeWithRotation(intPage + 1).Height);
                            else
                                objWriter.DirectContent.AddTemplate(objPage, 1f, 0, 0, 1f, 0, 0);
                        }
                    }
                    // Cierra el documento
                    if (objDocument != null)
                        objDocument.Close();
                    // Cierra el stream del archivo
                    stmFile.Close();
                }
                // Indica que se ha creado el documento
                blnMerged = true;
            }
            catch (Exception objException)
            {
                System.Diagnostics.Debug.WriteLine(objException.Message);
            }
            // Devuelve el valor que indica si se han mezclado los archivos
            return blnMerged;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}