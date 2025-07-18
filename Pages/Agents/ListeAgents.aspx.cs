using System;
using System.Data;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Agents
{
    public partial class ListeAgents : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAgents();
            }
        }

        private void LoadAgents()
        {
            DataTable dt = AgentManager.GetAllAgents();
            gvAgents.DataSource = dt;
            gvAgents.DataBind();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                LoadAgents();
            }
            else
            {
                string query = $@"SELECT matricule, nom, prenom, cin, tel, ville FROM dbo.Agent 
                                WHERE matricule LIKE '%{searchTerm}%' 
                                   OR nom LIKE '%{searchTerm}%' 
                                   OR prenom LIKE '%{searchTerm}%' 
                                   OR cin LIKE '%{searchTerm}%'
                                ORDER BY nom, prenom";

                DataTable dt = Database.GetData(query);
                gvAgents.DataSource = dt;
                gvAgents.DataBind();
            }
        }

        protected void gvAgents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string matricule = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                case "View":
                    Response.Redirect($"RechercheAgent.aspx?matricule={matricule}");
                    break;

                case "Edit":
                    Response.Redirect($"ModifierAgent.aspx?matricule={matricule}");
                    break;
            }
        }

        protected void gvAgents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string matricule = gvAgents.DataKeys[e.RowIndex].Value.ToString();

            if (AgentManager.SupprimerAgent(matricule))
            {
                LoadAgents();
                // Message de succès
            }
            else
            {
                // Message d'erreur
            }
        }
    }
}