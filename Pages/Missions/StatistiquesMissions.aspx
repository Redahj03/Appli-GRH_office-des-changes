<%@ Page Title="Statistiques des Missions" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="StatistiquesMissions.aspx.cs" Inherits="GestionRHv2.Pages.Missions.StatistiquesMissions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <style>
        .stats-container {
            max-width: 1400px;
            margin: 0 auto;
            padding: 20px;
        }

        /* Header moderne */
        .stats-header {
            background: linear-gradient(135deg, #8e44ad 0%, #3498db 100%);
            padding: 30px;
            border-radius: 20px;
            color: white;
            margin-bottom: 30px;
            box-shadow: 0 10px 30px rgba(142, 68, 173, 0.3);
        }

        .header-content {
            display: flex;
            align-items: center;
            gap: 15px;
        }

        .header-icon {
            font-size: 2.5rem;
            opacity: 0.9;
        }

        .header-text h1 {
            margin: 0;
            font-size: 2rem;
            font-weight: 600;
        }

        .header-text p {
            margin: 5px 0 0 0;
            opacity: 0.9;
        }

        /* Filtres de période */
        .period-filter {
            background: white;
            padding: 25px;
            border-radius: 15px;
            box-shadow: 0 5px 20px rgba(0, 0, 0, 0.08);
            margin-bottom: 30px;
        }

        .filter-content {
            display: flex;
            gap: 20px;
            align-items: end;
            flex-wrap: wrap;
        }

        .filter-group {
            flex: 1;
            min-width: 200px;
        }

        .filter-group label {
            display: block;
            color: #7f8c8d;
            font-size: 0.9rem;
            margin-bottom: 8px;
            font-weight: 500;
        }

        .filter-control {
            width: 100%;
            padding: 10px 15px;
            border: 2px solid #e0e6ed;
            border-radius: 10px;
            font-size: 1rem;
            transition: all 0.3s ease;
            background: #f8f9fa;
        }

        .filter-control:focus {
            border-color: #3498db;
            background: white;
            outline: none;
            box-shadow: 0 0 0 3px rgba(52, 152, 219, 0.1);
        }

        .btn-filter {
            padding: 10px 30px;
            background: #3498db;
            color: white;
            border: none;
            border-radius: 10px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
        }

        .btn-filter:hover {
            background: #2980b9;
            transform: translateY(-2px);
            box-shadow: 0 5px 15px rgba(52, 152, 219, 0.3);
        }

        /* Cartes de statistiques principales */
        .main-stats-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 25px;
            margin-bottom: 40px;
        }

        .stat-card {
            background: white;
            padding: 30px;
            border-radius: 15px;
            box-shadow: 0 5px 20px rgba(0, 0, 0, 0.08);
            transition: all 0.3s ease;
            position: relative;
            overflow: hidden;
        }

        .stat-card::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 5px;
        }

        .stat-card.primary::before { background: linear-gradient(90deg, #3498db, #2980b9); }
        .stat-card.success::before { background: linear-gradient(90deg, #27ae60, #229954); }
        .stat-card.warning::before { background: linear-gradient(90deg, #f39c12, #e67e22); }
        .stat-card.danger::before { background: linear-gradient(90deg, #e74c3c, #c0392b); }
        .stat-card.info::before { background: linear-gradient(90deg, #8e44ad, #7b3a9d); }

        .stat-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.12);
        }

        .stat-icon {
            width: 60px;
            height: 60px;
            border-radius: 15px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.8rem;
            margin-bottom: 20px;
        }

        .stat-card.primary .stat-icon { background: rgba(52, 152, 219, 0.1); color: #3498db; }
        .stat-card.success .stat-icon { background: rgba(39, 174, 96, 0.1); color: #27ae60; }
        .stat-card.warning .stat-icon { background: rgba(243, 156, 18, 0.1); color: #f39c12; }
        .stat-card.danger .stat-icon { background: rgba(231, 76, 60, 0.1); color: #e74c3c; }
        .stat-card.info .stat-icon { background: rgba(142, 68, 173, 0.1); color: #8e44ad; }

        .stat-value {
            font-size: 2.5rem;
            font-weight: 700;
            color: #2c3e50;
            margin-bottom: 10px;
        }

        .stat-label {
            color: #7f8c8d;
            font-size: 1rem;
            margin-bottom: 15px;
        }

        .stat-trend {
            display: flex;
            align-items: center;
            gap: 5px;
            font-size: 0.9rem;
            color: #27ae60;
        }

        .stat-trend.down {
            color: #e74c3c;
        }

        /* Graphiques */
        .charts-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(500px, 1fr));
            gap: 30px;
            margin-bottom: 40px;
        }

        .chart-card {
            background: white;
            padding: 25px;
            border-radius: 15px;
            box-shadow: 0 5px 20px rgba(0, 0, 0, 0.08);
        }

        .chart-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
            padding-bottom: 15px;
            border-bottom: 2px solid #f0f2f5;
        }

        .chart-title {
            font-size: 1.2rem;
            color: #2c3e50;
            font-weight: 600;
        }

        .chart-container {
            position: relative;
            height: 300px;
        }

        /* Tableau détaillé */
        .detail-table-card {
            background: white;
            border-radius: 15px;
            box-shadow: 0 5px 20px rgba(0, 0, 0, 0.08);
            overflow: hidden;
        }

        .table-header {
            background: #f8f9fa;
            padding: 20px 25px;
            border-bottom: 2px solid #e9ecef;
        }

        .table-title {
            font-size: 1.2rem;
            color: #2c3e50;
            font-weight: 600;
        }

        .table-responsive {
            overflow-x: auto;
        }

        .stats-table {
            width: 100%;
            border-collapse: collapse;
        }

        .stats-table th {
            background: #f8f9fa;
            padding: 15px;
            text-align: left;
            font-weight: 600;
            color: #7f8c8d;
            font-size: 0.9rem;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            border-bottom: 2px solid #e0e6ed;
        }

        .stats-table td {
            padding: 15px;
            border-bottom: 1px solid #f1f3f5;
            color: #2c3e50;
        }

        .stats-table tr:hover {
            background: #f8f9fa;
        }

        /* Indicateurs de performance */
        .kpi-section {
            background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
            padding: 30px;
            border-radius: 20px;
            margin-bottom: 40px;
        }

        .kpi-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 20px;
        }

        .kpi-item {
            background: white;
            padding: 20px;
            border-radius: 10px;
            text-align: center;
            box-shadow: 0 3px 10px rgba(0, 0, 0, 0.05);
        }

        .kpi-value {
            font-size: 2rem;
            font-weight: 700;
            color: #3498db;
            margin-bottom: 5px;
        }

        .kpi-label {
            color: #7f8c8d;
            font-size: 0.9rem;
        }

        /* Responsive */
        @media (max-width: 768px) {
            .charts-grid {
                grid-template-columns: 1fr;
            }

            .filter-content {
                flex-direction: column;
            }

            .filter-group {
                width: 100%;
            }
        }

        /* Export button */
        .export-section {
            display: flex;
            justify-content: flex-end;
            gap: 10px;
            margin-bottom: 20px;
        }

        .btn-export {
            padding: 10px 20px;
            border: none;
            border-radius: 10px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
            display: inline-flex;
            align-items: center;
            gap: 8px;
            text-decoration: none;
        }

        .btn-export-excel {
            background: #27ae60;
            color: white;
        }

        .btn-export-excel:hover {
            background: #229954;
            transform: translateY(-2px);
        }

        .btn-export-pdf {
            background: #e74c3c;
            color: white;
        }

        .btn-export-pdf:hover {
            background: #c0392b;
            transform: translateY(-2px);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="stats-container">
        <!-- Header -->
        <div class="stats-header">
            <div class="header-content">
                <i class="fas fa-chart-pie header-icon"></i>
                <div class="header-text">
                    <h1>Statistiques des Missions</h1>
                    <p>Tableau de bord et analyses des ordres de mission</p>
                </div>
            </div>
        </div>

        <!-- Filtres de période -->
        <div class="period-filter">
            <div class="filter-content">
                <div class="filter-group">
                    <label>Date début</label>
                    <asp:TextBox ID="txtDateDebut" runat="server" CssClass="filter-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="filter-group">
                    <label>Date fin</label>
                    <asp:TextBox ID="txtDateFin" runat="server" CssClass="filter-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="filter-group">
                    <label>Service</label>
                    <asp:DropDownList ID="ddlService" runat="server" CssClass="filter-control">
                        <asp:ListItem Text="Tous les services" Value=""></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div>
                    <asp:Button ID="btnFiltrer" runat="server" Text="Appliquer" CssClass="btn-filter" OnClick="btnFiltrer_Click" />
                </div>
            </div>
        </div>

        <!-- Statistiques principales -->
        <div class="main-stats-grid">
            <div class="stat-card primary">
                <div class="stat-icon">
                    <i class="fas fa-clipboard-list"></i>
                </div>
                <div class="stat-value">
                    <asp:Label ID="lblTotalMissions" runat="server" Text="0"></asp:Label>
                </div>
                <div class="stat-label">Total des missions</div>
                <div class="stat-trend">
                    <i class="fas fa-arrow-up"></i>
                    <span>+12% ce mois</span>
                </div>
            </div>

            <div class="stat-card success">
                <div class="stat-icon">
                    <i class="fas fa-users"></i>
                </div>
                <div class="stat-value">
                    <asp:Label ID="lblAgentsEnMission" runat="server" Text="0"></asp:Label>
                </div>
                <div class="stat-label">Agents en mission</div>
                <div class="stat-trend">
                    <i class="fas fa-arrow-up"></i>
                    <span>+5% ce mois</span>
                </div>
            </div>

            <div class="stat-card warning">
                <div class="stat-icon">
                    <i class="fas fa-route"></i>
                </div>
                <div class="stat-value">
                    <asp:Label ID="lblKmParcourus" runat="server" Text="0"></asp:Label>
                </div>
                <div class="stat-label">Km parcourus</div>
                <div class="stat-trend down">
                    <i class="fas fa-arrow-down"></i>
                    <span>-8% ce mois</span>
                </div>
            </div>

            <div class="stat-card danger">
                <div class="stat-icon">
                    <i class="fas fa-coins"></i>
                </div>
                <div class="stat-value">
                    <asp:Label ID="lblCoutTotal" runat="server" Text="0"></asp:Label>
                </div>
                <div class="stat-label">Coût total (DH)</div>
                <div class="stat-trend">
                    <i class="fas fa-arrow-up"></i>
                    <span>+3% ce mois</span>
                </div>
            </div>

            <div class="stat-card info">
                <div class="stat-icon">
                    <i class="fas fa-calendar-day"></i>
                </div>
                <div class="stat-value">
                    <asp:Label ID="lblDureeMoyenne" runat="server" Text="0"></asp:Label>
                </div>
                <div class="stat-label">Durée moyenne (jours)</div>
                <div class="stat-trend">
                    <i class="fas fa-minus"></i>
                    <span>Stable</span>
                </div>
            </div>
        </div>

        <!-- KPIs -->
        <div class="kpi-section">
            <h3 style="margin-bottom: 20px; color: #2c3e50;">Indicateurs de Performance</h3>
            <div class="kpi-grid">
                <div class="kpi-item">
                    <div class="kpi-value"><asp:Label ID="lblTauxValidation" runat="server" Text="0"></asp:Label>%</div>
                    <div class="kpi-label">Taux de validation</div>
                </div>
                <div class="kpi-item">
                    <div class="kpi-value"><asp:Label ID="lblDelaiMoyen" runat="server" Text="0"></asp:Label>h</div>
                    <div class="kpi-label">Délai moyen validation</div>
                </div>
                <div class="kpi-item">
                    <div class="kpi-value"><asp:Label ID="lblMissionsUrgentes" runat="server" Text="0"></asp:Label></div>
                    <div class="kpi-label">Missions urgentes</div>
                </div>
                <div class="kpi-item">
                    <div class="kpi-value"><asp:Label ID="lblTopDestination" runat="server" Text="-"></asp:Label></div>
                    <div class="kpi-label">Top destination</div>
                </div>
            </div>
        </div>

        <!-- Graphiques -->
        <div class="charts-grid">
            <!-- Graphique évolution mensuelle -->
            <div class="chart-card">
                <div class="chart-header">
                    <h3 class="chart-title">Évolution mensuelle des missions</h3>
                    <select class="filter-control" style="width: auto;" onchange="updateChart('monthly')">
                        <option>Cette année</option>
                        <option>Année dernière</option>
                    </select>
                </div>
                <div class="chart-container">
                    <canvas id="monthlyChart"></canvas>
                </div>
            </div>

            <!-- Graphique par statut -->
            <div class="chart-card">
                <div class="chart-header">
                    <h3 class="chart-title">Répartition par statut</h3>
                </div>
                <div class="chart-container">
                    <canvas id="statusChart"></canvas>
                </div>
            </div>

            <!-- Graphique par destination -->
            <div class="chart-card">
                <div class="chart-header">
                    <h3 class="chart-title">Top 10 destinations</h3>
                </div>
                <div class="chart-container">
                    <canvas id="destinationChart"></canvas>
                </div>
            </div>

            <!-- Graphique par moyen de transport -->
            <div class="chart-card">
                <div class="chart-header">
                    <h3 class="chart-title">Moyens de transport utilisés</h3>
                </div>
                <div class="chart-container">
                    <canvas id="transportChart"></canvas>
                </div>
            </div>
        </div>

        <!-- Export buttons -->
        <div class="export-section">
            <asp:Button ID="btnExportExcel" runat="server" Text="Exporter Excel" 
                CssClass="btn-export btn-export-excel" OnClick="btnExportExcel_Click" />
            <asp:Button ID="btnExportPDF" runat="server" Text="Exporter PDF" 
                CssClass="btn-export btn-export-pdf" OnClick="btnExportPDF_Click" />
        </div>

        <!-- Tableau détaillé -->
        <div class="detail-table-card">
            <div class="table-header">
                <h3 class="table-title">Détail par service</h3>
            </div>
            <div class="table-responsive">
                <asp:GridView ID="gvDetailService" runat="server" CssClass="stats-table" 
                    AutoGenerateColumns="False" ShowHeaderWhenEmpty="True">
                    <Columns>
                        <asp:BoundField DataField="Service" HeaderText="Service" />
                        <asp:BoundField DataField="NombreMissions" HeaderText="Nombre de missions" />
                        <asp:BoundField DataField="NombreAgents" HeaderText="Agents concernés" />
                        <asp:BoundField DataField="TotalKm" HeaderText="Total Km" />
                        <asp:BoundField DataField="CoutTotal" HeaderText="Coût total (DH)" DataFormatString="{0:N2}" />
                        <asp:BoundField DataField="DureeMoyenne" HeaderText="Durée moyenne" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <script>
        // Configuration des graphiques
        Chart.defaults.font.family = '-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif';
        
        // Graphique évolution mensuelle
        var ctx1 = document.getElementById('monthlyChart').getContext('2d');
        var monthlyChart = new Chart(ctx1, {
            type: 'line',
            data: {
                labels: <%= GetMonthlyLabels() %>,
                datasets: [{
                    label: 'Nombre de missions',
                    data: <%= GetMonthlyData() %>,
                    borderColor: '#3498db',
                    backgroundColor: 'rgba(52, 152, 219, 0.1)',
                    tension: 0.4,
                    fill: true
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: false
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });

        // Graphique par statut
        var ctx2 = document.getElementById('statusChart').getContext('2d');
        var statusChart = new Chart(ctx2, {
            type: 'doughnut',
            data: {
                labels: <%= GetStatusLabels() %>,
                datasets: [{
                    data: <%= GetStatusData() %>,
                    backgroundColor: [
                        '#27ae60',
                        '#f39c12', 
                        '#e74c3c',
                        '#3498db'
                    ]
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'bottom'
                    }
                }
            }
        });

        // Graphique destinations
        var ctx3 = document.getElementById('destinationChart').getContext('2d');
        var destinationChart = new Chart(ctx3, {
            type: 'bar',
            data: {
                labels: <%= GetDestinationLabels() %>,
                datasets: [{
                    label: 'Nombre de missions',
                    data: <%= GetDestinationData() %>,
                    backgroundColor: '#8e44ad'
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                indexAxis: 'y',
                plugins: {
                    legend: {
                        display: false
                    }
                }
            }
        });

        // Graphique transport
        var ctx4 = document.getElementById('transportChart').getContext('2d');
        var transportChart = new Chart(ctx4, {
            type: 'pie',
            data: {
                labels: <%= GetTransportLabels() %>,
                datasets: [{
                    data: <%= GetTransportData() %>,
                    backgroundColor: [
                        '#3498db',
                        '#e74c3c',
                        '#f39c12',
                        '#27ae60'
                    ]
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'bottom'
                    }
                }
            }
        });
    </script>
</asp:Content>