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
    public class FranchiseService
    {
        private IFranchiseManager franchiseManager;

        public FranchiseService(IFranchiseManager franchiseManager)
        {
            this.franchiseManager = franchiseManager;
        }

        /// <summary>
        /// Obtient une franchise par Id
        /// </summary>
        /// <param name="franchiseID">ID</param>
        /// <returns>Franchise</returns>
        public Franchise getFranchiseById(int franchiseID)
        {
            return franchiseManager.getFranchiseById(franchiseID);
        }

        /// <summary>
        /// Retourne toutes les franchises
        /// </summary>
        /// <returns>Liste de franchises</returns>
        public List<Franchise> getAllFranchises()
        {
            return franchiseManager.getAllFranchises();
        }

        /// <summary>
        /// Met une franchise à jour
        /// </summary>
        /// <param name="franchiseID">Id</param>
        /// <param name="nom">Nom</param>
        /// <returns>vrai si a fonctionné, faux sinon</returns>
        public bool updateFranchise(int franchiseID, string nom)
        {
            Franchise franchise = franchiseManager.getFranchiseById(franchiseID);
            nom = nom.Trim();

            if (nom == "" || franchise == null)
            {
                return false;
            }

            franchise.Nom = nom;
            return franchiseManager.updateFranchise();
        }

        /// <summary>
        /// Supprime une franchise par son ID
        /// </summary>
        /// <param name="franchiseID">ID</param>
        /// <returns>vrai si a fonctionné, faux sinon</returns>
        public bool deleteFranchise(int franchiseID)
        {
            return franchiseManager.deleteFranchise(franchiseID);
        }

        /// <summary>
        /// Crée une franchise
        /// </summary>
        /// <param name="nom">Nom</param>
        /// <returns>Id de la franchise crée ou -1 si erreur</returns>
        public int createFranchise(string nom)
        {
            Franchise franchise = new Franchise();
            nom = nom.Trim();

            if (nom == "")
            {
                return -1;
            }

            franchise.Nom = nom;

            return franchiseManager.createFranchise(franchise);
        }

    }
}
