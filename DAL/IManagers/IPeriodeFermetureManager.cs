using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.IManagers
{
    public interface IPeriodeFermetureManager
    {
        PeriodeFermeture getPeriodeFermetureById(int periodeFermetureID);
        List<PeriodeFermeture> getPeriodeFermeturesByGaragiste(int garagisteID);
        List<PeriodeFermeture> getAllPeriodeFermetures();
        bool updatePeriodeFermeture(PeriodeFermeture periodeFermeture, int garagisteID);
        bool deletePeriodeFermeture(int periodeFermetureID);
        int createPeriodeFermeture(PeriodeFermeture periodeFermeture, int garagisteID);
        bool isVacances(int garagisteID, DateTime jour);
    }
}
