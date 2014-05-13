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
    public class ModeleService
    {
        private IModeleManager modeleManager;

        public ModeleService(IModeleManager modeleManager)
        {
            this.modeleManager = modeleManager;
        }

        /// <summary>
        /// Obtient un modele par Id
        /// </summary>
        /// <param name="modeleID">ID</param>
        /// <returns>Modele</returns>
        public Modele getModeleById(int modeleID)
        {
            return modeleManager.getModeleById(modeleID);
        }

        /// <summary>
        /// Obtient tous les modèles d'une marque
        /// </summary>
        /// <param name="marqueID">ID</param>
        /// <returns>Liste de modèles</returns>
        public List<Modele> getModelesByMarque(int marqueID)
        {
            return modeleManager.getModelesByMarque(marqueID);
        }

        /// <summary>
        /// Retourne tous les modèles
        /// </summary>
        /// <returns>Liste de modèles</returns>
        public List<Modele> getAllModeles()
        {
            return modeleManager.getAllModeles();
        }

        /// <summary>
        /// Met à jour un modèle
        /// </summary>
        /// <param name="modeleID">Id du modèle</param>
        /// <param name="nom">nom</param>
        /// <param name="marqueID">Id de la marque</param>
        /// <returns>vrai si a fonctionné, sinon faux</returns>
        public bool updateModele(int modeleID, string nom, int marqueID)
        {
            Modele modele = modeleManager.getModeleById(modeleID);
            nom = nom.Trim();

            if (nom == "" || modele == null)
            {
                return false;
            }

            modele.Nom = nom;

            return modeleManager.updateModele(modele, marqueID);
        }

        /// <summary>
        /// Supprime un modèle par id
        /// </summary>
        /// <param name="modeleID"></param>
        /// <returns>vrai si a fonctionné, faux sinon</returns>
        public bool deleteModele(int modeleID)
        {
            return modeleManager.deleteModele(modeleID);
        }

        /// <summary>
        /// Créer un modèle
        /// </summary>
        /// <param name="nom">nom</param>
        /// <param name="marqueID">l'id de la marque</param>
        /// <returns>l'id du modèle ou -1 si erreur</returns>
        public int createModele(string nom, int marqueID)
        {
            Modele modele = new Modele();
            nom = nom.Trim();

            if (nom == "")
            {
                return -1;
            }

            modele.Nom = nom;

            return modeleManager.createModele(modele, marqueID);
        }
    }
}
