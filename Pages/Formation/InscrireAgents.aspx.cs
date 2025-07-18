using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Formation
{
    public partial class InscrireAgents : System.Web.UI.Page
    {
        private FormationManager formationManager;
        private int objetFormation;
        private DateTime dateFormation;

        protected void Page_Load(object sender, EventArgs e)
        {
            formationManager = new FormationManager();

            // Récupérer les paramètres
            if (Request.QueryString["objet"] != null && Request.QueryString["date"] != null)
            {
                try
                {
                    objetFormation = Convert.ToInt32(Request.QueryString["objet"]);
                    dateFormation = Convert.ToDateTime(Request.QueryString["date"]);

                    if (!IsPostBack)
                    {
                        ChargerInformationsFormation();
                        ChargerAgents();
                    }
                }
                catch (Exception ex)
                {
                    AfficherMessage("Paramètres invalides : " + ex.Message, "danger");
                }
            }
            else
            {
                AfficherMessage("Formation non spécifiée.", "danger");
                Response.Redirect("ListeFormations.aspx");
            }
        }

        private void ChargerInformationsFormation()
        {
            try
            {
                DataRow formation = formationManager.ObtenirFormation(objetFormation, dateFormation);

                if (formation != null)
                {
                    lblObjet.Text = formation["Objet"].ToString();
                    lblDate.Text = Convert.ToDateTime(formation["Date_Form"]).ToString("dd/MM/yyyy");
                    lblLieu.Text = formation["Lieu"]?.ToString() ?? "Non spécifié";

                    // Gestion de l'effectif
                    int effectifMax = 0;
                    bool effectifLimite = false;

                    if (formation["Effect"] != DBNull.Value && !string.IsNullOrEmpty(formation["Effect"].ToString()))
                    {
                        effectifMax = Convert.ToInt32(formation["Effect"]);
                        effectifLimite = true;
                    }

                    // Compter les inscrits actuels
                    DataTable inscrits = formationManager.ObtenirAgentsInscrits(objetFormation, dateFormation);
                    int nbInscrits = inscrits.Rows.Count;

                    if (effectifLimite)
                    {
                        lblEffectif.Text = $"{nbInscrits}/{effectifMax}";
                        int placesRestantes = Math.Max(0, effectifMax - nbInscrits);
                        lblPlacesRestantes.Text = placesRestantes.ToString();

                        // Marquer en rouge si complet
                        if (placesRestantes == 0)
                        {
                            lblPlacesRestantes.Style.Add("color", "#e74c3c");
                            lblPlacesRestantes.Style.Add("font-weight", "bold");
                        }
                    }
                    else
                    {
                        lblEffectif.Text = $"{nbInscrits}/Illimité";
                        lblPlacesRestantes.Text = "Illimité";
                    }
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

        private void ChargerAgents()
        {
            ChargerAgentsDisponibles();
            ChargerAgentsInscrits();
        }

        private void ChargerAgentsDisponibles()
        {
            try
            {
                // Obtenir tous les agents
                DataTable tousLesAgents = formationManager.ObtenirAgentsDisponibles();

                // Obtenir les agents déjà inscrits
                DataTable agentsInscrits = formationManager.ObtenirAgentsInscrits(objetFormation, dateFormation);

                // Créer une liste des matricules inscrits
                List<int> matriculesInscrits = new List<int>();
                foreach (DataRow row in agentsInscrits.Rows)
                {
                    matriculesInscrits.Add(Convert.ToInt32(row["matricule"]));
                }

                // Filtrer pour n'avoir que les agents non inscrits
                DataTable agentsDisponibles = tousLesAgents.Clone();

                foreach (DataRow row in tousLesAgents.Rows)
                {
                    int matricule = Convert.ToInt32(row["matricule"]);
                    if (!matriculesInscrits.Contains(matricule))
                    {
                        // Appliquer le filtre de recherche si nécessaire
                        if (string.IsNullOrEmpty(TxtRechercheDisponibles.Text) ||
                            FiltrerAgent(row, TxtRechercheDisponibles.Text))
                        {
                            agentsDisponibles.ImportRow(row);
                        }
                    }
                }

                // Afficher les résultats
                rptAgentsDisponibles.DataSource = agentsDisponibles;
                rptAgentsDisponibles.DataBind();

                lblCountDisponibles.Text = agentsDisponibles.Rows.Count.ToString();
                pnlAucunDisponible.Visible = agentsDisponibles.Rows.Count == 0;
            }
            catch (Exception ex)
            {
                AfficherMessage("Erreur lors du chargement des agents disponibles : " + ex.Message, "danger");
            }
        }

        private void ChargerAgentsInscrits()
        {
            try
            {
                DataTable agentsInscrits = formationManager.ObtenirAgentsInscrits(objetFormation, dateFormation);

                // Appliquer le filtre de recherche si nécessaire
                if (!string.IsNullOrEmpty(TxtRechercheInscrits.Text))
                {
                    DataTable agentsFiltres = agentsInscrits.Clone();
                    foreach (DataRow row in agentsInscrits.Rows)
                    {
                        if (FiltrerAgent(row, TxtRechercheInscrits.Text))
                        {
                            agentsFiltres.ImportRow(row);
                        }
                    }
                    agentsInscrits = agentsFiltres;
                }

                rptAgentsInscrits.DataSource = agentsInscrits;
                rptAgentsInscrits.DataBind();

                lblCountInscrits.Text = agentsInscrits.Rows.Count.ToString();
                pnlAucunInscrit.Visible = agentsInscrits.Rows.Count == 0;

                // Mettre à jour le badge si formation complète
                DataRow formation = formationManager.ObtenirFormation(objetFormation, dateFormation);
                if (formation != null && formation["Effect"] != DBNull.Value)
                {
                    int effectifMax = Convert.ToInt32(formation["Effect"]);
                    if (agentsInscrits.Rows.Count >= effectifMax)
                    {
                        badgeInscrits.Attributes["class"] = "count-badge full";
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessage("Erreur lors du chargement des agents inscrits : " + ex.Message, "danger");
            }
        }

        private bool FiltrerAgent(DataRow row, string recherche)
        {
            string termeRecherche = recherche.ToLower();
            string matricule = row["matricule"].ToString().ToLower();
            string nom = row["nom"]?.ToString().ToLower() ?? "";
            string prenom = row["prenom"]?.ToString().ToLower() ?? "";

            return matricule.Contains(termeRecherche) ||
                   nom.Contains(termeRecherche) ||
                   prenom.Contains(termeRecherche);
        }

        protected void btnInscrire_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> matriculesAInscrire = ObtenirMatriculesSelectionnes(rptAgentsDisponibles);

                if (matriculesAInscrire.Count == 0)
                {
                    AfficherMessage("Veuillez sélectionner au moins un agent à inscrire.", "warning");
                    return;
                }

                // Vérifier l'effectif maximum
                DataRow formation = formationManager.ObtenirFormation(objetFormation, dateFormation);
                if (formation != null && formation["Effect"] != DBNull.Value)
                {
                    int effectifMax = Convert.ToInt32(formation["Effect"]);
                    DataTable inscritsActuels = formationManager.ObtenirAgentsInscrits(objetFormation, dateFormation);
                    int placesRestantes = effectifMax - inscritsActuels.Rows.Count;

                    if (matriculesAInscrire.Count > placesRestantes)
                    {
                        AfficherMessage($"Impossible d'inscrire {matriculesAInscrire.Count} agents. " +
                                       $"Il ne reste que {placesRestantes} place(s) disponible(s).", "warning");
                        return;
                    }
                }

                // Inscrire les agents
                int nbInscrits = 0;
                foreach (int matricule in matriculesAInscrire)
                {
                    if (formationManager.InscrireAgent(matricule, objetFormation, dateFormation))
                    {
                        nbInscrits++;
                    }
                }

                if (nbInscrits > 0)
                {
                    AfficherMessage($"{nbInscrits} agent(s) inscrit(s) avec succès.", "success");
                    ChargerInformationsFormation();
                    ChargerAgents();
                }
            }
            catch (Exception ex)
            {
                AfficherMessage("Erreur lors de l'inscription : " + ex.Message, "danger");
            }
        }

        protected void btnDesinscrire_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> matriculesADesinscrire = ObtenirMatriculesSelectionnes(rptAgentsInscrits);

                if (matriculesADesinscrire.Count == 0)
                {
                    AfficherMessage("Veuillez sélectionner au moins un agent à désinscrire.", "warning");
                    return;
                }

                // Désinscrire les agents
                int nbDesinscrits = 0;
                foreach (int matricule in matriculesADesinscrire)
                {
                    if (formationManager.DesinscrireAgent(matricule, objetFormation, dateFormation))
                    {
                        nbDesinscrits++;
                    }
                }

                if (nbDesinscrits > 0)
                {
                    AfficherMessage($"{nbDesinscrits} agent(s) désinscrit(s) avec succès.", "success");
                    ChargerInformationsFormation();
                    ChargerAgents();
                }
            }
            catch (Exception ex)
            {
                AfficherMessage("Erreur lors de la désinscription : " + ex.Message, "danger");
            }
        }

        private List<int> ObtenirMatriculesSelectionnes(Repeater repeater)
        {
            List<int> matricules = new List<int>();

            foreach (RepeaterItem item in repeater.Items)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkAgent");
                HiddenField hfMatricule = (HiddenField)item.FindControl("hfMatricule");

                if (chk != null && chk.Checked && hfMatricule != null)
                {
                    int matricule = Convert.ToInt32(hfMatricule.Value);
                    matricules.Add(matricule);
                }
            }

            return matricules;
        }

        protected void TxtRechercheDisponibles_TextChanged(object sender, EventArgs e)
        {
            ChargerAgentsDisponibles();
        }

        protected void TxtRechercheInscrits_TextChanged(object sender, EventArgs e)
        {
            ChargerAgentsInscrits();
        }

        protected void ChkSelectAllDisponibles_CheckedChanged(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptAgentsDisponibles.Items)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkAgent");
                if (chk != null)
                {
                    chk.Checked = chkSelectAllDisponibles.Checked;
                }
            }
        }

        protected void ChkSelectAllInscrits_CheckedChanged(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptAgentsInscrits.Items)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkAgent");
                if (chk != null)
                {
                    chk.Checked = ChkSelectAllInscrits.Checked;
                }
            }
        }

        protected void btnRetour_Click(object sender, EventArgs e)
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