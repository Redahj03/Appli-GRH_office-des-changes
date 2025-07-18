<%@ Page Title="Inscription aux Formations" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="InscrireAgents.aspx.cs" Inherits="GestionRHv2.Pages.Formation.InscrireAgents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .container {
            max-width: 1400px;
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
        
        /* Info formation */
        .formation-info {
            background: white;
            padding: 25px;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            margin-bottom: 30px;
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 20px;
        }
        
        .info-item {
            display: flex;
            flex-direction: column;
        }
        
        .info-label {
            color: #7f8c8d;
            font-size: 13px;
            margin-bottom: 5px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }
        
        .info-value {
            color: #2c3e50;
            font-size: 16px;
            font-weight: 600;
        }
        
        /* Section principale */
        .main-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 30px;
        }
        
        /* Box communes */
        .box {
            background: white;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            overflow: hidden;
        }
        
        .box-header {
            background: #f8f9fa;
            padding: 20px;
            border-bottom: 1px solid #e9ecef;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        
        .box-title {
            font-size: 1.2rem;
            color: #2c3e50;
            margin: 0;
            display: flex;
            align-items: center;
            gap: 10px;
        }
        
        .box-body {
            padding: 20px;
        }
        
        /* Recherche */
        .search-section {
            margin-bottom: 20px;
        }
        
        .search-input {
            width: 100%;
            padding: 10px 15px;
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            font-size: 15px;
            transition: border-color 0.2s;
        }
        
        .search-input:focus {
            outline: none;
            border-color: #3498db;
        }
        
        /* Liste des agents */
        .agents-list {
            max-height: 400px;
            overflow-y: auto;
            border: 1px solid #e9ecef;
            border-radius: 8px;
        }
        
        .agent-item {
            padding: 15px;
            border-bottom: 1px solid #f0f0f0;
            display: flex;
            align-items: center;
            gap: 15px;
            transition: background-color 0.2s;
        }
        
        .agent-item:hover {
            background: #f8f9fa;
        }
        
        .agent-item:last-child {
            border-bottom: none;
        }
        
        .agent-checkbox {
            width: 20px;
            height: 20px;
            cursor: pointer;
        }
        
        .agent-info {
            flex: 1;
        }
        
        .agent-name {
            font-weight: 600;
            color: #2c3e50;
            margin-bottom: 3px;
        }
        
        .agent-details {
            font-size: 13px;
            color: #7f8c8d;
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
            display: inline-flex;
            align-items: center;
            gap: 8px;
        }
        
        .btn-add {
            background: #27ae60;
            color: white;
        }
        
        .btn-add:hover {
            background: #229954;
            transform: translateY(-2px);
        }
        
        .btn-remove {
            background: #e74c3c;
            color: white;
        }
        
        .btn-remove:hover {
            background: #c0392b;
            transform: translateY(-2px);
        }
        
        .btn-back {
            background: #95a5a6;
            color: white;
        }
        
        .btn-back:hover {
            background: #7f8c8d;
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
        
        /* Badge nombre */
        .count-badge {
            background: #3498db;
            color: white;
            padding: 4px 12px;
            border-radius: 20px;
            font-size: 14px;
        }
        
        .count-badge.full {
            background: #e74c3c;
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
        
        .alert-warning {
            background: #fff3cd;
            color: #856404;
            border: 1px solid #ffeeba;
        }
        
        /* CheckAll */
        .check-all-container {
            padding: 15px;
            background: #f8f9fa;
            border-bottom: 2px solid #e9ecef;
            display: flex;
            align-items: center;
            gap: 10px;
            font-weight: 600;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <!-- En-tête -->
        <div class="page-header">
            <h1 class="page-title">
                <i class="fas fa-user-plus"></i> Gestion des Inscriptions
            </h1>
            <div class="breadcrumb">
                <a href="ListeFormations.aspx">Formations</a>
                <span>/</span>
                <span>Inscriptions</span>
            </div>
        </div>
        
        <!-- Messages -->
        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert">
            <i class="fas fa-info-circle"></i>
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </asp:Panel>
        
        <!-- Informations de la formation -->
        <div class="formation-info">
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
                <span class="info-label">Effectif</span>
                <span class="info-value">
                    <asp:Label ID="lblEffectif" runat="server"></asp:Label>
                </span>
            </div>
            <div class="info-item">
                <span class="info-label">Places restantes</span>
                <span class="info-value">
                    <asp:Label ID="lblPlacesRestantes" runat="server"></asp:Label>
                </span>
            </div>
        </div>
        
        <!-- Grille principale -->
        <div class="main-grid">
            <!-- Agents disponibles -->
            <div class="box">
                <div class="box-header">
                    <h3 class="box-title">
                        <i class="fas fa-users"></i> Agents Disponibles
                        <span class="count-badge">
                            <asp:Label ID="lblCountDisponibles" runat="server" Text="0"></asp:Label>
                        </span>
                    </h3>
                </div>
                <div class="box-body">
                    <div class="search-section">
                        <asp:TextBox ID="TxtRechercheDisponibles" runat="server" 
                                    CssClass="search-input" 
                                    placeholder="Rechercher par nom, prénom, matricule..."
                                    AutoPostBack="true"
                                    OnTextChanged="TxtRechercheDisponibles_TextChanged"></asp:TextBox>
                    </div>
                    
                    <div class="check-all-container">
                        <asp:CheckBox ID="chkSelectAllDisponibles" runat="server" 
                                     CssClass="agent-checkbox"
                                     AutoPostBack="true"
                                     OnCheckedChanged="ChkSelectAllDisponibles_CheckedChanged" />
                        <label>Sélectionner tout</label>
                    </div>
                    
                    <div class="agents-list">
                        <asp:Repeater ID="rptAgentsDisponibles" runat="server">
                            <ItemTemplate>
                                <div class="agent-item">
                                    <asp:CheckBox ID="chkAgent" runat="server" 
                                                 CssClass="agent-checkbox" />
                                    <asp:HiddenField ID="hfMatricule" runat="server" 
                                                    Value='<%# Eval("matricule") %>' />
                                    <div class="agent-info">
                                        <div class="agent-name">
                                            <%# Eval("nom") %> <%# Eval("prenom") %>
                                        </div>
                                        <div class="agent-details">
                                            Mat: <%# Eval("matricule") %> | 
                                            <%# Eval("ville") %>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        
                        <asp:Panel ID="pnlAucunDisponible" runat="server" Visible="false">
                            <div class="empty-state">
                                <i class="fas fa-user-slash"></i>
                                <p>Aucun agent disponible</p>
                            </div>
                        </asp:Panel>
                    </div>
                    
                    <div class="action-buttons">
                        <asp:Button ID="btnInscrire" runat="server" 
                                   Text="Inscrire →" 
                                   CssClass="btn btn-add"
                                   OnClick="btnInscrire_Click" />
                    </div>
                </div>
            </div>
            
            <!-- Agents inscrits -->
            <div class="box">
                <div class="box-header">
                    <h3 class="box-title">
                        <i class="fas fa-user-check"></i> Agents Inscrits
                        <span class="count-badge" id="badgeInscrits" runat="server">
                            <asp:Label ID="lblCountInscrits" runat="server" Text="0"></asp:Label>
                        </span>
                    </h3>
                </div>
                <div class="box-body">
                    <div class="search-section">
                        <asp:TextBox ID="TxtRechercheInscrits" runat="server" 
                                    CssClass="search-input" 
                                    placeholder="Rechercher par nom, prénom, matricule..."
                                    AutoPostBack="true"
                                    OnTextChanged="TxtRechercheInscrits_TextChanged"></asp:TextBox>
                    </div>
                    
                    <div class="check-all-container">
                        <asp:CheckBox ID="ChkSelectAllInscrits" runat="server" 
                                     CssClass="agent-checkbox"
                                     AutoPostBack="true"
                                     OnCheckedChanged="ChkSelectAllInscrits_CheckedChanged" />
                        <label>Sélectionner tout</label>
                    </div>
                    
                    <div class="agents-list">
                        <asp:Repeater ID="rptAgentsInscrits" runat="server">
                            <ItemTemplate>
                                <div class="agent-item">
                                    <asp:CheckBox ID="chkAgent" runat="server" 
                                                 CssClass="agent-checkbox" />
                                    <asp:HiddenField ID="hfMatricule" runat="server" 
                                                    Value='<%# Eval("matricule") %>' />
                                    <div class="agent-info">
                                        <div class="agent-name">
                                            <%# Eval("nom") %> <%# Eval("prenom") %>
                                        </div>
                                        <div class="agent-details">
                                            Mat: <%# Eval("matricule") %> | 
                                            Inscrit le: <%# Convert.ToDateTime(Eval("date_form")).ToString("dd/MM/yyyy") %>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        
                        <asp:Panel ID="pnlAucunInscrit" runat="server" Visible="false">
                            <div class="empty-state">
                                <i class="fas fa-user-times"></i>
                                <p>Aucun agent inscrit</p>
                            </div>
                        </asp:Panel>
                    </div>
                    
                    <div class="action-buttons">
                        <asp:Button ID="btnDesinscrire" runat="server" 
                                   Text="← Désinscrire" 
                                   CssClass="btn btn-remove"
                                   OnClick="btnDesinscrire_Click" />
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Bouton retour -->
        <div style="text-align: center; margin-top: 30px;">
            <asp:Button ID="btnRetour" runat="server" 
                       Text="Retour à la liste" 
                       CssClass="btn btn-back"
                       OnClick="btnRetour_Click"
                       CausesValidation="false" />
        </div>
    </div>
</asp:Content>