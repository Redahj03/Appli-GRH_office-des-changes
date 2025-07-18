using System;
using System.Data;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Agents
{
    public partial class ModifierAgent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string matricule = Request.QueryString["matricule"];
                if (!string.IsNullOrEmpty(matricule))
                {
                    LoadAgent(matricule);
                }
                else
                {
                    Response.Redirect("ListeAgents.aspx");
                }
            }
        }

        private void LoadAgent(string matricule)
        {
            DataTable dt = AgentManager.GetAgentByMatricule(matricule);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                txtMatricule.Text = row["matricule"].ToString();
                txtCIN.Text = row["cin"].ToString();
                txtNom.Text = row["nom"].ToString();
                txtPrenom.Text = row["prenom"].ToString();
                txtDateNaissance.Text = row["date_n"] != DBNull.Value ?
                    Convert.ToDateTime(row["date_n"]).ToString("yyyy-MM-dd") : "";
                txtLieuNaissance.Text = row["lieu_n"].ToString();
                ddlSexe.SelectedValue = row["sex"].ToString();
                ddlSituation.SelectedValue = row["situation"].ToString();
                txtDateRecrutement.Text = row["date_r"] != DBNull.Value ?
                    Convert.ToDateTime(row["date_r"]).ToString("yyyy-MM-dd") : "";
                txtPecul.Text = row["pecul"].ToString();
                txtVille.Text = row["ville"].ToString();
                txtPas.Text = row["pas"].ToString();
                ddlRetrait.SelectedValue = row["retrait"].ToString() == "1" ? "1" : "0";
                txtTypeRet.Text = row["Type_ret"].ToString();
                txtPretLog.Text = row["pret_log"].ToString();
                txtBqv.Text = row["bqv"].ToString();
                txtBqe.Text = row["bqe"].ToString();
                txtCpt.Text = row["cpt"].ToString();
                txtTel.Text = row["tel"].ToString();
                txtNational.Text = row["national"].ToString();
                txtAdresse.Text = row["adresse"].ToString();
            }
        }

        protected void btnEnregistrer_Click(object sender, EventArgs e)
        {
            try
            {
                bool success = AgentManager.ModifierAgent(
                    txtMatricule.Text,
                    txtNom.Text,
                    txtPrenom.Text,
                    txtDateNaissance.Text,
                    txtLieuNaissance.Text,
                    txtDateRecrutement.Text,
                    txtPecul.Text,
                    ddlSexe.SelectedValue,
                    txtCIN.Text,
                    txtTel.Text,
                    txtNational.Text,
                    ddlSituation.SelectedValue,
                    txtAdresse.Text,
                    txtVille.Text,
                    txtBqv.Text,
                    txtBqe.Text,
                    txtCpt.Text,
                    txtPas.Text,
                    ddlRetrait.SelectedValue,
                    txtTypeRet.Text,
                    txtPretLog.Text
                );

                if (success)
                {
                    ShowMessage("Agent modifié avec succès !", true);
                    Response.AddHeader("REFRESH", "2;URL=ListeAgents.aspx");
                }
                else
                {
                    ShowMessage("Erreur lors de la modification de l'agent.", false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Erreur : {ex.Message}", false);
            }
        }

        protected void btnAnnuler_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListeAgents.aspx");
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = $"<i class='fas fa-{(isSuccess ? "check-circle" : "exclamation-circle")}'></i> {message}";
            pnlMessage.CssClass = $"alert alert-{(isSuccess ? "success" : "error")}";
            pnlMessage.Visible = true;
        }
    }
}