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
        private static int nbAccidents;

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
            data.nbAccidents = nbAccidents;
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
                    writer.WriteLine("\r\nIl y a eu " + nbAccidents + " accidents pendant la simulation");
                }

                return File(new MemoryStream(memoStream.GetBuffer()), "text/txt", "Rapport_simulation.txt");
            }
        }

        public ActionResult Fixtures()
        {
            //Franchise
            int faudiID = franchiseService.createFranchise("Audi");
            int fbmwID = franchiseService.createFranchise("BMW");
            int fchevroletID = franchiseService.createFranchise("Chevrolet");
            int fdodgeID = franchiseService.createFranchise("Dodge");
            int ffordID = franchiseService.createFranchise("Ford");
            int fhondaID = franchiseService.createFranchise("Honda");
            int fjaguarID = franchiseService.createFranchise("Jaguar");
            int fmazdaID = franchiseService.createFranchise("Mazda");
            int fnissanID = franchiseService.createFranchise("Nissan");
            int fporscheID = franchiseService.createFranchise("Porsche");

            //Marques
            int audiID = marqueService.createMarque("Audi");
            int bmwID = marqueService.createMarque("BMW");
            int chevroletID = marqueService.createMarque("Chevrolet");
            int dodgeID = marqueService.createMarque("Dodge");
            int fordID = marqueService.createMarque("Ford");
            int hondaID = marqueService.createMarque("Honda");
            int jaguarID = marqueService.createMarque("Jaguar");
            int mazdaID = marqueService.createMarque("Mazda");
            int nissanID = marqueService.createMarque("Nissan");
            int porscheID = marqueService.createMarque("Porsche");

            //Modèles
            int a4ID = modeleService.createModele("A4", audiID);
            int rs7ID = modeleService.createModele("RS 7", audiID);
            int m6ID = modeleService.createModele("M6", bmwID);
            int camaroID = modeleService.createModele("Camaro", chevroletID);
            int impalaID = modeleService.createModele("Impala", chevroletID);
            int challengerD = modeleService.createModele("Challenger", dodgeID);
            int mustangID = modeleService.createModele("Mustang", fordID);
            int fiestaID = modeleService.createModele("Fiesta", fordID);
            int civicID = modeleService.createModele("Civic", hondaID);
            int xfID = modeleService.createModele("XF", jaguarID);
            int cx5ID = modeleService.createModele("CX-5", mazdaID);
            int z370ID = modeleService.createModele("370Z", nissanID);
            int boxsterID = modeleService.createModele("Boxster", porscheID);
            int cayenneID = modeleService.createModele("Cayenne", porscheID);

            //Révisions
            revisionService.createRevision("Vidange", 20000, new TimeSpan(1, 0, 0), -1, audiID);
            revisionService.createRevision("Freins", 100000, new TimeSpan(2, 0, 0), -1, audiID);
            revisionService.createRevision("Turbo", 150000, new TimeSpan(3, 0, 0), rs7ID, -1);
            revisionService.createRevision("Vidange", 70000, new TimeSpan(1, 0, 0), -1, bmwID);
            revisionService.createRevision("Freins", 120000, new TimeSpan(1, 0, 0), -1, bmwID);
            revisionService.createRevision("Vidange", 20000, new TimeSpan(2, 0, 0), -1, chevroletID);
            revisionService.createRevision("Freins", 100000, new TimeSpan(1, 0, 0), -1, chevroletID);
            revisionService.createRevision("Turbo", 150000, new TimeSpan(4, 0, 0), impalaID, -1);
            revisionService.createRevision("Vidange", 20000, new TimeSpan(1, 0, 0), -1, dodgeID);
            revisionService.createRevision("Vidange", 50000, new TimeSpan(1, 0, 0), -1, fordID);
            revisionService.createRevision("Freins", 150000, new TimeSpan(3, 0, 0), -1, fordID);
            revisionService.createRevision("Vidange", 20000, new TimeSpan(1, 0, 0), -1, hondaID);
            revisionService.createRevision("Freins", 10000, new TimeSpan(1, 0, 0), -1, hondaID);
            revisionService.createRevision("Vidange", 80000, new TimeSpan(2, 0, 0), -1, jaguarID);
            revisionService.createRevision("Vidange", 20000, new TimeSpan(2, 0, 0), -1, mazdaID);
            revisionService.createRevision("Freins", 100000, new TimeSpan(1, 0, 0), -1, mazdaID);
            revisionService.createRevision("Vidange", 40000, new TimeSpan(1, 0, 0), -1, nissanID);
            revisionService.createRevision("Freins", 110000, new TimeSpan(1, 0, 0), -1, nissanID);
            revisionService.createRevision("Vidange", 60000, new TimeSpan(2, 0, 0), -1, porscheID);
            revisionService.createRevision("Freins", 140000, new TimeSpan(3, 0, 0), -1, porscheID);
            revisionService.createRevision("Turbo", 170000, new TimeSpan(5, 0, 0), boxsterID, -1);

            //Garagistes
            int g1 = garagisteService.createGaragiste("Ferdinand Piech", faudiID);
            int g2 = garagisteService.createGaragiste("August Horch", faudiID);
            int g3 = garagisteService.createGaragiste("Gustav Otto", fbmwID);
            int g4 = garagisteService.createGaragiste("Karl Rapp", fbmwID);
            int g5 = garagisteService.createGaragiste("William C. Durant", fchevroletID);
            int g6 = garagisteService.createGaragiste("John Dodge", fdodgeID);
            int g7 = garagisteService.createGaragiste("Henry Ford", ffordID);
            int g8 = garagisteService.createGaragiste("Takeo Fujisawa", fhondaID);
            int g9 = garagisteService.createGaragiste("William Lyons", fjaguarID);
            int g10 = garagisteService.createGaragiste("Takashi Yamanouchi", fmazdaID);
            int g11 = garagisteService.createGaragiste("Carlos Ghosn", fnissanID);
            int g12 = garagisteService.createGaragiste("Matthias Müller", fporscheID);
            int g13 = garagisteService.createGaragiste("Wolfgang Porsche", fporscheID);

            //Vacances
            periodeFermetureService.createPeriodeFermeture(new DateTime(2014, 5, 30), new DateTime(2014, 6, 5), g1);
            periodeFermetureService.createPeriodeFermeture(new DateTime(2014, 6, 20), new DateTime(2014, 6, 22), g1);
            periodeFermetureService.createPeriodeFermeture(new DateTime(2014, 8, 1), new DateTime(2014, 8, 5), g2);
            periodeFermetureService.createPeriodeFermeture(new DateTime(2014, 8, 25), new DateTime(2014, 8, 29), g2);
            periodeFermetureService.createPeriodeFermeture(new DateTime(2014, 9, 2), new DateTime(2014, 9, 5), g3);
            periodeFermetureService.createPeriodeFermeture(new DateTime(2014, 10, 19), new DateTime(2014, 10, 23), g3);
            periodeFermetureService.createPeriodeFermeture(new DateTime(2014, 5, 16), new DateTime(2014, 5, 20), g4);
            periodeFermetureService.createPeriodeFermeture(new DateTime(2014, 6, 15), new DateTime(2014, 6, 19), g4);
            periodeFermetureService.createPeriodeFermeture(new DateTime(2014, 10, 13), new DateTime(2014, 10, 14), g5);
            periodeFermetureService.createPeriodeFermeture(new DateTime(2014, 2, 12), new DateTime(2014, 2, 15), g5);
            periodeFermetureService.createPeriodeFermeture(new DateTime(2014, 3, 24), new DateTime(2014, 3, 27), g6);
            periodeFermetureService.createPeriodeFermeture(new DateTime(2014, 5, 21), new DateTime(2014, 5, 23), g6);
            periodeFermetureService.createPeriodeFermeture(new DateTime(2014, 7, 10), new DateTime(2014, 7, 15), g7);
            periodeFermetureService.createPeriodeFermeture(new DateTime(2014, 7, 22), new DateTime(2014, 7, 23), g7);
            periodeFermetureService.createPeriodeFermeture(new DateTime(2014, 9, 22), new DateTime(2014, 9, 24), g8);
            periodeFermetureService.createPeriodeFermeture(new DateTime(2014, 12, 23), new DateTime(2014, 12, 25), g9);
            periodeFermetureService.createPeriodeFermeture(new DateTime(2014, 11, 12), new DateTime(2014, 11, 14), g10);
            periodeFermetureService.createPeriodeFermeture(new DateTime(2014, 8, 23), new DateTime(2014, 8, 25), g11);
            periodeFermetureService.createPeriodeFermeture(new DateTime(2014, 10, 3), new DateTime(2014, 10, 5), g12);
            periodeFermetureService.createPeriodeFermeture(new DateTime(2014, 5, 11), new DateTime(2014, 5, 12), g13);

            return View();
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
            nbAccidents = 0;

            //Test graphe reparations
            chartDates = new List<string>();
            chartNbRep = new List<int>();
            int repAvant = 0;
            int[] taux;

            //Gestion des accidents
            Random rand = new Random();
            List<Voiture> voituresAccidentes = new List<Voiture>();

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
                                    while (periodeFermetureService.isVacances(garagistesEnJeu[i].ID, dateCourante.AddDays(jadd)))
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

                    else
                    {
                        //0,5% de chances d'effectuer un accident
                        if (rand.Next(0, 1000) <= 5)
                        {
                            logService.createLog("(" + dateJSON + ") La voiture (" + v.ID + ") a eu un accident grave, elle est retirée de la simulation !", DAL.Enums.LogType.Accident);
                            voituresAccidentes.Add(v);
                            nbAccidents++;
                        }
                    }
                }

                //On supprime les voitures accidentés de la simulation
                foreach (Voiture va in voituresAccidentes)
                {
                    voituresEnJeu.RemoveAll(voiture => voiture.ID == va.ID);
                }
                voituresAccidentes.Clear();

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
            public int nbAccidents { get; set; }
            public string[] chartDate { get; set; }
            public int[] chartRep { get; set; }
        }
    }
}
