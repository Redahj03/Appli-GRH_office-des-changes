using System;
using System.Data;
using System.Data.SqlClient;

namespace GestionRHv2.Classes
{
    public class ResponsabiliteManager
    {
        // ==================== GESTION DES TYPES DE RESPONSABILITÉS ====================

        // ✅ Obtenir tous les types de responsabilités
        public static DataTable GetAllResponsabilites()
        {
            try
            {
                string query = @"SELECT [CODE_RS], [LIBELLE]
                                FROM dbo.responsabilite
                                ORDER BY [CODE_RS]";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Obtenir une responsabilité par code
        public static DataTable GetResponsabiliteByCode(int codeRS)
        {
            try
            {
                string query = $"SELECT * FROM dbo.responsabilite WHERE [CODE_RS] = {codeRS}";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Vérifier si un code existe
        public static bool CodeResponsabiliteExists(int codeRS)
        {
            try
            {
                string query = $"SELECT COUNT(*) FROM dbo.responsabilite WHERE [CODE_RS] = {codeRS}";
                DataTable dt = Database.GetData(query);
                return dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0;
            }
            catch
            {
                return false;
            }
        }

        // ✅ Obtenir le prochain code disponible
        public static int GetNextCodeResponsabilite()
        {
            try
            {
                string query = "SELECT ISNULL(MAX([CODE_RS]), 0) + 1 FROM dbo.responsabilite";
                DataTable dt = Database.GetData(query);
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            catch
            {
                return 1;
            }
        }

        // ✅ Ajouter un type de responsabilité
        public static bool AjouterResponsabilite(int codeRS, string libelle)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO dbo.responsabilite ([CODE_RS], [LIBELLE]) 
                                    VALUES (@codeRS, @libelle)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@codeRS", codeRS);
                    cmd.Parameters.AddWithValue("@libelle", libelle?.Trim() ?? "");

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

        // ✅ Modifier un type de responsabilité
        public static bool ModifierResponsabilite(int codeRS, string libelle)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE dbo.responsabilite SET [LIBELLE] = @libelle 
                                    WHERE [CODE_RS] = @codeRS";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@codeRS", codeRS);
                    cmd.Parameters.AddWithValue("@libelle", libelle?.Trim() ?? "");

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

        // ✅ Supprimer un type de responsabilité
        public static bool SupprimerResponsabilite(int codeRS)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM dbo.responsabilite WHERE [CODE_RS] = @codeRS";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@codeRS", codeRS);

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

        // ==================== GESTION DES RESPONSABILITÉS D'AGENTS (Table agent_resp) ====================

        // ✅ Obtenir toutes les responsabilités des agents (table agent_resp)
        public static DataTable GetAllAgentsResponsabilites()
        {
            try
            {
                string query = @"SELECT 
                                ar.[matricule], 
                                a.[nom], 
                                a.[prenom],
                                ar.[code_res],
                                r.[LIBELLE] as responsabilite,
                                ar.[date_r],
                                ar.[code_a],
                                ar.[Actuel],
                                'agent_resp' as source_table
                                FROM dbo.agent_resp ar
                                INNER JOIN dbo.Agent a ON ar.[matricule] = a.[matricule]
                                LEFT JOIN dbo.responsabilite r ON ar.[code_res] = r.[CODE_RS]
                                ORDER BY a.[nom], a.[prenom], ar.[date_r] DESC";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Obtenir toutes les responsabilités des agents (table AGENT_RS)
        public static DataTable GetAllAgentsResponsabilitesRS()
        {
            try
            {
                string query = @"SELECT 
                                rs.[matricule], 
                                a.[nom], 
                                a.[prenom],
                                rs.[code_r] as code_res,
                                r.[LIBELLE] as responsabilite,
                                rs.[date_r],
                                rs.[code_a],
                                rs.[voiture],
                                'AGENT_RS' as source_table
                                FROM dbo.AGENT_RS rs
                                INNER JOIN dbo.Agent a ON rs.[matricule] = a.[matricule]
                                LEFT JOIN dbo.responsabilite r ON rs.[code_r] = r.[CODE_RS]
                                ORDER BY a.[nom], a.[prenom], rs.[date_r] DESC";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Obtenir toutes les responsabilités d'un agent (historique complet des deux tables)
        public static DataTable GetHistoriqueByAgent(string matricule)
        {
            try
            {
                string query = $@"
                    -- Responsabilités de la table agent_resp
                    SELECT 
                        ar.[matricule], 
                        ar.[code_res],
                        r.[LIBELLE] as responsabilite,
                        ar.[date_r],
                        ar.[code_a],
                        ar.[Actuel] as statut,
                        NULL as voiture,
                        'agent_resp' as source
                    FROM dbo.agent_resp ar
                    LEFT JOIN dbo.responsabilite r ON ar.[code_res] = r.[CODE_RS]
                    WHERE ar.[matricule] = '{matricule.Replace("'", "''")}' 
                    
                    UNION ALL
                    
                    -- Responsabilités de la table AGENT_RS
                    SELECT 
                        rs.[matricule], 
                        rs.[code_r] as code_res,
                        r.[LIBELLE] as responsabilite,
                        rs.[date_r],
                        rs.[code_a],
                        'N' as statut,
                        rs.[voiture],
                        'AGENT_RS' as source
                    FROM dbo.AGENT_RS rs
                    LEFT JOIN dbo.responsabilite r ON rs.[code_r] = r.[CODE_RS]
                    WHERE rs.[matricule] = '{matricule.Replace("'", "''")}' 
                    
                    ORDER BY date_r DESC";

                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Attribuer une responsabilité à un agent (table agent_resp)
        public static bool AttribuerResponsabilite(string matricule, int codeRes, DateTime dateR, int codeA, string actuel)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // D'abord, mettre à jour les anciennes responsabilités comme non actuelles
                    if (actuel == "A")
                    {
                        string updateQuery = @"UPDATE dbo.agent_resp 
                                             SET [Actuel] = 'N' 
                                             WHERE [matricule] = @matricule AND [Actuel] = 'A'";
                        SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@matricule", matricule);
                        conn.Open();
                        updateCmd.ExecuteNonQuery();
                        conn.Close();
                    }

                    // Ensuite, insérer la nouvelle responsabilité
                    string query = @"INSERT INTO dbo.agent_resp 
                                    ([matricule], [code_res], [date_r], [code_a], [Actuel]) 
                                    VALUES (@matricule, @codeRes, @dateR, @codeA, @actuel)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@matricule", matricule);
                    cmd.Parameters.AddWithValue("@codeRes", codeRes);
                    cmd.Parameters.AddWithValue("@dateR", dateR);
                    cmd.Parameters.AddWithValue("@codeA", codeA);
                    cmd.Parameters.AddWithValue("@actuel", actuel);

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

        // ✅ Supprimer une responsabilité d'agent
        public static bool SupprimerAgentResponsabilite(string matricule, int codeRes, DateTime dateR)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"DELETE FROM dbo.agent_resp 
                                    WHERE [matricule] = @matricule 
                                    AND [code_res] = @codeRes 
                                    AND [date_r] = @dateR";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@matricule", matricule);
                    cmd.Parameters.AddWithValue("@codeRes", codeRes);
                    cmd.Parameters.AddWithValue("@dateR", dateR);

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

        // ==================== FONCTIONS UTILITAIRES ====================

        // ✅ Obtenir liste des agents pour dropdown
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

        // ✅ Obtenir les statistiques
        public static DataTable GetStatistiques()
        {
            try
            {
                string query = @"SELECT 
                                (SELECT COUNT(*) FROM dbo.responsabilite) as NbTypes,
                                (SELECT COUNT(DISTINCT matricule) FROM (
                                    SELECT matricule FROM dbo.agent_resp
                                    UNION
                                    SELECT matricule FROM dbo.AGENT_RS
                                ) t) as NbAgentsAvecResp,
                                (SELECT COUNT(*) FROM dbo.agent_resp WHERE Actuel = 'A') as NbResponsabilitesActives";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Rechercher des responsabilités
        public static DataTable RechercherAgentsResponsabilites(string recherche = "", int codeRes = 0, string actuel = "")
        {
            try
            {
                string query = @"SELECT 
                                ar.[matricule], 
                                a.[nom], 
                                a.[prenom],
                                ar.[code_res],
                                r.[LIBELLE] as responsabilite,
                                ar.[date_r],
                                ar.[code_a],
                                ar.[Actuel]
                                FROM dbo.agent_resp ar
                                INNER JOIN dbo.Agent a ON ar.[matricule] = a.[matricule]
                                LEFT JOIN dbo.responsabilite r ON ar.[code_res] = r.[CODE_RS]
                                WHERE 1=1";

                // Recherche générale
                if (!string.IsNullOrEmpty(recherche))
                {
                    recherche = recherche.Replace("'", "''");
                    query += $@" AND (ar.[matricule] LIKE '%{recherche}%' 
                               OR a.[nom] LIKE '%{recherche}%' 
                               OR a.[prenom] LIKE '%{recherche}%')";
                }

                // Filtre par responsabilité
                if (codeRes > 0)
                {
                    query += $" AND ar.[code_res] = {codeRes}";
                }

                // Filtre par statut
                if (!string.IsNullOrEmpty(actuel))
                {
                    query += $" AND ar.[Actuel] = '{actuel}'";
                }

                query += " ORDER BY a.[nom], a.[prenom]";

                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }
    }
}