using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kinnemed05.Models;
using kinnemed05.Filters;
using kinnemed05.Security;
using kinnemed05.Reports.dataset;
using System.Configuration;
using System.Data.SqlClient;
using kinnemed05.Reports;
using iTextSharp.text.pdf.parser;
using System.IO;

namespace kinnemed05.Controllers
{
    [InitializeSimpleMembership]
    public class OftalmologiaController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Oftalmologia/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Index(int? id, int? paciente, int? medico)
        {
            var oftalmologia = db.oftalmologia.Include(o => o.paciente).Include(o => o.medico);
            if (id != null)
                oftalmologia = oftalmologia.Where(o => o.oft_paciente == id);
            if (paciente != null)
                oftalmologia = oftalmologia.Where(o => o.oft_paciente == paciente);
            if (medico != null)
                oftalmologia = oftalmologia.Where(o => o.oft_medico == medico);
            if (User.IsInRole("paciente"))
            {
                string cedula = Convert.ToString(User.Identity.Name);
                paciente paciente_ = db.paciente.Where(p => p.pac_cedula == cedula).First();
                oftalmologia = oftalmologia.Where(o => o.oft_paciente == paciente_.pac_id);
            }
            if (User.IsInRole("empresa"))
            {
                string cedula = Convert.ToString(User.Identity.Name);
                empresa empresa = db.empresa.Where(o => o.emp_cedula == cedula).First();
                oftalmologia = oftalmologia.Where(o => o.paciente.pac_empresa == empresa.emp_id);
            }


            if (Request.IsAjaxRequest())
                return PartialView("Index_historia", oftalmologia.ToList());
            return View(oftalmologia.ToList());
        }

        //
        // GET: /Oftalmologia/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Details(int id = 0)
        {
            oftalmologia oftalmologia = db.oftalmologia.Find(id);
            if (oftalmologia == null)
            {
                return HttpNotFound();
            }
            paciente paciente = db.paciente.Find(oftalmologia.oft_paciente);
            ViewBag.paciente = paciente.pac_nombres + " " + paciente.pac_apellidos;
            medico medico = db.medico.Find(oftalmologia.oft_medico);
            ViewBag.medico = medico.med_nombres + " " + medico.med_apellidos;
            return View(oftalmologia);
        }

        //
        // GET: /Oftalmologia/Create
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico)]
        public ActionResult Create()
        {
            

            ViewBag.oft_con_od = get_agudeza("20/20");
            ViewBag.oft_con_oi = get_agudeza("20/20");
            ViewBag.oft_sin_od = get_agudeza("20/20");
            ViewBag.oft_sin_oi = get_agudeza("20/20");

            ViewBag.oft_biomiscroscopia = get_valor();
            ViewBag.oft_fondo = get_valor();
            ViewBag.oft_colores = get_colores();
            ViewBag.oft_diagnostico = get_diagnostico();
            ViewBag.oft_indicaciones = get_indicacion();

            return View();
        }

        //
        // POST: /Oftalmologia/Create
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(oftalmologia oftalmologia)
        {
            string nom_pac;
            string nom_med;
            DateTime dd = DateTime.Now;
            oftalmologia.oft_fecha = dd.Date.ToString("d");
            if (ModelState.IsValid)
            {
                UserManager usermanager = new UserManager();
                oftalmologia.oft_responsable = usermanager.get_user_id(User);
                oftalmologia.oft_perfil = usermanager.get_perfil(User);
                db.oftalmologia.Add(oftalmologia);
                db.SaveChanges();

                
                    notificar(oftalmologia.oft_paciente);
                return RedirectToAction("Index");
            }

            ViewBag.oft_con_od = get_agudeza(oftalmologia.oft_con_od);
            ViewBag.oft_con_oi = get_agudeza(oftalmologia.oft_con_oi);
            ViewBag.oft_sin_od = get_agudeza(oftalmologia.oft_sin_od);
            ViewBag.oft_sin_oi = get_agudeza(oftalmologia.oft_sin_oi);

            ViewBag.oft_biomiscroscopia = get_valor(oftalmologia.oft_biomiscroscopia);
            ViewBag.oft_fondo = get_valor(oftalmologia.oft_fondo);
            ViewBag.oft_colores = get_colores(oftalmologia.oft_colores);
            ViewBag.oft_diagnostico = get_diagnostico(oftalmologia.oft_diagnostico);
            ViewBag.oft_indicaciones = get_indicacion(oftalmologia.oft_indicaciones);
            paciente paciente = db.paciente.Find(oftalmologia.oft_paciente);
            if (paciente != null)
                nom_pac = paciente.pac_nombres + " " + paciente.pac_apellidos;
            else
                nom_pac = "";
            ViewBag.paciente = nom_pac;
            medico medico = db.medico.Find(oftalmologia.oft_medico);
            if (medico != null)
                nom_med = medico.med_nombres + " " + medico.med_apellidos;
            else
                nom_med = "";
            ViewBag.medico = nom_med;
            return View(oftalmologia);
        }

        //
        // GET: /Oftalmologia/Edit/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.admin)]
        public ActionResult Edit(int id = 0)
        {
            oftalmologia oftalmologia = db.oftalmologia.Find(id);
            if (oftalmologia == null)
            {
                return HttpNotFound();
            }
            paciente paciente = db.paciente.Find(oftalmologia.oft_paciente);
            ViewBag.paciente = paciente.pac_nombres + " " + paciente.pac_apellidos;
            medico medico = db.medico.Find(oftalmologia.oft_medico);
            ViewBag.medico = medico.med_nombres + " " + medico.med_apellidos;

            ViewBag.oft_con_od = get_agudeza(oftalmologia.oft_con_od);
            ViewBag.oft_con_oi = get_agudeza(oftalmologia.oft_con_oi);
            ViewBag.oft_sin_od = get_agudeza(oftalmologia.oft_sin_od);
            ViewBag.oft_sin_oi = get_agudeza(oftalmologia.oft_sin_oi);

            ViewBag.oft_biomiscroscopia = get_valor(oftalmologia.oft_biomiscroscopia);
            ViewBag.oft_fondo = get_valor(oftalmologia.oft_fondo);
            ViewBag.oft_colores = get_colores(oftalmologia.oft_colores);
            ViewBag.oft_diagnostico = get_diagnostico(oftalmologia.oft_diagnostico);
            ViewBag.oft_indicaciones = get_indicacion(oftalmologia.oft_indicaciones);
            return View(oftalmologia);
        }

        //
        // POST: /Oftalmologia/Edit/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(oftalmologia oftalmologia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oftalmologia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            paciente paciente = db.paciente.Find(oftalmologia.oft_paciente);
            ViewBag.paciente = paciente.pac_nombres + " " + paciente.pac_apellidos;
            medico medico = db.medico.Find(oftalmologia.oft_medico);
            ViewBag.medico = medico.med_nombres + " " + medico.med_apellidos;

            ViewBag.oft_con_od = get_agudeza(oftalmologia.oft_con_od);
            ViewBag.oft_con_oi = get_agudeza(oftalmologia.oft_con_oi);
            ViewBag.oft_sin_od = get_agudeza(oftalmologia.oft_sin_od);
            ViewBag.oft_sin_oi = get_agudeza(oftalmologia.oft_sin_oi);

            ViewBag.oft_biomiscroscopia = get_valor(oftalmologia.oft_biomiscroscopia);
            ViewBag.oft_fondo = get_valor(oftalmologia.oft_fondo);
            ViewBag.oft_colores = get_colores(oftalmologia.oft_colores);
            ViewBag.oft_diagnostico = get_diagnostico(oftalmologia.oft_diagnostico);
            ViewBag.oft_indicaciones = get_indicacion(oftalmologia.oft_indicaciones);
            return View(oftalmologia);
        }

        //
        // GET: /Oftalmologia/Delete/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        public ActionResult Delete(int id = 0)
        {
            oftalmologia oftalmologia = db.oftalmologia.Find(id);
            if (oftalmologia == null)
            {
                return HttpNotFound();
            }
            paciente paciente = db.paciente.Find(oftalmologia.oft_paciente);
            ViewBag.paciente = paciente.pac_nombres + " " + paciente.pac_apellidos;
            medico medico = db.medico.Find(oftalmologia.oft_medico);
            ViewBag.medico = medico.med_nombres + " " + medico.med_apellidos;
            return View(oftalmologia);
        }

        //
        // POST: /Oftalmologia/Delete/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            oftalmologia oftalmologia = db.oftalmologia.Find(id);
            db.oftalmologia.Remove(oftalmologia);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Descargar(int id)
        {
            try
            {
                //string contentType = "application/pdf";
                dsOftalmologia dsOftalmologia = new dsOftalmologia();
                string conn = ConfigurationManager.AppSettings["conexion"];
                int oft_id = id;
                string fileName = String.Empty;
                //if (String.IsNullOrEmpty(fileName))
                //    fileName = "firma.png";
                //string path01 = Path.Combine(Server.MapPath("~/Content/firmas"), fileName);

                string strOftalmologia = "Select * from view_oftalmologia where oft_id=" + oft_id;

                SqlConnection sqlcon = new SqlConnection(conn);

                SqlDataAdapter daOftalmologia = new SqlDataAdapter(strOftalmologia, sqlcon);

                daOftalmologia.Fill(dsOftalmologia, "view_oftalmologia");
                
                RptOftalmologia_ rp = new RptOftalmologia_();
                string reportPath = Server.MapPath("~/Reports/RptOftalmologia_.rpt");
                rp.Load(reportPath);
                rp.SetDataSource(dsOftalmologia);
                
                Stream stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", oft_id + ".pdf");

            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
                //return View("Message");
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message });
            }
        }



        private SelectList get_agudeza(string agudeza = "")
        {
            List<SelectListItem> list_agudeza = new List<SelectListItem>();
            list_agudeza.Add(new SelectListItem { Text = "20/20", Value = "20/20" });
            list_agudeza.Add(new SelectListItem { Text = "20/25", Value = "20/25" });
            list_agudeza.Add(new SelectListItem { Text = "20/30", Value = "20/30" });
            list_agudeza.Add(new SelectListItem { Text = "20/40", Value = "20/40" });
            list_agudeza.Add(new SelectListItem { Text = "20/50", Value = "20/50" });
            list_agudeza.Add(new SelectListItem { Text = "20/60", Value = "20/60" });
            list_agudeza.Add(new SelectListItem { Text = "20/70", Value = "20/70" });
            list_agudeza.Add(new SelectListItem { Text = "20/80", Value = "20/80" });
            list_agudeza.Add(new SelectListItem { Text = "20/100", Value = "20/100" });
            list_agudeza.Add(new SelectListItem { Text = "20/150", Value = "20/150" });
            list_agudeza.Add(new SelectListItem { Text = "20/200", Value = "20/200" });
            list_agudeza.Add(new SelectListItem { Text = "20/250", Value = "20/250" });
            list_agudeza.Add(new SelectListItem { Text = "20/300", Value = "20/300" });
            list_agudeza.Add(new SelectListItem { Text = "20/350", Value = "20/350" });
            list_agudeza.Add(new SelectListItem { Text = "20/400", Value = "20/400" });


            SelectList agudezas;
            if (agudeza == "")
                agudezas = new SelectList(list_agudeza, "Value", "Text");
            else
                agudezas = new SelectList(list_agudeza, "Value", "Text", agudeza);
            return agudezas;
        }

        private SelectList get_valor(string jornada = "")
        {
            List<SelectListItem> list_jornada = new List<SelectListItem>();
            list_jornada.Add(new SelectListItem { Text = "NORMAL", Value = "NORMAL" });
            list_jornada.Add(new SelectListItem { Text = "ANORMAL", Value = "ANORMAL" });
            SelectList jornadas;
            if (jornada == "")
                jornadas = new SelectList(list_jornada, "Value", "Text","NORMAL");
            else
                jornadas = new SelectList(list_jornada, "Value", "Text", jornada);
            return jornadas;
        }
        private SelectList get_colores(string jornada = "")
        {
            List<SelectListItem> list_jornada = new List<SelectListItem>();
            list_jornada.Add(new SelectListItem { Text = "NORMAL", Value = "NORMAL" });
            list_jornada.Add(new SelectListItem { Text = "DALTONISMO", Value = "DALTONISMO" });
            list_jornada.Add(new SelectListItem { Text = "PRESENTA PROBLEMAS EN COLOR VERDE", Value = "PRESENTA PROBLEMAS EN COLOR VERDE" });
            list_jornada.Add(new SelectListItem { Text = "PRESENTA PROBLEMAS EN COLOR AMARILLO", Value = "PRESENTA PROBLEMAS EN COLOR AMARILLO" });
            list_jornada.Add(new SelectListItem { Text = "PRESENTA PROBLEMAS EN COLOR ROJO", Value = "PRESENTA PROBLEMAS EN COLOR ROJO" });
            SelectList jornadas;
            if (jornada == "")
                jornadas = new SelectList(list_jornada, "Value", "Text","NORMAL");
            else
                jornadas = new SelectList(list_jornada, "Value", "Text", jornada);
            return jornadas;
        }

        private SelectList get_diagnostico(string jornada = "")
        {
            List<SelectListItem> list_jornada = new List<SelectListItem>();
            list_jornada.Add(new SelectListItem { Text = "EMETROPE", Value = "EMETROPE" });
            list_jornada.Add(new SelectListItem { Text = "PRESBICIA", Value = "PRESBICIA" });
            list_jornada.Add(new SelectListItem { Text = "MIOPIA", Value = "MIOPIA" });
            list_jornada.Add(new SelectListItem { Text = "HIPERMETROPIA", Value = "HIPERMETROPIA" });
            list_jornada.Add(new SelectListItem { Text = "ASTIGMATISMO", Value = "ASTIGMATISMO" });
            list_jornada.Add(new SelectListItem { Text = "OTROS", Value = "OTROS" });
            SelectList jornadas;
            if (jornada == "")
                jornadas = new SelectList(list_jornada, "Value", "Text", "EMETROPE");
            else
                jornadas = new SelectList(list_jornada, "Value", "Text", jornada);
            return jornadas;
        }

        private SelectList get_indicacion(string jornada = "")
        {
            List<SelectListItem> list_jornada = new List<SelectListItem>();
            list_jornada.Add(new SelectListItem { Text = "CONTROL ANUAL", Value = "CONTROL ANUAL" });
            list_jornada.Add(new SelectListItem { Text = "USAR LENTES", Value = "USAR LENTES" });
            list_jornada.Add(new SelectListItem { Text = "INTERCONSULTA", Value = "INTERCONSULTA" });
            list_jornada.Add(new SelectListItem { Text = "OTROS", Value = "OTROS" });
            SelectList jornadas;
            if (jornada == "")
                jornadas = new SelectList(list_jornada, "Value", "Text","CONTROL ANUAL");
            else
                jornadas = new SelectList(list_jornada, "Value", "Text", jornada);
            return jornadas;
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

                resultado = mensaje.enviar(celular, "Los exámenes de oftalmologia se encuentran listos. Kinnemed");


            }
            if (!string.IsNullOrEmpty(correo))
            {
                resultado = " " + resultado + mensaje.mail(correo, "Los exámenes de oftalmologia se encuentran listos. Kinnemed");
            }
            ModelState.AddModelError("notificacion", resultado);


        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}