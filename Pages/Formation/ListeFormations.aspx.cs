using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Formation
{
    public partial class ListeFormations : System.Web.UI.Page
    {
        private FormationManager formationManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            formationManager = new FormationManager();

            if (!IsPostBack)
            {
                ChargerFormations();
                CalculerStatistiques();
            }
        }

        private void ChargerFormations()
        {
            try
            {
                DataTable formations;

                // Si des critères de recherche sont définis
                if (!string.IsNullOrEmpty(txtRecherche.Text) ||
                    !string.IsNullOrEmpty(txtDateDebut.Text) ||
                    !string.IsNullOrEmpty(txtDateFin.Text))
                {
                    DateTime? dateDebut = null;
                    DateTime? dateFin = null;

                    if (!string.IsNullOrEmpty(txtDateDebut.Text))
                        dateDebut = Convert.ToDateTime(txtDateDebut.Text);

                    if (!string.IsNullOrEmpty(txtDateFin.Text))
                        dateFin = Convert.ToDateTime(txtDateFin.Text);

                    formations = formationManager.RechercherFormations(txtRecherche.Text.Trim(), dateDebut, dateFin);
                }
                else
                {
                    formations = formationManager.ObtenirToutesLesFormations();
                }

                gvFormations.DataSource = formations;
                gvFormations.DataBind();
            }
            catch (Exception ex)
            {
                AfficherMessage("Erreur lors du chargement des formations : " + ex.Message, "danger");
            }
        }

        private void CalculerStatistiques()
        {
            try
            {
                // Total des formations
                string queryTotal = "SELECT COUNT(DISTINCT CONCAT(CAST(Objet AS VARCHAR), '_', CAST(Date_Form AS VARCHAR))) as Total FROM Formation";
                object totalFormations = Database.ExecuteScalar(queryTotal);
                lblTotalFormations.Text = totalFormations?.ToString() ?? "0";

                // Formations cette année
                string queryAnnee = @"
                    SELECT COUNT(DISTINCT CONCAT(CAST(Objet AS VARCHAR), '_', CAST(Date_Form AS VARCHAR))) as Total 
                    FROM Formation 
                    WHERE YEAR(Date_Form) = YEAR(GETDATE())";
                object formationsAnnee = Database.ExecuteScalar(queryAnnee);
                lblFormationsAnnee.Text = formationsAnnee?.ToString() ?? "0";

                // Total des participants uniques
                string queryParticipants = "SELECT COUNT(DISTINCT Matricule) as Total FROM Agent_Form";
                object totalParticipants = Database.ExecuteScalar(queryParticipants);
                lblTotalParticipants.Text = totalParticipants?.ToString() ?? "0";

                // Total des inscriptions
                string queryInscriptions = "SELECT COUNT(*) as Total FROM Agent_Form";
                object totalInscriptions = Database.ExecuteScalar(queryInscriptions);
                lblTotalInscriptions.Text = totalInscriptions?.ToString() ?? "0";
            }
            catch (Exception ex)
            {
                // En cas d'erreur, afficher 0 par défaut
                lblTotalFormations.Text = "0";
                lblFormationsAnnee.Text = "0";
                lblTotalParticipants.Text = "0";
                lblTotalInscriptions.Text = "0";

                System.Diagnostics.Debug.WriteLine("Erreur calcul statistiques : " + ex.Message);
            }
        }

        protected void GvFormations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                // Afficher le nombre d'inscrits
                Label lblNbInscrits = (Label)e.Row.FindControl("lblNbInscrits");
                if (lblNbInscrits != null && drv["NbInscrits"] != DBNull.Value)
                {
                    int nbInscrits = Convert.ToInt32(drv["NbInscrits"]);
                    int effectifMax = drv["Effect"] != DBNull.Value ? Convert.ToInt32(drv["Effect"]) : 0;

                    if (effectifMax > 0)
                    {
                        lblNbInscrits.Text = $"{nbInscrits}/{effectifMax}";

                        // Colorer en rouge si complet
                        if (nbInscrits >= effectifMax)
                        {
                            lblNbInscrits.Style.Add("color", "#e74c3c");
                            lblNbInscrits.Style.Add("font-weight", "bold");
                        }
                    }
                    else
                    {
                        lblNbInscrits.Text = nbInscrits.ToString();
                    }
                }

                // Définir le statut
                Label lblStatut = (Label)e.Row.FindControl("lblStatut");
                if (lblStatut != null && drv["Date_Form"] != DBNull.Value)
                {
                    DateTime dateFormation = Convert.ToDateTime(drv["Date_Form"]);
                    DateTime today = DateTime.Today;

                    if (dateFormation.Date < today)
                    {
                        lblStatut.Text = "<span class='status-badge status-terminee'>Terminée</span>";
                    }
                    else if (dateFormation.Date == today)
                    {
                        lblStatut.Text = "<span class='status-badge status-en-cours'>En cours</span>";
                    }
                    else
                    {
                        lblStatut.Text = "<span class='status-badge status-a-venir'>À venir</span>";
                    }
                }
            }
        }

        protected void GvFormations_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string[] args = e.CommandArgument.ToString().Split(',');
                if (args.Length >= 2)
                {
                    string objet = args[0];
                    DateTime dateForm = Convert.ToDateTime(args[1]);

                    switch (e.CommandName)
                    {
                        case "View":
                            Response.Redirect($"DetailFormation.aspx?objet={objet}&date={dateForm:yyyy-MM-dd}");
                            break;

                        case "Edit":
                            Response.Redirect($"ModifierFormation.aspx?objet={objet}&date={dateForm:yyyy-MM-dd}");
                            break;

                        case "Delete":
                            SupprimerFormation(Convert.ToInt32(objet), dateForm);
                            break;

                        case "Inscrire":
                            Response.Redirect($"InscrireAgents.aspx?objet={objet}&date={dateForm:yyyy-MM-dd}");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessage("Erreur : " + ex.Message, "danger");
            }
        }

        private void SupprimerFormation(int objet, DateTime dateForm)
        {
            try
            {
                // Vérifier d'abord s'il y a des agents inscrits
                string checkQuery = "SELECT COUNT(*) FROM Agent_Form WHERE Objet = @Objet AND date_form = @DateForm";
                var checkParams = new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@Objet", objet),
                    new System.Data.SqlClient.SqlParameter("@DateForm", dateForm)
                };

                object result = Database.ExecuteScalar(checkQuery, checkParams);
                int agentsInscrits = result != null ? Convert.ToInt32(result) : 0;

                if (agentsInscrits > 0)
                {
                    AfficherMessage($"Impossible de supprimer cette formation car {agentsInscrits} agent(s) y sont inscrits. " +
                                   "Veuillez d'abord désinscrire tous les agents.", "warning");
                    return;
                }

                // Si pas d'agents inscrits, procéder à la suppression
                if (formationManager.SupprimerFormation(objet, dateForm))
                {
                    AfficherMessage("Formation supprimée avec succès.", "success");
                    ChargerFormations();
                    CalculerStatistiques();
                }
                else
                {
                    AfficherMessage("Erreur lors de la suppression de la formation.", "danger");
                }
            }
            catch (Exception ex)
            {
                AfficherMessage("Erreur : " + ex.Message, "danger");
            }
        }

        protected void btnRechercher_Click(object sender, EventArgs e)
        {
            ChargerFormations();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtRecherche.Text = "";
            txtDateDebut.Text = "";
            txtDateFin.Text = "";
            ChargerFormations();
        }

        private void AfficherMessage(string message, string type)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;

            // Définir la classe CSS en fonction du type
            switch (type)
            {
                case "success":
                    pnlMessage.CssClass = "alert alert-success";
                    break;
                case "danger":
                    pnlMessage.CssClass = "alert alert-danger";
                    break;
                case "warning":
                    pnlMessage.CssClass = "alert alert-warning";
                    break;
                default:
                    pnlMessage.CssClass = "alert";
                    break;
            }
        }
    }
}