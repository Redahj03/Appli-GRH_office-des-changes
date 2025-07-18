using System;
using System.Data;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Diplomes
{
    public partial class NiveauxInstruction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadNiveaux();
                LoadAgents();
            }
        }

        private void LoadNiveaux()
        {
            try
            {
                DataTable dt = DiplomeManager.GetAllNiveauxInstruction();
                gvNiveaux.DataSource = dt;
                gvNiveaux.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors du chargement des niveaux d'instruction: {ex.Message}", false);
            }
        }

        private void LoadAgents()
        {
            try
            {
                DataTable dt = AgentManager.GetAgentsForDropdown();
                ddlAgent.DataSource = dt;
                ddlAgent.DataTextField = "agent_display";
                ddlAgent.DataValueField = "matricule";
                ddlAgent.DataBind();
                ddlAgent.Items.Insert(0, new ListItem("-- Sélectionner un agent --", ""));
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors du chargement des agents: {ex.Message}", false);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(ddlAgent.SelectedValue))
                {
                    ShowMessage("Veuillez sélectionner un agent.", false);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNiveau.Text))
                {
                    ShowMessage("Le niveau d'instruction est obligatoire.", false);
                    return;
                }

                int niveau;
                if (!int.TryParse(txtNiveau.Text, out niveau))
                {
                    ShowMessage("Le niveau d'instruction doit être un nombre valide.", false);
                    return;
                }

                // Enregistrer le niveau
                bool success = DiplomeManager.SetNiveauInstruction(
                    ddlAgent.SelectedValue,
                    niveau
                );

                if (success)
                {
                    ShowMessage("Niveau d'instruction enregistré avec succès !", true);
                    LoadNiveaux();

                    // Réinitialiser le formulaire
                    ddlAgent.SelectedIndex = 0;
                    txtNiveau.Text = "";
                }
                else
                {
                    ShowMessage("Erreur lors de l'enregistrement du niveau d'instruction.", false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur: {ex.Message}", false);
            }
        }

        protected void gvNiveaux_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string matricule = e.CommandArgument.ToString();

            if (e.CommandName == "Delete")
            {
                try
                {
                    if (DiplomeManager.SupprimerNiveauInstruction(matricule))
                    {
                        ShowMessage("Niveau d'instruction supprimé avec succès !", true);
                        LoadNiveaux();
                    }
                    else
                    {
                        ShowMessage("Erreur lors de la suppression du niveau d'instruction.", false);
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage($"Erreur: {ex.Message}", false);
                }
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