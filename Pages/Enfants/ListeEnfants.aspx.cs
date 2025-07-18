using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Enfants
{
    public partial class ListeEnfants : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ChargerStatistiques();
                ChargerEnfants();

                // Afficher un message si on revient d'une action
                if (Session["Message"] != null)
                {
                    AfficherMessage(Session["Message"].ToString(), Session["TypeMessage"]?.ToString() ?? "success");
                    Session.Remove("Message");
                    Session.Remove("TypeMessage");
                }
            }
        }

        private void ChargerStatistiques()
        {
            try
            {
                var stats = EnfantManager.GetStatistiques();

                lblTotalEnfants.Text = stats["TotalEnfants"].ToString();
                lblGarcons.Text = stats["NombreGarcons"].ToString();
                lblFilles.Text = stats["NombreFilles"].ToString();
                lblAgeMoyen.Text = Math.Round(Convert.ToDouble(stats["AgeMoyen"]), 1).ToString();
            }
            catch (Exception ex)
            {
                AfficherMessage($"Erreur lors du chargement des statistiques : {ex.Message}", "danger");
            }
        }

        private void ChargerEnfants(string recherche = null)
        {
            try
            {
                List<Enfant> enfants;

                if (!string.IsNullOrWhiteSpace(recherche))
                {
                    enfants = EnfantManager.RechercherEnfants(recherche);
                }
                else
                {
                    enfants = EnfantManager.GetAllEnfants();
                }

                gvEnfants.DataSource = enfants;
                gvEnfants.DataBind();
            }
            catch (Exception ex)
            {
                AfficherMessage($"Erreur lors du chargement des enfants : {ex.Message}", "danger");
            }
        }

        protected void btnRechercher_Click(object sender, EventArgs e)
        {
            ChargerEnfants(txtRecherche.Text.Trim());
        }

        protected void gvEnfants_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View" || e.CommandName == "Edit" || e.CommandName == "Delete")
            {
                string[] args = e.CommandArgument.ToString().Split(',');
                string matricule = args[0];
                int num = Convert.ToInt32(args[1]);

                switch (e.CommandName)
                {
                    case "View":
                        Response.Redirect($"DetailEnfant.aspx?matricule={matricule}&num={num}");
                        break;

                    case "Edit":
                        Response.Redirect($"ModifierEnfant.aspx?matricule={matricule}&num={num}");
                        break;

                    case "Delete":
                        SupprimerEnfant(matricule, num);
                        break;
                }
            }
        }

        private void SupprimerEnfant(string matricule, int num)
        {
            try
            {
                bool success = EnfantManager.SupprimerEnfant(matricule, num);

                if (success)
                {
                    AfficherMessage("Enfant supprimé avec succès.", "success");
                    ChargerStatistiques();
                    ChargerEnfants();
                }
                else
                {
                    AfficherMessage("Erreur lors de la suppression de l'enfant.", "danger");
                }
            }
            catch (Exception ex)
            {
                AfficherMessage($"Erreur : {ex.Message}", "danger");
            }
        }

        protected void gvEnfants_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Vous pouvez ajouter des styles conditionnels ici si nécessaire
                Enfant enfant = (Enfant)e.Row.DataItem;

                // Par exemple, mettre en évidence les enfants scolarisés
                if (enfant.Scolarise == "O")
                {
                    e.Row.Cells[5].Style["font-weight"] = "bold";
                    e.Row.Cells[5].Style["color"] = "green";
                }
            }
        }

        private void AfficherMessage(string message, string type)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;

            // Appliquer la classe CSS appropriée
            pnlMessage.CssClass = $"alert alert-{type}";

            // Ajouter l'icône appropriée
            string icon = "";
            switch (type)
            {
                case "success":
                    icon = "✅";
                    break;
                case "danger":
                    icon = "❌";
                    break;
                case "warning":
                    icon = "⚠️";
                    break;
                case "info":
                    icon = "ℹ️";
                    break;
            }

            if (!string.IsNullOrEmpty(icon))
            {
                lblMessage.Text = $"{icon} {message}";
            }
        }
    }
}