using System;
using System.Data;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Agents
{
    public partial class RechercheAgent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Visible = false;
                pnlResultat.Visible = false;

                // Si matricule passé en paramètre
                string matricule = Request.QueryString["matricule"];
                if (!string.IsNullOrEmpty(matricule))
                {
                    txtRecherche.Text = matricule;
                    ddlType.SelectedValue = "ID";
                    btnRechercher_Click(null, null);
                }
            }
        }

        protected void btnRechercher_Click(object sender, EventArgs e)
        {
            // Réinitialiser l'affichage
            lblMessage.Visible = false;
            pnlResultat.Visible = false;

            // Vérifier la saisie
            string valeurRecherche = txtRecherche.Text.Trim();

            if (string.IsNullOrEmpty(valeurRecherche))
            {
                AfficherMessage("<i class='fas fa-exclamation-circle'></i> Veuillez entrer une valeur de recherche !", true);
                return;
            }

            // Construire la requête
            string query = "";

            if (ddlType.SelectedValue == "ID")
            {
                // Recherche par matricule
                query = $@"SELECT * FROM dbo.Agent WHERE matricule = '{valeurRecherche}'";
            }
            else
            {
                // Recherche par nom/prénom
                query = $@"
                    SELECT TOP 1 * FROM dbo.Agent 
                    WHERE UPPER(nom) LIKE UPPER('%{valeurRecherche}%')
                       OR UPPER(prenom) LIKE UPPER('%{valeurRecherche}%')
                       OR UPPER(nom + ' ' + prenom) LIKE UPPER('%{valeurRecherche}%')
                       OR UPPER(prenom + ' ' + nom) LIKE UPPER('%{valeurRecherche}%')
                    ORDER BY nom, prenom";
            }

            try
            {
                // Exécuter la requête
                DataTable resultat = Database.GetData(query);

                if (resultat.Rows.Count > 0)
                {
                    AfficherInformationsAgent(resultat.Rows[0]);
                    AfficherMessage("<i class='fas fa-check-circle'></i> Agent trouvé avec succès !", false);
                }
                else
                {
                    AfficherMessage("<i class='fas fa-times-circle'></i> Aucun agent trouvé avec cette recherche !", true);
                }
            }
            catch (Exception ex)
            {
                AfficherMessage($"<i class='fas fa-exclamation-triangle'></i> Erreur : {ex.Message}", true);
            }
        }

        private void AfficherInformationsAgent(DataRow agent)
        {
            // Informations personnelles
            lblID.Text = agent["matricule"].ToString();
            lblNom.Text = agent["nom"].ToString().ToUpper();
            lblPrenom.Text = agent["prenom"].ToString();
            lblCIN.Text = GetValueOrDefault(agent["cin"]);
            lblDateNaiss.Text = FormatDate(agent["date_n"]);
            lblLieuNaiss.Text = GetValueOrDefault(agent["lieu_n"]);
            lblSexe.Text = GetValueOrDefault(agent["sex"]);
            lblSituationFam.Text = GetValueOrDefault(agent["situation"]);

            // Informations professionnelles
            lblPPR.Text = GetValueOrDefault(agent["pecul"]);
            lblDateRecrutement.Text = FormatDate(agent["date_r"]);
            lblGrade.Text = "Non défini"; // À lier avec la table GRADE plus tard
            lblFonction.Text = "Non défini"; // À lier avec la table agent_fonction plus tard
            lblLieuAffect.Text = GetValueOrDefault(agent["ville"]);

            // Informations bancaires et autres
            lblBqv.Text = GetValueOrDefault(agent["bqv"]);
            lblBqe.Text = GetValueOrDefault(agent["bqe"]);
            lblCpt.Text = GetValueOrDefault(agent["cpt"]);
            lblPas.Text = GetValueOrDefault(agent["pas"]);

            // Statut retraite
            string retrait = GetValueOrDefault(agent["retrait"]);
            lblRetrait.Text = retrait == "1" ? "Oui" : retrait == "0" ? "Non" : retrait;
            lblTypeRet.Text = GetValueOrDefault(agent["Type_ret"]);
            lblPretLog.Text = GetValueOrDefault(agent["pret_log"]);

            // Contact
            lblTel1.Text = GetValueOrDefault(agent["tel"]);
            lblTel2.Text = GetValueOrDefault(agent["national"]);
            lblEmail.Text = "Non renseigné"; // Pas d'email dans la table
            lblAdresse.Text = GetValueOrDefault(agent["adresse"]);

            // Afficher le panel
            pnlResultat.Visible = true;
        }

        private string GetValueOrDefault(object value)
        {
            if (value == null || value == DBNull.Value || string.IsNullOrEmpty(value.ToString()))
            {
                return "<span class='empty-value'>Non renseigné</span>";
            }
            return value.ToString();
        }

        private string FormatDate(object dateValue)
        {
            if (dateValue != null && dateValue != DBNull.Value)
            {
                try
                {
                    DateTime date = Convert.ToDateTime(dateValue);
                    return date.ToString("dd/MM/yyyy");
                }
                catch
                {
                    return "<span class='empty-value'>Date invalide</span>";
                }
            }
            return "<span class='empty-value'>Non renseigné</span>";
        }

        private void AfficherMessage(string message, bool isError)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isError ? "alert alert-error" : "alert alert-success";
            lblMessage.Visible = true;
        }
    }
}