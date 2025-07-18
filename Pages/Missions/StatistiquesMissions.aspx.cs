using GestionRHv2.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestionRHv2.Pages.Missions
{
    public partial class StatistiquesMissions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initialiser les dates par défaut (dernier mois)
                txtDateDebut.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                txtDateFin.Text = DateTime.Now.ToString("yyyy-MM-dd");

                LoadServices();
                LoadStatistics();
                LoadDetailsByService();
            }
        }

        private void LoadServices()
        {
            try
            {
                string query = "SELECT CODE, LIBELLE FROM LIEU_AFFECTATION ORDER BY LIBELLE";
                DataTable dt = Database.GetData(query);

                ddlService.DataSource = dt;
                ddlService.DataTextField = "LIBELLE";
                ddlService.DataValueField = "CODE";
                ddlService.DataBind();
                ddlService.Items.Insert(0, new ListItem("Tous les services", ""));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur LoadServices: {ex.Message}");
            }
        }

        private void LoadStatistics()
        {
            try
            {
                string dateDebut = txtDateDebut.Text;
                string dateFin = txtDateFin.Text;
                string service = ddlService.SelectedValue;

                // Total des missions
                string queryTotal = @"
                    SELECT COUNT(*) as Total
                    FROM ordre_miss om
                    LEFT JOIN Agent a ON om.matr = a.Matricule
                    WHERE om.date_d BETWEEN @DateDebut AND @DateFin";

                if (!string.IsNullOrEmpty(service))
                    queryTotal += " AND a.Affectation = @Service";

                DataTable dtTotal = Database.GetDataWithParams(queryTotal,
                    new SqlParameter("@DateDebut", dateDebut),
                    new SqlParameter("@DateFin", dateFin),
                    new SqlParameter("@Service", service));

                lblTotalMissions.Text = dtTotal.Rows.Count > 0 ? dtTotal.Rows[0]["Total"].ToString() : "0";

                // Agents en mission actuellement
                string queryAgentsEnMission = @"
                    SELECT COUNT(DISTINCT om.matr) as Total
                    FROM ordre_miss om
                    LEFT JOIN Agent a ON om.matr = a.Matricule
                    WHERE GETDATE() BETWEEN om.date_d AND om.date_r
                    AND om.Statut = 'En cours'";

                if (!string.IsNullOrEmpty(service))
                    queryAgentsEnMission += " AND a.Affectation = @Service";

                DataTable dtAgents = Database.GetDataWithParams(queryAgentsEnMission,
                    new SqlParameter("@Service", service));

                lblAgentsEnMission.Text = dtAgents.Rows.Count > 0 ? dtAgents.Rows[0]["Total"].ToString() : "0";

                // Kilomètres parcourus
                string queryKm = @"
                    SELECT ISNULL(SUM(m.Kilométrage), 0) as TotalKm
                    FROM Missions m
                    LEFT JOIN Agent a ON m.Matricule = a.Matricule
                    WHERE m.Date_deplacement BETWEEN @DateDebut AND @DateFin";

                if (!string.IsNullOrEmpty(service))
                    queryKm += " AND a.Affectation = @Service";

                DataTable dtKm = Database.GetDataWithParams(queryKm,
                    new SqlParameter("@DateDebut", dateDebut),
                    new SqlParameter("@DateFin", dateFin),
                    new SqlParameter("@Service", service));

                decimal totalKm = dtKm.Rows.Count > 0 && dtKm.Rows[0]["TotalKm"] != DBNull.Value
                    ? Convert.ToDecimal(dtKm.Rows[0]["TotalKm"]) : 0;
                lblKmParcourus.Text = totalKm.ToString("N0");

                // Coût total
                string queryCout = @"
                    SELECT ISNULL(SUM(m.Montant), 0) as TotalCout
                    FROM Missions m
                    LEFT JOIN Agent a ON m.Matricule = a.Matricule
                    WHERE m.Date_deplacement BETWEEN @DateDebut AND @DateFin";

                if (!string.IsNullOrEmpty(service))
                    queryCout += " AND a.Affectation = @Service";

                DataTable dtCout = Database.GetDataWithParams(queryCout,
                    new SqlParameter("@DateDebut", dateDebut),
                    new SqlParameter("@DateFin", dateFin),
                    new SqlParameter("@Service", service));

                decimal totalCout = dtCout.Rows.Count > 0 && dtCout.Rows[0]["TotalCout"] != DBNull.Value
                    ? Convert.ToDecimal(dtCout.Rows[0]["TotalCout"]) : 0;
                lblCoutTotal.Text = totalCout.ToString("N0");

                // Durée moyenne
                string queryDuree = @"
                    SELECT AVG(DATEDIFF(day, om.date_d, om.date_r) + 1) as DureeMoyenne
                    FROM ordre_miss om
                    LEFT JOIN Agent a ON om.matr = a.Matricule
                    WHERE om.date_d BETWEEN @DateDebut AND @DateFin";

                if (!string.IsNullOrEmpty(service))
                    queryDuree += " AND a.Affectation = @Service";

                DataTable dtDuree = Database.GetDataWithParams(queryDuree,
                    new SqlParameter("@DateDebut", dateDebut),
                    new SqlParameter("@DateFin", dateFin),
                    new SqlParameter("@Service", service));

                decimal dureeMoyenne = dtDuree.Rows.Count > 0 && dtDuree.Rows[0]["DureeMoyenne"] != DBNull.Value
                    ? Convert.ToDecimal(dtDuree.Rows[0]["DureeMoyenne"]) : 0;
                lblDureeMoyenne.Text = dureeMoyenne.ToString("0.#");

                // KPIs
                LoadKPIs(dateDebut, dateFin, service);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur LoadStatistics: {ex.Message}");
            }
        }

        private void LoadKPIs(string dateDebut, string dateFin, string service)
        {
            try
            {
                // Taux de validation
                string queryTaux = @"
                    SELECT 
                        COUNT(*) as Total,
                        SUM(CASE WHEN Statut = 'Validée' THEN 1 ELSE 0 END) as Validees
                    FROM ordre_miss om
                    LEFT JOIN Agent a ON om.matr = a.Matricule
                    WHERE om.date_d BETWEEN @DateDebut AND @DateFin";

                if (!string.IsNullOrEmpty(service))
                    queryTaux += " AND a.Affectation = @Service";

                DataTable dtTaux = Database.GetDataWithParams(queryTaux,
                    new SqlParameter("@DateDebut", dateDebut),
                    new SqlParameter("@DateFin", dateFin),
                    new SqlParameter("@Service", service));

                if (dtTaux.Rows.Count > 0)
                {
                    int total = Convert.ToInt32(dtTaux.Rows[0]["Total"]);
                    int validees = Convert.ToInt32(dtTaux.Rows[0]["Validees"]);
                    decimal taux = total > 0 ? (validees * 100.0m / total) : 0;
                    lblTauxValidation.Text = taux.ToString("0");
                }

                // Délai moyen de validation (en heures)
                lblDelaiMoyen.Text = "24"; // À implémenter avec la table MissionValidation

                // Missions urgentes (départ dans les 3 prochains jours)
                string queryUrgentes = @"
                    SELECT COUNT(*) as Total
                    FROM ordre_miss om
                    LEFT JOIN Agent a ON om.matr = a.Matricule
                    WHERE om.date_d BETWEEN GETDATE() AND DATEADD(day, 3, GETDATE())
                    AND om.Statut = 'En attente'";

                if (!string.IsNullOrEmpty(service))
                    queryUrgentes += " AND a.Affectation = @Service";

                DataTable dtUrgentes = Database.GetDataWithParams(queryUrgentes,
                    new SqlParameter("@Service", service));

                lblMissionsUrgentes.Text = dtUrgentes.Rows.Count > 0 ? dtUrgentes.Rows[0]["Total"].ToString() : "0";

                // Top destination
                string queryTopDest = @"
                    SELECT TOP 1 ville, COUNT(*) as Nombre
                    FROM ordre_miss om
                    LEFT JOIN Agent a ON om.matr = a.Matricule
                    WHERE om.date_d BETWEEN @DateDebut AND @DateFin";

                if (!string.IsNullOrEmpty(service))
                    queryTopDest += " AND a.Affectation = @Service";

                queryTopDest += " GROUP BY ville ORDER BY COUNT(*) DESC";

                DataTable dtTopDest = Database.GetDataWithParams(queryTopDest,
                    new SqlParameter("@DateDebut", dateDebut),
                    new SqlParameter("@DateFin", dateFin),
                    new SqlParameter("@Service", service));

                lblTopDestination.Text = dtTopDest.Rows.Count > 0 ? dtTopDest.Rows[0]["ville"].ToString() : "-";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur LoadKPIs: {ex.Message}");
            }
        }

        private void LoadDetailsByService()
        {
            try
            {
                string dateDebut = txtDateDebut.Text;
                string dateFin = txtDateFin.Text;

                string query = @"
                    SELECT 
                        la.LIBELLE as Service,
                        COUNT(DISTINCT om.ID) as NombreMissions,
                        COUNT(DISTINCT om.matr) as NombreAgents,
                        ISNULL(SUM(m.Kilométrage), 0) as TotalKm,
                        ISNULL(SUM(m.Montant), 0) as CoutTotal,
                        AVG(DATEDIFF(day, om.date_d, om.date_r) + 1) as DureeMoyenne
                    FROM LIEU_AFFECTATION la
                    LEFT JOIN Agent a ON la.CODE = a.Affectation
                    LEFT JOIN ordre_miss om ON a.Matricule = om.matr 
                        AND om.date_d BETWEEN @DateDebut AND @DateFin
                    LEFT JOIN Missions m ON om.matr = m.Matricule 
                        AND CAST(om.date_d as DATE) = CAST(m.Date_deplacement as DATE)
                    GROUP BY la.CODE, la.LIBELLE
                    HAVING COUNT(om.ID) > 0
                    ORDER BY COUNT(om.ID) DESC";

                DataTable dt = Database.GetDataWithParams(query,
                    new SqlParameter("@DateDebut", dateDebut),
                    new SqlParameter("@DateFin", dateFin));

                gvDetailService.DataSource = dt;
                gvDetailService.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur LoadDetailsByService: {ex.Message}");
            }
        }

        protected void btnFiltrer_Click(object sender, EventArgs e)
        {
            LoadStatistics();
            LoadDetailsByService();
        }

        // Méthodes pour les données des graphiques
        public string GetMonthlyLabels()
        {
            string[] mois = { "Jan", "Fév", "Mar", "Avr", "Mai", "Jun", "Jul", "Août", "Sep", "Oct", "Nov", "Déc" };
            return "['" + string.Join("','", mois) + "']";
        }

        public string GetMonthlyData()
        {
            try
            {
                int year = DateTime.Now.Year;
                var data = new int[12];

                for (int i = 1; i <= 12; i++)
                {
                    string query = @"
                        SELECT COUNT(*) as Total
                        FROM ordre_miss
                        WHERE YEAR(date_d) = @Year AND MONTH(date_d) = @Month";

                    DataTable dt = Database.GetDataWithParams(query,
                        new SqlParameter("@Year", year),
                        new SqlParameter("@Month", i));

                    data[i - 1] = dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["Total"]) : 0;
                }

                return "[" + string.Join(",", data) + "]";
            }
            catch
            {
                return "[0,0,0,0,0,0,0,0,0,0,0,0]";
            }
        }

        public string GetStatusLabels()
        {
            return "['Validée', 'En attente', 'Rejetée', 'En cours']";
        }

        public string GetStatusData()
        {
            try
            {
                string query = @"
                    SELECT 
                        SUM(CASE WHEN Statut = 'Validée' THEN 1 ELSE 0 END) as Validee,
                        SUM(CASE WHEN Statut = 'En attente' THEN 1 ELSE 0 END) as EnAttente,
                        SUM(CASE WHEN Statut = 'Rejetée' THEN 1 ELSE 0 END) as Rejetee,
                        SUM(CASE WHEN Statut = 'En cours' THEN 1 ELSE 0 END) as EnCours
                    FROM ordre_miss
                    WHERE date_d BETWEEN @DateDebut AND @DateFin";

                DataTable dt = Database.GetDataWithParams(query,
                    new SqlParameter("@DateDebut", txtDateDebut.Text),
                    new SqlParameter("@DateFin", txtDateFin.Text));

                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    return $"[{row["Validee"]},{row["EnAttente"]},{row["Rejetee"]},{row["EnCours"]}]";
                }
            }
            catch { }

            return "[0,0,0,0]";
        }

        public string GetDestinationLabels()
        {
            try
            {
                string query = @"
                    SELECT TOP 10 ville, COUNT(*) as Nombre
                    FROM ordre_miss
                    WHERE date_d BETWEEN @DateDebut AND @DateFin
                    GROUP BY ville
                    ORDER BY COUNT(*) DESC";

                DataTable dt = Database.GetDataWithParams(query,
                    new SqlParameter("@DateDebut", txtDateDebut.Text),
                    new SqlParameter("@DateFin", txtDateFin.Text));

                var labels = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    labels.Add(row["ville"].ToString());
                }

                return "['" + string.Join("','", labels) + "']";
            }
            catch
            {
                return "[]";
            }
        }

        public string GetDestinationData()
        {
            try
            {
                string query = @"
                    SELECT TOP 10 ville, COUNT(*) as Nombre
                    FROM ordre_miss
                    WHERE date_d BETWEEN @DateDebut AND @DateFin
                    GROUP BY ville
                    ORDER BY COUNT(*) DESC";

                DataTable dt = Database.GetDataWithParams(query,
                    new SqlParameter("@DateDebut", txtDateDebut.Text),
                    new SqlParameter("@DateFin", txtDateFin.Text));

                var data = new List<int>();
                foreach (DataRow row in dt.Rows)
                {
                    data.Add(Convert.ToInt32(row["Nombre"]));
                }

                return "[" + string.Join(",", data) + "]";
            }
            catch
            {
                return "[]";
            }
        }

        public string GetTransportLabels()
        {
            return "['Voiture de service', 'Voiture personnelle', 'Train', 'Avion']";
        }

        public string GetTransportData()
        {
            try
            {
                string query = @"
                    SELECT 
                        SUM(CASE WHEN trpt = 'Voiture de service' THEN 1 ELSE 0 END) as VoitureService,
                        SUM(CASE WHEN trpt = 'Voiture personnelle' THEN 1 ELSE 0 END) as VoiturePerso,
                        SUM(CASE WHEN trpt = 'Train' THEN 1 ELSE 0 END) as Train,
                        SUM(CASE WHEN trpt = 'Avion' THEN 1 ELSE 0 END) as Avion
                    FROM ordre_miss
                    WHERE date_d BETWEEN @DateDebut AND @DateFin";

                DataTable dt = Database.GetDataWithParams(query,
                    new SqlParameter("@DateDebut", txtDateDebut.Text),
                    new SqlParameter("@DateFin", txtDateFin.Text));

                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    return $"[{row["VoitureService"]},{row["VoiturePerso"]},{row["Train"]},{row["Avion"]}]";
                }
            }
            catch { }

            return "[0,0,0,0]";
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            // Export Excel simple
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=StatistiquesMissions_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            // Désactiver la vérification des événements
            gvDetailService.AllowPaging = false;
            LoadDetailsByService();

            gvDetailService.RenderControl(hw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            // Pour un export PDF, vous devriez utiliser une bibliothèque comme iTextSharp
            ClientScript.RegisterStartupScript(this.GetType(), "alert",
                "alert('Export PDF non implémenté. Utilisez l\\'export Excel.');", true);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Nécessaire pour l'export Excel
        }
    }
}