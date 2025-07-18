using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace GestionRHv2.Classes
{
    public class Conjoint
    {
        public int Id { get; set; }  // ID auto-incrémenté de la table
        public int? Num { get; set; }  // Numéro du conjoint (1, 2, 3...)
        public string Matricule { get; set; }  // Matricule de l'agent
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Prof { get; set; }  // Profession
        public DateTime? DateMariage { get; set; }
        public bool? AllocationFamiliale { get; set; }

        // Propriétés supplémentaires pour l'affichage
        public string NomEmploye { get; set; }
        public string PrenomEmploye { get; set; }
    }

    public class ConjointManager
    {
        // Obtenir le prochain numéro de conjoint pour un agent
        private static int GetNextNumForAgent(string matricule)
        {
            string query = "SELECT ISNULL(MAX(num), 0) + 1 FROM [RH_DB].[dbo].[CONJOINT] WHERE matricule = @matricule";
            SqlParameter matriculeParam = new SqlParameter("@matricule", matricule);
            object result = Database.ExecuteScalar(query, matriculeParam);
            return result != null ? Convert.ToInt32(result) : 1;
        }

        // Obtenir tous les conjoints
        public static DataTable GetAllConjoints()
        {
            string query = @"
                SELECT c.*, 
                       a.nom AS nom_employe,
                       a.prenom AS prenom_employe,
                       CONCAT(a.nom, ' ', a.prenom) AS employe_complet
                FROM [RH_DB].[dbo].[CONJOINT] c
                INNER JOIN [RH_DB].[dbo].[AGENT] a ON c.matricule = a.matricule
                ORDER BY a.nom, a.prenom, c.num";

            return Database.GetData(query);
        }

        // Rechercher des conjoints
        public static DataTable SearchConjoints(string searchTerm)
        {
            string query = @"
                SELECT c.*, 
                       a.nom AS nom_employe,
                       a.prenom AS prenom_employe,
                       CONCAT(a.nom, ' ', a.prenom) AS employe_complet
                FROM [RH_DB].[dbo].[CONJOINT] c
                INNER JOIN [RH_DB].[dbo].[AGENT] a ON c.matricule = a.matricule
                WHERE c.nom LIKE @search 
                   OR c.prenom LIKE @search 
                   OR c.matricule LIKE @search
                   OR c.prof LIKE @search
                   OR a.nom LIKE @search
                   OR a.prenom LIKE @search
                ORDER BY a.nom, a.prenom, c.num";

            SqlParameter searchParam = new SqlParameter("@search", "%" + searchTerm + "%");
            return Database.GetDataWithParams(query, searchParam);
        }

        // Obtenir un conjoint par ID (supposant qu'il y a un champ ID)
        public static Conjoint GetConjointById(int id)
        {
            // Si la table n'a pas de champ ID, on peut utiliser matricule + num comme clé composite
            string query = @"
                SELECT c.*, 
                       a.nom AS nom_employe,
                       a.prenom AS prenom_employe
                FROM [RH_DB].[dbo].[CONJOINT] c
                INNER JOIN [RH_DB].[dbo].[AGENT] a ON c.matricule = a.matricule
                WHERE c.num = @id"; // Utiliser le num comme ID temporaire

            SqlParameter idParam = new SqlParameter("@id", id);
            DataTable dt = Database.GetDataWithParams(query, idParam);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new Conjoint
                {
                    Num = row["num"] != DBNull.Value ? Convert.ToInt32(row["num"]) : (int?)null,
                    Matricule = row["matricule"] != DBNull.Value ? row["matricule"].ToString() : null,
                    Nom = row["nom"] != DBNull.Value ? row["nom"].ToString() : null,
                    Prenom = row["prenom"] != DBNull.Value ? row["prenom"].ToString() : null,
                    Prof = row["prof"] != DBNull.Value ? row["prof"].ToString() : null,
                    DateMariage = row["date_m"] != DBNull.Value ? Convert.ToDateTime(row["date_m"]) : (DateTime?)null,
                    AllocationFamiliale = ConvertToBoolean(row["allocation_f"]),
                    NomEmploye = row["nom_employe"] != DBNull.Value ? row["nom_employe"].ToString() : null,
                    PrenomEmploye = row["prenom_employe"] != DBNull.Value ? row["prenom_employe"].ToString() : null
                };
            }
            return null;
        }

        // Obtenir un conjoint par matricule et num
        public static Conjoint GetConjointByMatriculeAndNum(string matricule, int num)
        {
            string query = @"
                SELECT c.*, 
                       a.nom AS nom_employe,
                       a.prenom AS prenom_employe
                FROM [RH_DB].[dbo].[CONJOINT] c
                INNER JOIN [RH_DB].[dbo].[AGENT] a ON c.matricule = a.matricule
                WHERE c.matricule = @matricule AND c.num = @num";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@matricule", matricule),
                new SqlParameter("@num", num)
            };

            DataTable dt = Database.GetDataWithParams(query, parameters);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new Conjoint
                {
                    Num = Convert.ToInt32(row["num"]),
                    Matricule = row["matricule"].ToString(),
                    Nom = row["nom"] != DBNull.Value ? row["nom"].ToString() : null,
                    Prenom = row["prenom"] != DBNull.Value ? row["prenom"].ToString() : null,
                    Prof = row["prof"] != DBNull.Value ? row["prof"].ToString() : null,
                    DateMariage = row["date_m"] != DBNull.Value ? Convert.ToDateTime(row["date_m"]) : (DateTime?)null,
                    AllocationFamiliale = ConvertToBoolean(row["allocation_f"]),
                    NomEmploye = row["nom_employe"] != DBNull.Value ? row["nom_employe"].ToString() : null,
                    PrenomEmploye = row["prenom_employe"] != DBNull.Value ? row["prenom_employe"].ToString() : null
                };
            }
            return null;
        }

        // Méthode helper pour convertir allocation_f en booléen
        private static bool? ConvertToBoolean(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;

            string strValue = value.ToString().Trim().ToUpper();

            // Gérer différents formats possibles
            switch (strValue)
            {
                case "1":
                case "O":
                case "OUI":
                case "Y":
                case "YES":
                case "TRUE":
                case "X":
                    return true;
                case "0":
                case "N":
                case "NON":
                case "NO":
                case "FALSE":
                case "":
                    return false;
                default:
                    return null;
            }
        }

        // Ajouter un conjoint
        public static int AddConjoint(Conjoint conjoint)
        {
            // Obtenir automatiquement le prochain numéro pour cet agent
            if (!string.IsNullOrEmpty(conjoint.Matricule))
            {
                conjoint.Num = GetNextNumForAgent(conjoint.Matricule);
            }

            string query = @"INSERT INTO [RH_DB].[dbo].[CONJOINT] 
                           (num, matricule, nom, prenom, prof, date_m, allocation_f) 
                           VALUES (@num, @matricule, @nom, @prenom, @prof, @date_m, @allocation_f)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@num", conjoint.Num.HasValue ? (object)conjoint.Num.Value : DBNull.Value),
                new SqlParameter("@matricule", string.IsNullOrEmpty(conjoint.Matricule) ? DBNull.Value : (object)conjoint.Matricule),
                new SqlParameter("@nom", string.IsNullOrEmpty(conjoint.Nom) ? DBNull.Value : (object)conjoint.Nom),
                new SqlParameter("@prenom", string.IsNullOrEmpty(conjoint.Prenom) ? DBNull.Value : (object)conjoint.Prenom),
                new SqlParameter("@prof", string.IsNullOrEmpty(conjoint.Prof) ? DBNull.Value : (object)conjoint.Prof),
                new SqlParameter("@date_m", conjoint.DateMariage.HasValue ? (object)conjoint.DateMariage.Value : DBNull.Value),
                new SqlParameter("@allocation_f", ConvertFromBoolean(conjoint.AllocationFamiliale))
            };

            return Database.ExecuteNonQuery(query, parameters);
        }

        // Mettre à jour un conjoint
        public static int UpdateConjoint(Conjoint conjoint)
        {
            string query = @"UPDATE [RH_DB].[dbo].[CONJOINT] 
                           SET nom = @nom, 
                               prenom = @prenom, 
                               prof = @prof,
                               date_m = @date_m, 
                               allocation_f = @allocation_f 
                           WHERE matricule = @matricule AND num = @num";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@matricule", conjoint.Matricule),
                new SqlParameter("@num", conjoint.Num),
                new SqlParameter("@nom", string.IsNullOrEmpty(conjoint.Nom) ? DBNull.Value : (object)conjoint.Nom),
                new SqlParameter("@prenom", string.IsNullOrEmpty(conjoint.Prenom) ? DBNull.Value : (object)conjoint.Prenom),
                new SqlParameter("@prof", string.IsNullOrEmpty(conjoint.Prof) ? DBNull.Value : (object)conjoint.Prof),
                new SqlParameter("@date_m", conjoint.DateMariage.HasValue ? (object)conjoint.DateMariage.Value : DBNull.Value),
                new SqlParameter("@allocation_f", ConvertFromBoolean(conjoint.AllocationFamiliale))
            };

            return Database.ExecuteNonQuery(query, parameters);
        }

        // Méthode helper pour convertir un booléen vers le format de la base de données
        private static object ConvertFromBoolean(bool? value)
        {
            if (!value.HasValue)
                return DBNull.Value;

            // Adapter selon le format attendu par votre base de données
            // Si la base attend "N" ou "X", utilisez :
            return value.Value ? "X" : "N";

            // Si la base attend 0 ou 1, utilisez :
            // return value.Value ? 1 : 0;
        }

        // Supprimer un conjoint
        public static int DeleteConjoint(string matricule, int num)
        {
            string query = "DELETE FROM [RH_DB].[dbo].[CONJOINT] WHERE matricule = @matricule AND num = @num";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@matricule", matricule),
                new SqlParameter("@num", num)
            };
            return Database.ExecuteNonQuery(query, parameters);
        }

        // Obtenir les conjoints d'un employé
        public static DataTable GetConjointsByEmployee(string matricule)
        {
            string query = @"
                SELECT c.*, 
                       a.nom AS nom_employe,
                       a.prenom AS prenom_employe,
                       CONCAT(a.nom, ' ', a.prenom) AS employe_complet
                FROM [RH_DB].[dbo].[CONJOINT] c
                INNER JOIN [RH_DB].[dbo].[AGENT] a ON c.matricule = a.matricule
                WHERE c.matricule = @matricule
                ORDER BY c.num";

            SqlParameter matriculeParam = new SqlParameter("@matricule", matricule);
            return Database.GetDataWithParams(query, matriculeParam);
        }

        // Statistiques
        public static int GetTotalConjoints()
        {
            string query = "SELECT COUNT(*) FROM [RH_DB].[dbo].[CONJOINT]";
            object result = Database.ExecuteScalar(query);
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public static int GetConjointsWithAllocation()
        {
            string query = "SELECT COUNT(*) FROM [RH_DB].[dbo].[CONJOINT] WHERE allocation_f IN ('X', '1', 'O', 'OUI')";
            object result = Database.ExecuteScalar(query);
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public static int GetConjointsWithoutAllocation()
        {
            string query = "SELECT COUNT(*) FROM [RH_DB].[dbo].[CONJOINT] WHERE allocation_f IN ('N', '0', 'NON') OR allocation_f IS NULL";
            object result = Database.ExecuteScalar(query);
            return result != null ? Convert.ToInt32(result) : 0;
        }

        // Obtenir la liste des employés pour le dropdown
        public static DataTable GetEmployeesForDropdown()
        {
            string query = @"SELECT matricule, 
                                   CONCAT(nom, ' ', prenom, ' - ', matricule) AS nom_complet 
                           FROM [RH_DB].[dbo].[AGENT] 
                           ORDER BY nom, prenom";

            return Database.GetData(query);
        }

        // Obtenir les employés sans conjoint
        public static DataTable GetEmployeesWithoutConjoint()
        {
            string query = @"
                SELECT a.matricule,
                       CONCAT(a.nom, ' ', a.prenom, ' - ', a.matricule) AS nom_complet
                FROM [RH_DB].[dbo].[AGENT] a
                WHERE NOT EXISTS (
                    SELECT 1 FROM [RH_DB].[dbo].[CONJOINT] c 
                    WHERE c.matricule = a.matricule
                )
                ORDER BY a.nom, a.prenom";

            return Database.GetData(query);
        }
    }
}