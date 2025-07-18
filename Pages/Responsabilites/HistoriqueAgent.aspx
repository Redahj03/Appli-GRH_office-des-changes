<%@ Page Title="Historique des Responsabilités" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="HistoriqueAgent.aspx.cs" Inherits="GestionRHv2.Pages.Responsabilites.HistoriqueAgent" %>

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
            margin: 0;
            display: flex;
            align-items: center;
        }
        
        .page-title i {
            margin-right: 15px;
            color: #9b59b6;
        }
        
        .agent-info {
            background: #f8f9fa;
            padding: 20px;
            border-radius: 8px;
            margin-bottom: 30px;
            border-left: 4px solid #3498db;
        }
        
        .agent-info h3 {
            margin: 0 0 10px 0;
            color: #2c3e50;
        }
        
        .agent-info p {
            margin: 5px 0;
            color: #5a6c7d;
        }
        
        /* Recherche agent */
        .search-card {
            background: white;
            padding: 25px;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            margin-bottom: 30px;
        }
        
        .search-grid {
            display: grid;
            grid-template-columns: 1fr auto;
            gap: 20px;
            align-items: end;
        }
        
        .form-group {
            display: flex;
            flex-direction: column;
        }
        
        .form-label {
            font-weight: 600;
            color: #5a6c7d;
            margin-bottom: 8px;
        }
        
        .form-control {
            padding: 12px 15px;
            border: 2px solid #e0e0e0;
            border-radius: 6px;
            font-size: 16px;
            transition: border-color 0.3s;
        }
        
        .form-control:focus {
            outline: none;
            border-color: #9b59b6;
            box-shadow: 0 0 0 3px rgba(155, 89, 182, 0.1);
        }
        
        .btn-search {
            padding: 12px 30px;
            background: #9b59b6;
            color: white;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-weight: 600;
            font-size: 16px;
            transition: all 0.2s;
        }
        
        .btn-search:hover {
            background: #8e44ad;
            transform: translateY(-1px);
        }
        
        /* Timeline */
        .timeline {
            background: white;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            padding: 30px;
        }
        
        .timeline-item {
            position: relative;
            padding-left: 40px;
            padding-bottom: 30px;
            border-left: 3px solid #e0e0e0;
            margin-left: 20px;
        }
        
        .timeline-item:last-child {
            border-left: none;
            padding-bottom: 0;
        }
        
        .timeline-item::before {
            content: '';
            position: absolute;
            left: -9px;
            top: 0;
            width: 15px;
            height: 15px;
            border-radius: 50%;
            background: #3498db;
            border: 3px solid white;
            box-shadow: 0 2px 5px rgba(0,0,0,0.2);
        }
        
        .timeline-item.current::before {
            background: #27ae60;
        }
        
        .timeline-date {
            font-size: 0.9rem;
            color: #7f8c8d;
            margin-bottom: 5px;
        }
        
        .timeline-content {
            background: #f8f9fa;
            padding: 15px;
            border-radius: 8px;
            margin-bottom: 10px;
        }
        
        .timeline-title {
            font-weight: 600;
            color: #2c3e50;
            margin-bottom: 5px;
        }
        
        .timeline-details {
            font-size: 0.9rem;
            color: #5a6c7d;
        }
        
        .timeline-badge {
            display: inline-block;
            padding: 4px 10px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: 600;
            margin-left: 10px;
        }
        
        .badge-active {
            background: #d4edda;
            color: #155724;
        }
        
        .badge-inactive {
            background: #f8d7da;
            color: #721c24;
        }
        
        .badge-source {
            background: #cfe2ff;
            color: #084298;
        }
        
        .empty-state {
            text-align: center;
            padding: 60px 20px;
            color: #95a5a6;
        }
        
        .empty-state i {
            font-size: 4rem;
            margin-bottom: 20px;
        }
        
        .btn-back {
            background: #95a5a6;
            color: white;
            padding: 12px 25px;
            border-radius: 6px;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            font-weight: 600;
            transition: all 0.2s;
        }
        
        .btn-back:hover {
            background: #7f8c8d;
            color: white;
            text-decoration: none;
            transform: translateY(-1px);
        }
        
        .btn-back i {
            margin-right: 8px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <!-- Recherche -->
        <div class="search-card">
            <h3 style="margin-bottom: 20px;">🔍 Rechercher l'historique d'un agent</h3>
            <div class="search-grid">
                <div class="form-group">
                    <label class="form-label">Agent</label>
                    <asp:DropDownList ID="ddlAgent" runat="server" CssClass="form-control">
                        <asp:ListItem Value="">-- Sélectionner un agent --</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <asp:Button ID="btnRechercher" runat="server" Text="📋 Voir Historique" 
                           CssClass="btn-search" OnClick="btnRechercher_Click" />
            </div>
        </div>
        
        <!-- Info Agent -->
        <asp:Panel ID="pnlAgentInfo" runat="server" Visible="false" CssClass="agent-info">
            <h3>
                <i class="fas fa-user"></i>
                <asp:Label ID="lblNomAgent" runat="server"></asp:Label>
            </h3>
            <p><strong>Matricule:</strong> <asp:Label ID="lblMatricule" runat="server"></asp:Label></p>
        </asp:Panel>
        
        <!-- Header -->
        <div class="page-header">
            <h1 class="page-title">
                <i class="fas fa-history"></i>
                Historique des Responsabilités
            </h1>
        </div>
        
        <!-- Timeline -->
        <div class="timeline">
            <asp:Repeater ID="rptHistorique" runat="server">
                <ItemTemplate>
                    <div class='<%# Eval("statut").ToString() == "A" ? "timeline-item current" : "timeline-item" %>'>
                        <div class="timeline-date">
                            <i class="fas fa-calendar"></i> 
                            <%# Eval("date_r", "{0:dd/MM/yyyy}") %>
                        </div>
                        <div class="timeline-content">
                            <div class="timeline-title">
                                <%# Eval("responsabilite") %>
                                <%# Eval("statut").ToString() == "A" ? 
                                    "<span class='timeline-badge badge-active'>Actuel</span>" : 
                                    "<span class='timeline-badge badge-inactive'>Terminé</span>" %>
                                <span class="timeline-badge badge-source">
                                    <%# Eval("source") %>
                                </span>
                            </div>
                            <div class="timeline-details">
                                Code: <%# Eval("code_res") %> | 
                                Code A: <%# Eval("code_a") %>
                                <%# Eval("voiture") != DBNull.Value ? " | Voiture: " + Eval("voiture") : "" %>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Label ID="lblEmpty" runat="server" Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>'>
                        <div class="empty-state">
                            <i class="fas fa-history"></i>
                            <h3>Aucun historique trouvé</h3>
                            <p>Sélectionnez un agent pour voir son historique</p>
                        </div>
                    </asp:Label>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        
        <div style="margin-top: 30px; text-align: center;">
            <a href="AgentsResponsabilites.aspx" class="btn-back">
                <i class="fas fa-arrow-left"></i> Retour
            </a>
        </div>
    </div>
</asp:Content>