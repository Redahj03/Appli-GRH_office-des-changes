/*using System;
using System.Data;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Sanctions
{
    public partial class AttribuerSanction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAgents();
                LoadTypesSanctions();
                txtDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        private void LoadAgents()
        {
            try
            {
                // Charger la liste des agents
                DataTable dtAgents = AgentManager.GetAllAgents();
                ddlAgent.DataSource = dtAgents;
                ddlAgent.DataTextField = "NomComplet"; // Assurez-vous que cette colonne existe
                ddlAgent.DataValueField = "matricule";
                ddlAgent.DataBind();
                ddlAgent.Items.Insert(0, new ListItem("-- Sélectionner un agent --", ""));
            }
            catch (Exception ex)
            {
                // Gérer l'erreur
            }
        }

        private void LoadTypesSanctions()
        {
            try
            {
                // Charger la liste des types de sanctions
                DataTable dtTypes = SanctionManager.GetAllTypesSanctions();
                ddlTypeSanction.DataSource = dtTypes;
                ddlTypeSanction.DataTextField = "libelle_sc";
                ddlTypeSanction.DataValueField = "code_SC";
                ddlTypeSanction.DataBind();
                ddlTypeSanction.Items.Insert(0, new ListItem("-- Sélectionner un type --", ""));
            }
            catch (Exception ex)
            {
                // Gérer l'erreur
            }
        }

        protected void btnEnregistrer_Click(object sender, EventArgs e)
        {
            try
            {
                string matricule = ddlAgent.SelectedValue;
                int codeSC = Convert.ToInt32(ddlTypeSanction.SelectedValue);
                DateTime date = Convert.ToDateTime(txtDate.Text);
                int duree = Convert.ToInt32(txtDuree.Text);

                bool success = SanctionManager.AttribuerSanction(matricule, codeSC, date, duree);

                if (success)
                {
                    Response.Redirect("AgentsSanctions.aspx");
                }
            }
            catch (Exception ex)
            {
                // Gérer l'erreur
            }
        }
    }
}*/


// Pages/Sanctions/AttribuerSanction.aspx.cs
using System;
using System.Data;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Sanctions
{
    public partial class AttribuerSanction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAgents();
                LoadTypesSanctions();
                // Initialise le champ date (input type="date")
                txtDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        private void LoadAgents()
        {
            // 1) Récupère la table des agents
            DataTable dtAgents = AgentManager.GetAllAgents();
            if (dtAgents == null)
                throw new InvalidOperationException("GetAllAgents() a retourné null.");

            // 2) S’il n’y a pas de ligne, affiche un message
            if (dtAgents.Rows.Count == 0)
            {
                ddlAgent.Items.Clear();
                ddlAgent.Items.Add(new ListItem("-- Aucun agent disponible --", ""));
                return;
            }

            // 3) Crée la colonne NomComplet si elle n’existe pas
            if (!dtAgents.Columns.Contains("NomComplet"))
            {
                dtAgents.Columns.Add("NomComplet", typeof(string));
                foreach (DataRow row in dtAgents.Rows)
                {
                    string nom = row["nom"].ToString();    // adaptez si nécessaire
                    string prenom = row["prenom"].ToString(); // idem
                    row["NomComplet"] = $"{nom} {prenom}";
                }
            }

            // 4) Bind sur la DropDownList
            ddlAgent.DataSource = dtAgents;
            ddlAgent.DataTextField = "NomComplet";
            ddlAgent.DataValueField = "matricule";
            ddlAgent.DataBind();

            // 5) Ajoute l’option par défaut
            ddlAgent.Items.Insert(0, new ListItem("-- Sélectionner un agent --", ""));
        }

        private void LoadTypesSanctions()
        {
            DataTable dtTypes = SanctionManager.GetAllTypesSanctions();
            if (dtTypes == null)
                throw new InvalidOperationException("GetAllTypesSanctions() a retourné null.");

            if (dtTypes.Rows.Count == 0)
            {
                ddlTypeSanction.Items.Clear();
                ddlTypeSanction.Items.Add(new ListItem("-- Aucun type disponible --", ""));
                return;
            }

            ddlTypeSanction.DataSource = dtTypes;
            ddlTypeSanction.DataTextField = "libelle_sc";
            ddlTypeSanction.DataValueField = "code_SC";
            ddlTypeSanction.DataBind();
            ddlTypeSanction.Items.Insert(0, new ListItem("-- Sélectionner un type --", ""));
        }

        protected void btnEnregistrer_Click(object sender, EventArgs e)
        {
            // Vide le label d’erreur
            lblErreur.Text = "";

            // Vérifie que l’utilisateur a bien sélectionné les deux listes
            if (string.IsNullOrEmpty(ddlAgent.SelectedValue) ||
                string.IsNullOrEmpty(ddlTypeSanction.SelectedValue))
            {
                lblErreur.Text = "Veuillez sélectionner un agent et un type de sanction.";
                return;
            }

            try
            {
                string matricule = ddlAgent.SelectedValue;
                int codeSC = Convert.ToInt32(ddlTypeSanction.SelectedValue);
                DateTime date = DateTime.Parse(txtDate.Text);
                int duree = int.Parse(txtDuree.Text);

                bool success = SanctionManager.AttribuerSanction(matricule, codeSC, date, duree);
                if (!success)
                    throw new ApplicationException("La méthode AttribuerSanction a retourné false.");

                // Redirection vers la liste des sanctions en cas de succès
                Response.Redirect("AgentsSanctions.aspx");
            }
            catch (Exception ex)
            {
                // Affiche le message d’erreur pour debug
                lblErreur.Text = "Erreur lors de l’enregistrement : " + ex.Message;
            }
        }
    }
}
