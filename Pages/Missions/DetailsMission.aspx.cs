using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Missions
{
    public partial class DetailsMission : System.Web.UI.Page
    {
        private string MissionId
        {
            get { return Request.QueryString["id"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(MissionId))
                {
                    Response.Redirect("ListeMissions.aspx");
                    return;
                }

                LoadMissionDetails();
                LoadValidationHistory();
            }
        }

        private void LoadMissionDetails()
        {
            try
            {
                string query = @"
                    SELECT 
                        om.ID,
                        om.matr as Matricule,
                        CONCAT(a.Nom, ' ', a.Prénom) as NomAgent,
                        g.LIBELLE as Grade,
                        af.Libelle as Fonction,
                        om.ville as Destination,
                        om.Objet_mis as ObjetMission,
                        om.date_d as DateDepart,
                        om.Heure_d as HeureDepart,
                        om.date_r as DateRetour,
                        om.Heure_r as HeureRetour,
                        om.trpt as Transport,
                        om.Détail_trpt as DetailTransport,
                        ISNULL(om.Statut, 'En attente') as Statut,
                        m.Kilométrage,
                        m.Taux as TauxKm,
                        m.Montant
                    FROM ordre_miss om
                    LEFT JOIN Agent a ON om.matr = a.Matricule
                    LEFT JOIN GRADE g ON a.Grade = g.CODEGRADE
                    LEFT JOIN AGENT_FONCTION_LIEN afl ON a.Matricule = afl.Matricule
                    LEFT JOIN AGENT_FONCTION af ON afl.CodeFonction = af.Code
                    LEFT JOIN Missions m ON om.matr = m.Matricule 
                        AND CAST(om.date_d as DATE) = CAST(m.Date_deplacement as DATE)
                    WHERE om.ID = @ID";

                DataTable dt = Database.GetDataWithParams(query,
                    new SqlParameter("@ID", MissionId));

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    // Informations générales
                    lblMissionID.Text = row["ID"].ToString();
                    lblStatut.Text = row["Statut"].ToString();

                    // Informations de l'agent
                    lblMatricule.Text = row["Matricule"].ToString();
                    lblNomAgent.Text = row["NomAgent"]?.ToString() ?? "N/A";
                    lblGrade.Text = row["Grade"]?.ToString() ?? "N/A";
                    lblFonction.Text = row["Fonction"]?.ToString() ?? "N/A";

                    // Détails de la mission
                    lblDestination.Text = row["Destination"]?.ToString() ?? "N/A";
                    lblObjetMission.Text = row["ObjetMission"]?.ToString() ?? "N/A";
                    lblTransport.Text = row["Transport"]?.ToString() ?? "N/A";
                    lblDetailTransport.Text = row["DetailTransport"]?.ToString() ?? "N/A";

                    // Dates et durée
                    if (row["DateDepart"] != DBNull.Value)
                    {
                        DateTime dateDepart = Convert.ToDateTime(row["DateDepart"]);
                        DateTime heureDepart = Convert.ToDateTime(row["HeureDepart"]);
                        lblDateDepart.Text = $"{dateDepart:dd/MM/yyyy} à {heureDepart:HH:mm}";
                    }

                    if (row["DateRetour"] != DBNull.Value)
                    {
                        DateTime dateRetour = Convert.ToDateTime(row["DateRetour"]);
                        DateTime heureRetour = Convert.ToDateTime(row["HeureRetour"]);
                        lblDateRetour.Text = $"{dateRetour:dd/MM/yyyy} à {heureRetour:HH:mm}";

                        // Calculer la durée
                        if (row["DateDepart"] != DBNull.Value)
                        {
                            DateTime dateDepart = Convert.ToDateTime(row["DateDepart"]);
                            TimeSpan duree = dateRetour - dateDepart;
                            int jours = (int)duree.TotalDays + 1;
                            lblDuree.Text = $"{jours} jour(s)";
                        }
                    }

                    // Informations financières (si voiture personnelle)
                    if (row["Transport"]?.ToString() == "Voiture personnelle" &&
                        row["Kilométrage"] != DBNull.Value)
                    {
                        pnlFinancial.Visible = true;
                        lblKilometrage.Text = Convert.ToDecimal(row["Kilométrage"]).ToString("0");

                        if (row["TauxKm"] != DBNull.Value)
                            lblTauxKm.Text = Convert.ToDecimal(row["TauxKm"]).ToString("0.00");

                        if (row["Montant"] != DBNull.Value)
                            lblMontantTotal.Text = Convert.ToDecimal(row["Montant"]).ToString("0.00");
                    }

                    // Gérer la visibilité du bouton Modifier selon le statut
                    string statut = row["Statut"]?.ToString() ?? "En attente";
                    btnModifier.Visible = (statut == "En attente");
                }
                else
                {
                    // Mission non trouvée
                    Response.Redirect("ListeMissions.aspx");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur LoadMissionDetails: {ex.Message}");
            }
        }

        private void LoadValidationHistory()
        {
            try
            {
                string query = @"
                    SELECT 
                        Statut,
                        ValidePar,
                        DateValidation,
                        Commentaire
                    FROM MissionValidation
                    WHERE MissionID = @MissionID
                    ORDER BY DateValidation DESC";

                DataTable dt = Database.GetDataWithParams(query,
                    new SqlParameter("@MissionID", MissionId));

                if (dt.Rows.Count > 0)
                {
                    pnlValidation.Visible = true;
                    rptValidation.DataSource = dt;
                    rptValidation.DataBind();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur LoadValidationHistory: {ex.Message}");
                // Ne pas afficher d'erreur si la table n'existe pas encore
            }
        }

        protected string GetStatusIcon()
        {
            string status = lblStatut.Text.ToLower();
            switch (status)
            {
                case "en attente":
                    return "fas fa-clock";
                case "validée":
                    return "fas fa-check-circle";
                case "rejetée":
                    return "fas fa-times-circle";
                case "en cours":
                    return "fas fa-spinner";
                default:
                    return "fas fa-circle";
            }
        }

        protected string GetValidationIcon(string status)
        {
            switch (status.ToLower())
            {
                case "validée":
                    return "fas fa-check";
                case "rejetée":
                    return "fas fa-times";
                case "en attente":
                    return "fas fa-clock";
                default:
                    return "fas fa-info";
            }
        }

        protected void btnModifier_Click(object sender, EventArgs e)
        {
            Response.Redirect($"ModifierMission.aspx?id={MissionId}");
        }

        protected void btnRetour_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListeMissions.aspx");
        }
    }
}