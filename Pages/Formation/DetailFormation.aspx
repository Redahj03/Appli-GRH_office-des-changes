<%@ Page Title="Détail de la Formation" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="DetailFormation.aspx.cs" Inherits="GestionRHv2.Pages.Formation.DetailFormation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
        }
        
        .page-header {
            background: white;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            margin-bottom: 30px;
        }
        
        .page-title {
            font-size: 2rem;
            color: #2c3e50;
            margin: 0 0 10px 0;
            display: flex;
            align-items: center;
            gap: 10px;
        }
        
        .breadcrumb {
            display: flex;
            gap: 10px;
            color: #7f8c8d;
            font-size: 14px;
        }
        
        .breadcrumb a {
            color: #3498db;
            text-decoration: none;
        }
        
        .breadcrumb a:hover {
            text-decoration: underline;
        }
        
        /* Grille principale */
        .detail-grid {
            display: grid;
            grid-template-columns: 2fr 1fr;
            gap: 30px;
            margin-bottom: 30px;
        }
        
        /* Cards */
        .card {
            background: white;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            overflow: hidden;
        }
        
        .card-header {
            background: #f8f9fa;
            padding: 20px;
            border-bottom: 1px solid #e9ecef;
        }
        
        .card-title {
            font-size: 1.2rem;
            color: #2c3e50;
            margin: 0;
            display: flex;
            align-items: center;
            gap: 10px;
        }
        
        .card-body {
            padding: 25px;
        }
        
        /* Info items */
        .info-item {
            display: flex;
            justify-content: space-between;
            padding: 15px 0;
            border-bottom: 1px solid #f0f0f0;
        }
        
        .info-item:last-child {
            border-bottom: none;
        }
        
        .info-label {
            color: #7f8c8d;
            font-weight: 600;
            font-size: 14px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }
        
        .info-value {
            color: #2c3e50;
            font-weight: 500;
            text-align: right;
        }
        
        /* Stats cards */
        .stats-grid {
            display: grid;
            grid-template-columns: repeat(2, 1fr);
            gap: 15px;
            margin-bottom: 20px;
        }
        
        .stat-card {
            background: #f8f9fa;
            padding: 20px;
            border-radius: 8px;
            text-align: center;
            border: 1px solid #e9ecef;
        }
        
        .stat-value {
            font-size: 2rem;
            font-weight: bold;
            color: #2c3e50;
            margin-bottom: 5px;
        }
        
        .stat-label {
            color: #7f8c8d;
            font-size: 13px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }
        
        /* Statuts */
        .status-badge {
            padding: 6px 16px;
            border-radius: 20px;
            font-size: 14px;
            font-weight: 600;
            display: inline-block;
        }
        
        .status-terminee {
            background: #fee4e2;
            color: #d32f2f;
        }
        
        .status-en-cours {
            background: #fff3cd;
            color: #f57c00;
        }
        
        .status-a-venir {
            background: #e3f2fd;
            color: #1976d2;
        }
        
        /* Boutons d'action */
        .action-buttons {
            display: flex;
            gap: 10px;
            margin-top: 20px;
        }
        
        .btn {
            padding: 10px 20px;
            border: none;
            border-radius: 8px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            gap: 8px;
        }
        
        .btn-primary {
            background: #3498db;
            color: white;
        }
        
        .btn-primary:hover {
            background: #2980b9;
            color: white;
            text-decoration: none;
        }
        
        .btn-warning {
            background: #f39c12;
            color: white;
        }
        
        .btn-warning:hover {
            background: #e67e22;
            color: white;
            text-decoration: none;
        }
        
        .btn-danger {
            background: #e74c3c;
            color: white;
        }
        
        .btn-danger:hover {
            background: #c0392b;
            color: white;
            text-decoration: none;
        }
        
        .btn-success {
            background: #27ae60;
            color: white;
        }
        
        .btn-success:hover {
            background: #229954;
            color: white;
            text-decoration: none;
        }
        
        .btn-secondary {
            background: #95a5a6;
            color: white;
        }
        
        .btn-secondary:hover {
            background: #7f8c8d;
            color: white;
            text-decoration: none;
        }
        
        /* Liste des participants */
        .participants-list {
            max-height: 400px;
            overflow-y: auto;
        }
        
        .participant-item {
            display: flex;
            align-items: center;
            padding: 12px;
            border-bottom: 1px solid #f0f0f0;
            transition: background-color 0.2s;
        }
        
        .participant-item:hover {
            background: #f8f9fa;
        }
        
        .participant-item:last-child {
            border-bottom: none;
        }
        
        .participant-avatar {
            width: 40px;
            height: 40px;
            background: #e3f2fd;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            color: #1976d2;
            font-weight: bold;
            margin-right: 15px;
        }
        
        .participant-info {
            flex: 1;
        }
        
        .participant-name {
            font-weight: 600;
            color: #2c3e50;
            margin-bottom: 2px;
        }
        
        .participant-details {
            font-size: 13px;
            color: #7f8c8d;
        }
        
        /* État vide */
        .empty-state {
            text-align: center;
            padding: 40px;
            color: #95a5a6;
        }
        
        .empty-state i {
            font-size: 3rem;
            margin-bottom: 15px;
        }
        
        /* Contenu de la formation */
        .content-section {
            background: #f8f9fa;
            padding: 20px;
            border-radius: 8px;
            margin-top: 20px;
        }
        
        .content-text {
            color: #555;
            line-height: 1.6;
            white-space: pre-wrap;
        }
        
        /* Messages */
        .alert {
            padding: 15px 20px;
            border-radius: 8px;
            margin-bottom: 20px;
            display: flex;
            align-items: center;
            gap: 10px;
        }
        
        .alert-success {
            background: #d4edda;
            color: #155724;
            border: 1px solid #c3e6cb;
        }
        
        .alert-danger {
            background: #f8d7da;
            color: #721c24;
            border: 1px solid #f5c6cb;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <!-- En-tête -->
        <div class="page-header">
            <h1 class="page-title">
                <i class="fas fa-info-circle"></i> Détail de la Formation
            </h1>
            <div class="breadcrumb">
                <a href="ListeFormations.aspx">Formations</a>
                <span>/</span>
                <span>Détail</span>
            </div>
        </div>
        
        <!-- Messages -->
        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert">
            <i class="fas fa-info-circle"></i>
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </asp:Panel>
        
        <!-- Grille principale -->
        <div class="detail-grid">
            <!-- Informations principales -->
            <div class="card">
                <div class="card-header">
                    <h3 class="card-title">
                        <i class="fas fa-book"></i> Informations de la Formation
                    </h3>
                </div>
                <div class="card-body">
                    <div class="info-item">
                        <span class="info-label">N° Objet</span>
                        <span class="info-value">
                            <asp:Label ID="lblObjet" runat="server"></asp:Label>
                        </span>
                    </div>
                    <div class="info-item">
                        <span class="info-label">Date</span>
                        <span class="info-value">
                            <asp:Label ID="lblDate" runat="server"></asp:Label>
                        </span>
                    </div>
                    <div class="info-item">
                        <span class="info-label">Lieu</span>
                        <span class="info-value">
                            <asp:Label ID="lblLieu" runat="server"></asp:Label>
                        </span>
                    </div>
                    <div class="info-item">
                        <span class="info-label">Durée</span>
                        <span class="info-value">
                            <asp:Label ID="lblDuree" runat="server"></asp:Label>
                        </span>
                    </div>
                    <div class="info-item">
                        <span class="info-label">Effectif Maximum</span>
                        <span class="info-value">
                            <asp:Label ID="lblEffectif" runat="server"></asp:Label>
                        </span>
                    </div>
                    <div class="info-item">
                        <span class="info-label">Statut</span>
                        <span class="info-value">
                            <asp:Label ID="lblStatut" runat="server"></asp:Label>
                        </span>
                    </div>
                    
                    <!-- Contenu -->
                    <div class="content-section">
                        <h4 style="margin-bottom: 10px; color: #2c3e50;">
                            <i class="fas fa-file-alt"></i> Contenu de la formation
                        </h4>
                        <div class="content-text">
                            <asp:Label ID="lblContenu" runat="server"></asp:Label>
                        </div>
                    </div>
                    
                    <!-- Boutons d'action -->
                    <div class="action-buttons">
                        <asp:Button ID="btnModifier" runat="server" 
                                   Text="Modifier" 
                                   CssClass="btn btn-warning"
                                   OnClick="btnModifier_Click" />
                        
                        <asp:Button ID="btnGererInscrits" runat="server" 
                                   Text="Gérer les inscrits" 
                                   CssClass="btn btn-success"
                                   OnClick="btnGererInscrits_Click" />
                        
                        <asp:Button ID="btnSupprimer" runat="server" 
                                   Text="Supprimer" 
                                   CssClass="btn btn-danger"
                                   OnClick="btnSupprimer_Click"
                                   OnClientClick="return confirm('Êtes-vous sûr de vouloir supprimer cette formation ?');" />
                    </div>
                </div>
            </div>
            
            <!-- Statistiques et participants -->
            <div>
                <!-- Statistiques -->
                <div class="card" style="margin-bottom: 20px;">
                    <div class="card-header">
                        <h3 class="card-title">
                            <i class="fas fa-chart-bar"></i> Statistiques
                        </h3>
                    </div>
                    <div class="card-body">
                        <div class="stats-grid">
                            <div class="stat-card">
                                <div class="stat-value">
                                    <asp:Label ID="lblNbInscrits" runat="server" Text="0"></asp:Label>
                                </div>
                                <div class="stat-label">Inscrits</div>
                            </div>
                            <div class="stat-card">
                                <div class="stat-value">
                                    <asp:Label ID="lblPlacesRestantes" runat="server" Text="0"></asp:Label>
                                </div>
                                <div class="stat-label">Places Restantes</div>
                            </div>
                            <div class="stat-card">
                                <div class="stat-value">
                                    <asp:Label ID="lblTauxRemplissage" runat="server" Text="0%"></asp:Label>
                                </div>
                                <div class="stat-label">Taux de Remplissage</div>
                            </div>
                            <div class="stat-card">
                                <div class="stat-value">
                                    <asp:Label ID="lblJoursRestants" runat="server" Text="0"></asp:Label>
                                </div>
                                <div class="stat-label">Jours Restants</div>
                            </div>
                        </div>
                    </div>
                </div>
                
                <!-- Liste des participants -->
                <div class="card">
                    <div class="card-header">
                        <h3 class="card-title">
                            <i class="fas fa-users"></i> Participants Inscrits
                            <span style="font-size: 14px; color: #7f8c8d; margin-left: 10px;">
                                (<asp:Label ID="lblCountParticipants" runat="server" Text="0"></asp:Label>)
                            </span>
                        </h3>
                    </div>
                    <div class="card-body">
                        <div class="participants-list">
                            <asp:Repeater ID="rptParticipants" runat="server">
                                <ItemTemplate>
                                    <div class="participant-item">
                                        <div class="participant-avatar">
    <%# GetInitials(
         Convert.ToString(Eval("nom")),
         Convert.ToString(Eval("prenom"))
       ) %>
</div>
                                        <div class="participant-info">
                                            <div class="participant-name">
                                                <%# Eval("nom") %> <%# Eval("prenom") %>
                                            </div>
                                            <div class="participant-details">
                                                Mat: <%# Eval("matricule") %> | 
                                                Tél: <%# Eval("tel") %>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            
                            <asp:Panel ID="pnlAucunParticipant" runat="server" Visible="false">
                                <div class="empty-state">
                                    <i class="fas fa-user-slash"></i>
                                    <p>Aucun participant inscrit</p>
                                </div>
                            </asp:Panel>
                        </div>
                        
                        <asp:Button ID="btnExporter" runat="server" 
                                   Text="Exporter la liste" 
                                   CssClass="btn btn-primary" 
                                   OnClick="btnExporter_Click"
                                   style="width: 100%; margin-top: 15px;" />
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Bouton retour -->
        <div style="text-align: center;">
            <asp:Button ID="btnRetour" runat="server" 
                       Text="Retour à la liste" 
                       CssClass="btn btn-secondary"
                       OnClick="btnRetour_Click" />
        </div>
    </div>
</asp:Content>