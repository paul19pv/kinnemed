using System;
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
    public class LaboralController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Laboral/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Index()
        {
            var laboral = db.laboral.Include(l => l.ocupacional).Include(l => l.riesgo);
            return View(laboral.ToList());
        }

        //
        // GET: /Laboral/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Details(int id)
        {
            var consulta = db.ocupacional.Where(o => o.ocu_paciente == id && o.ocu_tipo == "actual");
            if (!consulta.Any())
            {
                return HttpNotFound();
            }
            ocupacional ocupacional = consulta.First();
            List<laboral> biologicos = db.laboral.Include(l => l.riesgo).Where(l => l.lab_ocupacional == ocupacional.ocu_id && l.riesgo.rie_grupo == "Biológicos").ToList();
            List<laboral> biomecanicos = db.laboral.Include(l => l.riesgo).Where(l => l.lab_ocupacional == ocupacional.ocu_id && l.riesgo.rie_grupo == "Biomecánicas").ToList();
            List<laboral> fisico = db.laboral.Include(l => l.riesgo).Where(l => l.lab_ocupacional == ocupacional.ocu_id && l.riesgo.rie_grupo == "Físico").ToList();
            List<laboral> mecanicos = db.laboral.Include(l => l.riesgo).Where(l => l.lab_ocupacional == ocupacional.ocu_id && l.riesgo.rie_grupo == "Mecánicos").ToList();
            List<laboral> psicosociales = db.laboral.Include(l => l.riesgo).Where(l => l.lab_ocupacional == ocupacional.ocu_id && l.riesgo.rie_grupo == "Psicosociales").ToList();
            List<laboral> quimicos = db.laboral.Include(l => l.riesgo).Where(l => l.lab_ocupacional == ocupacional.ocu_id && l.riesgo.rie_grupo == "Químicos").ToList();
            SetLaboral setlaboral = new SetLaboral();
            setlaboral.biologicos = biologicos;
            setlaboral.biomecanicos = biomecanicos;
            setlaboral.fisico = fisico;
            setlaboral.mecanicos = mecanicos;
            setlaboral.psicosociales = psicosociales;
            setlaboral.quimicos = quimicos;
            return PartialView(setlaboral);
        }

        //
        // GET: /Laboral/Create

        public void Create(int id)
        {

            //laboral laboral = db.laboral.Find(id);

            List<riesgo> list_riesgo = db.riesgo.ToList();

            foreach (var riesgo in list_riesgo)
            {
                laboral laboral = new laboral();
                laboral.lab_ocupacional = id;
                laboral.lab_riesgo = riesgo.rie_id;
                laboral.lab_estado = false;
                db.laboral.Add(laboral);
                db.SaveChanges();
            }


            //return RedirectToAction("Edit", new { id=id});
            //return PartialView();
        }

        //
        // POST: /Laboral/Create

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(laboral laboral)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.laboral.Add(laboral);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.lab_ocupacional = new SelectList(db.ocupacional, "ocu_id", "ocu_empresa", laboral.lab_ocupacional);
        //    ViewBag.lab_riesgo = new SelectList(db.riesgo, "rie_id", "rie_grupo", laboral.lab_riesgo);
        //    return View(laboral);
        //}

        //
        // GET: /Laboral/Edit/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin)]
        public ActionResult Edit(int id = 0)
        {
            var consulta = db.laboral.Where(l => l.lab_ocupacional == id);
            //laboral laboral = db.laboral.Find(id);
            if (!consulta.Any())
            {
                Create(id);
            }
            
            List<laboral> biologicos = db.laboral.Include(l => l.riesgo).Where(l => l.lab_ocupacional == id && l.riesgo.rie_grupo=="Biológicos").ToList();
            List<laboral> biomecanicos = db.laboral.Include(l => l.riesgo).Where(l => l.lab_ocupacional == id && l.riesgo.rie_grupo == "Biomecánicas").ToList();
            List<laboral> fisico = db.laboral.Include(l => l.riesgo).Where(l => l.lab_ocupacional == id && l.riesgo.rie_grupo == "Físico").ToList();
            List<laboral> mecanicos = db.laboral.Include(l => l.riesgo).Where(l => l.lab_ocupacional == id && l.riesgo.rie_grupo == "Mecánicos").ToList();
            List<laboral> psicosociales = db.laboral.Include(l => l.riesgo).Where(l => l.lab_ocupacional == id && l.riesgo.rie_grupo == "Psicosociales").ToList();
            List<laboral> quimicos = db.laboral.Include(l => l.riesgo).Where(l => l.lab_ocupacional == id && l.riesgo.rie_grupo == "Químicos").ToList();
            SetLaboral setlaboral = new SetLaboral();
            setlaboral.biologicos = biologicos;
            setlaboral.biomecanicos = biomecanicos;
            setlaboral.fisico = fisico;
            setlaboral.mecanicos = mecanicos;
            setlaboral.psicosociales = psicosociales;
            setlaboral.quimicos = quimicos;
            ViewBag.lab_ocupacional = id;
            return PartialView(setlaboral);
        }

        //
        // POST: /Laboral/Edit/5
        //[CustomAuthorize(UserRoles.medico, UserRoles.admin)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Edit(int id,bool estado)
        {
            laboral laboral = db.laboral.Find(id);
            laboral.lab_estado = estado;
            if (ModelState.IsValid)
            {
                db.Entry(laboral).State = EntityState.Modified;
                db.SaveChanges();
            }
            return new JsonResult { Data = new { mensaje = "Datos Guardados" } };
        }

        //
        // GET: /Laboral/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin)]
        public ActionResult Delete(int id = 0)
        {
            laboral laboral = db.laboral.Find(id);
            if (laboral == null)
            {
                return HttpNotFound();
            }
            return View(laboral);
        }

        //
        // POST: /Laboral/Delete/5
        [CustomAuthorize(UserRoles.medico, UserRoles.admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            laboral laboral = db.laboral.Find(id);
            db.laboral.Remove(laboral);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}