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
    public class LogService
    {
        private ILogManager logManager;

        public LogService(ILogManager logManager)
        {
            this.logManager = logManager;
        }

        public List<LogSimulation> getAllLogs()
        {
            return logManager.getAllLogs();
        }

        public List<LogSimulation> getLastestLogs(int number)
        {
            return logManager.getLastestLogs(number);
        }

        public int createLog(string texte)
        {
            LogSimulation log = new LogSimulation();
            log.Date = DateTime.Now;
            log.Texte = texte;

            return logManager.createLog(log);
        }
    }
}
