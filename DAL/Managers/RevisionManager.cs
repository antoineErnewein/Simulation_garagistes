﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IManagers;
using DAL.Models;

namespace DAL.Managers
{
    public class RevisionManager : IRevisionManager
    {
        private GarageEntities dbService;

        public RevisionManager()
        {
            dbService = new GarageEntities();
        }

        public Revision getRevisionById(int revisionID)
        {
            try
            {
                return (from r in dbService.RevisionJeu
                        where r.ID == revisionID
                        select r
                            ).First();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Revision> getRevisionsByMarque(int marqueID)
        {
            try
            {
                return (from r in dbService.RevisionJeu
                        where r.Marque.ID == marqueID 
                        select r).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Revision> getRevisionsByModele(int modeleID)
        {
            try
            {
                return (from r in dbService.RevisionJeu
                        where r.Modele.ID == modeleID
                        select r).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Revision> getAllRevisions()
        {
            try
            {
                return (from r in dbService.RevisionJeu
                        select r).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool updateRevision(Revision revision, int modeleID, int marqueID)
        {
            try
            {
                revision.Modele = dbService.ModeleJeu.Find(modeleID);
                revision.Marque = dbService.MarqueJeu.Find(marqueID);
                dbService.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool deleteRevision(int revisionID)
        {
            try
            {
                Revision revision = (from r in dbService.RevisionJeu
                             where r.ID == revisionID
                             select r).First();

                dbService.RevisionJeu.Remove(revision);
                dbService.SaveChanges();
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public int createRevision(Revision revision, int modeleID, int marqueID)
        {
            try
            {
                revision.Modele = dbService.ModeleJeu.Find(modeleID);
                revision.Marque = dbService.MarqueJeu.Find(marqueID);
                dbService.RevisionJeu.Add(revision);
                dbService.SaveChanges();
                return revision.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}