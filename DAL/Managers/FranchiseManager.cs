using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IManagers;
using DAL.Models;

namespace DAL.Managers
{
    public class FranchiseManager : IFranchiseManager
    {
        private GarageEntities dbService;

        public FranchiseManager()
        {
            dbService = new GarageEntities();
        }

        public Franchise getFranchiseById(int franchiseID)
        {
            try
            {
                return (from f in dbService.FranchiseJeu
                        where f.ID == franchiseID
                        select f
                            ).First();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Franchise> getAllFranchises()
        {
            try
            {
                return (from f in dbService.FranchiseJeu
                        select f).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool updateFranchise()
        {
            try
            {
                dbService.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool deleteFranchise(int franchiseID)
        {
            try
            {
                Franchise franchise = (from f in dbService.FranchiseJeu
                             where f.ID == franchiseID
                             select f).First();

                dbService.FranchiseJeu.Remove(franchise);
                dbService.SaveChanges();
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public int createFranchise(Franchise franchise)
        {
            try
            {
                dbService.FranchiseJeu.Add(franchise);
                dbService.SaveChanges();
                return franchise.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
