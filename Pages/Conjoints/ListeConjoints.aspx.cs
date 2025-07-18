using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Conjoints
{
    public partial class ListeConjoints : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadConjoints();
                LoadStatistics();
            }
        }

        // Charger les statistiques
        private void LoadStatistics()
        {
            try
            {
                lblTotalConjoints.Text = ConjointManager.GetTotalConjoints().ToString();
                lblWithAllocation.Text = ConjointManager.GetConjointsWithAllocation().ToString();
                lblWithoutAllocation.Text = ConjointManager.GetConjointsWithoutAllocation().ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement des statistiques: {ex.Message}");
            }
        }

        // Charger la liste des conjoints
        private void LoadConjoints(string searchTerm = "")
        {
            try
            {
                DataTable dt;
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    dt = ConjointManager.SearchConjoints(searchTerm);
                }
                else
                {
                    dt = ConjointManager.GetAllConjoints();
                }

                gvConjoints.DataSource = dt;
                gvConjoints.DataBind();
            }
            catch (Exception ex)
            {
                ShowError("Erreur lors du chargement des conjoints: " + ex.Message);
            }
        }

        // Recherche
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadConjoints(txtSearch.Text.Trim());
        }

        // Gestion des commandes du GridView
        protected void gvConjoints_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string commandArg = e.CommandArgument.ToString();
                string[] ids = commandArg.Split('|');

                if (ids.Length != 2)
                {
                    ShowError("Identifiant invalide");
                    return;
                }

                string matricule = ids[0];
                int num = Convert.ToInt32(ids[1]);

                switch (e.CommandName)
                {
                    case "ViewDetails":
                        // Rediriger vers une page de détails si elle existe
                        Response.Redirect($"DetailsConjoint.aspx?matricule={matricule}&num={num}");
                        break;

                    case "EditConjoint":
                        // Rediriger vers la page de modification
                        Response.Redirect($"AjouterConjoint.aspx?params={matricule}|{num}");
                        break;

                    case "DeleteConjoint":
                        DeleteConjoint(matricule, num);
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowError("Erreur: " + ex.Message);
            }
        }

        // Supprimer un conjoint
        private void DeleteConjoint(string matricule, int num)
        {
            try
            {
                int rowsAffected = ConjointManager.DeleteConjoint(matricule, num);

                if (rowsAffected > 0)
                {
                    // Recharger les données
                    LoadConjoints();
                    LoadStatistics();

                    // Afficher un message de succès
                    ShowSuccess("Conjoint supprimé avec succès!");
                }
                else
                {
                    ShowError("Erreur lors de la suppression.");
                }
            }
            catch (Exception ex)
            {
                ShowError("Erreur lors de la suppression: " + ex.Message);
            }
        }

        // Afficher un message d'erreur
        private void ShowError(string message)
        {
            // Vous pouvez implémenter une notification visuelle ici
            System.Diagnostics.Debug.WriteLine($"Erreur: {message}");
            // Ou utiliser un Label pour afficher le message
            // lblError.Text = message;
            // lblError.Visible = true;
        }

        // Afficher un message de succès
        private void ShowSuccess(string message)
        {
            // Vous pouvez implémenter une notification visuelle ici
            System.Diagnostics.Debug.WriteLine($"Succès: {message}");
            // Ou utiliser un Label pour afficher le message
            // lblSuccess.Text = message;
            // lblSuccess.Visible = true;
        }
    }
}