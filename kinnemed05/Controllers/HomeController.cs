using kinnemed05.Models;
using kinnemed05.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
//using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;



namespace kinnemed05.Controllers
{
    //[CustomAuthorize(UserRoles.admin,UserRoles.empresa,UserRoles.medico)]
    public class HomeController : Controller
    {
        public ActionResult Index(string mensaje)
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            
            if (String.IsNullOrEmpty(mensaje))
                ViewBag.mensaje = mensaje;
            ViewBag.perfil = get_perfil();
            return View();
        }

        public ActionResult Examen() {
            return View();
        }
        public ActionResult Historia() {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private string get_perfil() {
            string perfil = String.Empty;
            if(User.IsInRole("admin"))
                perfil="admin";
            else if(User.IsInRole("medico"))
                perfil="medico";
            else if (User.IsInRole("paciente"))
                perfil = "paciente";
            else if (User.IsInRole("laboratorista"))
                perfil = "laboratorista";
            return perfil;
        }

        public ActionResult Message(string mensaje) {
            ViewBag.mensaje = mensaje;
            if(Request.IsAjaxRequest())
                return PartialView();
            return View();
        }

       
        
    }
}
