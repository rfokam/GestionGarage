using Newtonsoft.Json;
using AppGestionGarage.Achats;
using AppGestionGarage.GestionPieces;
using AppGestionGarage.GestionReparations;
using static GarageApp.GestionnaireReparations;
using System.ComponentModel.DataAnnotations.Schema;
using AppGestionGarage.GestionReparations;


namespace GarageApp
{
    /// <summary>
    /// Classe abstraite représentant un utilisateur.
    /// </summary>
    public abstract class Utilisateur
    {
        public int User_ID { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public bool EstConnecte { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }

        /// <summary>
        /// Propriété abstraite pour obtenir le rôle de l'utilisateur.
        /// </summary>
        public abstract string Role { get; }

        /// <summary>
        /// Méthode pour connecter l'utilisateur.
        /// </summary>
        public void SeConnecter()
        {
            EstConnecte = true;
            Console.WriteLine($"L'utilisateur {Login} ({Role}) est connecté.");
        }

        /// <summary>
        /// Méthode pour déconnecter l'utilisateur.
        /// </summary>
        public void SeDeconnecter()
        {
            EstConnecte = false;
            Console.WriteLine($"L'utilisateur {Login} ({Role}) est déconnecté.");
        }
    }

    /// <summary>
    /// Classe représentant le propriétaire, disposant de l'ensemble des droits.
    /// </summary>
    public class Proprietaire : Utilisateur
    {
        public override string Role => "Propriétaire";
        // Ajoutez ici les méthodes spécifiques à la gestion des véhicules, pièces et utilisateurs.
    }

    /// <summary>
    /// Classe représentant un vendeur chargé des transactions commerciales.
    /// </summary>
    public class Vendeur : Utilisateur
    {
        public override string Role => "Vendeur";
        // Ajoutez ici les méthodes spécifiques aux ventes.
    }

    /// <summary>
    /// Classe représentant un client qui consulte le stock, achète et soumet des demandes de réparation.
    /// </summary>
    public class Client : Utilisateur
    {
        public override string Role => "Client";
        // Ajoutez ici les méthodes spécifiques à la consultation et aux achats.
    }

    /// <summary>
    /// Classe représentant un fournisseur assurant l'approvisionnement.
    /// </summary>
    public class Fournisseur : Utilisateur
    {
        public override string Role => "Fournisseur";
        // Ajoutez ici les méthodes spécifiques à l'approvisionnement.
    }

    /// <summary>
    /// Gestionnaire pour la persistance et la gestion des utilisateurs.
    /// </summary>
    public class GestionnaireUtilisateurs
    {
        public List<Utilisateur> Utilisateurs { get; set; }
        private readonly string fichier = "C:\\Users\\KOUONGA\\Documents\\Dossier Ecole\\UQAR\\POO\\PROJET1\\user.json";

        // Paramètres pour permettre la sérialisation/désérialisation polymorphique.
        private readonly JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        /// <summary>
        /// Constructeur qui charge les utilisateurs depuis le fichier JSON s'il existe.
        /// </summary>
        public GestionnaireUtilisateurs()
        {
            if (File.Exists(fichier))
            {
                string json = File.ReadAllText(fichier);
                Utilisateurs = JsonConvert.DeserializeObject<List<Utilisateur>>(json, settings);
            }
            else
            {
                Utilisateurs = new List<Utilisateur>();
            }
        }

        /// <summary>
        /// Sauvegarder la liste des utilisateurs dans le fichier JSON.
        /// </summary>
        public void SauvegarderUtilisateurs()
        {
            string json = JsonConvert.SerializeObject(Utilisateurs, Formatting.Indented, settings);
            File.WriteAllText(fichier, json);
        }

        /// <summary>
        /// Ajouter un nouvel utilisateur et sauvegarder.
        /// </summary>
        public void AjouterUtilisateur(Utilisateur utilisateur)
        {
            Utilisateurs.Add(utilisateur);
            SauvegarderUtilisateurs();
            Console.WriteLine("Utilisateur ajouté avec succès.");
        }

        /// <summary>
        /// Modifier un utilisateur existant en fonction de son User_ID.
        /// Pour conserver le typage, une nouvelle instance est créée en fonction du rôle.
        /// </summary>
        public void ModifierUtilisateur(int id, string nouveauLogin, string nouveauNom, string nouveauPrenom, string nouveauEmail, string nouvelleAdresse, string nouveauTelephone, string nouveauRole)
        {
            Utilisateur user = Utilisateurs.Find(u => u.User_ID == id);
            if (user != null)
            {
                // Mise à jour des informations communes.
                user.Login = nouveauLogin;
                user.Nom = nouveauNom;
                user.Prenom = nouveauPrenom;
                user.Email = nouveauEmail;
                user.Adresse = nouvelleAdresse;
                user.Telephone = nouveauTelephone;

                // Création d'une nouvelle instance selon le rôle pour conserver l'héritage.
                Utilisateur utilisateurModifie = null;
                switch (nouveauRole.ToLower())
                {
                    case "propriétaire":
                    case "proprietaire":
                        utilisateurModifie = new Proprietaire { User_ID = user.User_ID, Login = user.Login, Nom = user.Nom, Prenom = user.Prenom, Email = user.Email, EstConnecte = user.EstConnecte };
                        break;
                    case "vendeur":
                        utilisateurModifie = new Vendeur { User_ID = user.User_ID, Login = user.Login, Nom = user.Nom, Prenom = user.Prenom, Email = user.Email, EstConnecte = user.EstConnecte };
                        break;
                    case "client":
                        utilisateurModifie = new Client { User_ID = user.User_ID, Login = user.Login, Nom = user.Nom, Prenom = user.Prenom, Email = user.Email, EstConnecte = user.EstConnecte };
                        break;
                    case "fournisseur":
                        utilisateurModifie = new Fournisseur { User_ID = user.User_ID, Login = user.Login, Nom = user.Nom, Prenom = user.Prenom, Email = user.Email, EstConnecte = user.EstConnecte };
                        break;
                    default:
                        Console.WriteLine("Rôle invalide.");
                        return;
                }
                int index = Utilisateurs.IndexOf(user);
                Utilisateurs[index] = utilisateurModifie;
                SauvegarderUtilisateurs();
                Console.WriteLine("Utilisateur modifié avec succès.");
            }
            else
            {
                Console.WriteLine("Utilisateur non trouvé.");
            }
        }

        /// <summary>
        /// Supprimer un utilisateur par son User_ID.
        /// </summary>
        public void SupprimerUtilisateur(int id)
        {
            Utilisateur user = Utilisateurs.Find(u => u.User_ID == id);
            if (user != null)
            {
                Utilisateurs.Remove(user);
                SauvegarderUtilisateurs();
                Console.WriteLine("Utilisateur supprimé avec succès.");
            }
            else
            {
                Console.WriteLine("Utilisateur non trouvé.");
            }
        }

        /// <summary>
        /// Récupérer un utilisateur en fonction de son login.
        /// </summary>
        public Utilisateur ObtenirUtilisateurParLogin(string login)
        {
            return Utilisateurs.Find(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
        }
    }

    /// <summary>
    /// Classe pour gérer le menu en fonction du rôle de l'utilisateur connecté.
    /// </summary>
    public class Menu
    {
        private Utilisateur utilisateur;
        private GestionnaireUtilisateurs gestionnaire;
        private GestionnaireVehicules gestionnaireVehicules;
        private GestionnaireReparations gestionnaireReparations;
        private GestionnaireAchats gestionnaireAchats;
        private GestionnaireFactures gestionnaireFactures;
        private GestionnairePieces gestionnairePieces;
        private GestionnaireReparationsEffectuees gestionnaireReparationsEffectuees;
        private GestionnaireDevisReparation gestionnaireDevisReparation;
        private GestionnaireFacturesReparation gestionnaireFacturesReparation;

        public Menu(Utilisateur user, 
                    GestionnaireUtilisateurs gestionnaireUtilisateurs, 
                    GestionnaireVehicules gestionnaireVehicules, 
                    GestionnaireReparations gestionnaireReparations, 
                    GestionnaireAchats gestionnaireAchats, 
                    GestionnaireFactures gestionnaireFactures,
                    GestionnairePieces gestionnairePieces,
                    GestionnaireReparationsEffectuees gestionnaireReparationsEffectuees,
                    GestionnaireDevisReparation gestionnaireDevisReparation,
                    GestionnaireFacturesReparation gestionnaireFacturesReparation)
        {
            this.utilisateur = user;
            this.gestionnaire = gestionnaireUtilisateurs;
            this.gestionnaireVehicules = gestionnaireVehicules;
            this.gestionnaireReparations = gestionnaireReparations;
            this.gestionnaireReparations = gestionnaireReparations;
            this.gestionnaireAchats = gestionnaireAchats;
            this.gestionnaireFactures = gestionnaireFactures;
            this.gestionnairePieces = gestionnairePieces;
            this.gestionnaireReparationsEffectuees = gestionnaireReparationsEffectuees;
            this.gestionnaireDevisReparation = gestionnaireDevisReparation;
            this.gestionnaireFacturesReparation = gestionnaireFacturesReparation;
        }

        /// <summary>
        /// Afficher le menu adapté au rôle de l'utilisateur.
        /// </summary>
        public void AfficherMenu()
        {
            Console.WriteLine("\n--- Menu ---");
            if (utilisateur is Proprietaire)
            {
                Console.WriteLine("1. Ajouter un utilisateur");
                Console.WriteLine("2. Modifier un utilisateur");
                Console.WriteLine("3. Supprimer un utilisateur");
                Console.WriteLine("4. Afficher la liste des utilisateurs");
                Console.WriteLine("5. Gestion des véhicules");
                Console.WriteLine("6. Gérer les réparations (enregistrer, consulter, devis/factures)");
                Console.WriteLine("7. Déconnexion");
            }
            else if (utilisateur is Vendeur)
            {
                Console.WriteLine("1. Consulter le stock de véhicules");
                Console.WriteLine("2. Consulter les demandes d'achat");
                Console.WriteLine("3. Établir une facture pour une demande d'achat");
                Console.WriteLine("4. Consulter les factures générées");
                Console.WriteLine("5. Afficher les voitures vendues");
                Console.WriteLine("6. Déconnexion");
            }
            else if (utilisateur is Client)
            {
                Console.WriteLine("1. Gestion des Véhicules");
                Console.WriteLine("2. Gestion des Services");
                Console.WriteLine("3. Déconnexion");
            }
            else if (utilisateur is Fournisseur)
            {
                Console.WriteLine("1. Approvisionner en véhicules");
                Console.WriteLine("2. Approvisionner en pièces détachées");
                Console.WriteLine("3. Consulter mes approvisionnements");
                Console.WriteLine("4. Déconnexion");
            }
            else
            {
                Console.WriteLine("Rôle non reconnu.");
            }
        }

        /// <summary>
        /// Gérer le menu en fonction du rôle de l'utilisateur.
        /// Ici, seules les fonctionnalités pour le propriétaire sont entièrement implémentées.
        /// </summary>
        public void GererMenu()
        {
            bool continuer = true;
            while (continuer)
            {
                AfficherMenu();
                Console.Write("\nVotre choix: ");
                string choix = Console.ReadLine();

                if (utilisateur is Proprietaire)
                {
                    switch (choix)
                    {
                        case "1":
                            AjouterUtilisateur();
                            break;
                        case "2":
                            ModifierUtilisateur();
                            break;
                        case "3":
                            SupprimerUtilisateur();
                            break;
                        case "4":
                            AfficherListeUtilisateurs();
                            break;
                        case "5":
                            GererMenuVehicules();
                            break;
                        case "6":
                            GererReparations();
                            break;
                        case "7":
                            continuer = false;
                            break;
                        default:
                            Console.WriteLine("Choix invalide.");
                            break;
                    }
                }
                else if (utilisateur is Client)
                {
                    switch (choix)
                    {
                        case "1":
                            GererMenuVehiculesClient();
                            break;
                        case "2":
                            GererMenuServicesClient();
                            break;
                        case "3":
                            continuer = false;
                            break;
                        default:
                            Console.WriteLine("Choix invalide.");
                            break;
                    }
                }
                else if (utilisateur is Vendeur)
                {
                    switch (choix)
                    {
                        case "1":
                            ConsulterStockVendeur();
                            break;
                        case "2":
                            ConsulterDemandesAchatsVendeur();
                            break;
                        case "3":
                            EtablirFacture();
                            break;
                        case "4":
                            ConsulterFacturesVendeur();
                            break;
                        case "5":
                            AfficherVoituresVendues();
                            break;
                        case "6":
                            continuer = false;
                            break;
                        default:
                            Console.WriteLine("Choix invalide.");
                            break;
                    }
                }

                else if (utilisateur is Fournisseur)
                {
                    switch (choix)
                    {
                        case "1":
                            ApprovisionnerVehicules();
                            break;
                        case "2":
                            ApprovisionnerPieces();
                            break;
                        case "3":
                            ConsulterApprovisionnements();
                            break;
                        case "4":
                            continuer = false;
                            break;
                        default:
                            Console.WriteLine("Choix invalide.");
                            break;
                    }
                }

                else
                {
                    // Pour les autres rôles, seule la déconnexion est simulée ici.
                    if (choix == "3" || choix == "4")
                    {
                        continuer = false;
                    }
                    else
                    {
                        Console.WriteLine("Fonctionnalité non implémentée pour ce rôle.");
                    }
                }
            }
        }

        // Méthode pour créér le sous-menu de gestion des véhicules

        private void GererMenuVehiculesClient()
        {
            bool continuerVeh = true;
            while (continuerVeh)
            {
                Console.WriteLine("\n--- Gestion des Véhicules ---");
                Console.WriteLine("1. Consulter la liste des voitures disponibles");
                Console.WriteLine("2. Acheter une voiture");
                Console.WriteLine("3. Consulter mes demandes d'achat");
                Console.WriteLine("4. Consulter mes factures d'achat");
                Console.WriteLine("5. Payer une facture d'achat");
                Console.WriteLine("6. Retour au menu principal");
                Console.Write("Votre choix : ");
                string choixVeh = Console.ReadLine();
                switch (choixVeh)
                {
                    case "1":
                        ConsulterStockClient();
                        break;
                    case "2":
                        AcheterVoiture();
                        break;
                    case "3":
                        ConsulterMesDemandesAchats();
                        break;
                    case "4":
                        ConsulterMesFacturesClient();
                        break;
                    case "5":
                        PayerFacture();
                        break;
                    case "6":
                        continuerVeh = false;
                        break;
                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }
            }
        }

        private void GererMenuServicesClient()
{
    bool continuerServ = true;
    while (continuerServ)
    {
        Console.WriteLine("\n--- Gestion des Services ---");
        Console.WriteLine("1. Demander un devis de réparation");
        Console.WriteLine("2. Consulter mon historique de réparations");
        Console.WriteLine("3. Consulter mes factures de réparation");
        Console.WriteLine("4. Payer une facture de réparation");
        Console.WriteLine("5. Valider un devis de réparation");
        Console.WriteLine("6. Retour au menu principal");
        Console.Write("Votre choix : ");
        string choixServ = Console.ReadLine();
        switch (choixServ)
        {
            case "1":
                DemanderDevisReparation();
                break;
            case "2":
                ConsulterHistoriqueReparationsClient();
                break;
            case "3":
                ConsulterMesFacturesReparation();
                break;
            case "4":
                PayerFactureReparation();
                break;
            case "5":
                ValiderDevisReparation();
                break;
            case "6":
                continuerServ = false;
                break;
            default:
                Console.WriteLine("Choix invalide.");
                break;
        }
    }
}

        /// <summary>
        /// Procédure d'ajout d'un utilisateur via le menu (accessible au propriétaire).
        /// </summary>
        private void AjouterUtilisateur()
        {
            try
            {
                Console.Write("Entrez l'ID du nouvel utilisateur: ");
                int id = int.Parse(Console.ReadLine());
                Console.Write("Entrez le login du nouvel utilisateur: ");
                string login = Console.ReadLine();
                Console.Write("Entrez le nom: ");
                string nom = Console.ReadLine();
                Console.Write("Entrez le prénom: ");
                string prenom = Console.ReadLine();
                Console.Write("Entrez l'email: ");
                string email = Console.ReadLine();
                Console.Write("Entrez l'adresse: ");
                string adresse = Console.ReadLine();
                Console.Write("Entrez le Numéro de téléphone: ");
                string numerotelephone = Console.ReadLine();
                Console.Write("Entrez le rôle (Propriétaire, Vendeur, Client, Fournisseur): ");
                string roleStr = Console.ReadLine();

                Utilisateur nouvelUtilisateur = null;
                switch (roleStr.ToLower())
                {
                    case "propriétaire":
                    case "proprietaire":
                        nouvelUtilisateur = new Proprietaire { User_ID = id, Login = login, Nom = nom, Prenom = prenom, Email = email, EstConnecte = false, Adresse=adresse, Telephone=numerotelephone };
                        break;
                    case "vendeur":
                        nouvelUtilisateur = new Vendeur { User_ID = id, Login = login, Nom = nom, Prenom = prenom, Email = email, EstConnecte = false, Adresse = adresse, Telephone = numerotelephone };
                        break;
                    case "client":
                        nouvelUtilisateur = new Client { User_ID = id, Login = login, Nom = nom, Prenom = prenom, Email = email, EstConnecte = false, Adresse = adresse, Telephone = numerotelephone };
                        break;
                    case "fournisseur":
                        nouvelUtilisateur = new Fournisseur { User_ID = id, Login = login, Nom = nom, Prenom = prenom, Email = email, EstConnecte = false, Adresse = adresse, Telephone = numerotelephone };
                        break;
                    default:
                        Console.WriteLine("Rôle invalide.");
                        return;
                }
                gestionnaire.AjouterUtilisateur(nouvelUtilisateur);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'ajout de l'utilisateur: " + ex.Message);
            }
        }

        /// <summary>
        /// Procédure de modification d'un utilisateur via le menu (accessible au propriétaire).
        /// </summary>
        private void ModifierUtilisateur()
        {
            try
            {
                Console.Write("Entrez l'ID de l'utilisateur à modifier: ");
                int id = int.Parse(Console.ReadLine());
                Console.Write("Entrez le nouveau login: ");
                string nouveauLogin = Console.ReadLine();
                Console.Write("Entrez le nouveau nom: ");
                string nouveauNom = Console.ReadLine();
                Console.Write("Entrez le nouveau prénom: ");
                string nouveauPrenom = Console.ReadLine();
                Console.Write("Entrez le nouvel email: ");
                string nouveauEmail = Console.ReadLine();
                Console.Write("Entrez la nouvelle adresse: ");
                string nouvelleadresse = Console.ReadLine();
                Console.Write("Entrez le nouveau numéro de téléphone : ");
                string nouveautelephone = Console.ReadLine();
                Console.Write("Entrez le nouveau rôle (Propriétaire, Vendeur, Client, Fournisseur): ");
                string roleStr = Console.ReadLine();

                gestionnaire.ModifierUtilisateur(id, nouveauLogin, nouveauNom, nouveauPrenom, nouveauEmail, nouvelleadresse, nouveautelephone, roleStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la modification de l'utilisateur: " + ex.Message);
            }
        }

        /// <summary>
        /// Procédure de suppression d'un utilisateur via le menu (accessible au propriétaire).
        /// </summary>
        private void SupprimerUtilisateur()
        {
            try
            {
                Console.Write("Entrez l'ID de l'utilisateur à supprimer: ");
                int id = int.Parse(Console.ReadLine());
                gestionnaire.SupprimerUtilisateur(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la suppression de l'utilisateur: " + ex.Message);
            }
        }

        /// <summary>
        /// Procédure pour lister les utilisateurs via le menu (accessible au propriétaire).
        /// </summary>
        private void AfficherListeUtilisateurs()
        {
            Console.WriteLine("\nListe des utilisateurs :");
            foreach (var user in gestionnaire.Utilisateurs)
            {
                Console.WriteLine($"ID: {user.User_ID}, Login: {user.Login}, Nom: {user.Nom}, Prénom: {user.Prenom}, Email: {user.Email}, {user.Adresse}, {user.Telephone}, Rôle: {user.Role}");
            }
        }

        private void GererMenuVehicules()
        {
            bool continuerVehicule = true;
            while (continuerVehicule)
            {
                Console.WriteLine("\n--- Gestion des Véhicules ---");
                Console.WriteLine("1. Ajouter un véhicule");
                Console.WriteLine("2. Modifier un véhicule");
                Console.WriteLine("3. Supprimer un véhicule");
                Console.WriteLine("4. Consulter la liste des véhicules");
                Console.WriteLine("5. Retour au menu principal");
                Console.Write("Votre choix : ");
                string choixVehicule = Console.ReadLine();

                switch (choixVehicule)
                {
                    case "1":
                        AjouterVehicule();
                        break;
                    case "2":
                        ModifierVehicule();
                        break;
                    case "3":
                        SupprimerVehicule();
                        break;
                    case "4":
                        ConsulterVehicules();
                        break;
                    case "5":
                        continuerVehicule = false;
                        break;
                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }
            }
        }

        private void AjouterVehicule()
        {
            try
            {
                Console.Write("Marque : ");
                string marque = Console.ReadLine();
                Console.Write("Modèle : ");
                string modele = Console.ReadLine();
                Console.Write("Année : ");
                int annee = int.Parse(Console.ReadLine());
                Console.Write("Catégorie : ");
                string categorie = Console.ReadLine();
                Console.Write("Prix approximatif : ");
                decimal prix = decimal.Parse(Console.ReadLine());
                Console.Write("Kilométrage : ");
                int km = int.Parse(Console.ReadLine());
                Console.Write("Couleur : ");
                string couleur = Console.ReadLine();
                Console.Write("Type de carburant : ");
                string carburant = Console.ReadLine();
                Console.Write("Transmission : ");
                string transmission = Console.ReadLine();
                Console.Write("État général : ");
                string etat = Console.ReadLine();
                Console.Write("VIN : ");
                string vin = Console.ReadLine();
                Console.Write("Propriétaire actuel : ");
                string proprietaireActuel = Console.ReadLine();
                Console.Write("Date d'achat (yyyy-MM-dd) : ");
                DateTime dateAchat = DateTime.Parse(Console.ReadLine());
                Console.Write("Dernière révision (yyyy-MM-dd) : ");
                DateTime derniereRevision = DateTime.Parse(Console.ReadLine());
                Console.Write("Garantie restante (en mois) : ");
                int garantie = int.Parse(Console.ReadLine());
                Console.Write("Assurance (true/false) : ");
                bool assurance = bool.Parse(Console.ReadLine());

                Vehicule vehicule = new Vehicule
                {
                    Marque = marque,
                    Modele = modele,
                    Annee = annee,
                    Categorie = categorie,
                    PrixApproximatif = prix,
                    Kilometrage = km,
                    Couleur = couleur,
                    TypeDeCarburant = carburant,
                    Transmission = transmission,
                    EtatGeneral = etat,
                    VIN = vin,
                    ProprietaireActuel = proprietaireActuel,
                    DateAchat = dateAchat,
                    DerniereRevision = derniereRevision,
                    GarantieRestante = garantie,
                    Assurance = assurance
                };

                gestionnaireVehicules.AjouterVehicule(vehicule);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'ajout du véhicule: " + ex.Message);
            }
        }

        private void ModifierVehicule()
        {
            try
            {
                Console.Write("Entrez le VIN du véhicule à modifier : ");
                string vin = Console.ReadLine();
                Console.Write("Nouvelle marque : ");
                string marque = Console.ReadLine();
                Console.Write("Nouveau modèle : ");
                string modele = Console.ReadLine();
                Console.Write("Nouvelle année : ");
                int annee = int.Parse(Console.ReadLine());
                Console.Write("Nouvelle catégorie : ");
                string categorie = Console.ReadLine();
                Console.Write("Nouveau prix approximatif : ");
                decimal prix = decimal.Parse(Console.ReadLine());
                Console.Write("Nouveau kilométrage : ");
                int km = int.Parse(Console.ReadLine());
                Console.Write("Nouvelle couleur : ");
                string couleur = Console.ReadLine();
                Console.Write("Nouveau type de carburant : ");
                string carburant = Console.ReadLine();
                Console.Write("Nouvelle transmission : ");
                string transmission = Console.ReadLine();
                Console.Write("Nouvel état général : ");
                string etat = Console.ReadLine();
                Console.Write("Nouveau propriétaire actuel : ");
                string proprietaireActuel = Console.ReadLine();
                Console.Write("Nouvelle date d'achat (yyyy-MM-dd) : ");
                DateTime dateAchat = DateTime.Parse(Console.ReadLine());
                Console.Write("Nouvelle dernière révision (yyyy-MM-dd) : ");
                DateTime derniereRevision = DateTime.Parse(Console.ReadLine());
                Console.Write("Nouvelle garantie restante (en mois) : ");
                int garantie = int.Parse(Console.ReadLine());
                Console.Write("Nouvelle assurance (true/false) : ");
                bool assurance = bool.Parse(Console.ReadLine());

                Vehicule vehiculeMisAJour = new Vehicule
                {
                    Marque = marque,
                    Modele = modele,
                    Annee = annee,
                    Categorie = categorie,
                    PrixApproximatif = prix,
                    Kilometrage = km,
                    Couleur = couleur,
                    TypeDeCarburant = carburant,
                    Transmission = transmission,
                    EtatGeneral = etat,
                    VIN = vin, // Doit rester inchangé pour l'identification
                    ProprietaireActuel = proprietaireActuel,
                    DateAchat = dateAchat,
                    DerniereRevision = derniereRevision,
                    GarantieRestante = garantie,
                    Assurance = assurance
                };

                gestionnaireVehicules.ModifierVehicule(vin, vehiculeMisAJour);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la modification du véhicule: " + ex.Message);
            }
        }

        private void SupprimerVehicule()
        {
            Console.Write("Entrez le VIN du véhicule à supprimer : ");
            string vin = Console.ReadLine();
            gestionnaireVehicules.SupprimerVehicule(vin);
        }

        private void ConsulterVehicules()
        {
            Console.WriteLine("\n--- Voitures en Stock ---");
            bool stockFound = false;
            foreach (var v in gestionnaireVehicules.Vehicules)
            {
                if (v.EstDisponible)
                {
                    stockFound = true;
                    Console.WriteLine($"VIN: {v.VIN}, Marque: {v.Marque}, Modèle: {v.Modele}, Année: {v.Annee}, Catégorie: {v.Categorie}, Prix: {v.PrixApproximatif}, Km: {v.Kilometrage}");
                }
            }
            if (!stockFound)
            {
                Console.WriteLine("Aucune voiture en stock.");
            }

            Console.WriteLine("\n--- Voitures Vendues ---");
            bool soldFound = false;
            foreach (var v in gestionnaireVehicules.Vehicules)
            {
                if (!v.EstDisponible)
                {
                    soldFound = true;
                    Console.WriteLine($"VIN: {v.VIN}, Marque: {v.Marque}, Modèle: {v.Modele}, Année: {v.Annee}, Catégorie: {v.Categorie}, Prix: {v.PrixApproximatif}, Km: {v.Kilometrage}");
                }
            }
            if (!soldFound)
            {
                Console.WriteLine("Aucune voiture vendue.");
            }
        }


        // méthodes d'interaction pour le module client 
        private void ConsulterStockClient()
        {
            Console.WriteLine("\n--- Stock de Véhicules disponibles ---");
            foreach (var v in gestionnaireVehicules.Vehicules)
            {
                if (v.EstDisponible)
                {
                    Console.WriteLine($"VIN: {v.VIN}, Marque: {v.Marque}, Modèle: {v.Modele}, Année: {v.Annee}, Catégorie: {v.Categorie}, Prix: {v.PrixApproximatif}, Km: {v.Kilometrage}");
                }
            }
           
        }

        private void AcheterVoiture()
        {
            Console.WriteLine("\n--- Liste des véhicules disponibles ---");
            for (int i = 0; i < gestionnaireVehicules.Vehicules.Count; i++)
            {
                var vehicule = gestionnaireVehicules.Vehicules[i];
                if (vehicule.EstDisponible)
                {
                    Console.WriteLine($"{i + 1}. VIN: {vehicule.VIN}, Marque: {vehicule.Marque}, Modèle: {vehicule.Modele}, Année: {vehicule.Annee}, Prix: {vehicule.PrixApproximatif}");
                }
            }
            Console.Write("Sélectionnez le numéro du véhicule à acheter : ");
            int choix;
            if (int.TryParse(Console.ReadLine(), out choix) && choix >= 1 && choix <= gestionnaireVehicules.Vehicules.Count)
            {
                var vehiculeSelectionne = gestionnaireVehicules.Vehicules[choix - 1];
                DemandeAchat demande = new DemandeAchat
                {
                    Id = gestionnaireAchats.DemandesAchats.Count + 1,
                    ClientLogin = utilisateur.Login,
                    VehicleVIN = vehiculeSelectionne.VIN,
                    DateDemande = DateTime.Now,
                    Statut = "En attente"
                };
                gestionnaireAchats.AjouterDemandeAchat(demande);
                Console.WriteLine("Votre demande d'achat a été transmise au vendeur.");
            }
            else
            {
                Console.WriteLine("Choix invalide.");
            }
        }

        private void DemanderService()
        {
            Console.WriteLine("\n--- Catalogue des Services (Réparations/Entretien) ---");
            for (int i = 0; i < gestionnaireReparations.Reparations.Count; i++)
            {
                var rep = gestionnaireReparations.Reparations[i];
                Console.WriteLine($"{i + 1}. Catégorie: {rep.Categorie}, Pièce: {rep.NomDePiece}, Prix approximatif: {rep.PrixApprox}, Description: {rep.Description}");
            }
            Console.Write("Sélectionnez le numéro du service souhaité : ");
            int choixService;
            if (int.TryParse(Console.ReadLine(), out choixService) && choixService > 0 && choixService <= gestionnaireReparations.Reparations.Count)
            {
                // Simulation : la demande de service est transmise au propriétaire
                Console.WriteLine("Votre demande de service a été transmise au propriétaire pour prise en charge.");
            }
            else
            {
                Console.WriteLine("Service invalide.");
            }
        }

        // Methode pour permettre au client de consulter ses demandes d'achat
        private void ConsulterMesDemandesAchats()
        {
            Console.WriteLine("\n--- Mes demandes d'achat ---");
            var mesDemandes = gestionnaireAchats.ObtenirDemandesParClient(utilisateur.Login);
            if (mesDemandes.Count == 0)
            {
                Console.WriteLine("Aucune demande d'achat trouvée.");
            }
            else
            {
                foreach (var demande in mesDemandes)
                {
                    Console.WriteLine($"ID: {demande.Id}, VIN: {demande.VehicleVIN}, Date: {demande.DateDemande}, Statut: {demande.Statut}");
                }
            }
        }

        // Methode pour permettre au Vendeur de consulter ses demandes d'achat reçues
        private void ConsulterDemandesAchatsVendeur()
        {
            Console.WriteLine("\n--- Demandes d'achat reçues ---");
            foreach (var demande in gestionnaireAchats.DemandesAchats)
            {
                Console.WriteLine($"ID: {demande.Id}, Client: {demande.ClientLogin}, VIN: {demande.VehicleVIN}, Date: {demande.DateDemande}, Statut: {demande.Statut}");
            }
        }

        // Les methodes pour le vendeur

        private void ConsulterStockVendeur()
        {
            Console.WriteLine("\n--- Stock de Véhicules ---");
            foreach (var v in gestionnaireVehicules.Vehicules)
            {
                if (v.EstDisponible)
                {
                    Console.WriteLine($"VIN: {v.VIN}, Marque: {v.Marque}, Modèle: {v.Modele}, Année: {v.Annee}, Prix: {v.PrixApproximatif}");
                }
            }
        }

        private void EtablirFacture()
        {
            Console.Write("Entrez l'ID de la demande d'achat pour laquelle générer une facture : ");
            int idDemande;
            if (int.TryParse(Console.ReadLine(), out idDemande))
            {
                var demande = gestionnaireAchats.DemandesAchats.Find(d => d.Id == idDemande);
                if (demande != null)
                {
                    var vehicule = gestionnaireVehicules.Vehicules.Find(v => v.VIN == demande.VehicleVIN);
                    if (vehicule != null)
                    {
                        Facture facture = new Facture
                        {
                            Id = gestionnaireFactures.Factures.Count + 1,
                            DemandeAchatId = demande.Id,
                            ClientLogin = demande.ClientLogin,
                            VehicleVIN = demande.VehicleVIN,
                            DateFacture = DateTime.Now,
                            Montant = vehicule.PrixApproximatif,
                            StatutPaiement = "En attente"
                        };
                        gestionnaireFactures.AjouterFacture(facture);
                    }
                    else
                    {
                        Console.WriteLine("Véhicule associé introuvable.");
                    }
                }
                else
                {
                    Console.WriteLine("Demande d'achat introuvable.");
                }
            }
            else
            {
                Console.WriteLine("ID invalide.");
            }
        }

        private void ConsulterFacturesVendeur()
        {
            Console.WriteLine("\n--- Factures générées ---");
            foreach (var facture in gestionnaireFactures.Factures)
            {
                Console.WriteLine($"ID: {facture.Id}, Demande: {facture.DemandeAchatId}, Client: {facture.ClientLogin}, VIN: {facture.VehicleVIN}, Date: {facture.DateFacture}, Montant: {facture.Montant}, Statut: {facture.StatutPaiement}");
            }
        }

        private void AfficherVoituresVendues()
        {
            Console.WriteLine("\n--- Voitures Vendues ---");
            foreach (var v in gestionnaireVehicules.Vehicules)
            {
                if (!v.EstDisponible)
                {
                    Console.WriteLine($"VIN: {v.VIN}, Marque: {v.Marque}, Modèle: {v.Modele}, Année: {v.Annee}");
                }
            }
        }

        private void ConsulterMesFacturesClient()
        {
            Console.WriteLine("\n--- Mes Factures ---");
            var factures = gestionnaireFactures.ObtenirFacturesParClient(utilisateur.Login);
            if (factures.Count == 0)
            {
                Console.WriteLine("Aucune facture trouvée.");
            }
            else
            {
                foreach (var facture in factures)
                {
                    Console.WriteLine($"ID: {facture.Id}, Demande: {facture.DemandeAchatId}, VIN: {facture.VehicleVIN}, Date: {facture.DateFacture}, Montant: {facture.Montant}, Statut: {facture.StatutPaiement}");
                }
            }
        }

        private void PayerFacture()
        {
            Console.Write("Entrez l'ID de la facture à payer : ");
            int idFacture;
            if (int.TryParse(Console.ReadLine(), out idFacture))
            {
                var facture = gestionnaireFactures.Factures.Find(f => f.Id == idFacture && f.ClientLogin.Equals(utilisateur.Login, StringComparison.OrdinalIgnoreCase));
                if (facture != null)
                {
                    if (facture.StatutPaiement.Equals("En attente", StringComparison.OrdinalIgnoreCase))
                    {
                        // Simuler le paiement
                        facture.StatutPaiement = "Payé";
                        gestionnaireFactures.SauvegarderFactures();

                        // Marquer le véhicule comme vendu (non disponible)
                        var vehicule = gestionnaireVehicules.Vehicules.Find(v => v.VIN == facture.VehicleVIN);
                        if (vehicule != null)
                        {
                            vehicule.EstDisponible = false;
                            gestionnaireVehicules.SauvegarderVehicules();
                            Console.WriteLine("Paiement effectué. Le véhicule est désormais vendu.");
                        }
                        else
                        {
                            Console.WriteLine("Véhicule associé introuvable.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("La facture a déjà été payée.");
                    }
                }
                else
                {
                    Console.WriteLine("Facture introuvable.");
                }
            }
            else
            {
                Console.WriteLine("ID invalide.");
            }
        }

        private void ApprovisionnerVehicules()
        {
            try
            {
                Console.Write("Marque : ");
                string marque = Console.ReadLine();
                Console.Write("Modèle : ");
                string modele = Console.ReadLine();
                Console.Write("Année : ");
                int annee = int.Parse(Console.ReadLine());
                Console.Write("Catégorie : ");
                string categorie = Console.ReadLine();
                Console.Write("Prix approximatif : ");
                decimal prix = decimal.Parse(Console.ReadLine());
                Console.Write("Kilométrage : ");
                int km = int.Parse(Console.ReadLine());
                Console.Write("Couleur : ");
                string couleur = Console.ReadLine();
                Console.Write("Type de carburant : ");
                string carburant = Console.ReadLine();
                Console.Write("Transmission : ");
                string transmission = Console.ReadLine();
                Console.Write("État général : ");
                string etat = Console.ReadLine();
                Console.Write("VIN : ");
                string vin = Console.ReadLine();
                Console.Write("Propriétaire actuel : ");
                string proprietaireActuel = Console.ReadLine();
                Console.Write("Date d'achat (yyyy-MM-dd) : ");
                DateTime dateAchat = DateTime.Parse(Console.ReadLine());
                Console.Write("Dernière révision (yyyy-MM-dd) : ");
                DateTime derniereRevision = DateTime.Parse(Console.ReadLine());
                Console.Write("Garantie restante (en mois) : ");
                int garantie = int.Parse(Console.ReadLine());
                Console.Write("Assurance (true/false) : ");
                bool assurance = bool.Parse(Console.ReadLine());

                string fournisseurLogin = utilisateur.Login; // Récupère le login du fournisseur

                Vehicule vehicule = new Vehicule
                {
                    Marque = marque,
                    Modele = modele,
                    Annee = annee,
                    Categorie = categorie,
                    PrixApproximatif = prix,
                    Kilometrage = km,
                    Couleur = couleur,
                    TypeDeCarburant = carburant,
                    Transmission = transmission,
                    EtatGeneral = etat,
                    VIN = vin,
                    ProprietaireActuel = proprietaireActuel,
                    DateAchat = dateAchat,
                    DerniereRevision = derniereRevision,
                    GarantieRestante = garantie,
                    Assurance = assurance,
                    EstDisponible = true,
                    FournisseurLogin = fournisseurLogin
                };

                // L'approvisionnement se fait via l'ajout dans le gestionnaire des véhicules.
                gestionnaireVehicules.AjouterVehicule(vehicule);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'approvisionnement du véhicule: " + ex.Message);
            }
        }

        // Module permettant au fournisseur d’ajouter une nouvelle pièce détachée dans le magasin
        private void ApprovisionnerPieces()
        {
            try
            {
                Console.Write("Nom de la pièce : ");
                string nom = Console.ReadLine();
                Console.Write("Catégorie : ");
                string categorie = Console.ReadLine();
                Console.Write("Prix : ");
                decimal prix = decimal.Parse(Console.ReadLine());
                Console.Write("Quantité : ");
                int quantite = int.Parse(Console.ReadLine());
                Console.Write("Description : ");
                string description = Console.ReadLine();
                DateTime dateAppro = DateTime.Now; // Date d'approvisionnement actuelle
                string fournisseurLogin = utilisateur.Login;

                PieceDetachee piece = new PieceDetachee
                {
                    Id = new Random().Next(1, 100000), // Génération simplifiée d'un ID
                    Nom = nom,
                    Categorie = categorie,
                    Prix = prix,
                    Quantite = quantite,
                    Description = description,
                    DateApprovisionnement = dateAppro,
                    FournisseurLogin = fournisseurLogin
                };

                gestionnairePieces.AjouterPiece(piece);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'approvisionnement de la pièce détachée: " + ex.Message);
            }
        }

        private void ConsulterApprovisionnements()
        {
            Console.WriteLine("\n--- Approvisionnements effectués ---");

            Console.WriteLine("\nVéhicules approvisionnés par vous :");
            foreach (var v in gestionnaireVehicules.Vehicules)
            {
                if (v.FournisseurLogin.Equals(utilisateur.Login, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"VIN: {v.VIN}, Marque: {v.Marque}, Modèle: {v.Modele}, Année: {v.Annee}");
                }
            }

            Console.WriteLine("\nPièces détachées approvisionnées par vous :");
            foreach (var p in gestionnairePieces.Pieces)
            {
                if (p.FournisseurLogin.Equals(utilisateur.Login, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"ID: {p.Id}, Nom: {p.Nom}, Catégorie: {p.Categorie}, Quantité: {p.Quantite}");
                }
            }
        }

        private void DemanderDevisReparation()
        {
            try
            {
                Console.Write("Entrez le VIN du véhicule concerné : ");
                string vin = Console.ReadLine();
                Console.Write("Entrez la marque du véhicule : ");
                string marque = Console.ReadLine();
                Console.Write("Entrez l'année du véhicule : ");
                int annee = int.Parse(Console.ReadLine());
                Console.Write("Entrez la description de la réparation souhaitée : ");
                string description = Console.ReadLine();
                Console.Write("Entrez une estimation de coût (optionnel, 0 si inconnu) : ");
                decimal coutEstime = decimal.Parse(Console.ReadLine());

                DevisReparation devis = new DevisReparation
                {
                    Id = gestionnaireDevisReparation.DevisReparations.Count + 1,
                    VehicleVIN = vin,
                    ClientLogin = utilisateur.Login,
                    DateDevis = DateTime.Now,
                    CoutEstime = coutEstime,
                    DescriptionTravaux = description,
                    CoutMainOeuvre = 0, // Sera renseigné par le propriétaire
                    Statut = "En attente"
                };

                gestionnaireDevisReparation.AjouterDevis(devis);
                Console.WriteLine("Votre demande de devis a été soumise au propriétaire.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la demande de devis: " + ex.Message);
            }
        }

        private void ConsulterHistoriqueReparationsClient()
        {
            Console.WriteLine("\n--- Historique de vos réparations ---");
            var reparations = gestionnaireReparationsEffectuees.ObtenirReparationsParClient(utilisateur.Login);
            if (reparations.Count == 0)
            {
                Console.WriteLine("Aucune réparation enregistrée.");
            }
            else
            {
                foreach (var rep in reparations)
                {
                    Console.WriteLine($"ID: {rep.Id}, VIN: {rep.VehicleVIN}, Type: {rep.TypeIntervention}, Coût: {rep.CoutEstime}, Date: {rep.DateIntervention}");
                }
            }
        }

        private void ConsulterMesFacturesReparation()
        {
            Console.WriteLine("\n--- Mes Factures de Réparation ---");
            var factures = gestionnaireFacturesReparation.FacturesReparation.Where(f => f.ClientLogin.Equals(utilisateur.Login, StringComparison.OrdinalIgnoreCase)).ToList();
            if (factures.Count == 0)
            {
                Console.WriteLine("Aucune facture de réparation trouvée.");
            }
            else
            {
                foreach (var facture in factures)
                {
                    Console.WriteLine($"ID: {facture.Id}, Devis ID: {facture.DevisId}, Montant: {facture.MontantTotal}, Statut: {facture.StatutPaiement}, Date: {facture.DateFacture}");
                }
            }
        }

        private void PayerFactureReparation()
        {
            Console.Write("Entrez l'ID de la facture à payer : ");
            int idFacture;
            if (!int.TryParse(Console.ReadLine(), out idFacture))
            {
                Console.WriteLine("ID invalide.");
                return;
            }
            // Recherche de la facture correspondant à l'ID et appartenant au client connecté
            var facture = gestionnaireFacturesReparation.FacturesReparation
                                .Find(f => f.Id == idFacture && f.ClientLogin.Equals(utilisateur.Login, StringComparison.OrdinalIgnoreCase));
            if (facture == null)
            {
                Console.WriteLine("Facture non trouvée.");
                return;
            }

            // Afficher les détails complets de la facture
            AfficherDetailsFacture(facture);

            Console.Write("Voulez-vous confirmer le paiement de cette facture ? (O/N) : ");
            if (Console.ReadLine().Equals("O", StringComparison.OrdinalIgnoreCase))
            {
                if (facture.StatutPaiement.Equals("En attente", StringComparison.OrdinalIgnoreCase))
                {
                    facture.StatutPaiement = "Payé";
                    gestionnaireFacturesReparation.SauvegarderFacturesReparation();
                    Console.WriteLine("Paiement effectué. Merci !");
                }
                else
                {
                    Console.WriteLine("Cette facture a déjà été payée.");
                }
            }
            else
            {
                Console.WriteLine("Le paiement a été annulé.");
            }
        }

        private void GererReparations()
        {
            bool continuerRepar = true;
            while (continuerRepar)
            {
                Console.WriteLine("\n--- Gestion des Réparations ---");
                Console.WriteLine("1. Enregistrer une réparation effectuée");
                Console.WriteLine("2. Consulter l'historique des réparations");
                Console.WriteLine("3. Générer un devis de réparation");
                Console.WriteLine("4. Lister les demandes de réparation");
                Console.WriteLine("5. Générer une facture de réparation");
                Console.WriteLine("6. Lister les factures de réparation");
                Console.WriteLine("7. Retour au menu principal");
                Console.Write("Votre choix : ");
                string choixRep = Console.ReadLine();
                switch (choixRep)
                {
                    case "1":
                        EnregistrerReparationEffectuee();
                        break;
                    case "2":
                        ConsulterHistoriqueReparations();
                        break;
                    case "3":
                        GenererDevisReparation();
                        break;
                    case "4":
                        ListerDevisReparation();
                        break;
                    case "5":
                        GenererFactureReparation();
                        break;
                    case "6":
                        ListerFacturesReparation();
                        break;
                    case "7":
                        continuerRepar = false;
                        break;
                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }
            }
        }

        private void EnregistrerReparationEffectuee()
        {
            try
            {
                Console.Write("Entrez le VIN du véhicule réparé : ");
                string vin = Console.ReadLine();
                Console.Write("Entrez le login du client : ");
                string clientLogin = Console.ReadLine();

                // Afficher la liste des réparations disponibles (catalogue importé depuis reparation.json)
                Console.WriteLine("\n--- Liste des réparations disponibles ---");
                for (int i = 0; i < gestionnaireReparations.Reparations.Count; i++)
                {
                    var reparCatalog = gestionnaireReparations.Reparations[i];
                    Console.WriteLine($"{i + 1}. Catégorie: {reparCatalog.Categorie}, Pièce: {reparCatalog.NomDePiece}, Prix approximatif: {reparCatalog.PrixApprox}, Description: {reparCatalog.Description}");
                }
                Console.Write("Sélectionnez le numéro de la réparation à enregistrer : ");
                int choix;
                if (!int.TryParse(Console.ReadLine(), out choix) || choix < 1 || choix > gestionnaireReparations.Reparations.Count)
                {
                    Console.WriteLine("Sélection invalide.");
                    return;
                }
                var reparSelectionnee = gestionnaireReparations.Reparations[choix - 1];

                // Utiliser les valeurs du catalogue par défaut
                string typeIntervention = reparSelectionnee.Categorie;
                string description = reparSelectionnee.Description;
                decimal coutEstime = reparSelectionnee.PrixApprox;

                // Optionnel : possibilité de modifier le coût ou la description
                Console.Write("Voulez-vous modifier le coût estimé ? (O/N) : ");
                if (Console.ReadLine().Equals("O", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Entrez le nouveau coût estimé : ");
                    coutEstime = decimal.Parse(Console.ReadLine());
                }
                Console.Write("Voulez-vous modifier la description ? (O/N) : ");
                if (Console.ReadLine().Equals("O", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Entrez la nouvelle description : ");
                    description = Console.ReadLine();
                }

                // Création de la réparation effectuée en utilisant les données sélectionnées
                ReparationEffectuee repEffectuee = new ReparationEffectuee
                {
                    Id = gestionnaireReparationsEffectuees.ReparationsEffectuees.Count + 1,
                    VehicleVIN = vin,
                    ClientLogin = clientLogin,
                    TypeIntervention = typeIntervention,
                    DescriptionTravaux = description,
                    CoutEstime = coutEstime,
                    DateIntervention = DateTime.Now
                };

                // Ajout des pièces utilisées avec gestion de la quantité
                Console.Write("Des pièces ont-elles été utilisées ? (O/N) : ");
                string reponse = Console.ReadLine();
                while (reponse.Equals("O", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Entrez l'ID de la pièce utilisée : ");
                    int idPiece = int.Parse(Console.ReadLine());
                    PieceDetachee piece = gestionnairePieces.Pieces.Find(p => p.Id == idPiece);
                    if (piece != null)
                    {
                        Console.Write($"Quantité utilisée pour la pièce '{piece.Nom}' (stock disponible : {piece.Quantite}) : ");
                        int quantiteUtilisee = int.Parse(Console.ReadLine());
                        if (quantiteUtilisee > 0 && quantiteUtilisee <= piece.Quantite)
                        {
                            // Décrémenter le stock
                            piece.Quantite -= quantiteUtilisee;

                            // Ajouter la pièce utilisée à la réparation
                            repEffectuee.PiecesUtilisees.Add(new PieceUtilisee
                            {
                                Piece = piece,
                                QuantiteUtilisee = quantiteUtilisee
                            });

                            Console.Write("Ajouter une autre pièce ? (O/N) : ");
                            reponse = Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("Quantité invalide. La quantité doit être supérieure à 0 et inférieure ou égale au stock disponible.");
                            Console.Write("Réessayez ? (O/N) : ");
                            reponse = Console.ReadLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Pièce non trouvée.");
                        Console.Write("Réessayez ? (O/N) : ");
                        reponse = Console.ReadLine();
                    }
                }

                gestionnaireReparationsEffectuees.AjouterReparationEffectuee(repEffectuee);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'enregistrement de la réparation : " + ex.Message);
            }
        }

        private void ConsulterHistoriqueReparations()
        {
            Console.WriteLine("\n--- Historique des Réparations ---");
            foreach (var rep in gestionnaireReparationsEffectuees.ReparationsEffectuees)
            {
                Console.WriteLine($"ID: {rep.Id}, VIN: {rep.VehicleVIN}, Client: {rep.ClientLogin}, Type: {rep.TypeIntervention}, Coût: {rep.CoutEstime}, Date: {rep.DateIntervention}");
            }
        }

        private void GenererDevisReparation()
        {
            try
            {
                Console.Write("Entrez l'ID du devis à traiter : ");
                int idDevis = int.Parse(Console.ReadLine());
                var devis = gestionnaireDevisReparation.DevisReparations.Find(d => d.Id == idDevis);
                if (devis == null)
                {
                    Console.WriteLine("Devis non trouvé.");
                    return;
                }
                if (!devis.Statut.Equals("En attente", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Ce devis a déjà été traité.");
                    return;
                }

                Console.Write("Entrez le coût de la main-d'œuvre : ");
                decimal coutMainOeuvre = decimal.Parse(Console.ReadLine());
                devis.CoutMainOeuvre = coutMainOeuvre;

                // Ajout des pièces nécessaires
                Console.Write("Des pièces sont-elles nécessaires ? (O/N) : ");
                string rep = Console.ReadLine();
                while (rep.Equals("O", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Entrez l'ID de la pièce nécessaire : ");
                    int idPiece = int.Parse(Console.ReadLine());
                    PieceDetachee piece = gestionnairePieces.Pieces.Find(p => p.Id == idPiece);
                    if (piece != null)
                    {
                        Console.Write($"Quantité requise pour la pièce '{piece.Nom}' : ");
                        int quantite = int.Parse(Console.ReadLine());
                        if (quantite > 0)
                        {
                            devis.PiecesNecessaires.Add(new PieceUtilisee { Piece = piece, QuantiteUtilisee = quantite });
                        }
                        else
                        {
                            Console.WriteLine("Quantité invalide.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Pièce non trouvée.");
                    }
                    Console.Write("Ajouter une autre pièce ? (O/N) : ");
                    rep = Console.ReadLine();
                }

                // Calcul du coût total estimé : main-d'œuvre + coût des pièces
                decimal coutPieces = devis.PiecesNecessaires.Sum(x => x.Piece.Prix * x.QuantiteUtilisee);
                devis.CoutEstime = coutMainOeuvre + coutPieces;

                // Mettre à jour le statut pour transmission au client
                devis.Statut = "Envoyé";

                gestionnaireDevisReparation.SauvegarderDevis();
                Console.WriteLine("Devis généré et transmis au client.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la génération du devis: " + ex.Message);
            }
        }

        private void GenererFactureReparation()
        {
            try
            {
                Console.Write("Entrez l'ID du devis validé pour générer la facture : ");
                int idDevis = int.Parse(Console.ReadLine());
                var devis = gestionnaireDevisReparation.DevisReparations.Find(d => d.Id == idDevis);
                if (devis == null || !devis.Statut.Equals("Validé", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Devis non trouvé ou non validé.");
                    return;
                }

                // Récupérer les informations du client depuis le gestionnaire d'utilisateurs
                var client = gestionnaire.ObtenirUtilisateurParLogin(devis.ClientLogin);
                string nomClient = client != null ? client.Nom + " " + client.Prenom : "Inconnu";
                string adresseClient = client != null ? client.Adresse : "Non renseignée";
                string telephoneClient = client != null ? client.Telephone : "Non renseigné";

                // Demande des informations complémentaires
                Console.Write("Entrez la description détaillée des prestations effectuées : ");
                string descriptionPrestation = Console.ReadLine();
                Console.Write("Entrez le mode de paiement (Carte bancaire, Espèces, Virement, etc.) : ");
                string modePaiement = Console.ReadLine();

                // Calcul du montant total : coût main-d'œuvre + coût total des pièces nécessaires
                decimal coutMainOeuvre = devis.CoutMainOeuvre;
                decimal coutPieces = devis.PiecesNecessaires.Sum(p => p.Piece.Prix * p.QuantiteUtilisee);
                decimal montantTotal = coutMainOeuvre + coutPieces;

                FactureReparation facture = new FactureReparation
                {
                    Id = gestionnaireFacturesReparation.FacturesReparation.Count + 1,
                    DevisId = devis.Id,
                    ClientLogin = devis.ClientLogin,
                    ClientNom = nomClient,
                    ClientAdresse = adresseClient,
                    ClientTelephone = telephoneClient,
                    VehicleVIN = devis.VehicleVIN,
                    DateFacture = DateTime.Now,
                    CoutMainOeuvre = coutMainOeuvre,
                    PiecesUtilisees = devis.PiecesNecessaires, // On reprend la liste des pièces renseignées dans le devis
                    MontantTotal = montantTotal,
                    ModePaiement = modePaiement,
                    StatutPaiement = "En attente",
                    DescriptionPrestation = descriptionPrestation
                };

                gestionnaireFacturesReparation.AjouterFactureReparation(facture);
                devis.Statut = "Facturé";
                gestionnaireDevisReparation.SauvegarderDevis();
                Console.WriteLine("Facture générée et transmise au client.");

                // Appel de la méthode d'affichage des détails de la facture
                AfficherDetailsFacture(facture);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la génération de la facture: " + ex.Message);
            }
        }


        private void AfficherDetailsFacture(FactureReparation facture)
        {
            Console.WriteLine("\n--- Détails de la Facture ---");
            Console.WriteLine($"ID Facture: {facture.Id}");
            Console.WriteLine($"Date: {facture.DateFacture}");
            Console.WriteLine($"Client: {facture.ClientNom}");
            Console.WriteLine($"Adresse: {facture.ClientAdresse}");
            Console.WriteLine($"Téléphone: {facture.ClientTelephone}");
            Console.WriteLine($"Véhicule (VIN): {facture.VehicleVIN}");
            Console.WriteLine($"Description des prestations: {facture.DescriptionPrestation}");
            Console.WriteLine($"Coût main-d'œuvre: {facture.CoutMainOeuvre:C}");
            Console.WriteLine("Pièces utilisées:");
            foreach (var pu in facture.PiecesUtilisees)
            {
                Console.WriteLine($"   - {pu.Piece.Nom}: {pu.QuantiteUtilisee} x {pu.Piece.Prix:C} = {(pu.Piece.Prix * pu.QuantiteUtilisee):OC}");
            }
            Console.WriteLine($"Montant total à payer: {facture.MontantTotal:C}");
            Console.WriteLine($"Mode de paiement: {facture.ModePaiement}");
            Console.WriteLine($"Statut de paiement: {facture.StatutPaiement}");
        }

        private void ListerDevisReparation()
        {
            Console.WriteLine("\n--- Liste des devis de réparation ---");
            foreach (var devis in gestionnaireDevisReparation.DevisReparations)
            {
                Console.WriteLine($"ID: {devis.Id}, VIN: {devis.VehicleVIN}, Client: {devis.ClientLogin}, Cout estimé: {devis.CoutEstime}, Description: {devis.DescriptionTravaux}, Statut: {devis.Statut}");
            }
        }

        private void ListerFacturesReparation()
        {
            Console.WriteLine("\n--- Liste des factures de réparation ---");
            foreach (var facture in gestionnaireFacturesReparation.FacturesReparation)
            {
                Console.WriteLine($"ID: {facture.Id}, DevisID: {facture.DevisId}, Client: {facture.ClientLogin}, VIN: {facture.VehicleVIN}, Montant: {facture.MontantTotal}, Statut: {facture.StatutPaiement}, Date: {facture.DateFacture}");
            }
        }

        private void ValiderDevisReparation()
        {
            // Afficher les devis transmis pour le client
            AfficherDevisTransmisClient();

            Console.Write("Entrez l'ID du devis à valider parmi ceux affichés : ");
            int idDevis;
            if (!int.TryParse(Console.ReadLine(), out idDevis))
            {
                Console.WriteLine("ID invalide.");
                return;
            }

            var devis = gestionnaireDevisReparation.DevisReparations
                            .Find(d => d.Id == idDevis
                                && d.ClientLogin.Equals(utilisateur.Login, StringComparison.OrdinalIgnoreCase)
                                && d.Statut.Equals("Envoyé", StringComparison.OrdinalIgnoreCase));

            if (devis == null)
            {
                Console.WriteLine("Devis non trouvé ou non transmis.");
                return;
            }

            devis.Statut = "Validé";
            gestionnaireDevisReparation.SauvegarderDevis();
            Console.WriteLine("Devis validé et renvoyé au propriétaire.");
        }

        private void AfficherDevisTransmisClient()
        {
            Console.WriteLine("\n--- Liste des devis transmis ---");
            var devisTransmis = gestionnaireDevisReparation.DevisReparations
                                .Where(d => d.ClientLogin.Equals(utilisateur.Login, StringComparison.OrdinalIgnoreCase)
                                            && d.Statut.Equals("Envoyé", StringComparison.OrdinalIgnoreCase))
                                .ToList();
            if (devisTransmis.Count == 0)
            {
                Console.WriteLine("Aucun devis transmis n'est disponible.");
            }
            else
            {
                foreach (var devis in devisTransmis)
                {
                    Console.WriteLine($"ID: {devis.Id}, VIN: {devis.VehicleVIN}, Date: {devis.DateDevis}, Cout estimé: {devis.CoutEstime}, Description: {devis.DescriptionTravaux}");
                }
            }
        }

    }





    /// <summary>
    /// Classe représentant un véhicule.
    /// </summary>
    public class Vehicule
    {
        public string Marque { get; set; }
        public string Modele { get; set; }
        public int Annee { get; set; }
        public string Categorie { get; set; }
        public decimal PrixApproximatif { get; set; }
        public int Kilometrage { get; set; }
        public string Couleur { get; set; }
        public string TypeDeCarburant { get; set; }
        public string Transmission { get; set; }
        public string EtatGeneral { get; set; }
        public string VIN { get; set; }
        public string ProprietaireActuel { get; set; }
        public DateTime DateAchat { get; set; }
        public DateTime DerniereRevision { get; set; }
        public int GarantieRestante { get; set; }
        public bool Assurance { get; set; }
        public bool EstDisponible { get; set; } = true;
        public string FournisseurLogin { get; set; } = string.Empty;
    }

    /// <summary>
    /// Classe représentant une réparation.
    /// </summary>
    public class Reparation
    {
        public string Categorie { get; set; }
        public string NomDePiece { get; set; }
        public decimal PrixApprox { get; set; }
        public string Description { get; set; }
        public string ReparationAssociee { get; set; }
    }


    /// <summary>
    /// Gestionnaire pour les véhicules.
    /// </summary>
    public class GestionnaireVehicules
    {
        public List<Vehicule> Vehicules { get; set; }
        private readonly string fichierVehicules = "C:\\Users\\KOUONGA\\Documents\\Dossier Ecole\\UQAR\\POO\\PROJET1\\vehicules.json";

        public GestionnaireVehicules()
        {
            if (File.Exists(fichierVehicules))
            {
                string json = File.ReadAllText(fichierVehicules);
                Vehicules = JsonConvert.DeserializeObject<List<Vehicule>>(json);
            }
            else
            {
                Vehicules = new List<Vehicule>();
            }
        }

        public void SauvegarderVehicules()
        {
            string json = JsonConvert.SerializeObject(Vehicules, Formatting.Indented);
            File.WriteAllText(fichierVehicules, json);
        }

        public void ImporterVehiculesDepuisCSV(string cheminCSV)
        {
            if (!File.Exists(cheminCSV))
            {
                Console.WriteLine("Fichier CSV des véhicules non trouvé.");
                return;
            }

            var lignes = File.ReadAllLines(cheminCSV);
            // Supposons que la première ligne contient les en-têtes.
            for (int i = 1; i < lignes.Length; i++)
            {
                var colonnes = lignes[i].Split(',');
                try
                {
                    Vehicule vehicule = new Vehicule
                    {
                        Marque = colonnes[0],
                        Modele = colonnes[1],
                        Annee = int.Parse(colonnes[2]),
                        Categorie = colonnes[3],
                        PrixApproximatif = decimal.Parse(colonnes[4]),
                        Kilometrage = int.Parse(colonnes[5]),
                        Couleur = colonnes[6],
                        TypeDeCarburant = colonnes[7],
                        Transmission = colonnes[8],
                        EtatGeneral = colonnes[9],
                        VIN = colonnes[10],
                        ProprietaireActuel = colonnes[11],
                        DateAchat = DateTime.Parse(colonnes[12]),
                        DerniereRevision = DateTime.Parse(colonnes[13]),
                        GarantieRestante = int.Parse(colonnes[14]),
                        Assurance = bool.Parse(colonnes[15])
                    };

                    Vehicules.Add(vehicule);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de l'importation de la ligne {i + 1}: {ex.Message}");
                }
            }

            SauvegarderVehicules();
            Console.WriteLine("Importation des véhicules terminée.");
        }

        // Vous pouvez ajouter ici des méthodes pour ajouter, modifier ou supprimer un véhicule.

        /// <summary>
        /// Ajoute un nouveau véhicule et sauvegarde.
        /// </summary>
        public void AjouterVehicule(Vehicule vehicule)
        {
            Vehicules.Add(vehicule);
            SauvegarderVehicules();
            Console.WriteLine("Véhicule ajouté avec succès.");
        }

        /// <summary>
        /// Modifie un véhicule existant identifié par son VIN.
        /// </summary>
        public void ModifierVehicule(string VIN, Vehicule vehiculeMisAJour)
        {
            var vehicule = Vehicules.Find(v => v.VIN == VIN);
            if (vehicule != null)
            {
                vehicule.Marque = vehiculeMisAJour.Marque;
                vehicule.Modele = vehiculeMisAJour.Modele;
                vehicule.Annee = vehiculeMisAJour.Annee;
                vehicule.Categorie = vehiculeMisAJour.Categorie;
                vehicule.PrixApproximatif = vehiculeMisAJour.PrixApproximatif;
                vehicule.Kilometrage = vehiculeMisAJour.Kilometrage;
                vehicule.Couleur = vehiculeMisAJour.Couleur;
                vehicule.TypeDeCarburant = vehiculeMisAJour.TypeDeCarburant;
                vehicule.Transmission = vehiculeMisAJour.Transmission;
                vehicule.EtatGeneral = vehiculeMisAJour.EtatGeneral;
                vehicule.ProprietaireActuel = vehiculeMisAJour.ProprietaireActuel;
                vehicule.DateAchat = vehiculeMisAJour.DateAchat;
                vehicule.DerniereRevision = vehiculeMisAJour.DerniereRevision;
                vehicule.GarantieRestante = vehiculeMisAJour.GarantieRestante;
                vehicule.Assurance = vehiculeMisAJour.Assurance;
                SauvegarderVehicules();
                Console.WriteLine("Véhicule modifié avec succès.");
            }
            else
            {
                Console.WriteLine("Véhicule non trouvé.");
            }
        }

        /// <summary>
        /// Supprime un véhicule identifié par son VIN.
        /// </summary>
        public void SupprimerVehicule(string VIN)
        {
            var vehicule = Vehicules.Find(v => v.VIN == VIN);
            if (vehicule != null)
            {
                Vehicules.Remove(vehicule);
                SauvegarderVehicules();
                Console.WriteLine("Véhicule supprimé avec succès.");
            }
            else
            {
                Console.WriteLine("Véhicule non trouvé.");
            }
        }

    }

    /// <summary>
    /// Gestionnaire pour les réparations.
    /// </summary>
    public class GestionnaireReparations
    {
        public List<Reparation> Reparations { get; set; }
        private readonly string fichierReparations = "C:\\Users\\KOUONGA\\Documents\\Dossier Ecole\\UQAR\\POO\\PROJET1\\reparations.json";

        public GestionnaireReparations()
        {
            if (File.Exists(fichierReparations))
            {
                string json = File.ReadAllText(fichierReparations);
                Reparations = JsonConvert.DeserializeObject<List<Reparation>>(json);
            }
            else
            {
                Reparations = new List<Reparation>();
            }
        }

        public void SauvegarderReparations()
        {
            string json = JsonConvert.SerializeObject(Reparations, Formatting.Indented);
            File.WriteAllText(fichierReparations, json);
        }

        public void ImporterReparationsDepuisCSV(string cheminCSV)
        {
            if (!File.Exists(cheminCSV))
            {
                Console.WriteLine("Fichier CSV des réparations non trouvé.");
                return;
            }

            var lignes = File.ReadAllLines(cheminCSV);
            // Supposons que la première ligne contient les en-têtes.
            for (int i = 1; i < lignes.Length; i++)
            {
                var colonnes = lignes[i].Split(',');
                try
                {
                    Reparation reparation = new Reparation
                    {
                        Categorie = colonnes[0],
                        NomDePiece = colonnes[1],
                        PrixApprox = decimal.Parse(colonnes[2]),
                        Description = colonnes[3],
                        ReparationAssociee = colonnes[4]
                    };

                    Reparations.Add(reparation);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de l'importation de la ligne {i + 1}: {ex.Message}");
                }
            }

            SauvegarderReparations();
            Console.WriteLine("Importation des réparations terminée.");
        }


        // Vous pouvez ajouter ici des méthodes pour ajouter, modifier ou supprimer une réparation.
    }


    /// <summary>
    /// Gère l'enregistrement et la persistance des réparations effectuées.
    /// </summary>
    public class GestionnaireReparationsEffectuees
    {
        public List<ReparationEffectuee> ReparationsEffectuees { get; set; }
        private readonly string fichierReparationsEffectuees = "C:\\Users\\KOUONGA\\Documents\\Dossier Ecole\\UQAR\\POO\\PROJET1\\reparations_effectuees.json";

        public GestionnaireReparationsEffectuees()
        {
            if (File.Exists(fichierReparationsEffectuees))
            {
                string json = File.ReadAllText(fichierReparationsEffectuees);
                ReparationsEffectuees = JsonConvert.DeserializeObject<List<ReparationEffectuee>>(json);
            }
            else
            {
                ReparationsEffectuees = new List<ReparationEffectuee>();
            }
        }

        public void SauvegarderReparationsEffectuees()
        {
            string json = JsonConvert.SerializeObject(ReparationsEffectuees, Formatting.Indented);
            File.WriteAllText(fichierReparationsEffectuees, json);
        }

        public void AjouterReparationEffectuee(ReparationEffectuee rep)
        {
            ReparationsEffectuees.Add(rep);
            SauvegarderReparationsEffectuees();
            Console.WriteLine("Réparation enregistrée avec succès.");
        }

        public List<ReparationEffectuee> ObtenirReparationsParClient(string clientLogin)
        {
            return ReparationsEffectuees.FindAll(r => r.ClientLogin.Equals(clientLogin, StringComparison.OrdinalIgnoreCase));
        }
    }

    /// <summary>
    /// Gère l'enregistrement et la persistance des devis de réparation.
    /// </summary>
    public class GestionnaireDevisReparation
    {
        public List<DevisReparation> DevisReparations { get; set; }
        private readonly string fichierDevis = "C:\\Users\\KOUONGA\\Documents\\Dossier Ecole\\UQAR\\POO\\PROJET1\\devis_reparation.json";

        public GestionnaireDevisReparation()
        {
            if (File.Exists(fichierDevis))
            {
                string json = File.ReadAllText(fichierDevis);
                DevisReparations = JsonConvert.DeserializeObject<List<DevisReparation>>(json);
            }
            else
            {
                DevisReparations = new List<DevisReparation>();
            }
        }

        public void SauvegarderDevis()
        {
            string json = JsonConvert.SerializeObject(DevisReparations, Formatting.Indented);
            File.WriteAllText(fichierDevis, json);
        }

        public void AjouterDevis(DevisReparation devis)
        {
            DevisReparations.Add(devis);
            SauvegarderDevis();
            Console.WriteLine("Devis de réparation ajouté avec succès.");
        }
    }

    /// <summary>
    /// Gère l'enregistrement et la persistance des factures de réparation.
    /// </summary>
    public class GestionnaireFacturesReparation
    {
        public List<FactureReparation> FacturesReparation { get; set; }
        private readonly string fichierFacturesReparation = "C:\\Users\\KOUONGA\\Documents\\Dossier Ecole\\UQAR\\POO\\PROJET1\\factures_reparation.json";

        public GestionnaireFacturesReparation()
        {
            if (File.Exists(fichierFacturesReparation))
            {
                string json = File.ReadAllText(fichierFacturesReparation);
                FacturesReparation = JsonConvert.DeserializeObject<List<FactureReparation>>(json);
            }
            else
            {
                FacturesReparation = new List<FactureReparation>();
            }
        }

        public void SauvegarderFacturesReparation()
        {
            string json = JsonConvert.SerializeObject(FacturesReparation, Formatting.Indented);
            File.WriteAllText(fichierFacturesReparation, json);
        }

        public void AjouterFactureReparation(FactureReparation facture)
        {
            FacturesReparation.Add(facture);
            SauvegarderFacturesReparation();
            Console.WriteLine("Facture de réparation générée avec succès.");
        }
    }


    /// <summary>
    /// Classe représentant une facture pour une demande d'achat.
    /// </summary>
    public class Facture
    {
        public int Id { get; set; }
        public int DemandeAchatId { get; set; }
        public string ClientLogin { get; set; }
        public string VehicleVIN { get; set; }
        public DateTime DateFacture { get; set; }
        public decimal Montant { get; set; }
        public string StatutPaiement { get; set; } // "En attente" ou "Payé"
    }

    /// <summary>
    /// Gestionnaire pour les factures.
    /// </summary>
    public class GestionnaireFactures
    {
        public List<Facture> Factures { get; set; }
        private readonly string fichierFactures = "C:\\Users\\KOUONGA\\Documents\\Dossier Ecole\\UQAR\\POO\\PROJET1\\factures.json";

        public GestionnaireFactures()
        {
            if (File.Exists(fichierFactures))
            {
                string json = File.ReadAllText(fichierFactures);
                Factures = JsonConvert.DeserializeObject<List<Facture>>(json);
            }
            else
            {
                Factures = new List<Facture>();
            }
        }

        public void SauvegarderFactures()
        {
            string json = JsonConvert.SerializeObject(Factures, Formatting.Indented);
            File.WriteAllText(fichierFactures, json);
        }

        public void AjouterFacture(Facture facture)
        {
            Factures.Add(facture);
            SauvegarderFactures();
            Console.WriteLine("Facture générée et transmise au client.");
        }

        public List<Facture> ObtenirFacturesParClient(string clientLogin)
        {
            return Factures.FindAll(f => f.ClientLogin.Equals(clientLogin, StringComparison.OrdinalIgnoreCase));
        }
    }




    /// <summary>
    /// Programme principal simulant la connexion et la gestion des utilisateurs.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {

            // Instanciation des gestionnaires existants
            GestionnaireVehicules gestionnaireVehicules = new GestionnaireVehicules();
            GestionnaireReparations gestionnaireReparations = new GestionnaireReparations();
            GestionnaireAchats gestionnaireAchats = new GestionnaireAchats();
            GestionnaireFactures gestionnaireFactures = new GestionnaireFactures();
            GestionnairePieces gestionnairePieces = new GestionnairePieces();
            GestionnaireReparationsEffectuees gestionnaireReparationsEffectuees = new GestionnaireReparationsEffectuees();
            GestionnaireDevisReparation gestionnaireDevisReparation = new GestionnaireDevisReparation();
            GestionnaireFacturesReparation gestionnaireFacturesReparation = new GestionnaireFacturesReparation();

            // Importer depuis CSV uniquement si les fichiers JSON sont vides (première importation)
            if (gestionnaireVehicules.Vehicules.Count == 0)
            {
                gestionnaireVehicules.ImporterVehiculesDepuisCSV("C:\\Users\\KOUONGA\\Documents\\Dossier Ecole\\UQAR\\POO\\PROJET1\\vehicules_db.csv");
            }

            if (gestionnaireReparations.Reparations.Count == 0)
            {
                gestionnaireReparations.ImporterReparationsDepuisCSV("C:\\Users\\KOUONGA\\Documents\\Dossier Ecole\\UQAR\\POO\\PROJET1\\reparations_db.csv");
            }


            // Initialiser le gestionnaire des utilisateurs (charge depuis user.json s'il existe)
            GestionnaireUtilisateurs gestionnaire = new GestionnaireUtilisateurs();

            // Demander à l'utilisateur de se connecter
            Console.Write("Entrez votre login: ");
            string login = Console.ReadLine();

            // Récupérer l'utilisateur en fonction du login
            Utilisateur utilisateurConnecte = gestionnaire.ObtenirUtilisateurParLogin(login);

            // Si l'utilisateur n'existe pas, le créer avec un rôle par défaut (ici, Client)
            if (utilisateurConnecte == null)
            {
                Console.WriteLine("Utilisateur non trouvé. Création d'un nouvel utilisateur (rôle Client par défaut).");
                Console.Write("Entrez votre nom: ");
                string nom = Console.ReadLine();
                Console.Write("Entrez votre prénom: ");
                string prenom = Console.ReadLine();
                Console.Write("Entrez votre email: ");
                string email = Console.ReadLine();

                utilisateurConnecte = new Proprietaire
                {
                    User_ID = gestionnaire.Utilisateurs.Count + 1,
                    Login = login,
                    Nom = nom,
                    Prenom = prenom,
                    Email = email,
                    EstConnecte = false
                };
                gestionnaire.AjouterUtilisateur(utilisateurConnecte);
            }

            // Connexion de l'utilisateur
            utilisateurConnecte.SeConnecter();

            // Afficher et gérer le menu en fonction du rôle
            Menu menu = new Menu(utilisateurConnecte, 
                                 gestionnaire, 
                                 gestionnaireVehicules, 
                                 gestionnaireReparations, 
                                 gestionnaireAchats, 
                                 gestionnaireFactures, 
                                 gestionnairePieces,
                                 gestionnaireReparationsEffectuees, 
                                 gestionnaireDevisReparation, 
                                 gestionnaireFacturesReparation);
            menu.GererMenu();

            // Déconnexion et sauvegarde
            utilisateurConnecte.SeDeconnecter();
            gestionnaire.SauvegarderUtilisateurs();

            Console.WriteLine("Appuyez sur une touche pour quitter...");
            Console.ReadKey();
        }
    }
}
