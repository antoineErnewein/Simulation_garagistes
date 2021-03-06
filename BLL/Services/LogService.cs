﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IManagers;
using DAL.Managers;
using DAL.Models;
using DAL.Enums;

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

        public int createLog(string texte, LogType type)
        {
            LogSimulation log = new LogSimulation();
            log.Date = DateTime.Now;
            log.Texte = texte;
            log.Type = (int)type;

            return logManager.createLog(log);
        }

        public List<LogSimulation> getLastestSimulation()
        {
            return logManager.getLastestSimulation();
        }

        public List<LogSimulation> getLastestSimulationStats()
        {
            return logManager.getLastestSimulationStats();
        }
    }
}
