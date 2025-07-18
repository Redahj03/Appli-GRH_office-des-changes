<%@ Page Title="Liste des Sanctions" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ListeSanctions.aspx.cs" Inherits="GestionRHv2.Pages.Sanctions.ListeSanctions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
        }
        
        .page-header {
            background: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            margin-bottom: 20px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        
        .page-title {
            font-size: 24px;
            color: #2c3e50;
            display: flex;
            align-items: center;
            gap: 10px;
        }
        
        .btn-primary {
            background: #3498db;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 5px;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            gap: 5px;
            transition: background 0.3s;
        }
        
        .btn-primary:hover {
            background: #2980b9;
        }
        
        .data-table {
            background: white;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            overflow: hidden;
        }
        
        .gridview {
            width: 100%;
            border-collapse: collapse;
        }
        
        .gridview th {
            background: #f8f9fa;
            padding: 15px;
            text-align: left;
            font-weight: 600;
            color: #495057;
            border-bottom: 2px solid #dee2e6;
        }
        
        .gridview td {
            padding: 12px 15px;
            border-bottom: 1px solid #f1f3f4;
        }
        
        .gridview tr:hover {
            background: #f8f9fa;
        }
        
        .action-buttons {
            display: flex;
            gap: 5px;
        }
        
        .btn-action {
            padding: 5px 10px;
            border: none;
            border-radius: 3px;
            cursor: pointer;
            font-size: 12px;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            gap: 3px;
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
        
        .alert {
            padding: 15px;
            margin-bottom: 20px;
            border-radius: 5px;
            display: flex;
            align-items: center;
            gap: 10px;
        }
        
        .alert-success {
            background: #d4edda;
            color: #155724;
            border: 1px solid #c3e6cb;
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
                <i class="fas fa-gavel"></i>
                Liste des Types de Sanctions
            </h1>
            <a href="AjouterSanction.aspx" class="btn-primary">
                <i class="fas fa-plus"></i>
                Ajouter un Type
            </a>
        </div>
        
        <div class="data-table">
            <asp:GridView ID="GridViewSanctions" runat="server" 
                CssClass="gridview" 
                AutoGenerateColumns="False"
                DataKeyNames="code_SC"
                OnRowCommand="GridViewSanctions_RowCommand"
                EmptyDataText="Aucun type de sanction trouvé.">
                <Columns>
                    <asp:BoundField DataField="code_SC" HeaderText="Code" ItemStyle-Width="100px" />
                    <asp:BoundField DataField="libelle_sc" HeaderText="Libellé" />
                    <asp:BoundField DataField="jours" HeaderText="Durée (jours)" ItemStyle-Width="150px" />
                    <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <div class="action-buttons">
                                <a href='ModifierSanction.aspx?code=<%# Eval("code_SC") %>' class="btn-action btn-edit">
                                    <i class="fas fa-edit"></i>
                                    Modifier
                                </a>
                                <asp:LinkButton ID="btnDelete" runat="server" 
                                    CommandName="Delete" 
                                    CommandArgument='<%# Eval("code_SC") %>'
                                    CssClass="btn-action btn-delete"
                                    OnClientClick="return confirm('Êtes-vous sûr de vouloir supprimer ce type de sanction ?');">
                                    <i class="fas fa-trash"></i>
                                    Supprimer
                                </asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>