using System;
using System.Data;
using GestionRHv2.Classes;

namespace GestionRHv2.Pages.Sanctions
{
    public partial class ModifierSanction : System.Web.UI.Page
    {
        private int CodeSC
        {
            get { return ViewState["CodeSC"] != null ? (int)ViewState["CodeSC"] : 0; }
            set { ViewState["CodeSC"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["code"] != null)
                {
                    int code = Convert.ToInt32(Request.QueryString["code"]);
                    CodeSC = code;
                    LoadSanction(code);
                }
                else
                {
                    Response.Redirect("ListeSanctions.aspx");
                }
            }
        }

        private void LoadSanction(int code)
        {
            try
            {
                DataTable dt = SanctionManager.GetTypeSanctionByCode(code);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtCodeSC.Text = row["code_SC"].ToString();
                    txtLibelle.Text = row["libelle_sc"].ToString();
                    txtJours.Text = row["jours"].ToString();
                }
                else
                {
                    Response.Redirect("ListeSanctions.aspx");
                }
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
                string libelle = txtLibelle.Text.Trim();
                int jours = string.IsNullOrEmpty(txtJours.Text) ? 0 : Convert.ToInt32(txtJours.Text.Trim());

                bool success = SanctionManager.ModifierTypeSanction(CodeSC, libelle, jours);

                if (success)
                {
                    Response.Redirect("ListeSanctions.aspx?success=edit");
                }
            }
            catch (Exception ex)
            {
                // Gérer l'erreur
            }
        }
    }
}