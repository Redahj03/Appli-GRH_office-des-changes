using System;
using System.Data;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Responsabilites
{
    public partial class ModifierResponsabilite : System.Web.UI.Page
    {
        private int CodeResponsabilite
        {
            get { return ViewState["CodeResponsabilite"] != null ? (int)ViewState["CodeResponsabilite"] : 0; }
            set { ViewState["CodeResponsabilite"] = value; }
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
                        CodeResponsabilite = code;
                        LoadResponsabilite(code);
                    }
                    else
                    {
                        Response.Redirect("ListeResponsabilites.aspx");
                    }
                }
                else
                {
                    Response.Redirect("ListeResponsabilites.aspx");
                }
            }
        }

        private void LoadResponsabilite(int codeRS)
        {
            try
            {
                DataTable dt = ResponsabiliteManager.GetResponsabiliteByCode(codeRS);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtCodeRS.Text = row["CODE_RS"].ToString();
                    txtLibelle.Text = row["LIBELLE"].ToString();
                }
                else
                {
                    ShowMessage("Type de responsabilité introuvable.", false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors du chargement: {ex.Message}", false);
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

                // Modifier la responsabilité
                bool success = ResponsabiliteManager.ModifierResponsabilite(
                    CodeResponsabilite,
                    txtLibelle.Text.Trim()
                );

                if (success)
                {
                    Response.Redirect("ListeResponsabilites.aspx?success=edit");
                }
                else
                {
                    ShowMessage("Erreur lors de la modification.", false);
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