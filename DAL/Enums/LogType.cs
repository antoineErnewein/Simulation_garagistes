using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Enums
{
    /// <summary>
    /// Storage of the id of each role as defined in the DB
    /// </summary>
    public enum LogType
    {
        DebutSimulation = 1,
        FinSimulation = 2,
        VoitureRoule = 3,
        VoitureImmobilisee = 4,
        ReparationVoiture = 5,
        GaragisteEnVacances = 6,
        GaragistePlein = 7,
        Creations = 8,
        Autres = 9,
        ReparationSurDeuxJours = 10
    }
}