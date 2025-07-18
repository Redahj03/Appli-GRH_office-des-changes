using System;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Diplomes
{
    public partial class AjouterDiplome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Générer le prochain code disponible
                int nextCode = DiplomeManager.GetNextCodeDiplome();
                txtCodeDP.Text = nextCode.ToString();
            }
        }

        protected void btnEnregistrer_Click(object sender, EventArgs e)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(txtLibelle.Text))
                {
                    ShowMessage("Le libellé du diplôme est obligatoire.", false);
                    txtLibelle.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCodeProfile.Text))
                {
                    ShowMessage("Le code profil est obligatoire.", false);
                    txtCodeProfile.Focus();
                    return;
                }

                int codeDP = Convert.ToInt32(txtCodeDP.Text);
                int codeProfile;

                if (!int.TryParse(txtCodeProfile.Text, out codeProfile))
                {
                    ShowMessage("Le code profil doit être un nombre valide.", false);
                    txtCodeProfile.Focus();
                    return;
                }

                // Vérifier si le code existe déjà (au cas où)
                if (DiplomeManager.CodeDiplomeExists(codeDP))
                {
                    // Générer un nouveau code
                    codeDP = DiplomeManager.GetNextCodeDiplome();
                    txtCodeDP.Text = codeDP.ToString();
                }

                // Ajouter le diplôme
                bool success = DiplomeManager.AjouterDiplome(
                    codeDP,
                    txtLibelle.Text.Trim(),
                    codeProfile
                );

                if (success)
                {
                    Response.Redirect("ListeDiplomes.aspx?success=add");
                }
                else
                {
                    ShowMessage("Erreur lors de l'ajout du diplôme.", false);
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