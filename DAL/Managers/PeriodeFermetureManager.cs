using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IManagers;
using DAL.Models;

namespace DAL.Managers
{
    public class PeriodeFermetureManager : IPeriodeFermetureManager
    {
        private GarageEntities dbService;

        public PeriodeFermetureManager()
        {
            dbService = new GarageEntities();
        }

        public PeriodeFermeture getPeriodeFermetureById(int periodeFermetureID)
        {
            try
            {
                return (from p in dbService.PeriodeFermetureJeu
                        where p.ID == periodeFermetureID
                        select p
                            ).First();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<PeriodeFermeture> getPeriodeFermeturesByGaragiste(int garagisteID)
        {
            try
            {
                return (from p in dbService.PeriodeFermetureJeu
                        where p.Garagiste.ID == garagisteID 
                        select p).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<PeriodeFermeture> getAllPeriodeFermetures()
        {
            try
            {
                return (from p in dbService.PeriodeFermetureJeu
                        select p).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool updatePeriodeFermeture(PeriodeFermeture periodeFermeture, int garagisteID)
        {
            try
            {
                periodeFermeture.Garagiste = dbService.GaragisteJeu.Find(garagisteID);
                dbService.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool deletePeriodeFermeture(int periodeFermetureID)
        {
            try
            {
                PeriodeFermeture periodeFermeture = (from p in dbService.PeriodeFermetureJeu
                             where p.ID == periodeFermetureID
                             select p).First();

                dbService.PeriodeFermetureJeu.Remove(periodeFermeture);
                dbService.SaveChanges();
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public int createPeriodeFermeture(PeriodeFermeture periodeFermeture, int garagisteID)
        {
            try
            {
                periodeFermeture.Garagiste = dbService.GaragisteJeu.Find(garagisteID);
                dbService.PeriodeFermetureJeu.Add(periodeFermeture);
                dbService.SaveChanges();
                return periodeFermeture.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public bool isVacances(int garagisteID, DateTime jour)
        {
            try
            {
                return (from p in dbService.PeriodeFermetureJeu
                        where p.Garagiste.ID == garagisteID && p.DateDebut.Date <= jour.Date && p.DateFin.Date >= jour.Date
                        select p).Count() >= 1;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
