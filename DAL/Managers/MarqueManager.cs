using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IManagers;
using DAL.Models;

namespace DAL.Managers
{
    public class MarqueManager : IMarqueManager
    {
        private GarageEntities dbService;

        public MarqueManager()
        {
            dbService = new GarageEntities();
        }

        public Marque getMarqueById(int marqueID)
        {
            try
            {
                return (from m in dbService.MarqueJeu
                        where m.ID == marqueID
                        select m
                            ).First();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Marque> getAllMarques()
        {
            try
            {
                return (from m in dbService.MarqueJeu
                        select m).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool updateMarque()
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

        public bool deleteMarque(int marqueID)
        {
            try
            {
                Marque marque = (from m in dbService.MarqueJeu
                             where m.ID == marqueID
                             select m).First();

                dbService.MarqueJeu.Remove(marque);
                dbService.SaveChanges();
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public int createMarque(Marque marque)
        {
            try
            {
                dbService.MarqueJeu.Add(marque);
                dbService.SaveChanges();
                return marque.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
