using System;
using System.Data;
using System.Data.SqlClient;

namespace GestionRHv2.Classes
{
    public class AgentFonctionManager
    {
        // ✅ Obtenir toutes les fonctions d'agents
        public static DataTable GetAllAgentsFonctions()
        {
            try
            {
                string query = @"SELECT [num_ord], [matricule], [nom], [prenom], 
                                [affectation], [fonction]
                                FROM dbo.agent_fonction
                                ORDER BY [num_ord]";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Obtenir une fonction par numéro d'ordre
        public static DataTable GetAgentFonctionByNumOrd(int numOrd)
        {
            try
            {
                string query = $"SELECT * FROM dbo.agent_fonction WHERE [num_ord] = {numOrd}";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Obtenir les fonctions d'un agent spécifique
        public static DataTable GetFonctionsByMatricule(string matricule)
        {
            try
            {
                string query = $@"SELECT * FROM dbo.agent_fonction 
                                 WHERE [matricule] = '{matricule.Replace("'", "''")}'
                                 ORDER BY [num_ord]";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Obtenir le prochain numéro d'ordre
        public static int GetNextNumOrd()
        {
            try
            {
                string query = "SELECT ISNULL(MAX([num_ord]), 0) + 1 FROM dbo.agent_fonction";
                DataTable dt = Database.GetData(query);
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            catch
            {
                return 1;
            }
        }

        // ✅ Vérifier si un numéro d'ordre existe
        public static bool NumOrdExists(int numOrd)
        {
            try
            {
                string query = $"SELECT COUNT(*) FROM dbo.agent_fonction WHERE [num_ord] = {numOrd}";
                DataTable dt = Database.GetData(query);
                return dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0;
            }
            catch
            {
                return false;
            }
        }

        // ✅ Ajouter une nouvelle fonction d'agent
        public static bool AjouterAgentFonction(int numOrd, string matricule, string nom,
                                               string prenom, string affectation, string fonction)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO dbo.agent_fonction 
                                    ([num_ord], [matricule], [nom], [prenom], [affectation], [fonction]) 
                                    VALUES (@numOrd, @matricule, @nom, @prenom, @affectation, @fonction)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@numOrd", numOrd);
                    cmd.Parameters.AddWithValue("@matricule", matricule?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@nom", nom?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@prenom", prenom?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@affectation", affectation?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@fonction", fonction?.Trim() ?? "");

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

        // ✅ Modifier une fonction d'agent
        public static bool ModifierAgentFonction(int numOrd, string matricule, string nom,
                                                string prenom, string affectation, string fonction)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE dbo.agent_fonction SET
                                    [matricule] = @matricule,
                                    [nom] = @nom,
                                    [prenom] = @prenom,
                                    [affectation] = @affectation,
                                    [fonction] = @fonction
                                    WHERE [num_ord] = @numOrd";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@numOrd", numOrd);
                    cmd.Parameters.AddWithValue("@matricule", matricule?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@nom", nom?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@prenom", prenom?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@affectation", affectation?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@fonction", fonction?.Trim() ?? "");

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

        // ✅ Supprimer une fonction d'agent
        public static bool SupprimerAgentFonction(int numOrd)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM dbo.agent_fonction WHERE [num_ord] = @numOrd";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@numOrd", numOrd);

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

        // ✅ Rechercher des fonctions
        public static DataTable RechercherAgentsFonctions(string recherche = "", string fonctionFiltre = "")
        {
            try
            {
                string query = @"SELECT [num_ord], [matricule], [nom], [prenom], 
                                [affectation], [fonction]
                                FROM dbo.agent_fonction
                                WHERE 1=1";

                // Recherche générale
                if (!string.IsNullOrEmpty(recherche))
                {
                    recherche = recherche.Replace("'", "''");
                    query += $@" AND ([matricule] LIKE '%{recherche}%' 
                               OR [nom] LIKE '%{recherche}%' 
                               OR [prenom] LIKE '%{recherche}%'
                               OR [affectation] LIKE '%{recherche}%')";
                }

                // Filtre par fonction
                if (!string.IsNullOrEmpty(fonctionFiltre))
                {
                    query += $" AND [fonction] LIKE '%{fonctionFiltre.Replace("'", "''")}%'";
                }

                query += " ORDER BY [num_ord]";

                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Obtenir la liste des fonctions distinctes
        public static DataTable GetFonctionsDistinctes()
        {
            try
            {
                string query = @"SELECT DISTINCT [fonction] 
                                FROM dbo.agent_fonction 
                                WHERE [fonction] IS NOT NULL AND [fonction] != ''
                                ORDER BY [fonction]";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Obtenir les statistiques
        public static DataTable GetStatistiques()
        {
            try
            {
                string query = @"SELECT 
                                COUNT(*) as TotalAgents,
                                COUNT(DISTINCT [fonction]) as NbFonctions,
                                COUNT(DISTINCT [affectation]) as NbAffectations
                                FROM dbo.agent_fonction";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Obtenir liste des agents pour dropdown (depuis la table Agent)
        public static DataTable GetAgentsForDropdown()
        {
            try
            {
                string query = @"SELECT [matricule], 
                                CONCAT([matricule], ' - ', [nom], ' ', [prenom]) as agent_display
                                FROM dbo.Agent 
                                ORDER BY [nom], [prenom]";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }
    }
}