using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace GestionRHv2.Classes
{
    public class Database
    {
        // Connection string depuis Web.config
        private static string connectionString = ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

        // Méthode existante GetData
        public static DataTable GetData(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Erreur Database.GetData: {ex.Message}");
                throw new Exception("Erreur Base de données : " + ex.Message);
            }
            return dt;
        }

        // NOUVELLE MÉTHODE - GetData avec paramètres (sécurisée)
        public static DataTable GetDataWithParams(string query, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Erreur Database.GetDataWithParams: {ex.Message}");
                throw new Exception("Erreur Base de données : " + ex.Message);
            }
            return dt;
        }

        // NOUVELLE MÉTHODE - ExecuteNonQuery avec paramètres
        public static int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Erreur Database.ExecuteNonQuery: {ex.Message}");
                throw new Exception("Erreur Base de données : " + ex.Message);
            }
        }

        // NOUVELLE MÉTHODE - ExecuteScalar avec paramètres
        public static object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Erreur Database.ExecuteScalar: {ex.Message}");
                throw new Exception("Erreur Base de données : " + ex.Message);
            }
        }
    }
}