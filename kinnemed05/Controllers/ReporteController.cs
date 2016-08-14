using kinnemed05.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace kinnemed05.Controllers
{
    public class ReporteController : Controller
    {
        //
        // GET: /Reporte/

        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        public ActionResult Index(string fecha_ini,string fecha_fin, int empresa)
        {
            var lista = db.getLista01(fecha_ini, fecha_fin,empresa);

            return View(lista.ToList());
        }
        

        public ActionResult Reporte01()
        {
            ViewBag.empresa = new SelectList(db.empresa, "emp_id", "emp_nombre");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reporte01(string fec_ini, string fec_fin, int empresa)
        {
            try
            {
                var lista = db.getLista01(fec_ini, fec_fin, empresa);
                var grid = new GridView();
                grid.DataSource = lista;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return View("Lista02", lista.ToList());
            }
            catch (Exception ex)
            {
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message });
            }
        }


        public ActionResult Reporte02()
        {
            ViewBag.empresa = new SelectList(db.empresa, "emp_id", "emp_nombre");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reporte02(string fec_ini, string fec_fin, int empresa)
        {
            try
            {
                var lista = db.getLista02(fec_ini, fec_fin, empresa);
                var grid = new GridView();
                grid.DataSource = lista;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return View("Lista02", lista.ToList());
            }
            catch (Exception ex)
            {
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message });
            }
        }


        public ActionResult Reporte03()
        {
            ViewBag.empresa = new SelectList(db.empresa, "emp_id", "emp_nombre");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reporte03(string fec_ini, string fec_fin, int empresa)
        {
            try
            {
                var lista = db.getLista03(fec_ini, fec_fin, empresa);
                var grid = new GridView();
                grid.DataSource = lista;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return View("Lista02", lista.ToList());
            }
            catch (Exception ex)
            {
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message });
            }
        }


        public ActionResult Reporte04()
        {
            ViewBag.empresa = new SelectList(db.empresa, "emp_id", "emp_nombre");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reporte04(string fec_ini, string fec_fin, int empresa)
        {
            try
            {
                var lista = db.getLista04(fec_ini, fec_fin, empresa);
                var grid = new GridView();
                grid.DataSource = lista;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return View("Lista02", lista.ToList());
            }
            catch (Exception ex)
            {
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message });
            }
        }

        public ActionResult Reporte05()
        {
            ViewBag.empresa = new SelectList(db.empresa, "emp_id", "emp_nombre");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reporte05(string fec_ini, string fec_fin, int empresa)
        {
            try
            {
                var lista = db.getLista05(fec_ini, fec_fin, empresa);
                var grid = new GridView();
                grid.DataSource = lista;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return View("Lista02", lista.ToList());
            }
            catch (Exception ex)
            {
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message });
            }
        }

        public ActionResult Reporte06()
        {
            ViewBag.empresa = new SelectList(db.empresa, "emp_id", "emp_nombre");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reporte06(string fec_ini, string fec_fin, int empresa)
        {
            try
            {
                var lista = db.getLista06(fec_ini, fec_fin, empresa);
                var grid = new GridView();
                grid.DataSource = lista;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return View("Lista02", lista.ToList());
            }
            catch (Exception ex)
            {
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message });
            }
        }



        public ActionResult Reporte07()
        {
            ViewBag.empresa = new SelectList(db.empresa, "emp_id", "emp_nombre");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reporte07(string fec_ini, string fec_fin, int empresa)
        {
            try
            {
                var lista = db.getLista07(fec_ini, fec_fin, empresa);
                var grid = new GridView();
                grid.DataSource = lista;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return View("Lista02", lista.ToList());
            }
            catch (Exception ex)
            {
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message });
            }
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExportReport(string fec_ini, string fec_fin, int empresa) {

            try
            {
                var lista = db.getLista02(fec_ini, fec_fin, empresa);
                var grid = new GridView();
                grid.DataSource = lista;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return View("Lista02", lista.ToList());
            }
            catch (Exception ex) {
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message });
            }
            

        }


    }
}
