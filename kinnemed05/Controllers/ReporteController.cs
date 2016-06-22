using kinnemed05.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

    }
}
