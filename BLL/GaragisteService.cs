using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Models;

namespace BLL
{
    public class GaragisteService
    {
        private GarageEntities dbContext; 

        public GaragisteService()
        {
            this.dbContext = new GarageEntities();
        }

        //Renvoie les réparations effectuées par le garage id pour le jour donné
        public List<Reparation> getAgenda(int garageID, DateTime jour)
        {
            List<Reparation> reparations = new List<Reparation>();
            Garagiste garagiste = dbContext.GaragisteJeu.Find(garageID);
            
            if (garagiste != null)
            {
                reparations = (from r in garagiste.Reparation where r.DateDebut.Date == jour.Date select r).ToList();
            }

            return reparations;
        }

        //Renvoie la charge horaire d'un garagiste pour un certain jour
        public int getChargeHoraire(int garageID, DateTime jour)
        {
            Garagiste garagiste = dbContext.GaragisteJeu.Find(garageID);
            List<Reparation> reparations = new List<Reparation>();
            int charge = -1;

            if (garagiste != null)
            {
                reparations = (from r in garagiste.Reparation where r.DateDebut.Date == jour.Date select r).ToList();

                foreach (Reparation rep in reparations)
                {
                    charge += rep.Revision.DureeIntervention.Hours;
                }
            }

            return charge;
        }

        //Renvoie si le jour en question est un jour de vaccances pour le garage
        public bool isHoliday(int garageID, DateTime jour)
        {
            Garagiste garagiste = dbContext.GaragisteJeu.Find(garageID);
            List<PeriodeFermeture> fermetures = new List<PeriodeFermeture>();
            bool res = false;

            if (garagiste != null)
            {
                fermetures = (from f in garagiste.PeriodeFermeture where f.DateDebut.Date <= jour.Date && f.DateFin.Date >= jour.Date select f).ToList();

                if(fermetures.Count >= 1)
                    res = true;
            }

            return res;
        }

    }
}
