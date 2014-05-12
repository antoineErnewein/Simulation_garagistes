using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Models;

namespace BLL
{
    class VoitureService
    {
        private GarageEntities dbContext; 

        public VoitureService()
        {
            this.dbContext = new GarageEntities();
        }

        //Créer une voiture du modele voulu avec un kilométrage entre 20.000 et 200.000 
        public int createVoiture(int modeleID)
        {
            Modele modele = dbContext.ModeleJeu.Find(modeleID);
            int id = -1;

            if (modele != null)
            {
                Random rand = new Random();

                Voiture voiture = new Voiture();
                voiture.Modele = modele;
                voiture.Kilometrage = rand.Next(20000, 200000);

                dbContext.VoitureJeu.Add(voiture);
                dbContext.SaveChanges();
                id = voiture.ID;
            }

            return id;  
        }

        //Retourne la liste des révisions a effectuer pour une voiture
        public List<Revision> getRevisionsAFaire(int voitureID)
        {
            List<Revision> res = new List<Revision>();
            Voiture voiture = dbContext.VoitureJeu.Find(voitureID);
            List<Revision> revisionsModele = new List<Revision>();
            List<Revision> revisionsMarque = new List<Revision>();
            List<Revision> revisions = new List<Revision>();
            Reparation reparation;

            if (voiture != null)
            {
               revisionsModele = (from r in voiture.Modele.Revision where r.Kilometrage <= voiture.Kilometrage select r).ToList();
               revisionsMarque = (from r in voiture.Modele.Marque.Revision where r.Kilometrage <= voiture.Kilometrage select r).ToList();
               
               revisions.AddRange(revisionsModele);
               revisions.AddRange(revisionsMarque);

               foreach (Revision r in revisions)
               {
                   reparation = (from rep in r.Reparation where rep.Voiture.ID == voitureID select rep).First();

                   if (reparation == null)
                   {
                       res.Add(r);
                   }
               }
            }

            return res;
        }

    }
}
