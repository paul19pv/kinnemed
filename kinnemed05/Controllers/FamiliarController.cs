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
    public class FamiliarController : Controller
    {
        private bd_kinnemed02Entities db = new bd_kinnemed02Entities();

        //
        // GET: /Familiar/

        public ActionResult Index()
        {
            var familiar = db.familiar.Include(f => f.paciente);
            return View(familiar.ToList());
        }

        //
        // GET: /Familiar/Details/5

        public ActionResult Details(int id = 0)
        {
            familiar familiar = db.familiar.Find(id);
            if (familiar == null)
            {
                return HttpNotFound();
            }
            return PartialView(familiar);
        }

        //
        // GET: /Familiar/Create

        public ActionResult Create(int id)
        {
            familiar familiar = db.familiar.Find(id);
            if (familiar != null)
            {
                return RedirectToAction("Edit", new { id = id });
            }
            ViewBag.fam_id = id;
            return PartialView();
        }

        //
        // POST: /Familiar/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(familiar familiar)
        {
            if (ModelState.IsValid)
            {
                db.familiar.Add(familiar);
                db.SaveChanges();
                return RedirectToAction("Problema", "Historia", new { id=Session["his_id"] });
            }
            ViewBag.valor = familiar.fam_cardiopatia;
            ViewBag.fam_id = familiar.fam_id;
            return PartialView(familiar);
        }

        //
        // GET: /Familiar/Edit/5

        public ActionResult Edit(int id)
        {
            familiar familiar = db.familiar.Find(id);
            if (familiar == null)
            {
                return RedirectToAction("Create", new { id = id });
            }
            ViewBag.fam_id = id;
            return PartialView(familiar);
        }

        //
        // POST: /Familiar/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(familiar familiar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(familiar).State = EntityState.Modified;
                db.SaveChanges();
                int his_tipo = Convert.ToInt32(Session["his_tipo"]);
                if(his_tipo==1)
                    return RedirectToAction("Problema", "Historia", new { id = Session["his_id"] });
                else
                    return RedirectToAction("Edit", "Revision", new { id = Session["his_id"] });
            }
            ViewBag.fam_id = familiar.fam_id;
            return PartialView(familiar);
        }

        //
        // GET: /Familiar/Delete/5

        public ActionResult Delete(int id = 0)
        {
            familiar familiar = db.familiar.Find(id);
            if (familiar == null)
            {
                return HttpNotFound();
            }
            return View(familiar);
        }

        //
        // POST: /Familiar/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            familiar familiar = db.familiar.Find(id);
            db.familiar.Remove(familiar);
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