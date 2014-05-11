using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using Simulation_garagistes.ViewModels;

namespace Simulation_garagistes.Controllers
{
    public class GaragisteController : Controller
    {
        private GarageEntities db = new GarageEntities();

        //
        // GET: /Garagiste/

        public ActionResult Index()
        {
            return View(db.GaragisteJeu.Include(p =>p.Franchise).Include(p=>p.PeriodeFermeture).ToList());
        }

        //
        // GET: /Garagiste/Details/5

        public ActionResult Details(int id = 0)
        {
            Garagiste garagiste = db.GaragisteJeu.Find(id);
            if (garagiste == null)
            {
                return HttpNotFound();
            }
            return View(garagiste);
        }

        //
        // GET: /Garagiste/Create

        public ActionResult Create()
        {
            vmGaragiste vmGaragiste = new vmGaragiste();
            vmGaragiste.Franchises = db.FranchiseJeu.ToList();
            return View(vmGaragiste);
        }

        //
        // POST: /Garagiste/Create

        [HttpPost]
        public ActionResult Create(vmGaragiste vmGaragiste)
        {
            int franchiseId = int.Parse(Request["franchiseId"]);
            string garagisteName = Request["garagisteName"];

            if (!String.IsNullOrEmpty(garagisteName))
            {
                Garagiste garagiste = new Garagiste();
                garagiste.Nom = garagisteName;
                garagiste.Franchise = db.FranchiseJeu.Find(franchiseId);
                db.GaragisteJeu.Add(garagiste);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                vmGaragiste = new vmGaragiste();
                vmGaragiste.Franchises = db.FranchiseJeu.ToList();
                return View(vmGaragiste);
            }
        }

        //
        // GET: /Garagiste/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Garagiste garagiste = db.GaragisteJeu.Find(id);
            if (garagiste == null)
            {
                return HttpNotFound();
            }
            return View(garagiste);
        }

        //
        // POST: /Garagiste/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Garagiste garagiste)
        {
            if (ModelState.IsValid)
            {
                db.Entry(garagiste).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(garagiste);
        }

        //
        // GET: /Garagiste/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Garagiste garagiste = db.GaragisteJeu.Find(id);
            if (garagiste == null)
            {
                return HttpNotFound();
            }
            return View(garagiste);
        }

        //
        // POST: /Garagiste/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Garagiste garagiste = db.GaragisteJeu.Find(id);
            db.GaragisteJeu.Remove(garagiste);
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