using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Missions
{
    public partial class AjouterMission : System.Web.UI.Page
    {
        private string _matricule;
        private DateTime? _date;
        private bool _isEditMode = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Vérifier si on est en mode édition
            if (!string.IsNullOrEmpty(Request.QueryString["mat"]) && !string.IsNullOrEmpty(Request.QueryString["date"]))
            {
                _matricule = Request.QueryString["mat"];
                if (DateTime.TryParse(Request.QueryString["date"], out DateTime dateTemp))
                {
                    _date = dateTemp;
                    _isEditMode = true;
                }
            }

            if (!IsPostBack)
            {
                if (_isEditMode)
                {
                    lblTitle.Text = "Modifier la Mission";
                    LoadMissionData();
                }
                else
                {
                    lblTitle.Text = "Nouvelle Mission";
                    // Définir la date du jour par défaut
                    txtDateDeplacement.Text = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
        }

        private void LoadMissionData()
        {
            try
            {
                DataTable mission = MissionManager.GetMission(_matricule, _date.Value);
                if (mission.Rows.Count > 0)
                {
                    DataRow row = mission.Rows[0];

                    // Charger les données dans les contrôles
                    txtMatricule.Text = GetValueOrEmpty(row["Matricule"]);
                    txtNom.Text = GetValueOrEmpty(row["Nom"]);
                    txtGrade.Text = GetValueOrEmpty(row["Grade"]);
                    txtCompte.Text = GetValueOrEmpty(row["Compte"]);

                    if (row["Date_deplacement"] != DBNull.Value)
                        txtDateDeplacement.Text = Convert.ToDateTime(row["Date_deplacement"]).ToString("yyyy-MM-dd");

                    // Sélectionner le moyen de transport
                    string moyenTrans = GetValueOrEmpty(row["Moyen_trans"]).ToLower();
                    if (moyenTrans.Contains("voiture"))
                        rbVoiture.Checked = true;
                    else if (moyenTrans.Contains("train"))
                        rbTrain.Checked = true;
                    else if (moyenTrans.Contains("avion"))
                        rbAvion.Checked = true;

                    // Charger les données financières
                    if (row["Montant"] != DBNull.Value)
                        txtMontant.Text = Convert.ToDecimal(row["Montant"]).ToString("0.00");

                    if (row["Taux"] != DBNull.Value)
                        txtTaux.Text = Convert.ToDecimal(row["Taux"]).ToString("0.00");

                    if (row["Nb_Jour"] != DBNull.Value)
                        txtNbJour.Text = row["Nb_Jour"].ToString();

                    if (row["Taux_Jour"] != DBNull.Value)
                        txtTauxJour.Text = Convert.ToDecimal(row["Taux_Jour"]).ToString("0.00");

                    txtPuissFisc.Text = GetValueOrEmpty(row["Puiss_fisc"]);

                    if (row["Kilometrage"] != DBNull.Value)
                        txtKilometrage.Text = Convert.ToDecimal(row["Kilometrage"]).ToString("0.00");

                    if (row["Nb_vacation"] != DBNull.Value)
                        txtNbVacation.Text = row["Nb_vacation"].ToString();

                    txtTotalLettre.Text = GetValueOrEmpty(row["TotalLettre"]);

                    // Stocker les valeurs originales pour la mise à jour
                    ViewState["OriginalMatricule"] = _matricule;
                    ViewState["OriginalDate"] = _date;
                }
                else
                {
                    ShowError("Mission introuvable.");
                    Response.Redirect("ListeMissions.aspx");
                }
            }
            catch (Exception ex)
            {
                ShowError("Erreur lors du chargement de la mission : " + ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                // Récupérer les valeurs du formulaire
                string matricule = txtMatricule.Text.Trim();
                DateTime dateDeplacement = DateTime.Parse(txtDateDeplacement.Text);

                // Déterminer le moyen de transport sélectionné
                string moyenTrans = "Voiture"; // Par défaut
                if (rbTrain.Checked) moyenTrans = "Train";
                else if (rbAvion.Checked) moyenTrans = "Avion";

                // Récupérer les valeurs numériques (avec gestion des valeurs null)
                decimal? montant = string.IsNullOrEmpty(txtMontant.Text) ? (decimal?)null : decimal.Parse(txtMontant.Text);
                string puissFisc = txtPuissFisc.Text.Trim();
                decimal? kilometrage = string.IsNullOrEmpty(txtKilometrage.Text) ? (decimal?)null : decimal.Parse(txtKilometrage.Text);
                int? nbVacation = string.IsNullOrEmpty(txtNbVacation.Text) ? (int?)null : int.Parse(txtNbVacation.Text);
                string totalLettre = txtTotalLettre.Text.Trim();
                string nom = txtNom.Text.Trim();
                string compte = txtCompte.Text.Trim();
                string grade = txtGrade.Text.Trim();
                decimal? taux = string.IsNullOrEmpty(txtTaux.Text) ? (decimal?)null : decimal.Parse(txtTaux.Text);
                int? nbJour = string.IsNullOrEmpty(txtNbJour.Text) ? (int?)null : int.Parse(txtNbJour.Text);
                decimal? tauxJour = string.IsNullOrEmpty(txtTauxJour.Text) ? (decimal?)null : decimal.Parse(txtTauxJour.Text);

                int result;

                if (_isEditMode && ViewState["OriginalMatricule"] != null && ViewState["OriginalDate"] != null)
                {
                    // Mode édition
                    string oldMatricule = ViewState["OriginalMatricule"].ToString();
                    DateTime oldDate = (DateTime)ViewState["OriginalDate"];

                    result = MissionManager.UpdateMission(
                        matricule, dateDeplacement, moyenTrans, montant, puissFisc,
                        kilometrage, nbVacation, totalLettre, nom, compte, grade,
                        taux, nbJour, tauxJour, oldMatricule, oldDate
                    );

                    if (result > 0)
                    {
                        ShowSuccess("Mission mise à jour avec succès!");
                        Response.Redirect("ListeMissions.aspx");
                    }
                    else
                    {
                        ShowError("Erreur lors de la mise à jour de la mission.");
                    }
                }
                else
                {
                    // Mode ajout
                    result = MissionManager.AddMission(
                        matricule, dateDeplacement, moyenTrans, montant, puissFisc,
                        kilometrage, nbVacation, totalLettre, nom, compte, grade,
                        taux, nbJour, tauxJour
                    );

                    if (result > 0)
                    {
                        ShowSuccess("Mission ajoutée avec succès!");

                        // Réinitialiser le formulaire ou rediriger
                        if (Request.QueryString["redirect"] != "false")
                        {
                            Response.Redirect("ListeMissions.aspx");
                        }
                        else
                        {
                            ClearForm();
                        }
                    }
                    else
                    {
                        ShowError("Erreur lors de l'ajout de la mission.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Erreur : " + ex.Message);
            }
        }

        private void ClearForm()
        {
            txtMatricule.Text = "";
            txtNom.Text = "";
            txtGrade.Text = "";
            txtCompte.Text = "";
            txtDateDeplacement.Text = DateTime.Now.ToString("yyyy-MM-dd");
            rbVoiture.Checked = true;
            txtMontant.Text = "";
            txtTaux.Text = "";
            txtNbJour.Text = "";
            txtTauxJour.Text = "";
            txtPuissFisc.Text = "";
            txtKilometrage.Text = "";
            txtNbVacation.Text = "";
            txtTotalLettre.Text = "";
        }

        private string GetValueOrEmpty(object value)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;
            return value.ToString();
        }

        private void ShowError(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error",
                $"alert('{message.Replace("'", "\\'")}')", true);
        }

        private void ShowSuccess(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "success",
                $"alert('{message.Replace("'", "\\'")}')", true);
        }
    }
}