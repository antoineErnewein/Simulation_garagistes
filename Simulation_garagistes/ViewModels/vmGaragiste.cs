using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simulation_garagistes.ViewModels
{
    public class vmGaragiste
    {
        public int ID { get; set; }
        public string Nom { get; set; }
        public Franchise Franchise { get; set; }
        public List<PeriodeFermeture> PeriodeFermetures { get; set; }
        public List<Franchise> Franchises { get; set; }
    }
}