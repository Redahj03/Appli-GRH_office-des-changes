using System;
using System.Data;
using GestionRHv2.Classes;

namespace GestionRHv2
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStatistics();
            }
        }

        private void LoadStatistics()
        {
            try
            {
                // Total des agents
                DataTable dtTotal = Database.GetData("SELECT COUNT(*) as Total FROM dbo.Agent");
                lblTotalAgents.Text = dtTotal.Rows[0]["Total"].ToString();

                // Agents actifs (non retraités)
                DataTable dtActifs = Database.GetData("SELECT COUNT(*) as Total FROM dbo.Agent WHERE retrait = '0' OR retrait IS NULL");
                lblAgentsActifs.Text = dtActifs.Rows[0]["Total"].ToString();

                // Retraités
                DataTable dtRetraites = Database.GetData("SELECT COUNT(*) as Total FROM dbo.Agent WHERE retrait = '1'");
                lblRetraites.Text = dtRetraites.Rows[0]["Total"].ToString();

                // Nombre de villes
                DataTable dtVilles = Database.GetData("SELECT COUNT(DISTINCT ville) as Total FROM dbo.Agent WHERE ville IS NOT NULL");
                lblVilles.Text = dtVilles.Rows[0]["Total"].ToString();
            }
            catch
            {
                // En cas d'erreur, afficher 0
                lblTotalAgents.Text = "0";
                lblAgentsActifs.Text = "0";
                lblRetraites.Text = "0";
                lblVilles.Text = "0";
            }
        }
    }
}