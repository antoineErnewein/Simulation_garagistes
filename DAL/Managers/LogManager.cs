using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IManagers;
using DAL.Models;

namespace DAL.Managers
{
    public class LogManager : ILogManager
    {
        private GarageEntities dbService;

        public LogManager()
        {
            dbService = new GarageEntities();
        }

        public List<LogSimulation> getAllLogs()
        {
            try
            {
                return (from l in dbService.LogSimulationJeu
                        select l).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<LogSimulation> getLastestLogs(int number)
        {
            try
            {
                return (from l in dbService.LogSimulationJeu orderby l.ID descending
                        select l).Take(number).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<LogSimulation> getLastestSimulation()
        {
            try
            {
                int id = (from l in dbService.LogSimulationJeu where l.Type == 1
                        orderby l.ID descending
                        select l).First().ID;

                return (from l in dbService.LogSimulationJeu
                        where l.ID >= id
                        select l).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int createLog(LogSimulation log)
        {
            try
            {
                dbService.LogSimulationJeu.Add(log);
                dbService.SaveChanges();
                return log.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
