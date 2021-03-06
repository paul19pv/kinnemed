﻿using kinnemed05.Models;
using kinnemed05.Security;
using Newtonsoft.Json;
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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;



namespace kinnemed05.Controllers
{
    //[CustomAuthorize(UserRoles.admin,UserRoles.empresa,UserRoles.medico)]
    
    public class HomeController : Controller
    {
        [CustomAuthorize(UserRoles.admin, UserRoles.empresa, UserRoles.medico, UserRoles.laboratorista, UserRoles.paciente, UserRoles.trabajador, UserRoles.doctor)]
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
        [CustomAuthorize(UserRoles.admin, UserRoles.empresa, UserRoles.medico, UserRoles.laboratorista, UserRoles.trabajador,UserRoles.doctor)]
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
            else if (User.IsInRole("doctor"))
                perfil = "doctor";
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
            string respuesta = String.Empty;
            respuesta = mensaje.enviar("0998593448", "SR.(a) Paciente sus examenes realizados en el Centro M&aacute;dico Kinnmed están listos reviselos en kinnemed.com con cédula para usuario y clave");
            //string respuesta2 = mensaje.mail("juanjavierj@gmail.com", "mensaje prueba");
            //RunAsync().Wait(); ;
            return RedirectToAction("Message", new { mensaje = respuesta });
        
        }

        public static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://envia-movil.com");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                                                                        Convert.ToBase64String(
                                                                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                                                              string.Format("{0}:{1}", "C09B2EB4BC", "4E28EE0CBD2796D"))));

                string mensaje = "Mensaje titulo";
                string[] numeros = { "593998593448", "59384659882" };
                string[] Mensajes = { "msj 1", "msj 2" };


                var envio = new EnvioDTO();
                envio.Mensaje = mensaje;
                envio.Mensajes = Mensajes;
                envio.Destinatarios = numeros;
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Envios", envio);
                if (response.IsSuccessStatusCode)
                {
                    // Get the URI of the created resource.

                }
            }
        }


        public async Task<ActionResult> IndexMsg()
        {
            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://envia-movil.com");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                                                                        Convert.ToBase64String(
                                                                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                                                              string.Format("{0}:{1}", "FED3ADC535", "F37AEA0E3161838"))));

                string mensaje = "Mensaje titulo";
                string[] numeros = { "593998593448", "59384659882" };
                string[] Mensajes = { "msj 1", "msj 2" };


                var envio = new EnvioDTO();
                envio.Mensaje = mensaje;
                envio.Mensajes = Mensajes;
                envio.Destinatarios = numeros;
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Envios", envio);
                if (response.IsSuccessStatusCode)
                {
                    //var model = JsonConvert.DeserializeObject<Tweets>(content);
                    return View();
                }

                // an error occurred => here you could log the content returned by the remote server
                return Content("An error occurred: ");
            }
        }

       
        
    }
}
