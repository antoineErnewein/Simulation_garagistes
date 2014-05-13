using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using BLL.Services;
using DAL.Managers;

namespace Simulation_garagistes.Controllers
{
    public class FranchiseController : Controller
    {
        private GarageEntities db = new GarageEntities();
        private FranchiseService franchiseService = new FranchiseService(new FranchiseManager());

        //
        // GET: /Franchise/

        public ActionResult Index()
        {
            return View(franchiseService.getAllFranchises());
        }

        //
        // GET: /Franchise/Details/5

        public ActionResult Details(int id = 0)
        {
            Franchise franchise = franchiseService.getFranchiseById(id);
            if (franchise == null)
            {
                return HttpNotFound();
            }
            return View(franchise);
        }

        //
        // GET: /Franchise/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Franchise/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Franchise franchise)
        {
            if (ModelState.IsValid)
            {
                db.FranchiseJeu.Add(franchise);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(franchise);
        }

        //
        // GET: /Franchise/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Franchise franchise = franchiseService.getFranchiseById(id);
            if (franchise == null)
            {
                return HttpNotFound();
            }
            return View(franchise);
        }

        //
        // POST: /Franchise/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Franchise franchise)
        {
            if (ModelState.IsValid)
            {
                db.Entry(franchise).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(franchise);
        }

        //
        // GET: /Franchise/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Franchise franchise = franchiseService.getFranchiseById(id);
            if (franchise == null)
            {
                return HttpNotFound();
            }
            return View(franchise);
        }

        //
        // POST: /Franchise/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            franchiseService.deleteFranchise(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}