using System;
using System.Data;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Sanctions
{
    public partial class ListeSanctions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Vérifier les messages de succès
                if (Request.QueryString["success"] == "add")
                {
                    ShowMessage("Type de sanction ajouté avec succès!", true);
                }
                else if (Request.QueryString["success"] == "edit")
                {
                    ShowMessage("Type de sanction modifié avec succès!", true);
                }
                else if (Request.QueryString["success"] == "delete")
                {
                    ShowMessage("Type de sanction supprimé avec succès!", true);
                }

                LoadSanctions();
            }
        }

        private void LoadSanctions()
        {
            try
            {
                DataTable dt = SanctionManager.GetAllTypesSanctions();
                GridViewSanctions.DataSource = dt;
                GridViewSanctions.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors du chargement des sanctions: {ex.Message}", false);
            }
        }

        protected void GridViewSanctions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                try
                {
                    int codeSC = Convert.ToInt32(e.CommandArgument);
                    bool success = SanctionManager.SupprimerTypeSanction(codeSC);

                    if (success)
                    {
                        Response.Redirect("ListeSanctions.aspx?success=delete");
                    }
                    else
                    {
                        ShowMessage("Impossible de supprimer ce type de sanction car il est utilisé.", false);
                        LoadSanctions();
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage($"Erreur lors de la suppression: {ex.Message}", false);
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