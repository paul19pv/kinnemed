﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kinnemed05.Models;
using kinnemed05.Security;
using kinnemed05.Filters;

namespace kinnemed05.Controllers
{
    [InitializeSimpleMembership]
    public class ConceptoController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Concepto/

        public ActionResult Index()
        {
            var concepto = db.concepto.Include(c => c.historia);
            return View(concepto.ToList());
        }

        //
        // GET: /Concepto/Details/5

        public ActionResult Details(int id = 0)
        {
            concepto concepto = db.concepto.Find(id);
            if (concepto == null)
            {
                return HttpNotFound();
            }
            return PartialView(concepto);
        }

        //
        // GET: /Concepto/Create
        [CustomAuthorize(UserRoles.medico,UserRoles.doctor)]
        public ActionResult Create(int id)
        {
            concepto concepto = db.concepto.Find(id);
            if (concepto != null)
            {
                return RedirectToAction("Edit", new { id = id });
            }
            historia historia = db.historia.Find(id);
            if (historia.his_tipo == 2 || historia.his_tipo == 3){
                ViewBag.con_resultado = res_periodica();
                ViewBag.con_valor = val_periodica();
            }

            else if (historia.his_tipo == 4)
            {
                ViewBag.con_resultado = res_retiro();
                ViewBag.con_valor = val_retiro();
            }
            else {
                ViewBag.con_resultado = res_periodica();
                ViewBag.con_valor = val_periodica();
            }
                
            ViewBag.con_id = id;
            ViewBag.con_seguimiento = opcion();
            return PartialView();
        }

        //
        // POST: /Concepto/Create
        [CustomAuthorize(UserRoles.medico, UserRoles.doctor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(concepto concepto)
        {
            historia historia = db.historia.Find(concepto.con_id);
            if (ModelState.IsValid && validar_resultado(historia.his_tipo,concepto.con_valor))
            {
                db.concepto.Add(concepto);
                db.SaveChanges();
                //return RedirectToAction("Message", "Home", new {mensaje="Proceso Finalizado" });
                return RedirectToAction("Firma", "Historia", new { id = concepto.con_id });
            }
            
            if (historia.his_tipo == 2 || historia.his_tipo == 3)
            {
                ViewBag.con_resultado = res_periodica(concepto.con_resultado);
                ViewBag.con_valor = val_periodica(concepto.con_valor);
            }

            else if (historia.his_tipo == 4)
            {
                ViewBag.con_resultado = res_retiro(concepto.con_resultado);
                ViewBag.con_valor = val_retiro(concepto.con_valor);
            }
            ViewBag.con_id = concepto.con_id;
            ViewBag.con_seguimiento = opcion(concepto.con_seguimiento);
            return PartialView(concepto);
        }

        //
        // GET: /Concepto/Edit/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Edit(int id = 0)
        {
            concepto concepto = db.concepto.Find(id);
            if (concepto == null)
            {
                return RedirectToAction("Create", new {id=id });
            }
            historia historia = db.historia.Find(concepto.con_id);
            if (historia.his_tipo == 2 || historia.his_tipo == 3)
            {
                ViewBag.con_resultado = res_periodica(concepto.con_resultado);
                ViewBag.con_valor = val_periodica(concepto.con_valor);
            }

            else if (historia.his_tipo == 4)
            {
                ViewBag.con_resultado = res_retiro(concepto.con_resultado);
                ViewBag.con_valor = val_retiro(concepto.con_valor);
            }
            ViewBag.con_id = id;
            ViewBag.con_seguimiento = opcion(concepto.con_seguimiento);
            return PartialView(concepto);
        }

        //
        // POST: /Concepto/Edit/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(concepto concepto)
        {
            historia historia = db.historia.Find(concepto.con_id);
            if (ModelState.IsValid && validar_resultado(historia.his_tipo, concepto.con_valor))
            {
                db.Entry(concepto).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Message", "Home", new { mensaje = "Proceso Finalizado" });
                return RedirectToAction("Firma", "Historia", new { id = concepto.con_id });
            }
            
            if (historia.his_tipo == 2 || historia.his_tipo == 3)
            {
                ViewBag.con_resultado = res_periodica(concepto.con_resultado);
                ViewBag.con_valor = val_periodica(concepto.con_valor);
            }

            else if (historia.his_tipo == 4)
            {
                ViewBag.con_resultado = res_retiro(concepto.con_resultado);
                ViewBag.con_valor = val_retiro(concepto.con_valor);
            }
            ViewBag.con_id = concepto.con_id;
            ViewBag.con_seguimiento = opcion(concepto.con_seguimiento);
            return PartialView(concepto);
        }

        //
        // GET: /Concepto/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Delete(int id = 0)
        {
            concepto concepto = db.concepto.Find(id);
            if (concepto == null)
            {
                return HttpNotFound();
            }
            return View(concepto);
        }

        //
        // POST: /Concepto/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            concepto concepto = db.concepto.Find(id);
            db.concepto.Remove(concepto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private SelectList res_periodica(string resultado = "")
        {
            List<SelectListItem> list_resultado = new List<SelectListItem>();
            list_resultado.Add(new SelectListItem { Text = "APTO", Value = "APTO" });
            list_resultado.Add(new SelectListItem { Text = "APTO CON RESTRICCIONES PERSONALES", Value = "APTO CON RESTRICCIONES PERSONALES" });
            list_resultado.Add(new SelectListItem { Text = "APTO CON RESTRICCIONES LABORALES", Value = "APTO CON RESTRICCIONES LABORALES" });
            list_resultado.Add(new SelectListItem { Text = "NO APTO", Value = "NO APTO" });
            SelectList resultados;
            if (resultado == "")
                resultados = new SelectList(list_resultado, "Value", "Text");
            else
                resultados = new SelectList(list_resultado, "Value", "Text", resultado);
            return resultados;
        }

        private SelectList res_retiro(string resultado = "")
        {
            List<SelectListItem> list_resultado = new List<SelectListItem>();
            list_resultado.Add(new SelectListItem { Text = "SATISFACTORIA", Value = "SATISFACTORIA" });
            list_resultado.Add(new SelectListItem { Text = "NO SATISFACTORIA", Value = "NO SATISFACTORIA" });
            SelectList resultados;
            if (resultado == "")
                resultados = new SelectList(list_resultado, "Value", "Text");
            else
                resultados = new SelectList(list_resultado, "Value", "Text", resultado);
            return resultados;
        }

        private SelectList val_periodica(string resultado = "")
        {
            List<SelectListItem> list_resultado = new List<SelectListItem>();
            list_resultado.Add(new SelectListItem { Text = "", Value = "" });
            list_resultado.Add(new SelectListItem { Text = "PERSONAL", Value = "PERSONAL" });
            list_resultado.Add(new SelectListItem { Text = "ADAPTATIVA", Value = "ADAPTATIVA" });
            list_resultado.Add(new SelectListItem { Text = "LABORAL", Value = "LABORAL" });
            SelectList resultados;
            //if (resultado == "")
            //    resultados = new SelectList(list_resultado, "Value", "Text");
            //else
                resultados = new SelectList(list_resultado, "Value", "Text", resultado);
            return resultados;
        }

        private SelectList val_retiro(string resultado = "")
        {
            List<SelectListItem> list_resultado = new List<SelectListItem>();
            list_resultado.Add(new SelectListItem { Text = "", Value = "" });
            list_resultado.Add(new SelectListItem { Text = "SI", Value = "SI" });
            list_resultado.Add(new SelectListItem { Text = "NO", Value = "NO" });
            SelectList resultados;
            //if (resultado == "")
            //    resultados = new SelectList(list_resultado, "Value", "Text");
            //else
            resultados = new SelectList(list_resultado, "Value", "Text", resultado);
            return resultados;
        }
        private SelectList opcion(string valor = "")
        {
            List<SelectListItem> list_opcion = new List<SelectListItem>();
            list_opcion.Add(new SelectListItem { Text = "SI", Value = "SI" });
            list_opcion.Add(new SelectListItem { Text = "NO", Value = "NO" });
            SelectList opciones;
            if (valor == "")
                opciones = new SelectList(list_opcion, "Value", "Text");
            else
                opciones = new SelectList(list_opcion, "Value", "Text", valor);
            return opciones;
        }
        private bool validar_resultado(int his_tipo, string con_valor) {
            bool result=true;
            if (his_tipo == 4) {
                if (con_valor==null) {
                    result = false;
                    ModelState.AddModelError("valor","El campo es Obligatorio");
                }
                    
                
            }

            return result;
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}