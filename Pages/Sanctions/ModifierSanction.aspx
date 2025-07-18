<%@ Page Title="Modifier une Sanction" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ModifierSanction.aspx.cs" Inherits="GestionRHv2.Pages.Sanctions.ModifierSanction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Mêmes styles que AjouterSanction.aspx */
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
        
        .form-control[readonly] {
            background-color: #f8f9fa;
            cursor: not-allowed;
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
                    <i class="fas fa-edit"></i>
                    Modifier un Type de Sanction
                </h1>
            </div>
            
            <div class="form-group">
                <label class="form-label">
                    Code de la Sanction
                </label>
                <asp:TextBox ID="txtCodeSC" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
            
            <div class="form-group">
                <label class="form-label">
                    Libellé de la Sanction <span class="required">*</span>
                </label>
                <asp:TextBox ID="txtLibelle" runat="server" CssClass="form-control" 
                    placeholder="Ex: AVERTISSEMENT VERBAL" MaxLength="200"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvLibelle" runat="server" 
                    ControlToValidate="txtLibelle" 
                    ErrorMessage="Le libellé est obligatoire" 
                    Display="Dynamic" 
                    CssClass="text-danger" />
            </div>
            
            <div class="form-group">
                <label class="form-label">
                    Durée par défaut (jours)
                </label>
                <asp:TextBox ID="txtJours" runat="server" CssClass="form-control" 
                    placeholder="0 pour indéterminée, -1 pour permanente"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revJours" runat="server"
                    ControlToValidate="txtJours"
                    ValidationExpression="^-?\d+$"
                    ErrorMessage="La durée doit être un nombre entier"
                    Display="Dynamic"
                    CssClass="text-danger" />
            </div>
            
            <div class="form-actions">
                <asp:Button ID="btnEnregistrer" runat="server" Text="Enregistrer les modifications" 
                    CssClass="btn btn-primary" OnClick="btnEnregistrer_Click">
                </asp:Button>
                <a href="ListeSanctions.aspx" class="btn btn-secondary">
                    <i class="fas fa-arrow-left"></i>
                    Retour
                </a>
            </div>
        </div>
    </div>
</asp:Content>