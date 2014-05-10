using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simulation_garagistes.ViewModels
{
    public class vmModele
    {
        public int ID {get;set;}
        public string Nom { get; set; }
        public Marque Marque { get; set; }
        public List<Modele> Modeles { get; set; }
        public List<Marque> Marques { get; set; }
    }
}