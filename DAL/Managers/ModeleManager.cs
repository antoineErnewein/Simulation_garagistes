using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IManagers;
using DAL.Models;

namespace DAL.Managers
{
    public class ModeleManager : IModeleManager
    {
        private GarageEntities dbService;

        public ModeleManager()
        {
            dbService = new GarageEntities();
        }

        public Modele getModeleById(int modeleID)
        {
            try
            {
                return (from m in dbService.ModeleJeu
                        where m.ID == modeleID
                        select m
                            ).First();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Modele> getModelesByMarque(int marqueID)
        {
            try
            {
                return (from m in dbService.ModeleJeu
                        where m.Marque.ID == marqueID 
                        select m).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Modele> getAllModeles()
        {
            try
            {
                return (from m in dbService.ModeleJeu
                        select m).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool updateModele(Modele modele, int marqueID)
        {
            try
            {
                modele.Marque = dbService.MarqueJeu.Find(marqueID);
                dbService.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool deleteModele(int modeleID)
        {
            try
            {
                Modele modele = (from m in dbService.ModeleJeu
                             where m.ID == modeleID
                             select m).First();

                dbService.ModeleJeu.Remove(modele);
                dbService.SaveChanges();
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public int createModele(Modele modele, int marqueID)
        {
            try
            {
                modele.Marque = dbService.MarqueJeu.Find(marqueID);
                dbService.ModeleJeu.Add(modele);
                dbService.SaveChanges();
                return modele.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
