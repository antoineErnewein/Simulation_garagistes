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
    public class PeriodeFermetureService
    {
        private IPeriodeFermetureManager periodeFermetureManager;

        public PeriodeFermetureService(IPeriodeFermetureManager periodeFermetureManager)
        {
            this.periodeFermetureManager = periodeFermetureManager;
        }

        public PeriodeFermeture getPeriodeFermetureById(int periodeFermetureID)
        {
            return periodeFermetureManager.getPeriodeFermetureById(periodeFermetureID);
        }

        public List<PeriodeFermeture> getPeriodeFermeturesByGaragiste(int garagisteID)
        {
            return periodeFermetureManager.getPeriodeFermeturesByGaragiste(garagisteID);
        }

        public List<PeriodeFermeture> getAllPeriodeFermetures()
        {
            return periodeFermetureManager.getAllPeriodeFermetures();
        }

        public bool updatePeriodeFermeture(int periodeFermetureID, DateTime dateDebut, DateTime dateFin, int garagisteID)
        {
            PeriodeFermeture periodeFermeture = periodeFermetureManager.getPeriodeFermetureById(periodeFermetureID);

            if (dateDebut.Date > dateFin.Date)
            {
                return false;
            }

            periodeFermeture.DateDebut = dateDebut;
            periodeFermeture.DateFin = dateFin;

            return periodeFermetureManager.updatePeriodeFermeture(periodeFermeture, garagisteID);
        }

        public bool deletePeriodeFermeture(int periodeFermetureID)
        {
            return periodeFermetureManager.deletePeriodeFermeture(periodeFermetureID);
        }

        public int createPeriodeFermeture(DateTime dateDebut, DateTime dateFin, int garagisteID)
        {
            PeriodeFermeture periodeFermeture = new PeriodeFermeture();

            if (dateDebut.Date > dateFin.Date)
            {
                return -1;
            }

            periodeFermeture.DateDebut = dateDebut;
            periodeFermeture.DateFin = dateFin;

            return periodeFermetureManager.createPeriodeFermeture(periodeFermeture, garagisteID);
        }

        public bool isVacances(int garagisteID, DateTime jour)
        {
            return periodeFermetureManager.isVacances(garagisteID, jour);
        }
    }
}
