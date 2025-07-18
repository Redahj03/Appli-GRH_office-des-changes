using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GestionRHv2.Classes;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Web;

namespace GestionRHv2.Pages.Missions
{
    public partial class ListeMissions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStatistics();
                LoadMissions();
            }
        }

        private void LoadStatistics()
        {
            try
            {
                DataTable stats = MissionManager.GetMissionStats();
                if (stats.Rows.Count > 0)
                {
                    DataRow row = stats.Rows[0];
                    lblTotalMissions.Text = GetValueOrDefault(row["TotalMissions"], "0");
                    lblTotalAgents.Text = GetValueOrDefault(row["TotalAgents"], "0");

                    decimal montantTotal = Convert.ToDecimal(GetValueOrDefault(row["MontantTotal"], "0"));
                    lblMontantTotal.Text = montantTotal.ToString("N0");

                    decimal kilometrage = Convert.ToDecimal(GetValueOrDefault(row["KilometrageTotal"], "0"));
                    lblKilometrage.Text = kilometrage.ToString("N0");

                    lblMissionsMois.Text = GetValueOrDefault(row["MissionsCeMois"], "0");
                }
            }
            catch (Exception ex)
            {
                ShowError("Erreur lors du chargement des statistiques : " + ex.Message);
            }
        }

        private void LoadMissions()
        {
            try
            {
                DataTable missions = MissionManager.GetAllMissions();
                gvMissions.DataSource = missions;
                gvMissions.DataBind();
            }
            catch (Exception ex)
            {
                ShowError("Erreur lors du chargement des missions : " + ex.Message);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = txtSearch.Text.Trim();
                DateTime? dateDebut = string.IsNullOrEmpty(txtDateDebut.Text) ? (DateTime?)null : DateTime.Parse(txtDateDebut.Text);
                DateTime? dateFin = string.IsNullOrEmpty(txtDateFin.Text) ? (DateTime?)null : DateTime.Parse(txtDateFin.Text);

                DataTable missions = MissionManager.SearchMissions(searchTerm, dateDebut, dateFin);
                gvMissions.DataSource = missions;
                gvMissions.DataBind();
            }
            catch (Exception ex)
            {
                ShowError("Erreur lors de la recherche : " + ex.Message);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            txtDateDebut.Text = "";
            txtDateFin.Text = "";
            LoadMissions();
        }

        protected void gvMissions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string[] args = e.CommandArgument.ToString().Split(';');
                string matricule = args[0];
                DateTime date = DateTime.Parse(args[1]);

                switch (e.CommandName)
                {
                    case "View":
                        Response.Redirect($"DetailsMission.aspx?mat={matricule}&date={date:yyyy-MM-dd}");
                        break;
                    case "Edit":
                        Response.Redirect($"AjouterMission.aspx?mat={matricule}&date={date:yyyy-MM-dd}");
                        break;
                    case "Delete":
                        int result = MissionManager.DeleteMission(matricule, date);
                        if (result > 0)
                        {
                            ShowSuccess("Mission supprimée avec succès.");
                            LoadStatistics();
                            LoadMissions();
                        }
                        else
                        {
                            ShowError("Erreur lors de la suppression de la mission.");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowError("Erreur : " + ex.Message);
            }
        }

        protected void gvMissions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Gérer l'affichage des valeurs null
                for (int i = 0; i < e.Row.Cells.Count - 1; i++) // -1 pour exclure la colonne Actions
                {
                    if (string.IsNullOrEmpty(e.Row.Cells[i].Text) || e.Row.Cells[i].Text == "&nbsp;")
                    {
                        e.Row.Cells[i].Text = "<span class='null-value'>Non renseigné</span>";
                    }
                }
            }
        }

        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                // Récupérer les données actuelles (avec ou sans filtre)
                DataTable missions;
                if (!string.IsNullOrEmpty(txtSearch.Text) || !string.IsNullOrEmpty(txtDateDebut.Text) || !string.IsNullOrEmpty(txtDateFin.Text))
                {
                    string searchTerm = txtSearch.Text.Trim();
                    DateTime? dateDebut = string.IsNullOrEmpty(txtDateDebut.Text) ? (DateTime?)null : DateTime.Parse(txtDateDebut.Text);
                    DateTime? dateFin = string.IsNullOrEmpty(txtDateFin.Text) ? (DateTime?)null : DateTime.Parse(txtDateFin.Text);
                    missions = MissionManager.SearchMissions(searchTerm, dateDebut, dateFin);
                }
                else
                {
                    missions = MissionManager.GetAllMissions();
                }

                // Créer le document PDF
                Document pdfDoc = new Document(PageSize.A4.Rotate(), 25, 25, 25, 25);
                MemoryStream ms = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, ms);

                pdfDoc.Open();

                // Titre
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                Paragraph title = new Paragraph("LISTE DES MISSIONS", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                pdfDoc.Add(title);

                // Date
                Font dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                Paragraph date = new Paragraph($"Généré le : {DateTime.Now:dd/MM/yyyy HH:mm}", dateFont);
                date.Alignment = Element.ALIGN_RIGHT;
                pdfDoc.Add(date);

                pdfDoc.Add(new Paragraph(" ")); // Espace

                // Créer le tableau
                PdfPTable table = new PdfPTable(10);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 10, 15, 10, 10, 10, 10, 8, 8, 8, 11 });

                // En-têtes
                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                string[] headers = { "Matricule", "Nom", "Grade", "Date", "Transport", "Montant", "Km", "Vacations", "Jours", "Taux/Jour" };

                foreach (string header in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header, headerFont));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Padding = 5;
                    table.AddCell(cell);
                }

                // Données
                Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                foreach (DataRow row in missions.Rows)
                {
                    table.AddCell(new PdfPCell(new Phrase(GetValueOrDefault(row["Matricule"], "-"), cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(GetValueOrDefault(row["Nom"], "-"), cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(GetValueOrDefault(row["Grade"], "-"), cellFont)));

                    string dateStr = row["Date_deplacement"] != DBNull.Value
                        ? Convert.ToDateTime(row["Date_deplacement"]).ToString("dd/MM/yyyy")
                        : "-";
                    table.AddCell(new PdfPCell(new Phrase(dateStr, cellFont)));

                    table.AddCell(new PdfPCell(new Phrase(GetValueOrDefault(row["Moyen_trans"], "-"), cellFont)));

                    string montant = row["Montant"] != DBNull.Value
                        ? Convert.ToDecimal(row["Montant"]).ToString("N2")
                        : "-";
                    table.AddCell(new PdfPCell(new Phrase(montant, cellFont)));

                    table.AddCell(new PdfPCell(new Phrase(GetValueOrDefault(row["Kilometrage"], "-"), cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(GetValueOrDefault(row["Nb_vacation"], "-"), cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(GetValueOrDefault(row["Nb_Jour"], "-"), cellFont)));

                    string tauxJour = row["Taux_Jour"] != DBNull.Value
                        ? Convert.ToDecimal(row["Taux_Jour"]).ToString("N2")
                        : "-";
                    table.AddCell(new PdfPCell(new Phrase(tauxJour, cellFont)));
                }

                pdfDoc.Add(table);

                // Statistiques
                pdfDoc.Add(new Paragraph(" "));
                Font statsFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                pdfDoc.Add(new Paragraph("STATISTIQUES", statsFont));

                Font statsContentFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                pdfDoc.Add(new Paragraph($"Total des missions : {lblTotalMissions.Text}", statsContentFont));
                pdfDoc.Add(new Paragraph($"Agents en mission : {lblTotalAgents.Text}", statsContentFont));
                pdfDoc.Add(new Paragraph($"Montant total : {lblMontantTotal.Text} DH", statsContentFont));
                pdfDoc.Add(new Paragraph($"Kilométrage total : {lblKilometrage.Text} Km", statsContentFont));

                pdfDoc.Close();

                // Télécharger le PDF
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", $"attachment;filename=Missions_{DateTime.Now:yyyyMMdd}.pdf");
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }
            catch (Exception ex)
            {
                ShowError("Erreur lors de l'export PDF : " + ex.Message);
            }
        }

        // Méthodes utilitaires
        protected string GetValue(object value)
        {
            if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return "<span class='null-value'>Non renseigné</span>";
            }
            return value.ToString();
        }

        private string GetValueOrDefault(object value, string defaultValue = "-")
        {
            if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return defaultValue;
            }
            return value.ToString();
        }

        protected string GetTransportClass(object transport)
        {
            if (transport == null || transport == DBNull.Value)
                return "transport-badge";

            string transportStr = transport.ToString().ToLower();
            if (transportStr.Contains("voiture"))
                return "transport-badge transport-voiture";
            else if (transportStr.Contains("train"))
                return "transport-badge transport-train";
            else if (transportStr.Contains("avion"))
                return "transport-badge transport-avion";
            else
                return "transport-badge";
        }

        private void ShowError(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error",
                $"alert('{message.Replace("'", "\\'")}')", true);
        }

        private void ShowSuccess(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "success",
                $"alert('{message.Replace("'", "\\'")}')", true);
        }
    }
}