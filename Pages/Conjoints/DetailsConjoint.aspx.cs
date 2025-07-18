using System;
using System.Data;
using System.Web.UI;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Conjoints
{
    public partial class DetailsConjoint : System.Web.UI.Page
    {
        private string _matricule;
        private int _num;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Récupérer les paramètres
                if (Request.QueryString["matricule"] != null && Request.QueryString["num"] != null)
                {
                    _matricule = Request.QueryString["matricule"];
                    if (int.TryParse(Request.QueryString["num"], out _num))
                    {
                        LoadConjointDetails();
                    }
                    else
                    {
                        ShowError("Numéro de conjoint invalide.");
                    }
                }
                else
                {
                    ShowError("Paramètres manquants.");
                }
            }
        }

        private void LoadConjointDetails()
        {
            try
            {
                // Récupérer les détails du conjoint
                Conjoint conjoint = ConjointManager.GetConjointByMatriculeAndNum(_matricule, _num);

                if (conjoint != null)
                {
                    // Stocker les valeurs dans ViewState pour les utiliser dans les événements
                    ViewState["Matricule"] = conjoint.Matricule;
                    ViewState["Num"] = conjoint.Num;

                    // Informations de l'employé
                    lblMatricule.Text = conjoint.Matricule;
                    lblEmployeNom.Text = $"{conjoint.NomEmploye} {conjoint.PrenomEmploye}";
                    lblNumConjoint.Text = conjoint.Num.ToString();

                    // Mise à jour du titre de la page
                    lblConjointInfo.Text = $"Conjoint n°{conjoint.Num} de {conjoint.NomEmploye} {conjoint.PrenomEmploye}";

                    // Informations du conjoint
                    if (!string.IsNullOrEmpty(conjoint.Nom))
                    {
                        lblNom.Text = conjoint.Nom;
                        lblNom.CssClass = "";
                    }

                    if (!string.IsNullOrEmpty(conjoint.Prenom))
                    {
                        lblPrenom.Text = conjoint.Prenom;
                        lblPrenom.CssClass = "";
                    }

                    if (!string.IsNullOrEmpty(conjoint.Prof))
                    {
                        lblProfession.Text = conjoint.Prof;
                        lblProfession.CssClass = "";
                    }

                    // Informations du mariage
                    if (conjoint.DateMariage.HasValue)
                    {
                        lblDateMariage.Text = conjoint.DateMariage.Value.ToString("dd/MM/yyyy");
                        lblDateMariage.CssClass = "";

                        // Calculer la durée du mariage
                        TimeSpan duree = DateTime.Now - conjoint.DateMariage.Value;
                        int annees = (int)(duree.TotalDays / 365.25);
                        int mois = (int)((duree.TotalDays % 365.25) / 30.44);

                        if (annees > 0)
                        {
                            lblDureeMariage.Text = $"{annees} an{(annees > 1 ? "s" : "")}";
                            if (mois > 0)
                            {
                                lblDureeMariage.Text += $" et {mois} mois";
                            }
                        }
                        else if (mois > 0)
                        {
                            lblDureeMariage.Text = $"{mois} mois";
                        }
                        else
                        {
                            lblDureeMariage.Text = "Moins d'un mois";
                        }
                    }
                    else
                    {
                        lblDureeMariage.Text = "-";
                    }

                    // Allocation familiale
                    if (conjoint.AllocationFamiliale.HasValue)
                    {
                        if (conjoint.AllocationFamiliale.Value)
                        {
                            lblAllocation.Text = "<span class='status-badge badge-active'>✓ Oui</span>";
                        }
                        else
                        {
                            lblAllocation.Text = "<span class='status-badge badge-inactive'>✗ Non</span>";
                        }
                    }
                    else
                    {
                        lblAllocation.Text = "<span class='empty-value'>Non renseignée</span>";
                    }
                }
                else
                {
                    ShowError("Conjoint introuvable.");
                }
            }
            catch (Exception ex)
            {
                ShowError("Erreur lors du chargement des détails : " + ex.Message);
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            // Rediriger vers la page de modification
            string matricule = ViewState["Matricule"]?.ToString();
            string num = ViewState["Num"]?.ToString();

            if (!string.IsNullOrEmpty(matricule) && !string.IsNullOrEmpty(num))
            {
                Response.Redirect($"AjouterConjoint.aspx?params={matricule}|{num}");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string matricule = ViewState["Matricule"]?.ToString();
                int num = Convert.ToInt32(ViewState["Num"]);

                int rowsAffected = ConjointManager.DeleteConjoint(matricule, num);

                if (rowsAffected > 0)
                {
                    // Rediriger vers la liste avec un message de succès
                    Response.Redirect("ListeConjoints.aspx?msg=deleted");
                }
                else
                {
                    ShowError("Erreur lors de la suppression du conjoint.");
                }
            }
            catch (Exception ex)
            {
                ShowError("Erreur lors de la suppression : " + ex.Message);
            }
        }

        private void ShowError(string message)
        {
            pnlError.Visible = true;
            lblError.Text = message;
            pnlDetails.Visible = false;
        }
    }
}