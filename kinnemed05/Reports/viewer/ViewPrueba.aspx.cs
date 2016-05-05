using CrystalDecisions.CrystalReports.Engine;
using kinnemed05.Models;
using kinnemed05.Reports.dataset;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace kinnemed05.Reports.viewer
{
    public partial class ViewPrueba : System.Web.UI.Page
    {
        ReportDocument reportDocument = null;
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.reportDocument != null)
            {
                this.reportDocument.Close();
                this.reportDocument.Dispose();
            }
            dsPruebaPaciente dsPrueba = new dsPruebaPaciente();
            string conn = ConfigurationManager.AppSettings["conexion"];
            int pac_id = Convert.ToInt32(Session["reg_paciente"]);
            string fecha = Convert.ToString(Session["reg_fecha"]);
            string strAglutinacion = "Select * from view_prueba_paciente where reg_id="
                + Session["reg_id"] + " order by exa_id";
            int id=Convert.ToInt32(Session["reg_id"]);
            registro registro_ = db.registro.Find(id);
            laboratorista laboratorista = db.laboratorista.Find(registro_.reg_laboratorista);
            string fileName = laboratorista.lab_firma;
            if (String.IsNullOrEmpty(fileName))
                fileName = "firma_lab.png";
            SqlConnection sqlcon = new SqlConnection(conn);
            SqlDataAdapter daQuimico = new SqlDataAdapter(strAglutinacion, sqlcon);
            daQuimico.Fill(dsPrueba, "view_prueba_paciente");

            reportDocument = new ReportDocument();
            //Report path
            string reportPath = Server.MapPath("~/Reports/RptPrueba.rpt");
            reportDocument.Load(reportPath);



            reportDocument.SetDataSource(dsPrueba.Tables[0]);
            var registro = db.registro.Where(r => r.reg_paciente == pac_id && r.reg_fecha == fecha).First();
            var paciente = db.paciente.Where(p => p.pac_id == pac_id).First();
            reportDocument.SetParameterValue("medico", "WORKFORCE");
            if (registro.reg_medico != null)
            {
                var medico = db.medico.Where(m => m.med_id == registro.reg_medico).First();
                if (medico.med_nombres != "NO APLICA")
                {
                    reportDocument.SetParameterValue("medico", medico.med_nombres + " " + medico.med_apellidos);
                }
                else {
                    var empresa = db.empresa.Where(emp => emp.emp_id == registro.paciente.pac_empresa).First();
                    reportDocument.SetParameterValue("medico", empresa.emp_nombre);

                }
            }


            reportDocument.SetParameterValue("paciente", paciente.pac_nombres + " " + paciente.pac_apellidos);
            reportDocument.SetParameterValue("fecha", fecha);
            reportDocument.SetParameterValue("edad", paciente.pac_edad);
            if(paciente.pac_genero!=null)
            reportDocument.SetParameterValue("genero", paciente.pac_genero);
            else
                reportDocument.SetParameterValue("genero", "");
            reportDocument.SetParameterValue("hc", "");
            reportDocument.SetParameterValue("orden", registro.reg_orden);
            string path01 = Path.Combine(Server.MapPath("~/Content/firmas"), fileName);
            reportDocument.SetParameterValue("picturePath", path01);

            crViewer.ReportSource = reportDocument;
            crViewer.DataBind();
        }
    }
}