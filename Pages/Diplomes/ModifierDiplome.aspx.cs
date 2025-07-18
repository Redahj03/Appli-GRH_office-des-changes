using System;
using System.Data;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Diplomes
{
    public partial class ModifierDiplome : System.Web.UI.Page
    {
        private int CodeDiplome
        {
            get { return ViewState["CodeDiplome"] != null ? (int)ViewState["CodeDiplome"] : 0; }
            set { ViewState["CodeDiplome"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["code"] != null)
                {
                    int code;
                    if (int.TryParse(Request.QueryString["code"], out code))
                    {
                        CodeDiplome = code;
                        LoadDiplome(code);
                    }
                    else
                    {
                        Response.Redirect("ListeDiplomes.aspx");
                    }
                }
                else
                {
                    Response.Redirect("ListeDiplomes.aspx");
                }
            }
        }

        private void LoadDiplome(int codeDP)
        {
            try
            {
                DataTable dt = DiplomeManager.GetDiplomeByCode(codeDP);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    txtCodeDP.Text = row["CODE_DP"].ToString();
                    txtLibelle.Text = row["LIBELLE"].ToString();
                    txtCodeProfile.Text = row["code_profile"].ToString();
                }
                else
                {
                    ShowMessage("Diplôme introuvable.", false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors du chargement du diplôme: {ex.Message}", false);
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

                int codeProfile;
                if (!int.TryParse(txtCodeProfile.Text, out codeProfile))
                {
                    ShowMessage("Le code profil doit être un nombre valide.", false);
                    txtCodeProfile.Focus();
                    return;
                }

                // Modifier le diplôme
                bool success = DiplomeManager.ModifierDiplome(
                    CodeDiplome,
                    txtLibelle.Text.Trim(),
                    codeProfile
                );

                if (success)
                {
                    Response.Redirect("ListeDiplomes.aspx?success=edit");
                }
                else
                {
                    ShowMessage("Erreur lors de la modification du diplôme.", false);
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