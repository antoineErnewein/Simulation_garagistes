using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using BLL.Services;
using DAL.Managers;
using Simulation_garagistes.ViewModels;

namespace Simulation_garagistes.Controllers
{
    public class SimulationController : Controller
    {
        private FranchiseService franchiseService = new FranchiseService(new FranchiseManager());
        private MarqueService marqueService = new MarqueService(new MarqueManager());
        private ModeleService modeleService = new ModeleService(new ModeleManager());
        private GaragisteService garagisteService = new GaragisteService(new GaragisteManager());
        private VoitureService voitureService = new VoitureService(new VoitureManager());
        private LogService logService = new LogService(new LogManager());

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
            string[] myData = new string[20];
            for(int i = 0; i<20; i++)
            {
                myData[i] = logs[i].Texte;
            }

            return Json(myData, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult Test(vmGaragiste vmGaragiste)
        {
            new System.Threading.Thread(() =>
            {
                initSimulation();
                run();

            }).Start();

            return View();
        }

        public void run()
        {
            //FAIRE TOUS LES TESTS SUR LES DATES !
            string[] debut = Request["debut_simu"].Split('/');
            string[] fin = Request["fin_simu"].Split('/');
            DateTime dateCourante = new DateTime(int.Parse(debut[2]), int.Parse(debut[0]), int.Parse(debut[1]));
            DateTime dateFin = new DateTime(int.Parse(fin[2]), int.Parse(fin[0]), int.Parse(fin[1]));

            logService.createLog("Début de la simulation au : " + Request["debut_simu"]);
            List<Voiture> voituresEnJeu = voitureService.getAllVoitures();
            List<Revision> revisionsAEffectuer = null;

            while (dateCourante.CompareTo(dateFin) < 0)
            {
                foreach (Voiture v in voituresEnJeu)
                {
                    if ((revisionsAEffectuer = voitureService.rouler(v.ID, dateCourante)) != null)
                    {
                        //Il faut trouver un garage et faire les réparations
                    }
                }

                dateCourante = dateCourante.AddDays(1);
            }

            logService.createLog("Fin de la simulation au : " + Request["fin_simu"]);
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
    }
}
