using System;
using System.Data;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.AgentsFonctions
{
    public partial class ListeAgentsFonctions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAgentsFonctions();
                LoadFilters();
                LoadStatistics();
                CheckForMessage();
            }
        }

        private void LoadAgentsFonctions()
        {
            try
            {
                DataTable dt = AgentFonctionManager.GetAllAgentsFonctions();
                gvAgentsFonctions.DataSource = dt;
                gvAgentsFonctions.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors du chargement des fonctions: {ex.Message}", false);
            }
        }

        private void LoadFilters()
        {
            try
            {
                // Charger les fonctions distinctes
                DataTable dtFonctions = AgentFonctionManager.GetFonctionsDistinctes();
                ddlFonction.DataSource = dtFonctions;
                ddlFonction.DataTextField = "fonction";
                ddlFonction.DataValueField = "fonction";
                ddlFonction.DataBind();
                ddlFonction.Items.Insert(0, new ListItem("-- Toutes les fonctions --", ""));
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors du chargement des filtres: {ex.Message}", false);
            }
        }

        private void LoadStatistics()
        {
            try
            {
                DataTable dt = AgentFonctionManager.GetStatistiques();
                if (dt.Rows.Count > 0)
                {
                    lblTotalAgents.Text = dt.Rows[0]["TotalAgents"].ToString();
                    lblNbFonctions.Text = dt.Rows[0]["NbFonctions"].ToString();
                    lblNbAffectations.Text = dt.Rows[0]["NbAffectations"].ToString();
                }
            }
            catch
            {
                lblTotalAgents.Text = "0";
                lblNbFonctions.Text = "0";
                lblNbAffectations.Text = "0";
            }
        }

        protected void btnFiltrer_Click(object sender, EventArgs e)
        {
            try
            {
                string recherche = txtRecherche.Text.Trim();
                string fonction = ddlFonction.SelectedValue;

                DataTable dt = AgentFonctionManager.RechercherAgentsFonctions(recherche, fonction);
                gvAgentsFonctions.DataSource = dt;
                gvAgentsFonctions.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors de la recherche: {ex.Message}", false);
            }
        }

        protected void gvAgentsFonctions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int numOrd = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "Edit":
                    Response.Redirect($"ModifierAgentFonction.aspx?num={numOrd}");
                    break;

                case "Delete":
                    try
                    {
                        if (AgentFonctionManager.SupprimerAgentFonction(numOrd))
                        {
                            ShowMessage("Fonction supprimée avec succès !", true);
                            LoadAgentsFonctions();
                            LoadStatistics();
                        }
                        else
                        {
                            ShowMessage("Erreur lors de la suppression de la fonction.", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowMessage($"Erreur: {ex.Message}", false);
                    }
                    break;
            }
        }

        private void CheckForMessage()
        {
            if (Request.QueryString["success"] == "add")
            {
                ShowMessage("Fonction ajoutée avec succès !", true);
            }
            else if (Request.QueryString["success"] == "edit")
            {
                ShowMessage("Fonction modifiée avec succès !", true);
            }
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            string iconClass = isSuccess ? "fas fa-check-circle" : "fas fa-exclamation-circle";
            lblMessage.Text = $"<i class='{iconClass}'></i> {message}";
            pnlMessage.CssClass = $"alert alert-{(isSuccess ? "success" : "error")}";
            pnlMessage.Visible = true;
        }
    }
}