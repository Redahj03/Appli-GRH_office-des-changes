using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Enfants
{
    public partial class AjouterEnfant : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ChargerAgents();

                // Si un matricule est passé en paramètre
                string matricule = Request.QueryString["matricule"];
                if (!string.IsNullOrEmpty(matricule))
                {
                    ddlMatricule.SelectedValue = matricule;
                    AfficherInfoAgent();
                }

                // Configurer le validateur de date
                cvDateNaissance.ValueToCompare = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        private void ChargerAgents()
        {
            try
            {
                // Récupérer la liste des agents
                string query = "SELECT matricule, nom, prenom FROM AGENT ORDER BY nom, prenom";
                DataTable dtAgents = Database.GetData(query);

                ddlMatricule.Items.Clear();
                ddlMatricule.Items.Add(new ListItem("-- Sélectionner un agent --", ""));

                foreach (DataRow row in dtAgents.Rows)
                {
                    string matricule = row["matricule"].ToString();
                    string nom = row["nom"].ToString();
                    string prenom = row["prenom"].ToString();
                    string display = $"{matricule} - {nom} {prenom}";

                    ddlMatricule.Items.Add(new ListItem(display, matricule));
                }
            }
            catch (Exception ex)
            {
                AfficherMessage($"Erreur lors du chargement des agents : {ex.Message}", "danger");
            }
        }

        protected void ddlMatricule_SelectedIndexChanged(object sender, EventArgs e)
        {
            AfficherInfoAgent();
        }

        private void AfficherInfoAgent()
        {
            if (!string.IsNullOrEmpty(ddlMatricule.SelectedValue))
            {
                try
                {
                    // Récupérer les informations de l'agent
                    string query = "SELECT nom, prenom FROM AGENT WHERE matricule = @matricule";
                    DataTable dt = Database.GetDataWithParams(query,
                        new System.Data.SqlClient.SqlParameter("@matricule", ddlMatricule.SelectedValue));

                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        string nomAgent = row["nom"].ToString();
                        string prenomAgent = row["prenom"].ToString();

                        lblAgentInfo.Text = $"{ddlMatricule.SelectedValue} - {nomAgent} {prenomAgent}";
                        pnlAgentInfo.Visible = true;

                        // Compter les enfants existants
                        string countQuery = "SELECT COUNT(*) FROM AGENT_EF WHERE matricule = @matricule";
                        object count = Database.ExecuteScalar(countQuery,
                            new System.Data.SqlClient.SqlParameter("@matricule", ddlMatricule.SelectedValue));

                        int nbEnfants = Convert.ToInt32(count);
                        if (nbEnfants > 0)
                        {
                            lblAgentInfo.Text += $" (Enfants existants : {nbEnfants})";
                        }
                    }
                }
                catch (Exception ex)
                {
                    AfficherMessage($"Erreur : {ex.Message}", "danger");
                }
            }
            else
            {
                pnlAgentInfo.Visible = false;
            }
        }

        protected void rblScolarise_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool estScolarise = rblScolarise.SelectedValue == "O";
            pnlScolarite.Visible = estScolarise;
            pnlDateScolarite.Visible = estScolarise;
        }

        protected void btnEnregistrer_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    // Créer l'objet Enfant
                    Enfant enfant = new Enfant
                    {
                        Matricule = ddlMatricule.SelectedValue,
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
                        }
                    }

                    // Enregistrer l'enfant
                    bool success = EnfantManager.AjouterEnfant(enfant);

                    if (success)
                    {
                        // Stocker le message en session pour l'afficher sur la page de liste
                        Session["Message"] = $"Enfant {enfant.NomComplet} ajouté avec succès (N°{enfant.Num})";
                        Session["TypeMessage"] = "success";

                        // Rediriger vers la liste
                        Response.Redirect("ListeEnfants.aspx");
                    }
                    else
                    {
                        AfficherMessage("Erreur lors de l'ajout de l'enfant.", "danger");
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