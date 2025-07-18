using System;
using System.Data;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Responsabilites
{
    public partial class HistoriqueAgent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAgents();

                // Si un matricule est passé en paramètre
                if (!string.IsNullOrEmpty(Request.QueryString["matricule"]))
                {
                    string matricule = Request.QueryString["matricule"];
                    ddlAgent.SelectedValue = matricule;
                    LoadHistorique(matricule);
                }
            }
        }

        private void LoadAgents()
        {
            try
            {
                DataTable dt = ResponsabiliteManager.GetAgentsForDropdown();
                ddlAgent.DataSource = dt;
                ddlAgent.DataTextField = "agent_display";
                ddlAgent.DataValueField = "matricule";
                ddlAgent.DataBind();
                ddlAgent.Items.Insert(0, new ListItem("-- Sélectionner un agent --", ""));
            }
            catch (Exception )
            {
                // Log error
            }
        }

        protected void btnRechercher_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlAgent.SelectedValue))
            {
                LoadHistorique(ddlAgent.SelectedValue);
            }
        }

        private void LoadHistorique(string matricule)
        {
            try
            {
                // Charger les infos de l'agent
                string query = $"SELECT [matricule], [nom], [prenom] FROM dbo.Agent WHERE [matricule] = '{matricule.Replace("'", "''")}'";
                DataTable dtAgent = Database.GetData(query);

                if (dtAgent.Rows.Count > 0)
                {
                    DataRow agent = dtAgent.Rows[0];
                    lblNomAgent.Text = $"{agent["nom"]} {agent["prenom"]}";
                    lblMatricule.Text = agent["matricule"].ToString();
                    pnlAgentInfo.Visible = true;
                }

                // Charger l'historique complet
                DataTable dt = ResponsabiliteManager.GetHistoriqueByAgent(matricule);
                rptHistorique.DataSource = dt;
                rptHistorique.DataBind();
            }
            catch (Exception )
            {
                // Log error
            }
        }
    }
}