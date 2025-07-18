using System;
using System.Data;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Responsabilites
{
    public partial class AgentsResponsabilites : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAgentsResponsabilites();
                LoadFilters();
                CheckForMessage();
            }
        }

        private void LoadAgentsResponsabilites()
        {
            try
            {
                DataTable dt = ResponsabiliteManager.GetAllAgentsResponsabilites();
                gvAgentsResponsabilites.DataSource = dt;
                gvAgentsResponsabilites.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors du chargement: {ex.Message}", false);
            }
        }

        private void LoadFilters()
        {
            try
            {
                // Charger les types de responsabilités
                DataTable dtResp = ResponsabiliteManager.GetAllResponsabilites();
                ddlResponsabilite.DataSource = dtResp;
                ddlResponsabilite.DataTextField = "LIBELLE";
                ddlResponsabilite.DataValueField = "CODE_RS";
                ddlResponsabilite.DataBind();
                ddlResponsabilite.Items.Insert(0, new ListItem("-- Toutes --", ""));
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors du chargement des filtres: {ex.Message}", false);
            }
        }

        protected void btnFiltrer_Click(object sender, EventArgs e)
        {
            try
            {
                string recherche = txtRecherche.Text.Trim();
                int codeRes = string.IsNullOrEmpty(ddlResponsabilite.SelectedValue) ? 0 : Convert.ToInt32(ddlResponsabilite.SelectedValue);
                string statut = ddlStatut.SelectedValue;

                DataTable dt = ResponsabiliteManager.RechercherAgentsResponsabilites(recherche, codeRes, statut);
                gvAgentsResponsabilites.DataSource = dt;
                gvAgentsResponsabilites.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors de la recherche: {ex.Message}", false);
            }
        }

        protected void gvAgentsResponsabilites_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "History":
                    string matricule = e.CommandArgument.ToString();
                    Response.Redirect($"HistoriqueAgent.aspx?matricule={matricule}");
                    break;

                case "Delete":
                    try
                    {
                        string[] args = e.CommandArgument.ToString().Split('|');
                        string mat = args[0];
                        int codeRes = Convert.ToInt32(args[1]);
                        DateTime dateR = Convert.ToDateTime(args[2]);

                        if (ResponsabiliteManager.SupprimerAgentResponsabilite(mat, codeRes, dateR))
                        {
                            ShowMessage("Responsabilité supprimée avec succès !", true);
                            LoadAgentsResponsabilites();
                        }
                        else
                        {
                            ShowMessage("Erreur lors de la suppression.", false);
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
                ShowMessage("Responsabilité attribuée avec succès !", true);
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