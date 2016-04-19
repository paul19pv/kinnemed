using kinnemed05.Security;
using kinnemed05.Filters;
using kinnemed05.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kinnemed05.Reports;
using System.IO;
using kinnemed05.Reports.dataset;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Helpers;
using Microsoft.VisualBasic.FileIO;
using System.Data.Entity.Validation;

namespace kinnemed05.Controllers
{
    [InitializeSimpleMembership]
    [CustomAuthorize(UserRoles.admin, UserRoles.medico)]
    public class PacienteController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();
        //
        // GET: /Paciente/
        //[Authorize(Roles = "medico")]

        public ActionResult Index(string sortOrder, string searchString, string exportar, string empresa)
        {
            ViewBag.NombreSort = String.IsNullOrEmpty(sortOrder) ? "Nombre desc" : "";
            ViewBag.ApellidoSort = sortOrder == "Apellido" ? "Apellido desc" : "Apellido";
            ViewBag.SearchString = searchString;
            ViewBag.empresa = new SelectList(db.empresa, "emp_id", "emp_nombre");
            int cod_empresa = 0;
            var paciente = db.paciente.Include(p => p.canton).Include(p => p.empresa).Include(p => p.pais).Include(p => p.profesion);
            if (!String.IsNullOrEmpty(searchString)) { 
                searchString=searchString.ToUpper();
                paciente = paciente.Where(p => p.pac_nombres.ToUpper().Contains(searchString)||p.pac_apellidos.Contains(searchString)||p.pac_cedula.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(empresa))
                cod_empresa = Int32.Parse(empresa);
            if (cod_empresa != 0)
            {
                paciente = paciente.Where(p=>p.pac_empresa.Equals(cod_empresa));
                //ViewBag.valor = especialidad;
            }
            switch (sortOrder)
            {
                case "Nombre desc":
                    paciente = paciente.OrderByDescending(p => p.pac_nombres);
                    break;
                case "Apellido":
                    paciente = paciente.OrderBy(p=>p.pac_apellidos);
                    break;
                case "Apellido desc":
                    paciente = paciente.OrderByDescending(p => p.pac_apellidos);
                    break;
                default:
                    paciente = paciente.OrderBy(p => p.pac_nombres);
                    break;
            }
            if (!String.IsNullOrEmpty(exportar))
            {
                return RedirectToAction("ExportReport", new { filtro = searchString,empresa=cod_empresa });
            }

            return View(paciente.ToList());
        }

        //
        // GET: /Paciente/Details/5

        public ActionResult Details(int id = 0)
        {
            paciente paciente = db.paciente.Find(id);
            //paciente = paciente.Include(p => p.canton);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            return View(paciente);
        }

        //
        // GET: /Paciente/Create

        public ActionResult Create()
        {
            ViewBag.pac_canton = new SelectList(db.canton, "can_id", "can_nombre");
            ViewBag.pac_empresa = new SelectList(db.empresa, "emp_id", "emp_nombre");
            ViewBag.pac_pais = new SelectList(db.pais, "pais_id", "pais_nombre");
            ViewBag.pac_profesion = new SelectList(db.profesion, "pro_id", "pro_nombre");
            ViewBag.pac_provincia = new SelectList(db.provincia, "pro_id", "pro_nombre");
            ViewBag.pac_genero = this.genero();
            ViewBag.pac_estadocivil = this.estadocivil();
            ViewBag.pac_instruccion = this.instruccion();
            ViewBag.pac_tipodiscapacidad = this.tipodiscapacidad();
            ViewBag.pac_porcentajediscapacidad = this.porcentajediscapacidad();
            ViewBag.nom_profesion = "";
            ViewBag.dia = dia(String.Empty);
            ViewBag.mes = mes(String.Empty);
            ViewBag.anio = anio(String.Empty);

            return View();
        }

        //
        // POST: /Paciente/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(paciente paciente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.paciente.Add(paciente);
                    db.SaveChanges();


                    AccountController account = new AccountController();
                    account.CreateUserProfile(paciente.pac_cedula, paciente.pac_cedula);
                    UserManager userManager = new UserManager();
                    int Userid = userManager.UpdatePaciente(paciente.pac_cedula, paciente.pac_id);
                    UsersInRoles usersinroles = new UsersInRoles();
                    usersinroles.RoleId = 3;
                    usersinroles.UserId = Userid;
                    account.CreateUsersInRole(usersinroles);
                    return RedirectToAction("Index");
                }

                ViewBag.pac_canton = new SelectList(db.canton, "can_id", "can_nombre", paciente.pac_canton);
                ViewBag.pac_empresa = new SelectList(db.empresa, "emp_id", "emp_nombre", paciente.pac_empresa);
                ViewBag.pac_pais = new SelectList(db.pais, "pais_id", "pais_nombre", paciente.pac_pais);
                ViewBag.pac_profesion = new SelectList(db.profesion, "pro_id", "pro_nombre", paciente.pac_profesion);
                ViewBag.pac_provincia = new SelectList(db.provincia, "pro_id", "pro_nombre", paciente.pac_provincia);
                ViewBag.pac_genero = this.genero(paciente);
                ViewBag.pac_estadocivil = this.estadocivil(paciente);
                ViewBag.pac_instruccion = this.instruccion(paciente);
                ViewBag.pac_tipodiscapacidad = this.tipodiscapacidad(paciente);
                ViewBag.pac_porcentajediscapacidad = this.porcentajediscapacidad(paciente);
                //ViewBag.nom_profesion = txt_profesion;
                if (paciente.pac_fechanacimiento != null)
                {
                    string[] fecha = paciente.pac_fechanacimiento.Split('/');
                    ViewBag.dia = dia(fecha[0]);
                    ViewBag.mes = mes(fecha[1]);
                    ViewBag.anio = anio(fecha[2]);
                }
                else
                {
                    ViewBag.dia = dia("");
                    ViewBag.mes = mes("");
                    ViewBag.anio = anio("");
                }
                return View(paciente);
            }

            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

                //return RedirectToAction("Message", "Home", new { mensaje = ex.Message });
            }
        }

        //
        // GET: /Paciente/Edit/5

        public ActionResult Edit(int id = 0)
        {
            paciente paciente = db.paciente.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            ViewBag.pac_canton = new SelectList(db.canton, "can_id", "can_nombre", paciente.pac_canton);
            ViewBag.pac_empresa = new SelectList(db.empresa, "emp_id", "emp_nombre", paciente.pac_empresa);
            ViewBag.pac_pais = new SelectList(db.pais, "pais_id", "pais_nombre", paciente.pac_pais);
            ViewBag.pac_profesion = new SelectList(db.profesion, "pro_id", "pro_nombre", paciente.pac_profesion);
            ViewBag.pac_provincia = new SelectList(db.provincia, "pro_id", "pro_nombre", paciente.pac_provincia);
            ViewBag.pac_genero = this.genero(paciente);
            ViewBag.pac_estadocivil = this.estadocivil(paciente);
            ViewBag.pac_instruccion = this.instruccion(paciente);
            ViewBag.pac_tipodiscapacidad = this.tipodiscapacidad(paciente);
            ViewBag.pac_porcentajediscapacidad = this.porcentajediscapacidad(paciente);
            if (paciente.pac_fechanacimiento != null)
            {
                string[] fecha = paciente.pac_fechanacimiento.Split('/');
                ViewBag.dia = dia(fecha[0]);
                ViewBag.mes = mes(fecha[1]);
                ViewBag.anio = anio(fecha[2]);
            }
            else {
                ViewBag.dia = dia("");
                ViewBag.mes = mes("");
                ViewBag.anio = anio("");
            }
            //ViewBag.dia = dia("");
            //ViewBag.mes = mes("");
            //ViewBag.anio = anio("");
            return View(paciente);
        }

        //
        // POST: /Paciente/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(paciente paciente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paciente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.pac_canton = new SelectList(db.canton, "can_id", "can_nombre", paciente.pac_canton);
            ViewBag.pac_empresa = new SelectList(db.empresa, "emp_id", "emp_nombre", paciente.pac_empresa);
            ViewBag.pac_pais = new SelectList(db.pais, "pais_id", "pais_nombre", paciente.pac_pais);
            ViewBag.pac_profesion = new SelectList(db.profesion, "pro_id", "pro_nombre", paciente.pac_profesion);
            ViewBag.pac_provincia = new SelectList(db.provincia, "pro_id", "pro_nombre", paciente.pac_provincia);
            ViewBag.pac_genero = this.genero(paciente);
            ViewBag.pac_estadocivil = this.estadocivil(paciente);
            ViewBag.pac_instruccion = this.instruccion(paciente);
            ViewBag.pac_tipodiscapacidad = this.tipodiscapacidad(paciente);
            ViewBag.pac_porcentajediscapacidad = this.porcentajediscapacidad(paciente);
            if (paciente.pac_fechanacimiento != null)
            {
                string[] fecha = paciente.pac_fechanacimiento.Split('/');
                ViewBag.dia = dia(fecha[0]);
                ViewBag.mes = mes(fecha[1]);
                ViewBag.anio = anio(fecha[2]);
            }
            else
            {
                ViewBag.dia = dia("");
                ViewBag.mes = mes("");
                ViewBag.anio = anio("");
            }
            return View(paciente);
        }

        //
        // GET: /Paciente/Delete/5

        public ActionResult Delete(int id = 0)
        {
            paciente paciente = db.paciente.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            return View(paciente);
        }

        //
        // POST: /Paciente/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            paciente paciente = db.paciente.Find(id);
            UserManager usermanager = new UserManager();
            usermanager.DeleteUser(id, 3);
            db.paciente.Remove(paciente);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Cargar() {
            ViewBag.pac_empresa = new SelectList(db.empresa, "emp_id", "emp_nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cargar(HttpPostedFileBase FileUpload, int pac_empresa)
        {
            try
            {
                DataTable dt = new DataTable();
                string fileName = Path.GetFileName(FileUpload.FileName);
                string mensaje = String.Empty;
                if (fileName != "")
                {
                    string path = Path.Combine(Server.MapPath("~/Content/img"), fileName);
                    FileUpload.SaveAs(path);
                    dt = Process_CSV(path);
                    //ViewBag.algo = "algo";
                    ViewBag.mensaje = ProcessData(dt, pac_empresa);
                    //ViewBag.mensaje = "algo";
                    return View("Message");
                }
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
                return View("Message");

            }
            return View();
            
        }


        public DataTable Process_CSV(string fileName)
        {
            DataTable csvData = new DataTable();
            try
            {

                using (TextFieldParser csvReader = new TextFieldParser(fileName))
                {
                    csvReader.SetDelimiters(new string[] { ";","," });
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
                        //Making empty value as null
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

        public string ProcessData(DataTable dt, int pac_empresa)
        {
            string[] strArray = new string[] { };
            string mensaje = String.Empty;
            int cont=dt.Rows.Count;
            foreach (DataRow dr in dt.Rows)
            {
                mensaje = mensaje+" " +ProcessArray(dr, pac_empresa);
            }
            return mensaje;
        }

        private string ProcessArray(DataRow strArray, int pac_empresa)
        {
            string mensaje = String.Empty;
            string cedula = strArray[0].ToString();
            //var obj_examen = db.examen.Where(e => e.exa_codigo == codigo);
            try
            {
                paciente paciente = new paciente();
                var consulta = db.paciente.Where(p => p.pac_cedula == cedula);
                if (!consulta.Any())
                {
                    paciente.pac_cedula = strArray[0].ToString();
                    paciente.pac_nombres = strArray[1].ToString();
                    paciente.pac_apellidos = strArray[2].ToString();
                    paciente.pac_edad = Convert.ToInt32(strArray[3]);
                    paciente.pac_estado = true;
                    paciente.pac_empresa = pac_empresa;
                    db.paciente.Add(paciente);
                    db.SaveChanges();
                    AccountController account = new AccountController();
                    account.CreateUserProfile(paciente.pac_cedula, paciente.pac_cedula);
                    UserManager userManager = new UserManager();
                    int Userid = userManager.UpdatePaciente(paciente.pac_cedula, paciente.pac_id);
                    UsersInRoles usersinroles = new UsersInRoles();
                    usersinroles.RoleId = 3;
                    usersinroles.UserId = Userid;
                    account.CreateUsersInRole(usersinroles);

                    mensaje = "Paciente "+ paciente.pac_nombres+" "+paciente.pac_apellidos+" Ingresado";
                }
                else {
                    mensaje = "El paciente " + consulta.First().pac_nombres + " " + consulta.First().pac_apellidos + " ya existe";
                }
                
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return mensaje;
        }

        public SelectList genero()
        {
            List<SelectListItem> list_genero = new List<SelectListItem>();
            list_genero.Add(new SelectListItem { Text = "Masculino", Value = "Masculino" });
            list_genero.Add(new SelectListItem { Text = "Femenino", Value = "Femenino" });
            SelectList generos = new SelectList(list_genero, "Value", "Text");
            return generos;
        }
        public SelectList genero(paciente paciente)
        {
            List<SelectListItem> list_genero = new List<SelectListItem>();
            list_genero.Add(new SelectListItem { Text = "Masculino", Value = "Masculino" });
            list_genero.Add(new SelectListItem { Text = "Femenino", Value = "Femenino" });
            SelectList generos = new SelectList(list_genero, "Value", "Text", paciente.pac_genero);
            return generos;
        }
        public SelectList estadocivil()
        {
            List<SelectListItem> list_estado = new List<SelectListItem>();
            list_estado.Add(new SelectListItem { Text = "Soltero", Value = "Soltero" });
            list_estado.Add(new SelectListItem { Text = "Casado", Value = "Casado" });
            list_estado.Add(new SelectListItem { Text = "Divorciado", Value = "Divorciado" });
            list_estado.Add(new SelectListItem { Text = "Viudo", Value = "Viudo" });
            list_estado.Add(new SelectListItem { Text = "Union de Hecho", Value = "Union de Hecho" });
            SelectList estados = new SelectList(list_estado, "Value", "Text");
            return estados;
        }
        public SelectList estadocivil(paciente paciente)
        {
            List<SelectListItem> list_estado = new List<SelectListItem>();
            list_estado.Add(new SelectListItem { Text = "Soltero", Value = "Soltero" });
            list_estado.Add(new SelectListItem { Text = "Casado", Value = "Casado" });
            list_estado.Add(new SelectListItem { Text = "Divorciado", Value = "Divorciado" });
            list_estado.Add(new SelectListItem { Text = "Viudo", Value = "Viudo" });
            list_estado.Add(new SelectListItem { Text = "Unión de Hecho", Value = "Union de Hecho" });
            SelectList estados = new SelectList(list_estado, "Value", "Text", paciente.pac_estadocivil);
            return estados;
        }
        public SelectList instruccion()
        {
            List<SelectListItem> list_instruccion = new List<SelectListItem>();
            list_instruccion.Add(new SelectListItem { Text = "Primaria", Value = "Primaria" });
            list_instruccion.Add(new SelectListItem { Text = "Secundaria", Value = "Secundaria" });
            list_instruccion.Add(new SelectListItem { Text = "Superior", Value = "Superior" });
            list_instruccion.Add(new SelectListItem { Text = "Cuarto Nivel", Value = "Cuarto Nivel" });
            SelectList instrucciones = new SelectList(list_instruccion, "Value", "Text");
            return instrucciones;
        }
        public SelectList instruccion(paciente paciente)
        {
            List<SelectListItem> list_instruccion = new List<SelectListItem>();
            list_instruccion.Add(new SelectListItem { Text = "Primaria", Value = "Primaria" });
            list_instruccion.Add(new SelectListItem { Text = "Secundaria", Value = "Secundaria" });
            list_instruccion.Add(new SelectListItem { Text = "Superior", Value = "Superior" });
            list_instruccion.Add(new SelectListItem { Text = "Cuarto Nivel", Value = "Cuarto Nivel" });
            SelectList instrucciones = new SelectList(list_instruccion, "Value", "Text", paciente.pac_instruccion);
            return instrucciones;
        }

        public SelectList tipodiscapacidad() {
            List<SelectListItem> list_tipo = new List<SelectListItem>();
            list_tipo.Add(new SelectListItem { Text = "No Aplica", Value = "No Aplica" ,Selected=true});
            list_tipo.Add(new SelectListItem { Text = "Sensorial Auditiva", Value = "Sensorial Auditiva" });
            list_tipo.Add(new SelectListItem { Text = "Sensorial Visual", Value = "Sensorial Visual" });
            list_tipo.Add(new SelectListItem { Text = "Intelectual", Value = "Intelectual" });
            list_tipo.Add(new SelectListItem { Text = "Motora", Value = "Motora" });
            SelectList tipos = new SelectList(list_tipo, "Value", "Text","No Aplica");
            return tipos;
        }
        public SelectList tipodiscapacidad(paciente paciente)
        {
            List<SelectListItem> list_tipo = new List<SelectListItem>();
            list_tipo.Add(new SelectListItem { Text = "No Aplica", Value = "No Aplica" });
            list_tipo.Add(new SelectListItem { Text = "Sensorial Auditiva", Value = "Sensorial Auditiva" });
            list_tipo.Add(new SelectListItem { Text = "Sensorial Visual", Value = "Sensorial Visual" });
            list_tipo.Add(new SelectListItem { Text = "Intelectual", Value = "Intelectual" });
            list_tipo.Add(new SelectListItem { Text = "Motora", Value = "Motora" });
            SelectList tipos = new SelectList(list_tipo, "Value", "Text",paciente.pac_tipodiscapacidad);
            return tipos;
        }
        public SelectList porcentajediscapacidad()
        {
            List<SelectListItem> list_porcentaje = new List<SelectListItem>();
            for(int i=0;i<=100;i+=10){
                list_porcentaje.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            SelectList porcentajes = new SelectList(list_porcentaje, "Value", "Text");
            return porcentajes;
        }
        public SelectList dia(string valor)
        {
            List<SelectListItem> list_dia = new List<SelectListItem>();
            string item = String.Empty;
            for (int i = 1; i <= 31; i ++)
            {
                item = i.ToString();
                if (item.Length == 1)
                    item = "0" + item;
                list_dia.Add(new SelectListItem { Text = item, Value = item });
            }
            SelectList dias;
            if(String.IsNullOrEmpty(valor))
                dias=new SelectList(list_dia, "Value", "Text");
            else
                dias = new SelectList(list_dia, "Value", "Text", valor);
            return dias;
        }
        public SelectList mes(string valor)
        {
            List<SelectListItem> list_mes = new List<SelectListItem>();
            string item = String.Empty;
            for (int i = 1; i <= 12; i++)
            {
                item = i.ToString();
                if (item.Length == 1)
                    item = "0" + item;
                list_mes.Add(new SelectListItem { Text = item, Value = item });
            }
            SelectList meses;
            if (String.IsNullOrEmpty(valor))
                meses = new SelectList(list_mes, "Value", "Text");
            else
                meses = new SelectList(list_mes, "Value", "Text",valor);
            return meses;
        }
        public SelectList anio(string valor)
        {
            List<SelectListItem> list_anio = new List<SelectListItem>();
            DateTime dd = DateTime.Now;
            int anio = dd.Year;
            for (int i = anio-100; i <= anio; i++)
            {
                list_anio.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            SelectList anios;
            if (String.IsNullOrEmpty(valor))
                anios = new SelectList(list_anio, "Value", "Text");
            else
                anios = new SelectList(list_anio, "Value", "Text",valor);
            return anios;
        }

        public SelectList porcentajediscapacidad(paciente paciente)
        {
            List<SelectListItem> list_porcentaje = new List<SelectListItem>();
            for (int i = 0; i <= 100; i += 10)
            {
                list_porcentaje.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            SelectList porcentajes = new SelectList(list_porcentaje, "Value", "Text",paciente.pac_porcentajediscapacidad);
            return porcentajes;
        }


        //[ActionName("CantonesPorProvincia")]
        public JsonResult GetCantonesPorProvincia(int id)
        {
            var lista = from c in db.canton where c.can_provincia == id select c;
            var cantones = new SelectList(lista, "can_id", "can_nombre");
            return new JsonResult() { Data = cantones };
        }
        public JsonResult AutocompleteProfesion(string search)
        {
            search = search.ToUpper();
            var result = (from p in db.profesion
                          where p.pro_nombre.ToUpper().Contains(search)
                          select new { p.pro_id, p.pro_nombre}).Distinct();
            if (result.Count() == 0) { 
                return new JsonResult() { Data = new {Data= new{ pro_id = 0, pro_nombre = "Sin Datos" }} };
            }
            return new JsonResult() { Data = result };
        }

        public ActionResult ExportReport(string filtro, int empresa)
        {

            try
            {
                dsPaciente dsPaciente = new dsPaciente();
                //string conn = "Data Source=(local); Initial Catalog=bd_kinnemed02;user id=Todos;password=kinnemed2015";
                string conn = ConfigurationManager.AppSettings["conexion"];
                string strEmpresa = "Select * from empresa";
                string strProfesion = "Select * from profesion";
                string strProvincia = "Select * from provincia";
                string strCanton = "Select * from canton";
                string strPaciente = "Select * from paciente";
                if (!String.IsNullOrEmpty(filtro))
                {
                    //medico = medico.Where(s => s.med_nombres.ToUpper().Contains(filtro.ToUpper()) || s.med_apellidos.ToUpper().Contains(filtro.ToUpper()));
                    strPaciente += " where pac_nombres like '%" + filtro + "%' or pac_apellidos like '%" + filtro + "%'";
                }
                if (empresa != 0 && !String.IsNullOrEmpty(filtro))
                {
                    //medico = medico.Where(s => s.med_especialidad.Equals(especialidad));
                    strPaciente += " and pac_empresa=" + empresa;
                }
                else if (empresa != 0 && String.IsNullOrEmpty(filtro))
                {
                    strPaciente += " where pac_empresa=" + empresa;
                }
                SqlConnection sqlcon = new SqlConnection(conn);
                SqlDataAdapter daEmpresa = new SqlDataAdapter(strEmpresa, sqlcon);
                SqlDataAdapter daProfesion = new SqlDataAdapter(strProfesion, sqlcon);
                SqlDataAdapter daProvincia = new SqlDataAdapter(strProvincia, sqlcon);
                SqlDataAdapter daCanton = new SqlDataAdapter(strCanton,sqlcon);
                SqlDataAdapter daPaciente = new SqlDataAdapter(strPaciente,sqlcon);
                daEmpresa.Fill(dsPaciente, "empresa");
                daProfesion.Fill(dsPaciente, "profesion");
                daProvincia.Fill(dsPaciente, "provincia");
                daCanton.Fill(dsPaciente, "canton");
                daPaciente.Fill(dsPaciente, "paciente");
                RptPaciente rp = new RptPaciente();
                rp.Load(Path.Combine(Server.MapPath("~/Reports"), "RptPaciente.rpt"));
                rp.SetDataSource(dsPaciente);
                rp.SetParameterValue("titulo", "General de Pacientes");
                if (empresa != 0)
                {
                    rp.SetParameterValue("titulo", "Pacientes por Empresa");
                }
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "ReportePaciente.pdf");
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
                return View("Message");
            }
        }

        protected void reporte() {
            //var medico = db.medico.Include(m=>m.med_especialidad);
            //var paciente = from s in db.paciente select s;
            //RptPaciente rp = new RptPaciente();
            //rp.Load(Path.Combine(Server.MapPath("~/Reports"), "RptPaciente.rpt"));
            //if (!String.IsNullOrEmpty(filtro))
            //{
            //    filtro = filtro.ToUpper();
            //    paciente = paciente.Where(p => p.pac_nombres.ToUpper().Contains(filtro) || p.pac_apellidos.Contains(filtro) || p.pac_cedula.Contains(filtro));
            //}
            //if (empresa != 0)
            //{
            //    paciente = paciente.Where(p => p.pac_empresa.Equals(empresa));
            //    //ViewBag.valor = especialidad;
            //}


            //rp.SetDataSource(paciente.ToList());
            //rp.SetParameterValue("titulo", "Reporte General de Pacientes");
            //if (empresa != 0) {
            //    rp.SetParameterValue("titulo", "Reporte de Pacientes por Empresa");
            //}

            //Response.Buffer = false;
            //Response.ClearContent();
            //Response.ClearHeaders();
            //try
            //{
            //    Stream stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //    stream.Seek(0, SeekOrigin.Begin);
            //    return File(stream, "application/pdf", "ReportePacientes.pdf");
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}