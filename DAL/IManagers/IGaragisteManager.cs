using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.IManagers
{
    public interface IGaragisteManager
    {
        Garagiste getGaragisteById(int garagisteID);
        List<Garagiste> getGaragistesByFranchise(int franchiseID);
        List<Garagiste> getAllGaragistes();
        bool updateGaragiste(Garagiste garagiste, int franchiseID);
        bool deleteGaragiste(int garagisteID);
        int createGaragiste(Garagiste garagiste, int franchiseID);
    }
}
