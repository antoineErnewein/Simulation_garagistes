﻿using System;
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
    public class VoitureController : Controller
    {
        private GarageEntities db = new GarageEntities();
        private VoitureService voitureService = new VoitureService(new VoitureManager());

        //
        // GET: /Voiture/

        public ActionResult Index()
        {
            return View(voitureService.getAllVoitures());
        }

        //
        // GET: /Voiture/Details/5

        public ActionResult Details(int id = 0)
        {
            Voiture voiture = voitureService.getVoitureById(id);
            if (voiture == null)
            {
                return HttpNotFound();
            }
            return View(voiture);
        }

        //
        // GET: /Voiture/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Voiture/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Voiture voiture)
        {
            if (ModelState.IsValid)
            {
                db.VoitureJeu.Add(voiture);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(voiture);
        }

        //
        // GET: /Voiture/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Voiture voiture = voitureService.getVoitureById(id);
            if (voiture == null)
            {
                return HttpNotFound();
            }
            return View(voiture);
        }

        //
        // POST: /Voiture/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Voiture voiture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(voiture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(voiture);
        }

        //
        // GET: /Voiture/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Voiture voiture = voitureService.getVoitureById(id);
            if (voiture == null)
            {
                return HttpNotFound();
            }
            return View(voiture);
        }

        //
        // POST: /Voiture/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            voitureService.deleteVoiture(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}