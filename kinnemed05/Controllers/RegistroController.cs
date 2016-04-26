using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kinnemed05.Models;
using kinnemed05.Reports.dataset;
using System.Configuration;
using System.Data.SqlClient;
using kinnemed05.Reports;
using System.IO;
using System.Drawing.Printing;
using CrystalDecisions.Shared;
using System.Security.Principal;
using Microsoft.VisualBasic.FileIO;
using kinnemed05.Filters;
using kinnemed05.Security;

namespace kinnemed05.Controllers
{
    [InitializeSimpleMembership]
    //[CustomAuthorize(UserRoles.laboratorista)]
    public class RegistroController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();
        private UsersContext db_users = new UsersContext();

        //
        // GET: /Registro/
        //[InitializeSimpleMembership]
        //[CustomAuthorize(UserRoles.admin,UserRoles.paciente)]
        //[]
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Index(int? id, int? paciente, string fecha)
        {
            var registro = db.registro.Include(r => r.paciente);
            if (id != null)
                registro = db.registro.Where(r => r.reg_paciente == id);
            if (paciente != null)
                registro = registro.Where(r=>r.reg_paciente==paciente);
            if (!String.IsNullOrEmpty(fecha))
                registro = registro.Where(r=>r.reg_fecha == fecha);
            registro = registro.Where(r => r.reg_estado != false);
            if (Request.IsAjaxRequest())
                return PartialView("Index_historia",registro.ToList());
            return View(registro.ToList());
        }


        //public JsonResult GetLista(int id, string fecha)
        //{
        //    var registro = db.registro.Join(db.area, r => r.reg_area, a => a.are_id, (r, a) => new { id = r.reg_id, nombre = a.are_nombre, paciente = r.reg_paciente, fecha = r.reg_fecha });
        //    registro = registro.Where(r => r.paciente == id && r.fecha == fecha);
        //    var list_registro = new SelectList(registro, "id", "nombre");
        //    return new JsonResult() { Data = list_registro };
        //}

        [CustomAuthorize(UserRoles.laboratorista)]
        public ActionResult Insert() {
            DateTime dd = DateTime.Now;
            string fecha = dd.Date.ToString("d");
            ViewBag.fecha = fecha;
            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Insert(registro registro) {
        //    return RedirectToAction("Lista", registro);
        //}


        //
        // GET: /Registro/Details/5
        
        public ActionResult Details(int id = 0)
        {
            registro registro = db.registro.Find(id);
            if (registro == null)
            {
                return HttpNotFound();
            }
            return View(registro);
        }
        

        //
        // GET: /Registro/Create
        [CustomAuthorize(UserRoles.laboratorista)]
        public ActionResult Create()
        {
            //ViewBag.reg_id = new SelectList(db.paciente, "pac_id", "pac_cedula");
            DateTime dd = DateTime.Today;
            string fecha = dd.Date.ToString("d");
            //ViewBag.reg_area = new SelectList(db.area, "are_id", "are_nombre");
            ViewBag.fecha = fecha;
            
            return View();
        }

        //
        // POST: /Registro/Create
        [CustomAuthorize(UserRoles.laboratorista)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(registro registro)
        {
            var consulta = db.registro.Where(r => r.reg_fecha == registro.reg_fecha && r.reg_paciente == registro.reg_paciente && r.reg_estado==true);
            if (consulta.Any())
                return RedirectToAction("Message", "Home", new { mensaje = "El registro ya existe, por favor seleccione la opción editar" });
            DateTime dd = DateTime.Today;
            string fecha = dd.Date.ToString("d");
            registro.reg_orden = GetOrden(fecha);
            registro.reg_estado = true;
            registro.reg_laboratorista = get_user();
            if (registro.reg_laboratorista == 0)
                return RedirectToAction("Message", "Home", new { mensaje="Su perfil de usuario no permite realizar esta acción "});
            if (ModelState.IsValid)
            {
                db.registro.Add(registro);
                db.SaveChanges();
                return RedirectToAction("Create", "Prueba", new { id = registro.reg_id });
            }

            
            //ViewBag.reg_area = new SelectList(db.area, "are_id", "are_nombre");
            ViewBag.fecha = fecha;
            return View(registro);
        }
        [CustomAuthorize(UserRoles.laboratorista)]
        public ActionResult Perfil() {
            ViewBag.con_perfil = new SelectList(db.perfil, "per_id", "per_nombre");
            return View();
        }
        [CustomAuthorize(UserRoles.laboratorista)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Perfil(int reg_paciente,int con_perfil) {
            try
            {
                

                DateTime dd = DateTime.Today;
                string fecha = dd.Date.ToString("d");
                var consulta = db.registro.Where(r => r.reg_fecha == fecha && r.reg_paciente == reg_paciente && r.reg_estado == true);
                if (consulta.Any())
                    return RedirectToAction("Message", "Home", new { mensaje = "El registro ya existe, por favor seleccione la opción editar" });

                barcode barcode = new barcode();
                List<control> list_control = db.control.Where(c => c.con_perfil == con_perfil).ToList();
                
                registro registro = new registro();




                registro.reg_paciente = reg_paciente;
                registro.reg_fecha = fecha;
                registro.reg_orden = GetOrden(fecha);
                registro.reg_laboratorista = get_user();
                if (registro.reg_laboratorista == 0)
                    return RedirectToAction("Message", "Home", new { mensaje = "Su perfil de usuario no permite realizar esta acción " });
                db.registro.Add(registro);
                db.SaveChanges();
                int reg_id = registro.reg_id;
                foreach (var item in list_control)
                {
                    prueba prueba = new prueba();
                    prueba.pru_examen = item.con_examen;
                    prueba.pru_registro = reg_id;
                    prueba.pru_codigo = GetCodigo(reg_id, item.con_examen);
                    prueba.pru_imagen = barcode.GenerarCodigo(prueba.pru_codigo);
                    db.prueba.Add(prueba);
                    db.SaveChanges();
                }
                return RedirectToAction("Index", "Prueba", new { id = registro.reg_id });
            }
            catch (Exception ex) {
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message });
            }

            //return View();
            
            
        }
        //
        // GET: /Registro/Edit/5
        [CustomAuthorize(UserRoles.laboratorista,UserRoles.admin)]
        public ActionResult Edit(int id = 0)
        {
            registro registro = db.registro.Find(id);
            if (registro == null)
            {
                return HttpNotFound();
            }
            return View(registro);
        }

        //
        // POST: /Registro/Edit/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(registro registro)
        {
            
                return Codigo(registro);
            
        }
        [CustomAuthorize(UserRoles.laboratorista)]
        public ActionResult Cargar()
        {
            return View();
        }
        [CustomAuthorize(UserRoles.laboratorista)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cargar(HttpPostedFileBase FileUpload)
        {
            try
            {
                DataTable dt = new DataTable();
                string mensaje = String.Empty;
                string fileName = Path.GetFileName(FileUpload.FileName);
                if (fileName != "")
                {
                    string path = Path.Combine(Server.MapPath("~/Content/biometria"), fileName);
                    FileUpload.SaveAs(path);
                    dt = Process_CSV(path);
                    mensaje = ProcessData(dt);
                    //return View("IndexCSV", dt);
                    //return View("Message");
                    return RedirectToAction("Message", "Home", new { mensaje=mensaje});
                }
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
                //return View("Message");
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message });

            }

            return View();

        }




        //
        // GET: /Registro/Delete/5
        [CustomAuthorize(UserRoles.laboratorista,UserRoles.admin)]
        public ActionResult Delete(int id = 0)
        {
            registro registro = db.registro.Find(id);
            if (registro == null)
            {
                return HttpNotFound();
            }
            return View(registro);
        }


        //
        // POST: /Registro/Delete/5
        [CustomAuthorize(UserRoles.laboratorista,UserRoles.admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            registro registro = db.registro.Find(id);
            //db.registro.Remove(registro);
            registro.reg_estado = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Codigo() {
            DateTime dd = DateTime.Today;
            string fecha = dd.Date.ToString("d");
            ViewBag.fecha = fecha;
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Codigo(registro registro)
        {
            var consulta = db.registro.Where(r => r.reg_paciente == registro.reg_paciente && r.reg_fecha == registro.reg_fecha && r.reg_estado == true);
            if (!consulta.Any())
                return RedirectToAction("Message", "Home", new { mensaje = "El paciente no tiene exámenes para esta fecha"+registro.reg_fecha+registro.reg_paciente });

            
            string conn = ConfigurationManager.AppSettings["conexion"];
            int reg_id = consulta.First().reg_id;
            string paciente=consulta.First().paciente.pac_nombres+" "+consulta.First().paciente.pac_apellidos;
            string strRegistro = "Select * from view_codigo where reg_id="+ reg_id;

            SqlConnection conexion = new SqlConnection(conn);
            DataTable dt = new DataTable();
            try
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand(strRegistro, conexion);
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                dap.Fill(dt);

                RptCodigo rp = new RptCodigo();
                rp.Load(Path.Combine(Server.MapPath("~/Reports"), "RptCodigo.rpt"));
                rp.SetDataSource(dt);
                rp.SetParameterValue("paciente", paciente);
                //Response.Buffer = false;
                //Response.ClearContent();
                //Response.ClearHeaders();

                Stream stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", reg_id + ".pdf");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message });
            }


        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Codigo(int id)
        {
            //var consulta = db.registro.Where(r => r.reg_paciente == registro.reg_paciente && r.reg_fecha == registro.reg_fecha && r.reg_estado == true);
            //if (!consulta.Any())
            //    return RedirectToAction("Message", "Home", new { mensaje = "El paciente no tiene exámenes para esta fecha" + registro.reg_fecha + registro.reg_paciente });


            string conn = ConfigurationManager.AppSettings["conexion"];
            registro registro = db.registro.Find(id);
            int reg_id = registro.reg_id;
            string paciente = registro.paciente.pac_nombres + " " + registro.paciente.pac_apellidos;
            string strRegistro = "Select * from view_codigo where reg_id=" + reg_id+"";

            SqlConnection conexion = new SqlConnection(conn);
            DataTable dt = new DataTable();
            try
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand(strRegistro, conexion);
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                dap.Fill(dt);

                RptCodigo rp = new RptCodigo();
                rp.Load(Path.Combine(Server.MapPath("~/Reports"), "RptCodigo.rpt"));
                rp.SetDataSource(dt);
                rp.SetParameterValue("paciente", paciente);
                //Response.Buffer = false;
                //Response.ClearContent();
                //Response.ClearHeaders();

                Stream stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", reg_id + ".pdf");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message });
            }


        }

        public ActionResult Reporte()
        {
            DateTime dd = DateTime.Now;
            string fecha = dd.Date.ToString("d");
            ViewBag.fecha = fecha;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reporte(registro registro)
        {
            try
            {
                var consulta = db.registro.Where(r => r.reg_paciente == registro.reg_paciente && r.reg_fecha == registro.reg_fecha && r.reg_estado == true);
                if (!consulta.Any())
                    return RedirectToAction("Message", "Home", new { mensaje = "El paciente no tiene exámenes para esta fecha" });
                Session["reg_paciente"] = registro.reg_paciente;
                Session["reg_fecha"] = registro.reg_fecha;
                ReportViewerViewModel model = new ReportViewerViewModel();
                string content = Url.Content("~/Reports/Viewer/ViewPrueba.aspx");
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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Reporte(int id)
        {
            try
            {
                //var consulta = db.registro.Where(r => r.reg_paciente == registro.reg_paciente && r.reg_fecha == registro.reg_fecha && r.reg_estado == true);
                //if (!consulta.Any())
                //    return RedirectToAction("Message", "Home", new { mensaje = "El paciente no tiene exámenes para esta fecha" });
                registro registro = db.registro.Find(id);
                Session["reg_paciente"] = registro.reg_paciente;
                Session["reg_fecha"] = registro.reg_fecha;
                ReportViewerViewModel model = new ReportViewerViewModel();
                string content = Url.Content("~/Reports/Viewer/ViewPrueba.aspx");
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
        public ActionResult ReporteLimpio()
        {
            DateTime dd = DateTime.Now;
            string fecha = dd.Date.ToString("d");
            ViewBag.fecha = fecha;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReporteLimpio(registro registro)
        {
            try
            {
                var consulta = db.registro.Where(r => r.reg_paciente == registro.reg_paciente && r.reg_fecha == registro.reg_fecha && r.reg_estado == true);
                if (!consulta.Any())
                    return RedirectToAction("Message", "Home", new { mensaje = "El paciente no tiene exámenes para esta fecha" });
                Session["reg_paciente"] = registro.reg_paciente;
                Session["reg_fecha"] = registro.reg_fecha;
                ReportViewerViewModel model = new ReportViewerViewModel();
                string content = Url.Content("~/Reports/Viewer/ViewLimpio.aspx");
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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ReporteLimpio(int id)
        {
            try
            {
                //var consulta = db.registro.Where(r => r.reg_paciente == registro.reg_paciente && r.reg_fecha == registro.reg_fecha && r.reg_estado == true);
                //if (!consulta.Any())
                //    return RedirectToAction("Message", "Home", new { mensaje = "El paciente no tiene exámenes para esta fecha" });
                registro registro = db.registro.Find(id);
                Session["reg_paciente"] = registro.reg_paciente;
                Session["reg_fecha"] = registro.reg_fecha;
                ReportViewerViewModel model = new ReportViewerViewModel();
                string content = Url.Content("~/Reports/Viewer/ViewLimpio.aspx");
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

       
        //public ActionResult Message(string mensaje) {
        //    return View(mensaje);
        //}

        public DataTable Process_CSV(string fileName)
        {
            DataTable csvData = new DataTable();
            try
            {

                using (TextFieldParser csvReader = new TextFieldParser(fileName))
                {
                    csvReader.SetDelimiters(new string[] { "\t" });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }

                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return csvData;
        }

        public string ProcessData(DataTable dt)
        {
            string[] strArray = new string[] { };
            string mensaje = String.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                mensaje =mensaje+" "+ ProcessArray(dr);
            }
            return mensaje;
        }

        private string ProcessArray(DataRow strArray)
        {
            string mensaje = String.Empty;
            string codigo = strArray[0].ToString();
            int reg_id;
            
            try
            {
                //biometria biometria = new biometria();
                var prueba = db.prueba.Where(p => p.pru_codigo == codigo);
                if (prueba != null)
                {
                    reg_id = prueba.First().pru_registro;
                    List<examen> list_examen = GetExamen();
                    List<prueba> list_prueba = GetPrueba(reg_id);
                    foreach (var item in list_prueba)
                    {
                        var objeto = db.prueba.Find(item.pru_id);
                        if (objeto != null)
                        {
                            objeto.pru_resultado = strArray[Convert.ToInt32(item.examen.exa_item)].ToString();
                            db.SaveChanges();
                            mensaje = "Resultado Guardado";
                        }

                    }
                }
                
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return mensaje;
        }

        private List<examen> GetExamen()
        {
            List<examen> list_examen = new List<examen>();
            var examen = db.examen.Where(e => e.exa_area == 11).ToList();
            list_examen = examen;
            return list_examen;
        }
        private List<prueba> GetPrueba(int reg_id)
        {
            List<prueba> list_prueba = new List<prueba>();
            var prueba = db.prueba.Include(p => p.examen).Where(p => p.pru_registro == reg_id && p.examen.exa_area == 11).ToList();
            list_prueba = prueba;
            return list_prueba;
        }

        public JsonResult AutocompletePaciente(string search)
        {
            search = search.ToUpper();
            var result = (from p in db.paciente
                          where p.pac_cedula.ToUpper().Contains(search) ||
                              p.pac_nombres.ToUpper().Contains(search) ||
                              p.pac_apellidos.ToUpper().Contains(search)
                          select new { p.pac_id, p.pac_nombres, p.pac_apellidos }).Distinct();
            if (result.Count() == 0)
            {
                return new JsonResult() { Data = new { Data = new { pac_id = 0, pac_nombres = "Sin Datos", pac_apellidos="" } } };
            }
            return new JsonResult() { Data = result };
        }

        public JsonResult AutocompleteMedico(string search)
        {
            search = search.ToUpper();
            var result = (from m in db.medico
                          where m.med_cedula.ToUpper().Contains(search) ||
                              m.med_nombres.ToUpper().Contains(search) ||
                              m.med_apellidos.ToUpper().Contains(search)
                          select new { m.med_id, m.med_nombres, m.med_apellidos }).Distinct();
            if (result.Count() == 0)
            {
                return new JsonResult() { Data = new { Data = new { med_id = 0, med_nombres = "Sin Datos", med_apellidos="" } } };
            }
            return new JsonResult() { Data = result };
        }

        private int GetOrden(string fecha)
        {
            string orden = String.Empty;
            int num = 0;
            int num_exa = 0;
            var consulta = db.registro.Where(r => r.reg_fecha == fecha);
            if (consulta.Any())
                num_exa = db.registro.Where(r => r.reg_fecha == fecha).OrderByDescending(r => r.reg_orden).First().reg_orden;
            else
                num_exa = 0;
            num = num_exa + 1;
            return num;
        }

        public string GetCodigo(int reg_id, int exa_id)
        {
            string codigo = String.Empty;
            string dia = String.Empty;
            string mes = String.Empty;
            string num = String.Empty;
            DateTime dd = DateTime.Today;
            string date_now = dd.Date.ToString("d");


            int orden = db.registro.Where(r => r.reg_id == reg_id).First().reg_orden;
            int area = db.examen.Where(e => e.exa_id == exa_id).First().exa_area;
            //string prefijo = (from g in db.grupo where g.gru_id == grupo select g).First().gru_prefijo;
            num = orden.ToString();
            dia = dd.Day.ToString();
            if (dia.Length == 1)
                dia = "0" + dia;
            mes = dd.Month.ToString();
            if (mes.Length == 1)
                mes = "0" + mes;
            string fecha = dia + mes;
            switch (num.Length)
            {
                case 1:
                    num = "000" + num;
                    break;
                case 2:
                    num = "00" + num;
                    break;
                case 3:
                    num = "0" + num;
                    break;
            }
            codigo += area.ToString() + fecha + num;
            //return codigo;
            return codigo;
        }

        //public JsonResult GetAreas(int id, string fecha) {
        //    var registro = db.registro.Join(db.area, r => r.reg_area, a => a.are_id, (r, a) => new { id = r.reg_area, nombre = a.are_nombre, paciente = r.reg_paciente, fecha = r.reg_fecha });
        //    registro = registro.Where(r => r.paciente == id && r.fecha == fecha);
        //    var areas = new SelectList(registro, "id", "nombre");
        //    return new JsonResult() { Data = areas };

        //}
        
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