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
    public class GaragisteController : Controller
    {
        private GarageEntities db = new GarageEntities();
        private GaragisteService garagisteService = new GaragisteService(new GaragisteManager());
        private FranchiseService franchiseService = new FranchiseService(new FranchiseManager());
        private PeriodeFermetureService periodeFermetureService = new PeriodeFermetureService(new PeriodeFermetureManager());
       
        //
        // GET: /Garagiste/

        public ActionResult Index()
        {
            return View(garagisteService.getAllGaragistes());
        }

        //
        // GET: /Garagiste/Details/5

        public ActionResult Details(int id = 0)
        {
            Garagiste garagiste = garagisteService.getGaragisteById(id);
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
            vmGaragiste.Franchises = franchiseService.getAllFranchises();
            return View(vmGaragiste);
        }

        //
        // POST: /Garagiste/Create

        [HttpPost]
        public ActionResult Create(vmGaragiste vmGaragiste)
        {
            int franchiseId = int.Parse(Request["franchiseId"]);
            string garagisteName = Request["garagisteName"];
            DateTime fermetureBegin; 
            DateTime fermetureEnd;

            if (!String.IsNullOrEmpty(garagisteName))
            {
                Garagiste garagiste = new Garagiste();
                garagiste.Nom = garagisteName;
                garagiste.Franchise = db.FranchiseJeu.Find(franchiseId);
                if (DateTime.TryParse(Request["fermetureBegin"], out fermetureBegin) && DateTime.TryParse(Request["fermetureEnd"], out fermetureEnd))
                {
                    PeriodeFermeture periodeFermeture = new PeriodeFermeture();
                    periodeFermeture.DateDebut = fermetureBegin;
                    periodeFermeture.DateFin = fermetureEnd;
                    db.PeriodeFermetureJeu.Add(periodeFermeture);
                    garagiste.PeriodeFermeture.Add(periodeFermeture);
                }
                db.GaragisteJeu.Add(garagiste);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                vmGaragiste = new vmGaragiste();
                vmGaragiste.Franchises = franchiseService.getAllFranchises();
                return View(vmGaragiste);
            }
        }

        //
        // GET: /Garagiste/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Garagiste garagiste = db.GaragisteJeu.Find(id);
           
            vmGaragiste vmGaragiste = new vmGaragiste();
            vmGaragiste.Nom = garagiste.Nom;
            vmGaragiste.Franchise = garagiste.Franchise;
            vmGaragiste.ID = id;
            vmGaragiste.Franchises = franchiseService.getAllFranchises();
            vmGaragiste.PeriodeFermetures = garagiste.PeriodeFermeture.ToList();

            return View(vmGaragiste);
        }

        //
        // POST: /Garagiste/Edit/5

        [HttpPost]
        public ActionResult Edit(vmGaragiste vmGaragiste)
        {
            int franchiseId = int.Parse(Request["franchiseId"]);
            string garagisteName = Request["garagisteName"];
            int garagisteId = int.Parse(Request["garagisteId"]);
            DateTime fermetureBegin;
            DateTime fermetureEnd;

            if (!String.IsNullOrEmpty(garagisteName))
            {
                if (DateTime.TryParse(Request["fermetureBegin"], out fermetureBegin) && DateTime.TryParse(Request["fermetureEnd"], out fermetureEnd))
                {
                    PeriodeFermeture periodeFermeture = new PeriodeFermeture();
                    periodeFermeture.DateDebut = fermetureBegin;
                    periodeFermeture.DateFin = fermetureEnd;
                    db.PeriodeFermetureJeu.Add(periodeFermeture);
                    garagisteService.getGaragisteById(garagisteId).PeriodeFermeture.Add(periodeFermeture);
                }
                garagisteService.updateGaragiste(garagisteId, garagisteName, franchiseId);
                return RedirectToAction("Edit/" + garagisteId);
            }
            return RedirectToAction("Edit/" + garagisteId);
            //if (ModelState.IsValid)
            //{
            //    db.Entry(garagiste).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(garagiste);
        }

        //
        // GET: /Garagiste/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Garagiste garagiste = garagisteService.getGaragisteById(id);
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
            garagisteService.deleteGaragiste(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult SupprimerPeriode(int id = 0)
        {
            PeriodeFermetureService periodeFermetureService = new PeriodeFermetureService(new PeriodeFermetureManager());
            periodeFermetureService.deletePeriodeFermeture(id);
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }
    }
}