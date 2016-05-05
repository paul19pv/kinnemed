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

namespace kinnemed05.Reports.Viewer
{
    public partial class ViewRayos : System.Web.UI.Page
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

            dsRayos dsPrueba = new dsRayos();
            string conn = ConfigurationManager.AppSettings["conexion"];
            int id = Convert.ToInt32(Session["ray_id"]);
            rayos rayos = db.rayos.Find(id);
            medico medico = db.medico.Find(rayos.ray_medico);
            string fileName = medico.med_firma;
            if (String.IsNullOrEmpty(fileName))
                fileName = "firma.png";

            string strAudiometia = "Select * from rayos where ray_id=" + id;
            string strPaciente = "Select * from paciente where pac_id=" + rayos.ray_paciente;
            string strMedico = "Select * from medico where med_id=" + rayos.ray_medico;


            SqlConnection sqlcon = new SqlConnection(conn);
            SqlDataAdapter daAudiometria = new SqlDataAdapter(strAudiometia, sqlcon);
            SqlDataAdapter daPaciente = new SqlDataAdapter(strPaciente, sqlcon);
            SqlDataAdapter daMedico = new SqlDataAdapter(strMedico, sqlcon);
            daAudiometria.Fill(dsPrueba, "rayos");
            daPaciente.Fill(dsPrueba, "paciente");
            daMedico.Fill(dsPrueba, "medico");

            reportDocument = new ReportDocument();
            //Report path
            string reportPath = Server.MapPath("~/Reports/RptRayos.rpt");
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dsPrueba);
            reportDocument.SetParameterValue("hc", "");
            reportDocument.SetParameterValue("orden", "");
            string path01 = Path.Combine(Server.MapPath("~/Content/firmas"), fileName);
            reportDocument.SetParameterValue("picturePath", path01);
            crViewer.ReportSource = reportDocument;
            crViewer.DataBind();

        }
    }
}