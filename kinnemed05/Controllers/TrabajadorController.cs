using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kinnemed05.Models;

namespace kinnemed05.Controllers
{
    public class TrabajadorController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Trabajador/

        public ActionResult Index()
        {
            return View(db.trabajador.ToList());
        }

        //
        // GET: /Trabajador/Details/5

        public ActionResult Details(int id = 0)
        {
            trabajador trabajador = db.trabajador.Find(id);
            if (trabajador == null)
            {
                return HttpNotFound();
            }
            return View(trabajador);
        }

        //
        // GET: /Trabajador/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Trabajador/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(trabajador trabajador)
        {
            if (ModelState.IsValid)
            {
                db.trabajador.Add(trabajador);
                db.SaveChanges();

                AccountController account = new AccountController();
                account.CreateUserProfile(trabajador.tra_cedula, trabajador.tra_cedula);
                UserManager userManager = new UserManager();
                int Userid = userManager.UpdateTrabajador(trabajador.tra_cedula, trabajador.tra_id);
                UsersInRoles usersinroles = new UsersInRoles();
                usersinroles.RoleId = 6;
                usersinroles.UserId = Userid;
                account.CreateUsersInRole(usersinroles);

                return RedirectToAction("Index");
            }

            return View(trabajador);
        }

        //
        // GET: /Trabajador/Edit/5

        public ActionResult Edit(int id = 0)
        {
            trabajador trabajador = db.trabajador.Find(id);
            if (trabajador == null)
            {
                return HttpNotFound();
            }
            return View(trabajador);
        }

        //
        // POST: /Trabajador/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(trabajador trabajador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trabajador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trabajador);
        }

        //
        // GET: /Trabajador/Delete/5

        public ActionResult Delete(int id = 0)
        {
            trabajador trabajador = db.trabajador.Find(id);
            if (trabajador == null)
            {
                return HttpNotFound();
            }
            return View(trabajador);
        }

        //
        // POST: /Trabajador/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            trabajador trabajador = db.trabajador.Find(id);
            UserManager usermanager = new UserManager();
            usermanager.DeleteUser(id, 6);
            db.trabajador.Remove(trabajador);
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