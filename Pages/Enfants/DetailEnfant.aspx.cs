using System;
using System.Data;
using System.Web.UI;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Enfants
{
    public partial class DetailEnfant : System.Web.UI.Page
    {
        private string matricule;
        private int num;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Récupérer les paramètres
            matricule = Request.QueryString["matricule"];
            if (!int.TryParse(Request.QueryString["num"], out num))
            {
                // Rediriger si les paramètres sont invalides
                Response.Redirect("ListeEnfants.aspx");
                return;
            }

            if (!IsPostBack)
            {
                ChargerDetailEnfant();

                // Configurer le lien de modification
                lnkModifier.NavigateUrl = $"ModifierEnfant.aspx?matricule={matricule}&num={num}";
            }
        }

        private void ChargerDetailEnfant()
        {
            try
            {
                // Récupérer l'enfant
                Enfant enfant = EnfantManager.GetEnfant(matricule, num);

                if (enfant == null)
                {
                    Session["Message"] = "Enfant introuvable.";
                    Session["TypeMessage"] = "danger";
                    Response.Redirect("ListeEnfants.aspx");
                    return;
                }

                // Charger les informations de l'agent parent
                string queryAgent = "SELECT nom, prenom FROM AGENT WHERE matricule = @matricule";
                DataTable dtAgent = Database.GetDataWithParams(queryAgent,
                    new System.Data.SqlClient.SqlParameter("@matricule", matricule));

                if (dtAgent.Rows.Count > 0)
                {
                    DataRow rowAgent = dtAgent.Rows[0];
                    lblMatriculeAgent.Text = matricule;
                    lblNomAgent.Text = $"{rowAgent["nom"]} {rowAgent["prenom"]}";
                }

                // Afficher les informations personnelles
                lblNum.Text = enfant.Num.ToString();
                lblNom.Text = enfant.Nom;
                lblPrenom.Text = enfant.Prenom;

                // Sexe avec icône
                if (enfant.Sexe == "M")
                {
                    lblSexe.Text = "Masculin";
                    lblSexeIcon.Text = "♂";
                    lblSexeIcon.CssClass = "gender-icon gender-male";
                }
                else if (enfant.Sexe == "F")
                {
                    lblSexe.Text = "Féminin";
                    lblSexeIcon.Text = "♀";
                    lblSexeIcon.CssClass = "gender-icon gender-female";
                }

                // Date de naissance et âge
                if (enfant.DateNaissance.HasValue)
                {
                    lblDateNaissance.Text = enfant.DateNaissance.Value.ToString("dd/MM/yyyy");
                    lblAge.Text = enfant.Age.ToString();
                }
                else
                {
                    lblDateNaissance.Text = "-";
                    lblAge.Text = "-";
                }

                // Lieu de naissance
                lblLieuNaissance.Text = string.IsNullOrWhiteSpace(enfant.LieuNaissance) ? "-" : enfant.LieuNaissance;

                // Informations de scolarité
                if (enfant.Scolarise == "O")
                {
                    lblScolarise.Text = "<span class='badge badge-success'>Oui</span>";
                    pnlDetailsScolarite.Visible = true;

                    lblCodSc.Text = string.IsNullOrWhiteSpace(enfant.CodSc) ? "-" : enfant.CodSc;

                    if (enfant.DatePSc.HasValue)
                    {
                        lblDatePSc.Text = enfant.DatePSc.Value.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        lblDatePSc.Text = "-";
                    }
                }
                else
                {
                    lblScolarise.Text = "<span class='badge badge-warning'>Non</span>";
                    pnlDetailsScolarite.Visible = false;
                }

                // Autres informations
                if (!string.IsNullOrWhiteSpace(enfant.Ch))
                {
                    pnlAutresInfos.Visible = true;
                    lblCh.Text = enfant.Ch;
                }
                else
                {
                    pnlAutresInfos.Visible = false;
                }

                // Définir le titre de la page
                Page.Title = $"Détails - {enfant.NomComplet}";
            }
            catch (Exception ex)
            {
                Session["Message"] = $"Erreur lors du chargement des détails : {ex.Message}";
                Session["TypeMessage"] = "danger";
                Response.Redirect("ListeEnfants.aspx");
            }
        }

        protected void btnSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                bool success = EnfantManager.SupprimerEnfant(matricule, num);

                if (success)
                {
                    Session["Message"] = "Enfant supprimé avec succès.";
                    Session["TypeMessage"] = "success";
                }
                else
                {
                    Session["Message"] = "Erreur lors de la suppression de l'enfant.";
                    Session["TypeMessage"] = "danger";
                }
            }
            catch (Exception ex)
            {
                Session["Message"] = $"Erreur : {ex.Message}";
                Session["TypeMessage"] = "danger";
            }

            // Rediriger vers la liste
            Response.Redirect("ListeEnfants.aspx");
        }
    }
}