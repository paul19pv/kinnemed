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
    public partial class ViewHistoria : System.Web.UI.Page
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
            int id = Convert.ToInt32(Session["his_id"]);

            dsHistoria dshistoria = new dsHistoria();
            //dsHistorico dshistorico = new dsHistorico();
            string conn = ConfigurationManager.AppSettings["conexion"];
            SqlConnection sqlcon = new SqlConnection(conn);



            historia historia = db.historia.Find(id);

            string strHistoria = "Select * from view_historia where his_id=" + id;
            

            SqlDataAdapter daHistoria = new SqlDataAdapter(strHistoria, sqlcon);

            daHistoria.Fill(dshistoria, "view_historia");
            

            reportDocument = new ReportDocument();
            //Report path
            string reportPath = Server.MapPath("~/Reports/RptHistoria.rpt");
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dshistoria);

            crViewer.ReportSource = reportDocument;
            crViewer.DataBind();
        }
    }
}