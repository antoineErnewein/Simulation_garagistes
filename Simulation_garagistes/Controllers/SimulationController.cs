﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using BLL.Services;
using DAL.Managers;
using Simulation_garagistes.ViewModels;
using System.Threading;
using System.IO;

namespace Simulation_garagistes.Controllers
{
    public class SimulationController : Controller
    {
        private FranchiseService franchiseService = new FranchiseService(new FranchiseManager());
        private MarqueService marqueService = new MarqueService(new MarqueManager());
        private ModeleService modeleService = new ModeleService(new ModeleManager());
        private GaragisteService garagisteService = new GaragisteService(new GaragisteManager());
        private VoitureService voitureService = new VoitureService(new VoitureManager());
        private ReparationService reparationService = new ReparationService(new ReparationManager());
        private LogService logService = new LogService(new LogManager());
        private PeriodeFermetureService periodeFermetureService = new PeriodeFermetureService(new PeriodeFermetureManager());
        private static String dateJSON = "";
        private static bool interruptThread = false;
        private static bool fini = false;

        //
        // GET: /Simulation/

        public ActionResult Index()
        {
            vmSimulation vmSimulation = new vmSimulation();
            vmSimulation.Franchises = franchiseService.getAllFranchises();
            vmSimulation.Marques = marqueService.getAllMarques();
            vmSimulation.Modeles = modeleService.getAllModeles();

            return View(vmSimulation);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetLogs()
        {

            List<LogSimulation> logs = logService.getLastestLogs(20);

            Data data = new Data();
            data.logs = new string[logs.Count];
            data.types = new int[logs.Count];

            for(int i = 0; i<logs.Count; i++)
            {
                data.logs[i] = logs[i].Texte;
                data.types[i] = logs[i].Type;
            }

            data.date = "Jour : " + dateJSON;
            data.simulationTerminee = fini;

            return Json(data, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Pause(string etat)
        {
            if (etat == "Pause")
            {
                interruptThread = true;
            }
            else if (etat == "Unpause")
            {
                interruptThread = false;
            }
            else
            {
                return Json("'Success':'false'");
            }

            return Json("'Success':'true'");
        }

        [HttpPost]
        public ActionResult Test(vmGaragiste vmGaragiste)
        {
            new Thread(() =>
            {
                initSimulation();
                run();
                fini = true;

            }).Start();
    
            return View();
        }

        public ActionResult GetFile()
        {
            using (MemoryStream memoStream = new MemoryStream(1024 * 5))
            {
                using (StreamWriter writer = new StreamWriter(memoStream))
                {
                    List<LogSimulation> logsInSimu = logService.getLastestSimulation();
                    foreach (LogSimulation l in logsInSimu)
                    {
                        writer.WriteLine("[" + l.Date + "] " + l.Texte);
                    }
                }

                return File(new MemoryStream(memoStream.GetBuffer()), "text/txt", "Rapport_simulation.txt");
            }
        }

        public void run()
        {
            //FAIRE TOUS LES TESTS SUR LES DATES !
            string[] debut = Request["debut_simu"].Split('/');
            string[] fin = Request["fin_simu"].Split('/');
            DateTime dateCourante = new DateTime(int.Parse(debut[2]), int.Parse(debut[0]), int.Parse(debut[1]));
            DateTime dateFin = new DateTime(int.Parse(fin[2]), int.Parse(fin[0]), int.Parse(fin[1]));
            dateJSON = dateCourante.Day + "/" + dateCourante.Month + "/" + dateCourante.Year;

            logService.createLog("Début de la simulation au : " + dateJSON, DAL.Enums.LogType.DebutSimulation);
            List<Voiture> voituresEnJeu = voitureService.getAllVoitures();
            List<Garagiste> garagistesEnJeu = garagisteService.getAllGaragistes();
            List<Revision> revisionsAEffectuer = null;
            int charge = 0;
            bool repare = false;

            while (dateCourante.CompareTo(dateFin) < 0)
            {

                foreach (Voiture v in voituresEnJeu)
                {
                    checkIfInterrupted();

                    if ((revisionsAEffectuer = voitureService.rouler(v.ID, dateCourante)) != null)
                    {
                        //Il faut trouver un garage et faire les réparations
                        for (int i = 0; i < garagistesEnJeu.Count && !repare; i++)
                        {
                            //On teste si il est en vacances
                            if (periodeFermetureService.isVacances(garagistesEnJeu[i].ID, dateCourante))
                            {
                                logService.createLog("(" + dateJSON + ") Le garagiste (" + garagistesEnJeu[i].ID + ") est en vacances, il ne peut prendre la voiture (" + v.ID + ") pour la revision (" + revisionsAEffectuer[0].ID + ")", DAL.Enums.LogType.GaragisteEnVacances);
                            }

                            else
                            {
                                if (((charge = reparationService.getChargeHoraire(garagistesEnJeu[i].ID, dateCourante)) + revisionsAEffectuer[0].DureeIntervention.Hours) > 8)
                                {
                                    logService.createLog("(" + dateJSON + ") La voiture (" + v.ID + ") ne peut pas effectuer la revision (" + revisionsAEffectuer[0].ID + ") [" + revisionsAEffectuer[0].DureeIntervention.Hours + "h] chez le garagiste (" + garagistesEnJeu[i].ID + ") [" + charge + "/8h]", DAL.Enums.LogType.GaragistePlein);
                                }

                                else
                                {
                                    reparationService.createReparation(dateCourante, dateCourante, garagistesEnJeu[i].ID, v.ID, revisionsAEffectuer[0].ID);
                                    repare = true;
                                }
                            }
                        }

                        repare = false;
                    }
                }

                dateCourante = dateCourante.AddDays(1);
                dateJSON = dateCourante.Day + "/" + dateCourante.Month + "/" + dateCourante.Year;
            }

            logService.createLog("Fin de la simulation au : " + dateJSON, DAL.Enums.LogType.FinSimulation);
        }

        public void initSimulation()
        {
            int franchiseID, marqueID, modeleID;
            string franchiseName, marqueName, modeleName;
            int nbVoulu;

            foreach (string str in Request.Form)
            {
                //Création des garagistes
                if (str.StartsWith("franchise_"))
                {
                    franchiseID = int.Parse(str.Substring(10)); //FAIRE UN CONTROLE
                    franchiseName = franchiseService.getFranchiseById(franchiseID).Nom;
                    nbVoulu = int.Parse(Request.Form[str]); //FAIRE UN CONTROLE

                    for (int i = 0; i < nbVoulu; i++)
                    {
                        garagisteService.createGaragiste("Garagiste_" + franchiseName + "_" + i, franchiseID);
                    }
                }

                //Création des voitures
                //Par marque
                if (str.StartsWith("marque_"))
                {
                    marqueID = int.Parse(str.Substring(7)); //FAIRE UN CONTROLE
                    marqueName = marqueService.getMarqueById(marqueID).Nom;
                    nbVoulu = int.Parse(Request.Form[str]); //FAIRE UN CONTROLE

                    for (int i = 0; i < nbVoulu; i++)
                    {
                        voitureService.createVoiture(marqueService.getModeleAleatByMarque(marqueID));
                    }
                }

                //Par modèle
                if (str.StartsWith("modele_"))
                {
                    modeleID = int.Parse(str.Substring(7)); //FAIRE UN CONTROLE
                    modeleName = modeleService.getModeleById(modeleID).Nom;
                    nbVoulu = int.Parse(Request.Form[str]); //FAIRE UN CONTROLE

                    for (int i = 0; i < nbVoulu; i++)
                    {
                        voitureService.createVoiture(modeleID);
                    }
                }
            }

        }

        public void checkIfInterrupted()
        {
            while (interruptThread)
            {
                Thread.Sleep(1000);
            }
        }

        public class Data
        {
            public string[] logs { get; set; }
            public int[] types { get; set; }
            public string date { get; set; }
            public bool simulationTerminee { get; set; }
        }
    }
}
