using System;
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
                        orderby r.Libelle
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
                        orderby r.Libelle
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
                        orderby r.Libelle
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
                revision.Modele = ((modeleID > 0) ?  dbService.ModeleJeu.Find(modeleID) : null);
                revision.Marque = ((marqueID > 0) ? dbService.MarqueJeu.Find(marqueID) : null);
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
                if(modeleID != -1)
                revision.Modele = dbService.ModeleJeu.Find(modeleID);

                if(marqueID != -1)
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

        public int createRevisionWithoutID(Revision revision)
        {
            try
            {
                dbService.RevisionJeu.Add(revision);
                dbService.SaveChanges();
                return revision.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public bool deleteAllRevisions()
        {
            try
            {
                List<Revision> revisions = (from r in dbService.RevisionJeu
                                            select r).ToList();

                foreach (Revision r in revisions)
                {
                    dbService.RevisionJeu.Remove(r);
                }

                dbService.SaveChanges();
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
