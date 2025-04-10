using System;
using Newtonsoft.Json;

namespace AppGestionGarage.GestionPieces
{

    public class PieceDetachee
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Categorie { get; set; }
        public decimal Prix { get; set; }
        public int Quantite { get; set; }
        public string Description { get; set; }
        public DateTime DateApprovisionnement { get; set; }
        public string FournisseurLogin { get; set; }
    }

    /// <summary>
    /// Représente une pièce utilisée dans une réparation, avec la quantité utilisée.
    /// </summary>
    public class PieceUtilisee
    {
        public PieceDetachee Piece { get; set; }
        public int QuantiteUtilisee { get; set; }
    }

    public class GestionnairePieces
    {
        public List<PieceDetachee> Pieces { get; set; }
        private readonly string fichierPieces = "C:\\Users\\KOUONGA\\Documents\\Dossier Ecole\\UQAR\\POO\\PROJET1\\pieces.json";

        public GestionnairePieces()
        {
            if (File.Exists(fichierPieces))
            {
                string json = File.ReadAllText(fichierPieces);
                Pieces = JsonConvert.DeserializeObject<List<PieceDetachee>>(json);
            }
            else
            {
                Pieces = new List<PieceDetachee>();
            }
        }

        public void SauvegarderPieces()
        {
            string json = JsonConvert.SerializeObject(Pieces, Formatting.Indented);
            File.WriteAllText(fichierPieces, json);
        }

        public void AjouterPiece(PieceDetachee piece)
        {
            Pieces.Add(piece);
            SauvegarderPieces();
            Console.WriteLine("Pièce détachée ajoutée avec succès.");
        }

        // D'autres méthodes (modifier, supprimer) pourront être ajoutées ultérieurement.
    }
}