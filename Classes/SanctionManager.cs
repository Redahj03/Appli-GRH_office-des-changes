using System;
using System.Data;
using System.Data.SqlClient;

namespace GestionRHv2.Classes
{
    public class SanctionManager
    {
        // ========== GESTION DES TYPES DE SANCTIONS ==========

        // ✅ Obtenir tous les types de sanctions
        public static DataTable GetAllTypesSanctions()
        {
            try
            {
                string query = @"SELECT 
                                code_SC,
                                libelle_sc,
                                jours
                                FROM dbo.sanctions
                                ORDER BY code_SC";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Obtenir un type de sanction par code
        public static DataTable GetTypeSanctionByCode(int codeSC)
        {
            try
            {
                string query = $@"SELECT * FROM dbo.sanctions 
                                 WHERE code_SC = {codeSC}";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Ajouter un nouveau type de sanction
        public static bool AjouterTypeSanction(int codeSC, string libelle, int jours)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO dbo.sanctions 
                                    (code_SC, libelle_sc, jours) 
                                    VALUES (@codeSC, @libelle, @jours)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@codeSC", codeSC);
                    cmd.Parameters.AddWithValue("@libelle", libelle?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@jours", jours);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        // ✅ Modifier un type de sanction
        public static bool ModifierTypeSanction(int codeSC, string libelle, int jours)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE dbo.sanctions SET
                                    libelle_sc = @libelle,
                                    jours = @jours
                                    WHERE code_SC = @codeSC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@codeSC", codeSC);
                    cmd.Parameters.AddWithValue("@libelle", libelle?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@jours", jours);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        // ✅ Supprimer un type de sanction
        public static bool SupprimerTypeSanction(int codeSC)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Vérifier d'abord si le type est utilisé
                    string checkQuery = "SELECT COUNT(*) FROM dbo.AGENT_SC WHERE code_sc = @codeSC";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@codeSC", codeSC);

                    conn.Open();
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                        return false; // Ne pas supprimer si utilisé

                    string query = "DELETE FROM dbo.sanctions WHERE code_SC = @codeSC";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@codeSC", codeSC);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        // ========== GESTION DES SANCTIONS DES AGENTS ==========

        // ✅ Obtenir toutes les sanctions avec détails
        public static DataTable GetAllSanctionsAgents()
        {
            try
            {
                string query = @"
                    SELECT ags.ID, 
                           ags.matricule, 
                           a.nom + ' ' + a.prenom AS NomComplet,
                           ags.code_sc, 
                           s.libelle_sc AS TypeSanction,
                           ags.date, 
                           ags.duree
                    FROM dbo.AGENT_SC ags
                    INNER JOIN dbo.AGENT a ON ags.matricule = a.matricule
                    INNER JOIN dbo.sanctions s ON ags.code_sc = s.code_SC
                    ORDER BY ags.date DESC";

                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Obtenir les sanctions d'un agent spécifique
        public static DataTable GetSanctionsByAgent(string matricule)
        {
            try
            {
                string query = $@"
                    SELECT ags.ID, 
                           ags.code_sc, 
                           s.libelle_sc AS TypeSanction,
                           ags.date, 
                           ags.duree
                    FROM dbo.AGENT_SC ags
                    INNER JOIN dbo.sanctions s ON ags.code_sc = s.code_SC
                    WHERE ags.matricule = '{matricule.Replace("'", "''")}'
                    ORDER BY ags.date DESC";

                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Obtenir les agents sanctionnés par type de sanction
        public static DataTable GetAgentsBySanctionType(int codeSC)
        {
            try
            {
                string query = $@"
                    SELECT ags.ID, 
                           ags.matricule,
                           a.nom + ' ' + a.prenom AS NomComplet,
                           ags.date, 
                           ags.duree
                    FROM dbo.AGENT_SC ags
                    INNER JOIN dbo.AGENT a ON ags.matricule = a.matricule
                    WHERE ags.code_sc = {codeSC}
                    ORDER BY ags.date DESC";

                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Attribuer une sanction à un agent
        public static bool AttribuerSanction(string matricule, int codeSC, DateTime date, int duree)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO dbo.AGENT_SC 
                                    (matricule, code_sc, date, duree) 
                                    VALUES (@matricule, @codeSC, @date, @duree)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@matricule", matricule);
                    cmd.Parameters.AddWithValue("@codeSC", codeSC);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@duree", duree);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        // ✅ Supprimer une sanction d'un agent
        public static bool SupprimerSanctionAgent(int id)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM dbo.AGENT_SC WHERE ID = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        // ✅ Rechercher des sanctions
        public static DataTable RechercherSanctions(string searchTerm)
        {
            try
            {
                string query = $@"
                    SELECT ags.ID, 
                           ags.matricule, 
                           a.nom + ' ' + a.prenom AS NomComplet,
                           ags.code_sc, 
                           s.libelle_sc AS TypeSanction,
                           ags.date, 
                           ags.duree
                    FROM dbo.AGENT_SC ags
                    INNER JOIN dbo.AGENT a ON ags.matricule = a.matricule
                    INNER JOIN dbo.sanctions s ON ags.code_sc = s.code_SC
                    WHERE a.matricule LIKE '%{searchTerm.Replace("'", "''")}%' 
                       OR a.nom LIKE '%{searchTerm.Replace("'", "''")}%' 
                       OR a.prenom LIKE '%{searchTerm.Replace("'", "''")}%'
                       OR s.libelle_sc LIKE '%{searchTerm.Replace("'", "''")}%'
                    ORDER BY ags.date DESC";

                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Obtenir les statistiques des sanctions
        public static DataTable GetStatistiquesSanctions()
        {
            try
            {
                string query = @"
                    SELECT s.libelle_sc AS TypeSanction, 
                           COUNT(*) AS Nombre,
                           s.code_SC
                    FROM dbo.AGENT_SC ags
                    INNER JOIN dbo.sanctions s ON ags.code_sc = s.code_SC
                    GROUP BY s.libelle_sc, s.code_SC
                    ORDER BY COUNT(*) DESC";

                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Vérifier si un code de sanction existe déjà
        public static bool CodeSanctionExiste(int codeSC)
        {
            try
            {
                string query = $"SELECT COUNT(*) FROM dbo.sanctions WHERE code_SC = {codeSC}";
                DataTable dt = Database.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(dt.Rows[0][0]) > 0;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}