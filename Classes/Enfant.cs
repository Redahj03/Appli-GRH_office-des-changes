using System;

namespace GestionRHv2.Classes
{
    public class Enfant
    {
        public string Matricule { get; set; }
        public int Num { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Sexe { get; set; }
        public string Scolarise { get; set; }
        public DateTime? DateNaissance { get; set; }
        public string LieuNaissance { get; set; }
        public string Ch { get; set; }
        public string CodSc { get; set; }
        public DateTime? DatePSc { get; set; }

        // Propriétés calculées pour l'affichage
        public string NomComplet => $"{Nom} {Prenom}".Trim();

        public int Age
        {
            get
            {
                if (DateNaissance.HasValue)
                {
                    var today = DateTime.Today;
                    var age = today.Year - DateNaissance.Value.Year;
                    if (DateNaissance.Value.Date > today.AddYears(-age)) age--;
                    return age;
                }
                return 0;
            }
        }

        public string SexeLibelle => Sexe == "M" ? "Masculin" : Sexe == "F" ? "Féminin" : "";
        public string ScolariseLibelle => Scolarise == "O" ? "Oui" : "Non";

        // Constructeurs
        public Enfant()
        {
            Scolarise = "N"; // Valeur par défaut
        }

        public Enfant(string matricule, string nom, string prenom, string sexe, DateTime? dateNaissance)
        {
            Matricule = matricule;
            Nom = nom;
            Prenom = prenom;
            Sexe = sexe;
            DateNaissance = dateNaissance;
            Scolarise = "N";
        }
    }
}