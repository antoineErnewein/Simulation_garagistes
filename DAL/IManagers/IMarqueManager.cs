using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.IManagers
{
    public interface IMarqueManager
    {
        Marque getMarqueById(int marqueID);
        List<Marque> getAllMarques();
        bool updateMarque();
        bool deleteMarque(int marqueID);
        int createMarque(Marque marque);
    }
}
