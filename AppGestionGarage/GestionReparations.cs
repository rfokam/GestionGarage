using System;
using AppGestionGarage.GestionPieces;

namespace AppGestionGarage.GestionReparations
{

    /// <summary>
    /// Représente une réparation effectuée sur un véhicule.
    /// </summary>
    public class ReparationEffectuee
    {
        public int Id { get; set; }
        public string VehicleVIN { get; set; }
        public string ClientLogin { get; set; }
        public string TypeIntervention { get; set; } // Par exemple : Mécanique, Carrosserie, Électronique, etc.
        public string DescriptionTravaux { get; set; }
        public decimal CoutEstime { get; set; }
        public List<PieceUtilisee> PiecesUtilisees { get; set; } = new List<PieceUtilisee>();
        public DateTime DateIntervention { get; set; }
    }

    /// <summary>
    /// Représente un devis de réparation détaillé.
    /// </summary>
    public class DevisReparation
    {
        public int Id { get; set; }
        public string VehicleVIN { get; set; }
        public string ClientLogin { get; set; }
        public DateTime DateDevis { get; set; }
        public decimal CoutEstime { get; set; }          // Coût total estimé (main-d’œuvre + pièces)
        public string DescriptionTravaux { get; set; }     // Détail des interventions prévues
        public decimal CoutMainOeuvre { get; set; }        // Coût de la main-d’œuvre (à renseigner par le propriétaire)
        public List<PieceUtilisee> PiecesNecessaires { get; set; } = new List<PieceUtilisee>();  // Liste des pièces nécessaires avec quantités
        public string Statut { get; set; }                 // Par exemple : "En attente", "Envoyé", "Validé", "Facturé"
    }

    /// <summary>
    /// Représente une facture liée à une réparation (générée à partir d'un devis).
    /// </summary>
    public class FactureReparation
    {
        public int Id { get; set; }
        public int DevisId { get; set; }
        public string ClientLogin { get; set; }
        public string ClientNom { get; set; }
        public string ClientAdresse { get; set; }
        public string ClientTelephone { get; set; }
        public string VehicleVIN { get; set; }
        public DateTime DateFacture { get; set; }
        public decimal CoutMainOeuvre { get; set; }
        public List<PieceUtilisee> PiecesUtilisees { get; set; } = new List<PieceUtilisee>();
        public decimal MontantTotal { get; set; }
        public string ModePaiement { get; set; } // ex. "Carte bancaire", "Espèces", "Virement", etc.
        public string StatutPaiement { get; set; } // "En attente" ou "Payé"
        public string DescriptionPrestation { get; set; } // Détail des interventions effectuées
    }


}
