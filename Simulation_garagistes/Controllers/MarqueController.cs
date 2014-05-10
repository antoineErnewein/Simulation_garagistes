using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;

namespace Simulation_garagistes.Controllers
{
    public class MarqueController : Controller
    {
        private GarageEntities db = new GarageEntities();

        //
        // GET: /Marque/

        public ActionResult Index()
        {
            return View(db.MarqueJeu.ToList());
        }

        //
        // GET: /Marque/Details/5

        public ActionResult Details(int id = 0)
        {
            Marque marque = db.MarqueJeu.Find(id);
            if (marque == null)
            {
                return HttpNotFound();
            }
            return View(marque);
        }

        //
        // GET: /Marque/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Marque/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Marque marque)
        {
            if (ModelState.IsValid)
            {
                db.MarqueJeu.Add(marque);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(marque);
        }

        //
        // GET: /Marque/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Marque marque = db.MarqueJeu.Find(id);
            if (marque == null)
            {
                return HttpNotFound();
            }
            return View(marque);
        }

        //
        // POST: /Marque/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Marque marque)
        {
            if (ModelState.IsValid)
            {
                db.Entry(marque).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(marque);
        }

        //
        // GET: /Marque/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Marque marque = db.MarqueJeu.Find(id);
            if (marque == null)
            {
                return HttpNotFound();
            }
            return View(marque);
        }

        //
        // POST: /Marque/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Marque marque = db.MarqueJeu.Find(id);
            db.MarqueJeu.Remove(marque);
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