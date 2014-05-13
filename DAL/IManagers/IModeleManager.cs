using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.IManagers
{
    public interface IModeleManager
    {
        Modele getModeleById(int modeleID);
        List<Modele> getModelesByMarque(int marqueID);
        List<Modele> getAllModeles();
        bool updateModele(Modele modele, int marqueID);
        bool deleteModele(int modeleID);
        int createModele(Modele modele, int marqueID);
    }
}
