//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Reparation
    {
        public int ID { get; set; }
        public System.DateTime DateDebut { get; set; }
        public System.DateTime DateFin { get; set; }
    
        public virtual Garagiste Garagiste { get; set; }
        public virtual Voiture Voiture { get; set; }
        public virtual Revision Revision { get; set; }
    }
}