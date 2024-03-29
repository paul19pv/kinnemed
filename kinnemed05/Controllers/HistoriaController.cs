﻿using System;
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
using System.IO;
using CrystalDecisions.Shared;

using PagedList;

namespace kinnemed05.Controllers
{
    [InitializeSimpleMembership]
    //[CustomAuthorize(UserRoles.medico)]
    public class HistoriaController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();
        private UsersContext db_users = new UsersContext();

        //
        // GET: /Historia/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.empresa, UserRoles.admin, UserRoles.trabajador,UserRoles.doctor)]
        public ActionResult Index(int tipo, int? paciente,string empresa,int? current_emp_id, string fecha, string sortOrder, int? page)
        {
            //variables auxiliares
            int emp_id =0;
            //parametro de ordenacion
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FechaSort=String.IsNullOrEmpty(sortOrder) ? "Fecha" : ""; 
            //condicion para la paginacion
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            } 
            //ViewBag.FechaSort = sortOrder == "Fecha" ? "Fecha desc" : "Fecha"; 
            var historia = db.historia.Include(h => h.paciente).Where(h => h.his_tipo == tipo);
            //filtros de busqueda
            if (paciente != null) {
                historia = historia.Where(h => h.his_paciente == paciente);
                fecha = null;
                empresa = null;
                emp_id = 0;
            }
            if (current_emp_id != null) {
               
                historia = historia.Where(p => p.paciente.pac_empresa==current_emp_id);
            }

            if (!String.IsNullOrEmpty(empresa))
            {
                emp_id = Int32.Parse(empresa);
                current_emp_id = Int32.Parse(empresa);
                if (emp_id != 0)
                    historia = historia.Where(p => p.paciente.pac_empresa.Equals(emp_id));
                fecha = null;
                paciente = null;
            }
            if (!String.IsNullOrEmpty(fecha))
            {
                historia = historia.Where(h => h.his_fecha == fecha);
                paciente = null;
                empresa = null;
                emp_id = 0;
            }

                
            //filtros por usuario
            if (User.IsInRole("trabajador"))
            {
                string cedula = Convert.ToString(User.Identity.Name);
                trabajador trabajador = db.trabajador.Where(t => t.tra_cedula == cedula).First();
                historia = historia.Where(a => a.paciente.pac_empresa == trabajador.tra_empresa);
            }
            if (User.IsInRole("doctor")) {
                string cedula = Convert.ToString(User.Identity.Name);
                doctor doctor = db.doctor.Where(d => d.doc_cedula == cedula).First();
                historia = historia.Where(h => h.paciente.pac_empresa == doctor.doc_empresa);
            }
            if (User.IsInRole("empresa"))
            {
                string cedula = Convert.ToString(User.Identity.Name);
                empresa empresa_ = db.empresa.Where(e => e.emp_cedula == cedula).First();
                historia = historia.Where(a => a.paciente.pac_empresa == empresa_.emp_id);
            }
            //metodo de ordenacion
            switch (sortOrder)
            {
                case "Fecha":
                    historia = historia.OrderBy(s => s.his_id);
                    break;
                case "Fecha desc":
                    historia = historia.OrderByDescending(s => s.his_id);
                    break;
                default:
                    historia = historia.OrderByDescending(s => s.his_id);
                    break;
            } 

            ViewBag.paciente = paciente;
            ViewBag.fecha = fecha;
            ViewBag.tipo = tipo;
            ViewBag.titulo = titulo(tipo);
            ViewBag.empresa = new SelectList(db.empresa, "emp_id", "emp_nombre",emp_id);
            ViewBag.current_emp_id = current_emp_id;
            int pageSize = 10;
            int pageIndex = (page ?? 1);
            return View(historia.ToPagedList(pageIndex, pageSize)); 
            //return View(historia.ToList());
        }

        //
        // GET: /Historia/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.empresa, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Details(int id = 0)
        {
            historia historia = db.historia.Find(id);
            if (historia == null)
            {
                return HttpNotFound();
            }
            return View(historia);
        }

        //
        // GET: /Historia/Create
        [CustomAuthorize(UserRoles.medico,UserRoles.doctor)]
        public ActionResult Create(int tipo)
        {
            ViewBag.tipo = tipo;
            ViewBag.his_tipo = his_tipo(tipo);
            if (tipo != 1)
            {
                ViewBag.his_motivo = motivo(tipo);
                return View("Create01");
            }

            return View();
        }

        //
        // POST: /Historia/Create
        [CustomAuthorize(UserRoles.medico,UserRoles.doctor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(historia historia)
        {
            DateTime dd = DateTime.Now;
            historia.his_fecha = dd.Date.ToString("d");
            historia.his_numero = numero_historia(historia);
            historia.his_tipo = 1;
            historia.his_medico = get_user();
            if (historia.his_medico == 0)
                return RedirectToAction("Message", "Home", new { mensaje = "Su perfil de usuario no permite realizar esta acción" });
            if (ModelState.IsValid)
            {
                db.historia.Add(historia);
                db.SaveChanges();
                Session["his_id"] = historia.his_id;
                Session["his_tipo"] = historia.his_tipo;
                return RedirectToAction("Create", "Personal", new { id = historia.his_paciente });
            }
            ViewBag.numero = numero_historia(historia);
            return PartialView(historia);
        }

        //Historias preocupacionales, ocupaciones y retiro
        //public ActionResult Create01()
        //{
        //    ViewBag.his_tipo = his_tipo();
        //    return View();
        //}
        [CustomAuthorize(UserRoles.medico,UserRoles.doctor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create01(historia historia)
        {
            DateTime dd = DateTime.Now;
            historia.his_fecha = dd.Date.ToString("d");
            historia.his_numero = numero_historia(historia);
            historia.his_medico = get_user();
            if (historia.his_medico == 0)
                return RedirectToAction("Message", "Home", new { mensaje = "Su perfil de usuario no permite realizar esta acción" });
            if (ModelState.IsValid)
            {
                db.historia.Add(historia);
                db.SaveChanges();
                if (historia.his_tipo == 2)
                    change_tipo(historia.his_paciente);
                Session["his_id"] = historia.his_id;
                Session["his_tipo"] = historia.his_tipo;
                return RedirectToAction("Create", "Ocupacional", new { id = historia.his_paciente });
            }
            ViewBag.his_tipo = his_tipo(historia.his_tipo);
            return PartialView(historia);
        }


        //
        // GET: /Historia/Edit/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin,UserRoles.doctor)]
        public ActionResult Edit(int id = 0)
        {
            int user_current = get_user();
            historia historia = db.historia.Find(id);
            if (historia.his_medico == user_current && User.IsInRole("medico"))
            {
                ViewBag.tipo = historia.his_tipo;
                if (historia == null)
                {
                    return HttpNotFound();
                }
                //if (historia.his_tipo != 1)
                //    return RedirectToAction("Historico", "Ocupacional", new { id = historia.his_paciente });
                if (Request.IsAjaxRequest())
                {
                    return PartialView(historia);
                }

                return View("Edit01", historia);
            }
            return RedirectToAction("Message", "Home", new { mensaje = "Usted no tiene permiso para editar esta historia" });
            
        }



        //
        // POST: /Historia/Edit/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin,UserRoles.doctor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(historia historia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(historia).State = EntityState.Modified;
                db.SaveChanges();
                Session["his_id"] = historia.his_id;
                Session["his_tipo"] = historia.his_tipo;
                return RedirectToAction("Edit", "Personal", new { id = historia.his_paciente });
            }

            return PartialView(historia);
        }

        [CustomAuthorize(UserRoles.medico, UserRoles.admin,UserRoles.doctor)]
        public ActionResult Edit02(int id = 0)
        {
            int user_current = get_user();
            historia historia = db.historia.Find(id);
            if (historia.his_medico == user_current && User.IsInRole("medico"))
            {
                ViewBag.his_tipo = his_tipo(historia.his_tipo);
                ViewBag.tipo = historia.his_tipo;
                if (historia == null)
                {
                    return HttpNotFound();
                }
                if (Request.IsAjaxRequest())
                {
                    return PartialView(historia);
                }
                return View("Edit03", historia);
            }
            return RedirectToAction("Message", "Home", new { mensaje = "Usted no tiene permiso para editar esta historia" });
            
        }



        //
        // POST: /Historia/Edit/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin,UserRoles.doctor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit02(historia historia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(historia).State = EntityState.Modified;
                db.SaveChanges();
                Session["his_id"] = historia.his_id;
                Session["his_tipo"] = historia.his_tipo;
                return RedirectToAction("Edit", "Ocupacional", new { id = historia.his_paciente });
            }
            ViewBag.his_tipo = his_tipo(historia.his_tipo);
            ViewBag.tipo = historia.his_tipo;
            return PartialView(historia);
        }
        [CustomAuthorize(UserRoles.medico,UserRoles.doctor)]
        public ActionResult Problema(int id)
        {
            historia historia = db.historia.Find(id);
            if (historia == null)
            {
                return HttpNotFound();
            }
            int tipo = Convert.ToInt32(Session["his_tipo"]);
            //if (tipo != 1)
            //    return RedirectToAction("Edit", "Familiar", new { id = historia.his_paciente });
            return PartialView(historia);
        }

        [CustomAuthorize(UserRoles.medico,UserRoles.doctor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Problema(historia historia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(historia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Revision", new { id = historia.his_id });
            }
            return PartialView(historia);
        }


        [CustomAuthorize(UserRoles.medico,UserRoles.doctor)]
        public ActionResult Firma(int id)
        {
            historia historia = db.historia.Find(id);
            if (historia == null)
            {
                return HttpNotFound();
            }
            return PartialView(historia);
        }

        [CustomAuthorize(UserRoles.medico,UserRoles.doctor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Firma(historia historia)
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
                    //paciente.pac_firma = fileName;
                    string path = Path.Combine(Server.MapPath("~/Content/firmas_"), fileName);
                    string path01 = Path.Combine(Server.MapPath("~/Content/firmas"), fileName);
                    file.SaveAs(path);
                    objfirma.ResizeImage(path, path01, 200, 120);
                    historia.his_firma = ConvertBytes(path01);
                    return RedirectToAction("Message", "Home", new { mensaje = "Proceso Finalizado" });
                }
                else
                {
                    if (!String.IsNullOrEmpty(ext))
                        if (Array.IndexOf(formatos, ext) <= 0)
                            ModelState.AddModelError("ext", "Extensión no Válida");
                    //else if (String.IsNullOrEmpty(fileName))
                    //    ModelState.AddModelError("ext", "Debe Seleccionar un archivo");
                    return RedirectToAction("Message", "Home", new { mensaje = "Proceso Finalizado" });
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(historia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Message", "Home", new { mensaje = "Proceso Finalizado" });
            }
            return RedirectToAction("Message", "Home", new { mensaje = "Proceso Finalizado" });
        }
        //
        // GET: /Historia/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin,UserRoles.doctor)]
        public ActionResult Delete(int id = 0)
        {
            
            historia historia = db.historia.Find(id);
            if (historia == null)
            {
                return HttpNotFound();
            }
            ViewBag.tipo = historia.his_tipo;
            return View(historia);
        }

        //
        // POST: /Historia/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin,UserRoles.doctor)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            historia historia = db.historia.Find(id);
            db.historia.Remove(historia);
            db.SaveChanges();
            return RedirectToAction("Index", new { tipo = historia.his_tipo });
        }


        public ActionResult Certificado(int id, int pac_id)
        {

            try
            {
                dsCertificado dsCertificado = new dsCertificado();
                paciente paciente = db.paciente.Find(pac_id);
                historia historia = db.historia.Find(id);
                concepto concepto = db.concepto.Find(id);
                medico medico = db.medico.Find(historia.his_medico);

                if (concepto == null)
                    return RedirectToAction("Message", "Home", new { mensaje = "La historia no tiene información completa. Por llene toda la información y genere el certificado correspondiente" });
                var consulta = db.ocupacional.Where(o => o.ocu_paciente == pac_id && o.ocu_tipo == "actual" && o.ocu_estado == true);
                ocupacional ocupacional = new ocupacional();

                //string fileName = medico.med_firma;
                //if (String.IsNullOrEmpty(fileName))
                //    fileName = "firma.png";

                if (consulta.Any())
                    ocupacional = consulta.First();
                string conn = ConfigurationManager.AppSettings["conexion"];

                string strHistoria = "Select * from historia where his_id=" + id;
                string strPaciente = "Select * from paciente where pac_id=" + pac_id;
                string strConcepto = "Select * from concepto where con_id=" + id;
                string strEmpresa = "Select * from empresa where emp_id=" + paciente.pac_empresa;
                string strMedico = "Select * from medico where med_id=" + historia.his_medico;
                string strOcupacional = String.Empty;
                if (ocupacional.ocu_id != null)
                    strOcupacional = "Select * from ocupacional where ocu_id=" + ocupacional.ocu_id;
                else
                    strOcupacional = "Select top 1 * from ocupacional";

                SqlConnection sqlcon = new SqlConnection(conn);
                SqlDataAdapter daHistoria = new SqlDataAdapter(strHistoria, sqlcon);
                SqlDataAdapter daPaciente = new SqlDataAdapter(strPaciente, sqlcon);
                SqlDataAdapter daConcepto = new SqlDataAdapter(strConcepto, sqlcon);
                SqlDataAdapter daEmpresa = new SqlDataAdapter(strEmpresa, sqlcon);
                SqlDataAdapter daMedico = new SqlDataAdapter(strMedico, sqlcon);
                SqlDataAdapter daOcupacional = new SqlDataAdapter(strOcupacional, sqlcon);
                daHistoria.Fill(dsCertificado, "historia");
                daPaciente.Fill(dsCertificado, "paciente");
                daConcepto.Fill(dsCertificado, "concepto");
                daEmpresa.Fill(dsCertificado, "empresa");
                daMedico.Fill(dsCertificado, "medico");
                daOcupacional.Fill(dsCertificado, "ocupacional");
                Stream stream = MemoryStream.Null;
                if (concepto.con_resultado == "APTO")
                {
                    RptCerApto rp = new RptCerApto();
                    rp.Load(Path.Combine(Server.MapPath("~/Reports"), "RptCerApto.rpt"));
                    rp.SetDataSource(dsCertificado);
                    rp.SetParameterValue("fecha", get_fecha(historia.his_fecha));
                    stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    rp.Close();
                    rp.Dispose();
                }
                else if (concepto.con_resultado == "APTO CON RESTRICCIONES PERSONALES" || concepto.con_resultado == "APTO CON RESTRICCIONES LABORALES")
                {
                    RptCerAptoRes rp = new RptCerAptoRes();
                    rp.Load(Path.Combine(Server.MapPath("~/Reports"), "RptCerAptoRes.rpt"));
                    rp.SetDataSource(dsCertificado);
                    rp.SetParameterValue("fecha", get_fecha(historia.his_fecha));
                    stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    rp.Close();
                    rp.Dispose();
                }
                else if (concepto.con_resultado == "NO APTO")
                {
                    RptCerNoApto rp = new RptCerNoApto();
                    rp.Load(Path.Combine(Server.MapPath("~/Reports"), "RptCerNoApto.rpt"));
                    rp.SetDataSource(dsCertificado);
                    rp.SetParameterValue("fecha", get_fecha(historia.his_fecha));
                    stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    rp.Close();
                    rp.Dispose();
                }
                else if (concepto.con_resultado == "SATISFACTORIA" || concepto.con_resultado == "NO SATISFACTORIA")
                {
                    RptCerRetiro rp = new RptCerRetiro();
                    rp.Load(Path.Combine(Server.MapPath("~/Reports"), "RptCerRetiro.rpt"));
                    rp.SetDataSource(dsCertificado);
                    string nexo = String.Empty;
                    if (concepto.con_valor == "NO")
                        nexo = "NINGUNA";
                    else
                        nexo = "UNA";
                    rp.SetParameterValue("nexo", nexo);
                    rp.SetParameterValue("fecha", get_fecha(historia.his_fecha));
                    stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    rp.Close();
                    rp.Dispose();
                }
                
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Certificado.pdf");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message });
            }

        }


        public ActionResult Reporte(int id)
        {
            //string pathRpt = Path.Combine(Server.MapPath("~/Reports"), "RptHistoria.rpt");
            try
            {
                dsHistoria dshistoria = new dsHistoria();
                string conn = ConfigurationManager.AppSettings["conexion"];
                SqlConnection sqlcon = new SqlConnection(conn);
                historia historia = db.historia.Find(id);
                string strHistoria = "Select * from view_historia where his_id=" + id;
                SqlDataAdapter daHistoria = new SqlDataAdapter(strHistoria, sqlcon);
                daHistoria.Fill(dshistoria, "view_historia");
                RptHistoria rp = new RptHistoria();
                string reportPath = Server.MapPath("~/Reports/RptHistoria.rpt");
                rp.Load(reportPath);
                rp.SetDataSource(dshistoria);

                //Subreportes
                //TRABAJOS ANTERIORES
                dsHistorico dshistorico = new dsHistorico();
                string strHistorico = "Select * from ocupacional where ocu_tipo='histórico' and ocu_paciente=" + historia.his_paciente;
                SqlDataAdapter daHistorico = new SqlDataAdapter(strHistorico, sqlcon);
                daHistorico.Fill(dshistorico, "ocupacional");
                //INFORMACIÓN OCUPACIONAL
                dsHistorico dsocupacional = new dsHistorico();
                string strOcupacional = "Select top 1 * from ocupacional where ocu_tipo='actual' and ocu_paciente=" + historia.his_paciente;
                SqlDataAdapter daOcupacional = new SqlDataAdapter(strOcupacional, sqlcon);
                daOcupacional.Fill(dsocupacional, "ocupacional");
                //riesgos laborales
                dsRiesgos dsriesgos = new dsRiesgos();
                string strRiesgos = "Select * from view_riesgo where ocu_paciente=" + historia.his_paciente;
                SqlDataAdapter daRiesgos = new SqlDataAdapter(strRiesgos, sqlcon);
                daRiesgos.Fill(dsriesgos, "view_riesgo");
                ////diagnostico
                dsDiagnostico dsdiagnostico = new dsDiagnostico();
                string strDiagnostico = "Select * from view_diagnostico where dia_historia=" + historia.his_id;
                SqlDataAdapter daDiagnostico = new SqlDataAdapter(strDiagnostico, sqlcon);
                daDiagnostico.Fill(dsdiagnostico, "view_diagnostico");
                ////inmunizacion
                dsInmunizacion dsinmunizacion = new dsInmunizacion();
                string strInmunizacion = "Select * from view_inmunizacion where inm_paciente=" + historia.his_paciente;
                SqlDataAdapter daInmunizacion = new SqlDataAdapter(strInmunizacion, sqlcon);
                daInmunizacion.Fill(dsinmunizacion, "view_inmunizacion");
                //concepto
                dsConcepto dsconcepto = new dsConcepto();
                string strConcepto = "Select * from concepto where con_id="+historia.his_id;
                SqlDataAdapter daConcepto = new SqlDataAdapter(strConcepto, sqlcon);
                daConcepto.Fill(dsconcepto,"concepto");

                rp.Subreports["RptHistorico.rpt"].SetDataSource(dshistorico);
                rp.Subreports["RptOcupacional.rpt"].SetDataSource(dsocupacional);
                rp.Subreports["RptRiesgos.rpt"].SetDataSource(dsriesgos);
                rp.Subreports["RptDiagnostico.rpt"].SetDataSource(dsdiagnostico);
                rp.Subreports["RptInmunizacion.rpt"].SetDataSource(dsinmunizacion);
                rp.Subreports["RptConcepto.rpt"].SetDataSource(dsconcepto);
                Stream stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Reporte.pdf");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message + ex.InnerException });
            }

        }

        public ActionResult RepHisGen(int id)
        {
            //string pathRpt = Path.Combine(Server.MapPath("~/Reports"), "RptHistoria.rpt");
            try
            {
                dsHistoria dshistoria = new dsHistoria();
                string conn = ConfigurationManager.AppSettings["conexion"];
                SqlConnection sqlcon = new SqlConnection(conn);
                historia historia = db.historia.Find(id);
                string strHistoria = "Select * from view_historia where his_id=" + id;
                SqlDataAdapter daHistoria = new SqlDataAdapter(strHistoria, sqlcon);
                daHistoria.Fill(dshistoria, "view_historia");
                RptHisGen rp = new RptHisGen();
                string reportPath = Server.MapPath("~/Reports/RptHisGen.rpt");
                rp.Load(reportPath);
                rp.SetDataSource(dshistoria);

                //Subreportes
                ////diagnostico
                dsDiagnostico dsdiagnostico = new dsDiagnostico();
                string strDiagnostico = "Select * from view_diagnostico where dia_historia=" + historia.his_id;
                SqlDataAdapter daDiagnostico = new SqlDataAdapter(strDiagnostico, sqlcon);
                daDiagnostico.Fill(dsdiagnostico, "view_diagnostico");
                ////inmunizacion
                dsInmunizacion dsinmunizacion = new dsInmunizacion();
                string strInmunizacion = "Select * from view_inmunizacion where inm_paciente=" + historia.his_paciente;
                SqlDataAdapter daInmunizacion = new SqlDataAdapter(strInmunizacion, sqlcon);
                daInmunizacion.Fill(dsinmunizacion, "view_inmunizacion");
                

                
                rp.Subreports["RptDiagnostico.rpt"].SetDataSource(dsdiagnostico);
                rp.Subreports["RptInmunizacion.rpt"].SetDataSource(dsinmunizacion);
                //rp.Subreports["RptConcepto.rpt"].SetDataSource(dsconcepto);
                Stream stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Reporte.pdf");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message + ex.InnerException });
            }

        }

        public ActionResult Reposo(int id) {
            try
            {
                reposo reposo = db.reposo.Find(id);
                if(reposo==null)
                    return RedirectToAction("Message", "Home", new { mensaje = "La historia no tiene certificado de reposo"});
                dsReposo dsReposo = new dsReposo();
                string conn = ConfigurationManager.AppSettings["conexion"];
                SqlConnection sqlcon = new SqlConnection(conn);
                historia historia = db.historia.Find(id);
                string strReposo = "Select top 1 * from view_reposo where his_id=" + id;
                SqlDataAdapter daReposo = new SqlDataAdapter(strReposo, sqlcon);
                daReposo.Fill(dsReposo, "view_reposo");
                RptReposo rp = new RptReposo();
                string reportPath = Server.MapPath("~/Reports/RptReposo.rpt");
                rp.Load(reportPath);
                rp.SetDataSource(dsReposo);
                rp.SetParameterValue("fecha", get_fecha(historia.his_fecha));
                
                Stream stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Certificado.pdf");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message + ex.InnerException });
            }
        }



        public ActionResult Reporte01(int id)
        {
            try
            {
                //var consulta = db.registro.Where(r => r.reg_paciente == registro.reg_paciente && r.reg_fecha == registro.reg_fecha && r.reg_estado == true);
                //if (!consulta.Any())
                //    return RedirectToAction("Message", "Home", new { mensaje = "El paciente no tiene exámenes para esta fecha" });

                Session["his_id"] = id;
                ReportViewerViewModel model = new ReportViewerViewModel();
                string content = Url.Content("~/Reports/Viewer/ViewHistoria.aspx");
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

        public JsonResult AutocompletePaciente(string search)
        {
            search = search.ToUpper();
            var result = (from p in db.paciente select new { p.pac_id, p.pac_nombres, p.pac_apellidos, p.pac_edad }).Distinct();
            //var result = (from p in db.paciente
            //              where p.pac_cedula.ToUpper().Contains(search) ||
            //                  p.pac_nombres.ToUpper().Contains(search) ||
            //                  p.pac_apellidos.ToUpper().Contains(search) 
            //              select new { p.pac_id, p.pac_nombres, p.pac_apellidos, p.pac_canton }).Distinct();
            if (User.IsInRole("doctor"))
            {
                var doctor = db.doctor.Where(d => d.doc_cedula == User.Identity.Name).First();
                result = (from p in db.paciente
                          where (p.pac_cedula.ToUpper().Contains(search) ||
                              p.pac_nombres.ToUpper().Contains(search) ||
                              p.pac_apellidos.ToUpper().Contains(search)) && p.pac_empresa == doctor.doc_empresa
                          select new { p.pac_id, p.pac_nombres, p.pac_apellidos, p.pac_edad }).Distinct();
            }
            else {
                result = (from p in db.paciente
                              where p.pac_cedula.ToUpper().Contains(search) ||
                                  p.pac_nombres.ToUpper().Contains(search) ||
                                  p.pac_apellidos.ToUpper().Contains(search)
                          select new { p.pac_id, p.pac_nombres, p.pac_apellidos, p.pac_edad }).Distinct();
            }
            if (result.Count() == 0)
            {
                return new JsonResult() { Data = new { Data = new { pac_id = 0, pac_nombres = "Sin Datos", pac_apellidos = "", pac_edad = "" } } };
            }
            return new JsonResult() { Data = result };
        }
        private int numero_historia(historia historia)
        {
            int num = 0;
            num = db.historia.Where(h => h.his_tipo == historia.his_tipo && h.his_paciente == historia.his_paciente).Count();
            num++;
            return num;
        }

        private SelectList his_tipo(int? tipo = 0)
        {
            List<SelectListItem> list_tipo = new List<SelectListItem>();
            list_tipo.Add(new SelectListItem { Text = "Preocupacional", Value = "2" });
            list_tipo.Add(new SelectListItem { Text = "Periodica", Value = "3" });
            list_tipo.Add(new SelectListItem { Text = "Retiro", Value = "4" });
            SelectList tipos;
            if (tipo == 0)
                tipos = new SelectList(list_tipo, "Value", "Text");
            else
                tipos = new SelectList(list_tipo, "Value", "Text", tipo.ToString());
            return tipos;
        }

        private string change_tipo(int pac_id)
        {
            string mensaje = String.Empty;
            var consulta = db.ocupacional.Where(o => o.ocu_paciente == pac_id && o.ocu_tipo == "actual");
            if (!consulta.Any())
            {
                mensaje = "El paciente no registra un trabajo actual";
            }
            else
            {
                ocupacional ocupacional = db.ocupacional.Where(o => o.ocu_paciente == pac_id && o.ocu_tipo == "actual").First();
                ocupacional.ocu_tipo = "registro";
                ocupacional.ocu_estado = false;
                db.Entry(ocupacional).State = EntityState.Modified;
                db.SaveChanges();
                mensaje = "El trabajo actual anterior quedará como histórico";
            }

            return mensaje;
        }
        private string titulo(int tipo)
        {
            string titulo = String.Empty;
            switch (tipo)
            {
                case 1:
                    titulo = "Generales";
                    break;
                case 2:
                    titulo = "Preocupacionales";
                    break;
                case 3:
                    titulo = "Periodicas";
                    break;
                case 4:
                    titulo = "de Retiro";
                    break;
            }
            return titulo;
        }

        private string motivo(int tipo)
        {
            string titulo = String.Empty;
            switch (tipo)
            {
                case 1:
                    titulo = "Historia General";
                    break;
                case 2:
                    titulo = "Historia Preocupacionales";
                    break;
                case 3:
                    titulo = "Historia Periodicas";
                    break;
                case 4:
                    titulo = "Historia de Retiro";
                    break;
            }
            return titulo;
        }
        private int get_user()
        {
            int user_id = 0;
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("medico")) {
                    string user_name = String.Empty;
                    user_name = User.Identity.Name;
                    UserProfile userprofile = db_users.UserProfiles.Where(u => u.UserName == user_name).First();
                    user_id = userprofile.UserMedico.GetValueOrDefault();
                }
                else if (User.IsInRole("doctor")) {
                    string user_name = String.Empty;
                    user_name = User.Identity.Name;
                    var doctor = db.doctor.Where(d => d.doc_cedula == user_name).First();
                    user_id = doctor.doc_id;
                }
                
            }
            return user_id;
        }

        private string get_fecha(string fec_his) {
            string fecha = String.Empty;
            DateTime fecha_cal = DateTime.Parse(fec_his);
            string dia = fecha_cal.Day.ToString();
            string mes = String.Empty;
            string anio = fecha_cal.Year.ToString();
            switch (fecha_cal.Month) { 
                case 1:
                    mes = "enero";
                    break;
                case 2:
                    mes = "febrero";
                    break;
                case 3:
                    mes = "marzo";
                    break;
                case 4:
                    mes = "abril";
                    break;
                case 5:
                    mes = "mayo";
                    break;
                case 6:
                    mes = "junio";
                    break;
                case 7:
                    mes = "julio";
                    break;
                case 8:
                    mes = "agosto";
                    break;
                case 9:
                    mes = "septiembre";
                    break;
                case 10:
                    mes = "octubre";
                    break;
                case 11:
                    mes = "noviembre";
                    break;
                case 12:
                    mes = "diciembre";
                    break;
            }
            fecha = "Quito, " + dia + " de " + mes + " de " + anio;
            return fecha;
        }

        public Byte[] ConvertBytes(String ruta)
        {
            FileStream foto = new FileStream(ruta, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            Byte[] arreglo = new Byte[foto.Length];
            BinaryReader reader = new BinaryReader(foto);
            arreglo = reader.ReadBytes(Convert.ToInt32(foto.Length));
            return arreglo;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}