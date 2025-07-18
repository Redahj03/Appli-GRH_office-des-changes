using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Missions
{
    public partial class ModifierMission : System.Web.UI.Page
    {
        // Taux kilométriques par puissance fiscale (en DH)
        private readonly decimal[] tauxKilometriques = { 0, 0, 0, 0, 1.20m, 1.20m, 1.20m, 1.75m, 1.75m, 1.75m, 2.30m, 2.30m };

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

                LoadMissionData();
                LoadModificationHistory();
            }
        }

        private void LoadMissionData()
        {
            try
            {
                string query = @"
                    SELECT 
                        om.ID,
                        om.matr as Matricule,
                        CONCAT(a.Nom, ' ', a.Prénom) as NomComplet,
                        g.LIBELLE as Grade,
                        af.Libelle as Fonction,
                        om.ville as Ville,
                        om.Objet_mis as ObjetMission,
                        om.date_d as DateDepart,
                        om.Heure_d as HeureDepart,
                        om.date_r as DateRetour,
                        om.Heure_r as HeureRetour,
                        om.trpt as Transport,
                        om.Détail_trpt as DetailTransport,
                        ISNULL(om.Statut, 'En attente') as Statut,
                        m.Puiss_fisc as PuissanceFiscale,
                        m.Kilométrage,
                        m.Taux as TauxKm,
                        m.Nb_vacation as NombreVacations,
                        m.Taux_Jour as TauxJournalier
                    FROM ordre_miss om
                    LEFT JOIN Agent a ON om.matr = a.Matricule
                    LEFT JOIN GRADE g ON a.Grade = g.CODEGRADE
                    LEFT JOIN AGENT_FONCTION_LIEN afl ON a.Matricule = afl.Matricule
                    LEFT JOIN AGENT_FONCTION af ON afl.CodeFonction = af.Code
                    LEFT JOIN Missions m ON om.matr = m.Matricule 
                        AND CAST(om.date_d as DATE) = CAST(m.Date_deplacement as DATE)
                    WHERE om.ID = @ID";

                DataTable dt = Database.GetDataWithParams(query,
                    new SqlParameter("@ID", MissionId));

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    // Informations générales
                    lblMissionID.Text = row["ID"].ToString();
                    lblStatutActuel.Text = row["Statut"].ToString();

                    // Informations de l'agent (lecture seule)
                    txtMatricule.Text = row["Matricule"].ToString();
                    txtNomAgent.Text = row["NomComplet"]?.ToString() ?? "";
                    txtGrade.Text = row["Grade"]?.ToString() ?? "";
                    txtFonction.Text = row["Fonction"]?.ToString() ?? "";

                    // Détails de la mission
                    txtVille.Text = row["Ville"]?.ToString() ?? "";
                    txtObjet.Text = row["ObjetMission"]?.ToString() ?? "";
                    txtDetailTransport.Text = row["DetailTransport"]?.ToString() ?? "";

                    // Dates
                    if (row["DateDepart"] != DBNull.Value)
                    {
                        DateTime dateDepart = Convert.ToDateTime(row["DateDepart"]);
                        txtDateDepart.Text = dateDepart.ToString("yyyy-MM-dd");

                        if (row["HeureDepart"] != DBNull.Value)
                        {
                            DateTime heureDepart = Convert.ToDateTime(row["HeureDepart"]);
                            txtHeureDepart.Text = heureDepart.ToString("HH:mm");
                        }
                    }

                    if (row["DateRetour"] != DBNull.Value)
                    {
                        DateTime dateRetour = Convert.ToDateTime(row["DateRetour"]);
                        txtDateRetour.Text = dateRetour.ToString("yyyy-MM-dd");

                        if (row["HeureRetour"] != DBNull.Value)
                        {
                            DateTime heureRetour = Convert.ToDateTime(row["HeureRetour"]);
                            txtHeureRetour.Text = heureRetour.ToString("HH:mm");
                        }
                    }

                    // Transport
                    string transport = row["Transport"]?.ToString() ?? "";
                    switch (transport)
                    {
                        case "Voiture de service":
                            rbVoitureService.Checked = true;
                            break;
                        case "Voiture personnelle":
                            rbVoiturePersonnelle.Checked = true;
                            pnlIndemnites.Visible = true;
                            break;
                        case "Train":
                            rbTrain.Checked = true;
                            break;
                        case "Avion":
                            rbAvion.Checked = true;
                            break;
                    }

                    // Indemnités kilométriques (si voiture personnelle)
                    if (transport == "Voiture personnelle")
                    {
                        if (row["PuissanceFiscale"] != DBNull.Value)
                            ddlPuissanceFiscale.SelectedValue = row["PuissanceFiscale"].ToString();

                        if (row["Kilométrage"] != DBNull.Value)
                            txtKilometrage.Text = Convert.ToDecimal(row["Kilométrage"]).ToString("0");

                        if (row["TauxKm"] != DBNull.Value)
                            txtTauxKm.Text = Convert.ToDecimal(row["TauxKm"]).ToString("0.00");

                        CalculerIndemnites(null, null);
                    }

                    // Indemnités journalières
                    if (row["NombreVacations"] != DBNull.Value)
                        txtNombreVacations.Text = row["NombreVacations"].ToString();

                    if (row["TauxJournalier"] != DBNull.Value)
                        txtTauxJournalier.Text = Convert.ToDecimal(row["TauxJournalier"]).ToString("0.00");

                    // Calculer le nombre de jours
                    CalculerNombreJours();
                }
                else
                {
                    Response.Redirect("ListeMissions.aspx");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur LoadMissionData: {ex.Message}");
                ShowAlert("Erreur lors du chargement des données.");
            }
        }

        private void LoadModificationHistory()
        {
            // À implémenter si vous avez une table d'historique des modifications
            // Pour l'instant, on masque le panneau
            pnlHistory.Visible = false;
        }

        private void CalculerNombreJours()
        {
            if (!string.IsNullOrEmpty(txtDateDepart.Text) && !string.IsNullOrEmpty(txtDateRetour.Text))
            {
                DateTime dateDepart = Convert.ToDateTime(txtDateDepart.Text);
                DateTime dateRetour = Convert.ToDateTime(txtDateRetour.Text);
                int jours = (dateRetour - dateDepart).Days + 1;
                txtNombreJours.Text = jours > 0 ? jours.ToString() : "1";
            }
        }

        protected void Transport_CheckedChanged(object sender, EventArgs e)
        {
            pnlIndemnites.Visible = rbVoiturePersonnelle.Checked;
        }

        protected void CalculerIndemnites(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlPuissanceFiscale.SelectedValue) ||
                string.IsNullOrEmpty(txtKilometrage.Text))
            {
                divResultatCalcul.Visible = false;
                return;
            }

            try
            {
                int puissance = Convert.ToInt32(ddlPuissanceFiscale.SelectedValue);
                decimal kilometrage = Convert.ToDecimal(txtKilometrage.Text);
                decimal taux = tauxKilometriques[puissance];
                decimal total = kilometrage * taux;

                // Afficher le taux
                txtTauxKm.Text = taux.ToString("0.00");

                // Afficher le résultat du calcul
                lblDistance.Text = kilometrage.ToString("0");
                lblTaux.Text = taux.ToString("0.00");
                lblTotalIndemnites.Text = total.ToString("0.00");

                divResultatCalcul.Visible = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur calcul indemnités: {ex.Message}");
            }
        }

        protected void btnSauvegarder_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                UpdateMission();
            }
        }

        private bool ValidateForm()
        {
            // Validation des champs obligatoires
            if (string.IsNullOrEmpty(txtVille.Text))
            {
                ShowAlert("Veuillez entrer la ville de destination.");
                return false;
            }

            if (string.IsNullOrEmpty(txtObjet.Text))
            {
                ShowAlert("Veuillez entrer l'objet de la mission.");
                return false;
            }

            if (string.IsNullOrEmpty(txtDateDepart.Text) || string.IsNullOrEmpty(txtDateRetour.Text))
            {
                ShowAlert("Veuillez entrer les dates de départ et de retour.");
                return false;
            }

            // Vérifier qu'un moyen de transport est sélectionné
            if (!rbVoitureService.Checked && !rbVoiturePersonnelle.Checked &&
                !rbTrain.Checked && !rbAvion.Checked)
            {
                ShowAlert("Veuillez sélectionner un moyen de transport.");
                return false;
            }

            // Vérifier que la date de retour est après la date de départ
            DateTime dateDepart = Convert.ToDateTime(txtDateDepart.Text);
            DateTime dateRetour = Convert.ToDateTime(txtDateRetour.Text);

            if (dateRetour < dateDepart)
            {
                ShowAlert("La date de retour doit être après la date de départ.");
                return false;
            }

            return true;
        }

        private void UpdateMission()
        {
            try
            {
                // Déterminer le moyen de transport sélectionné
                string transport = "";
                if (rbVoitureService.Checked) transport = "Voiture de service";
                else if (rbVoiturePersonnelle.Checked) transport = "Voiture personnelle";
                else if (rbTrain.Checked) transport = "Train";
                else if (rbAvion.Checked) transport = "Avion";

                // Construire les dates avec heures
                DateTime dateDepart = Convert.ToDateTime(txtDateDepart.Text);
                if (!string.IsNullOrEmpty(txtHeureDepart.Text))
                {
                    TimeSpan heureDepart = TimeSpan.Parse(txtHeureDepart.Text);
                    dateDepart = dateDepart.Add(heureDepart);
                }

                DateTime dateRetour = Convert.ToDateTime(txtDateRetour.Text);
                if (!string.IsNullOrEmpty(txtHeureRetour.Text))
                {
                    TimeSpan heureRetour = TimeSpan.Parse(txtHeureRetour.Text);
                    dateRetour = dateRetour.Add(heureRetour);
                }

                // Mettre à jour ordre_miss
                string queryOrdre = @"
                    UPDATE ordre_miss 
                    SET ville = @Ville,
                        Objet_mis = @Objet,
                        date_d = @DateDepart,
                        Heure_d = @HeureDepart,
                        date_r = @DateRetour,
                        Heure_r = @HeureRetour,
                        trpt = @Transport,
                        Détail_trpt = @DetailTransport
                    WHERE ID = @ID";

                SqlParameter[] paramsOrdre = {
                    new SqlParameter("@ID", MissionId),
                    new SqlParameter("@Ville", txtVille.Text),
                    new SqlParameter("@Objet", txtObjet.Text),
                    new SqlParameter("@DateDepart", dateDepart),
                    new SqlParameter("@HeureDepart", dateDepart),
                    new SqlParameter("@DateRetour", dateRetour),
                    new SqlParameter("@HeureRetour", dateRetour),
                    new SqlParameter("@Transport", transport),
                    new SqlParameter("@DetailTransport", txtDetailTransport.Text)
                };

                Database.ExecuteNonQuery(queryOrdre, paramsOrdre);

                // Si voiture personnelle, mettre à jour aussi la table Missions
                if (rbVoiturePersonnelle.Checked && !string.IsNullOrEmpty(txtKilometrage.Text))
                {
                    decimal montant = 0;
                    if (!string.IsNullOrEmpty(txtKilometrage.Text) && !string.IsNullOrEmpty(txtTauxKm.Text))
                    {
                        montant = Convert.ToDecimal(txtKilometrage.Text) * Convert.ToDecimal(txtTauxKm.Text);
                    }

                    // Vérifier si un enregistrement existe déjà
                    string queryCheck = @"
                        SELECT COUNT(*) 
                        FROM Missions 
                        WHERE Matricule = @Matricule 
                        AND CAST(Date_deplacement as DATE) = CAST(@DateDeplacement as DATE)";

                    int count = Convert.ToInt32(Database.ExecuteScalar(queryCheck,
                        new SqlParameter("@Matricule", txtMatricule.Text),
                        new SqlParameter("@DateDeplacement", dateDepart)));

                    if (count > 0)
                    {
                        // Mettre à jour
                        string queryUpdateMission = @"
                            UPDATE Missions 
                            SET Moyen_trans = @MoyenTrans,
                                Montant = @Montant,
                                Puiss_fisc = @PuissanceFiscale,
                                Kilométrage = @Kilometrage,
                                Nb_vacation = @NbVacation,
                                Taux = @Taux,
                                Nb_Jour = @NbJour,
                                Taux_Jour = @TauxJour
                            WHERE Matricule = @Matricule 
                            AND CAST(Date_deplacement as DATE) = CAST(@DateDeplacement as DATE)";

                        SqlParameter[] paramsUpdateMission = {
                            new SqlParameter("@Matricule", txtMatricule.Text),
                            new SqlParameter("@DateDeplacement", dateDepart),
                            new SqlParameter("@MoyenTrans", transport),
                            new SqlParameter("@Montant", montant),
                            new SqlParameter("@PuissanceFiscale",
                                string.IsNullOrEmpty(ddlPuissanceFiscale.SelectedValue) ?
                                DBNull.Value : (object)Convert.ToInt16(ddlPuissanceFiscale.SelectedValue)),
                            new SqlParameter("@Kilometrage",
                                string.IsNullOrEmpty(txtKilometrage.Text) ?
                                DBNull.Value : (object)Convert.ToSingle(txtKilometrage.Text)),
                            new SqlParameter("@NbVacation",
                                string.IsNullOrEmpty(txtNombreVacations.Text) ?
                                DBNull.Value : (object)Convert.ToInt16(txtNombreVacations.Text)),
                            new SqlParameter("@Taux",
                                string.IsNullOrEmpty(txtTauxKm.Text) ?
                                DBNull.Value : (object)Convert.ToDecimal(txtTauxKm.Text)),
                            new SqlParameter("@NbJour",
                                string.IsNullOrEmpty(txtNombreJours.Text) ?
                                DBNull.Value : (object)Convert.ToInt16(txtNombreJours.Text)),
                            new SqlParameter("@TauxJour",
                                string.IsNullOrEmpty(txtTauxJournalier.Text) ?
                                DBNull.Value : (object)Convert.ToDecimal(txtTauxJournalier.Text))
                        };

                        Database.ExecuteNonQuery(queryUpdateMission, paramsUpdateMission);
                    }
                    else
                    {
                        // Insérer
                        string queryInsertMission = @"
                            INSERT INTO Missions (Matricule, Date_deplacement, Moyen_trans, Montant,
                                                Puiss_fisc, Kilométrage, Nb_vacation, Nom, Grade,
                                                Taux, Nb_Jour, Taux_Jour)
                            VALUES (@Matricule, @DateDeplacement, @MoyenTrans, @Montant,
                                    @PuissanceFiscale, @Kilometrage, @NbVacation, @Nom, @Grade,
                                    @Taux, @NbJour, @TauxJour)";

                        SqlParameter[] paramsInsertMission = {
                            new SqlParameter("@Matricule", txtMatricule.Text),
                            new SqlParameter("@DateDeplacement", dateDepart),
                            new SqlParameter("@MoyenTrans", transport),
                            new SqlParameter("@Montant", montant),
                            new SqlParameter("@PuissanceFiscale",
                                string.IsNullOrEmpty(ddlPuissanceFiscale.SelectedValue) ?
                                DBNull.Value : (object)Convert.ToInt16(ddlPuissanceFiscale.SelectedValue)),
                            new SqlParameter("@Kilometrage",
                                string.IsNullOrEmpty(txtKilometrage.Text) ?
                                DBNull.Value : (object)Convert.ToSingle(txtKilometrage.Text)),
                            new SqlParameter("@NbVacation",
                                string.IsNullOrEmpty(txtNombreVacations.Text) ?
                                DBNull.Value : (object)Convert.ToInt16(txtNombreVacations.Text)),
                            new SqlParameter("@Nom", txtNomAgent.Text),
                            new SqlParameter("@Grade", txtGrade.Text),
                            new SqlParameter("@Taux",
                                string.IsNullOrEmpty(txtTauxKm.Text) ?
                                DBNull.Value : (object)Convert.ToDecimal(txtTauxKm.Text)),
                            new SqlParameter("@NbJour",
                                string.IsNullOrEmpty(txtNombreJours.Text) ?
                                DBNull.Value : (object)Convert.ToInt16(txtNombreJours.Text)),
                            new SqlParameter("@TauxJour",
                                string.IsNullOrEmpty(txtTauxJournalier.Text) ?
                                DBNull.Value : (object)Convert.ToDecimal(txtTauxJournalier.Text))
                        };

                        Database.ExecuteNonQuery(queryInsertMission, paramsInsertMission);
                    }
                }
                else if (transport != "Voiture personnelle")
                {
                    // Si ce n'est plus une voiture personnelle, supprimer l'enregistrement de Missions
                    string queryDelete = @"
                        DELETE FROM Missions 
                        WHERE Matricule = @Matricule 
                        AND CAST(Date_deplacement as DATE) = CAST(@DateDeplacement as DATE)";

                    Database.ExecuteNonQuery(queryDelete,
                        new SqlParameter("@Matricule", txtMatricule.Text),
                        new SqlParameter("@DateDeplacement", dateDepart));
                }

                ShowAlert("Mission modifiée avec succès!");

                // Rediriger vers les détails
                Response.Redirect($"DetailsMission.aspx?id={MissionId}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur UpdateMission: {ex.Message}");
                ShowAlert("Erreur lors de la modification de la mission.");
            }
        }

        protected void btnAnnuler_Click(object sender, EventArgs e)
        {
            Response.Redirect($"DetailsMission.aspx?id={MissionId}");
        }

        private void ShowAlert(string message)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert",
                $"alert('{message.Replace("'", "\\'")}');", true);
        }
    }
}