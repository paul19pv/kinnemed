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
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;



namespace kinnemed05.Controllers
{
    //[CustomAuthorize(UserRoles.admin,UserRoles.empresa,UserRoles.medico)]
    
    public class HomeController : Controller
    {
        [CustomAuthorize(UserRoles.admin, UserRoles.empresa, UserRoles.medico, UserRoles.laboratorista, UserRoles.paciente, UserRoles.trabajador)]
        public ActionResult Index(string mensaje)
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            
            if (String.IsNullOrEmpty(mensaje))
                ViewBag.mensaje = mensaje;
            ViewBag.perfil = get_perfil();
            return View();
        }
        [CustomAuthorize(UserRoles.admin, UserRoles.empresa, UserRoles.medico, UserRoles.laboratorista, UserRoles.paciente)]
        public ActionResult Examen() {
            return View();
        }
        [CustomAuthorize(UserRoles.admin, UserRoles.empresa, UserRoles.medico, UserRoles.laboratorista, UserRoles.trabajador)]
        public ActionResult Historia() {
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
            else if (User.IsInRole("empresa"))
                perfil = "empresa";
            else if (User.IsInRole("laboratorista"))
                perfil = "laboratorista";
            else if (User.IsInRole("trabajador"))
                perfil = "trabajador";
            return perfil;
        }

        public ActionResult Message(string mensaje) {
            ViewBag.mensaje = mensaje;
            if(Request.IsAjaxRequest())
                return PartialView();
            return View();
        }


        public ActionResult Mensajetxt() {
            Mensaje mensaje = new Mensaje();
            string respuesta = mensaje.enviar("0984659882","Hola mundo");
            string respuesta2 = mensaje.mail("juanjavierj@gmail.com", "mensaje prueba");
            return RedirectToAction("Message", new { mensaje = respuesta+respuesta2 });
        
        }

       
        
    }
}
