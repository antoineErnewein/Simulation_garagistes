using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IManagers;
using DAL.Managers;
using DAL.Models;

namespace BLL.Services
{
    public class ReparationService
    {
        private IReparationManager reparationManager;
        private LogService logService = new LogService(new LogManager());

        public ReparationService(IReparationManager reparationManager)
        {
            this.reparationManager = reparationManager;
        }

        public Reparation getReparationById(int reparationID)
        {
            return reparationManager.getReparationById(reparationID);
        }

        public List<Reparation> getReparationsByVoiture(int voitureID)
        {
            return reparationManager.getReparationsByVoiture(voitureID);
        }

        public List<Reparation> getReparationsByGaragiste(int garagisteID)
        {
            return reparationManager.getReparationsByGaragiste(garagisteID);
        }

        public List<Reparation> getReparationsByGaragisteAndJour(int garagisteID, DateTime jour)
        {
            return reparationManager.getReparationsByGaragisteAndJour(garagisteID, jour);
        }

        public Reparation getReparationByVoitureAndRevision(int voitureID, int revisionID)
        {
            return reparationManager.getReparationByVoitureAndRevision(voitureID, revisionID);
        }

        public List<Reparation> getAllReparations()
        {
            return reparationManager.getAllReparations();
        }

        public bool updateReparation(int reparationID, DateTime dateDebut, DateTime dateFin, int garagisteID, int voitureID, int revisionID)
        {
            Reparation reparation = reparationManager.getReparationById(reparationID);
            
            if (dateDebut.Date > dateFin.Date)
            {
                return false;
            }

            reparation.DateDebut = dateDebut;
            reparation.DateFin = dateFin;

            return reparationManager.updateReparation(reparation, garagisteID, voitureID, revisionID);
        }

        public bool deleteReparation(int reparationID)
        {
            return reparationManager.deleteReparation(reparationID);
        }

        public int createReparation(DateTime dateDebut, DateTime dateFin, int garagisteID, int voitureID, int revisionID)
        {
            Reparation reparation = new Reparation();

            if (dateDebut.Date > dateFin.Date)
            {
                return -1;
            }

            reparation.DateDebut = dateDebut;
            reparation.DateFin = dateFin;

            logService.createLog("Création réparation : Début : " + dateDebut.ToShortDateString() + " Fin : " + dateFin.ToShortDateString() + " GaragisteID : " + garagisteID + " VoitureID : " + voitureID + "RevisionID : " + revisionID);
            return reparationManager.createReparation(reparation, garagisteID, voitureID, revisionID);
        }

        //Renvoie la charge horaire d'un garagiste pour un certain jour
        public int getChargeHoraire(int garageID, DateTime jour)
        {
            List<Reparation> reparations = reparationManager.getReparationsByGaragisteAndJour(garageID, jour);
            int charge = 0;

            foreach (Reparation rep in reparations)
            {
                charge += rep.Revision.DureeIntervention.Hours;
            }

            return charge;
        }
    }
}
