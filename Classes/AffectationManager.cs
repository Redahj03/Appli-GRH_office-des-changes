using System;
using System.Data;
using System.Data.SqlClient;

namespace GestionRHv2.Classes
{
    public class AffectationManager
    {
        // ✅ Obtenir tous les lieux d'affectation
        public static DataTable GetAllAffectations()
        {
            try
            {
                string query = @"SELECT [CODE_AFF], [LIB], [Code_type_Entite], [lien], [Etat]
                                FROM dbo.Lieu_AFFECT
                                ORDER BY [LIB]";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Obtenir un lieu d'affectation par code
        public static DataTable GetAffectationByCode(string codeAff)
        {
            try
            {
                string query = $"SELECT * FROM dbo.Lieu_AFFECT WHERE [CODE_AFF] = '{codeAff.Replace("'", "''")}'";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Obtenir les types d'entités distincts
        public static DataTable GetTypesEntite()
        {
            try
            {
                string query = @"SELECT DISTINCT [Code_type_Entite] 
                                FROM dbo.Lieu_AFFECT 
                                WHERE [Code_type_Entite] IS NOT NULL AND [Code_type_Entite] != ''
                                ORDER BY [Code_type_Entite]";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ Vérifier si un code existe déjà
        public static bool CodeExists(string codeAff)
        {
            try
            {
                string query = $"SELECT COUNT(*) FROM dbo.Lieu_AFFECT WHERE [CODE_AFF] = '{codeAff.Replace("'", "''")}'";
                DataTable dt = Database.GetData(query);
                return dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0;
            }
            catch
            {
                return false;
            }
        }

        // ✅ Ajouter nouveau lieu d'affectation
        public static bool AjouterAffectation(string codeAff, string lib, string codeTypeEntite, string lien, string etat)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO dbo.Lieu_AFFECT 
                                    ([CODE_AFF], [LIB], [Code_type_Entite], [lien], [Etat]) 
                                    VALUES (@codeAff, @lib, @codeTypeEntite, @lien, @etat)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@codeAff", codeAff?.Trim().ToUpper() ?? "");
                    cmd.Parameters.AddWithValue("@lib", lib?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@codeTypeEntite", string.IsNullOrEmpty(codeTypeEntite) ? DBNull.Value : (object)codeTypeEntite.Trim());
                    cmd.Parameters.AddWithValue("@lien", string.IsNullOrEmpty(lien) ? DBNull.Value : (object)lien.Trim());
                    cmd.Parameters.AddWithValue("@etat", string.IsNullOrEmpty(etat) ? "A" : etat.Trim().ToUpper());

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

        // ✅ Modifier lieu d'affectation
        public static bool ModifierAffectation(string codeAff, string lib, string codeTypeEntite, string lien, string etat)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE dbo.Lieu_AFFECT SET
                                    [LIB] = @lib,
                                    [Code_type_Entite] = @codeTypeEntite,
                                    [lien] = @lien,
                                    [Etat] = @etat
                                    WHERE [CODE_AFF] = @codeAff";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@codeAff", codeAff?.Trim().ToUpper() ?? "");
                    cmd.Parameters.AddWithValue("@lib", lib?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@codeTypeEntite", string.IsNullOrEmpty(codeTypeEntite) ? DBNull.Value : (object)codeTypeEntite.Trim());
                    cmd.Parameters.AddWithValue("@lien", string.IsNullOrEmpty(lien) ? DBNull.Value : (object)lien.Trim());
                    cmd.Parameters.AddWithValue("@etat", string.IsNullOrEmpty(etat) ? "A" : etat.Trim().ToUpper());

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

        // ✅ Supprimer lieu d'affectation
        public static bool SupprimerAffectation(string codeAff)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM dbo.Lieu_AFFECT WHERE [CODE_AFF] = @codeAff";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@codeAff", codeAff);

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

        // ✅ Rechercher lieux d'affectation avec filtres
        public static DataTable RechercherAffectations(string lib = "", string codeTypeEntite = "", string etat = "")
        {
            try
            {
                string query = @"SELECT [CODE_AFF], [LIB], [Code_type_Entite], [lien], [Etat]
                                FROM dbo.Lieu_AFFECT
                                WHERE 1=1";

                // Filtre par libellé
                if (!string.IsNullOrEmpty(lib))
                {
                    query += $" AND [LIB] LIKE '%{lib.Replace("'", "''")}%'";
                }

                // Filtre par type d'entité
                if (!string.IsNullOrEmpty(codeTypeEntite))
                {
                    query += $" AND [Code_type_Entite] = '{codeTypeEntite.Replace("'", "''")}'";
                }

                // Filtre par état
                if (!string.IsNullOrEmpty(etat))
                {
                    query += $" AND [Etat] = '{etat.Replace("'", "''")}'";
                }

                query += " ORDER BY [LIB]";

                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }
    }
}