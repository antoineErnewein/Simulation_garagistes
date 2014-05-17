using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IManagers;
using DAL.Models;


namespace DAL.Managers
{
    public class GaragisteManager : IGaragisteManager
    {
        private GarageEntities dbService;

        public GaragisteManager()
        {
            dbService = new GarageEntities();
        }

        public Garagiste getGaragisteById(int garagisteID)
        {
            try
            {
                return (from g in dbService.GaragisteJeu
                        where g.ID == garagisteID
                        select g
                            ).First();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Garagiste> getGaragistesByFranchise(int franchiseID)
        {
            try
            {
                return (from g in dbService.GaragisteJeu
                        where g.Franchise.ID == franchiseID 
                        select g).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Garagiste> getAllGaragistes()
        {
            try
            {
                return (from g in dbService.GaragisteJeu
                        select g).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool deleteAllGaragistes()
        {
            try
            {
                List<Garagiste> garagistes = (from g in dbService.GaragisteJeu
                        select g).ToList();

                foreach (Garagiste g in garagistes)
                {
                    dbService.GaragisteJeu.Remove(g);
                }
                
                dbService.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool updateGaragiste(Garagiste garagiste, int franchiseID)
        {
            try
            {
                garagiste.Franchise = dbService.FranchiseJeu.Find(franchiseID);
                dbService.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool deleteGaragiste(int garagisteID)
        {
            try
            {
                Garagiste garagiste = (from g in dbService.GaragisteJeu
                             where g.ID == garagisteID
                             select g).First();

                dbService.GaragisteJeu.Remove(garagiste);
                dbService.SaveChanges();
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public int createGaragiste(Garagiste garagiste, int franchiseID)
        {
            try
            {
                garagiste.Franchise = dbService.FranchiseJeu.Find(franchiseID);
                dbService.GaragisteJeu.Add(garagiste);
                dbService.SaveChanges();
                return garagiste.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

    }
}
