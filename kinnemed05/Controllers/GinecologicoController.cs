﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kinnemed05.Models;
using kinnemed05.Filters;
using kinnemed05.Security;

namespace kinnemed05.Controllers
{
    [InitializeSimpleMembership]
    //[CustomAuthorize(UserRoles.medico)]
    public class GinecologicoController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Ginecologico/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Index()
        {
            var ginecologico = db.ginecologico.Include(g => g.paciente);
            return View(ginecologico.ToList());
        }

        //
        // GET: /Ginecologico/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Details(int id = 0)
        {
            ginecologico ginecologico = db.ginecologico.Find(id);
            if (ginecologico == null)
            {
                return HttpNotFound();
            }
            return View(ginecologico);
        }

        //
        // GET: /Ginecologico/Create
        [CustomAuthorize(UserRoles.medico, UserRoles.doctor)]
        public ActionResult Create(int id)
        {
            ginecologico ginecologico = db.ginecologico.Find(id);
            if (ginecologico != null)
            {
                return RedirectToAction("Edit", new { id=id});
            }
            ViewBag.gin_id = id;
            ViewBag.gin_ciclos = gin_ciclo();
            return PartialView();
        }

        //
        // POST: /Ginecologico/Create
        [CustomAuthorize(UserRoles.medico, UserRoles.doctor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ginecologico ginecologico)
        {
            if (ModelState.IsValid)
            {
                db.ginecologico.Add(ginecologico);
                db.SaveChanges();
                return RedirectToAction("Create", "Habitos", new { id=ginecologico.gin_id});
            }

            ViewBag.gin_id = ginecologico.gin_id;
            ViewBag.gin_ciclos = gin_ciclo();
            return PartialView(ginecologico);
        }

        //
        // GET: /Ginecologico/Edit/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Edit(int id)
        {
            ginecologico ginecologico = db.ginecologico.Find(id);
            if (ginecologico == null)
            {
                paciente paciente = db.paciente.Find(id);
                if (paciente.pac_genero == "Femenino")
                {
                    return RedirectToAction("Create", new { id = id });
                }
                else {
                    return RedirectToAction("Edit","Personal", new { id = id });
                }
                
            }
            ViewBag.gin_id = ginecologico.gin_id;
            ViewBag.gin_ciclos = gin_ciclo();
            return PartialView(ginecologico);
        }

        //
        // POST: /Ginecologico/Edit/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ginecologico ginecologico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ginecologico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Habitos", new { id = ginecologico.gin_id });
            }
            ViewBag.gin_id = ginecologico.gin_id;
            ViewBag.gin_ciclos = gin_ciclo();
            return PartialView(ginecologico);
        }

        //
        // GET: /Ginecologico/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
        public ActionResult Delete(int id = 0)
        {
            ginecologico ginecologico = db.ginecologico.Find(id);
            if (ginecologico == null)
            {
                return HttpNotFound();
            }
            return View(ginecologico);
        }

        //
        // POST: /Ginecologico/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin, UserRoles.doctor)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ginecologico ginecologico = db.ginecologico.Find(id);
            db.ginecologico.Remove(ginecologico);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private SelectList gin_ciclo(string ciclo = "")
        {
            List<SelectListItem> list_ciclo = new List<SelectListItem>();
            list_ciclo.Add(new SelectListItem { Text = "REGULAR", Value = "REGULAR" });
            list_ciclo.Add(new SelectListItem { Text = "IRREGULAR", Value = "IRREGULAR" });
            list_ciclo.Add(new SelectListItem { Text = "MENOPAUSIA", Value = "MENOPAUSIA" });
            SelectList ciclos;
            if (ciclo == "")
                ciclos = new SelectList(list_ciclo, "Value", "Text");
            else
                ciclos = new SelectList(list_ciclo, "Value", "Text", ciclo);
            return ciclos;
        }
        
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}