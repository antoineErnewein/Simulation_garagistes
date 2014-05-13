using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.IManagers
{
    public interface IReparationManager
    {
        Reparation getReparationById(int reparationID);
        List<Reparation> getReparationsByVoiture(int voitureID);
        List<Reparation> getReparationsByGaragiste(int garagisteID);
        List<Reparation> getReparationsByGaragisteAndJour(int garagisteID, DateTime jour);
        List<Reparation> getReparationsByVoitureAndRevision(int voitureID, int revisionID);
        List<Reparation> getAllReparations();
        bool updateReparation(Reparation reparation, int garagisteID, int voitureID, int revisionID);
        bool deleteReparation(int reparationID);
        int createReparation(Reparation reparation, int garagisteID, int voitureID, int revisionID);
    }
}
