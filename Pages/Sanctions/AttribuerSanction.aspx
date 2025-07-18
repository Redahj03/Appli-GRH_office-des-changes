<%@ Page Title="Attribuer une Sanction" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AttribuerSanction.aspx.cs" Inherits="GestionRHv2.Pages.Sanctions.AttribuerSanction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .container {
            max-width: 800px;
            margin: 0 auto;
            padding: 20px;
        }
        
        .form-card {
            background: white;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            padding: 30px;
        }
        
        .form-header {
            margin-bottom: 30px;
            padding-bottom: 20px;
            border-bottom: 2px solid #f0f0f0;
        }
        
        .form-title {
            font-size: 24px;
            color: #2c3e50;
            display: flex;
            align-items: center;
            gap: 10px;
        }
        
        .form-group {
            margin-bottom: 20px;
        }
        
        .form-label {
            display: block;
            margin-bottom: 8px;
            font-weight: 600;
            color: #495057;
        }
        
        .form-label .required {
            color: #e74c3c;
        }
        
        .form-control {
            width: 100%;
            padding: 10px 15px;
            border: 1px solid #ddd;
            border-radius: 5px;
            font-size: 14px;
            transition: border-color 0.3s;
        }
        
        .form-control:focus {
            outline: none;
            border-color: #3498db;
            box-shadow: 0 0 0 3px rgba(52, 152, 219, 0.1);
        }
        
        .form-actions {
            display: flex;
            gap: 10px;
            margin-top: 30px;
            padding-top: 20px;
            border-top: 2px solid #f0f0f0;
        }
        
        .btn {
            padding: 10px 20px;
            border: none;
            border-radius: 5px;
            font-size: 14px;
            cursor: pointer;
            display: inline-flex;
            align-items: center;
            gap: 5px;
            text-decoration: none;
            transition: all 0.3s;
        }
        
        .btn-primary {
            background: #3498db;
            color: white;
        }
        
        .btn-primary:hover {
            background: #2980b9;
        }
        
        .btn-secondary {
            background: #95a5a6;
            color: white;
        }
        
        .btn-secondary:hover {
            background: #7f8c8d;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="form-card">
            <div class="form-header">
                <h1 class="form-title">
                    <i class="fas fa-user-slash"></i>
                    Attribuer une Sanction
                </h1>
            </div>
            
            <div class="form-group">
                <label class="form-label">
                    Agent <span class="required">*</span>
                </label>
                <asp:DropDownList ID="ddlAgent" runat="server" CssClass="form-control">
                    <asp:ListItem Text="-- Sélectionner un agent --" Value="" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvAgent" runat="server" 
                    ControlToValidate="ddlAgent" 
                    ErrorMessage="Veuillez sélectionner un agent" 
                    Display="Dynamic" 
                    CssClass="text-danger" />
            </div>
            
            <div class="form-group">
                <label class="form-label">
                    Type de Sanction <span class="required">*</span>
                </label>
                <asp:DropDownList ID="ddlTypeSanction" runat="server" CssClass="form-control">
                    <asp:ListItem Text="-- Sélectionner un type --" Value="" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvTypeSanction" runat="server" 
                    ControlToValidate="ddlTypeSanction" 
                    ErrorMessage="Veuillez sélectionner un type de sanction" 
                    Display="Dynamic" 
                    CssClass="text-danger" />
            </div>
            
            <div class="form-group">
                <label class="form-label">
                    Date de la Sanction <span class="required">*</span>
                </label>
                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" 
                    TextMode="Date"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDate" runat="server" 
                    ControlToValidate="txtDate" 
                    ErrorMessage="La date est obligatoire" 
                    Display="Dynamic" 
                    CssClass="text-danger" />
            </div>
            
            <div class="form-group">
                <label class="form-label">
                    Durée (jours) <span class="required">*</span>
                </label>
                <asp:TextBox ID="txtDuree" runat="server" CssClass="form-control" 
                    placeholder="Nombre de jours" Text="0"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDuree" runat="server" 
                    ControlToValidate="txtDuree" 
                    ErrorMessage="La durée est obligatoire" 
                    Display="Dynamic" 
                    CssClass="text-danger" />
                <asp:RegularExpressionValidator ID="revDuree" runat="server"
                    ControlToValidate="txtDuree"
                    ValidationExpression="^\d+$"
                    ErrorMessage="La durée doit être un nombre positif"
                    Display="Dynamic"
                    CssClass="text-danger" />
            </div>
            
            <div class="form-actions">
                <asp:Button ID="btnEnregistrer" runat="server" Text="Enregistrer" 
                    CssClass="btn btn-primary" OnClick="btnEnregistrer_Click">
                </asp:Button>
                <asp:Button ID="Button1" runat="server" Text="Enregistrer" OnClick="btnEnregistrer_Click" CssClass="btn btn-primary" />
<asp:Label  ID="lblErreur"     runat="server" CssClass="text-danger" />
                <a href="AgentsSanctions.aspx" class="btn btn-secondary">
                    <i class="fas fa-arrow-left"></i>
                    Retour
                </a>
            </div>
        </div>
    </div>
</asp:Content>