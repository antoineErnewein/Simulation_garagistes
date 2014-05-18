using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.IManagers
{
    public interface ILogManager
    {
        List<LogSimulation> getAllLogs();
        List<LogSimulation> getLastestLogs(int number);
        int createLog(LogSimulation log);
        List<LogSimulation> getLastestSimulation();
        List<LogSimulation> getLastestSimulationStats();
    }
}
