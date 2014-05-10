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
    public class ModeleController : Controller
    {
        private GarageEntities db = new GarageEntities();

        //
        // GET: /Modele/

        public ActionResult Index()
        {
            var modeles = db.ModeleJeu.Include(p => p.Marque);
            vmModele vmModele = new vmModele();
            vmModele.Modeles = modeles.ToList();
            vmModele.Marques = db.MarqueJeu.ToList();
            //return View(vmModele);
            return View(db.ModeleJeu.Include(p => p.Marque).ToList());
        }

        //
        // GET: /Modele/Details/5

        public ActionResult Details(int id = 0)
        {
            Modele modele = db.ModeleJeu.Find(id);
            if (modele == null)
            {
                return HttpNotFound();
            }
            return View(modele);
        }

        //
        // GET: /Modele/Create

        public ActionResult Create()
        {
            vmModele vmModele = new vmModele();
            vmModele.Marques = db.MarqueJeu.ToList();
            return View(vmModele);
        }

        //
        // POST: /Modele/Create

        [HttpPost]
        public ActionResult Create(vmModele vmModele)
        {
            int marqueId = int.Parse(Request["marqueId"]);
            string modeleName = Request["modeleName"];

            if (!String.IsNullOrEmpty(modeleName))
            {
                Modele modele = new Modele();
                modele.Nom = modeleName;
                modele.Marque = db.MarqueJeu.Find(marqueId);
                db.ModeleJeu.Add(modele);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                vmModele = new vmModele();
                vmModele.Marques = db.MarqueJeu.ToList();
                return View(vmModele);
            }

            //return View(modele);
        }

        //
        // GET: /Modele/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Modele modele = db.ModeleJeu.Find(id);
            vmModele vmModele = new vmModele();
            vmModele.Nom = modele.Nom;
            vmModele.Marque = modele.Marque;
            vmModele.ID = id;
            vmModele.Marques = db.MarqueJeu.ToList();

            return View(vmModele);
            //if (modele == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(modele);
        }

        //
        // POST: /Modele/Edit/5

        [HttpPost]
        public ActionResult Edit(Modele modele)
        {
            int marqueId = int.Parse(Request["marqueId"]);
            string modeleName = Request["modeleName"];
            int modeleID = int.Parse(Request["modeleId"]);

            if (!String.IsNullOrEmpty(modeleName))
            {
                modele = db.ModeleJeu.Find(modeleID); ;
                modele.Nom = modeleName;
                modele.Marque = db.MarqueJeu.Find(marqueId);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
            //if (ModelState.IsValid)
            //{
            //    db.Entry(modele).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(modele);
        }

        //
        // GET: /Modele/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Modele modele = db.ModeleJeu.Find(id);
            if (modele == null)
            {
                return HttpNotFound();
            }
            return View(modele);
        }

        //
        // POST: /Modele/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Modele modele = db.ModeleJeu.Find(id);
            db.ModeleJeu.Remove(modele);
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