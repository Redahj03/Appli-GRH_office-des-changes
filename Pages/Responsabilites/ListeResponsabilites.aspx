<%@ Page Title="Gestion des Types de Responsabilités" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ListeResponsabilites.aspx.cs" Inherits="GestionRHv2.Pages.Responsabilites.ListeResponsabilites" %>

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
            color: #e74c3c;
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
            background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
        }
        
        .btn-secondary:hover {
            box-shadow: 0 5px 15px rgba(52, 152, 219, 0.3);
        }
        
        /* Statistiques */
        .stats-cards {
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
            transition: transform 0.3s;
        }
        
        .stat-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 5px 20px rgba(0,0,0,0.15);
        }
        
        .stat-icon {
            font-size: 3rem;
            margin-bottom: 15px;
        }
        
        .stat-icon.types {
            color: #e74c3c;
        }
        
        .stat-icon.agents {
            color: #3498db;
        }
        
        .stat-icon.actives {
            color: #27ae60;
        }
        
        .stat-value {
            font-size: 2.5rem;
            font-weight: 700;
            color: #2c3e50;
            margin: 10px 0;
        }
        
        .stat-label {
            color: #7f8c8d;
            font-size: 1rem;
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
        
        .btn-edit {
            background: #f39c12;
            color: white;
        }
        
        .btn-edit:hover {
            background: #e67e22;
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
        
        .search-box {
            background: white;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            margin-bottom: 30px;
        }
        
        .search-input {
            width: 100%;
            padding: 12px 20px;
            font-size: 16px;
            border: 2px solid #e0e0e0;
            border-radius: 6px;
            transition: border-color 0.3s;
        }
        
        .search-input:focus {
            outline: none;
            border-color: #e74c3c;
            box-shadow: 0 0 0 3px rgba(231, 76, 60, 0.1);
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
                <i class="fas fa-user-shield"></i>
                Types de Responsabilités
            </h1>
            <div>
                <asp:HyperLink NavigateUrl="~/Pages/Responsabilites/AjouterResponsabilite.aspx" 
                              CssClass="btn-add" runat="server">
                    <i class="fas fa-plus-circle"></i>
                    Nouveau Type
                </asp:HyperLink>
                <asp:HyperLink NavigateUrl="~/Pages/Responsabilites/AgentsResponsabilites.aspx" 
                              CssClass="btn-add btn-secondary" runat="server">
                    <i class="fas fa-users"></i>
                    Responsabilités Agents
                </asp:HyperLink>
            </div>
        </div>
        
        <!-- Statistiques -->
        <div class="stats-cards">
            <div class="stat-card">
                <i class="fas fa-list stat-icon types"></i>
                <div class="stat-value">
                    <asp:Label ID="lblNbTypes" runat="server" Text="0" />
                </div>
                <div class="stat-label">Types de Responsabilités</div>
            </div>
            
            <div class="stat-card">
                <i class="fas fa-users stat-icon agents"></i>
                <div class="stat-value">
                    <asp:Label ID="lblNbAgents" runat="server" Text="0" />
                </div>
                <div class="stat-label">Agents avec Responsabilité</div>
            </div>
            
            <div class="stat-card">
                <i class="fas fa-check-circle stat-icon actives"></i>
                <div class="stat-value">
                    <asp:Label ID="lblNbActives" runat="server" Text="0" />
                </div>
                <div class="stat-label">Responsabilités Actives</div>
            </div>
        </div>
        
        <!-- Recherche -->
        <div class="search-box">
            <asp:TextBox ID="txtRecherche" runat="server" CssClass="search-input" 
                        placeholder="🔍 Rechercher un type de responsabilité..."
                        AutoPostBack="true" OnTextChanged="txtRecherche_TextChanged" />
        </div>
        
        <!-- Table -->
        <div class="table-container">
            <asp:GridView ID="gvResponsabilites" runat="server" CssClass="data-table" 
                         AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                         OnRowCommand="gvResponsabilites_RowCommand" 
                         DataKeyNames="CODE_RS">
                <Columns>
                    <asp:BoundField DataField="CODE_RS" HeaderText="Code" />
                    <asp:BoundField DataField="LIBELLE" HeaderText="Libellé de la Responsabilité" />
                    
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" 
                                           CommandArgument='<%# Eval("CODE_RS") %>' 
                                           CssClass="btn-action btn-edit">
                                <i class="fas fa-edit"></i> Modifier
                            </asp:LinkButton>
                            
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" 
                                           CommandArgument='<%# Eval("CODE_RS") %>' 
                                           CssClass="btn-action btn-delete"
                                           OnClientClick="return confirm('Êtes-vous sûr de vouloir supprimer ce type de responsabilité ?');">
                                <i class="fas fa-trash"></i> Supprimer
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                
                <EmptyDataTemplate>
                    <div class="empty-state">
                        <i class="fas fa-user-shield"></i>
                        <h3>Aucun type de responsabilité trouvé</h3>
                        <p>Commencez par ajouter un nouveau type</p>
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>