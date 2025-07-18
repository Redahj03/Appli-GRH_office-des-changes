using System;
using System.Collections.Generic;

namespace GestionRHv2.Classes
{
    public static class MissionHelper
    {
        // ==================== TAUX KILOMÉTRIQUES PAR PUISSANCE FISCALE ====================
        // Basé sur la table Taux_Voiture de votre base de données

        private static readonly Dictionary<int, decimal> TauxKilometriques = new Dictionary<int, decimal>
        {
            { 4, 1.20m },   // 4 CV et moins
            { 5, 1.20m },   // 5 CV
            { 6, 1.20m },   // 6 CV
            { 7, 1.75m },   // 7 CV
            { 8, 1.75m },   // 8 CV
            { 9, 1.75m },   // 9 CV
            { 10, 2.30m },  // 10 CV
            { 11, 2.30m },  // 11 CV
            { 12, 2.30m },  // 12 CV
            { 13, 2.30m },  // 13 CV
            { 14, 2.30m },  // 14 CV et plus
            { 0, 0.00m }    // Valeur par défaut
        };

        // ✅ Obtenir le taux kilométrique par puissance fiscale
        public static decimal GetTauxKilometrique(int puissanceFiscale)
        {
            if (puissanceFiscale <= 4) return TauxKilometriques[4];
            if (puissanceFiscale >= 14) return TauxKilometriques[14];

            return TauxKilometriques.ContainsKey(puissanceFiscale)
                ? TauxKilometriques[puissanceFiscale]
                : 0.00m;
        }

        // ==================== CALCULS FINANCIERS ====================

        // ✅ Calculer les indemnités kilométriques
        public static decimal CalculerIndemnitesKilometriques(float kilometrage, int puissanceFiscale)
        {
            decimal taux = GetTauxKilometrique(puissanceFiscale);
            return (decimal)kilometrage * taux;
        }

        // ✅ Calculer les indemnités journalières
        public static decimal CalculerIndemnitesJournalieres(int nombreJours, decimal tauxJournalier)
        {
            return nombreJours * tauxJournalier;
        }

        // ✅ Calculer le montant total d'une mission
        public static decimal CalculerMontantTotalMission(float kilometrage, int puissanceFiscale,
            int nombreJours, decimal tauxJournalier, int nombreVacations = 0, decimal tauxVacation = 0)
        {
            decimal montantKm = CalculerIndemnitesKilometriques(kilometrage, puissanceFiscale);
            decimal montantJours = CalculerIndemnitesJournalieres(nombreJours, tauxJournalier);
            decimal montantVacations = nombreVacations * tauxVacation;

            return montantKm + montantJours + montantVacations;
        }

        // ==================== CONVERSIONS ET FORMATAGE ====================

        // ✅ Convertir un montant en lettres (français)
        public static string ConvertirMontantEnLettres(decimal montant)
        {
            if (montant == 0) return "zéro dirham";

            int partieEntiere = (int)montant;
            int centimes = (int)((montant - partieEntiere) * 100);

            string resultat = ConvertirNombreEnLettres(partieEntiere) + " dirham";

            if (partieEntiere > 1)
                resultat += "s";

            if (centimes > 0)
            {
                resultat += " et " + ConvertirNombreEnLettres(centimes) + " centime";
                if (centimes > 1)
                    resultat += "s";
            }

            return Capitaliser(resultat);
        }

        // ✅ Convertir un nombre en lettres
        private static string ConvertirNombreEnLettres(int nombre)
        {
            if (nombre == 0) return "zéro";

            string[] unites = { "", "un", "deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf" };
            string[] dizaines = { "", "dix", "vingt", "trente", "quarante", "cinquante", "soixante", "soixante-dix", "quatre-vingt", "quatre-vingt-dix" };
            string[] especiales = { "dix", "onze", "douze", "treize", "quatorze", "quinze", "seize", "dix-sept", "dix-huit", "dix-neuf" };

            if (nombre < 10)
                return unites[nombre];

            if (nombre >= 10 && nombre < 20)
                return especiales[nombre - 10];

            if (nombre < 100)
            {
                int d = nombre / 10;
                int u = nombre % 10;

                if (d == 7 || d == 9)
                {
                    d--;
                    u += 10;
                }

                string resultat = dizaines[d];
                if (u > 0)
                {
                    if (u == 1 && d != 8)
                        resultat += " et " + unites[u];
                    else if (u < 10)
                        resultat += "-" + unites[u];
                    else
                        resultat += "-" + especiales[u - 10];
                }

                return resultat;
            }

            if (nombre < 1000)
            {
                int c = nombre / 100;
                int r = nombre % 100;

                string resultat = "";
                if (c == 1)
                    resultat = "cent";
                else
                    resultat = unites[c] + " cent";

                if (r > 0)
                {
                    if (c > 1 && r == 0)
                        resultat += "s";
                    else
                        resultat += " " + ConvertirNombreEnLettres(r);
                }

                return resultat;
            }

            if (nombre < 1000000)
            {
                int m = nombre / 1000;
                int r = nombre % 1000;

                string resultat = "";
                if (m == 1)
                    resultat = "mille";
                else
                    resultat = ConvertirNombreEnLettres(m) + " mille";

                if (r > 0)
                    resultat += " " + ConvertirNombreEnLettres(r);

                return resultat;
            }

            return nombre.ToString(); // Pour les très grands nombres
        }

        // ✅ Capitaliser la première lettre
        private static string Capitaliser(string texte)
        {
            if (string.IsNullOrEmpty(texte)) return texte;
            return char.ToUpper(texte[0]) + texte.Substring(1);
        }

        // ==================== VALIDATION ====================

        // ✅ Valider les dates d'une mission
        public static bool ValiderDatesMission(DateTime dateDepart, DateTime dateRetour)
        {
            // La date de retour doit être après ou égale à la date de départ
            if (dateRetour < dateDepart)
                return false;

            // La mission ne doit pas dépasser 30 jours (règle business exemple)
            if ((dateRetour - dateDepart).TotalDays > 30)
                return false;

            return true;
        }

        // ✅ Valider le kilométrage
        public static bool ValiderKilometrage(float kilometrage)
        {
            // Le kilométrage doit être positif et raisonnable (max 5000 km par mission)
            return kilometrage > 0 && kilometrage <= 5000;
        }

        // ==================== CODES MOYENS DE TRANSPORT ====================

        public static string GetCodeTransport(string libelleTransport)
        {
            switch (libelleTransport.ToLower())
            {
                case "voiture de service":
                case "service":
                    return "S";

                case "voiture personnelle":
                case "personnelle":
                    return "V";

                case "train":
                    return "T";

                case "avion":
                    return "A";

                default:
                    return "";
            }
        }

        public static string GetLibelleTransport(string codeTransport)
        {
            switch (codeTransport?.ToUpper())
            {
                case "S":
                    return "Voiture de service";
                case "V":
                    return "Voiture personnelle";
                case "T":
                    return "Train";
                case "A":
                    return "Avion";
                default:
                    return "Non spécifié";
            }
        }

        // ==================== FORMATAGE POUR AFFICHAGE ====================

        // ✅ Formater la durée d'une mission
        public static string FormatDureeMission(DateTime dateDepart, DateTime dateRetour)
        {
            int jours = (dateRetour - dateDepart).Days + 1;

            if (jours == 1)
                return "1 jour";
            else
                return jours + " jours";
        }

        // ✅ Formater une période
        public static string FormatPeriodeMission(DateTime dateDepart, DateTime dateRetour)
        {
            if (dateDepart.Date == dateRetour.Date)
                return dateDepart.ToString("dd/MM/yyyy");
            else
                return string.Format("Du {0:dd/MM/yyyy} au {1:dd/MM/yyyy}", dateDepart, dateRetour);
        }

        // ==================== GÉNÉRATION DE RÉFÉRENCES ====================

        // ✅ Générer une référence unique pour un ordre de mission
        public static string GenererReferenceMission(int id, DateTime date)
        {
            return string.Format("OM-{0}-{1:yyyyMMdd}", id.ToString("D5"), date);
        }

        // ==================== STATISTIQUES ====================

        // ✅ Calculer le coût moyen par jour d'une mission
        public static decimal CalculerCoutMoyenParJour(decimal montantTotal, int nombreJours)
        {
            if (nombreJours == 0) return 0;
            return montantTotal / nombreJours;
        }

        // ✅ Calculer le kilométrage moyen par mission
        public static float CalculerKilometrageMoyen(float totalKilometrage, int nombreMissions)
        {
            if (nombreMissions == 0) return 0;
            return totalKilometrage / nombreMissions;
        }
    }
}