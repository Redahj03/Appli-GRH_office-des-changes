using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Missions
{
    public partial class ImprimerMission : System.Web.UI.Page
    {
        private string MissionId
        {
            get { return Request.QueryString["id"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(MissionId))
                {
                    Response.Redirect("ListeMissions.aspx");
                    return;
                }

                LoadMissionForPrint();
            }
        }

        private void LoadMissionForPrint()
        {
            try
            {
                string query = @"
                    SELECT 
                        om.ID,
                        om.matr as Matricule,
                        a.Nom,
                        a.Prénom,
                        CONCAT(a.Nom, ' ', a.Prénom) as NomComplet,
                        g.LIBELLE as Grade,
                        af.Libelle as Fonction,
                        aff.LIBELLE as Service,
                        om.ville as Destination,
                        om.Objet_mis as ObjetMission,
                        om.date_d as DateDepart,
                        om.Heure_d as HeureDepart,
                        om.date_r as DateRetour,
                        om.Heure_r as HeureRetour,
                        om.trpt as Transport,
                        om.Détail_trpt as DetailTransport,
                        m.Kilométrage,
                        m.Taux as TauxKm,
                        m.Nb_Jour as NbJours,
                        m.Taux_Jour as TauxJournalier,
                        m.Montant
                    FROM ordre_miss om
                    LEFT JOIN Agent a ON om.matr = a.Matricule
                    LEFT JOIN GRADE g ON a.Grade = g.CODEGRADE
                    LEFT JOIN AGENT_FONCTION_LIEN afl ON a.Matricule = afl.Matricule
                    LEFT JOIN AGENT_FONCTION af ON afl.CodeFonction = af.Code
                    LEFT JOIN LIEU_AFFECTATION aff ON a.Affectation = aff.CODE
                    LEFT JOIN Missions m ON om.matr = m.Matricule 
                        AND CAST(om.date_d as DATE) = CAST(m.Date_deplacement as DATE)
                    WHERE om.ID = @ID";

                DataTable dt = Database.GetDataWithParams(query,
                    new SqlParameter("@ID", MissionId));

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    // Numéro et date d'émission
                    lblNumeroMission.Text = row["ID"].ToString().PadLeft(4, '0');
                    lblAnnee.Text = DateTime.Now.Year.ToString();
                    lblDateEmission.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    // Informations de l'agent
                    lblMatricule.Text = row["Matricule"].ToString();
                    lblNomComplet.Text = row["NomComplet"]?.ToString() ?? "N/A";
                    lblGrade.Text = row["Grade"]?.ToString() ?? "N/A";
                    lblFonction.Text = row["Fonction"]?.ToString() ?? "N/A";
                    lblService.Text = row["Service"]?.ToString() ?? "Service non spécifié";

                    // Objet de la mission
                    lblObjetMission.Text = row["ObjetMission"]?.ToString() ?? "";

                    // Détails du déplacement
                    lblDestination.Text = row["Destination"]?.ToString() ?? "";

                    if (row["DateDepart"] != DBNull.Value)
                    {
                        DateTime dateDepart = Convert.ToDateTime(row["DateDepart"]);
                        lblDateDepart.Text = dateDepart.ToString("dd/MM/yyyy");

                        if (row["HeureDepart"] != DBNull.Value)
                        {
                            DateTime heureDepart = Convert.ToDateTime(row["HeureDepart"]);
                            lblHeureDepart.Text = heureDepart.ToString("HH:mm");
                        }
                    }

                    if (row["DateRetour"] != DBNull.Value)
                    {
                        DateTime dateRetour = Convert.ToDateTime(row["DateRetour"]);
                        lblDateRetour.Text = dateRetour.ToString("dd/MM/yyyy");

                        if (row["HeureRetour"] != DBNull.Value)
                        {
                            DateTime heureRetour = Convert.ToDateTime(row["HeureRetour"]);
                            lblHeureRetour.Text = heureRetour.ToString("HH:mm");
                        }

                        // Calculer la durée
                        if (row["DateDepart"] != DBNull.Value)
                        {
                            DateTime dateDepart = Convert.ToDateTime(row["DateDepart"]);
                            int jours = (dateRetour - dateDepart).Days + 1;
                            lblDuree.Text = $"{jours} jour(s)";
                        }
                    }

                    // Transport
                    lblTransport.Text = row["Transport"]?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(row["DetailTransport"]?.ToString()))
                    {
                        pnlDetailTransport.Visible = true;
                        lblDetailTransport.Text = row["DetailTransport"].ToString();
                    }

                    // Section financière
                    bool hasFinancialInfo = false;
                    decimal totalMission = 0;

                    // Indemnités journalières
                    if (row["NbJours"] != DBNull.Value && row["TauxJournalier"] != DBNull.Value)
                    {
                        int nbJours = Convert.ToInt32(row["NbJours"]);
                        decimal tauxJour = Convert.ToDecimal(row["TauxJournalier"]);
                        decimal totalJours = nbJours * tauxJour;

                        lblNbJours.Text = nbJours.ToString();
                        lblTauxJour.Text = tauxJour.ToString("0.00");
                        lblTotalJours.Text = totalJours.ToString("0.00");

                        totalMission += totalJours;
                        hasFinancialInfo = true;
                    }

                    // Indemnités kilométriques (voiture personnelle)
                    if (row["Transport"]?.ToString() == "Voiture personnelle" &&
                        row["Kilométrage"] != DBNull.Value && row["TauxKm"] != DBNull.Value)
                    {
                        pnlKilometrage.Visible = true;
                        decimal km = Convert.ToDecimal(row["Kilométrage"]);
                        decimal tauxKm = Convert.ToDecimal(row["TauxKm"]);
                        decimal totalKm = km * tauxKm;

                        lblKm.Text = km.ToString("0");
                        lblTauxKm.Text = tauxKm.ToString("0.00");
                        lblTotalKm.Text = totalKm.ToString("0.00");

                        totalMission += totalKm;
                        hasFinancialInfo = true;
                    }

                    // Afficher la section financière si nécessaire
                    if (hasFinancialInfo)
                    {
                        pnlFinancier.Visible = true;
                        lblMontantTotal.Text = totalMission.ToString("0.00");
                        lblMontantLettres.Text = ConvertirEnLettres(totalMission) + " dirhams";
                    }
                }
                else
                {
                    Response.Redirect("ListeMissions.aspx");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur LoadMissionForPrint: {ex.Message}");
            }
        }

        private string ConvertirEnLettres(decimal montant)
        {
            // Fonction simplifiée de conversion en lettres
            // Dans un cas réel, utilisez une bibliothèque complète
            int partieEntiere = (int)montant;
            int partieDecimale = (int)((montant - partieEntiere) * 100);

            string resultat = ConvertirNombreEnLettres(partieEntiere);

            if (partieDecimale > 0)
            {
                resultat += " virgule " + ConvertirNombreEnLettres(partieDecimale);
            }

            return resultat.Substring(0, 1).ToUpper() + resultat.Substring(1);
        }

        private string ConvertirNombreEnLettres(int nombre)
        {
            // Conversion simplifiée - à améliorer pour une production réelle
            string[] unites = { "", "un", "deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf" };
            string[] dizaines = { "", "dix", "vingt", "trente", "quarante", "cinquante", "soixante", "soixante-dix", "quatre-vingt", "quatre-vingt-dix" };
            string[] speciales = { "dix", "onze", "douze", "treize", "quatorze", "quinze", "seize", "dix-sept", "dix-huit", "dix-neuf" };

            if (nombre == 0)
                return "zéro";

            if (nombre < 10)
                return unites[nombre];

            if (nombre >= 10 && nombre < 20)
                return speciales[nombre - 10];

            if (nombre < 100)
            {
                int dizaine = nombre / 10;
                int unite = nombre % 10;

                if (unite == 0)
                    return dizaines[dizaine];
                else if (unite == 1 && (dizaine == 7 || dizaine == 9))
                    return dizaines[dizaine - 1] + "-et-" + speciales[1];
                else
                    return dizaines[dizaine] + "-" + unites[unite];
            }

            if (nombre < 1000)
            {
                int centaine = nombre / 100;
                int reste = nombre % 100;

                string resultat = "";
                if (centaine == 1)
                    resultat = "cent";
                else
                    resultat = unites[centaine] + " cent";

                if (reste > 0)
                    resultat += " " + ConvertirNombreEnLettres(reste);

                return resultat;
            }

            if (nombre < 1000000)
            {
                int milliers = nombre / 1000;
                int reste = nombre % 1000;

                string resultat = "";
                if (milliers == 1)
                    resultat = "mille";
                else
                    resultat = ConvertirNombreEnLettres(milliers) + " mille";

                if (reste > 0)
                    resultat += " " + ConvertirNombreEnLettres(reste);

                return resultat;
            }

            return nombre.ToString(); // Pour les très grands nombres
        }
    }
}