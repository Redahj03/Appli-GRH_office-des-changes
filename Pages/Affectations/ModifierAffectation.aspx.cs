using System;
using System.Data;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Affectations
{
    public partial class ModifierAffectation : System.Web.UI.Page
    {
        private string CodeAffectation
        {
            get { return ViewState["CodeAffectation"] != null ? ViewState["CodeAffectation"].ToString() : ""; }
            set { ViewState["CodeAffectation"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["code"] != null)
                {
                    string code = Request.QueryString["code"];
                    CodeAffectation = code;
                    LoadAffectation(code);
                }
                else
                {
                    Response.Redirect("ListeAffectations.aspx");
                }
            }
        }

        private void LoadAffectation(string codeAff)
        {
            try
            {
                DataTable dt = AffectationManager.GetAffectationByCode(codeAff);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    txtCodeAff.Text = row["CODE_AFF"].ToString();
                    txtLib.Text = row["LIB"].ToString();
                    txtCodeTypeEntite.Text = row["Code_type_Entite"].ToString();
                    txtLien.Text = row["lien"].ToString();

                    if (row["Etat"] != DBNull.Value)
                        ddlEtat.SelectedValue = row["Etat"].ToString();
                }
                else
                {
                    ShowMessage("Lieu d'affectation introuvable.", false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors du chargement du lieu d'affectation: {ex.Message}", false);
            }
        }

        protected void btnEnregistrer_Click(object sender, EventArgs e)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(txtLib.Text))
                {
                    ShowMessage("Le libellé est obligatoire.", false);
                    txtLib.Focus();
                    return;
                }

                // Modifier le lieu d'affectation
                bool success = AffectationManager.ModifierAffectation(
                    CodeAffectation,
                    txtLib.Text.Trim(),
                    txtCodeTypeEntite.Text.Trim(),
                    txtLien.Text.Trim(),
                    ddlEtat.SelectedValue
                );

                if (success)
                {
                    Response.Redirect("ListeAffectations.aspx?success=edit");
                }
                else
                {
                    ShowMessage("Erreur lors de la modification du lieu d'affectation.", false);
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