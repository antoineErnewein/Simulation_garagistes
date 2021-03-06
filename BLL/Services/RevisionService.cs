﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IManagers;
using DAL.Managers;
using DAL.Models;

namespace BLL.Services
{
    public class RevisionService
    {
        private IRevisionManager revisionManager;
        private LogService logService = new LogService(new LogManager());

        public RevisionService(IRevisionManager revisionManager)
        {
            this.revisionManager = revisionManager;
        }

        public Revision getRevisionById(int revisionID)
        {
            return revisionManager.getRevisionById(revisionID);
        }

        public List<Revision> getRevisionsByMarque(int marqueID)
        {
            return revisionManager.getRevisionsByMarque(marqueID);
        }

        public List<Revision> getRevisionsByModele(int modeleID)
        {
            return revisionManager.getRevisionsByModele(modeleID);
        }

        public List<Revision> getAllRevisions()
        {
            return revisionManager.getAllRevisions();
        }

        public bool updateRevision(int revisionID, string libelle, int kilometrage, TimeSpan duree, int modeleID, int marqueID)
        {
            Revision revision = revisionManager.getRevisionById(revisionID);
            libelle = libelle.Trim();

            if (libelle == "")
            {
                return false;
            }

            revision.Libelle = libelle;
            revision.Kilometrage = kilometrage;
            revision.DureeIntervention = duree;

            return revisionManager.updateRevision(revision, modeleID, marqueID);
        }

        public bool deleteRevision(int revisionID)
        {
            return revisionManager.deleteRevision(revisionID);
        }

        public int createRevision(string libelle, int kilometrage, TimeSpan duree, int modeleID, int marqueID)
        {
            Revision revision = new Revision();
            libelle = libelle.Trim();

            if (libelle == "")
            {
                return -1;
            }

            revision.Libelle = libelle;
            revision.Kilometrage = kilometrage;
            revision.DureeIntervention = duree;

            logService.createLog("Création révision : Libelle : " + libelle + " Kilométrage : " + kilometrage + " Durée : " + duree + " ModeleID : " + modeleID + " MarqueID : " + marqueID, DAL.Enums.LogType.Creations);
            return revisionManager.createRevision(revision, modeleID, marqueID);
        }

        public int createRevisionWhithoutID(string libelle, int kilometrage, TimeSpan duree)
        {
            Revision revision = new Revision();
            libelle = libelle.Trim();

            if (libelle == "")
            {
                return -1;
            }

            revision.Libelle = libelle;
            revision.Kilometrage = kilometrage;
            revision.DureeIntervention = duree;

            return revisionManager.createRevisionWithoutID(revision);
        }

        public bool deleteAllRevisions()
        {
            return revisionManager.deleteAllRevisions();
        }
    }
}
