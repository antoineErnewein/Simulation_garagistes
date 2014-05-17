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
using Simulation_garagistes.ViewModels;

namespace Simulation_garagistes.Controllers
{
    public class RevisionController : Controller
    {
        private GarageEntities db = new GarageEntities();
        private RevisionService revisionService = new RevisionService(new RevisionManager());
        private ModeleService modeleService = new ModeleService(new ModeleManager());
        private MarqueService marqueService = new MarqueService(new MarqueManager());

        //
        // GET: /Revision/

        public ActionResult Index()
        {
            return View(revisionService.getAllRevisions());
        }

        //
        // GET: /Revision/Details/5

        public ActionResult Details(int id = 0)
        {
            Revision revision = revisionService.getRevisionById(id);
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
            vmRevision vmRevision = new vmRevision();
            vmRevision.Marques = marqueService.getAllMarques();
            vmRevision.Modeles = modeleService.getAllModeles();

            return View(vmRevision);
        }

        //
        // POST: /Revision/Create

        [HttpPost]
        public ActionResult Create(Revision revision)
        {
            string revisionLibelle = Request["revisionName"];
            int revisionKm = int.Parse(Request["revisionKm"]);
            TimeSpan dureeIntervention = TimeSpan.Parse(Request["revisionDuree"]);
            int marqueId = int.Parse(Request["marqueId"]);
            int modeleId = int.Parse(Request["modeleId"]);
            //TODO Cas NULL marchant en pas à pas
            revisionService.createRevision(revisionLibelle, revisionKm, dureeIntervention, modeleId, marqueId);

            return RedirectToAction("Index");
        }

        //
        // GET: /Revision/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Revision revision = revisionService.getRevisionById(id);
            if (revision == null)
            {
                return HttpNotFound();
            }
            vmRevision vmRevision = new vmRevision();
            vmRevision.ID = revision.ID;
            vmRevision.Kilometrage = revision.Kilometrage;
            vmRevision.Libelle = revision.Libelle;
            vmRevision.Marque = revision.Marque;
            vmRevision.Marques = marqueService.getAllMarques();
            vmRevision.DureeIntervention = revision.DureeIntervention ;
            vmRevision.Modele = revision.Modele ;
            vmRevision.Modeles = modeleService.getAllModeles();

            return View(vmRevision);
        }

        //
        // POST: /Revision/Edit/5

        [HttpPost]
        public ActionResult Edit(Revision revision)
        {
            int revisionId = int.Parse(Request["revisionId"]);
            string revisionLibelle = Request["revisionName"];
            int revisionKm = int.Parse(Request["revisionKm"]);
            TimeSpan dureeIntervention = TimeSpan.Parse(Request["revisionDuree"]);
            int marqueId = int.Parse(Request["marqueId"]);
            int modeleId = int.Parse(Request["modeleId"]);
            revision = revisionService.getRevisionById(revisionId);
            //TODO Cas NULL marchant en pas à pas
            revisionService.updateRevision(revisionId, revisionLibelle, revisionKm, dureeIntervention, modeleId, marqueId);
            
            return RedirectToAction("Edit/"+revisionId);
        }

        //
        // GET: /Revision/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Revision revision = revisionService.getRevisionById(id);
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
            revisionService.deleteRevision(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}