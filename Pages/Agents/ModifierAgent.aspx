<%@ Page Title="Modifier un Agent" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ModifierAgent.aspx.cs" Inherits="GestionRHv2.Pages.Agents.ModifierAgent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Même style que AjouterAgent.aspx */
        .container {
            max-width: 1200px;
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
            font-size: 1.5rem;
            color: #2c3e50;
            margin-bottom: 25px;
            padding-bottom: 15px;
            border-bottom: 2px solid #e9ecef;
            display: flex;
            align-items: center;
        }
        
        .form-header i {
            margin-right: 10px;
            color: #3498db;
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
            padding: 10px 15px;
            border: 2px solid #e0e0e0;
            border-radius: 6px;
            font-size: 16px;
            transition: border-color 0.3s;
        }
        
        .form-control:focus {
            outline: none;
            border-color: #3498db;
            box-shadow: 0 0 0 3px rgba(52, 152, 219, 0.1);
        }
        
        .form-control[readonly] {
            background-color: #f5f5f5;
            cursor: not-allowed;
        }
        
        .form-select {
            width: 100%;
            padding: 10px 15px;
            border: 2px solid #e0e0e0;
            border-radius: 6px;
            font-size: 16px;
            background: white;
        }
        
        .btn-group {
            display: flex;
            gap: 15px;
            margin-top: 30px;
            padding-top: 30px;
            border-top: 2px solid #e9ecef;
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
        }
        
        .btn i {
            margin-right: 8px;
        }
        
        .btn-primary {
            background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
            color: white;
        }
        
        .btn-primary:hover {
            transform: translateY(-2px);
            box-shadow: 0 5px 15px rgba(52, 152, 219, 0.3);
        }
        
        .btn-secondary {
            background: #95a5a6;
            color: white;
        }
        
        .btn-secondary:hover {
            background: #7f8c8d;
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
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert">
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </asp:Panel>
        
        <div class="form-card">
            <h2 class="form-header">
                <i class="fas fa-user-edit"></i>
                Modifier un Agent
            </h2>
            
            <!-- Informations Personnelles -->
            <h3 class="form-header">
                <i class="fas fa-user"></i>
                Informations Personnelles
            </h3>
            
            <div class="form-grid">
                <div class="form-group">
                    <label class="form-label">
                        Matricule <span class="required">*</span>
                    </label>
                    <asp:TextBox ID="txtMatricule" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">
                        CIN <span class="required">*</span>
                    </label>
                    <asp:TextBox ID="txtCIN" runat="server" CssClass="form-control" Required="true"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">
                        Nom <span class="required">*</span>
                    </label>
                    <asp:TextBox ID="txtNom" runat="server" CssClass="form-control" Required="true"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">
                        Prénom <span class="required">*</span>
                    </label>
                    <asp:TextBox ID="txtPrenom" runat="server" CssClass="form-control" Required="true"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Date de naissance</label>
                    <asp:TextBox ID="txtDateNaissance" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Lieu de naissance</label>
                    <asp:TextBox ID="txtLieuNaissance" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Sexe</label>
                    <asp:DropDownList ID="ddlSexe" runat="server" CssClass="form-select">
                        <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>
                        <asp:ListItem Value="M">Masculin</asp:ListItem>
                        <asp:ListItem Value="F">Féminin</asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Situation familiale</label>
                    <asp:DropDownList ID="ddlSituation" runat="server" CssClass="form-select">
                        <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>
                        <asp:ListItem Value="Célibataire">Célibataire</asp:ListItem>
                        <asp:ListItem Value="Marié(e)">Marié(e)</asp:ListItem>
                        <asp:ListItem Value="Divorcé(e)">Divorcé(e)</asp:ListItem>
                        <asp:ListItem Value="Veuf(ve)">Veuf(ve)</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            
            <!-- Informations Professionnelles -->
            <h3 class="form-header">
                <i class="fas fa-briefcase"></i>
                Informations Professionnelles
            </h3>
            
            <div class="form-grid">
                <div class="form-group">
                    <label class="form-label">Date de recrutement</label>
                    <asp:TextBox ID="txtDateRecrutement" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">PPR (Pecul)</label>
                    <asp:TextBox ID="txtPecul" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Ville d'affectation</label>
                    <asp:TextBox ID="txtVille" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">PAS</label>
                    <asp:TextBox ID="txtPas" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Retraité</label>
                    <asp:DropDownList ID="ddlRetrait" runat="server" CssClass="form-select">
                        <asp:ListItem Value="0">Non</asp:ListItem>
                        <asp:ListItem Value="1">Oui</asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Type de retraite</label>
                    <asp:TextBox ID="txtTypeRet" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Prêt logement</label>
                    <asp:TextBox ID="txtPretLog" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            
            <!-- Informations Bancaires -->
            <h3 class="form-header">
                <i class="fas fa-university"></i>
                Informations Bancaires
            </h3>
            
            <div class="form-grid">
                <div class="form-group">
                    <label class="form-label">Banque (BQV)</label>
                    <asp:TextBox ID="txtBqv" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">Agence (BQE)</label>
                    <asp:TextBox ID="txtBqe" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">N° Compte (CPT)</label>
                    <asp:TextBox ID="txtCpt" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            
            <!-- Contact -->
            <h3 class="form-header">
                <i class="fas fa-phone"></i>
                Contact
            </h3>
            
            <div class="form-grid">
                <div class="form-group">
                    <label class="form-label">Téléphone</label>
                    <asp:TextBox ID="txtTel" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label class="form-label">National</label>
                    <asp:TextBox ID="txtNational" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                
                <div class="form-group full-width">
                    <label class="form-label">Adresse</label>
                    <asp:TextBox ID="txtAdresse" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                </div>
            </div>
            
            <!-- Boutons -->
            <div class="btn-group">
                <asp:Button ID="btnEnregistrer" runat="server" Text="Enregistrer les modifications" 
                           CssClass="btn btn-primary" OnClick="btnEnregistrer_Click" />
                
                <asp:Button ID="btnAnnuler" runat="server" Text="Annuler" 
                           CssClass="btn btn-secondary" OnClick="btnAnnuler_Click" />
            </div>
        </div>
    </div>
</asp:Content>