﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace kinnemed05.Models
{
    public class Mensaje
    {
        public string enviar(string numero, string mensaje) {
            if (numero.Length >= 10)
                numero = numero.Substring(1, 9);

            //Se fija la URL sobre la que enviar la petici´on POST
            //Como ejemplo la petici´on se env´ıa a www.altiria.net/sustituirPOSTsms
            //Se debe reemplazar la cadena ’/sustituirPOSTsms’ por la parte correspondiente
            //de la URL suministrada por Altiria al dar de alta el servicio
            HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create("http://www.textoatodos.com/sistema/wss/smsapi16.php");
            //mensaje = HttpUtility.HtmlDecode(mensaje);
            mensaje = "SR(A). PACIENTE SUS EXAMANES REALIZADOS EN EL CENTRO MEDICO KINNEMED ESTAN LISTOS REVISELOS EN kinnemed.com CON CEDULA PARA USUARIO Y CLAVE";
            // Compone el mensaje a enviar
            // XX, YY y ZZ se corresponden con los valores de identificaci´on del usuario en el sistema.
            string lcPostData = "usuario=dixasoft&password=302299&celular=" + numero + "&mensaje=" + mensaje + "&lada=9";

            //string lcPostData = "HLRQuery?USERNAME=dixasoft12&PASSWORD=Sa5MwF6J&MSISDN=593998593448";

            // Lo codifica en utf-8
            byte[] lbPostBuffer = System.Text.Encoding.GetEncoding("utf-8").GetBytes(lcPostData);
            loHttp.Method = "POST";
            loHttp.ContentType = "application/x-www-form-urlencoded";
            loHttp.ContentLength = lbPostBuffer.Length;
            // Env´ıa la peticion
            Stream loPostData = loHttp.GetRequestStream();
            loPostData.Write(lbPostBuffer, 0, lbPostBuffer.Length);
            loPostData.Close();
            // Prepara el objeto para obtener la respuesta
            HttpWebResponse loWebResponse = (HttpWebResponse)loHttp.GetResponse();
            // La respuesta vendr´a codificada en utf-8
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader loResponseStream = new StreamReader(loWebResponse.GetResponseStream(), enc);
            // Conseguimos la respuesta en una cadena de texto
            string lcHtml = loResponseStream.ReadToEnd();
            loWebResponse.Close();
            loResponseStream.Close();
            return lcHtml;
        }

        


       

        public string mail(string destinatario, string mensaje) {
            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(destinatario));
            email.From = new MailAddress("info@kinnemedsystem.com");
            email.Subject = "Resultado de exámenes";
            email.Body = mensaje;
            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mail.kinnemedsystem.com";
            smtp.Port = 8889;
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("info@kinnemedsystem.com", "Kinnemed2015!");

            string output = null;

            try
            {
                smtp.Send(email);
                email.Dispose();
                output = "Correo electrónico fue enviado satisfactoriamente.";
            }
            catch (Exception ex)
            {
                output = "Error enviando correo electrónico: " + ex.Message;
            }
            return output;
        }
    }



    public class EnvioDTO {
        public string[] Mensajes { get; set; }
        public string[] Destinatarios { get; set; }
        public string Mensaje { get; set; }
    }
}