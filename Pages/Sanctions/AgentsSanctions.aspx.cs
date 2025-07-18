using System;
using System.Data;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Sanctions
{
    public partial class AgentsSanctions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSanctionsAgents();
            }
        }

        private void LoadSanctionsAgents()
        {
            try
            {
                DataTable dt = SanctionManager.GetAllSanctionsAgents();
                GridViewSanctionsAgents.DataSource = dt;
                GridViewSanctionsAgents.DataBind();
            }
            catch (Exception )
            {
                // Gérer l'erreur
            }
        }

        protected void GridViewSanctionsAgents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                try
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    bool success = SanctionManager.SupprimerSanctionAgent(id);

                    if (success)
                    {
                        LoadSanctionsAgents();
                    }
                }
                catch (Exception )
                {
                    // Gérer l'erreur
                }
            }
        }
    }
}