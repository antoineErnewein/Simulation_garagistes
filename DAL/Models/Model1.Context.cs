﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Ce code a été généré à partir d'un modèle.
//
//    Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//    Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class GarageEntities : DbContext
    {
        public GarageEntities()
            : base("name=GarageEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Garagiste> GaragisteJeu { get; set; }
        public DbSet<Voiture> VoitureJeu { get; set; }
        public DbSet<PeriodeFermeture> PeriodeFermetureJeu { get; set; }
        public DbSet<Revision> RevisionJeu { get; set; }
        public DbSet<Franchise> FranchiseJeu { get; set; }
        public DbSet<Reparation> ReparationJeu { get; set; }
        public DbSet<Modele> ModeleJeu { get; set; }
        public DbSet<Marque> MarqueJeu { get; set; }
    }
}