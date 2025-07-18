<%@ Page Title="Sanctions des Agents" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AgentsSanctions.aspx.cs" Inherits="GestionRHv2.Pages.Sanctions.AgentsSanctions" %>

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
        
        .btn-delete {
            background: #e74c3c;
            color: white;
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
        
        .btn-delete:hover {
            background: #c0392b;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="page-header">
            <h1 class="page-title">
                <i class="fas fa-users-slash"></i>
                Sanctions des Agents
            </h1>
            <a href="AttribuerSanction.aspx" class="btn-primary">
                <i class="fas fa-plus"></i>
                Attribuer une Sanction
            </a>
        </div>
        
        <div class="data-table">
            <asp:GridView ID="GridViewSanctionsAgents" runat="server" 
                CssClass="gridview" 
                AutoGenerateColumns="False"
                DataKeyNames="ID"
                OnRowCommand="GridViewSanctionsAgents_RowCommand"
                EmptyDataText="Aucune sanction attribuée.">
                <Columns>
                    <asp:BoundField DataField="matricule" HeaderText="Matricule" ItemStyle-Width="100px" />
                    <asp:BoundField DataField="NomComplet" HeaderText="Agent" />
                    <asp:BoundField DataField="TypeSanction" HeaderText="Type de Sanction" />
                    <asp:BoundField DataField="date" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="120px" />
                    <asp:BoundField DataField="duree" HeaderText="Durée (jours)" ItemStyle-Width="120px" />
                    <asp:TemplateField HeaderText="Actions" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnDelete" runat="server" 
                                CommandName="Delete" 
                                CommandArgument='<%# Eval("ID") %>'
                                CssClass="btn-delete"
                                OnClientClick="return confirm('Êtes-vous sûr de vouloir supprimer cette sanction ?');">
                                <i class="fas fa-trash"></i>
                                Supprimer
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>