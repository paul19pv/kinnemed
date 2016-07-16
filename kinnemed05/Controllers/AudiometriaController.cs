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
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace kinnemed05.Controllers
{
    [InitializeSimpleMembership]
    //[CustomAuthorize(UserRoles.laboratorista,UserRoles.medico)]
    public class AudiometriaController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();
        private UsersContext db_users = new UsersContext();
        //
        // GET: /Audiometria/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Index(int? id,int? paciente, int? medico)
        {
            var audiometria = db.audiometria.Include(a => a.paciente);
            if (id != null)
                audiometria = audiometria.Where(a => a.aud_paciente == id);
            if (paciente != null)
                audiometria = audiometria.Where(a => a.aud_paciente == paciente);
            if (medico != null)
                audiometria = audiometria.Where(a => a.aud_medico == medico);
            //if (!String.IsNullOrEmpty(fecha))
            //    audiometria = audiometria.Where(a => a.aud_fecha == fecha);
            if (User.IsInRole("paciente"))
            {
                string cedula = Convert.ToString(User.Identity.Name);
                paciente paciente_ = db.paciente.Where(p => p.pac_cedula == cedula).First();
                audiometria = audiometria.Where(a => a.aud_paciente == paciente_.pac_id);
            }
            if (User.IsInRole("empresa")) {
                string cedula = Convert.ToString(User.Identity.Name);
                empresa empresa = db.empresa.Where(e => e.emp_cedula == cedula).First();
                audiometria = audiometria.Where(a => a.paciente.pac_empresa == empresa.emp_id);
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
                    DateTime dd = DateTime.Now; 
                    audiometria.aud_fecha=dd.Date.ToString("d");
                    audiometria.aud_laboratorista = get_user();
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
            //string nom_pac;
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
                        UserManager usermanager = new UserManager();
                        audiometria.aud_responsable = usermanager.get_user_id(User);
                        audiometria.aud_perfil = usermanager.get_perfil(User);
                        db.Entry(audiometria).State = EntityState.Modified;
                        db.SaveChanges();
                        if(audiometria.aud_observacion!="")
                            notificar(audiometria.aud_paciente);
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
                //var consulta = db.audiometria.Where(a => a.aud_paciente == audiometria.aud_paciente && r.aud_fecha == audiometria.aud_fecha && r.aud_estado == true);
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

        //[AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Descargar(int id)
        {
            try
            {
                string contentType = "application/pdf";
                dsAudiometria dsAudiometria = new dsAudiometria();
                string conn = ConfigurationManager.AppSettings["conexion"];
                int aud_id = id;
                audiometria audiometria = db.audiometria.Find(aud_id);
                //medico medico = db.medico.Find(audiometria.aud_medico);
                string fileName =String.Empty;
                //if (String.IsNullOrEmpty(fileName))
                //    fileName = "firma.png";
                //string path01 = Path.Combine(Server.MapPath("~/Content/firmas"), fileName);

                string strAudiometia = "Select * from view_audiometria where aud_id=" + aud_id;

                SqlConnection sqlcon = new SqlConnection(conn);
                
                SqlDataAdapter daAudiometria = new SqlDataAdapter(strAudiometia, sqlcon);

                daAudiometria.Fill(dsAudiometria, "view_audiometria");

                RptAudiometria_ rp = new RptAudiometria_();
                rp.Load(Path.Combine(Server.MapPath("~/Reports"), "RptAudiometria_.rpt"));
                rp.SetDataSource(dsAudiometria);
                //rp.SetParameterValue("picturePath", path01);
                rp.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Path.Combine(Server.MapPath("~/Content/audiometria"), aud_id + ".pdf"));
                
                //Stream stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                //stream.Seek(0, SeekOrigin.Begin);
                //return File(stream, "application/pdf", aud_id + ".pdf");

                var document = new Document();
                var ms = new MemoryStream();
                string archivo1 = Path.Combine(Server.MapPath("~/Content/audiometria"), audiometria.aud_id + ".pdf");
                string archivo2 = Path.Combine(Server.MapPath("~/Content/audiometria"), audiometria.aud_archivo);
                fileName = "Reporte" + aud_id + ".pdf";
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

        private void notificar( int pac_id){
            string resultado = String.Empty;
            //ModelState.AddModelError("msn", "Llego");
            paciente paciente = db.paciente.Find(pac_id);
            string celular = paciente.pac_celular;
            string correo=paciente.pac_correo;
            Mensaje mensaje = new Mensaje();
            if (!string.IsNullOrEmpty(celular))
            {
                
                resultado = mensaje.enviar(celular, "Los exámenes de audiometria se encuentran listos. Kinnemed");
                
               
            }
            if (!string.IsNullOrEmpty(correo))
            {
                resultado = " " + resultado + mensaje.mail(correo, "Los exámenes de audiometria se encuentran listos. Kinnemed");
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


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}