using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.IManagers
{
    public interface IFranchiseManager
    {
        Franchise getFranchiseById(int franchiseID);
        List<Franchise> getAllFranchises();
        bool updateFranchise();
        bool deleteFranchise(int franchiseID);
        int createFranchise(Franchise franchise);
    }
}
