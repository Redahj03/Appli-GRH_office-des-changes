using System;
using System.Data;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Sanctions
{
    public partial class RechercheSanction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTypesSanctions();
            }
        }

        private void LoadTypesSanctions()
        {
            try
            {
                DataTable dtTypes = SanctionManager.GetAllTypesSanctions();
                ddlTypeSanction.DataSource = dtTypes;
                ddlTypeSanction.DataTextField = "libelle_sc";
                ddlTypeSanction.DataValueField = "code_SC";
                ddlTypeSanction.DataBind();
                ddlTypeSanction.Items.Insert(0, new ListItem("-- Sélectionner un type --", ""));
            }
            catch (Exception )
            {
                // Gérer l'erreur
            }
        }

        protected void ddlSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSearchType.SelectedValue == "agent")
            {
                lblSearchLabel.Text = "Nom ou Matricule";
                txtSearch.Visible = true;
                ddlTypeSanction.Visible = false;
            }
            else
            {
                lblSearchLabel.Text = "Type de Sanction";
                txtSearch.Visible = false;
                ddlTypeSanction.Visible = true;
            }
            divResults.Visible = false;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtResults = new DataTable();

                if (ddlSearchType.SelectedValue == "agent")
                {
                    string searchTerm = txtSearch.Text.Trim();
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        dtResults = SanctionManager.RechercherSanctions(searchTerm);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(ddlTypeSanction.SelectedValue))
                    {
                        int codeSC = Convert.ToInt32(ddlTypeSanction.SelectedValue);
                        dtResults = SanctionManager.GetAgentsBySanctionType(codeSC);
                    }
                }

                GridViewResults.DataSource = dtResults;
                GridViewResults.DataBind();
                divResults.Visible = true;
            }
            catch (Exception )
            {
                // Gérer l'erreur
            }
        }
    }
}