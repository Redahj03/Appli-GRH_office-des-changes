using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Missions
{
    public partial class ValidationMissions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMissions("all");
            }
        }

        private void LoadMissions(string filter)
        {
            try
            {
                string query = @"
                    SELECT 
                        om.ID,
                        om.matr as Matricule,
                        CONCAT(a.Nom, ' ', a.Prénom) as NomAgent,
                        om.ville as Ville,
                        om.Objet_mis as ObjetMission,
                        om.date_d as DateDepart,
                        om.date_r as DateRetour,
                        om.trpt as Transport,
                        om.Détail_trpt as DetailTransport
                    FROM ordre_miss om
                    LEFT JOIN Agent a ON om.matr = a.Matricule
                    WHERE om.Statut = 'En attente'";

                // Appliquer les filtres
                switch (filter)
                {
                    case "urgent":
                        // Missions qui commencent dans les 3 prochains jours
                        query += " AND om.date_d <= DATEADD(day, 3, GETDATE())";
                        break;
                    case "today":
                        // Missions qui commencent aujourd'hui
                        query += " AND CAST(om.date_d as DATE) = CAST(GETDATE() as DATE)";
                        break;
                }

                query += " ORDER BY om.date_d ASC";

                DataTable dt = Database.GetData(query);

                if (dt.Rows.Count > 0)
                {
                    rptMissions.DataSource = dt;
                    rptMissions.DataBind();
                    missionsGrid.Visible = true;
                    pnlEmptyState.Visible = false;
                }
                else
                {
                    missionsGrid.Visible = false;
                    pnlEmptyState.Visible = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur LoadMissions: {ex.Message}");
            }
        }

        protected string GetHeaderClass(object dateDepart)
        {
            if (dateDepart != null && dateDepart != DBNull.Value)
            {
                DateTime date = Convert.ToDateTime(dateDepart);
                TimeSpan diff = date - DateTime.Now;

                // Mission urgente si elle commence dans moins de 3 jours
                if (diff.TotalDays <= 3 && diff.TotalDays >= 0)
                {
                    return "mission-card-header urgent";
                }
            }
            return "mission-card-header";
        }

        protected string GetUrgencyBadge(object dateDepart)
        {
            if (dateDepart != null && dateDepart != DBNull.Value)
            {
                DateTime date = Convert.ToDateTime(dateDepart);
                TimeSpan diff = date - DateTime.Now;

                if (diff.TotalDays < 0)
                {
                    return "En retard";
                }
                else if (diff.TotalDays == 0)
                {
                    return "Aujourd'hui";
                }
                else if (diff.TotalDays <= 1)
                {
                    return "Demain";
                }
                else if (diff.TotalDays <= 3)
                {
                    return "Urgent";
                }
                else
                {
                    return $"Dans {(int)diff.TotalDays} jours";
                }
            }
            return "";
        }

        protected string FormatDateRange(object dateDepart, object dateRetour)
        {
            if (dateDepart != null && dateRetour != null &&
                dateDepart != DBNull.Value && dateRetour != DBNull.Value)
            {
                DateTime date1 = Convert.ToDateTime(dateDepart);
                DateTime date2 = Convert.ToDateTime(dateRetour);

                if (date1.Date == date2.Date)
                {
                    return date1.ToString("dd/MM/yyyy");
                }
                else
                {
                    return $"{date1:dd/MM/yyyy} - {date2:dd/MM/yyyy}";
                }
            }
            return "";
        }

        protected void btnValidate_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string missionId = btn.CommandArgument;
            string action = btn.CommandName;

            // Trouver le TextBox de commentaire dans le même conteneur
            RepeaterItem item = (RepeaterItem)btn.NamingContainer;
            TextBox txtComment = (TextBox)item.FindControl("txtComment");
            string comment = txtComment?.Text ?? "";

            try
            {
                // Mettre à jour le statut de la mission
                string newStatus = action == "Approve" ? "Validée" : "Rejetée";

                string queryUpdate = @"
                    UPDATE ordre_miss 
                    SET Statut = @Statut 
                    WHERE ID = @ID";

                int result = Database.ExecuteNonQuery(queryUpdate,
                    new SqlParameter("@Statut", newStatus),
                    new SqlParameter("@ID", missionId));

                // Enregistrer dans l'historique de validation
                if (result > 0)
                {
                    string queryHistory = @"
                        INSERT INTO MissionValidation 
                        (MissionID, Statut, ValidePar, DateValidation, Commentaire)
                        VALUES (@MissionID, @Statut, @ValidePar, GETDATE(), @Commentaire)";

                    Database.ExecuteNonQuery(queryHistory,
                        new SqlParameter("@MissionID", missionId),
                        new SqlParameter("@Statut", newStatus),
                        new SqlParameter("@ValidePar", User.Identity.Name ?? "Admin"),
                        new SqlParameter("@Commentaire", comment));

                    // Afficher un message de succès
                    string message = action == "Approve"
                        ? "Mission approuvée avec succès!"
                        : "Mission rejetée.";

                    ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        $"alert('{message}'); hideLoading();", true);

                    // Recharger les missions
                    LoadMissions("all");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur validation: {ex.Message}");
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('Erreur lors de la validation.'); hideLoading();", true);
            }
        }

        protected void btnDetails_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string missionId = btn.CommandArgument;
            Response.Redirect($"DetailsMission.aspx?id={missionId}");
        }

        protected void lnkTous_Click(object sender, EventArgs e)
        {
            SetActiveTab(lnkTous);
            LoadMissions("all");
        }

        protected void lnkEnAttente_Click(object sender, EventArgs e)
        {
            SetActiveTab(lnkEnAttente);
            LoadMissions("waiting");
        }

        protected void lnkUrgentes_Click(object sender, EventArgs e)
        {
            SetActiveTab(lnkUrgentes);
            LoadMissions("urgent");
        }

        protected void lnkAujourdhui_Click(object sender, EventArgs e)
        {
            SetActiveTab(lnkAujourdhui);
            LoadMissions("today");
        }

        private void SetActiveTab(LinkButton activeTab)
        {
            // Retirer la classe active de tous les tabs
            lnkTous.CssClass = "filter-tab";
            lnkEnAttente.CssClass = "filter-tab";
            lnkUrgentes.CssClass = "filter-tab";
            lnkAujourdhui.CssClass = "filter-tab";

            // Ajouter la classe active au tab sélectionné
            activeTab.CssClass = "filter-tab active";
        }
    }
}