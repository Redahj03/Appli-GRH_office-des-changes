using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GestionRHv2.Classes
{
    /// <summary>
    /// Classe pour la gestion des formations
    /// </summary>
    public class FormationManager
    {
        /// <summary>
        /// Ajouter une nouvelle formation
        /// </summary>
        public bool AjouterFormation(int objet, DateTime dateFormation, string lieu, string duree, int? effectif, string contenu)
        {
            try
            {
                string query = @"INSERT INTO Formation (Objet, Date_Form, Lieu, Durée, Effect, Contenu) 
                                VALUES (@Objet, @DateForm, @Lieu, @Duree, @Effectif, @Contenu)";

                SqlParameter[] parameters = {
                    new SqlParameter("@Objet", objet),
                    new SqlParameter("@DateForm", dateFormation),
                    new SqlParameter("@Lieu", lieu ?? (object)DBNull.Value),
                    new SqlParameter("@Duree", duree ?? (object)DBNull.Value),
                    new SqlParameter("@Effectif", effectif ?? (object)DBNull.Value),
                    new SqlParameter("@Contenu", contenu ?? (object)DBNull.Value)
                };

                return Database.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'ajout de la formation : " + ex.Message);
            }
        }

        /// <summary>
        /// Modifier une formation existante
        /// </summary>
        public bool ModifierFormation(int objet, DateTime dateFormation, string lieu, string duree, int? effectif, string contenu)
        {
            try
            {
                string query = @"UPDATE Formation SET 
                                Lieu = @Lieu, 
                                Durée = @Duree, 
                                Effect = @Effectif, 
                                Contenu = @Contenu 
                                WHERE Objet = @Objet AND Date_Form = @DateForm";

                SqlParameter[] parameters = {
                    new SqlParameter("@Objet", objet),
                    new SqlParameter("@DateForm", dateFormation),
                    new SqlParameter("@Lieu", lieu ?? (object)DBNull.Value),
                    new SqlParameter("@Duree", duree ?? (object)DBNull.Value),
                    new SqlParameter("@Effectif", effectif ?? (object)DBNull.Value),
                    new SqlParameter("@Contenu", contenu ?? (object)DBNull.Value)
                };

                return Database.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la modification de la formation : " + ex.Message);
            }
        }

        /// <summary>
        /// Modifier une formation avec changement de date possible
        /// </summary>
        public bool ModifierFormationAvecNouvelleDate(int objet, DateTime ancienneDate, DateTime nouvelleDate,
            string lieu, string duree, int? effectif, string contenu)
        {
            try
            {
                if (ancienneDate.Date != nouvelleDate.Date)
                {
                    using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RH_DB"].ConnectionString))
                    {
                        conn.Open();
                        SqlTransaction transaction = conn.BeginTransaction();

                        try
                        {
                            // 1. Créer la nouvelle formation
                            string insertQuery = @"INSERT INTO Formation (Objet, Date_Form, Lieu, Durée, Effect, Contenu) 
                                                 VALUES (@Objet, @NouvelleDate, @Lieu, @Duree, @Effectif, @Contenu)";

                            SqlCommand cmdInsert = new SqlCommand(insertQuery, conn, transaction);
                            cmdInsert.Parameters.AddRange(new SqlParameter[] {
                                new SqlParameter("@Objet", objet),
                                new SqlParameter("@NouvelleDate", nouvelleDate),
                                new SqlParameter("@Lieu", lieu ?? (object)DBNull.Value),
                                new SqlParameter("@Duree", duree ?? (object)DBNull.Value),
                                new SqlParameter("@Effectif", effectif ?? (object)DBNull.Value),
                                new SqlParameter("@Contenu", contenu ?? (object)DBNull.Value)
                            });
                            cmdInsert.ExecuteNonQuery();

                            // 2. Mettre à jour les inscriptions
                            string updateInscriptionsQuery = @"UPDATE Agent_Form 
                                                             SET date_form = @NouvelleDate 
                                                             WHERE Objet = @Objet AND date_form = @AncienneDate";

                            SqlCommand cmdUpdate = new SqlCommand(updateInscriptionsQuery, conn, transaction);
                            cmdUpdate.Parameters.AddRange(new SqlParameter[] {
                                new SqlParameter("@Objet", objet),
                                new SqlParameter("@NouvelleDate", nouvelleDate),
                                new SqlParameter("@AncienneDate", ancienneDate)
                            });
                            cmdUpdate.ExecuteNonQuery();

                            // 3. Supprimer l'ancienne formation
                            string deleteQuery = @"DELETE FROM Formation 
                                                 WHERE Objet = @Objet AND Date_Form = @AncienneDate";

                            SqlCommand cmdDelete = new SqlCommand(deleteQuery, conn, transaction);
                            cmdDelete.Parameters.AddRange(new SqlParameter[] {
                                new SqlParameter("@Objet", objet),
                                new SqlParameter("@AncienneDate", ancienneDate)
                            });
                            cmdDelete.ExecuteNonQuery();

                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                else
                {
                    return ModifierFormation(objet, nouvelleDate, lieu, duree, effectif, contenu);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la modification de la formation : " + ex.Message);
            }
        }

        /// <summary>
        /// Supprimer une formation
        /// </summary>
        public bool SupprimerFormation(int objet, DateTime dateFormation)
        {
            try
            {
                string checkQuery = "SELECT COUNT(*) FROM Agent_Form WHERE Objet = @Objet AND date_form = @DateForm";
                SqlParameter[] checkParams = {
                    new SqlParameter("@Objet", objet),
                    new SqlParameter("@DateForm", dateFormation)
                };

                object result = Database.ExecuteScalar(checkQuery, checkParams);
                int agentsInscrits = result != null ? Convert.ToInt32(result) : 0;

                if (agentsInscrits > 0)
                {
                    throw new Exception($"Impossible de supprimer cette formation car {agentsInscrits} agent(s) y sont inscrits.");
                }

                string query = "DELETE FROM Formation WHERE Objet = @Objet AND Date_Form = @DateForm";
                SqlParameter[] parameters = {
                    new SqlParameter("@Objet", objet),
                    new SqlParameter("@DateForm", dateFormation)
                };

                return Database.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la suppression de la formation : " + ex.Message);
            }
        }

        /// <summary>
        /// Obtenir toutes les formations
        /// </summary>
        public DataTable ObtenirToutesLesFormations()
        {
            try
            {
                string query = @"SELECT 
                                Objet,
                                Date_Form,
                                ISNULL(Lieu, '') as Lieu,
                                ISNULL(Durée, '') as Durée,
                                Effect,
                                ISNULL(Contenu, '') as Contenu,
                                (SELECT COUNT(*) FROM Agent_Form af WHERE af.Objet = f.Objet AND af.date_form = f.Date_Form) as NbInscrits
                                FROM Formation f
                                ORDER BY Date_Form DESC";

                return Database.GetData(query);
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la récupération des formations : " + ex.Message);
            }
        }

        /// <summary>
        /// Obtenir une formation spécifique
        /// </summary>
        public DataRow ObtenirFormation(int objet, DateTime dateFormation)
        {
            try
            {
                string query = @"SELECT * FROM Formation 
                                WHERE Objet = @Objet AND Date_Form = @DateForm";

                SqlParameter[] parameters = {
                    new SqlParameter("@Objet", objet),
                    new SqlParameter("@DateForm", dateFormation)
                };

                DataTable dt = Database.GetDataWithParams(query, parameters);
                return dt.Rows.Count > 0 ? dt.Rows[0] : null;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la récupération de la formation : " + ex.Message);
            }
        }

        /// <summary>
        /// Rechercher des formations
        /// </summary>
        public DataTable RechercherFormations(string terme, DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            try
            {
                string query = @"SELECT 
                                Objet,
                                Date_Form,
                                ISNULL(Lieu, '') as Lieu,
                                ISNULL(Durée, '') as Durée,
                                Effect,
                                ISNULL(Contenu, '') as Contenu,
                                (SELECT COUNT(*) FROM Agent_Form af WHERE af.Objet = f.Objet AND af.date_form = f.Date_Form) as NbInscrits
                                FROM Formation f
                                WHERE 1=1";

                List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(terme))
                {
                    query += " AND (ISNULL(Lieu, '') LIKE @Terme OR ISNULL(Contenu, '') LIKE @Terme)";
                    parameters.Add(new SqlParameter("@Terme", "%" + terme + "%"));
                }

                if (dateDebut.HasValue)
                {
                    query += " AND Date_Form >= @DateDebut";
                    parameters.Add(new SqlParameter("@DateDebut", dateDebut.Value));
                }

                if (dateFin.HasValue)
                {
                    query += " AND Date_Form <= @DateFin";
                    parameters.Add(new SqlParameter("@DateFin", dateFin.Value));
                }

                query += " ORDER BY Date_Form DESC";

                if (parameters.Count > 0)
                {
                    return Database.GetDataWithParams(query, parameters.ToArray());
                }
                else
                {
                    return Database.GetData(query);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la recherche des formations : " + ex.Message);
            }
        }

        /// <summary>
        /// Inscrire un agent à une formation
        /// </summary>
        public bool InscrireAgent(int matricule, int objet, DateTime dateFormation)
        {
            try
            {
                // Vérifier si l'agent n'est pas déjà inscrit
                string checkQuery = "SELECT COUNT(*) FROM Agent_Form WHERE Matricule = @Matricule AND Objet = @Objet AND date_form = @DateForm";
                SqlParameter[] checkParams = {
                    new SqlParameter("@Matricule", matricule),
                    new SqlParameter("@Objet", objet),
                    new SqlParameter("@DateForm", dateFormation)
                };

                object result = Database.ExecuteScalar(checkQuery, checkParams);
                int dejaInscrit = result != null ? Convert.ToInt32(result) : 0;

                if (dejaInscrit > 0)
                {
                    return false; // L'agent est déjà inscrit
                }

                // Vérifier l'effectif maximum - CRÉER DE NOUVEAUX PARAMÈTRES
                string checkEffectifQuery = @"SELECT Effect FROM Formation WHERE Objet = @Objet AND Date_Form = @DateForm";
                SqlParameter[] effectifParams = {
                    new SqlParameter("@Objet", objet),
                    new SqlParameter("@DateForm", dateFormation)
                };

                DataTable dtFormation = Database.GetDataWithParams(checkEffectifQuery, effectifParams);
                if (dtFormation.Rows.Count > 0 && dtFormation.Rows[0]["Effect"] != DBNull.Value)
                {
                    int effectifMax = Convert.ToInt32(dtFormation.Rows[0]["Effect"]);

                    // Compter les inscrits actuels - CRÉER DE NOUVEAUX PARAMÈTRES
                    string countQuery = "SELECT COUNT(*) FROM Agent_Form WHERE Objet = @Objet AND date_form = @DateForm";
                    SqlParameter[] countParams = {
                        new SqlParameter("@Objet", objet),
                        new SqlParameter("@DateForm", dateFormation)
                    };

                    object countResult = Database.ExecuteScalar(countQuery, countParams);
                    int inscritsActuels = countResult != null ? Convert.ToInt32(countResult) : 0;

                    if (inscritsActuels >= effectifMax)
                    {
                        throw new Exception("La formation est complète. Effectif maximum atteint.");
                    }
                }

                // Inscrire l'agent
                string query = "INSERT INTO Agent_Form (Matricule, Objet, date_form) VALUES (@Matricule, @Objet, @DateForm)";
                SqlParameter[] parameters = {
                    new SqlParameter("@Matricule", matricule),
                    new SqlParameter("@Objet", objet),
                    new SqlParameter("@DateForm", dateFormation)
                };

                return Database.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'inscription de l'agent : " + ex.Message);
            }
        }

        /// <summary>
        /// Désinscrire un agent d'une formation
        /// </summary>
        public bool DesinscrireAgent(int matricule, int objet, DateTime dateFormation)
        {
            try
            {
                string query = "DELETE FROM Agent_Form WHERE Matricule = @Matricule AND Objet = @Objet AND date_form = @DateForm";
                SqlParameter[] parameters = {
                    new SqlParameter("@Matricule", matricule),
                    new SqlParameter("@Objet", objet),
                    new SqlParameter("@DateForm", dateFormation)
                };

                return Database.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la désinscription de l'agent : " + ex.Message);
            }
        }

        /// <summary>
        /// Obtenir la liste des agents inscrits à une formation
        /// </summary>
        public DataTable ObtenirAgentsInscrits(int objet, DateTime dateFormation)
        {
            try
            {
                string query = @"SELECT 
                                a.matricule,
                                ISNULL(a.nom, '') as nom,
                                ISNULL(a.prenom, '') as prenom,
                                ISNULL(a.cin, '') as cin,
                                ISNULL(a.tel, '') as tel,
                                ISNULL(a.ville, '') as ville,
                                af.date_form
                                FROM Agent_Form af
                                INNER JOIN Agent a ON af.Matricule = a.matricule
                                WHERE af.Objet = @Objet AND af.date_form = @DateForm
                                ORDER BY a.nom, a.prenom";

                SqlParameter[] parameters = {
                    new SqlParameter("@Objet", objet),
                    new SqlParameter("@DateForm", dateFormation)
                };

                return Database.GetDataWithParams(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la récupération des agents inscrits : " + ex.Message);
            }
        }

        /// <summary>
        /// Obtenir les statistiques des formations
        /// </summary>
        public DataTable ObtenirStatistiquesFormations()
        {
            try
            {
                string query = @"SELECT 
                                COUNT(DISTINCT CONCAT(CAST(Objet AS VARCHAR), '_', CAST(Date_Form AS VARCHAR))) as TotalFormations,
                                (SELECT COUNT(DISTINCT Matricule) FROM Agent_Form) as TotalParticipants,
                                COUNT(DISTINCT CASE WHEN Date_Form >= DATEADD(year, -1, GETDATE()) THEN CONCAT(CAST(Objet AS VARCHAR), '_', CAST(Date_Form AS VARCHAR)) END) as FormationsDerniereAnnee,
                                (SELECT COUNT(*) FROM Agent_Form) as TotalInscriptions
                                FROM Formation";

                return Database.GetData(query);
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la récupération des statistiques : " + ex.Message);
            }
        }

        /// <summary>
        /// Obtenir le prochain numéro d'objet disponible
        /// </summary>
        public int ObtenirProchainNumeroObjet()
        {
            try
            {
                string query = "SELECT ISNULL(MAX(Objet), 0) + 1 FROM Formation";
                object result = Database.ExecuteScalar(query);
                return result != null ? Convert.ToInt32(result) : 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la génération du numéro d'objet : " + ex.Message);
            }
        }

        /// <summary>
        /// Obtenir tous les agents disponibles pour inscription
        /// </summary>
        public DataTable ObtenirAgentsDisponibles()
        {
            try
            {
                string query = @"SELECT 
                                matricule,
                                ISNULL(nom, '') as nom,
                                ISNULL(prenom, '') as prenom,
                                ISNULL(cin, '') as cin,
                                ISNULL(tel, '') as tel,
                                ISNULL(ville, '') as ville
                                FROM Agent 
                                WHERE (retrait IS NULL OR retrait = '')
                                ORDER BY nom, prenom";

                return Database.GetData(query);
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la récupération des agents : " + ex.Message);
            }
        }
    }
}