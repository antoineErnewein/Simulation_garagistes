﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;

namespace Simulation_garagistes.Controllers
{
    public class RevisionController : Controller
    {
        private GarageEntities db = new GarageEntities();

        //
        // GET: /Revision/

        public ActionResult Index()
        {
            return View(db.RevisionJeu.ToList());
        }

        //
        // GET: /Revision/Details/5

        public ActionResult Details(int id = 0)
        {
            Revision revision = db.RevisionJeu.Find(id);
            if (revision == null)
            {
                return HttpNotFound();
            }
            return View(revision);
        }

        //
        // GET: /Revision/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Revision/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Revision revision)
        {
            if (ModelState.IsValid)
            {
                db.RevisionJeu.Add(revision);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(revision);
        }

        //
        // GET: /Revision/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Revision revision = db.RevisionJeu.Find(id);
            if (revision == null)
            {
                return HttpNotFound();
            }
            return View(revision);
        }

        //
        // POST: /Revision/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Revision revision)
        {
            if (ModelState.IsValid)
            {
                db.Entry(revision).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(revision);
        }

        //
        // GET: /Revision/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Revision revision = db.RevisionJeu.Find(id);
            if (revision == null)
            {
                return HttpNotFound();
            }
            return View(revision);
        }

        //
        // POST: /Revision/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Revision revision = db.RevisionJeu.Find(id);
            db.RevisionJeu.Remove(revision);
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