using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simulation_garagistes.ViewModels
{
    public class vmRevision
    {
        public int ID {get;set;}
        public string Libelle { get; set; }
        public double Kilometrage { get; set; }
        public System.TimeSpan DureeIntervention { get; set; }
        public Modele Modele { get; set; }
        public Marque Marque { get; set; }
        public List<Modele> Modeles { get; set; }
        public List<Marque> Marques { get; set; }
    }
}