<%@ Page Title="Ajouter une Fonction d'Agent" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AjouterAgentFonction.aspx.cs" Inherits="GestionRHv2.Pages.AgentsFonctions.AjouterAgentFonction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .container {
            max-width: 800px;
            margin: 0 auto;
            padding: 20px;
        }
        
        .form-card {
            background: white;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            padding: 30px;
            margin-bottom: 20px;
        }
        
        .form-header {
            text-align: center;
            margin-bottom: 30px;
        }
        
        .form-title {
            font-size: 2rem;
            color: #2c3e50;
            margin: 0;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        
        .form-title i {
            margin-right: 15px;
            color: #27ae60;
        }
        
        .form-grid {
            display: grid;
            grid-template-columns: repeat(2, 1fr);
            gap: 20px;
        }
        
        .form-group {
            margin-bottom: 20px;
        }
        
        .form-group.full-width {
            grid-column: span 2;
        }
        
        .form-label {
            display: block;
            margin-bottom: 8px;
            font-weight: 600;
            color: #5a6c7d;
        }
        
        .form-label .required {
            color: #e74c3c;
        }
        
        .form-control {
            width: 100%;
            padding: 12px 15px;
            border: 2px solid #e0e0e0;
            border-radius: 6px;
            font-size: 16px;
            transition: border-color 0.3s;
        }
        
        .form-control:focus {
            outline: none;
            border-color: #27ae60;
            box-shadow: 0 0 0 3px rgba(39, 174, 96, 0.1);
        }
        
        .form-control[readonly] {
            background-color: #f8f9fa;
            cursor: not-allowed;
        }
        
        .btn-group {
            display: flex;
            gap: 15px;
            margin-top: 30px;
            padding-top: 30px;
            border-top: 2px solid #e9ecef;
            justify-content: center;
        }
        
        .btn {
            padding: 12px 30px;
            border: none;
            border-radius: 6px;
            font-size: 16px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s;
            display: inline-flex;
            align-items: center;
            text-decoration: none;
        }
        
        .btn i {
            margin-right: 8px;
        }
        
        .btn-primary {
            background: linear-gradient(135deg, #27ae60 0%, #229954 100%);
            color: white;
        }
        
        .btn-primary:hover {
            transform: translateY(-2px);
            box-shadow: 0 5px 15px rgba(39, 174, 96, 0.3);
        }
        
        .btn-secondary {
            background: #95a5a6;
            color: white;
        }
        
        .btn-secondary:hover {
            background: #7f8c8d;
            color: white;
            text-decoration: none;
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
            font-size: 1.2rem;
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
        
        .breadcrumb {
            background: white;
            padding: 15px 20px;
            border-radius: 6px;
            margin-bottom: 20px;
            box-shadow: 0 1px 3px rgba(0,0,0,0.1);
        }
        
        .breadcrumb a {
            color: #3498db;
            text-decoration: none;
        }
        
        .breadcrumb a:hover {
            text-decoration: underline;
        }
        
        .info-text {
            font-size: 0.9rem;
            color: #7f8c8d;
            margin-top: 5px;
        }
        
        .info-box {
            background: #e8f5e9;
            border-left: 4px solid #4caf50;
            padding: 15px;
            margin-bottom: 20px;
            border-radius: 4px;
        }
        
        .info-box i {
            color: #4caf50;
            margin-right: 8px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="breadcrumb">
            <a href="ListeAgentsFonctions.aspx">🎯 Fonctions d'Agents</a> / ➕ Ajouter une Fonction
        </div>
        
        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert">
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </asp:Panel>
        
        <div class="form-card">
            <div class="form-header">
                <h1 class="form-title">
                    <i class="fas fa-plus-circle"></i>
                    Ajouter une Fonction d'Agent
                </h1>
            </div>
            
            <div class="info-box">
                <i class="fas fa-info-circle"></i>
                Le numéro d'ordre sera généré automatiquement
            </div>
            
            <div class="form-grid">
                <div class="form-group">
                    <label class="form-label">
                        Numéro d'Ordre
                    </label>
                    <asp:TextBox ID="txtNumOrd" runat="server" CssClass="form-control" 
                                ReadOnly="true" placeholder="Auto-généré"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">
                        Agent <span class="required">*</span>
                    </label>
                    <asp:DropDownList ID="ddlAgent" runat="server" CssClass="form-control" 
                                     Required="true" AutoPostBack="true" 
                                     OnSelectedIndexChanged="ddlAgent_SelectedIndexChanged">
                        <asp:ListItem Value="">-- Sélectionner un agent --</asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Matricule</label>
                    <asp:TextBox ID="txtMatricule" runat="server" CssClass="form-control" 
                                ReadOnly="true"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Nom</label>
                    <asp:TextBox ID="txtNom" runat="server" CssClass="form-control" 
                                ReadOnly="true"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Prénom</label>
                    <asp:TextBox ID="txtPrenom" runat="server" CssClass="form-control" 
                                ReadOnly="true"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">
                        Affectation <span class="required">*</span>
                    </label>
                    <asp:TextBox ID="txtAffectation" runat="server" CssClass="form-control" 
                                Required="true" MaxLength="255" 
                                placeholder="Ex: DEPARTEMENT DES OPERATIONS FINANCIERES"></asp:TextBox>
                </div>
                
                <div class="form-group full-width">
                    <label class="form-label">
                        Fonction <span class="required">*</span>
                    </label>
                    <asp:TextBox ID="txtFonction" runat="server" CssClass="form-control" 
                                Required="true" MaxLength="255" 
                                placeholder="Ex: Chef de Département, Agent d'étude, Inspecteur..."></asp:TextBox>
                </div>
            </div>
            
            <div class="btn-group">
                <asp:Button ID="btnEnregistrer" runat="server" Text="💾 Enregistrer" 
                           CssClass="btn btn-primary" OnClick="btnEnregistrer_Click" />
                
                <a href="ListeAgentsFonctions.aspx" class="btn btn-secondary">
                    <i class="fas fa-times"></i> Annuler
                </a>
            </div>
        </div>
    </div>
</asp:Content>