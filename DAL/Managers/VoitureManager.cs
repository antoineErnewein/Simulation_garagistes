using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IManagers;
using DAL.Models;

namespace DAL.Managers
{
    public class VoitureManager : IVoitureManager
    {
        private GarageEntities dbService;

        public VoitureManager()
        {
            dbService = new GarageEntities();
        }

        public Voiture getVoitureById(int voitureID)
        {
            try
            {
                return (from v in dbService.VoitureJeu
                        where v.ID == voitureID
                        select v
                            ).First();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Voiture> getVoituresByMarque(int marqueID)
        {
            try
            {
                return (from v in dbService.VoitureJeu
                        where v.Modele.Marque.ID == marqueID 
                        select v).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Voiture> getVoituresByModele(int modeleID)
        {
            try
            {
                return (from v in dbService.VoitureJeu
                        where v.Modele.ID == modeleID
                        select v).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Voiture> getAllVoitures()
        {
            try
            {
                return (from v in dbService.VoitureJeu
                        select v).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool updateVoiture(Voiture voiture, int modeleID)
        {
            try
            {
                voiture.Modele = dbService.ModeleJeu.Find(modeleID);
                dbService.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool deleteVoiture(int voitureID)
        {
            try
            {
                Voiture voiture = (from v in dbService.VoitureJeu
                             where v.ID == voitureID
                             select v).First();

                dbService.VoitureJeu.Remove(voiture);
                dbService.SaveChanges();
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public bool deleteAllVoitures()
        {
            try
            {
                List<Voiture> voitures = (from v in dbService.VoitureJeu
                                   select v).ToList();

                foreach (Voiture v in voitures)
                {
                    dbService.VoitureJeu.Remove(v);
                }
                
                dbService.SaveChanges();
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public int createVoiture(Voiture voiture, int modeleID)
        {
            try
            {
                voiture.Modele = dbService.ModeleJeu.Find(modeleID);
                dbService.VoitureJeu.Add(voiture);
                dbService.SaveChanges();
                return voiture.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
