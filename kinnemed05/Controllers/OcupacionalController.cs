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
    //[CustomAuthorize(UserRoles.medico)]
    public class OcupacionalController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Ocupacional/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Index(int id)
        {
            var ocupacional = db.ocupacional.Include(o => o.paciente).Where(o => o.ocu_paciente == id && o.ocu_tipo == "histórico");
            return PartialView(ocupacional.ToList());
        }
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Consulta(int id)
        {
            var ocupacional = db.ocupacional.Include(o => o.paciente).Where(o => o.ocu_paciente == id && o.ocu_tipo == "histórico");
            return PartialView(ocupacional.ToList());
        }

        //
        // GET: /Ocupacional/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Details(int id, int tipo)
        {
            ocupacional ocupacional = db.ocupacional.Where(o => o.ocu_paciente == id && o.ocu_tipo == "actual").First();
            if (ocupacional == null)
            {
                return HttpNotFound();
            }
            ViewBag.his_tipo = tipo;
            return PartialView(ocupacional);
        }

        //
        // GET: /Ocupacional/Create

        [CustomAuthorize(UserRoles.medico, UserRoles.doctor)]
        public ActionResult Create(int id)
        {
            var consulta = db.ocupacional.Where(o => o.ocu_paciente == id && o.ocu_tipo == "actual" && o.ocu_estado == true);
            //ocupacional ocupacional = db.ocupacional.Find(id);
            if (consulta.Any())
            {
                return RedirectToAction("Edit", new { id = id });
            }
            ViewBag.ocu_paciente = id;
            ViewBag.ocu_tipo = "actual";
            ViewBag.ocu_jornada = ocu_jornada();
            ViewBag.his_tipo = Session["his_tipo"];
            return PartialView();
        }

        //
        // POST: /Ocupacional/Create

        [CustomAuthorize(UserRoles.medico, UserRoles.doctor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ocupacional ocupacional)
        {
            ocupacional.ocu_estado = true;
            ocupacional.ocu_tiempo = get_tiempo(ocupacional.ocu_inicio);
            if (ModelState.IsValid)
            {
                db.ocupacional.Add(ocupacional);
                db.SaveChanges();
                Session["pac_id"] = ocupacional.ocu_paciente;
                return RedirectToAction("Edit", "Laboral", new { id = ocupacional.ocu_id });
            }
            ViewBag.ocu_paciente = ocupacional.ocu_paciente;
            ViewBag.ocu_tipo = ocupacional.ocu_tipo;
            ViewBag.ocu_jornada = ocu_jornada(ocupacional.ocu_jornada);

            ViewBag.his_tipo = Session["his_tipo"];
            return PartialView(ocupacional);
        }


        //
        // GET: /Ocupacional/Create

        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Historico(int id)
        {
            ocupacional ocupacional = db.ocupacional.Where(o => o.ocu_paciente == id && o.ocu_tipo == "actual").First();
            ViewBag.ocu_paciente = id;
            ViewBag.ocu_tipo = "histórico";
            ViewBag.ocu_jornada = ocu_jornada();
            ViewBag.ocu_id = ocupacional.ocu_id;
            return PartialView();
        }

        //
        // POST: /Ocupacional/Create

        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Historico(ocupacional ocupacional)
        {
            ocupacional.ocu_estado = true;
            if (ModelState.IsValid)
            {
                db.ocupacional.Add(ocupacional);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = ocupacional.ocu_paciente });
            }
            ViewBag.ocu_paciente = ocupacional.ocu_paciente;
            ViewBag.ocu_tipo = "histórico";
            ViewBag.ocu_jornada = ocu_jornada(ocupacional.ocu_jornada);
            return PartialView(ocupacional);
        }



        //
        // GET: /Ocupacional/Edit/5

        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Edit(int id)
        {
            var consulta = db.ocupacional.Where(o => o.ocu_paciente == id && o.ocu_tipo == "actual" && o.ocu_estado == true);
            //ocupacional ocupacional = db.ocupacional.Find(id);
            if (!consulta.Any())
            {
                return RedirectToAction("Create", new { id = id });
            }
            ocupacional ocupacional = db.ocupacional.Where(o => o.ocu_paciente == id && o.ocu_tipo == "actual").First();
            ocupacional.ocu_tiempo = get_tiempo(ocupacional.ocu_inicio);
            ViewBag.ocu_jornada = ocu_jornada(ocupacional.ocu_jornada);
            ViewBag.his_tipo = Session["his_tipo"];
            return PartialView(ocupacional);
        }


        //
        // POST: /Ocupacional/Edit/5

        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ocupacional ocupacional)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ocupacional).State = EntityState.Modified;
                db.SaveChanges();
                Session["pac_id"] = ocupacional.ocu_paciente;
                return RedirectToAction("Edit", "Laboral", new { id = ocupacional.ocu_id });
            }
            ViewBag.ocu_jornada = ocu_jornada(ocupacional.ocu_jornada);
            ViewBag.his_tipo = Session["his_tipo"];
            return PartialView(ocupacional);
        }

        //
        // GET: /Ocupacional/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Delete(int id)
        {
            ocupacional ocupacional = db.ocupacional.Find(id);
            db.ocupacional.Remove(ocupacional);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = ocupacional.ocu_paciente });
        }

        //
        // POST: /Ocupacional/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ocupacional ocupacional = db.ocupacional.Find(id);
            db.ocupacional.Remove(ocupacional);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private string get_estado(int his_id)
        {
            string estado = String.Empty;
            return estado;
        }
        private SelectList ocu_jornada(string jornada = "")
        {
            List<SelectListItem> list_jornada = new List<SelectListItem>();
            list_jornada.Add(new SelectListItem { Text = "Diurno", Value = "Diurno" });
            list_jornada.Add(new SelectListItem { Text = "Nocturno", Value = "Nocturno" });
            list_jornada.Add(new SelectListItem { Text = "Rotativo", Value = "Rotativo" });
            list_jornada.Add(new SelectListItem { Text = "Vespertino", Value = "Vespertino" });
            SelectList jornadas;
            if (jornada == "")
                jornadas = new SelectList(list_jornada, "Value", "Text");
            else
                jornadas = new SelectList(list_jornada, "Value", "Text", jornada);
            return jornadas;
        }

        private decimal get_tiempo(string inicio)
        {
            decimal tiempo = 0;
            string[] fec_ini = inicio.Split('/');
            DateTime da = DateTime.Now;
            DateTime dn = new DateTime(Convert.ToInt32(fec_ini[2]), Convert.ToInt32(fec_ini[1]), Convert.ToInt32(fec_ini[0]));
            tiempo = Convert.ToDecimal(da.Year - dn.Year);
            return tiempo;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}