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
    public partial class ViewLimpio : System.Web.UI.Page
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
            int id = Convert.ToInt32(Session["reg_id"]);
            dsPruebaPaciente dsPrueba = new dsPruebaPaciente();
            string conn = ConfigurationManager.AppSettings["conexion"];
            string strConsulta = "Select * from view_prueba_paciente where reg_id="+ id + " order by exa_id";
            registro registro = db.registro.Find(id);
            SqlConnection sqlcon = new SqlConnection(conn);
            SqlDataAdapter daQuimico = new SqlDataAdapter(strConsulta, sqlcon);
            daQuimico.Fill(dsPrueba, "view_prueba_paciente");

            reportDocument = new ReportDocument();
            string reportPath = Server.MapPath("~/Reports/RptLimpio.rpt");
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dsPrueba.Tables[0]);
            var paciente = db.paciente.Where(p => p.pac_id == registro.reg_paciente).First();
            var medico = db.medico.Where(m => m.med_id == registro.reg_medico).First();
            
            reportDocument.SetParameterValue("paciente", paciente.pac_nombres + " " + paciente.pac_apellidos);
            reportDocument.SetParameterValue("medico", medico.med_nombres + " " + medico.med_apellidos);
            reportDocument.SetParameterValue("fecha", registro.reg_fecha);
            reportDocument.SetParameterValue("edad", paciente.pac_edad);
            if (paciente.pac_genero != null)
                reportDocument.SetParameterValue("genero", paciente.pac_genero);
            else
                reportDocument.SetParameterValue("genero", "");
            reportDocument.SetParameterValue("hc", "");
            reportDocument.SetParameterValue("orden", registro.reg_orden);

            crViewer.ReportSource = reportDocument;
            crViewer.DataBind();
        }
    }
}