<%@ Page Title="Liste des Enfants" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ListeEnfants.aspx.cs" Inherits="GestionRHv2.Pages.Enfants.ListeEnfants" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .stats-card {
            background: white;
            border-radius: 8px;
            padding: 20px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            text-align: center;
            margin-bottom: 20px;
        }
        .stats-value {
            font-size: 2.5em;
            font-weight: bold;
            color: #2196F3;
            margin: 10px 0;
        }
        .stats-label {
            color: #666;
            font-size: 0.9em;
        }
        .search-box {
            margin: 20px 0;
        }
        .btn-action {
            padding: 5px 10px;
            margin: 0 2px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            color: white;
            text-decoration: none;
            display: inline-block;
        }
        .btn-view { background-color: #2196F3; }
        .btn-edit { background-color: #FF9800; }
        .btn-delete { background-color: #F44336; }
        .btn-view:hover { background-color: #1976D2; }
        .btn-edit:hover { background-color: #F57C00; }
        .btn-delete:hover { background-color: #D32F2F; }
        .header-section {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
        }
        .header-title {
            display: flex;
            align-items: center;
            gap: 10px;
        }
        .header-title h2 {
            margin: 0;
        }
        .btn-add {
            background-color: #4CAF50;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 4px;
            text-decoration: none;
            display: inline-block;
        }
        .btn-add:hover {
            background-color: #45a049;
        }
        .menu-links {
            display: flex;
            gap: 20px;
            margin-bottom: 20px;
        }
        .menu-links a {
            color: #2196F3;
            text-decoration: none;
        }
        .menu-links a:hover {
            text-decoration: underline;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="header-section">
            <div class="header-title">
                <span style="font-size: 2em;">👶</span>
                <h2>Liste des Enfants</h2>
            </div>
            <a href="AjouterEnfant.aspx" class="btn-add">➕ Ajouter un Enfant</a>
        </div>
        
        <div class="menu-links">
            <a href="#">📋 Liste des Enfants</a>
            <a href="AjouterEnfant.aspx">➕ Ajouter un Enfant</a>
            <a href="#">👥 Enfants par Agent</a>
            <a href="#">🔍 Rechercher</a>
        </div>

        <!-- Statistiques -->
        <div class="row">
            <div class="col-md-3">
                <div class="stats-card">
                    <div class="stats-value">
                        <asp:Label ID="lblTotalEnfants" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="stats-label">Total Enfants</div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="stats-card">
                    <div class="stats-value" style="color: #03A9F4;">
                        <asp:Label ID="lblGarcons" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="stats-label">Garçons</div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="stats-card">
                    <div class="stats-value" style="color: #E91E63;">
                        <asp:Label ID="lblFilles" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="stats-label">Filles</div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="stats-card">
                    <div class="stats-value" style="color: #FF9800;">
                        <asp:Label ID="lblAgeMoyen" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="stats-label">Âge Moyen</div>
                </div>
            </div>
        </div>

        <!-- Recherche -->
        <div class="search-box">
            <div class="input-group">
                <asp:TextBox ID="txtRecherche" runat="server" CssClass="form-control" 
                    placeholder="Rechercher par nom, prénom, matricule agent..."></asp:TextBox>
                <div class="input-group-append">
                    <asp:Button ID="btnRechercher" runat="server" Text="Rechercher" 
                        CssClass="btn btn-primary" OnClick="btnRechercher_Click" />
                </div>
            </div>
        </div>

        <!-- Message -->
        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert" role="alert">
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </asp:Panel>

        <!-- Grille des enfants -->
        <div class="table-responsive">
            <asp:GridView ID="gvEnfants" runat="server" CssClass="table table-striped table-bordered" 
                AutoGenerateColumns="False" DataKeyNames="Matricule,Num" 
                OnRowCommand="gvEnfants_RowCommand" OnRowDataBound="gvEnfants_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="Num" HeaderText="N°" ItemStyle-Width="50px" />
                    <asp:BoundField DataField="NomComplet" HeaderText="Nom Complet" />
                    <asp:TemplateField HeaderText="Sexe" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <span style='color: <%# Eval("Sexe").ToString() == "M" ? "#03A9F4" : "#E91E63" %>'>
                                <%# Eval("Sexe").ToString() == "M" ? "♂" : "♀" %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DateNaissance" HeaderText="Date de Naissance" 
                        DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="120px" />
                    <asp:BoundField DataField="LieuNaissance" HeaderText="Lieu de Naissance" />
                    <asp:BoundField DataField="ScolariseLibelle" HeaderText="Scolarisé" ItemStyle-Width="80px" />
                    <asp:BoundField DataField="Age" HeaderText="Âge" ItemStyle-Width="60px" />
                    <asp:BoundField DataField="Matricule" HeaderText="Parent (Agent)" ItemStyle-Width="120px" />
                    <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnView" runat="server" CommandName="View" 
                                CommandArgument='<%# Eval("Matricule") + "," + Eval("Num") %>' 
                                CssClass="btn-action btn-view" ToolTip="Voir détails">
                                👁
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" 
                                CommandArgument='<%# Eval("Matricule") + "," + Eval("Num") %>' 
                                CssClass="btn-action btn-edit" ToolTip="Modifier">
                                ✏
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" 
                                CommandArgument='<%# Eval("Matricule") + "," + Eval("Num") %>' 
                                CssClass="btn-action btn-delete" ToolTip="Supprimer"
                                OnClientClick="return confirm('Êtes-vous sûr de vouloir supprimer cet enfant ?');">
                                🗑
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div style="text-align: center; padding: 20px;">
                        <p>Aucun enfant trouvé.</p>
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>