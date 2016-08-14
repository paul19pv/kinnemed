using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kinnemed05.Models;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using kinnemed05.Filters;
using kinnemed05.Security;

namespace kinnemed05.Controllers
{
    [InitializeSimpleMembership]
    
    public class PruebaController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Prueba/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa, UserRoles.admin)]
        public ActionResult Index(int id)
        {
            var prueba = db.prueba.Include(p => p.registro).Include(p => p.examen).Where(p => p.pru_registro == id && p.examen.exa_estado == "ACTIVO");
            return PartialView(prueba.ToList());
        }

        //
        // GET: /Prueba/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa,UserRoles.admin)]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Details(int reg_id)
        {
            //var consulta = db.registro.Where(r => r.reg_paciente == paciente && r.reg_fecha == fecha);
            //int reg_id = 0;
            //if (!consulta.Any())
                //return RedirectToAction("Message", "Home", new { mensaje = "El paciente no tiene exámenes para esta fecha" });
            //reg_id = consulta.First().reg_id;
            List<prueba> resultado = db.prueba.Include(p => p.examen).Where(r => r.pru_registro == reg_id && r.examen.exa_tipo != "PLANTILLA").OrderBy(r => r.examen.exa_area).OrderBy(p => p.pru_examen).ToList();
            SetPrueba setprueba = new SetPrueba();
            setprueba.prueba = resultado;
            if(Request.IsAjaxRequest())
                return PartialView(setprueba);
            return View(setprueba);
        }
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.paciente, UserRoles.empresa)]
        public ActionResult Details(int paciente,string fecha)
        {
            var consulta = db.registro.Where(r => r.reg_paciente == paciente && r.reg_fecha == fecha);
            int reg_id = 0;
            if (!consulta.Any())
                return RedirectToAction("Message", "Home", new { mensaje = "El paciente no tiene exámenes para esta fecha" });
            reg_id = consulta.First().reg_id;
            List<prueba> resultado = db.prueba.Include(p => p.examen).Where(r => r.pru_registro == reg_id && r.examen.exa_tipo != "PLANTILLA").OrderBy(r => r.examen.exa_area).OrderBy(p => p.pru_examen).ToList();
            SetPrueba setprueba = new SetPrueba();
            setprueba.prueba = resultado;
            return PartialView(setprueba);
        }

        //
        // GET: /Prueba/Create
        [CustomAuthorize(UserRoles.laboratorista)]
        public ActionResult Create(int id)
        {
            ViewBag.reg_id = id;
            return PartialView();
        }

        //
        // POST: /Prueba/Create
        [CustomAuthorize(UserRoles.laboratorista)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(prueba prueba)
        {
            barcode barcode = new barcode();
            //prueba.pru_codigo = GetCodigo(prueba.pru_registro, prueba.pru_examen);
            //prueba.pru_imagen = barcode.GenerarCodigo(prueba.pru_codigo);

            if (ModelState.IsValid && ExistPrueba(prueba.pru_examen,prueba.pru_registro))
            {

                db.prueba.Add(prueba);
                db.SaveChanges();
                set_codigo(prueba.pru_registro, prueba.pru_examen);
                if (prueba.examen.exa_tipo == "PLANTILLA")
                {
                    CreatePrueba(prueba.pru_examen, prueba.pru_registro);
                }
                else {
                    CreateObservacion(prueba.pru_registro,prueba.pru_examen);
                }
            }
            //ModelState.AddModelError("","La información ingresada no es la correcta");
            //return RedirectToAction("Index", new { id = prueba.pru_registro });

            var list_prueba = db.prueba.Include(p => p.registro).Include(p => p.examen).Where(p => p.pru_registro == prueba.pru_registro && p.examen.exa_estado == "ACTIVO");
            return PartialView("Index",list_prueba.ToList());
        }

        //
        // GET: /Prueba/Edit/5
        [CustomAuthorize(UserRoles.laboratorista,UserRoles.admin)]
        public ActionResult Edit(int id = 0)
        {
            List<prueba> resultado = db.prueba.Where(r => r.pru_registro == id && r.examen.exa_tipo != "PLANTILLA").OrderBy(r => r.examen.exa_area).OrderBy(p => p.pru_examen).Include(p => p.examen).ToList();
            SetPrueba setprueba = new SetPrueba();
            setprueba.prueba = resultado;
            ViewBag.reg_id = id;
            return View(setprueba);
        }
        public ActionResult Edit01(int id = 0) {
            List<prueba> resultado = db.prueba.Where(r => r.pru_registro == id && r.examen.exa_tipo != "PLANTILLA").OrderBy(r => r.examen.exa_area).OrderBy(p => p.pru_examen).Include(p => p.examen).ToList();
            return View(resultado);
        }

        [HttpPost]
        public ActionResult Edit01(SetPrueba setprueba){

            List<prueba> resultado = db.prueba.Where(r => r.pru_registro == 584 && r.examen.exa_tipo != "PLANTILLA").OrderBy(r => r.examen.exa_area).OrderBy(p => p.pru_examen).Include(p => p.examen).ToList();
            SetPrueba setprueba1 = new SetPrueba();
            setprueba1.prueba = resultado;
            ViewBag.pru_id = "asd";
            return View("Lista", setprueba1);
        }

        //
        // POST: /Prueba/Edit/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(registro registro)
        {
            var consulta = db.registro.Where(r => r.reg_paciente == registro.reg_paciente && r.reg_fecha == registro.reg_fecha);
            int reg_id=0;
            if(!consulta.Any())
                return RedirectToAction("Message", "Home", new { mensaje = "El paciente no tiene exámenes para esta fecha" });
            reg_id = consulta.First().reg_id;    
            List<prueba> resultado = db.prueba.Include(p=>p.examen).Where(r => r.pru_registro == reg_id && r.examen.exa_tipo!="PLANTILLA").OrderBy(r=>r.examen.exa_area).OrderBy(p=>p.pru_examen).ToList();
            SetPrueba setprueba = new SetPrueba();
            setprueba.prueba = resultado;
            return PartialView(setprueba);
        }

        

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        public JsonResult Modificar(int id, string valor)
        {
            prueba prueba = db.prueba.Find(id);
            prueba.pru_resultado = valor;
            db.SaveChanges();
            return new JsonResult { Data = new { mensaje = "Datos Guardados" } };
        }
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        public JsonResult Valor(int id, string valor)
        {
            prueba prueba = db.prueba.Find(id);
            prueba.pru_valor = valor;
            prueba.pru_resultado = prueba.pru_resultado + valor;
            db.SaveChanges();
            return new JsonResult { Data = new { mensaje = "Datos Guardados" } };
        }

        //
        // GET: /Prueba/Delete/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        public ActionResult Delete(int id = 0)
        {
            prueba prueba = db.prueba.Find(id);
            if (prueba.examen.exa_tipo == "PLANTILLA") {
                DeleteGrupo(prueba.pru_examen, prueba.pru_registro);
            }
            db.prueba.Remove(prueba);
            db.SaveChanges();
            return RedirectToAction("Index", new { id=prueba.pru_registro});
        }

        private void DeleteGrupo(int exa_id, int reg_id)
        {
            examen examen = db.examen.Find(exa_id);
            int are_id = db.area.Where(a => a.are_nombre == examen.exa_nombre).First().are_id;
            List<examen> list_exa = new List<examen>();
            list_exa = db.examen.Where(e => e.exa_area == are_id).ToList();
                foreach (var item in list_exa)
                {
                    var consulta = db.prueba.Where(p => p.pru_examen == item.exa_id && p.pru_registro == reg_id);
                    if (consulta.Any()) {
                        prueba prueba = db.prueba.Where(p => p.pru_examen == item.exa_id && p.pru_registro == reg_id).First();
                        db.prueba.Remove(prueba);
                    }
                    
                }
                db.SaveChanges();
            
            
        }

        
        private void CreatePrueba(int exa_id, int reg_id) {
            examen examen = db.examen.Find(exa_id);
            int are_id = db.area.Where(a => a.are_nombre == examen.exa_nombre).First().are_id;
            List<examen> list_exa = new List<examen>();
            barcode barcode = new barcode();
            list_exa = db.examen.Where(e => e.exa_area == are_id).ToList();
            foreach (var item in list_exa) {
                prueba prueba = new prueba();
                prueba.pru_examen = item.exa_id;
                prueba.pru_registro = reg_id;
                prueba.pru_resultado = item.exa_inicial;
                //prueba.pru_codigo = GetCodigo(reg_id, item.exa_id);
                //prueba.pru_imagen = barcode.GenerarCodigo(prueba.pru_codigo);
                set_codigo(prueba.pru_registro, prueba.pru_examen);
                db.prueba.Add(prueba);
                //db.SaveChanges();
            }
            db.SaveChanges();
        }

        private void CreateObservacion(int reg_id, int exa_id) {
            try
            {
                int are_id = db.examen.Find(exa_id).exa_area;
                int obs_id = db.examen.Where(e => e.exa_area == are_id && e.exa_nombre.Contains("OBSERVACIONES")).First().exa_id;
                var consulta = db.prueba.Where(p => p.pru_examen == obs_id && p.pru_registro==reg_id);
                ModelState.AddModelError("error", obs_id+"");
                if (!consulta.Any())
                {
                    //ModelState.AddModelError("error", "if observacion");
                    prueba prueba = new prueba();
                    prueba.pru_examen = obs_id;
                    prueba.pru_registro = reg_id;
                    //prueba.pru_codigo = GetCodigo(reg_id, exa_id);
                    //prueba.pru_imagen = barcode.GenerarCodigo(prueba.pru_codigo);
                    db.prueba.Add(prueba);
                    db.SaveChanges();


                }
                
            }
            catch (Exception ex) {
                ModelState.AddModelError("error", ex.Message);
            }

            
        }

        private void set_codigo(int reg_id,int exa_id){
            barcode barcode = new barcode();
            
            var consulta = db.codigo.Where(c => c.cod_registro == reg_id && c.cod_area == exa_id);
            examen examen = db.examen.Find(exa_id);
            if (!consulta.Any()) {
                codigo codigo = new codigo();
                codigo.cod_codigo = GetCodigo(reg_id, examen.exa_area);
                codigo.cod_imagen = barcode.GenerarCodigo(codigo.cod_codigo);
                codigo.cod_registro = reg_id;
                codigo.cod_area = examen.exa_area;
                db.codigo.Add(codigo);
                db.SaveChanges();
            }
        }


        public string GetCodigo(int reg_id, int are_id)
        {
            string codigo = String.Empty;
            string dia = String.Empty;
            string mes = String.Empty;
            string num = String.Empty;
            DateTime dd = DateTime.Today;
            string date_now = dd.Date.ToString("d");
            int orden = db.registro.Where(r=>r.reg_id==reg_id).First().reg_orden;
            //int area = db.examen.Where(e => e.exa_id == exa_id).First().exa_area;
            int area = are_id;
            num = orden.ToString();
            dia = dd.Day.ToString();
            if (dia.Length == 1)
                dia = "0" + dia;
            mes = dd.Month.ToString();
            if (mes.Length == 1)
                mes = "0" + mes;
            string fecha = dia + mes;
            switch (num.Length)
            {
                case 1:
                    num = "000" + num;
                    break;
                case 2:
                    num = "00" + num;
                    break;
                case 3:
                    num = "0" + num;
                    break;
            }
            codigo += area.ToString() + fecha + num;
            return codigo;
        }

        public JsonResult AutocompleteExamen(string search)
        {
            search = search.ToUpper();
            var result = (from e in db.examen
                          where e.exa_nombre.ToUpper().Contains(search) && e.exa_estado =="ACTIVO"
                          select new { e.exa_id, e.exa_nombre }).Distinct();
            if (result.Count() == 0)
            {
                return new JsonResult() { Data = new { Data = new { exa_id = 0, exa_nombre = "Sin Datos" } } };
            }
            return new JsonResult() { Data = result };
        }

        public SelectList valores()
        {
            List<SelectListItem> list_valores = new List<SelectListItem>();
            list_valores.Add(new SelectListItem { Text = "POSITIVO", Value = "POSITIVO" });
            list_valores.Add(new SelectListItem { Text = "NEGATIVO", Value = "NEGATIVO" });
            SelectList valores = new SelectList(list_valores, "Value", "Text","NEGATIVO");
            return valores;
        }
        public SelectList valores(string defaul)
        {
            List<SelectListItem> list_valores = new List<SelectListItem>();
            list_valores.Add(new SelectListItem { Text = "", Value = "" });
            list_valores.Add(new SelectListItem { Text = "POSITIVO", Value = "POSITIVO" });
            list_valores.Add(new SelectListItem { Text = "NEGATIVO", Value = "NEGATIVO" });
            SelectList valores;
            if (String.IsNullOrEmpty(defaul))
                valores = new SelectList(list_valores, "Value", "Text");
            else
                valores = new SelectList(list_valores, "Value", "Text", defaul);
            return valores;
        }

        private bool ExistPrueba(int exa_id, int reg_id) {
            bool estado=true;
            var consulta = db.prueba.Where(p => p.pru_examen == exa_id && p.pru_registro == reg_id);
            if (consulta.Any()) {
                estado = false;
                ModelState.AddModelError("error", "El examen seleccionado ya esta registrado");
            }
                
            return estado;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}