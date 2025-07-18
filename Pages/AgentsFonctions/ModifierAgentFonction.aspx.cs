using System;
using System.Data;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.AgentsFonctions
{
    public partial class ModifierAgentFonction : System.Web.UI.Page
    {
        private int NumOrd
        {
            get { return ViewState["NumOrd"] != null ? (int)ViewState["NumOrd"] : 0; }
            set { ViewState["NumOrd"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAgents();

                if (Request.QueryString["num"] != null)
                {
                    int num;
                    if (int.TryParse(Request.QueryString["num"], out num))
                    {
                        NumOrd = num;
                        LoadAgentFonction(num);
                    }
                    else
                    {
                        Response.Redirect("ListeAgentsFonctions.aspx");
                    }
                }
                else
                {
                    Response.Redirect("ListeAgentsFonctions.aspx");
                }
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

        private void LoadAgentFonction(int numOrd)
        {
            try
            {
                DataTable dt = AgentFonctionManager.GetAgentFonctionByNumOrd(numOrd);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    txtNumOrd.Text = row["num_ord"].ToString();
                    txtMatricule.Text = row["matricule"].ToString();
                    txtNom.Text = row["nom"].ToString();
                    txtPrenom.Text = row["prenom"].ToString();
                    txtAffectation.Text = row["affectation"].ToString();
                    txtFonction.Text = row["fonction"].ToString();

                    // Sélectionner l'agent dans la dropdown
                    if (ddlAgent.Items.FindByValue(row["matricule"].ToString()) != null)
                    {
                        ddlAgent.SelectedValue = row["matricule"].ToString();
                    }
                }
                else
                {
                    ShowMessage("Fonction introuvable.", false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur lors du chargement de la fonction: {ex.Message}", false);
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

                // Modifier la fonction
                bool success = AgentFonctionManager.ModifierAgentFonction(
                    NumOrd,
                    txtMatricule.Text.Trim(),
                    txtNom.Text.Trim(),
                    txtPrenom.Text.Trim(),
                    txtAffectation.Text.Trim(),
                    txtFonction.Text.Trim()
                );

                if (success)
                {
                    Response.Redirect("ListeAgentsFonctions.aspx?success=edit");
                }
                else
                {
                    ShowMessage("Erreur lors de la modification de la fonction.", false);
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