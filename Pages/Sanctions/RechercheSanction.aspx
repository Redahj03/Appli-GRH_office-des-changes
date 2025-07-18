<%@ Page Title="Rechercher des Sanctions" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="RechercheSanction.aspx.cs" Inherits="GestionRHv2.Pages.Sanctions.RechercheSanction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
        }
        
        .search-card {
            background: white;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            padding: 20px;
            margin-bottom: 20px;
        }
        
        .search-form {
            display: flex;
            gap: 10px;
            align-items: flex-end;
        }
        
        .form-group {
            flex: 1;
        }
        
        .form-label {
            display: block;
            margin-bottom: 8px;
            font-weight: 600;
            color: #495057;
        }
        
        .form-control {
            width: 100%;
            padding: 10px 15px;
            border: 1px solid #ddd;
            border-radius: 5px;
            font-size: 14px;
        }
        
        .btn-search {
            background: #3498db;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            display: inline-flex;
            align-items: center;
            gap: 5px;
        }
        
        .btn-search:hover {
            background: #2980b9;
        }
        
        .results-section {
            margin-top: 20px;
        }
        
        .section-title {
            font-size: 20px;
            color: #2c3e50;
            margin-bottom: 15px;
            display: flex;
            align-items: center;
            gap: 10px;
        }
        
        .data-table {
            background: white;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            overflow: hidden;
            margin-bottom: 30px;
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
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="search-card">
            <h1 class="section-title">
                <i class="fas fa-search"></i>
                Rechercher des Sanctions
            </h1>
            
            <div class="search-form">
                <div class="form-group">
                    <label class="form-label">Rechercher par</label>
                    <asp:DropDownList ID="ddlSearchType" runat="server" CssClass="form-control" 
                        AutoPostBack="true" OnSelectedIndexChanged="ddlSearchType_SelectedIndexChanged">
                        <asp:ListItem Text="Agent (Nom/Matricule)" Value="agent" Selected="True" />
                        <asp:ListItem Text="Type de Sanction" Value="type" />
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">
                        <asp:Label ID="lblSearchLabel" runat="server" Text="Nom ou Matricule"></asp:Label>
                    </label>
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" 
                        placeholder="Entrez votre recherche..." Visible="true"></asp:TextBox>
                    <asp:DropDownList ID="ddlTypeSanction" runat="server" CssClass="form-control" 
                        Visible="false">
                    </asp:DropDownList>
                </div>
                
                <asp:Button ID="btnSearch" runat="server" Text="Rechercher" 
                    CssClass="btn-search" OnClick="btnSearch_Click">
                </asp:Button>
            </div>
        </div>
        
        <div class="results-section" runat="server" id="divResults" visible="false">
            <h2 class="section-title">
                <i class="fas fa-list"></i>
                Résultats de la recherche
            </h2>
            
            <div class="data-table">
                <asp:GridView ID="GridViewResults" runat="server" 
                    CssClass="gridview" 
                    AutoGenerateColumns="False"
                    EmptyDataText="Aucun résultat trouvé.">
                    <Columns>
                        <asp:BoundField DataField="matricule" HeaderText="Matricule" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="NomComplet" HeaderText="Agent" />
                        <asp:BoundField DataField="TypeSanction" HeaderText="Type de Sanction" />
                        <asp:BoundField DataField="date" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="120px" />
                        <asp:BoundField DataField="duree" HeaderText="Durée (jours)" ItemStyle-Width="120px" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>