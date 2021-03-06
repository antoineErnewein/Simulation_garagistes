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
    public class GaragisteService
    {
        private IGaragisteManager garagisteManager;
        private LogService logService = new LogService(new LogManager());
        private ReparationService reparationService = new ReparationService(new ReparationManager());
        private PeriodeFermetureService periodeFermetureService = new PeriodeFermetureService(new PeriodeFermetureManager());

        public GaragisteService(IGaragisteManager garagisteManager)
        {
            this.garagisteManager = garagisteManager;
        }

        //Renvoie un garagiste selon son ID
        public Garagiste getGaragisteById(int garagisteID)
        {
            return garagisteManager.getGaragisteById(garagisteID);
        }

        //Renvoie la liste des garagistes appartenant à une franchise
        public List<Garagiste> getGaragistesByFranchise(int franchiseID)
        {
            return garagisteManager.getGaragistesByFranchise(franchiseID);
        }

        //Renvoie la liste de tous les garagistes
        public List<Garagiste> getAllGaragistes()
        {
            return garagisteManager.getAllGaragistes();
        }

        //Update un garagiste
        public bool updateGaragiste(int garagisteID, string nom, int franchiseID)
        {
            Garagiste garagiste = garagisteManager.getGaragisteById(garagisteID);
            nom = nom.Trim();

            if (nom == "" || garagiste == null)
            {
                return false;
            }

            garagiste.Nom = nom;
            return garagisteManager.updateGaragiste(garagiste, franchiseID);
        }

        //Supprime un garagiste
        public bool deleteGaragiste(int garagisteID)
        {
            return garagisteManager.deleteGaragiste(garagisteID);
        }

        //Créer un garagiste 
        public int createGaragiste(string nom, int franchiseID)
        {
            Garagiste garagiste = new Garagiste();
            nom = nom.Trim();

            if (nom == "")
            {
                return -1;
            }

            garagiste.Nom = nom;
   
            logService.createLog("Création garagiste : " + nom + " franchiseID :" + franchiseID, DAL.Enums.LogType.Creations); 
            return garagisteManager.createGaragiste(garagiste, franchiseID);           
        }

        public int[] getTauxOccupation(DateTime jour)
        {
            List<Garagiste> garagistes = garagisteManager.getAllGaragistes();
            int[] res = new int[3];

            foreach (Garagiste g in garagistes)
            {
                res[0] += reparationService.getChargeHoraire(g.ID, jour);
                res[1] += (periodeFermetureService.isVacances(g.ID, jour)) ? 8 : 0;
            }

            res[2] = garagistes.Count * 8;

            return res;
        }

        public bool deleteAllGaragistes()
        {
            return garagisteManager.deleteAllGaragistes();
        }

        /*public bool EstDisponiblePourRevision(int garagisteID, int revisionID, DateTime jour)
        {
            Garagiste garagiste = garagisteManager.getGaragisteById(garagisteID);

        }*/
    }
}
