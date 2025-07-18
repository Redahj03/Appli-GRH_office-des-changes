using System;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Agents
{
    public partial class AjouterAgent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlMessage.Visible = false;
            }
        }

        protected void txtMatricule_TextChanged(object sender, EventArgs e)
        {
            string matricule = txtMatricule.Text.Trim();
            if (!string.IsNullOrEmpty(matricule))
            {
                try
                {
                    bool existe = AgentManager.MatriculeExiste(matricule);
                    if (existe)
                    {
                        ShowMessage($"⚠️ Le matricule '{matricule}' existe déjà !", false);
                        txtMatricule.Focus();
                    }
                    else
                    {
                        pnlMessage.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage($"Erreur lors de la vérification: {ex.Message}", false);
                }
            }
        }

        protected void btnEnregistrer_Click(object sender, EventArgs e)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(txtMatricule.Text))
                {
                    ShowMessage("Le matricule est obligatoire.", false);
                    txtMatricule.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNom.Text))
                {
                    ShowMessage("Le nom est obligatoire.", false);
                    txtNom.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPrenom.Text))
                {
                    ShowMessage("Le prénom est obligatoire.", false);
                    txtPrenom.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCIN.Text))
                {
                    ShowMessage("Le CIN est obligatoire.", false);
                    txtCIN.Focus();
                    return;
                }

                string matricule = txtMatricule.Text.Trim();
                string cin = txtCIN.Text.Trim();

                // Vérifications d'unicité
                if (AgentManager.MatriculeExiste(matricule))
                {
                    ShowMessage($"Le matricule '{matricule}' existe déjà.", false);
                    txtMatricule.Focus();
                    return;
                }

                if (AgentManager.CinExiste(cin))
                {
                    ShowMessage($"Le CIN '{cin}' existe déjà.", false);
                    txtCIN.Focus();
                    return;
                }

                // Ajout de l'agent
                bool success = AgentManager.AjouterAgent(
                    matricule,
                    txtNom.Text.Trim(),
                    txtPrenom.Text.Trim(),
                    txtDateNaissance.Text,
                    txtLieuNaissance.Text.Trim(),
                    txtDateRecrutement.Text,
                    txtPecul.Text.Trim(),
                    ddlSexe.SelectedValue,
                    cin,
                    txtTel.Text.Trim(),
                    txtNational.Text.Trim(),
                    ddlSituation.SelectedValue,
                    txtAdresse.Text.Trim(),
                    txtVille.Text.Trim(),
                    txtBqv.Text.Trim(),
                    txtBqe.Text.Trim(),
                    txtCpt.Text.Trim(),
                    txtPas.Text.Trim(),
                    ddlRetrait.SelectedValue,
                    txtTypeRet.Text.Trim(),
                    txtPretLog.Text.Trim()
                );

                if (success)
                {
                    ShowMessage($"✅ Agent '{txtNom.Text.Trim()} {txtPrenom.Text.Trim()}' ajouté avec succès !", true);
                    ClearForm();
                    Response.AddHeader("REFRESH", "3;URL=ListeAgents.aspx");
                }
                else
                {
                    ShowMessage("❌ Erreur lors de l'ajout de l'agent.", false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"❌ Erreur: {ex.Message}", false);
                System.Diagnostics.Debug.WriteLine($"❌ Erreur btnEnregistrer_Click: {ex}");
            }
        }

        protected void btnAnnuler_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListeAgents.aspx");
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            string iconClass = isSuccess ? "fas fa-check-circle" : "fas fa-exclamation-circle";
            lblMessage.Text = $"<i class='{iconClass}'></i> {message}";
            pnlMessage.CssClass = $"alert alert-{(isSuccess ? "success" : "error")}";
            pnlMessage.Visible = true;
        }

        private void ClearForm()
        {
            txtMatricule.Text = "";
            txtCIN.Text = "";
            txtNom.Text = "";
            txtPrenom.Text = "";
            txtDateNaissance.Text = "";
            txtLieuNaissance.Text = "";
            ddlSexe.SelectedIndex = 0;
            ddlSituation.SelectedIndex = 0;
            txtDateRecrutement.Text = "";
            txtPecul.Text = "";
            txtVille.Text = "";
            txtPas.Text = "";
            ddlRetrait.SelectedIndex = 0;
            txtTypeRet.Text = "";
            txtPretLog.Text = "";
            txtBqv.Text = "";
            txtBqe.Text = "";
            txtCpt.Text = "";
            txtTel.Text = "";
            txtNational.Text = "";
            txtAdresse.Text = "";
            pnlMessage.Visible = false;
        }
    }
}