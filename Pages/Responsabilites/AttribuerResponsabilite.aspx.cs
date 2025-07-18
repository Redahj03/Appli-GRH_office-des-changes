using System;
using System.Data;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Responsabilites
{
    public partial class AttribuerResponsabilite : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAgents();
                LoadResponsabilites();
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        private void LoadAgents()
        {
            try
            {
                DataTable dt = ResponsabiliteManager.GetAgentsForDropdown();
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

        private void LoadResponsabilites()
        {
            try
            {
                DataTable dt = ResponsabiliteManager.GetAllResponsabilites();
                ddlResponsabilite.DataSource = dt;
                ddlResponsabilite.DataTextField = "LIBELLE";
                ddlResponsabilite.DataValueField = "CODE_RS";
                ddlResponsabilite.DataBind();
                ddlResponsabilite.Items.Insert(0, new ListItem("-- Sélectionner --", ""));
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors du chargement des responsabilités: {ex.Message}", false);
            }
        }

        protected void btnEnregistrer_Click(object sender, EventArgs e)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(ddlAgent.SelectedValue))
                {
                    ShowMessage("Veuillez sélectionner un agent.", false);
                    return;
                }

                if (string.IsNullOrWhiteSpace(ddlResponsabilite.SelectedValue))
                {
                    ShowMessage("Veuillez sélectionner une responsabilité.", false);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDate.Text))
                {
                    ShowMessage("La date est obligatoire.", false);
                    return;
                }

                DateTime dateR;
                if (!DateTime.TryParse(txtDate.Text, out dateR))
                {
                    ShowMessage("Format de date invalide.", false);
                    return;
                }

                int codeA = 0;
                if (!string.IsNullOrWhiteSpace(txtCodeA.Text))
                {
                    if (!int.TryParse(txtCodeA.Text, out codeA))
                    {
                        ShowMessage("Code A doit être un nombre.", false);
                        return;
                    }
                }

                // Attribuer la responsabilité
                bool success = ResponsabiliteManager.AttribuerResponsabilite(
                    ddlAgent.SelectedValue,
                    Convert.ToInt32(ddlResponsabilite.SelectedValue),
                    dateR,
                    codeA,
                    ddlActuel.SelectedValue
                );

                if (success)
                {
                    Response.Redirect("AgentsResponsabilites.aspx?success=add");
                }
                else
                {
                    ShowMessage("Erreur lors de l'attribution de la responsabilité.", false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur: {ex.Message}", false);
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