using kinnemed05.Security;
using kinnemed05.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace kinnemed05.Controllers
{
    //[CustomAuthorize(UserRoles.admin)]
    public class EmpresaController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Empresa/
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.admin)]
        public ActionResult Index()
        {
            try
            {
                return View(db.empresa.Where(e=>e.emp_estado!="INACTIVO").ToList());
            }
            catch (Exception ex) {
                return RedirectToAction("Message", "Home", new { mensaje = ex.Message });
            }
            
        }

        //
        // GET: /Empresa/Details/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.medico, UserRoles.admin)]
        public ActionResult Details(int id = 0)
        {
            empresa empresa = db.empresa.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        //
        // GET: /Empresa/Create
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Empresa/Create
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(empresa empresa)
        {
            if (ModelState.IsValid)
            {
                db.empresa.Add(empresa);
                db.SaveChanges();
                //crear los valores de usuario
                AccountController account = new AccountController();
                account.CreateUserProfile(empresa.emp_cedula, empresa.emp_cedula);
                UserManager userManager = new UserManager();
                int Userid = userManager.UpdateEmpresa(empresa.emp_cedula, empresa.emp_id);
                UsersInRoles usersinroles = new UsersInRoles();
                usersinroles.RoleId = 4;
                usersinroles.UserId = Userid;
                account.CreateUsersInRole(usersinroles);
                return RedirectToAction("Index");
            }

            return View(empresa);
        }

        //
        // GET: /Empresa/Edit/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        public ActionResult Edit(int id = 0)
        {
            empresa empresa = db.empresa.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        //
        // POST: /Empresa/Edit/5
        [CustomAuthorize(UserRoles.laboratorista, UserRoles.admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(empresa empresa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empresa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(empresa);
        }

        //
        // GET: /Empresa/Delete/5
        [CustomAuthorize(UserRoles.admin)]
        public ActionResult Delete(int id = 0)
        {
            empresa empresa = db.empresa.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        //
        // POST: /Empresa/Delete/5
        [CustomAuthorize(UserRoles.admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            empresa empresa = db.empresa.Find(id);
            db.empresa.Remove(empresa);
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