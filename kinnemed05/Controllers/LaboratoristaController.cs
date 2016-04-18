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
    [CustomAuthorize(UserRoles.admin)]
    public class LaboratoristaController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

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
            if (ModelState.IsValid)
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
            usermanager.DeleteUser(id, 3);
            db.laboratorista.Remove(laboratorista);
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