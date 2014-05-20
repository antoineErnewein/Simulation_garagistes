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
    public class MarqueService
    {
        private IMarqueManager marqueManager;
        private LogService logService = new LogService(new LogManager());

        public MarqueService(IMarqueManager marqueManager)
        {
            this.marqueManager = marqueManager;
        }

        /// <summary>
        /// Obtient une marque par son ID
        /// </summary>
        /// <param name="marqueID">ID</param>
        /// <returns>Marque</returns>
        public Marque getMarqueById(int marqueID)
        {
            return marqueManager.getMarqueById(marqueID);
        }

        /// <summary>
        /// Retourne toutes les marques
        /// </summary>
        /// <returns>Liste de marques</returns>
        public List<Marque> getAllMarques()
        {
            return marqueManager.getAllMarques();
        }

        /// <summary>
        /// Met une marque à jour
        /// </summary>
        /// <param name="marqueID">ID</param>
        /// <param name="nom">Nom</param>
        /// <returns>vrai si a fonctionné, faux sinon</returns>
        public bool updateMarque(int marqueID, string nom)
        {
            Marque marque = marqueManager.getMarqueById(marqueID);
            nom = nom.Trim();

            if (nom == "" || marque == null)
            {
                return false;
            }

            marque.Nom = nom;
            return marqueManager.updateMarque();
        }

        /// <summary>
        /// Supprime une marque par son ID
        /// </summary>
        /// <param name="marqueID">ID</param>
        /// <returns>vrai si a fonctionné, faux sinon</returns>
        public bool deleteMarque(int marqueID)
        {
            return marqueManager.deleteMarque(marqueID);
        }

        /// <summary>
        /// Créé une marque
        /// </summary>
        /// <param name="nom">nom</param>
        /// <returns>Id de la marque ou -1 si erreur</returns>
        public int createMarque(string nom)
        {
            Marque marque = new Marque();
            nom = nom.Trim();

            if (nom == "")
            {
                return -1;
            }

            marque.Nom = nom;

            logService.createLog("Création marque : Nom : " + nom, DAL.Enums.LogType.Creations);
            return marqueManager.createMarque(marque);
        }

        /// <summary>
        /// Ajoute un modèle a une marque
        /// </summary>
        /// <param name="marqueID">ID de marque</param>
        /// <param name="nom">Nom du modèle</param>
        /// <returns>Id du modèle ou -1 si erreur</returns>
        public int addModele(int marqueID, string nom)
        {
            Modele modele = new Modele();
            nom = nom.Trim();

            if (nom == "")
            {
                return -1;
            }

            modele.Nom = nom;
            Marque marque = marqueManager.getMarqueById(marqueID);
            marque.Modele.Add(modele);
            marqueManager.updateMarque();

            return modele.ID;
        }

        public int getModeleAleatByMarque(int marqueID)
        {
            Marque marque = marqueManager.getMarqueById(marqueID);
            
            if(marque.Modele == null)
                return -1;

            List<Modele> modeles = marque.Modele.ToList();
            Random rand = new Random();

            if (modeles.Count == 0)
                return -1;

            return modeles[rand.Next(0,modeles.Count() -1)].ID;
        }

        public bool deleteAllMarques()
        {
            return marqueManager.deleteAllMarques();
        }
    }
}
