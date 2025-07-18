using System;
using System.Data;
using System.Web.UI;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Enfants
{
    public partial class ModifierEnfant : System.Web.UI.Page
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
                ChargerEnfant();

                // Configurer le validateur de date
                cvDateNaissance.ValueToCompare = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        private void ChargerEnfant()
        {
            try
            {
                Enfant enfant = EnfantManager.GetEnfant(matricule, num);

                if (enfant == null)
                {
                    Session["Message"] = "Enfant introuvable.";
                    Session["TypeMessage"] = "danger";
                    Response.Redirect("ListeEnfants.aspx");
                    return;
                }

                // Charger les informations de l'agent
                string queryAgent = "SELECT nom, prenom FROM AGENT WHERE matricule = @matricule";
                DataTable dtAgent = Database.GetDataWithParams(queryAgent,
                    new System.Data.SqlClient.SqlParameter("@matricule", matricule));

                if (dtAgent.Rows.Count > 0)
                {
                    DataRow rowAgent = dtAgent.Rows[0];
                    lblAgentInfo.Text = $"{matricule} - {rowAgent["nom"]} {rowAgent["prenom"]}";
                }

                // Remplir les champs du formulaire
                lblNumEnfant.Text = enfant.Num.ToString();
                txtMatricule.Text = enfant.Matricule;
                txtNum.Text = enfant.Num.ToString();
                txtNom.Text = enfant.Nom;
                txtPrenom.Text = enfant.Prenom;
                rblSexe.SelectedValue = enfant.Sexe;

                if (enfant.DateNaissance.HasValue)
                {
                    txtDateNaissance.Text = enfant.DateNaissance.Value.ToString("yyyy-MM-dd");
                }

                txtLieuNaissance.Text = enfant.LieuNaissance;
                rblScolarise.SelectedValue = enfant.Scolarise;
                txtCh.Text = enfant.Ch;

                // Gérer l'affichage des champs de scolarité
                bool estScolarise = enfant.Scolarise == "O";
                pnlScolarite.Visible = estScolarise;
                pnlDateScolarite.Visible = estScolarise;

                if (estScolarise)
                {
                    txtCodSc.Text = enfant.CodSc;
                    if (enfant.DatePSc.HasValue)
                    {
                        txtDatePSc.Text = enfant.DatePSc.Value.ToString("yyyy-MM-dd");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessage($"Erreur lors du chargement de l'enfant : {ex.Message}", "danger");
            }
        }

        protected void rblScolarise_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool estScolarise = rblScolarise.SelectedValue == "O";
            pnlScolarite.Visible = estScolarise;
            pnlDateScolarite.Visible = estScolarise;

            if (!estScolarise)
            {
                // Effacer les champs de scolarité si non scolarisé
                txtCodSc.Text = "";
                txtDatePSc.Text = "";
            }
        }

        protected void btnModifier_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    // Créer l'objet Enfant avec les nouvelles valeurs
                    Enfant enfant = new Enfant
                    {
                        Matricule = matricule,
                        Num = num,
                        Nom = txtNom.Text.Trim().ToUpper(),
                        Prenom = txtPrenom.Text.Trim(),
                        Sexe = rblSexe.SelectedValue,
                        Scolarise = rblScolarise.SelectedValue,
                        LieuNaissance = txtLieuNaissance.Text.Trim(),
                        Ch = txtCh.Text.Trim()
                    };

                    // Date de naissance
                    if (!string.IsNullOrEmpty(txtDateNaissance.Text))
                    {
                        enfant.DateNaissance = DateTime.Parse(txtDateNaissance.Text);

                        // Vérifier que l'enfant a moins de 21 ans
                        if (enfant.Age > 21)
                        {
                            AfficherMessage("L'enfant doit avoir moins de 21 ans.", "warning");
                            return;
                        }
                    }

                    // Informations de scolarité
                    if (rblScolarise.SelectedValue == "O")
                    {
                        enfant.CodSc = txtCodSc.Text.Trim();
                        if (!string.IsNullOrEmpty(txtDatePSc.Text))
                        {
                            enfant.DatePSc = DateTime.Parse(txtDatePSc.Text);

                            // Vérifier que la date de scolarisation est après la date de naissance
                            if (enfant.DateNaissance.HasValue && enfant.DatePSc.HasValue)
                            {
                                if (enfant.DatePSc.Value <= enfant.DateNaissance.Value)
                                {
                                    AfficherMessage("La date de scolarisation doit être après la date de naissance.", "warning");
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        // Si non scolarisé, effacer les champs de scolarité
                        enfant.CodSc = "";
                        enfant.DatePSc = null;
                    }

                    // Modifier l'enfant
                    bool success = EnfantManager.ModifierEnfant(enfant);

                    if (success)
                    {
                        // Stocker le message en session pour l'afficher sur la page de liste
                        Session["Message"] = $"Enfant {enfant.NomComplet} modifié avec succès.";
                        Session["TypeMessage"] = "success";

                        // Rediriger vers la liste
                        Response.Redirect("ListeEnfants.aspx");
                    }
                    else
                    {
                        AfficherMessage("Erreur lors de la modification de l'enfant.", "danger");
                    }
                }
                catch (Exception ex)
                {
                    AfficherMessage($"Erreur : {ex.Message}", "danger");
                }
            }
        }

        private void AfficherMessage(string message, string type)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;
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