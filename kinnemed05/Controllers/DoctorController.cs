using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kinnemed05.Models;
using System.IO;

namespace kinnemed05.Controllers
{
    public class DoctorController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();
        private UsersContext db_user = new UsersContext();

        //
        // GET: /Doctor/

        public ActionResult Index(string sortOrder, string searchString, string especialidad)
        {
            //var doctor = db.doctor.Include(d => d.empresa).Include(d => d.especialidad);
            //return View(doctor.ToList());
            ViewBag.NombreSort = String.IsNullOrEmpty(sortOrder) ? "Nombre desc" : "";
            ViewBag.ApellidoSort = sortOrder == "Apellido" ? "Apellido desc" : "Apellido";
            ViewBag.SearchString = searchString;
            ViewBag.especialidad = new SelectList(db.especialidad, "esp_id", "esp_nombre");
            int cod_especialidad = 0;
            //ViewBag.Reporte = exportar;
            var doctor = db.doctor.Include(m => m.especialidad);
            doctor = doctor.Where(m => m.doc_estado != false);
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper();
                doctor = doctor.Where(s => s.doc_nombres.ToUpper().Contains(searchString) || s.doc_apellidos.ToUpper().Contains(searchString) || s.doc_cedula.Contains(searchString));

            }
            if (!String.IsNullOrEmpty(especialidad))
                cod_especialidad = Int32.Parse(especialidad);
            if (cod_especialidad != 0)
            {
                doctor = doctor.Where(s => s.doc_especialidad.Equals(cod_especialidad));
                //ViewBag.valor = especialidad;
            }
            switch (sortOrder)
            {
                case "Nombre desc":
                    doctor = doctor.OrderByDescending(s => s.doc_nombres);
                    break;
                case "Apellido":
                    doctor = doctor.OrderBy(s => s.doc_apellidos);
                    break;
                case "Apellido desc":
                    doctor = doctor.OrderByDescending(s => s.doc_apellidos);
                    break;
                default:
                    doctor = doctor.OrderBy(s => s.doc_nombres);
                    break;
            }
            
            return View(doctor.ToList());
        }

        //
        // GET: /Doctor/Details/5

        public ActionResult Details(int id = 0)
        {
            doctor doctor = db.doctor.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        //
        // GET: /Doctor/Create

        public ActionResult Create()
        {
            ViewBag.doc_empresa = new SelectList(db.empresa, "emp_id", "emp_cedula");
            ViewBag.doc_especialidad = new SelectList(db.especialidad, "esp_id", "esp_nombre");
            return View();
        }

        //
        // POST: /Doctor/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(doctor doctor)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                string fileName = Path.GetFileName(file.FileName);
                string ext = Path.GetExtension(fileName);
                string[] formatos = new string[] { ".jpg", ".jpeg", ".bmp", ".png", ".gif", ".JPG", ".JPEG", ".BMP", ".PNG", ".GIF" };
                if (!String.IsNullOrEmpty(fileName) && (Array.IndexOf(formatos, ext) >= 0))
                {
                    Firma objfirma = new Firma();
                    //doctor.doc_firma = fileName;
                    string path = Path.Combine(Server.MapPath("~/Content/firmas_"), fileName);
                    string path01 = Path.Combine(Server.MapPath("~/Content/firmas"), fileName);
                    file.SaveAs(path);
                    objfirma.ResizeImage(path, path01, 200, 120);
                    doctor.doc_firma = ConvertBytes(path01);
                }
                else
                {
                    if (!String.IsNullOrEmpty(ext))
                        if (Array.IndexOf(formatos, ext) <= 0)
                            ModelState.AddModelError("ext", "Extensión no Válida");
                }
            }

            if (ModelState.IsValid && IsUserExist(doctor.doc_cedula))
            {
                db.doctor.Add(doctor);
                db.SaveChanges();
                AccountController account = new AccountController();
                account.CreateUserProfile(doctor.doc_cedula, doctor.doc_cedula);
                UserManager userManager = new UserManager();
                int Userid = userManager.UserId(doctor.doc_cedula);
                UsersInRoles usersinroles = new UsersInRoles();
                usersinroles.RoleId = 7;
                usersinroles.UserId = Userid;
                account.CreateUsersInRole(usersinroles);
                return RedirectToAction("Index");
            }

            ViewBag.doc_empresa = new SelectList(db.empresa, "emp_id", "emp_cedula", doctor.doc_empresa);
            ViewBag.doc_especialidad = new SelectList(db.especialidad, "esp_id", "esp_nombre", doctor.doc_especialidad);
            return View(doctor);
        }

        //
        // GET: /Doctor/Edit/5

        public ActionResult Edit(int id = 0)
        {
            doctor doctor = db.doctor.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            ViewBag.doc_empresa = new SelectList(db.empresa, "emp_id", "emp_cedula", doctor.doc_empresa);
            ViewBag.doc_especialidad = new SelectList(db.especialidad, "esp_id", "esp_nombre", doctor.doc_especialidad);
            return View(doctor);
        }

        //
        // POST: /Doctor/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(doctor doctor)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                string fileName = Path.GetFileName(file.FileName);
                string ext = Path.GetExtension(fileName);
                string[] formatos = new string[] { ".jpg", ".jpeg", ".bmp", ".png", ".gif", ".JPG", ".JPEG", ".BMP", ".PNG", ".GIF" };
                if (!String.IsNullOrEmpty(fileName) && (Array.IndexOf(formatos, ext) >= 0))
                {
                    Firma objfirma = new Firma();
                    //doctor.doc_firma = fileName;
                    string path = Path.Combine(Server.MapPath("~/Content/firmas_"), fileName);
                    string path01 = Path.Combine(Server.MapPath("~/Content/firmas"), fileName);
                    file.SaveAs(path);

                    objfirma.ResizeImage(path, path01, 300, 260);
                    doctor.doc_firma = ConvertBytes(path01);

                }
                else
                {
                    if (!String.IsNullOrEmpty(ext))
                        if (Array.IndexOf(formatos, ext) <= 0)
                            ModelState.AddModelError("ext", "Extensión no Válida");
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.doc_empresa = new SelectList(db.empresa, "emp_id", "emp_cedula", doctor.doc_empresa);
            ViewBag.doc_especialidad = new SelectList(db.especialidad, "esp_id", "esp_nombre", doctor.doc_especialidad);
            return View(doctor);
        }

        //
        // GET: /Doctor/Delete/5

        public ActionResult Delete(int id = 0)
        {
            doctor doctor = db.doctor.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        //
        // POST: /Doctor/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            doctor doctor = db.doctor.Find(id);
            db.doctor.Remove(doctor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public Byte[] ConvertBytes(String ruta)
        {
            FileStream foto = new FileStream(ruta, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            Byte[] arreglo = new Byte[foto.Length];
            BinaryReader reader = new BinaryReader(foto);
            arreglo = reader.ReadBytes(Convert.ToInt32(foto.Length));
            return arreglo;
        }
        private bool IsUserExist(string usuario)
        {
            bool estado = true;
            var consulta = db_user.UserProfiles.Where(u => u.UserName == usuario);
            if (consulta.Any())
            {
                estado = false;
                ModelState.AddModelError("user", "El médico ya esta registrado con otro perfil por favor verifique la información");
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