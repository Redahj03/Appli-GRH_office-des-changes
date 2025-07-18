using System;
using System.Data;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Diplomes
{
    public partial class ListeDiplomes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDiplomes();
                LoadStatistics();
                CheckForMessage();
            }
        }

        private void LoadDiplomes()
        {
            try
            {
                DataTable dt = DiplomeManager.GetAllDiplomes();
                gvDiplomes.DataSource = dt;
                gvDiplomes.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors du chargement des diplômes: {ex.Message}", false);
            }
        }

        private void LoadStatistics()
        {
            try
            {
                DataTable dt = DiplomeManager.GetStatistiquesDiplomes();
                if (dt.Rows.Count > 0)
                {
                    lblNbDiplomes.Text = dt.Rows[0]["NbDiplomes"].ToString();
                    lblNbAgents.Text = dt.Rows[0]["NbAgentsAvecNiveau"].ToString();
                }
            }
            catch
            {
                lblNbDiplomes.Text = "0";
                lblNbAgents.Text = "0";
            }
        }

        protected void txtRecherche_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = DiplomeManager.GetAllDiplomes();
                if (!string.IsNullOrEmpty(txtRecherche.Text))
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = $"LIBELLE LIKE '%{txtRecherche.Text.Replace("'", "''")}%'";
                    dt = dv.ToTable();
                }
                gvDiplomes.DataSource = dt;
                gvDiplomes.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors de la recherche: {ex.Message}", false);
            }
        }

        protected void gvDiplomes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int codeDP = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "Edit":
                    Response.Redirect($"ModifierDiplome.aspx?code={codeDP}");
                    break;

                case "Delete":
                    try
                    {
                        if (DiplomeManager.SupprimerDiplome(codeDP))
                        {
                            ShowMessage("Diplôme supprimé avec succès !", true);
                            LoadDiplomes();
                            LoadStatistics();
                        }
                        else
                        {
                            ShowMessage("Erreur lors de la suppression du diplôme.", false);
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
                ShowMessage("Diplôme ajouté avec succès !", true);
            }
            else if (Request.QueryString["success"] == "edit")
            {
                ShowMessage("Diplôme modifié avec succès !", true);
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