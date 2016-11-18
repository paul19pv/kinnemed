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
using System.IO;

namespace kinnemed05.Controllers
{
    [InitializeSimpleMembership]
    [CustomAuthorize(UserRoles.admin)]
    public class LaboratoristaController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();
        private UsersContext db_user = new UsersContext();
        //
        // GET: /Laboratorista/

        public ActionResult Index()
        {
            return View(db.laboratorista.ToList());
        }

        //
        // GET: /Laboratorista/Details/5

        public ActionResult Details(int id = 0)
        {
            laboratorista laboratorista = db.laboratorista.Find(id);
            if (laboratorista == null)
            {
                return HttpNotFound();
            }
            return View(laboratorista);
        }

        //
        // GET: /Laboratorista/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Laboratorista/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(laboratorista laboratorista)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                string fileName = Path.GetFileName(file.FileName);
                string ext = Path.GetExtension(fileName);
                string[] formatos = new string[] { ".jpg", ".jpeg", ".bmp", ".png", ".gif", ".JPG", ".JPEG", ".BMP", ".PNG", ".GIF" };
                if (!String.IsNullOrEmpty(fileName) && (Array.IndexOf(formatos, ext) > 0))
                {
                    Firma objfirma = new Firma();
                    //laboratorista.lab_firma = fileName;
                    string path = Path.Combine(Server.MapPath("~/Content/firmas_"), fileName);
                    string path01 = Path.Combine(Server.MapPath("~/Content/firmas"), fileName);
                    file.SaveAs(path);
                    objfirma.ResizeImage(path, path01, 200, 120);
                    laboratorista.lab_firma = ConvertBytes(path01);
                }
                else
                {
                    if (!String.IsNullOrEmpty(ext))
                        if (Array.IndexOf(formatos, ext) <= 0)
                            ModelState.AddModelError("ext", "Extensión no Válida");
                }
            }
            if (ModelState.IsValid && IsUserExist(laboratorista.lab_cedula))
            {
                db.laboratorista.Add(laboratorista);
                db.SaveChanges();

                AccountController account = new AccountController();
                account.CreateUserProfile(laboratorista.lab_cedula, laboratorista.lab_cedula);
                UserManager userManager = new UserManager();
                int Userid = userManager.UpdateLaboratorista(laboratorista.lab_cedula, laboratorista.lab_id);
                UsersInRoles usersinroles = new UsersInRoles();
                usersinroles.RoleId = 5;
                usersinroles.UserId = Userid;
                account.CreateUsersInRole(usersinroles);

                return RedirectToAction("Index");
            }

            return View(laboratorista);
        }

        //
        // GET: /Laboratorista/Edit/5

        public ActionResult Edit(int id = 0)
        {
            laboratorista laboratorista = db.laboratorista.Find(id);
            if (laboratorista == null)
            {
                return HttpNotFound();
            }
            return View(laboratorista);
        }

        //
        // POST: /Laboratorista/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(laboratorista laboratorista)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                string fileName = Path.GetFileName(file.FileName);
                string ext = Path.GetExtension(fileName);
                string[] formatos = new string[] { ".jpg", ".jpeg", ".bmp", ".png", ".gif", ".JPG", ".JPEG", ".BMP", ".PNG", ".GIF" };
                if (!String.IsNullOrEmpty(fileName) && (Array.IndexOf(formatos, ext) > 0))
                {
                    Firma objfirma = new Firma();
                    //laboratorista.lab_firma = fileName;
                    string path = Path.Combine(Server.MapPath("~/Content/firmas_"), fileName);
                    string path01 = Path.Combine(Server.MapPath("~/Content/firmas"), fileName);
                    file.SaveAs(path);
                    //objfirma.ResizeImage(path, path01, 195, 130);
                    laboratorista.lab_firma = ConvertBytes(path);
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
                db.Entry(laboratorista).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(laboratorista);
        }

        //
        // GET: /Laboratorista/Delete/5

        public ActionResult Delete(int id = 0)
        {
            laboratorista laboratorista = db.laboratorista.Find(id);
            if (laboratorista == null)
            {
                return HttpNotFound();
            }
            return View(laboratorista);
        }

        //
        // POST: /Laboratorista/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            laboratorista laboratorista = db.laboratorista.Find(id);
            UserManager usermanager = new UserManager();
            usermanager.DeleteUser(id, 5);
            db.laboratorista.Remove(laboratorista);
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
                ModelState.AddModelError("user", "El técnico ya esta registrado con otro perfil por favor verifique la información");
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