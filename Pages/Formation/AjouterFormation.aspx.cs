using System;
using System.Web.UI;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Formation
{
    public partial class AjouterFormation : System.Web.UI.Page
    {
        private FormationManager formationManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            formationManager = new FormationManager();

            if (!IsPostBack)
            {
                // Générer automatiquement le prochain numéro d'objet disponible
                try
                {
                    int prochainObjet = formationManager.ObtenirProchainNumeroObjet();
                    txtObjet.Text = prochainObjet.ToString();
                }
                catch (Exception ex)
                {
                    AfficherMessage("Erreur lors de la génération du numéro d'objet : " + ex.Message, "danger");
                    txtObjet.Text = "1"; // Valeur par défaut
                }
            }
        }

        protected void btnEnregistrer_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    // Récupérer les valeurs du formulaire
                    int objet = Convert.ToInt32(txtObjet.Text);
                    DateTime dateFormation = Convert.ToDateTime(txtDateFormation.Text);
                    string lieu = txtLieu.Text.Trim();
                    string duree = txtDuree.Text.Trim();
                    int? effectif = null;

                    if (!string.IsNullOrEmpty(txtEffectif.Text))
                    {
                        effectif = Convert.ToInt32(txtEffectif.Text);
                    }

                    string contenu = txtContenu.Text.Trim();

                    // Validation supplémentaire
                    if (dateFormation < DateTime.Today)
                    {
                        AfficherMessage("La date de formation ne peut pas être dans le passé.", "danger");
                        return;
                    }

                    // Ajouter la formation
                    bool succes = formationManager.AjouterFormation(objet, dateFormation, lieu, duree, effectif, contenu);

                    if (succes)
                    {
                        AfficherMessage("Formation créée avec succès !", "success");

                        // Réinitialiser le formulaire après 2 secondes et rediriger
                        string script = @"
                            setTimeout(function() {
                                window.location.href = 'ListeFormations.aspx';
                            }, 2000);";

                        ClientScript.RegisterStartupScript(this.GetType(), "redirect", script, true);
                    }
                    else
                    {
                        AfficherMessage("Erreur lors de l'enregistrement de la formation.", "danger");
                    }
                }
                catch (Exception ex)
                {
                    AfficherMessage("Erreur : " + ex.Message, "danger");
                }
            }
        }

        protected void btnAnnuler_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListeFormations.aspx");
        }

        private void AfficherMessage(string message, string type)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;

            switch (type)
            {
                case "success":
                    pnlMessage.CssClass = "alert alert-success";
                    break;
                case "danger":
                    pnlMessage.CssClass = "alert alert-danger";
                    break;
                default:
                    pnlMessage.CssClass = "alert";
                    break;
            }
        }
    }
}