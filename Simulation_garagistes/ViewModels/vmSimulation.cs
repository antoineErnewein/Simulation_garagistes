using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.Models;

namespace Simulation_garagistes.ViewModels
{
    public class vmSimulation
    {
        public List<Franchise> Franchises { get; set; }
        public List<Marque> Marques { get; set; }
        public List<Modele> Modeles { get; set; }
    }
}