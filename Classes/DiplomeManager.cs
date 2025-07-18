using System;
using System.Data;
using System.Data.SqlClient;

namespace GestionRHv2.Classes
{
    public class DiplomeManager
    {
        // ==================== GESTION DES DIPLÔMES ====================

        // ✅ Obtenir tous les diplômes
        public static DataTable GetAllDiplomes()
        {
            try
            {
                string query = @"SELECT [CODE_DP], [LIBELLE], [code_profile]
                                FROM dbo.DIPLOME
                                ORDER BY [LIBELLE]";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Obtenir un diplôme par code
        public static DataTable GetDiplomeByCode(int codeDP)
        {
            try
            {
                string query = $"SELECT * FROM dbo.DIPLOME WHERE [CODE_DP] = {codeDP}";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Vérifier si un code existe déjà
        public static bool CodeDiplomeExists(int codeDP)
        {
            try
            {
                string query = $"SELECT COUNT(*) FROM dbo.DIPLOME WHERE [CODE_DP] = {codeDP}";
                DataTable dt = Database.GetData(query);
                return dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0;
            }
            catch
            {
                return false;
            }
        }

        // ✅ Obtenir le prochain code disponible
        public static int GetNextCodeDiplome()
        {
            try
            {
                string query = "SELECT ISNULL(MAX([CODE_DP]), 0) + 1 FROM dbo.DIPLOME";
                DataTable dt = Database.GetData(query);
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            catch
            {
                return 1;
            }
        }

        // ✅ Ajouter un nouveau diplôme
        public static bool AjouterDiplome(int codeDP, string libelle, int codeProfile)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO dbo.DIPLOME 
                                    ([CODE_DP], [LIBELLE], [code_profile]) 
                                    VALUES (@codeDP, @libelle, @codeProfile)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@codeDP", codeDP);
                    cmd.Parameters.AddWithValue("@libelle", libelle?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@codeProfile", codeProfile);

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

        // ✅ Modifier un diplôme
        public static bool ModifierDiplome(int codeDP, string libelle, int codeProfile)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE dbo.DIPLOME SET
                                    [LIBELLE] = @libelle,
                                    [code_profile] = @codeProfile
                                    WHERE [CODE_DP] = @codeDP";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@codeDP", codeDP);
                    cmd.Parameters.AddWithValue("@libelle", libelle?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@codeProfile", codeProfile);

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

        // ✅ Supprimer un diplôme
        public static bool SupprimerDiplome(int codeDP)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM dbo.DIPLOME WHERE [CODE_DP] = @codeDP";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@codeDP", codeDP);

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

        // ==================== GESTION DES NIVEAUX D'INSTRUCTION ====================

        // ✅ Obtenir tous les niveaux d'instruction
        public static DataTable GetAllNiveauxInstruction()
        {
            try
            {
                string query = @"SELECT 
                                dn.[matricule], 
                                a.[nom], 
                                a.[prenom],
                                dn.[Niveau_instruction]
                                FROM dbo.Diplome_Niveau_instr dn
                                INNER JOIN dbo.Agent a ON dn.[matricule] = a.[matricule]
                                ORDER BY a.[nom], a.[prenom]";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Obtenir le niveau d'instruction d'un agent
        public static DataTable GetNiveauInstructionByMatricule(string matricule)
        {
            try
            {
                string query = $"SELECT * FROM dbo.Diplome_Niveau_instr WHERE [matricule] = '{matricule.Replace("'", "''")}'";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Ajouter/Modifier niveau d'instruction
        public static bool SetNiveauInstruction(string matricule, int niveauInstruction)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Vérifier si l'enregistrement existe déjà
                    string checkQuery = "SELECT COUNT(*) FROM dbo.Diplome_Niveau_instr WHERE [matricule] = @matricule";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@matricule", matricule);

                    conn.Open();
                    int count = (int)checkCmd.ExecuteScalar();

                    string query;
                    if (count > 0)
                    {
                        // Update
                        query = @"UPDATE dbo.Diplome_Niveau_instr 
                                 SET [Niveau_instruction] = @niveau 
                                 WHERE [matricule] = @matricule";
                    }
                    else
                    {
                        // Insert
                        query = @"INSERT INTO dbo.Diplome_Niveau_instr 
                                 ([matricule], [Niveau_instruction]) 
                                 VALUES (@matricule, @niveau)";
                    }

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@matricule", matricule);
                    cmd.Parameters.AddWithValue("@niveau", niveauInstruction);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        // ✅ Supprimer niveau d'instruction
        public static bool SupprimerNiveauInstruction(string matricule)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM dbo.Diplome_Niveau_instr WHERE [matricule] = @matricule";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@matricule", matricule);

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

        // ✅ Obtenir les statistiques
        public static DataTable GetStatistiquesDiplomes()
        {
            try
            {
                string query = @"SELECT 
                                COUNT(DISTINCT d.CODE_DP) as NbDiplomes,
                                COUNT(DISTINCT dn.matricule) as NbAgentsAvecNiveau
                                FROM dbo.DIPLOME d
                                CROSS JOIN dbo.Diplome_Niveau_instr dn";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }
    }
}