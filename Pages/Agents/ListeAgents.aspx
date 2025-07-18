<%@ Page Title="Liste des Agents" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ListeAgents.aspx.cs" Inherits="GestionRHv2.Pages.Agents.ListeAgents" %>

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
        }
        
        .btn-add i {
            margin-right: 8px;
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
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            font-size: 16px;
        }
        
        .search-input:focus {
            outline: none;
            border-color: #3498db;
        }
        
        .table-container {
            background: white;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            overflow: hidden;
        }
        
        .agents-table {
            width: 100%;
            border-collapse: collapse;
        }
        
        .agents-table th {
            background: #f8f9fa;
            padding: 15px;
            text-align: left;
            font-weight: 600;
            color: #5a6c7d;
            border-bottom: 2px solid #e9ecef;
        }
        
        .agents-table td {
            padding: 15px;
            border-bottom: 1px solid #f0f0f0;
        }
        
        .agents-table tr:hover {
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
        }
        
        .btn-view {
            background: #3498db;
            color: white;
        }
        
        .btn-view:hover {
            background: #2980b9;
        }
        
        .btn-edit {
            background: #f39c12;
            color: white;
        }
        
        .btn-edit:hover {
            background: #e67e22;
        }
        
        .btn-delete {
            background: #e74c3c;
            color: white;
        }
        
        .btn-delete:hover {
            background: #c0392b;
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
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="page-header">
            <h1 class="page-title">
                <i class="fas fa-users"></i> Liste des Agents
            </h1>
            <a href="AjouterAgent.aspx" class="btn-add">
                <i class="fas fa-plus-circle"></i>
                Ajouter un Agent
            </a>
        </div>
        
        <div class="search-box">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input" 
                        placeholder="Rechercher par nom, prénom, matricule..." 
                        AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
        </div>
        
        <div class="table-container">
            <asp:GridView ID="gvAgents" runat="server" CssClass="agents-table" 
                         AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                         DataKeyNames="matricule"
                         OnRowCommand="gvAgents_RowCommand" OnRowDeleting="gvAgents_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="matricule" HeaderText="Matricule" />
                    <asp:BoundField DataField="nom" HeaderText="Nom" />
                    <asp:BoundField DataField="prenom" HeaderText="Prénom" />
                    <asp:BoundField DataField="cin" HeaderText="CIN" />
                    <asp:BoundField DataField="tel" HeaderText="Téléphone" />
                    <asp:BoundField DataField="ville" HeaderText="Ville" />
                    
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnView" runat="server" CommandName="View" 
                                           CommandArgument='<%# Eval("matricule") %>' 
                                           CssClass="btn-action btn-view">
                                <i class="fas fa-eye"></i> Voir
                            </asp:LinkButton>
                            
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" 
                                           CommandArgument='<%# Eval("matricule") %>' 
                                           CssClass="btn-action btn-edit">
                                <i class="fas fa-edit"></i> Modifier
                            </asp:LinkButton>
                            
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" 
                                           CommandArgument='<%# Eval("matricule") %>' 
                                           CssClass="btn-action btn-delete"
                                           OnClientClick="return confirm('Êtes-vous sûr de vouloir supprimer cet agent ?');">
                                <i class="fas fa-trash"></i> Supprimer
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                
                <EmptyDataTemplate>
                    <div class="empty-state">
                        <i class="fas fa-users-slash"></i>
                        <h3>Aucun agent trouvé</h3>
                        <p>Commencez par ajouter un nouvel agent</p>
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>