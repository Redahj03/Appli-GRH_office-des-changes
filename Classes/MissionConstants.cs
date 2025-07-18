namespace GestionRHv2.Classes
{
    /// <summary>
    /// Constantes utilisées dans le module de gestion des missions
    /// </summary>
    public static class MissionConstants
    {
        // ==================== CODES MOYENS DE TRANSPORT ====================
        public static class TransportCodes
        {
            public const string VOITURE_SERVICE = "S";
            public const string VOITURE_PERSONNELLE = "V";
            public const string TRAIN = "T";
            public const string AVION = "A";
        }

        // ==================== LIBELLÉS MOYENS DE TRANSPORT ====================
        public static class TransportLabels
        {
            public const string VOITURE_SERVICE = "Voiture de service";
            public const string VOITURE_PERSONNELLE = "Voiture personnelle";
            public const string TRAIN = "Train";
            public const string AVION = "Avion";
        }

        // ==================== LIMITES ET RÈGLES BUSINESS ====================
        public static class BusinessRules
        {
            // Durée maximale d'une mission en jours
            public const int DUREE_MAX_MISSION = 30;

            // Kilométrage maximum par mission
            public const float KILOMETRAGE_MAX = 5000f;

            // Montant maximum des indemnités journalières
            public const decimal INDEMNITE_JOUR_MAX = 500m;

            // Délai minimum entre deux missions (en jours)
            public const int DELAI_MIN_ENTRE_MISSIONS = 1;
        }

        // ==================== TAUX PAR DÉFAUT ====================
        public static class TauxDefaut
        {
            // Taux journalier par défaut
            public const decimal TAUX_JOURNALIER_DEFAUT = 250m;

            // Taux de vacation par défaut
            public const decimal TAUX_VACATION_DEFAUT = 100m;
        }

        // ==================== MESSAGES ====================
        public static class Messages
        {
            // Messages de succès
            public const string MISSION_AJOUTEE = "Mission enregistrée avec succès!";
            public const string MISSION_MODIFIEE = "Mission modifiée avec succès!";
            public const string MISSION_SUPPRIMEE = "Mission supprimée avec succès!";

            // Messages d'erreur
            public const string ERREUR_MISSION_EXISTE = "Une mission existe déjà pour cet agent à cette date!";
            public const string ERREUR_DATES_INVALIDES = "La date de retour doit être après la date de départ!";
            public const string ERREUR_DUREE_EXCESSIVE = "La durée de la mission ne peut pas dépasser 30 jours!";
            public const string ERREUR_KILOMETRAGE_INVALIDE = "Le kilométrage doit être compris entre 1 et 5000 km!";
            public const string ERREUR_AGENT_INTROUVABLE = "Agent non trouvé avec ce matricule!";
            public const string ERREUR_ENREGISTREMENT = "Erreur lors de l'enregistrement de la mission.";
            public const string ERREUR_SUPPRESSION = "Erreur lors de la suppression de la mission.";

            // Messages de validation
            public const string CHAMPS_OBLIGATOIRES = "Veuillez remplir tous les champs obligatoires!";
            public const string CONFIRMER_SUPPRESSION = "Êtes-vous sûr de vouloir supprimer cette mission?";
        }

        // ==================== FORMATS D'AFFICHAGE ====================
        public static class Formats
        {
            public const string FORMAT_DATE = "dd/MM/yyyy";
            public const string FORMAT_DATE_HEURE = "dd/MM/yyyy HH:mm";
            public const string FORMAT_HEURE = "HH:mm";
            public const string FORMAT_MONTANT = "N2";
            public const string FORMAT_REFERENCE = "OM-{0:D5}-{1:yyyyMMdd}";
        }

        // ==================== NOMS DES SESSIONS/VIEWSTATE ====================
        public static class SessionKeys
        {
            public const string MISSION_ID = "MissionID";
            public const string MATRICULE_AGENT = "MatriculeAgent";
            public const string MODE_EDITION = "ModeEdition";
        }

        // ==================== PAGES DU MODULE ====================
        public static class Pages
        {
            public const string LISTE_MISSIONS = "~/Pages/Missions/ListeMissions.aspx";
            public const string AJOUTER_MISSION = "~/Pages/Missions/AjouterMission.aspx";
            public const string MODIFIER_MISSION = "~/Pages/Missions/ModifierMission.aspx";
            public const string DETAILS_MISSION = "~/Pages/Missions/DetailsMission.aspx";
            public const string IMPRIMER_MISSION = "~/Pages/Missions/ImprimerMission.aspx";
            public const string VALIDATION_MISSIONS = "~/Pages/Missions/ValidationMissions.aspx";
            public const string STATISTIQUES_MISSIONS = "~/Pages/Missions/StatistiquesMissions.aspx";
        }

        // ==================== PUISSANCES FISCALES ====================
        public static class PuissancesFiscales
        {
            public static readonly int[] VALEURS = { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };

            public static string GetLibelle(int puissance)
            {
                if (puissance <= 4) return "4 CV et moins";
                if (puissance >= 11) return "11 CV et plus";
                return puissance + " CV";
            }
        }

        // ==================== CSS CLASSES ====================
        public static class CssClasses
        {
            // Alertes
            public const string ALERT_SUCCESS = "alert alert-success";
            public const string ALERT_WARNING = "alert alert-warning";
            public const string ALERT_DANGER = "alert alert-danger";
            public const string ALERT_INFO = "alert alert-info";

            // Boutons
            public const string BTN_PRIMARY = "btn btn-primary";
            public const string BTN_SUCCESS = "btn btn-success";
            public const string BTN_WARNING = "btn btn-warning";
            public const string BTN_DANGER = "btn btn-danger";
            public const string BTN_SECONDARY = "btn btn-secondary";

            // Tables
            public const string TABLE_RESPONSIVE = "table table-striped table-hover";
        }
    }
}