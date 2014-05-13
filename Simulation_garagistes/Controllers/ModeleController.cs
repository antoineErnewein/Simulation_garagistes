using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using Simulation_garagistes.ViewModels;
using BLL.Services;
using DAL.Managers;

namespace Simulation_garagistes.Controllers
{
    public class ModeleController : Controller
    {
        private GarageEntities db = new GarageEntities();
        private ModeleService modeleService = new ModeleService(new ModeleManager());
        private MarqueService marqueService = new MarqueService(new MarqueManager());

        //
        // GET: /Modele/

        public ActionResult Index()
        {
            return View(modeleService.getAllModeles());
        }

        //
        // GET: /Modele/Details/5

        public ActionResult Details(int id = 0)
        {
            Modele modele = modeleService.getModeleById(id);
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
            vmModele.Marques = marqueService.getAllMarques();
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
                modeleService.createModele(modeleName, marqueId);
                return RedirectToAction("Index");
            }
            else
            {
                vmModele = new vmModele();
                vmModele.Marques = marqueService.getAllMarques();
                return View(vmModele);
            }

            //return View(modele);
        }

        //
        // GET: /Modele/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Modele modele = modeleService.getModeleById(id);
            vmModele vmModele = new vmModele();
            vmModele.Nom = modele.Nom;
            vmModele.Marque = modele.Marque;
            vmModele.ID = id;
            vmModele.Marques = marqueService.getAllMarques();

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
                modeleService.updateModele(modeleID, modeleName, marqueId);
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
            Modele modele = modeleService.getModeleById(id);
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
            modeleService.deleteModele(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}