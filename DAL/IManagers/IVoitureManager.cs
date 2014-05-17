using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.IManagers
{
    public interface IVoitureManager
    {
        Voiture getVoitureById(int voitureID);
        List<Voiture> getVoituresByMarque(int marqueID);
        List<Voiture> getVoituresByModele(int modeleID);
        List<Voiture> getAllVoitures();
        bool updateVoiture(Voiture voiture, int modeleID);
        bool deleteVoiture(int voitureID);
        int createVoiture(Voiture voiture, int modeleID);
        bool deleteAllVoitures();
    }
}
