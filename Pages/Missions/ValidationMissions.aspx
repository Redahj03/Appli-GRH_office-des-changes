<%@ Page Title="Validation des Missions" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ValidationMissions.aspx.cs" Inherits="GestionRHv2.Pages.Missions.ValidationMissions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .validation-container {
            max-width: 1400px;
            margin: 0 auto;
            padding: 20px;
        }

        /* Header moderne */
        .page-header {
            background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
            padding: 30px;
            border-radius: 20px;
            color: white;
            margin-bottom: 30px;
            box-shadow: 0 10px 30px rgba(243, 156, 18, 0.3);
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

        /* Cartes de missions */
        .missions-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(400px, 1fr));
            gap: 25px;
            margin-bottom: 30px;
        }

        .mission-card {
            background: white;
            border-radius: 15px;
            box-shadow: 0 5px 20px rgba(0, 0, 0, 0.08);
            overflow: hidden;
            transition: all 0.3s ease;
            position: relative;
        }

        .mission-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.12);
        }

        .mission-card-header {
            background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
            color: white;
            padding: 20px;
            position: relative;
        }

        .mission-card-header.urgent {
            background: linear-gradient(135deg, #e74c3c 0%, #c0392b 100%);
        }

        .mission-number {
            font-size: 1.5rem;
            font-weight: 700;
            margin-bottom: 5px;
        }

        .mission-agent {
            font-size: 1.1rem;
            opacity: 0.95;
        }

        .mission-badge {
            position: absolute;
            top: 20px;
            right: 20px;
            background: rgba(255, 255, 255, 0.2);
            padding: 5px 15px;
            border-radius: 20px;
            font-size: 0.85rem;
            font-weight: 600;
        }

        .mission-card-body {
            padding: 25px;
        }

        .mission-info {
            margin-bottom: 20px;
        }

        .info-row {
            display: flex;
            align-items: center;
            gap: 10px;
            margin-bottom: 12px;
            color: #2c3e50;
        }

        .info-row i {
            color: #3498db;
            width: 20px;
            text-align: center;
        }

        .info-label {
            font-weight: 600;
            margin-right: 5px;
        }

        .mission-objet {
            background: #f8f9fa;
            padding: 15px;
            border-radius: 10px;
            margin: 20px 0;
            border-left: 4px solid #3498db;
        }

        .mission-objet h4 {
            margin: 0 0 10px 0;
            color: #2c3e50;
            font-size: 0.9rem;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }

        /* Section validation */
        .validation-section {
            border-top: 2px solid #f0f2f5;
            padding-top: 20px;
            margin-top: 20px;
        }

        .validation-buttons {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 10px;
            margin-bottom: 15px;
        }

        .btn-validate {
            padding: 12px 20px;
            border: none;
            border-radius: 10px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
        }

        .btn-approve {
            background: #27ae60;
            color: white;
        }

        .btn-approve:hover {
            background: #229954;
            transform: scale(1.05);
            box-shadow: 0 5px 15px rgba(39, 174, 96, 0.3);
        }

        .btn-reject {
            background: #e74c3c;
            color: white;
        }

        .btn-reject:hover {
            background: #c0392b;
            transform: scale(1.05);
            box-shadow: 0 5px 15px rgba(231, 76, 60, 0.3);
        }

        .btn-details {
            background: #f8f9fa;
            color: #3498db;
            border: 2px solid #3498db;
            width: 100%;
        }

        .btn-details:hover {
            background: #3498db;
            color: white;
        }

        /* Commentaire de validation */
        .comment-section {
            margin-top: 15px;
        }

        .comment-box {
            width: 100%;
            padding: 10px;
            border: 2px solid #e0e6ed;
            border-radius: 10px;
            resize: vertical;
            min-height: 80px;
            font-family: inherit;
        }

        .comment-box:focus {
            border-color: #3498db;
            outline: none;
            box-shadow: 0 0 0 3px rgba(52, 152, 219, 0.1);
        }

        /* Filtres */
        .filter-section {
            background: white;
            padding: 25px;
            border-radius: 15px;
            box-shadow: 0 5px 20px rgba(0, 0, 0, 0.08);
            margin-bottom: 30px;
        }

        .filter-tabs {
            display: flex;
            gap: 10px;
            flex-wrap: wrap;
        }

        .filter-tab {
            padding: 10px 25px;
            border: 2px solid #e0e6ed;
            background: #f8f9fa;
            border-radius: 25px;
            cursor: pointer;
            transition: all 0.3s ease;
            font-weight: 600;
            color: #7f8c8d;
            text-decoration: none;
        }

        .filter-tab:hover {
            border-color: #3498db;
            color: #3498db;
            transform: translateY(-2px);
        }

        .filter-tab.active {
            background: #3498db;
            color: white;
            border-color: #3498db;
        }

        /* État vide */
        .empty-state {
            text-align: center;
            padding: 60px 20px;
            background: white;
            border-radius: 15px;
            box-shadow: 0 5px 20px rgba(0, 0, 0, 0.08);
        }

        .empty-state i {
            font-size: 4rem;
            color: #e0e6ed;
            margin-bottom: 20px;
        }

        .empty-state h3 {
            color: #2c3e50;
            margin-bottom: 10px;
        }

        /* Animation de chargement */
        .loading-overlay {
            position: fixed;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: rgba(255, 255, 255, 0.9);
            display: none;
            align-items: center;
            justify-content: center;
            z-index: 9999;
        }

        .loading-spinner {
            text-align: center;
        }

        .spinner {
            border: 4px solid #f3f3f3;
            border-top: 4px solid #3498db;
            border-radius: 50%;
            width: 50px;
            height: 50px;
            animation: spin 1s linear infinite;
            margin: 0 auto 20px;
        }

        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }

        /* Responsive */
        @media (max-width: 768px) {
            .missions-grid {
                grid-template-columns: 1fr;
            }

            .validation-buttons {
                grid-template-columns: 1fr;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="validation-container">
        <!-- Header -->
        <div class="page-header">
            <div class="header-content">
                <i class="fas fa-check-double header-icon"></i>
                <div class="header-text">
                    <h1>Validation des Missions</h1>
                    <p>Approuvez ou rejetez les ordres de mission en attente</p>
                </div>
            </div>
        </div>

        <!-- Filtres -->
        <div class="filter-section">
            <div class="filter-tabs">
                <asp:LinkButton ID="lnkTous" runat="server" CssClass="filter-tab active" 
                    OnClick="lnkTous_Click">
                    <i class="fas fa-list"></i> Toutes les missions
                </asp:LinkButton>
                <asp:LinkButton ID="lnkEnAttente" runat="server" CssClass="filter-tab" 
                    OnClick="lnkEnAttente_Click">
                    <i class="fas fa-clock"></i> En attente
                </asp:LinkButton>
                <asp:LinkButton ID="lnkUrgentes" runat="server" CssClass="filter-tab" 
                    OnClick="lnkUrgentes_Click">
                    <i class="fas fa-exclamation-triangle"></i> Urgentes
                </asp:LinkButton>
                <asp:LinkButton ID="lnkAujourdhui" runat="server" CssClass="filter-tab" 
                    OnClick="lnkAujourdhui_Click">
                    <i class="fas fa-calendar-day"></i> Départ aujourd'hui
                </asp:LinkButton>
            </div>
        </div>

        <!-- Grille des missions -->
        <div class="missions-grid" id="missionsGrid" runat="server">
            <asp:Repeater ID="rptMissions" runat="server">
                <ItemTemplate>
                    <div class="mission-card">
                        <div class='<%# GetHeaderClass(Eval("DateDepart")) %>'>
                            <div class="mission-number">Mission #<%# Eval("ID") %></div>
                            <div class="mission-agent"><%# Eval("NomAgent") %></div>
                            <div class="mission-badge"><%# GetUrgencyBadge(Eval("DateDepart")) %></div>
                        </div>
                        
                        <div class="mission-card-body">
                            <div class="mission-info">
                                <div class="info-row">
                                    <i class="fas fa-id-badge"></i>
                                    <span class="info-label">Matricule:</span>
                                    <span><%# Eval("Matricule") %></span>
                                </div>
                                
                                <div class="info-row">
                                    <i class="fas fa-map-marker-alt"></i>
                                    <span class="info-label">Destination:</span>
                                    <span><%# Eval("Ville") %></span>
                                </div>
                                
                                <div class="info-row">
                                    <i class="fas fa-calendar"></i>
                                    <span class="info-label">Période:</span>
                                    <span><%# FormatDateRange(Eval("DateDepart"), Eval("DateRetour")) %></span>
                                </div>
                                
                                <div class="info-row">
                                    <i class="fas fa-car"></i>
                                    <span class="info-label">Transport:</span>
                                    <span><%# Eval("Transport") %></span>
                                </div>
                            </div>
                            
                            <div class="mission-objet">
                                <h4>Objet de la mission</h4>
                                <p><%# Eval("ObjetMission") %></p>
                            </div>
                            
                            <div class="validation-section">
                                <div class="validation-buttons">
                                    <asp:Button ID="btnApprove" runat="server" Text="Approuver" 
                                        CssClass="btn-validate btn-approve"
                                        CommandName="Approve" CommandArgument='<%# Eval("ID") %>'
                                        OnClick="btnValidate_Click" />
                                    <asp:Button ID="btnReject" runat="server" Text="Rejeter" 
                                        CssClass="btn-validate btn-reject"
                                        CommandName="Reject" CommandArgument='<%# Eval("ID") %>'
                                        OnClick="btnValidate_Click" />
                                </div>
                                
                                <div class="comment-section">
                                    <asp:TextBox ID="txtComment" runat="server" 
                                        TextMode="MultiLine" CssClass="comment-box"
                                        placeholder="Ajouter un commentaire (optionnel)..."></asp:TextBox>
                                </div>
                                
                                <asp:Button ID="btnDetails" runat="server" Text="Voir les détails" 
                                    CssClass="btn-validate btn-details"
                                    CommandArgument='<%# Eval("ID") %>'
                                    OnClick="btnDetails_Click" />
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- État vide -->
        <asp:Panel ID="pnlEmptyState" runat="server" Visible="false" CssClass="empty-state">
            <i class="fas fa-check-circle"></i>
            <h3>Aucune mission en attente de validation</h3>
            <p>Toutes les missions ont été traitées.</p>
        </asp:Panel>
    </div>

    <!-- Overlay de chargement -->
    <div class="loading-overlay" id="loadingOverlay">
        <div class="loading-spinner">
            <div class="spinner"></div>
            <p>Traitement en cours...</p>
        </div>
    </div>

    <script type="text/javascript">
        function showLoading() {
            document.getElementById('loadingOverlay').style.display = 'flex';
        }

        function hideLoading() {
            document.getElementById('loadingOverlay').style.display = 'none';
        }

        // Afficher le chargement lors de la validation
        document.querySelectorAll('.btn-validate').forEach(function (btn) {
            btn.addEventListener('click', function () {
                if (confirm('Êtes-vous sûr de votre décision ?')) {
                    showLoading();
                    return true;
                }
                return false;
            });
        });
    </script>
</asp:Content>