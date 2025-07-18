using GestionRHv2.Classes;
using System;
using System.Data;
using System.IO;
using System.Web.UI;

namespace GestionRHv2.Pages.Formation
{
    public partial class DetailFormation : System.Web.UI.Page
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
                        ChargerDetailsFormation();
                        ChargerParticipants();
                        CalculerStatistiques();
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

        private void ChargerDetailsFormation()
        {
            try
            {
                DataRow formation = formationManager.ObtenirFormation(objetFormation, dateFormation);

                if (formation != null)
                {
                    // Informations de base
                    lblObjet.Text = formation["Objet"].ToString();
                    lblDate.Text = Convert.ToDateTime(formation["Date_Form"]).ToString("dddd dd MMMM yyyy",
                        new System.Globalization.CultureInfo("fr-FR"));
                    lblLieu.Text = formation["Lieu"]?.ToString() ?? "Non spécifié";
                    lblDuree.Text = formation["Durée"]?.ToString() ?? "Non spécifiée";
                    lblContenu.Text = formation["Contenu"]?.ToString() ?? "Aucune description disponible";

                    // Effectif
                    if (formation["Effect"] != DBNull.Value && !string.IsNullOrEmpty(formation["Effect"].ToString()))
                    {
                        lblEffectif.Text = formation["Effect"].ToString();
                    }
                    else
                    {
                        lblEffectif.Text = "Illimité";
                    }

                    // Statut de la formation
                    DateTime today = DateTime.Today;
                    if (dateFormation.Date < today)
                    {
                        lblStatut.Text = "<span class='status-badge status-terminee'>Terminée</span>";
                        btnGererInscrits.Visible = false; // Pas d'inscription si terminée
                    }
                    else if (dateFormation.Date == today)
                    {
                        lblStatut.Text = "<span class='status-badge status-en-cours'>En cours</span>";
                    }
                    else
                    {
                        lblStatut.Text = "<span class='status-badge status-a-venir'>À venir</span>";
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

        private void ChargerParticipants()
        {
            try
            {
                DataTable participants = formationManager.ObtenirAgentsInscrits(objetFormation, dateFormation);

                rptParticipants.DataSource = participants;
                rptParticipants.DataBind();

                lblCountParticipants.Text = participants.Rows.Count.ToString();
                pnlAucunParticipant.Visible = participants.Rows.Count == 0;

                // Désactiver le bouton export si pas de participants
                btnExporter.Enabled = participants.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                AfficherMessage("Erreur lors du chargement des participants : " + ex.Message, "danger");
            }
        }

        private void CalculerStatistiques()
        {
            try
            {
                DataRow formation = formationManager.ObtenirFormation(objetFormation, dateFormation);
                DataTable participants = formationManager.ObtenirAgentsInscrits(objetFormation, dateFormation);

                int nbInscrits = participants.Rows.Count;
                lblNbInscrits.Text = nbInscrits.ToString();

                // Places restantes et taux de remplissage
                if (formation != null && formation["Effect"] != DBNull.Value && !string.IsNullOrEmpty(formation["Effect"].ToString()))
                {
                    int effectifMax = Convert.ToInt32(formation["Effect"]);
                    int placesRestantes = Math.Max(0, effectifMax - nbInscrits);

                    lblPlacesRestantes.Text = placesRestantes.ToString();

                    // Taux de remplissage
                    if (effectifMax > 0)
                    {
                        double taux = (double)nbInscrits / effectifMax * 100;
                        lblTauxRemplissage.Text = taux.ToString("F0") + "%";

                        // Colorer en rouge si complet ou presque
                        if (taux >= 90)
                        {
                            lblTauxRemplissage.Style.Add("color", "#e74c3c");
                        }
                    }
                    else
                    {
                        lblTauxRemplissage.Text = "N/A";
                    }

                    // Marquer en rouge si plus de places
                    if (placesRestantes == 0)
                    {
                        lblPlacesRestantes.Style.Add("color", "#e74c3c");
                        lblPlacesRestantes.Style.Add("font-weight", "bold");
                    }
                }
                else
                {
                    lblPlacesRestantes.Text = "∞";
                    lblTauxRemplissage.Text = "N/A";
                }

                // Jours restants
                TimeSpan diff = dateFormation.Date - DateTime.Today;
                if (diff.Days > 0)
                {
                    lblJoursRestants.Text = diff.Days.ToString();
                }
                else if (diff.Days == 0)
                {
                    lblJoursRestants.Text = "Aujourd'hui";
                    lblJoursRestants.Style.Add("color", "#f39c12");
                }
                else
                {
                    lblJoursRestants.Text = "Terminée";
                    lblJoursRestants.Style.Add("color", "#e74c3c");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Erreur calcul statistiques : " + ex.Message);
            }
        }

        protected string GetInitials(string nom, string prenom)
        {
            string initials = "";

            if (!string.IsNullOrEmpty(nom))
                initials += nom.Substring(0, 1).ToUpper();

            if (!string.IsNullOrEmpty(prenom))
                initials += prenom.Substring(0, 1).ToUpper();

            return string.IsNullOrEmpty(initials) ? "?" : initials;
        }

        protected void btnModifier_Click(object sender, EventArgs e)
        {
            Response.Redirect($"ModifierFormation.aspx?objet={objetFormation}&date={dateFormation:yyyy-MM-dd}");
        }

        protected void btnGererInscrits_Click(object sender, EventArgs e)
        {
            Response.Redirect($"InscrireAgents.aspx?objet={objetFormation}&date={dateFormation:yyyy-MM-dd}");
        }

        protected void btnSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                // Vérifier s'il y a des inscrits
                DataTable participants = formationManager.ObtenirAgentsInscrits(objetFormation, dateFormation);
                if (participants.Rows.Count > 0)
                {
                    AfficherMessage($"Impossible de supprimer cette formation car {participants.Rows.Count} agent(s) y sont inscrits.", "danger");
                    return;
                }

                // Supprimer la formation
                if (formationManager.SupprimerFormation(objetFormation, dateFormation))
                {
                    AfficherMessage("Formation supprimée avec succès.", "success");

                    // Redirection après 2 secondes
                    string script = @"
                        setTimeout(function() {
                            window.location.href = 'ListeFormations.aspx';
                        }, 2000);";

                    ClientScript.RegisterStartupScript(this.GetType(), "redirect", script, true);
                }
                else
                {
                    AfficherMessage("Erreur lors de la suppression de la formation.", "danger");
                }
            }
            catch (Exception ex)
            {
                AfficherMessage("Erreur : " + ex.Message, "danger");
            }
        }

        protected void btnExporter_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable participants = formationManager.ObtenirAgentsInscrits(objetFormation, dateFormation);
                ExporterVersExcel(participants);
            }
            catch (Exception ex)
            {
                AfficherMessage("Erreur lors de l'export : " + ex.Message, "danger");
            }
        }

        private void ExporterVersExcel(DataTable participants)
        {
            string fileName = $"Formation_{objetFormation}_{dateFormation:yyyyMMdd}_Participants.xls";

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", $"attachment;filename={fileName}");
            Response.Charset = "";
            Response.ContentEncoding = System.Text.Encoding.UTF8;

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    // En-tête du document
                    hw.WriteLine("<html>");
                    hw.WriteLine("<head>");
                    hw.WriteLine("<meta charset='utf-8'/>");
                    hw.WriteLine("<style>");
                    hw.WriteLine("table { border-collapse: collapse; width: 100%; }");
                    hw.WriteLine("th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
                    hw.WriteLine("th { background-color: #f2f2f2; font-weight: bold; }");
                    hw.WriteLine("</style>");
                    hw.WriteLine("</head>");
                    hw.WriteLine("<body>");

                    // Titre
                    hw.WriteLine($"<h2>Liste des participants - Formation N°{objetFormation}</h2>");
                    hw.WriteLine($"<p><strong>Date:</strong> {dateFormation:dd/MM/yyyy}</p>");
                    hw.WriteLine($"<p><strong>Lieu:</strong> {lblLieu.Text}</p>");
                    hw.WriteLine($"<p><strong>Nombre de participants:</strong> {participants.Rows.Count}</p>");
                    hw.WriteLine("<hr/>");

                    // Table des participants
                    hw.WriteLine("<table>");
                    hw.WriteLine("<thead>");
                    hw.WriteLine("<tr>");
                    hw.WriteLine("<th>N°</th>");
                    hw.WriteLine("<th>Matricule</th>");
                    hw.WriteLine("<th>Nom</th>");
                    hw.WriteLine("<th>Prénom</th>");
                    hw.WriteLine("<th>CIN</th>");
                    hw.WriteLine("<th>Téléphone</th>");
                    hw.WriteLine("<th>Date inscription</th>");
                    hw.WriteLine("</tr>");
                    hw.WriteLine("</thead>");
                    hw.WriteLine("<tbody>");

                    int numero = 1;
                    foreach (DataRow row in participants.Rows)
                    {
                        hw.WriteLine("<tr>");
                        hw.WriteLine($"<td>{numero++}</td>");
                        hw.WriteLine($"<td>{row["matricule"]}</td>");
                        hw.WriteLine($"<td>{row["nom"]}</td>");
                        hw.WriteLine($"<td>{row["prenom"]}</td>");
                        hw.WriteLine($"<td>{row["cin"]}</td>");
                        hw.WriteLine($"<td>{row["tel"]}</td>");
                        hw.WriteLine($"<td>{Convert.ToDateTime(row["date_form"]):dd/MM/yyyy}</td>");
                        hw.WriteLine("</tr>");
                    }

                    hw.WriteLine("</tbody>");
                    hw.WriteLine("</table>");

                    // Pied de page
                    hw.WriteLine("<hr/>");
                    hw.WriteLine($"<p><small>Document généré le {DateTime.Now:dd/MM/yyyy HH:mm}</small></p>");

                    hw.WriteLine("</body>");
                    hw.WriteLine("</html>");
                }

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
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
                default:
                    pnlMessage.CssClass = "alert";
                    break;
            }
        }
    }
}