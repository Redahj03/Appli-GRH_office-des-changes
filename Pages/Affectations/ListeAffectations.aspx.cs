using System;
using System.Data;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Affectations
{
    public partial class ListeAffectations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAffectations();
                LoadFilters();
                CheckForMessage();
            }
        }

        private void LoadAffectations()
        {
            try
            {
                DataTable dt = AffectationManager.GetAllAffectations();
                gvAffectations.DataSource = dt;
                gvAffectations.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors du chargement des lieux d'affectation: {ex.Message}", false);
            }
        }

        private void LoadFilters()
        {
            try
            {
                // Charger types d'entité
                DataTable dtTypes = AffectationManager.GetTypesEntite();
                ddlTypeEntite.DataSource = dtTypes;
                ddlTypeEntite.DataTextField = "Code_type_Entite";
                ddlTypeEntite.DataValueField = "Code_type_Entite";
                ddlTypeEntite.DataBind();
                ddlTypeEntite.Items.Insert(0, new ListItem("-- Tous les types --", ""));
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
                string lib = txtRecherche.Text.Trim();
                string typeEntite = ddlTypeEntite.SelectedValue;
                string etat = ddlEtat.SelectedValue;

                DataTable dt = AffectationManager.RechercherAffectations(lib, typeEntite, etat);
                gvAffectations.DataSource = dt;
                gvAffectations.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors de la recherche: {ex.Message}", false);
            }
        }

        protected void gvAffectations_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string codeAff = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                case "Edit":
                    Response.Redirect($"ModifierAffectation.aspx?code={codeAff}");
                    break;

                case "Delete":
                    try
                    {
                        if (AffectationManager.SupprimerAffectation(codeAff))
                        {
                            ShowMessage("Lieu d'affectation supprimé avec succès !", true);
                            LoadAffectations();
                        }
                        else
                        {
                            ShowMessage("Erreur lors de la suppression du lieu d'affectation.", false);
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
                ShowMessage("Lieu d'affectation ajouté avec succès !", true);
            }
            else if (Request.QueryString["success"] == "edit")
            {
                ShowMessage("Lieu d'affectation modifié avec succès !", true);
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