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
    public partial class ViewEspirometria : System.Web.UI.Page
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

            dsEspirometria dsPrueba = new dsEspirometria();
            string conn = ConfigurationManager.AppSettings["conexion"];
            int id = Convert.ToInt32(Session["esp_id"]);
            espirometria espirometria = db.espirometria.Find(id);
            
            //string fileName = medico.med_firma;
            //if (String.IsNullOrEmpty(fileName))
            //    fileName = "firma.png";

            string strEspirometria = "Select * from espirometria where esp_id=" + id;
            string strPaciente = "Select * from paciente where pac_id=" + espirometria.esp_paciente;
            


            SqlConnection sqlcon = new SqlConnection(conn);
            SqlDataAdapter daEspirometria = new SqlDataAdapter(strEspirometria, sqlcon);
            SqlDataAdapter daPaciente = new SqlDataAdapter(strPaciente, sqlcon);
            
            daEspirometria.Fill(dsPrueba, "espirometria");
            daPaciente.Fill(dsPrueba, "paciente");

            reportDocument = new ReportDocument();
            //Report path
            string reportPath = Server.MapPath("~/Reports/RptEspirometria.rpt");
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dsPrueba);
            reportDocument.SetParameterValue("hc", "");
            reportDocument.SetParameterValue("orden", "");
            //string path01 = Path.Combine(Server.MapPath("~/Content/firmas"), fileName);
            //reportDocument.SetParameterValue("picturePath", path01);
            crViewer.ReportSource = reportDocument;
            crViewer.DataBind();

        }
    }
}