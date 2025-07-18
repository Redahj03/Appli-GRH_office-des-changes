using System;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Responsabilites
{
    public partial class AjouterResponsabilite : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Générer le prochain code disponible
                int nextCode = ResponsabiliteManager.GetNextCodeResponsabilite();
                txtCodeRS.Text = nextCode.ToString();
            }
        }

        protected void btnEnregistrer_Click(object sender, EventArgs e)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(txtLibelle.Text))
                {
                    ShowMessage("Le libellé est obligatoire.", false);
                    txtLibelle.Focus();
                    return;
                }

                int codeRS = Convert.ToInt32(txtCodeRS.Text);

                // Vérifier si le code existe déjà (au cas où)
                if (ResponsabiliteManager.CodeResponsabiliteExists(codeRS))
                {
                    // Générer un nouveau code
                    codeRS = ResponsabiliteManager.GetNextCodeResponsabilite();
                    txtCodeRS.Text = codeRS.ToString();
                }

                // Ajouter la responsabilité
                bool success = ResponsabiliteManager.AjouterResponsabilite(
                    codeRS,
                    txtLibelle.Text.Trim()
                );

                if (success)
                {
                    Response.Redirect("ListeResponsabilites.aspx?success=add");
                }
                else
                {
                    ShowMessage("Erreur lors de l'ajout du type de responsabilité.", false);
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