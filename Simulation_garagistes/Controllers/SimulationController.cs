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
        public JsonResult PopulateDetails(LogResultModel log)
        {
            /*LogResultModel logResultModel = new LogResultModel();
            

            logResultModel.Texte = logService.getLastestLogs(1)[0].Texte;
            logResultModel.Id = logService.getLastestLogs(1)[0].ID;
           

            return Json(logResultModel);*/
            //var myData = new[] { new { first = "Jan'e", last = "Doe" }, new { first = "John", last = "Doe" } };
            List<LogSimulation> logs = logService.getLastestLogs(20);
            string[] myData = new string[20];
            for(int i = 0; i<20; i++)
            {
                myData[i] = logs[i].Texte;
            }

            return Json(myData, JsonRequestBehavior.AllowGet);

        }

        public class LogResultModel
        {
            public string Texte { get; set; }
            public int Id { get; set;}
        }



        [HttpPost]
        public ActionResult Test(vmGaragiste vmGaragiste)
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
                        garagisteService.createGaragiste("Garagiste_"+franchiseName+"_"+i, franchiseID);
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

            return View();
        }

        //
        // GET: /Simulation/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Simulation/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Simulation/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Simulation/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Simulation/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Simulation/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Simulation/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
