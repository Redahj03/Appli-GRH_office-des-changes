using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Formation
{
    public partial class ModifierFormation : System.Web.UI.Page
    {
        private FormationManager formationManager;
        private int objetFormation;
        private DateTime dateFormationOriginale;
        private int nombreInscrits = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            formationManager = new FormationManager();

            // Récupérer les paramètres
            if (Request.QueryString["objet"] != null && Request.QueryString["date"] != null)
            {
                try
                {
                    objetFormation = Convert.ToInt32(Request.QueryString["objet"]);
                    dateFormationOriginale = Convert.ToDateTime(Request.QueryString["date"]);

                    if (!IsPostBack)
                    {
                        ChargerFormation();
                    }
                    else
                    {
                        // Récupérer le nombre d'inscrits depuis ViewState
                        if (ViewState["NombreInscrits"] != null)
                        {
                            nombreInscrits = (int)ViewState["NombreInscrits"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    AfficherMessage("Paramètres invalides : " + ex.Message, "danger");
                    Response.Redirect("ListeFormations.aspx");
                }
            }
            else
            {
                AfficherMessage("Formation non spécifiée.", "danger");
                Response.Redirect("ListeFormations.aspx");
            }
        }

        private void ChargerFormation()
        {
            try
            {
                DataRow formation = formationManager.ObtenirFormation(objetFormation, dateFormationOriginale);

                if (formation != null)
                {
                    // Remplir les champs du formulaire
                    txtObjet.Text = formation["Objet"].ToString();
                    txtDateFormation.Text = Convert.ToDateTime(formation["Date_Form"]).ToString("yyyy-MM-dd");
                    txtLieu.Text = formation["Lieu"]?.ToString() ?? "";
                    txtDuree.Text = formation["Durée"]?.ToString() ?? "";

                    if (formation["Effect"] != DBNull.Value && !string.IsNullOrEmpty(formation["Effect"].ToString()))
                    {
                        txtEffectif.Text = formation["Effect"].ToString();
                    }

                    txtContenu.Text = formation["Contenu"]?.ToString() ?? "";

                    // Charger les statistiques
                    ChargerStatistiques();
                }
                else
                {
                    AfficherMessage("Formation introuvable.", "danger");
                    Response.Redirect("ListeFormations.aspx");
                }
            }
            catch (Exception ex)
            {
                AfficherMessage("Erreur lors du chargement de la formation : " + ex.Message, "danger");
            }
        }

        private void ChargerStatistiques()
        {
            try
            {
                // Numéro d'objet
                lblStatObjet.Text = objetFormation.ToString();

                // Nombre d'inscrits
                DataTable inscrits = formationManager.ObtenirAgentsInscrits(objetFormation, dateFormationOriginale);
                nombreInscrits = inscrits.Rows.Count;
                lblStatInscrits.Text = nombreInscrits.ToString();

                // Stocker dans ViewState pour la validation
                ViewState["NombreInscrits"] = nombreInscrits;

                // Statut
                DateTime today = DateTime.Today;
                if (dateFormationOriginale.Date < today)
                {
                    lblStatStatut.Text = "Terminée";
                    lblStatStatut.Style.Add("color", "#e74c3c");
                }
                else if (dateFormationOriginale.Date == today)
                {
                    lblStatStatut.Text = "En cours";
                    lblStatStatut.Style.Add("color", "#f39c12");
                }
                else
                {
                    lblStatStatut.Text = "À venir";
                    lblStatStatut.Style.Add("color", "#27ae60");
                }

                // Afficher un avertissement si des agents sont inscrits
                if (nombreInscrits > 0)
                {
                    pnlWarning.Visible = true;
                    lblWarning.Text = $"Attention : {nombreInscrits} agent(s) sont déjà inscrits à cette formation. " +
                                     "Les modifications d'effectif maximum ne peuvent pas être inférieures à ce nombre.";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Erreur statistiques : " + ex.Message);
            }
        }

        protected void CvEffectif_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(txtEffectif.Text))
            {
                int effectif = Convert.ToInt32(txtEffectif.Text);

                // Récupérer le nombre d'inscrits actuel
                if (ViewState["NombreInscrits"] != null)
                {
                    int inscrits = (int)ViewState["NombreInscrits"];

                    if (effectif < inscrits)
                    {
                        args.IsValid = false;
                        CvEffectif.ErrorMessage = $"L'effectif ne peut pas être inférieur au nombre d'inscrits actuels ({inscrits})";
                    }
                    else
                    {
                        args.IsValid = true;
                    }
                }
                else
                {
                    args.IsValid = true;
                }
            }
            else
            {
                args.IsValid = true; // Effectif illimité est toujours valide
            }
        }

        protected void btnEnregistrer_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    // Récupérer les valeurs du formulaire
                    DateTime nouvelleDate = Convert.ToDateTime(txtDateFormation.Text);
                    string lieu = txtLieu.Text.Trim();
                    string duree = txtDuree.Text.Trim();
                    int? effectif = null;

                    if (!string.IsNullOrEmpty(txtEffectif.Text))
                    {
                        effectif = Convert.ToInt32(txtEffectif.Text);
                    }

                    string contenu = txtContenu.Text.Trim();

                    // Si la date change et qu'il y a des inscrits, utiliser la méthode spéciale
                    if (nouvelleDate.Date != dateFormationOriginale.Date && nombreInscrits > 0)
                    {
                        // Utiliser la méthode qui gère le changement de date
                        bool succes = formationManager.ModifierFormationAvecNouvelleDate(
                            objetFormation, dateFormationOriginale, nouvelleDate,
                            lieu, duree, effectif, contenu);

                        if (succes)
                        {
                            AfficherMessage($"Formation modifiée avec succès ! La nouvelle date a été appliquée et les {nombreInscrits} inscriptions ont été mises à jour.", "success");

                            // Redirection après 2 secondes vers la nouvelle date
                            string script = @"
                                setTimeout(function() {
                                    window.location.href = 'DetailFormation.aspx?objet=" + objetFormation + "&date=" + nouvelleDate.ToString("yyyy-MM-dd") + @"';
                                }, 2000);";

                            ClientScript.RegisterStartupScript(this.GetType(), "redirect", script, true);
                        }
                        else
                        {
                            AfficherMessage("Erreur lors de la modification de la formation.", "danger");
                        }
                    }
                    else
                    {
                        // Modification normale (sans changement de date ou sans inscrits)
                        bool succes = formationManager.ModifierFormation(objetFormation, nouvelleDate, lieu, duree, effectif, contenu);

                        if (succes)
                        {
                            AfficherMessage("Formation modifiée avec succès !", "success");

                            // Redirection après 2 secondes
                            string script = @"
                                setTimeout(function() {
                                    window.location.href = 'DetailFormation.aspx?objet=" + objetFormation + "&date=" + nouvelleDate.ToString("yyyy-MM-dd") + @"';
                                }, 2000);";

                            ClientScript.RegisterStartupScript(this.GetType(), "redirect", script, true);
                        }
                        else
                        {
                            AfficherMessage("Erreur lors de la modification de la formation.", "danger");
                        }
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
            Response.Redirect($"DetailFormation.aspx?objet={objetFormation}&date={dateFormationOriginale:yyyy-MM-dd}");
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
                case "warning":
                    pnlMessage.CssClass = "alert alert-warning";
                    break;
                default:
                    pnlMessage.CssClass = "alert";
                    break;
            }
        }
    }
}