using System;
using System.Data;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Responsabilites
{
    public partial class ListeResponsabilites : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadResponsabilites();
                LoadStatistics();
                CheckForMessage();
            }
        }

        private void LoadResponsabilites()
        {
            try
            {
                DataTable dt = ResponsabiliteManager.GetAllResponsabilites();
                gvResponsabilites.DataSource = dt;
                gvResponsabilites.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors du chargement des responsabilités: {ex.Message}", false);
            }
        }

        private void LoadStatistics()
        {
            try
            {
                DataTable dt = ResponsabiliteManager.GetStatistiques();
                if (dt.Rows.Count > 0)
                {
                    lblNbTypes.Text = dt.Rows[0]["NbTypes"].ToString();
                    lblNbAgents.Text = dt.Rows[0]["NbAgentsActifs"].ToString();
                    lblNbActives.Text = dt.Rows[0]["NbResponsabilitesActives"].ToString();
                }
            }
            catch
            {
                lblNbTypes.Text = "0";
                lblNbAgents.Text = "0";
                lblNbActives.Text = "0";
            }
        }

        protected void txtRecherche_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ResponsabiliteManager.GetAllResponsabilites();
                if (!string.IsNullOrEmpty(txtRecherche.Text))
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = $"LIBELLE LIKE '%{txtRecherche.Text.Replace("'", "''")}%'";
                    dt = dv.ToTable();
                }
                gvResponsabilites.DataSource = dt;
                gvResponsabilites.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors de la recherche: {ex.Message}", false);
            }
        }

        protected void gvResponsabilites_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int codeRS = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "Edit":
                    Response.Redirect($"ModifierResponsabilite.aspx?code={codeRS}");
                    break;

                case "Delete":
                    try
                    {
                        if (ResponsabiliteManager.SupprimerResponsabilite(codeRS))
                        {
                            ShowMessage("Type de responsabilité supprimé avec succès !", true);
                            LoadResponsabilites();
                            LoadStatistics();
                        }
                        else
                        {
                            ShowMessage("Erreur lors de la suppression. Ce type est peut-être utilisé par des agents.", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowMessage($"Erreur: {ex.Message}", false);
                    }
                    break;
            }
        }

        private void CheckForMessage()
        {
            if (Request.QueryString["success"] == "add")
            {
                ShowMessage("Type de responsabilité ajouté avec succès !", true);
            }
            else if (Request.QueryString["success"] == "edit")
            {
                ShowMessage("Type de responsabilité modifié avec succès !", true);
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