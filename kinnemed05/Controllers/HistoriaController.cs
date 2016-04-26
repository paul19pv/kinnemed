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
using System.IO;

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
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Index(int tipo, int? paciente, string fecha)
        {
            var historia = db.historia.Include(h=>h.paciente).Where(h => h.his_tipo == tipo);
            if (paciente != null)
                historia = historia.Where(h => h.his_paciente == paciente);
            if (!String.IsNullOrEmpty(fecha))
                historia = historia.Where(h => h.his_fecha == fecha);

            ViewBag.paciente = "";
            ViewBag.fecha = "";
            ViewBag.tipo = tipo;
            ViewBag.titulo = titulo(tipo);
            return View(historia.ToList());
        }

        //
        // GET: /Historia/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
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
        [CustomAuthorize(UserRoles.medico)]
        public ActionResult Create(int tipo)
        {
            ViewBag.tipo = tipo;
            ViewBag.his_tipo = his_tipo(tipo);
            if (tipo != 1) {
                ViewBag.his_motivo = motivo(tipo);
                return View("Create01");
            }
                
            return View();
        }

        //
        // POST: /Historia/Create
        [CustomAuthorize(UserRoles.medico)]
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
                return RedirectToAction("Create", "Personal", new { id = historia.his_paciente});
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
        [CustomAuthorize(UserRoles.medico)]
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
        [CustomAuthorize(UserRoles.medico)]
        public ActionResult Edit(int id = 0)
        {
            historia historia = db.historia.Find(id);
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
            
            return View("Edit01",historia);
        }



        //
        // POST: /Historia/Edit/5
        [CustomAuthorize(UserRoles.medico)]
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

        [CustomAuthorize(UserRoles.medico)]
        public ActionResult Edit02(int id = 0)
        {
            historia historia = db.historia.Find(id);
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



        //
        // POST: /Historia/Edit/5
        [CustomAuthorize(UserRoles.medico)]
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
        [CustomAuthorize(UserRoles.medico)]
        public ActionResult Problema(int id)
        {
            historia historia = db.historia.Find(id);
            if (historia == null)
            {
                return HttpNotFound();
            }
            int tipo=Convert.ToInt32(Session["his_tipo"]);
            //if (tipo != 1)
            //    return RedirectToAction("Edit", "Familiar", new { id = historia.his_paciente });
            return PartialView(historia);
        }

        [CustomAuthorize(UserRoles.medico)]
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
        //
        // GET: /Historia/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin)]
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
        [CustomAuthorize(UserRoles.medico, UserRoles.admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            historia historia = db.historia.Find(id);
            db.historia.Remove(historia);
            db.SaveChanges();
            return RedirectToAction("Index", new {tipo=historia.his_tipo });
        }


        public ActionResult Certificado(int id,int pac_id) {

            try
            {
                dsCertificado dsCertificado = new dsCertificado();
                paciente paciente = db.paciente.Find(pac_id);
                historia historia = db.historia.Find(id);
                concepto concepto = db.concepto.Find(id);
                medico medico = db.medico.Find(historia.his_medico);
                var consulta = db.ocupacional.Where(o => o.ocu_paciente == pac_id && o.ocu_tipo == "actual" && o.ocu_estado == true);
                ocupacional ocupacional=new ocupacional();

                string fileName = medico.med_firma;
                if (String.IsNullOrEmpty(fileName))
                    fileName = "firma.png";

                if(consulta.Any())
                    ocupacional=consulta.First();
                string conn = ConfigurationManager.AppSettings["conexion"];

                string strHistoria = "Select * from historia where his_id="+id;
                string strPaciente = "Select * from paciente where pac_id="+pac_id;
                string strConcepto = "Select * from concepto where con_id="+id;
                string strEmpresa = "Select * from empresa where emp_id="+paciente.pac_empresa;
                string strMedico = "Select * from medico where med_id="+historia.his_medico;
                string strOcupacional=String.Empty;
                if (ocupacional.ocu_id != null)
                    strOcupacional = "Select * from ocupacional where ocu_id=" + ocupacional.ocu_id;
                else
                    strOcupacional = "Select top 1 * from ocupacional";

                SqlConnection sqlcon = new SqlConnection(conn);
                SqlDataAdapter daHistoria=new SqlDataAdapter(strHistoria,sqlcon);
                SqlDataAdapter daPaciente=new SqlDataAdapter(strPaciente,sqlcon);
                SqlDataAdapter daConcepto=new SqlDataAdapter(strConcepto,sqlcon);
                SqlDataAdapter daEmpresa = new SqlDataAdapter(strEmpresa, sqlcon);
                SqlDataAdapter daMedico = new SqlDataAdapter(strMedico, sqlcon);
                SqlDataAdapter daOcupacional = new SqlDataAdapter(strOcupacional, sqlcon);
                daHistoria.Fill(dsCertificado,"historia");
                daPaciente.Fill(dsCertificado,"paciente");
                daConcepto.Fill(dsCertificado,"concepto");
                daEmpresa.Fill(dsCertificado, "empresa");
                daMedico.Fill(dsCertificado, "medico");
                daOcupacional.Fill(dsCertificado, "ocupacional");

                RptCertificado rp_base = new RptCertificado();
                rp_base.Load(Path.Combine(Server.MapPath("~/Reports"), "RptCertificado.rpt"));
                rp_base.SetDataSource(dsCertificado);
                Stream stream = rp_base.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                if (concepto.con_resultado == "APTO") {
                    RptCerApto rp = new RptCerApto();
                    rp.Load(Path.Combine(Server.MapPath("~/Reports"),"RptCerApto.rpt"));
                    rp.SetDataSource(dsCertificado);
                    stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (concepto.con_resultado == "APTO CON RESTRICCIONES PERSONALES" || concepto.con_resultado == "APTO CON RESTRICCIONES LABORALES")
                {
                    RptCerAptoRes rp = new RptCerAptoRes();
                    rp.Load(Path.Combine(Server.MapPath("~/Reports"), "RptCerAptoRes.rpt"));
                    rp.SetDataSource(dsCertificado);
                    string path01 = Path.Combine(Server.MapPath("~/Content/firmas"), fileName);
                    rp.SetParameterValue("picturePath", path01);
                    stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (concepto.con_resultado == "NO APTO") {
                    RptCerNoApto rp = new RptCerNoApto();
                    rp.Load(Path.Combine(Server.MapPath("~/Reports"), "RptCerNoApto.rpt"));
                    rp.SetDataSource(dsCertificado);
                    stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
                else if (concepto.con_resultado == "SATISFACTORIA" || concepto.con_resultado == "NO SATISFACTORIA") {
                    RptCerRetiro rp = new RptCerRetiro();
                    rp.Load(Path.Combine(Server.MapPath("~/Reports"), "RptCerRetiro.rpt"));
                    rp.SetDataSource(dsCertificado);
                    string nexo=String.Empty;
                    if(concepto.con_valor=="NO")
                        nexo="NINGUNA";
                    else
                        nexo="UNA";
                    rp.SetParameterValue("nexo",nexo);
                    stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                }
               
                    
                
                //RptCertificado rp= new RptCertificado();
                //rp.Load(Path.Combine(Server.MapPath("~/Reports"), "RptCertificado.rpt"));
                //rp.SetDataSource(dsCertificado);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                //Stream stream = rp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Certificado.pdf");

            }
            catch (Exception ex) {
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message });
            }

        }

        public JsonResult AutocompletePaciente(string search)
        {
            search = search.ToUpper();
            var result = (from p in db.paciente
                          where p.pac_cedula.ToUpper().Contains(search) ||
                              p.pac_nombres.ToUpper().Contains(search) ||
                              p.pac_apellidos.ToUpper().Contains(search)
                          select new { p.pac_id, p.pac_nombres, p.pac_apellidos, p.pac_edad }).Distinct();
            if (result.Count() == 0)
            {
                return new JsonResult() { Data = new { Data = new { pac_id = 0, pac_nombres = "Sin Datos", pac_apellidos = "",pac_edad="" } } };
            }
            return new JsonResult() { Data = result };
        }
        private int numero_historia(historia historia) {
            int num = 0;
            num = db.historia.Where(h => h.his_tipo == historia.his_tipo && h.his_paciente == historia.his_paciente).Count();
            num++;
            return num;
        }

        private SelectList his_tipo(int? tipo=0)
        {
            List<SelectListItem> list_tipo = new List<SelectListItem>();
            list_tipo.Add(new SelectListItem { Text = "Preocupacional", Value = "2" });
            list_tipo.Add(new SelectListItem { Text = "Periodica", Value = "3" });
            list_tipo.Add(new SelectListItem { Text = "Retiro", Value = "4" });
            SelectList tipos;
            if(tipo==0)
                tipos= new SelectList(list_tipo, "Value", "Text");
            else
                tipos = new SelectList(list_tipo, "Value", "Text",tipo.ToString());
            return tipos;
        }

        private string change_tipo(int pac_id) {
            string mensaje = String.Empty;
            var consulta = db.ocupacional.Where(o => o.ocu_paciente == pac_id && o.ocu_tipo == "actual");
            if (!consulta.Any())
            {
                mensaje = "El paciente no registra un trabajo actual";
            }
            else {
                ocupacional ocupacional = db.ocupacional.Where(o => o.ocu_paciente == pac_id && o.ocu_tipo == "actual").First();
                ocupacional.ocu_tipo = "registro";
                ocupacional.ocu_estado = false;
                db.Entry(ocupacional).State = EntityState.Modified;
                db.SaveChanges();
                mensaje = "El trabajo actual anterior quedará como histórico";
            }
            
            return mensaje;
        }
        private string titulo(int tipo) {
            string titulo = String.Empty;
            switch (tipo) { 
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
        private int get_user() {
            int user_id = 0;
            if (Request.IsAuthenticated) {
                string user_name = String.Empty;
                user_name = User.Identity.Name;
                UserProfile userprofile = db_users.UserProfiles.Where(u => u.UserName == user_name).First();
                user_id = userprofile.UserMedico.GetValueOrDefault();
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