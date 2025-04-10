using System;
using Newtonsoft.Json;

namespace AppGestionGarage.Achats
{

    /// <summary>
    /// Classe représentant une demande d'achat d'un véhicule.
    /// </summary>
    public class DemandeAchat
    {
        public int Id { get; set; }
        public string ClientLogin { get; set; }
        public string VehicleVIN { get; set; }
        public DateTime DateDemande { get; set; }
        public string Statut { get; set; } // Exemple : "En attente", "Approuvé", "Rejeté"
    }

    /// <summary>
    /// Gestionnaire pour les demandes d'achat.
    /// </summary>
    public class GestionnaireAchats
    {
        public List<DemandeAchat> DemandesAchats { get; set; }
        private readonly string fichierAchats = "C:\\Users\\KOUONGA\\Documents\\Dossier Ecole\\UQAR\\POO\\PROJET1\\achats.json";

        public GestionnaireAchats()
        {
            if (File.Exists(fichierAchats))
            {
                string json = File.ReadAllText(fichierAchats);
                DemandesAchats = JsonConvert.DeserializeObject<List<DemandeAchat>>(json);
            }
            else
            {
                DemandesAchats = new List<DemandeAchat>();
            }
        }

        public void SauvegarderAchats()
        {
            string json = JsonConvert.SerializeObject(DemandesAchats, Formatting.Indented);
            File.WriteAllText(fichierAchats, json);
        }

        public void AjouterDemandeAchat(DemandeAchat demande)
        {
            DemandesAchats.Add(demande);
            SauvegarderAchats();
            Console.WriteLine("Demande d'achat ajoutée.");
        }

        public List<DemandeAchat> ObtenirDemandesParClient(string clientLogin)
        {
            return DemandesAchats.FindAll(d => d.ClientLogin.Equals(clientLogin, StringComparison.OrdinalIgnoreCase));
        }
    }


}