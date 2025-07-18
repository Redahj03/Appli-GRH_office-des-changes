using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GestionRHv2.Classes
{
    public class EnfantManager
    {
        // Obtenir le prochain numéro pour un matricule
        private static int GetNextNumForMatricule(string matricule)
        {
            string query = "SELECT ISNULL(MAX(num), 0) + 1 FROM AGENT_EF WHERE matricule = @matricule";
            SqlParameter param = new SqlParameter("@matricule", matricule);

            object result = Database.ExecuteScalar(query, param);
            return Convert.ToInt32(result);
        }

        // Ajouter un enfant
        public static bool AjouterEnfant(Enfant enfant)
        {
            try
            {
                // Obtenir automatiquement le prochain numéro pour ce matricule
                enfant.Num = GetNextNumForMatricule(enfant.Matricule);

                string query = @"INSERT INTO AGENT_EF 
                               (matricule, num, nom, prenom, sex, scolarise, date_n, lieu_n, ch, cod_sc, date_p_sc) 
                               VALUES 
                               (@matricule, @num, @nom, @prenom, @sex, @scolarise, @date_n, @lieu_n, @ch, @cod_sc, @date_p_sc)";

                SqlParameter[] parameters = {
                    new SqlParameter("@matricule", enfant.Matricule),
                    new SqlParameter("@num", enfant.Num),
                    new SqlParameter("@nom", enfant.Nom ?? ""),
                    new SqlParameter("@prenom", enfant.Prenom ?? ""),
                    new SqlParameter("@sex", enfant.Sexe ?? ""),
                    new SqlParameter("@scolarise", enfant.Scolarise ?? "N"),
                    new SqlParameter("@date_n", (object)enfant.DateNaissance ?? DBNull.Value),
                    new SqlParameter("@lieu_n", enfant.LieuNaissance ?? ""),
                    new SqlParameter("@ch", enfant.Ch ?? ""),
                    new SqlParameter("@cod_sc", enfant.CodSc ?? ""),
                    new SqlParameter("@date_p_sc", (object)enfant.DatePSc ?? DBNull.Value)
                };

                int rowsAffected = Database.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Erreur AjouterEnfant: {ex.Message}");
                throw;
            }
        }

        // Modifier un enfant
        public static bool ModifierEnfant(Enfant enfant)
        {
            try
            {
                string query = @"UPDATE AGENT_EF SET 
                               nom = @nom, 
                               prenom = @prenom, 
                               sex = @sex, 
                               scolarise = @scolarise, 
                               date_n = @date_n, 
                               lieu_n = @lieu_n, 
                               ch = @ch, 
                               cod_sc = @cod_sc, 
                               date_p_sc = @date_p_sc 
                               WHERE matricule = @matricule AND num = @num";

                SqlParameter[] parameters = {
                    new SqlParameter("@matricule", enfant.Matricule),
                    new SqlParameter("@num", enfant.Num),
                    new SqlParameter("@nom", enfant.Nom ?? ""),
                    new SqlParameter("@prenom", enfant.Prenom ?? ""),
                    new SqlParameter("@sex", enfant.Sexe ?? ""),
                    new SqlParameter("@scolarise", enfant.Scolarise ?? "N"),
                    new SqlParameter("@date_n", (object)enfant.DateNaissance ?? DBNull.Value),
                    new SqlParameter("@lieu_n", enfant.LieuNaissance ?? ""),
                    new SqlParameter("@ch", enfant.Ch ?? ""),
                    new SqlParameter("@cod_sc", enfant.CodSc ?? ""),
                    new SqlParameter("@date_p_sc", (object)enfant.DatePSc ?? DBNull.Value)
                };

                int rowsAffected = Database.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Erreur ModifierEnfant: {ex.Message}");
                throw;
            }
        }

        // Supprimer un enfant
        public static bool SupprimerEnfant(string matricule, int num)
        {
            try
            {
                string query = "DELETE FROM AGENT_EF WHERE matricule = @matricule AND num = @num";
                SqlParameter[] parameters = {
                    new SqlParameter("@matricule", matricule),
                    new SqlParameter("@num", num)
                };

                int rowsAffected = Database.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Erreur SupprimerEnfant: {ex.Message}");
                throw;
            }
        }

        // Obtenir un enfant par matricule et numéro
        public static Enfant GetEnfant(string matricule, int num)
        {
            try
            {
                string query = "SELECT * FROM AGENT_EF WHERE matricule = @matricule AND num = @num";
                SqlParameter[] parameters = {
                    new SqlParameter("@matricule", matricule),
                    new SqlParameter("@num", num)
                };

                DataTable dt = Database.GetDataWithParams(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    return DataRowToEnfant(dt.Rows[0]);
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Erreur GetEnfant: {ex.Message}");
                throw;
            }
        }

        // Obtenir tous les enfants
        public static List<Enfant> GetAllEnfants()
        {
            try
            {
                string query = @"SELECT e.*, a.nom AS agent_nom, a.prenom AS agent_prenom 
                               FROM AGENT_EF e 
                               LEFT JOIN AGENT a ON e.matricule = a.matricule 
                               ORDER BY e.matricule, e.num";

                DataTable dt = Database.GetData(query);
                return ConvertDataTableToList(dt);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Erreur GetAllEnfants: {ex.Message}");
                throw;
            }
        }

        // Obtenir les enfants d'un agent
        public static List<Enfant> GetEnfantsByMatricule(string matricule)
        {
            try
            {
                string query = "SELECT * FROM AGENT_EF WHERE matricule = @matricule ORDER BY num";
                SqlParameter param = new SqlParameter("@matricule", matricule);

                DataTable dt = Database.GetDataWithParams(query, param);
                return ConvertDataTableToList(dt);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Erreur GetEnfantsByMatricule: {ex.Message}");
                throw;
            }
        }

        // Rechercher des enfants
        public static List<Enfant> RechercherEnfants(string recherche)
        {
            try
            {
                string query = @"SELECT e.*, a.nom AS agent_nom, a.prenom AS agent_prenom 
                               FROM AGENT_EF e 
                               LEFT JOIN AGENT a ON e.matricule = a.matricule 
                               WHERE e.nom LIKE @recherche 
                                  OR e.prenom LIKE @recherche 
                                  OR e.matricule LIKE @recherche 
                                  OR a.nom LIKE @recherche 
                                  OR a.prenom LIKE @recherche 
                               ORDER BY e.matricule, e.num";

                SqlParameter param = new SqlParameter("@recherche", $"%{recherche}%");

                DataTable dt = Database.GetDataWithParams(query, param);
                return ConvertDataTableToList(dt);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Erreur RechercherEnfants: {ex.Message}");
                throw;
            }
        }

        // Obtenir les statistiques
        public static Dictionary<string, object> GetStatistiques()
        {
            try
            {
                var stats = new Dictionary<string, object>();

                // Total enfants
                string queryTotal = "SELECT COUNT(*) FROM AGENT_EF";
                stats["TotalEnfants"] = Database.ExecuteScalar(queryTotal);

                // Nombre de garçons
                string queryGarcons = "SELECT COUNT(*) FROM AGENT_EF WHERE sex = 'M'";
                stats["NombreGarcons"] = Database.ExecuteScalar(queryGarcons);

                // Nombre de filles
                string queryFilles = "SELECT COUNT(*) FROM AGENT_EF WHERE sex = 'F'";
                stats["NombreFilles"] = Database.ExecuteScalar(queryFilles);

                // Âge moyen
                string queryAgeMoyen = @"SELECT AVG(DATEDIFF(YEAR, date_n, GETDATE())) 
                                       FROM AGENT_EF 
                                       WHERE date_n IS NOT NULL";
                object ageMoyen = Database.ExecuteScalar(queryAgeMoyen);
                stats["AgeMoyen"] = ageMoyen != DBNull.Value ? Convert.ToDouble(ageMoyen) : 0;

                // Nombre d'enfants scolarisés
                string queryScolarises = "SELECT COUNT(*) FROM AGENT_EF WHERE scolarise = 'O'";
                stats["NombreScolarises"] = Database.ExecuteScalar(queryScolarises);

                return stats;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Erreur GetStatistiques: {ex.Message}");
                throw;
            }
        }

        // Convertir DataRow en objet Enfant
        private static Enfant DataRowToEnfant(DataRow row)
        {
            var enfant = new Enfant();

            // Matricule (obligatoire)
            enfant.Matricule = row["matricule"] != DBNull.Value ? row["matricule"].ToString() : "";

            // Num (obligatoire)
            enfant.Num = row["num"] != DBNull.Value ? Convert.ToInt32(row["num"]) : 0;

            // Autres champs (peuvent être NULL)
            enfant.Nom = row["nom"] != DBNull.Value ? row["nom"].ToString() : "";
            enfant.Prenom = row["prenom"] != DBNull.Value ? row["prenom"].ToString() : "";
            enfant.Sexe = row["sex"] != DBNull.Value ? row["sex"].ToString() : "";
            enfant.Scolarise = row["scolarise"] != DBNull.Value ? row["scolarise"].ToString() : "N";
            enfant.DateNaissance = row["date_n"] != DBNull.Value ? Convert.ToDateTime(row["date_n"]) : (DateTime?)null;
            enfant.LieuNaissance = row["lieu_n"] != DBNull.Value ? row["lieu_n"].ToString() : "";
            enfant.Ch = row["ch"] != DBNull.Value ? row["ch"].ToString() : "";
            enfant.CodSc = row["cod_sc"] != DBNull.Value ? row["cod_sc"].ToString() : "";
            enfant.DatePSc = row["date_p_sc"] != DBNull.Value ? Convert.ToDateTime(row["date_p_sc"]) : (DateTime?)null;

            return enfant;
        }

        // Convertir DataTable en List<Enfant>
        private static List<Enfant> ConvertDataTableToList(DataTable dt)
        {
            List<Enfant> enfants = new List<Enfant>();
            foreach (DataRow row in dt.Rows)
            {
                enfants.Add(DataRowToEnfant(row));
            }
            return enfants;
        }
    }
}