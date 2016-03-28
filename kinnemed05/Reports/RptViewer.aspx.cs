using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace kinnemed05.Reports
{
    public partial class RptViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CrystalDecisions.CrystalReports.Engine.ReportDocument report =new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            string reportPath = Server.MapPath("~/Reports/RptMedico.rpt");
            report.Load(reportPath);
            report.SetDataSource(Session["ReportSource"]);
            report.SetParameterValue("titulo", "General");
            crViewer.ReportSource = report;
            crViewer.DataBind();
        }
    }
}