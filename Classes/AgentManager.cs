using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace GestionRHv2.Classes
{
    public class AgentManager
    {
        // ✅ MÉTHODE SIMPLE - GetAllAgents (comme avant mais avec crochets)
        public static DataTable GetAllAgents()
        {
            string query = "SELECT [matricule], [nom], [prenom], [cin], [tel], [ville] FROM dbo.Agent ORDER BY [nom], [prenom]";
            return Database.GetData(query);
        }

        // ✅ MÉTHODE SIMPLE - GetAgentByMatricule (comme avant mais sécurisée)
        public static DataTable GetAgentByMatricule(string matricule)
        {
            try
            {
                // Utilise Database.GetData avec requête sécurisée simple
                string query = $"SELECT * FROM dbo.Agent WHERE [matricule] = '{matricule.Replace("'", "''")}'";
                return Database.GetData(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // ✅ MÉTHODE RAPIDE - MatriculeExiste (simple)
        public static bool MatriculeExiste(string matricule)
        {
            try
            {
                string query = $"SELECT COUNT(*) FROM dbo.Agent WHERE [matricule] = '{matricule.Replace("'", "''")}'";
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

        // ✅ MÉTHODE RAPIDE - CinExiste (simple)
        public static bool CinExiste(string cin)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cin)) return false;

                string query = $"SELECT COUNT(*) FROM dbo.Agent WHERE [cin] = '{cin.Replace("'", "''")}'";
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

        // ✅ MÉTHODE OPTIMISÉE - AjouterAgent (rapide, sans logs debug excessifs)
        public static bool AjouterAgent(
           string matricule, string nom, string prenom, string dateN, string lieuN,
           string dateR, string pecul, string sex, string cin, string tel,
           string national, string situation, string adresse, string ville,
           string bqv, string bqe, string cpt, string pas, string retrait,
           string typeRet, string pretLog)
        {
            try
            {
                string connectionString =
                    System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
INSERT INTO dbo.Agent
  ([matricule], [nom], [prenom], [date_n], [lieu_n], [date_r], [pecul],
   [sex], [cin], [tel], [national], [situation], [adresse], [ville],
   [bqv], [bqe], [cpt], [pas], [retrait], [Type_ret], [pret_log])
VALUES
  (@matricule, @nom, @prenom, @dateN, @lieuN, @dateR, @pecul,
   @sex, @cin, @tel, @national, @situation, @adresse, @ville,
   @bqv, @bqe, @cpt, @pas, @retrait, @typeRet, @pretLog)";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Paramètres standards
                    cmd.Parameters.AddWithValue("@matricule", matricule?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@nom", nom?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@prenom", prenom?.Trim() ?? "");

                    // Parsing de la date de naissance
                    DateTime? dtNaissance = null;
                    if (!string.IsNullOrWhiteSpace(dateN))
                    {
                        if (DateTime.TryParseExact(
                                dateN, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                                DateTimeStyles.None, out var tmpN))
                        {
                            dtNaissance = tmpN;
                        }
                        else if (DateTime.TryParseExact(
                                dateN, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                DateTimeStyles.None, out tmpN))
                        {
                            dtNaissance = tmpN;
                        }
                    }
                    cmd.Parameters.Add("@dateN", SqlDbType.Date)
                       .Value = dtNaissance.HasValue
                                  ? (object)dtNaissance.Value
                                  : DBNull.Value;

                    // Lieu de naissance
                    cmd.Parameters.AddWithValue(
                        "@lieuN",
                        string.IsNullOrEmpty(lieuN) ? DBNull.Value : (object)lieuN.Trim());

                    // Parsing de la date de recrutement
                    DateTime? dtRecrutement = null;
                    if (!string.IsNullOrWhiteSpace(dateR)
                        && DateTime.TryParseExact(
                               dateR, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                               DateTimeStyles.None, out var tmpR))
                    {
                        dtRecrutement = tmpR;
                    }
                    cmd.Parameters.Add("@dateR", SqlDbType.Date)
                       .Value = dtRecrutement.HasValue
                                  ? (object)dtRecrutement.Value
                                  : DBNull.Value;

                    // Suite des paramètres
                    cmd.Parameters.AddWithValue("@pecul", string.IsNullOrEmpty(pecul) ? DBNull.Value : (object)pecul.Trim());
                    cmd.Parameters.AddWithValue("@sex", string.IsNullOrEmpty(sex) ? DBNull.Value : (object)sex);
                    cmd.Parameters.AddWithValue("@cin", string.IsNullOrEmpty(cin) ? DBNull.Value : (object)cin.Trim());
                    cmd.Parameters.AddWithValue("@tel", string.IsNullOrEmpty(tel) ? DBNull.Value : (object)tel.Trim());
                    cmd.Parameters.AddWithValue("@national", string.IsNullOrEmpty(national) ? DBNull.Value : (object)national.Trim());
                    cmd.Parameters.AddWithValue("@situation", string.IsNullOrEmpty(situation) ? DBNull.Value : (object)situation);
                    cmd.Parameters.AddWithValue("@adresse", string.IsNullOrEmpty(adresse) ? DBNull.Value : (object)adresse.Trim());
                    cmd.Parameters.AddWithValue("@ville", string.IsNullOrEmpty(ville) ? DBNull.Value : (object)ville.Trim());
                    cmd.Parameters.AddWithValue("@bqv", string.IsNullOrEmpty(bqv) ? DBNull.Value : (object)bqv.Trim());
                    cmd.Parameters.AddWithValue("@bqe", string.IsNullOrEmpty(bqe) ? DBNull.Value : (object)bqe.Trim());
                    cmd.Parameters.AddWithValue("@cpt", string.IsNullOrEmpty(cpt) ? DBNull.Value : (object)cpt.Trim());
                    cmd.Parameters.AddWithValue("@pas", string.IsNullOrEmpty(pas) ? DBNull.Value : (object)pas.Trim());
                    cmd.Parameters.AddWithValue("@retrait", string.IsNullOrEmpty(retrait) ? DBNull.Value : (object)retrait);
                    cmd.Parameters.AddWithValue("@typeRet", string.IsNullOrEmpty(typeRet) ? DBNull.Value : (object)typeRet.Trim());
                    cmd.Parameters.AddWithValue("@pretLog", string.IsNullOrEmpty(pretLog) ? DBNull.Value : (object)pretLog.Trim());

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur SQL lors de l'insertion : " + ex.Message, ex);
            }
        }

        // ✅ MÉTHODE OPTIMISÉE - ModifierAgent (rapide)
        public static bool ModifierAgent(string matricule, string nom, string prenom, string dateN, string lieuN,
                                       string dateR, string pecul, string sex, string cin, string tel,
                                       string national, string situation, string adresse, string ville,
                                       string bqv, string bqe, string cpt, string pas, string retrait,
                                       string typeRet, string pretLog)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE dbo.Agent SET [nom]=@nom, [prenom]=@prenom, [date_n]=@dateN, [lieu_n]=@lieuN, 
                                    [date_r]=@dateR, [pecul]=@pecul, [sex]=@sex, [cin]=@cin, [tel]=@tel, [national]=@national, 
                                    [situation]=@situation, [adresse]=@adresse, [ville]=@ville, [bqv]=@bqv, [bqe]=@bqe, 
                                    [cpt]=@cpt, [pas]=@pas, [retrait]=@retrait, [Type_ret]=@typeRet, [pret_log]=@pretLog 
                                    WHERE [matricule]=@matricule";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@matricule", matricule?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@nom", nom?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@prenom", prenom?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@dateN", string.IsNullOrEmpty(dateN) ? DBNull.Value : (object)dateN);
                    cmd.Parameters.AddWithValue("@lieuN", string.IsNullOrEmpty(lieuN) ? DBNull.Value : (object)lieuN?.Trim());
                    cmd.Parameters.AddWithValue("@dateR", string.IsNullOrEmpty(dateR) ? DBNull.Value : (object)dateR);
                    cmd.Parameters.AddWithValue("@pecul", string.IsNullOrEmpty(pecul) ? DBNull.Value : (object)pecul?.Trim());
                    cmd.Parameters.AddWithValue("@sex", string.IsNullOrEmpty(sex) ? DBNull.Value : (object)sex);
                    cmd.Parameters.AddWithValue("@cin", string.IsNullOrEmpty(cin) ? DBNull.Value : (object)cin?.Trim());
                    cmd.Parameters.AddWithValue("@tel", string.IsNullOrEmpty(tel) ? DBNull.Value : (object)tel?.Trim());
                    cmd.Parameters.AddWithValue("@national", string.IsNullOrEmpty(national) ? DBNull.Value : (object)national?.Trim());
                    cmd.Parameters.AddWithValue("@situation", string.IsNullOrEmpty(situation) ? DBNull.Value : (object)situation);
                    cmd.Parameters.AddWithValue("@adresse", string.IsNullOrEmpty(adresse) ? DBNull.Value : (object)adresse?.Trim());
                    cmd.Parameters.AddWithValue("@ville", string.IsNullOrEmpty(ville) ? DBNull.Value : (object)ville?.Trim());
                    cmd.Parameters.AddWithValue("@bqv", string.IsNullOrEmpty(bqv) ? DBNull.Value : (object)bqv?.Trim());
                    cmd.Parameters.AddWithValue("@bqe", string.IsNullOrEmpty(bqe) ? DBNull.Value : (object)bqe?.Trim());
                    cmd.Parameters.AddWithValue("@cpt", string.IsNullOrEmpty(cpt) ? DBNull.Value : (object)cpt?.Trim());
                    cmd.Parameters.AddWithValue("@pas", string.IsNullOrEmpty(pas) ? DBNull.Value : (object)pas?.Trim());
                    cmd.Parameters.AddWithValue("@retrait", string.IsNullOrEmpty(retrait) ? DBNull.Value : (object)retrait);
                    cmd.Parameters.AddWithValue("@typeRet", string.IsNullOrEmpty(typeRet) ? DBNull.Value : (object)typeRet?.Trim());
                    cmd.Parameters.AddWithValue("@pretLog", string.IsNullOrEmpty(pretLog) ? DBNull.Value : (object)pretLog?.Trim());

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

        // ✅ MÉTHODE OPTIMISÉE - SupprimerAgent (rapide)
        public static bool SupprimerAgent(string matricule)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM dbo.Agent WHERE [matricule] = @matricule";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@matricule", matricule?.Trim() ?? "");

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

        // ✅ MÉTHODE POUR DROPDOWN - GetAgentsForDropdown
        public static DataTable GetAgentsForDropdown()
        {
            try
            {
                string query = @"SELECT [matricule], 
                               CONCAT([nom], ' ', [prenom], ' (', [matricule], ')') as agent_display 
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