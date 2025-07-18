using System;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Sanctions
{
    public partial class AjouterSanction : System.Web.UI.Page
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
                // Récupérer les valeurs
                int codeSC = Convert.ToInt32(txtCodeSC.Text.Trim());
                string libelle = txtLibelle.Text.Trim();
                int jours = string.IsNullOrEmpty(txtJours.Text) ? 0 : Convert.ToInt32(txtJours.Text.Trim());

                // Vérifier si le code existe déjà
                if (SanctionManager.CodeSanctionExiste(codeSC))
                {
                    ShowMessage("Ce code de sanction existe déjà. Veuillez en choisir un autre.", false);
                    txtCodeSC.Focus();
                    return;
                }

                // Ajouter le type de sanction
                bool success = SanctionManager.AjouterTypeSanction(codeSC, libelle, jours);

                if (success)
                {
                    Response.Redirect("ListeSanctions.aspx?success=add");
                }
                else
                {
                    ShowMessage("Erreur lors de l'ajout du type de sanction.", false);
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