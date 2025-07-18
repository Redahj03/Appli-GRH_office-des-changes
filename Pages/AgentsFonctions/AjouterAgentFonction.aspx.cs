using System;
using System.Data;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.AgentsFonctions
{
    public partial class AjouterAgentFonction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAgents();
                // Générer le prochain numéro d'ordre
                int nextNum = AgentFonctionManager.GetNextNumOrd();
                txtNumOrd.Text = nextNum.ToString();
            }
        }

        private void LoadAgents()
        {
            try
            {
                DataTable dt = AgentFonctionManager.GetAgentsForDropdown();
                ddlAgent.DataSource = dt;
                ddlAgent.DataTextField = "agent_display";
                ddlAgent.DataValueField = "matricule";
                ddlAgent.DataBind();
                ddlAgent.Items.Insert(0, new ListItem("-- Sélectionner un agent --", ""));
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors du chargement des agents: {ex.Message}", false);
            }
        }

        protected void ddlAgent_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(ddlAgent.SelectedValue))
                {
                    // Récupérer les infos de l'agent depuis la table Agent
                    string query = $"SELECT [matricule], [nom], [prenom] FROM dbo.Agent WHERE [matricule] = '{ddlAgent.SelectedValue.Replace("'", "''")}'";
                    DataTable dt = Database.GetData(query);

                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        txtMatricule.Text = row["matricule"].ToString();
                        txtNom.Text = row["nom"].ToString();
                        txtPrenom.Text = row["prenom"].ToString();
                    }
                }
                else
                {
                    // Réinitialiser les champs
                    txtMatricule.Text = "";
                    txtNom.Text = "";
                    txtPrenom.Text = "";
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors du chargement des informations de l'agent: {ex.Message}", false);
            }
        }

        protected void btnEnregistrer_Click(object sender, EventArgs e)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(ddlAgent.SelectedValue))
                {
                    ShowMessage("Veuillez sélectionner un agent.", false);
                    ddlAgent.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtAffectation.Text))
                {
                    ShowMessage("L'affectation est obligatoire.", false);
                    txtAffectation.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtFonction.Text))
                {
                    ShowMessage("La fonction est obligatoire.", false);
                    txtFonction.Focus();
                    return;
                }

                int numOrd = Convert.ToInt32(txtNumOrd.Text);

                // Vérifier si le numéro existe déjà (au cas où)
                if (AgentFonctionManager.NumOrdExists(numOrd))
                {
                    // Générer un nouveau numéro
                    numOrd = AgentFonctionManager.GetNextNumOrd();
                    txtNumOrd.Text = numOrd.ToString();
                }

                // Ajouter la fonction
                bool success = AgentFonctionManager.AjouterAgentFonction(
                    numOrd,
                    txtMatricule.Text.Trim(),
                    txtNom.Text.Trim(),
                    txtPrenom.Text.Trim(),
                    txtAffectation.Text.Trim(),
                    txtFonction.Text.Trim()
                );

                if (success)
                {
                    Response.Redirect("ListeAgentsFonctions.aspx?success=add");
                }
                else
                {
                    ShowMessage("Erreur lors de l'ajout de la fonction.", false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur: {ex.Message}", false);
            }
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            string iconClass = isSuccess ? "fas fa-check-circle" : "fas fa-exclamation-circle";
            lblMessage.Text = $"<i class='{iconClass}'></i> {message}";
            pnlMessage.CssClass = $"alert alert-{(isSuccess ? "success" : "error")}";
            pnlMessage.Visible = true;
        }
    }
}