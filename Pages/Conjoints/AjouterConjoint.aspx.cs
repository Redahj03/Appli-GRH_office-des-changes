using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Conjoints
{
    public partial class AjouterConjoint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployees();

                // Vérifier si c'est une modification
                if (Request.QueryString["id"] != null)
                {
                    int conjointId;
                    if (int.TryParse(Request.QueryString["id"], out conjointId))
                    {
                        LoadConjointData(conjointId);
                    }
                }
            }
        }

        // Charger la liste des employés
        private void LoadEmployees()
        {
            try
            {
                DataTable dt = ConjointManager.GetEmployeesForDropdown();

                ddlEmploye.DataSource = dt;
                ddlEmploye.DataTextField = "nom_complet";
                ddlEmploye.DataValueField = "matricule";
                ddlEmploye.DataBind();

                ddlEmploye.Items.Insert(0, new ListItem("-- Sélectionner un employé --", ""));
            }
            catch (Exception ex)
            {
                ShowMessage("Erreur lors du chargement des employés: " + ex.Message, false);
            }
        }

        // Charger les données d'un conjoint pour modification
        private void LoadConjointData(int conjointId)
        {
            try
            {
                // Si l'ID passé est en fait le num, on va chercher les données
                string[] queryParams = Request.QueryString["params"]?.Split('|');
                Conjoint conjoint = null;

                if (queryParams != null && queryParams.Length == 2)
                {
                    string matricule = queryParams[0];
                    int num = Convert.ToInt32(queryParams[1]);
                    conjoint = ConjointManager.GetConjointByMatriculeAndNum(matricule, num);
                }
                else
                {
                    // Essayer de récupérer par num uniquement
                    conjoint = ConjointManager.GetConjointById(conjointId);
                }

                if (conjoint != null)
                {
                    hdnConjointId.Value = conjoint.Matricule + "|" + conjoint.Num;

                    // Remplir les champs
                    if (!string.IsNullOrEmpty(conjoint.Matricule))
                        ddlEmploye.SelectedValue = conjoint.Matricule;

                    txtNom.Text = conjoint.Nom ?? "";
                    txtPrenom.Text = conjoint.Prenom ?? "";
                    txtProfession.Text = conjoint.Prof ?? "";

                    if (conjoint.DateMariage.HasValue)
                        txtDateMariage.Text = conjoint.DateMariage.Value.ToString("yyyy-MM-dd");

                    if (conjoint.AllocationFamiliale.HasValue)
                        ddlAllocation.SelectedValue = conjoint.AllocationFamiliale.Value ? "1" : "0";

                    // Modifier le titre de la page
                    lblPageTitle.Text = "Modifier le conjoint";
                    btnSave.Text = "Mettre à jour";
                }
                else
                {
                    ShowMessage("Conjoint introuvable.", false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Erreur lors du chargement des données: " + ex.Message, false);
            }
        }

        // Sauvegarder (Ajouter ou Modifier)
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Pas de validation obligatoire - tous les champs peuvent être vides
                Conjoint conjoint = new Conjoint
                {
                    Matricule = ddlEmploye.SelectedValue,
                    Nom = txtNom.Text.Trim(),
                    Prenom = txtPrenom.Text.Trim(),
                    Prof = txtProfession.Text.Trim(),
                    DateMariage = string.IsNullOrEmpty(txtDateMariage.Text) ? (DateTime?)null : Convert.ToDateTime(txtDateMariage.Text),
                    AllocationFamiliale = string.IsNullOrEmpty(ddlAllocation.SelectedValue) ? (bool?)null : Convert.ToBoolean(Convert.ToInt32(ddlAllocation.SelectedValue))
                };

                int rowsAffected;
                if (string.IsNullOrEmpty(hdnConjointId.Value))
                {
                    // Insertion - le numéro sera généré automatiquement
                    rowsAffected = ConjointManager.AddConjoint(conjoint);
                }
                else
                {
                    // Mise à jour - récupérer matricule et num depuis le hidden field
                    string[] ids = hdnConjointId.Value.Split('|');
                    if (ids.Length == 2)
                    {
                        conjoint.Matricule = ids[0];
                        conjoint.Num = Convert.ToInt32(ids[1]);
                        rowsAffected = ConjointManager.UpdateConjoint(conjoint);
                    }
                    else
                    {
                        ShowMessage("Identifiant du conjoint invalide.", false);
                        return;
                    }
                }

                if (rowsAffected > 0)
                {
                    string message = string.IsNullOrEmpty(hdnConjointId.Value) ?
                        "Conjoint ajouté avec succès!" :
                        "Conjoint modifié avec succès!";

                    ShowMessage(message, true);

                    // Rediriger vers la liste après un court délai
                    Response.AddHeader("REFRESH", "2;URL=ListeConjoints.aspx");
                }
                else
                {
                    ShowMessage("Aucune modification effectuée.", false);
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 547) // Violation de contrainte de clé étrangère
                {
                    ShowMessage("L'employé sélectionné n'existe pas dans la base de données.", false);
                }
                else
                {
                    ShowMessage("Erreur SQL: " + sqlEx.Message, false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Erreur lors de l'enregistrement: " + ex.Message, false);
            }
        }

        // Annuler et retourner à la liste
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListeConjoints.aspx");
        }

        // Afficher un message
        private void ShowMessage(string message, bool isSuccess)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;

            if (isSuccess)
            {
                pnlMessage.CssClass = "alert alert-success";
            }
            else
            {
                pnlMessage.CssClass = "alert alert-error";
            }
        }
    }
}