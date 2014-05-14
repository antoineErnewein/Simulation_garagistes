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
    public class VoitureService
    {
        private IVoitureManager voitureManager;
        private LogService logService = new LogService(new LogManager());
        private ReparationService reparationService = new ReparationService(new ReparationManager());

        public VoitureService(IVoitureManager voitureManager)
        {
            this.voitureManager = voitureManager;
        }

        //Créer une voiture du modele voulu avec un kilométrage entre 20.000 et 200.000 
        public int createVoiture(int modeleID)
        {
            Random rand = new Random();
            Voiture voiture = new Voiture();
            voiture.Kilometrage = rand.Next(20000, 200000);

            logService.createLog("Création voiture : ModèleID : " + modeleID + " Kilométrage : " + voiture.Kilometrage);
            return voitureManager.createVoiture(voiture, modeleID);
        }

        public Voiture getVoitureById(int voitureID)
        {
            return voitureManager.getVoitureById(voitureID);
        }

        public List<Voiture> getVoituresByMarque(int marqueID)
        {
            return voitureManager.getVoituresByMarque(marqueID);
        }

        public List<Voiture> getVoituresByModele(int modeleID)
        {
            return voitureManager.getVoituresByModele(modeleID);
        }

        public List<Voiture> getAllVoitures()
        {
            return voitureManager.getAllVoitures();
        }

        public bool updateVoiture(int voitureID, double kilometrage, int modeleID)
        {
            Voiture voiture = voitureManager.getVoitureById(voitureID);

            if (voiture == null)
            {
                return false;
            }

            voiture.Kilometrage = kilometrage;

            return voitureManager.updateVoiture(voiture, modeleID);
        }

        public bool deleteVoiture(int voitureID)
        {
            return voitureManager.deleteVoiture(voitureID);
        }

        public List<Revision> rouler(int voitureID, DateTime jour)
        {
            Voiture voiture = voitureManager.getVoitureById(voitureID);
            Random rand = new Random();
            int km = 0;           
            List<Revision> revisions = getRevisionsAFaire(voitureID);

            if (revisions.Count == 0)
            {
                if (jour.DayOfWeek == DayOfWeek.Saturday || jour.DayOfWeek == DayOfWeek.Sunday)
                {
                    km = rand.Next(50, 100);
                    voiture.Kilometrage += km;
                    logService.createLog("Week-end (" + jour.DayOfWeek + "), la voiture (" + voitureID + ") [" + voiture.Kilometrage + "km] : roule " + km + "Km");
                }
                else
                {
                    km = rand.Next(20, 50);
                    voiture.Kilometrage += km;
                    logService.createLog("Semaine (" + jour.DayOfWeek + "), la voiture (" + voitureID + ") [" + voiture.Kilometrage + "km] : roule " + km + "Km");
                }

                voitureManager.updateVoiture(voiture, voiture.Modele.ID);

                revisions = null;
            }

            else
            {
                logService.createLog("La voiture : " + voitureID + " ne peut pas rouler, elle doit effectuer une révision");
            }

            return revisions;
        }

        public List<Revision> getRevisionsAFaire(int voitureID)
        {
            List<Revision> res = new List<Revision>();
            Voiture voiture = voitureManager.getVoitureById(voitureID);
            List<Revision> revisionsModele = new List<Revision>();
            List<Revision> revisionsMarque = new List<Revision>();
            List<Revision> revisions = new List<Revision>();
            Reparation reparation;

            if (voiture != null)
            {
                revisionsModele = (from r in voiture.Modele.Revision where r.Kilometrage <= voiture.Kilometrage select r).ToList();
                revisionsMarque = (from r in voiture.Modele.Marque.Revision where r.Kilometrage <= voiture.Kilometrage select r).ToList();

                revisions.AddRange(revisionsModele);
                revisions.AddRange(revisionsMarque);

                foreach (Revision r in revisions)
                {
                    //reparation = (from rep in r.Reparation where rep.Voiture.ID == voitureID select rep).First();
                    reparation = reparationService.getReparationByVoitureAndRevision(voitureID, r.ID);

                    if (reparation == null)
                    {
                        res.Add(r);
                    }
                }
            }

            return res;
        }

    }
}
