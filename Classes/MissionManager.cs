using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace GestionRHv2.Classes
{
    public class MissionManager
    {
        // Récupérer toutes les missions
        public static DataTable GetAllMissions()
        {
            string query = @"
                SELECT 
                    Matricule,
                    Date_deplacement,
                    Moyen_trans,
                    Montant,
                    Puiss_fisc,
                    Kilometrage,
                    Nb_vacation,
                    TotalLettre,
                    Nom,
                    Compte,
                    Grade,
                    Taux,
                    Nb_Jour,
                    Taux_Jour
                FROM Missions
                ORDER BY Date_deplacement DESC";

            return Database.GetData(query);
        }

        // Rechercher des missions
        public static DataTable SearchMissions(string searchTerm, DateTime? dateDebut, DateTime? dateFin)
        {
            string query = @"
                SELECT 
                    Matricule,
                    Date_deplacement,
                    Moyen_trans,
                    Montant,
                    Puiss_fisc,
                    Kilometrage,
                    Nb_vacation,
                    TotalLettre,
                    Nom,
                    Compte,
                    Grade,
                    Taux,
                    Nb_Jour,
                    Taux_Jour
                FROM Missions
                WHERE 1=1";

            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query += @" AND (Matricule LIKE @search 
                          OR Nom LIKE @search 
                          OR Moyen_trans LIKE @search
                          OR TotalLettre LIKE @search
                          OR Grade LIKE @search)";
                parameters.Add(new SqlParameter("@search", "%" + searchTerm + "%"));
            }

            if (dateDebut.HasValue)
            {
                query += " AND Date_deplacement >= @dateDebut";
                parameters.Add(new SqlParameter("@dateDebut", dateDebut.Value));
            }

            if (dateFin.HasValue)
            {
                query += " AND Date_deplacement <= @dateFin";
                parameters.Add(new SqlParameter("@dateFin", dateFin.Value));
            }

            query += " ORDER BY Date_deplacement DESC";

            return Database.GetDataWithParams(query, parameters.ToArray());
        }

        // Récupérer une mission par matricule et date
        public static DataTable GetMission(string matricule, DateTime date)
        {
            string query = @"
                SELECT * FROM Missions 
                WHERE Matricule = @matricule 
                AND Date_deplacement = @date";

            SqlParameter[] parameters = {
                new SqlParameter("@matricule", matricule),
                new SqlParameter("@date", date)
            };

            return Database.GetDataWithParams(query, parameters);
        }

        // Ajouter une mission
        public static int AddMission(string matricule, DateTime dateDeplacement, string moyenTrans,
            decimal? montant, string puissFisc, decimal? kilometrage, int? nbVacation,
            string totalLettre, string nom, string compte, string grade, decimal? taux,
            int? nbJour, decimal? tauxJour)
        {
            string query = @"
                INSERT INTO Missions (
                    Matricule, Date_deplacement, Moyen_trans, Montant, 
                    Puiss_fisc, Kilometrage, Nb_vacation, TotalLettre, 
                    Nom, Compte, Grade, Taux, Nb_Jour, Taux_Jour
                ) VALUES (
                    @matricule, @date, @moyen, @montant, 
                    @puiss, @km, @vacation, @lettre, 
                    @nom, @compte, @grade, @taux, @nbJour, @tauxJour
                )";

            SqlParameter[] parameters = {
                new SqlParameter("@matricule", matricule),
                new SqlParameter("@date", dateDeplacement),
                new SqlParameter("@moyen", (object)moyenTrans ?? DBNull.Value),
                new SqlParameter("@montant", (object)montant ?? DBNull.Value),
                new SqlParameter("@puiss", (object)puissFisc ?? DBNull.Value),
                new SqlParameter("@km", (object)kilometrage ?? DBNull.Value),
                new SqlParameter("@vacation", (object)nbVacation ?? DBNull.Value),
                new SqlParameter("@lettre", (object)totalLettre ?? DBNull.Value),
                new SqlParameter("@nom", (object)nom ?? DBNull.Value),
                new SqlParameter("@compte", (object)compte ?? DBNull.Value),
                new SqlParameter("@grade", (object)grade ?? DBNull.Value),
                new SqlParameter("@taux", (object)taux ?? DBNull.Value),
                new SqlParameter("@nbJour", (object)nbJour ?? DBNull.Value),
                new SqlParameter("@tauxJour", (object)tauxJour ?? DBNull.Value)
            };

            return Database.ExecuteNonQuery(query, parameters);
        }

        // Mettre à jour une mission
        public static int UpdateMission(string matricule, DateTime dateDeplacement, string moyenTrans,
            decimal? montant, string puissFisc, decimal? kilometrage, int? nbVacation,
            string totalLettre, string nom, string compte, string grade, decimal? taux,
            int? nbJour, decimal? tauxJour, string oldMatricule, DateTime oldDate)
        {
            string query = @"
                UPDATE Missions SET 
                    Matricule = @matricule,
                    Date_deplacement = @date,
                    Moyen_trans = @moyen,
                    Montant = @montant,
                    Puiss_fisc = @puiss,
                    Kilometrage = @km,
                    Nb_vacation = @vacation,
                    TotalLettre = @lettre,
                    Nom = @nom,
                    Compte = @compte,
                    Grade = @grade,
                    Taux = @taux,
                    Nb_Jour = @nbJour,
                    Taux_Jour = @tauxJour
                WHERE Matricule = @oldMatricule 
                AND Date_deplacement = @oldDate";

            SqlParameter[] parameters = {
                new SqlParameter("@matricule", matricule),
                new SqlParameter("@date", dateDeplacement),
                new SqlParameter("@moyen", (object)moyenTrans ?? DBNull.Value),
                new SqlParameter("@montant", (object)montant ?? DBNull.Value),
                new SqlParameter("@puiss", (object)puissFisc ?? DBNull.Value),
                new SqlParameter("@km", (object)kilometrage ?? DBNull.Value),
                new SqlParameter("@vacation", (object)nbVacation ?? DBNull.Value),
                new SqlParameter("@lettre", (object)totalLettre ?? DBNull.Value),
                new SqlParameter("@nom", (object)nom ?? DBNull.Value),
                new SqlParameter("@compte", (object)compte ?? DBNull.Value),
                new SqlParameter("@grade", (object)grade ?? DBNull.Value),
                new SqlParameter("@taux", (object)taux ?? DBNull.Value),
                new SqlParameter("@nbJour", (object)nbJour ?? DBNull.Value),
                new SqlParameter("@tauxJour", (object)tauxJour ?? DBNull.Value),
                new SqlParameter("@oldMatricule", oldMatricule),
                new SqlParameter("@oldDate", oldDate)
            };

            return Database.ExecuteNonQuery(query, parameters);
        }

        // Supprimer une mission
        public static int DeleteMission(string matricule, DateTime date)
        {
            string query = @"
                DELETE FROM Missions 
                WHERE Matricule = @matricule 
                AND Date_deplacement = @date";

            SqlParameter[] parameters = {
                new SqlParameter("@matricule", matricule),
                new SqlParameter("@date", date)
            };

            return Database.ExecuteNonQuery(query, parameters);
        }

        // Obtenir les statistiques
        public static DataTable GetMissionStats()
        {
            string query = @"
                SELECT 
                    COUNT(*) as TotalMissions,
                    COUNT(DISTINCT Matricule) as TotalAgents,
                    SUM(ISNULL(Montant, 0)) as MontantTotal,
                    SUM(ISNULL(Kilometrage, 0)) as KilometrageTotal,
                    AVG(ISNULL(Montant, 0)) as MontantMoyen,
                    COUNT(CASE WHEN MONTH(Date_deplacement) = MONTH(GETDATE()) 
                               AND YEAR(Date_deplacement) = YEAR(GETDATE()) THEN 1 END) as MissionsCeMois
                FROM Missions";

            return Database.GetData(query);
        }

        // Obtenir les missions par agent
        public static DataTable GetMissionsByAgent(string matricule)
        {
            string query = @"
                SELECT * FROM Missions 
                WHERE Matricule = @matricule 
                ORDER BY Date_deplacement DESC";

            SqlParameter[] parameters = {
                new SqlParameter("@matricule", matricule)
            };

            return Database.GetDataWithParams(query, parameters);
        }
    }
}