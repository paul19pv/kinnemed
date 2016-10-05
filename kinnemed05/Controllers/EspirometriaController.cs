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
        private UsersContext db_users = new UsersContext();

        //
        // GET: /Espirometria/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Index(int? id, int? paciente)
        {
            var espirometria = db.espirometria.Include(e => e.paciente);
            if (id != null)
                espirometria = espirometria.Where(e => e.esp_paciente == id);
            if (paciente != null)
                espirometria = espirometria.Where(e => e.esp_paciente == paciente);
           
            if (User.IsInRole("paciente"))
            {
                string cedula = Convert.ToString(User.Identity.Name);
                paciente paciente_ = db.paciente.Where(p => p.pac_cedula == cedula).First();
                espirometria = espirometria.Where(e => e.esp_paciente == paciente_.pac_id);
            }
            if (User.IsInRole("empresa"))
            {
                string cedula = Convert.ToString(User.Identity.Name);
                empresa empresa = db.empresa.Where(e => e.emp_cedula == cedula).First();
                espirometria = espirometria.Where(e => e.paciente.pac_empresa == empresa.emp_id);
            }

            if (Request.IsAjaxRequest())
                return PartialView("Index_historia", espirometria.ToList());
            return View(espirometria.ToList());
        }

        //
        // GET: /Espirometria/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Details(int id = 0)
        {
            espirometria espirometria = db.espirometria.Find(id);
            if (espirometria == null)
            {
                return HttpNotFound();
            }
            
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
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                string fileName = Path.GetFileName(file.FileName);
                string ext = Path.GetExtension(fileName);
                if (fileName != "") {
                    espirometria.esp_archivo = fileName;
                    DateTime dd = DateTime.Now;
                    espirometria.esp_fecha = dd.Date.ToString("d");
                    espirometria.esp_laboratorista = get_user();
                    espirometria.esp_orden = get_orden(espirometria.esp_fecha);
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
                        UserManager usermanager = new UserManager();
                        espirometria.esp_responsable = usermanager.get_user_id(User);
                        espirometria.esp_perfil = usermanager.get_perfil(User);
                        db.Entry(espirometria).State = EntityState.Modified;
                        db.SaveChanges();
                        if (espirometria.esp_observacion != "")
                            notificar(espirometria.esp_paciente);
                        return RedirectToAction("Index");
                    }
                }


            }
            
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
                string fileName = String.Empty;
                //if (String.IsNullOrEmpty(fileName))
                //    fileName = "firma.png";
                //string path01 = Path.Combine(Server.MapPath("~/Content/firmas"), fileName);

                string strEspirometria = "Select * from view_espirometria where esp_id=" + id;


                SqlConnection sqlcon = new SqlConnection(conn);
                SqlDataAdapter daEspirometria = new SqlDataAdapter(strEspirometria, sqlcon);
                daEspirometria.Fill(dsPrueba, "view_espirometria");

                RptEspirometria_ rp = new RptEspirometria_();
                rp.Load(Path.Combine(Server.MapPath("~/Reports"), "RptEspirometria_.rpt"));
                rp.SetDataSource(dsPrueba);
                rp.SetParameterValue("hc", "");
                rp.SetParameterValue("orden", "");
                //rp.SetParameterValue("picturePath", path01);
                rp.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Path.Combine(Server.MapPath("~/Content/espirometria"), id + ".pdf"));

                //Stream stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                //stream.Seek(0, SeekOrigin.Begin);
                //return File(stream, "application/pdf", esp_id + ".pdf");

                var document = new Document();
                var ms = new MemoryStream();
                string archivo1 = Path.Combine(Server.MapPath("~/Content/espirometria"), id + ".pdf");
                string archivo2 = Path.Combine(Server.MapPath("~/Content/espirometria"), espirometria.esp_archivo);
                fileName = "Reporte" + id + ".pdf";
                string fileTarget = Path.Combine(Server.MapPath("~/Content/espirometria/") + fileName);
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
                resultado = " " + resultado + mensaje.mail(correo, "Los exámenes de espirometria se encuentran listos. Kinnemed");
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
            var consulta = db.espirometria.Where(r => r.esp_fecha == fecha);
            if (consulta.Any())
                num_exa = db.espirometria.Where(r => r.esp_fecha == fecha).OrderByDescending(r => r.esp_orden).First().esp_orden.GetValueOrDefault();
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