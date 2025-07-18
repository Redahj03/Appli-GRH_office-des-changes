using System;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Affectations
{
    public partial class AjouterAffectation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initialisation si nécessaire
            }
        }

        protected void btnEnregistrer_Click(object sender, EventArgs e)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(txtCodeAff.Text))
                {
                    ShowMessage("Le code est obligatoire.", false);
                    txtCodeAff.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtLib.Text))
                {
                    ShowMessage("Le libellé est obligatoire.", false);
                    txtLib.Focus();
                    return;
                }

                // Vérifier si le code existe déjà
                string codeAff = txtCodeAff.Text.Trim().ToUpper();
                if (AffectationManager.CodeExists(codeAff))
                {
                    ShowMessage($"Le code '{codeAff}' existe déjà. Veuillez en choisir un autre.", false);
                    txtCodeAff.Focus();
                    return;
                }

                // Ajouter le lieu d'affectation
                bool success = AffectationManager.AjouterAffectation(
                    codeAff,
                    txtLib.Text.Trim(),
                    txtCodeTypeEntite.Text.Trim(),
                    txtLien.Text.Trim(),
                    ddlEtat.SelectedValue
                );

                if (success)
                {
                    Response.Redirect("ListeAffectations.aspx?success=add");
                }
                else
                {
                    ShowMessage("Erreur lors de l'ajout du lieu d'affectation.", false);
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