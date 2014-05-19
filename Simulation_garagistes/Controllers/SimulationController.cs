using System;
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
        private RevisionService revisionService = new RevisionService(new RevisionManager());
        private static String dateJSON = "";
        private static bool interruptThread = false;
        private static bool fini = false;
        private static DateTime dateCourante;
        private static int nbReparations;

        private static List<String> chartDates = new List<string>();
        private static List<int> chartNbRep = new List<int>();

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
            data.tauxOccupation = garagisteService.getTauxOccupation(dateCourante);
            data.nbReparations = nbReparations;
            data.chartDate = chartDates.ToArray();
            data.chartRep = chartNbRep.ToArray();

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

        [HttpPost, ActionName("Test")]
        public ActionResult Test()
        {
            new Thread(() =>
            {
                fini = false;
                initSimulation();
                run();
                fini = true;
                suppressionData();

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

                    writer.WriteLine("\r\nStatistiques :\r\n");
                    List<LogSimulation> statsInSimu = logService.getLastestSimulationStats();
                    foreach (LogSimulation l in statsInSimu)
                    {
                        writer.WriteLine("[" + l.Date + "] " + l.Texte);
                    }
                }

                return File(new MemoryStream(memoStream.GetBuffer()), "text/txt", "Rapport_simulation.txt");
            }
        }

        public void run()
        {
            string[] debut = Request["debut_simu"].Split('/');
            string[] fin = Request["fin_simu"].Split('/');

            dateCourante = DateTime.Parse(Request["debut_simu"]);
            DateTime dateFin = DateTime.Parse(Request["fin_simu"]);
            dateJSON = dateCourante.Day + "/" + dateCourante.Month + "/" + dateCourante.Year;

            logService.createLog("Début de la simulation au : " + dateJSON, DAL.Enums.LogType.DebutSimulation);
            List<Voiture> voituresEnJeu = voitureService.getAllVoitures();
            List<Garagiste> garagistesEnJeu = garagisteService.getAllGaragistes();
            List<Revision> revisionsAEffectuer = null;
            int chargeFaisable = 0;
            bool repare = false;
            nbReparations = 0;

            //Test graphe reparations
            chartDates = new List<string>();
            chartNbRep = new List<int>();
            int repAvant = 0;
            int[] taux;

            while (dateCourante.CompareTo(dateFin) < 0)
            {
                chartDates.Add(dateCourante.Day + "/" + dateCourante.Month);
                repAvant = nbReparations;

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
                                if (reparationService.getChargeHoraire(garagistesEnJeu[i].ID, dateCourante) >= 8)
                                {
                                    logService.createLog("(" + dateJSON + ") La voiture (" + v.ID + ") ne peut pas effectuer la revision (" + revisionsAEffectuer[0].ID + ") [" + revisionsAEffectuer[0].DureeIntervention.Hours + "h] chez le garagiste (" + garagistesEnJeu[i].ID + ") [Agenda plein]", DAL.Enums.LogType.GaragistePlein);
                                }

                                else if (reparationService.getChargeHoraire(garagistesEnJeu[i].ID, dateCourante) + revisionsAEffectuer[0].DureeIntervention.Hours > 8)
                                {
                                    //On démarre la réparation à ce jour mais on la poursuit le lendemain
                                    chargeFaisable = 8 - reparationService.getChargeHoraire(garagistesEnJeu[i].ID, dateCourante);
                                      
                                    //On regarde si le(s) jour(s) suivant(s) ne sont pas des jours de vacances
                                    int jadd = 1;
                                    while(periodeFermetureService.isVacances(garagistesEnJeu[i].ID, dateCourante.AddDays(jadd)))
                                    {
                                        jadd++;
                                    }

                                    reparationService.createReparation(dateCourante, dateCourante.AddDays(jadd), chargeFaisable, garagistesEnJeu[i].ID, v.ID, revisionsAEffectuer[0].ID);
                                    reparationService.createReparation(dateCourante.AddDays(jadd), dateCourante.AddDays(jadd), (revisionsAEffectuer[0].DureeIntervention.Hours - chargeFaisable), garagistesEnJeu[i].ID, v.ID, revisionsAEffectuer[0].ID);
                                    
                                    logService.createLog("(" + dateJSON + ") La voiture (" + v.ID + ") effectue la revision (" + revisionsAEffectuer[0].ID + ") en 2 jours chez le garagiste (" + garagistesEnJeu[i].ID + ")", DAL.Enums.LogType.ReparationSurDeuxJours);
                                    nbReparations++;
                                }

                                else
                                {
                                    reparationService.createReparation(dateCourante, dateCourante, revisionsAEffectuer[0].DureeIntervention.Hours, garagistesEnJeu[i].ID, v.ID, revisionsAEffectuer[0].ID);
                                    repare = true;
                                    nbReparations++;
                                }
                            }
                        }

                        repare = false;
                    }
                }

                taux = garagisteService.getTauxOccupation(dateCourante);
                logService.createLog("Statistiques au (" + dateJSON + ") :\r\nOccupation des garages : " + (taux[0]+taux[1]) + "/" + taux[2] + "H\r\nNombre de réparations effectuées : " + (nbReparations - repAvant), DAL.Enums.LogType.Stats);
                dateCourante = dateCourante.AddDays(1);
                dateJSON = dateCourante.Day + "/" + dateCourante.Month + "/" + dateCourante.Year;
                chartNbRep.Add(nbReparations - repAvant);
            }

            logService.createLog("Fin de la simulation au : " + dateJSON, DAL.Enums.LogType.FinSimulation);
        }

        public void initSimulation()
        {
            int franchiseID, marqueID, modeleID;
            string franchiseName, marqueName, modeleName;
            int nbVoulu;
            int existant;

            foreach (string str in Request.Form)
            {
                //Création des garagistes
                if (str.StartsWith("franchise_"))
                {
                    franchiseID = int.Parse(str.Substring(10));
                    franchiseName = franchiseService.getFranchiseById(franchiseID).Nom;
                    nbVoulu = int.Parse(Request.Form[str]);

                    //On utilise les garagistes déjà créé 
                    existant = garagisteService.getGaragistesByFranchise(franchiseID).Count();

                    for (int i = existant; i < nbVoulu; i++)
                    {
                        garagisteService.createGaragiste("Garagiste_" + franchiseName + "_" + i, franchiseID);
                    }
                }

                //Création des voitures
                //Par marque
                if (str.StartsWith("marque_"))
                {
                    marqueID = int.Parse(str.Substring(7));
                    marqueName = marqueService.getMarqueById(marqueID).Nom;
                    nbVoulu = int.Parse(Request.Form[str]);

                    for (int i = 0; i < nbVoulu; i++)
                    {
                        voitureService.createVoiture(marqueService.getModeleAleatByMarque(marqueID));
                    }
                }

                //Par modèle
                if (str.StartsWith("modele_"))
                {
                    modeleID = int.Parse(str.Substring(7));
                    modeleName = modeleService.getModeleById(modeleID).Nom;
                    nbVoulu = int.Parse(Request.Form[str]);

                    for (int i = 0; i < nbVoulu; i++)
                    {
                        voitureService.createVoiture(modeleID);
                    }
                }
            }

        }

        public void suppressionData()
        {
            voitureService.deleteAllVoitures();
            garagisteService.deleteAllGaragistes();
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
            public int[] tauxOccupation { get; set; }
            public int nbReparations  { get; set; }
            public string[] chartDate { get; set; }
            public int[] chartRep { get; set; }
        }
    }
}
