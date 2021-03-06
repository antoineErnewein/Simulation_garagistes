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
    public class MarqueController : Controller
    {
        private GarageEntities db = new GarageEntities();
        private MarqueService marqueService = new MarqueService(new MarqueManager());

        //
        // GET: /Marque/

        public ActionResult Index()
        {
            //List<Marque> marques;
            //if ((marques = marqueService.getAllMarques()) == null)
            //    marques = new List<Marque>();
            //return View(marques);
            return View(marqueService.getAllMarques());
        }

        //
        // GET: /Marque/Details/5

        public ActionResult Details(int id = 0)
        {
            Marque marque = marqueService.getMarqueById(id);
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
            if (System.Web.HttpContext.Current.User == null || !System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return HttpNotFound();

            return View();
        }

        //
        // POST: /Marque/Create

        [HttpPost]
        public ActionResult Create(Marque marque)
        {
            if (System.Web.HttpContext.Current.User == null || !System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return HttpNotFound();

            string marqueName = Request["marqueName"];
            marqueService.createMarque(marqueName);

            return RedirectToAction("Index");
        }

        //
        // GET: /Marque/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (System.Web.HttpContext.Current.User == null || !System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return HttpNotFound();

            Marque marque = marqueService.getMarqueById(id);
            if (marque == null)
            {
                return HttpNotFound();
            }
            return View(marque);
        }

        //
        // POST: /Marque/Edit/5

        [HttpPost]
        public ActionResult Edit(Marque marque)
        {
            if (System.Web.HttpContext.Current.User == null || !System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return HttpNotFound();

            string marqueName = Request["marqueName"];
            int marqueId = int.Parse(Request["marqueId"]);
            marqueService.updateMarque(marqueId,marqueName);

            return RedirectToAction("Edit/" + marqueId);
        }

        //
        // GET: /Marque/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (System.Web.HttpContext.Current.User == null || !System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return HttpNotFound();

            Marque marque = marqueService.getMarqueById(id);
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
            if (System.Web.HttpContext.Current.User == null || !System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return HttpNotFound();

            marqueService.deleteMarque(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}