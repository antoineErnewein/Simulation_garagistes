using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IManagers;
using DAL.Models;

namespace DAL.Managers
{
    public class ReparationManager : IReparationManager
    {
        private GarageEntities dbService;

        public ReparationManager()
        {
            dbService = new GarageEntities();
        }

        public Reparation getReparationById(int reparationID)
        {
            try
            {
                return (from r in dbService.ReparationJeu
                        where r.ID == reparationID
                        select r
                            ).First();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Reparation> getReparationsByVoiture(int voitureID)
        {
            try
            {
                return (from r in dbService.ReparationJeu
                        where r.Voiture.ID == voitureID 
                        select r).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Reparation> getReparationsByGaragiste(int garagisteID)
        {
            try
            {
                return (from r in dbService.ReparationJeu
                        where r.Garagiste.ID == garagisteID
                        select r).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Reparation> getReparationsByGaragisteAndJour(int garagisteID, DateTime jour)
        {
            try
            {
                return (from r in dbService.ReparationJeu
                        where r.Garagiste.ID == garagisteID && r.DateDebut.Date == jour.Date
                        select r).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Reparation> getReparationsByVoitureAndRevision(int voitureID, int revisionID)
        {
            try
            {
                return (from r in dbService.ReparationJeu
                        where r.Voiture.ID == voitureID && r.Revision.ID == revisionID
                        select r).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Reparation> getAllReparations()
        {
            try
            {
                return (from r in dbService.ReparationJeu
                        select r).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool updateReparation(Reparation reparation, int garagisteID, int voitureID, int revisionID)
        {
            try
            {
                reparation.Garagiste = dbService.GaragisteJeu.Find(garagisteID);
                reparation.Voiture = dbService.VoitureJeu.Find(voitureID);
                reparation.Revision = dbService.RevisionJeu.Find(revisionID);
                dbService.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool deleteReparation(int reparationID)
        {
            try
            {
                Reparation reparation = (from r in dbService.ReparationJeu
                             where r.ID == reparationID
                             select r).First();

                dbService.ReparationJeu.Remove(reparation);
                dbService.SaveChanges();
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public int createReparation(Reparation reparation, int garagisteID, int voitureID, int revisionID)
        {
            try
            {
                reparation.Garagiste = dbService.GaragisteJeu.Find(garagisteID);
                reparation.Voiture = dbService.VoitureJeu.Find(voitureID);
                reparation.Revision = dbService.RevisionJeu.Find(revisionID);
                dbService.ReparationJeu.Add(reparation);
                dbService.SaveChanges();
                return reparation.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
