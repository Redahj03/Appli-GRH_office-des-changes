<%@ Page Title="Responsabilités des Agents" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AgentsResponsabilites.aspx.cs" Inherits="GestionRHv2.Pages.Responsabilites.AgentsResponsabilites" %>

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
            display: flex;
            align-items: center;
        }
        
        .page-title i {
            margin-right: 15px;
            color: #3498db;
        }
        
        .btn-add {
            background: linear-gradient(135deg, #27ae60 0%, #229954 100%);
            color: white;
            padding: 15px 25px;
            border: none;
            border-radius: 8px;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            font-weight: 600;
            transition: transform 0.2s;
            font-size: 1rem;
            margin: 0 5px;
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
        
        .btn-secondary {
            background: linear-gradient(135deg, #9b59b6 0%, #8e44ad 100%);
        }
        
        .btn-secondary:hover {
            box-shadow: 0 5px 15px rgba(155, 89, 182, 0.3);
        }
        
        /* Filtres */
        .filters-card {
            background: white;
            padding: 25px;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            margin-bottom: 30px;
        }
        
        .filters-title {
            font-size: 1.2rem;
            color: #2c3e50;
            margin-bottom: 20px;
            font-weight: 600;
        }
        
        .filters-grid {
            display: grid;
            grid-template-columns: 2fr 1fr 1fr auto;
            gap: 20px;
            align-items: end;
        }
        
        .filter-group {
            display: flex;
            flex-direction: column;
        }
        
        .filter-label {
            font-weight: 600;
            color: #5a6c7d;
            margin-bottom: 8px;
        }
        
        .filter-control {
            padding: 10px 15px;
            border: 2px solid #e0e0e0;
            border-radius: 6px;
            font-size: 16px;
            transition: border-color 0.3s;
        }
        
        .filter-control:focus {
            outline: none;
            border-color: #3498db;
            box-shadow: 0 0 0 3px rgba(52, 152, 219, 0.1);
        }
        
        .btn-filter {
            padding: 10px 20px;
            background: #3498db;
            color: white;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-weight: 600;
            font-size: 16px;
            transition: all 0.2s;
        }
        
        .btn-filter:hover {
            background: #2980b9;
            transform: translateY(-1px);
        }
        
        /* Table */
        .table-container {
            background: white;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            overflow: hidden;
        }
        
        .data-table {
            width: 100%;
            border-collapse: collapse;
        }
        
        .data-table th {
            background: #f8f9fa;
            padding: 15px;
            text-align: left;
            font-weight: 600;
            color: #5a6c7d;
            border-bottom: 2px solid #e9ecef;
        }
        
        .data-table td {
            padding: 15px;
            border-bottom: 1px solid #f0f0f0;
            vertical-align: middle;
        }
        
        .data-table tr:hover {
            background: #f8f9fa;
        }
        
        .badge {
            padding: 5px 10px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: 600;
        }
        
        .badge-active {
            background: #d4edda;
            color: #155724;
        }
        
        .badge-inactive {
            background: #f8d7da;
            color: #721c24;
        }
        
        .btn-action {
            padding: 8px 15px;
            margin: 0 3px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 14px;
            text-decoration: none;
            display: inline-block;
            transition: all 0.2s;
            font-weight: 500;
        }
        
        .btn-history {
            background: #9b59b6;
            color: white;
        }
        
        .btn-history:hover {
            background: #8e44ad;
            color: white;
        }
        
        .btn-delete {
            background: #e74c3c;
            color: white;
        }
        
        .btn-delete:hover {
            background: #c0392b;
            color: white;
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
        
        .alert {
            padding: 15px 20px;
            border-radius: 6px;
            margin-bottom: 20px;
            display: flex;
            align-items: center;
        }
        
        .alert i {
            margin-right: 10px;
        }
        
        .alert-success {
            background: #d4edda;
            color: #155724;
            border: 1px solid #c3e6cb;
        }
        
        .alert-error {
            background: #f8d7da;
            color: #721c24;
            border: 1px solid #f5c6cb;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert">
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </asp:Panel>
        
        <div class="page-header">
            <h1 class="page-title">
                <i class="fas fa-users-cog"></i>
                Responsabilités des Agents
            </h1>
            <div>
                <asp:HyperLink NavigateUrl="~/Pages/Responsabilites/AttribuerResponsabilite.aspx" 
                              CssClass="btn-add" runat="server">
                    <i class="fas fa-plus-circle"></i>
                    Attribuer Responsabilité
                </asp:HyperLink>
                <asp:HyperLink NavigateUrl="~/Pages/Responsabilites/HistoriqueAgent.aspx" 
                              CssClass="btn-add btn-secondary" runat="server">
                    <i class="fas fa-history"></i>
                    Consulter Historique
                </asp:HyperLink>
            </div>
        </div>
        
        <!-- Filtres -->
        <div class="filters-card">
            <h3 class="filters-title">🔍 Filtres de Recherche</h3>
            <div class="filters-grid">
                <div class="filter-group">
                    <label class="filter-label">Recherche</label>
                    <asp:TextBox ID="txtRecherche" runat="server" CssClass="filter-control" 
                                placeholder="Matricule, nom, prénom..." />
                </div>
                
                <div class="filter-group">
                    <label class="filter-label">Responsabilité</label>
                    <asp:DropDownList ID="ddlResponsabilite" runat="server" CssClass="filter-control">
                        <asp:ListItem Value="">-- Toutes --</asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="filter-group">
                    <label class="filter-label">Statut</label>
                    <asp:DropDownList ID="ddlStatut" runat="server" CssClass="filter-control">
                        <asp:ListItem Value="">-- Tous --</asp:ListItem>
                        <asp:ListItem Value="A">Actuel</asp:ListItem>
                        <asp:ListItem Value="N">Non Actuel</asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="filter-group">
                    <asp:Button ID="btnFiltrer" runat="server" Text="🔍 Filtrer" 
                               CssClass="btn-filter" OnClick="btnFiltrer_Click" />
                </div>
            </div>
        </div>
        
        <!-- Table -->
        <div class="table-container">
            <asp:GridView ID="gvAgentsResponsabilites" runat="server" CssClass="data-table" 
                         AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                         OnRowCommand="gvAgentsResponsabilites_RowCommand">
                <Columns>
                    <asp:BoundField DataField="matricule" HeaderText="Matricule" />
                    <asp:BoundField DataField="nom" HeaderText="Nom" />
                    <asp:BoundField DataField="prenom" HeaderText="Prénom" />
                    <asp:BoundField DataField="responsabilite" HeaderText="Responsabilité" />
                    <asp:BoundField DataField="date_r" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" />
                    
                    <asp:TemplateField HeaderText="Statut">
                        <ItemTemplate>
                            <%# Eval("Actuel").ToString() == "A" ? 
                                "<span class='badge badge-active'>Actuel</span>" : 
                                "<span class='badge badge-inactive'>Non Actuel</span>" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnHistory" runat="server" CommandName="History" 
                                           CommandArgument='<%# Eval("matricule") %>' 
                                           CssClass="btn-action btn-history">
                                <i class="fas fa-history"></i> Historique
                            </asp:LinkButton>
                            
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" 
                                           CommandArgument='<%# Eval("matricule") + "|" + Eval("code_res") + "|" + Eval("date_r") %>' 
                                           CssClass="btn-action btn-delete"
                                           OnClientClick="return confirm('Êtes-vous sûr de vouloir supprimer cette responsabilité ?');">
                                <i class="fas fa-trash"></i> Supprimer
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                
                <EmptyDataTemplate>
                    <div class="empty-state">
                        <i class="fas fa-users-cog"></i>
                        <h3>Aucune responsabilité d'agent trouvée</h3>
                        <p>Commencez par attribuer des responsabilités aux agents</p>
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>