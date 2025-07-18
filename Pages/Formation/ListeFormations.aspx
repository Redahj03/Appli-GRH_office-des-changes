<%@ Page Title="Liste des Formations" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ListeFormations.aspx.cs" Inherits="GestionRHv2.Pages.Formation.ListeFormations" %>

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
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        
        .page-title {
            font-size: 2rem;
            color: #2c3e50;
            margin: 0;
        }
        
        .btn-add {
            background: linear-gradient(135deg, #27ae60 0%, #229954 100%);
            color: white;
            padding: 12px 25px;
            border: none;
            border-radius: 8px;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            font-weight: 600;
            transition: transform 0.2s;
        }
        
        .btn-add:hover {
            transform: translateY(-2px);
            box-shadow: 0 5px 15px rgba(39, 174, 96, 0.3);
            color: white;
            text-decoration: none;
        }
        
        .btn-add i {
            margin-right: 8px;
        }
        
        /* Cartes de statistiques */
        .stats-container {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 20px;
            margin-bottom: 30px;
        }
        
        .stat-card {
            background: white;
            padding: 25px;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            text-align: center;
            transition: transform 0.2s;
        }
        
        .stat-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 5px 20px rgba(0,0,0,0.15);
        }
        
        .stat-card i {
            font-size: 2.5rem;
            margin-bottom: 15px;
        }
        
        .stat-card.blue i { color: #3498db; }
        .stat-card.green i { color: #27ae60; }
        .stat-card.orange i { color: #f39c12; }
        .stat-card.purple i { color: #9b59b6; }
        
        .stat-value {
            font-size: 2rem;
            font-weight: bold;
            color: #2c3e50;
            margin: 10px 0;
        }
        
        .stat-label {
            color: #7f8c8d;
            font-size: 0.9rem;
            text-transform: uppercase;
            letter-spacing: 1px;
        }
        
        /* Boîte de recherche */
        .search-box {
            background: white;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            margin-bottom: 30px;
        }
        
        .search-filters {
            display: grid;
            grid-template-columns: 2fr 1fr 1fr 150px 150px;
            gap: 15px;
            align-items: end;
        }
        
        .form-group {
            margin-bottom: 0;
        }
        
        .form-group label {
            display: block;
            margin-bottom: 5px;
            color: #5a6c7d;
            font-weight: 600;
            font-size: 14px;
        }
        
        .form-control {
            width: 100%;
            padding: 10px 15px;
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            font-size: 15px;
            transition: border-color 0.2s;
        }
        
        .form-control:focus {
            outline: none;
            border-color: #3498db;
        }
        
        .btn-search, .btn-reset {
            padding: 10px 20px;
            border: none;
            border-radius: 8px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s;
            height: 42px;
        }
        
        .btn-search {
            background: #3498db;
            color: white;
        }
        
        .btn-search:hover {
            background: #2980b9;
            transform: translateY(-2px);
        }
        
        .btn-reset {
            background: #95a5a6;
            color: white;
        }
        
        .btn-reset:hover {
            background: #7f8c8d;
        }
        
        /* Table container */
        .table-container {
            background: white;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            overflow: hidden;
        }
        
        .formations-table {
            width: 100%;
            border-collapse: collapse;
        }
        
        .formations-table th {
            background: #f8f9fa;
            padding: 15px;
            text-align: left;
            font-weight: 600;
            color: #5a6c7d;
            border-bottom: 2px solid #e9ecef;
            font-size: 14px;
        }
        
        .formations-table td {
            padding: 15px;
            border-bottom: 1px solid #f0f0f0;
            vertical-align: middle;
        }
        
        .formations-table tr:hover {
            background: #f8f9fa;
        }
        
        /* Badges de statut */
        .status-badge {
            padding: 6px 12px;
            border-radius: 20px;
            font-size: 13px;
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
        .btn-action {
            padding: 6px 12px;
            margin: 0 3px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 13px;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            gap: 5px;
            transition: all 0.2s;
        }
        
        .btn-action i {
            font-size: 14px;
        }
        
        .btn-view {
            background: #3498db;
            color: white;
        }
        
        .btn-view:hover {
            background: #2980b9;
            color: white;
            text-decoration: none;
        }
        
        .btn-edit {
            background: #f39c12;
            color: white;
        }
        
        .btn-edit:hover {
            background: #e67e22;
            color: white;
            text-decoration: none;
        }
        
        .btn-delete {
            background: #e74c3c;
            color: white;
        }
        
        .btn-delete:hover {
            background: #c0392b;
            color: white;
            text-decoration: none;
        }
        
        .btn-inscrire {
            background: #27ae60;
            color: white;
        }
        
        .btn-inscrire:hover {
            background: #229954;
            color: white;
            text-decoration: none;
        }
        
        /* Badge pour le nombre d'inscrits */
        .inscrits-badge {
            display: inline-flex;
            align-items: center;
            gap: 5px;
            font-weight: 600;
        }
        
        .inscrits-badge i {
            color: #7f8c8d;
        }
        
        /* État vide */
        .empty-state {
            text-align: center;
            padding: 60px 20px;
            color: #95a5a6;
        }
        
        .empty-state i {
            font-size: 4rem;
            margin-bottom: 20px;
        }
        
        /* Message d'alerte */
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
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <!-- En-tête de la page -->
        <div class="page-header">
            <h1 class="page-title">
                <i class="fas fa-graduation-cap"></i> Gestion des Formations
            </h1>
            <a href="AjouterFormation.aspx" class="btn-add">
                <i class="fas fa-plus-circle"></i>
                Nouvelle Formation
            </a>
        </div>
        
        <!-- Messages d'alerte -->
        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert">
            <i class="fas fa-info-circle"></i>
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </asp:Panel>
        
        <!-- Cartes de statistiques -->
        <div class="stats-container">
            <div class="stat-card blue">
                <i class="fas fa-book"></i>
                <div class="stat-value">
                    <asp:Label ID="lblTotalFormations" runat="server" Text="0"></asp:Label>
                </div>
                <div class="stat-label">Total Formations</div>
            </div>
            
            <div class="stat-card green">
                <i class="fas fa-calendar-check"></i>
                <div class="stat-value">
                    <asp:Label ID="lblFormationsAnnee" runat="server" Text="0"></asp:Label>
                </div>
                <div class="stat-label">Cette Année</div>
            </div>
            
            <div class="stat-card orange">
                <i class="fas fa-users"></i>
                <div class="stat-value">
                    <asp:Label ID="lblTotalParticipants" runat="server" Text="0"></asp:Label>
                </div>
                <div class="stat-label">Total Participants</div>
            </div>
            
            <div class="stat-card purple">
                <i class="fas fa-user-check"></i>
                <div class="stat-value">
                    <asp:Label ID="lblTotalInscriptions" runat="server" Text="0"></asp:Label>
                </div>
                <div class="stat-label">Total Inscriptions</div>
            </div>
        </div>
        
        <!-- Boîte de recherche -->
        <div class="search-box">
            <div class="search-filters">
                <div class="form-group">
                    <label>Rechercher</label>
                    <asp:TextBox ID="txtRecherche" runat="server" CssClass="form-control" 
                                placeholder="Lieu, contenu..."></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Date début</label>
                    <asp:TextBox ID="txtDateDebut" runat="server" CssClass="form-control" 
                                TextMode="Date"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Date fin</label>
                    <asp:TextBox ID="txtDateFin" runat="server" CssClass="form-control" 
                                TextMode="Date"></asp:TextBox>
                </div>
                <asp:Button ID="btnRechercher" runat="server" Text="Rechercher" 
                           CssClass="btn-search" OnClick="btnRechercher_Click" />
                <asp:Button ID="btnReset" runat="server" Text="Réinitialiser" 
                           CssClass="btn-reset" OnClick="btnReset_Click" />
            </div>
        </div>
        
        <!-- Table des formations -->
        <div class="table-container">
           <asp:GridView ID="gvFormations" runat="server" 
    AutoGenerateColumns="False" 
    CssClass="formations-table"
    OnRowDataBound="GvFormations_RowDataBound"
    OnRowCommand="GvFormations_RowCommand"
    DataKeyNames="Objet,Date_Form">
  

                <Columns>
                    <asp:BoundField DataField="Objet" HeaderText="N° Objet" ItemStyle-Width="80px" />
                    
                    <asp:TemplateField HeaderText="Date Formation" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <%# Convert.ToDateTime(Eval("Date_Form")).ToString("dd/MM/yyyy") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:BoundField DataField="Lieu" HeaderText="Lieu" />
                    <asp:BoundField DataField="Durée" HeaderText="Durée" ItemStyle-Width="80px" />
                    
                    <asp:TemplateField HeaderText="Effectif" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <%# Eval("Effect") != DBNull.Value ? Eval("Effect") : "Illimité" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Inscrits" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div class="inscrits-badge">
                                <i class="fas fa-users"></i>
                                <asp:Label ID="lblNbInscrits" runat="server"></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Statut" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lblStatut" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Contenu">
                        <ItemTemplate>
                            <%# Eval("Contenu") != null && Eval("Contenu").ToString().Length > 50 
                                ? Eval("Contenu").ToString().Substring(0, 50) + "..." 
                                : Eval("Contenu") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Actions" ItemStyle-Width="280px">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnView" runat="server" CommandName="View" 
                                           CommandArgument='<%# Eval("Objet") + "," + Eval("Date_Form") %>' 
                                           CssClass="btn-action btn-view">
                                <i class="fas fa-eye"></i> Voir
                            </asp:LinkButton>
                            
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" 
                                           CommandArgument='<%# Eval("Objet") + "," + Eval("Date_Form") %>' 
                                           CssClass="btn-action btn-edit">
                                <i class="fas fa-edit"></i> Modifier
                            </asp:LinkButton>
                            
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" 
                                           CommandArgument='<%# Eval("Objet") + "," + Eval("Date_Form") %>' 
                                           CssClass="btn-action btn-delete"
                                           OnClientClick="return confirm('Êtes-vous sûr de vouloir supprimer cette formation ?');">
                                <i class="fas fa-trash"></i> Supprimer
                            </asp:LinkButton>
                            
                            <asp:LinkButton ID="btnInscrire" runat="server" CommandName="Inscrire" 
                                           CommandArgument='<%# Eval("Objet") + "," + Eval("Date_Form") %>' 
                                           CssClass="btn-action btn-inscrire">
                                <i class="fas fa-user-plus"></i> Inscrire
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                
                <EmptyDataTemplate>
                    <div class="empty-state">
                        <i class="fas fa-book-open"></i>
                        <h3>Aucune formation trouvée</h3>
                        <p>Commencez par créer une nouvelle formation ou modifiez vos critères de recherche</p>
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>