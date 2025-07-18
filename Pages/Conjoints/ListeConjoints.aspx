<%@ Page Title="Liste des Conjoints" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ListeConjoints.aspx.cs" Inherits="GestionRHv2.Pages.Conjoints.ListeConjoints" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .conjoints-container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
        }

        .page-header {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 30px;
            border-radius: 15px;
            margin-bottom: 30px;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .page-header h1 {
            font-size: 2.5em;
            margin: 0;
        }

        .btn-new {
            background: white;
            color: #667eea;
            padding: 12px 24px;
            border: none;
            border-radius: 8px;
            font-weight: 600;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            gap: 8px;
            transition: all 0.3s ease;
        }

        .btn-new:hover {
            transform: translateY(-2px);
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.2);
        }

        .search-section {
            background: white;
            padding: 20px;
            border-radius: 10px;
            margin-bottom: 20px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
        }

        .search-box {
            display: flex;
            gap: 10px;
            align-items: center;
        }

        .search-input {
            flex: 1;
            padding: 12px 20px;
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            font-size: 1em;
        }

        .btn-search {
            background: #667eea;
            color: white;
            border: none;
            padding: 12px 24px;
            border-radius: 8px;
            cursor: pointer;
            font-weight: 600;
            display: flex;
            align-items: center;
            gap: 8px;
        }

        .table-container {
            background: white;
            border-radius: 10px;
            padding: 20px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
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
            color: #333;
            border-bottom: 2px solid #e0e0e0;
        }

        .data-table td {
            padding: 15px;
            border-bottom: 1px solid #f0f0f0;
        }

        .data-table tr:hover {
            background: #f8f9fa;
        }

        .badge {
            padding: 4px 12px;
            border-radius: 20px;
            font-size: 0.85em;
            font-weight: 600;
        }

        .badge-success {
            background: #d4edda;
            color: #155724;
        }

        .badge-danger {
            background: #f8d7da;
            color: #721c24;
        }

        .action-buttons {
            display: flex;
            gap: 5px;
        }

        .btn-action {
            padding: 6px 12px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 0.85em;
            transition: all 0.2s;
        }

        .btn-edit {
            background: #4CAF50;
            color: white;
        }

        .btn-edit:hover {
            background: #45a049;
        }

        .btn-delete {
            background: #f44336;
            color: white;
        }

        .btn-delete:hover {
            background: #da190b;
        }

        .btn-details {
            background: #2196F3;
            color: white;
        }

        .btn-details:hover {
            background: #1976D2;
        }

        .empty-state {
            text-align: center;
            padding: 60px;
            color: #999;
        }

        .empty-state i {
            font-size: 4em;
            margin-bottom: 20px;
            opacity: 0.3;
        }

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
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
            display: flex;
            align-items: center;
            gap: 20px;
        }

        .stat-icon {
            width: 60px;
            height: 60px;
            border-radius: 10px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.5em;
        }

        .stat-icon.total {
            background: #e3f2fd;
            color: #2196F3;
        }

        .stat-icon.with-allocation {
            background: #d4edda;
            color: #4CAF50;
        }

        .stat-icon.without-allocation {
            background: #f8d7da;
            color: #f44336;
        }

        .stat-content h3 {
            margin: 0;
            font-size: 2em;
            color: #333;
        }

        .stat-content p {
            margin: 5px 0 0 0;
            color: #666;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="conjoints-container">
        <div class="page-header">
            <div>
                <h1><i class="fas fa-heart"></i> Liste des Conjoints</h1>
            </div>
            <a href="AjouterConjoint.aspx" class="btn-new">
                <i class="fas fa-plus"></i> Nouveau Conjoint
            </a>
        </div>

        <!-- Cartes statistiques -->
        <div class="stats-cards">
            <div class="stat-card">
                <div class="stat-icon total">
                    <i class="fas fa-users"></i>
                </div>
                <div class="stat-content">
                    <h3><asp:Label ID="lblTotalConjoints" runat="server" Text="0"></asp:Label></h3>
                    <p>Total des conjoints</p>
                </div>
            </div>
            <div class="stat-card">
                <div class="stat-icon with-allocation">
                    <i class="fas fa-check-circle"></i>
                </div>
                <div class="stat-content">
                    <h3><asp:Label ID="lblWithAllocation" runat="server" Text="0"></asp:Label></h3>
                    <p>Avec allocation</p>
                </div>
            </div>
            <div class="stat-card">
                <div class="stat-icon without-allocation">
                    <i class="fas fa-times-circle"></i>
                </div>
                <div class="stat-content">
                    <h3><asp:Label ID="lblWithoutAllocation" runat="server" Text="0"></asp:Label></h3>
                    <p>Sans allocation</p>
                </div>
            </div>
        </div>

        <!-- Section de recherche -->
        <div class="search-section">
            <div class="search-box">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input" 
                             placeholder="Rechercher par nom, prénom, matricule..."></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="Rechercher" CssClass="btn-search" OnClick="btnSearch_Click">
                </asp:Button>
            </div>
        </div>

        <!-- Tableau des conjoints -->
        <div class="table-container">
                            <asp:GridView ID="gvConjoints" runat="server" CssClass="data-table" 
                          AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                          OnRowCommand="gvConjoints_RowCommand"
                          GridLines="None">
                <Columns>
                    <asp:BoundField DataField="num" HeaderText="N°" />
                    <asp:BoundField DataField="matricule" HeaderText="Matricule" />
                    <asp:BoundField DataField="nom_employe" HeaderText="Employé" />
                    <asp:BoundField DataField="nom" HeaderText="Nom" />
                    <asp:BoundField DataField="prenom" HeaderText="Prénom" />
                    <asp:BoundField DataField="prof" HeaderText="Profession" />
                    <asp:BoundField DataField="date_m" HeaderText="Date mariage" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Allocation">
                        <ItemTemplate>
                            <span class='<%# Eval("allocation_f").ToString() == "1" ? "badge badge-success" : "badge badge-danger" %>'>
                                <%# Eval("allocation_f").ToString() == "1" ? "Oui" : "Non" %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <div class="action-buttons">
                                <asp:Button ID="btnDetails" runat="server" Text="Détails" 
                                            CssClass="btn-action btn-details"
                                            CommandName="ViewDetails" 
                                            CommandArgument='<%# Eval("matricule") + "|" + Eval("num") %>' />
                                <asp:Button ID="btnEdit" runat="server" Text="Modifier" 
                                            CssClass="btn-action btn-edit"
                                            CommandName="EditConjoint" 
                                            CommandArgument='<%# Eval("matricule") + "|" + Eval("num") %>' />
                                <asp:Button ID="btnDelete" runat="server" Text="Supprimer" 
                                            CssClass="btn-action btn-delete"
                                            CommandName="DeleteConjoint" 
                                            CommandArgument='<%# Eval("matricule") + "|" + Eval("num") %>'
                                            OnClientClick="return confirm('Êtes-vous sûr de vouloir supprimer ce conjoint ?');" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div class="empty-state">
                        <i class="fas fa-heart"></i>
                        <p>Aucun conjoint trouvé</p>
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>