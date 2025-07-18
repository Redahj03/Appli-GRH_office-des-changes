<%@ Page Title="Ajouter une Formation" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AjouterFormation.aspx.cs" Inherits="GestionRHv2.Pages.Formation.AjouterFormation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .container {
            max-width: 900px;
            margin: 0 auto;
            padding: 20px;
        }
        
        .page-header {
            background: white;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            margin-bottom: 30px;
        }
        
        .page-title {
            font-size: 2rem;
            color: #2c3e50;
            margin: 0 0 10px 0;
            display: flex;
            align-items: center;
            gap: 10px;
        }
        
        .breadcrumb {
            display: flex;
            gap: 10px;
            color: #7f8c8d;
            font-size: 14px;
        }
        
        .breadcrumb a {
            color: #3498db;
            text-decoration: none;
        }
        
        .breadcrumb a:hover {
            text-decoration: underline;
        }
        
        .form-container {
            background: white;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }
        
        .form-row {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 20px;
            margin-bottom: 20px;
        }
        
        .form-row.full-width {
            grid-template-columns: 1fr;
        }
        
        .form-group {
            display: flex;
            flex-direction: column;
        }
        
        .form-group label {
            margin-bottom: 8px;
            color: #5a6c7d;
            font-weight: 600;
            font-size: 14px;
        }
        
        .form-group label .required {
            color: #e74c3c;
            margin-left: 3px;
        }
        
        .form-control {
            padding: 10px 15px;
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            font-size: 15px;
            transition: border-color 0.2s;
        }
        
        .form-control:focus {
            outline: none;
            border-color: #3498db;
            box-shadow: 0 0 0 3px rgba(52, 152, 219, 0.1);
        }
        
        textarea.form-control {
            min-height: 120px;
            resize: vertical;
        }
        
        .info-box {
            background: #e3f2fd;
            border-left: 4px solid #2196f3;
            padding: 15px;
            margin-bottom: 20px;
            border-radius: 5px;
            display: flex;
            align-items: center;
            gap: 10px;
        }
        
        .info-box i {
            color: #2196f3;
            font-size: 20px;
        }
        
        .info-box p {
            margin: 0;
            color: #1976d2;
            font-size: 14px;
        }
        
        .form-actions {
            display: flex;
            gap: 10px;
            margin-top: 30px;
            padding-top: 20px;
            border-top: 1px solid #e0e0e0;
        }
        
        .btn {
            padding: 12px 25px;
            border: none;
            border-radius: 8px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            gap: 8px;
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
        }
        
        .validation-error {
            color: #e74c3c;
            font-size: 13px;
            margin-top: 5px;
            display: block;
        }
        
        .alert {
            padding: 15px 20px;
            border-radius: 8px;
            margin-bottom: 20px;
            display: flex;
            align-items: center;
            gap: 10px;
        }
        
        .alert-success {
            background: #d4edda;
            color: #155724;
            border: 1px solid #c3e6cb;
        }
        
        .alert-danger {
            background: #f8d7da;
            color: #721c24;
            border: 1px solid #f5c6cb;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <!-- En-tête de la page -->
        <div class="page-header">
            <h1 class="page-title">
                <i class="fas fa-plus-circle"></i> Nouvelle Formation
            </h1>
            <div class="breadcrumb">
                <a href="ListeFormations.aspx">Formations</a>
                <span>/</span>
                <span>Nouvelle Formation</span>
            </div>
        </div>
        
        <!-- Messages d'alerte -->
        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert">
            <i class="fas fa-info-circle"></i>
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </asp:Panel>
        
        <!-- Formulaire -->
        <div class="form-container">
            <div class="info-box">
                <i class="fas fa-info-circle"></i>
                <p>Le numéro d'objet sera généré automatiquement. Remplissez tous les champs obligatoires (*) pour créer une nouvelle formation.</p>
            </div>
            
            <div class="form-row">
                <div class="form-group">
                    <label>N° Objet (automatique)</label>
                    <asp:TextBox ID="txtObjet" runat="server" CssClass="form-control" ReadOnly="true" 
                                BackColor="#f8f9fa"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label>Date de Formation <span class="required">*</span></label>
                    <asp:TextBox ID="txtDateFormation" runat="server" CssClass="form-control" 
                                TextMode="Date" required="required"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDate" runat="server" 
                                               ControlToValidate="txtDateFormation" 
                                               ErrorMessage="La date est obligatoire" 
                                               CssClass="validation-error" 
                                               Display="Dynamic" />
                </div>
            </div>
            
            <div class="form-row">
                <div class="form-group">
                    <label>Lieu <span class="required">*</span></label>
                    <asp:TextBox ID="txtLieu" runat="server" CssClass="form-control" 
                                placeholder="Ex: Salle de conférence A" required="required"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvLieu" runat="server" 
                                               ControlToValidate="txtLieu" 
                                               ErrorMessage="Le lieu est obligatoire" 
                                               CssClass="validation-error" 
                                               Display="Dynamic" />
                </div>
                
                <div class="form-group">
                    <label>Durée</label>
                    <asp:TextBox ID="txtDuree" runat="server" CssClass="form-control" 
                                placeholder="Ex: 2 jours, 8 heures"></asp:TextBox>
                </div>
            </div>
            
            <div class="form-row">
                <div class="form-group">
                    <label>Effectif Maximum</label>
                    <asp:TextBox ID="txtEffectif" runat="server" CssClass="form-control" 
                                TextMode="Number" placeholder="Laisser vide pour illimité"></asp:TextBox>
                    <asp:RangeValidator ID="rvEffectif" runat="server" 
                                       ControlToValidate="txtEffectif" 
                                       MinimumValue="1" 
                                       MaximumValue="999" 
                                       Type="Integer" 
                                       ErrorMessage="L'effectif doit être entre 1 et 999" 
                                       CssClass="validation-error" 
                                       Display="Dynamic" />
                </div>
                
                <div class="form-group">
                    <!-- Espace pour l'alignement -->
                </div>
            </div>
            
            <div class="form-row full-width">
                <div class="form-group">
                    <label>Contenu de la Formation</label>
                    <asp:TextBox ID="txtContenu" runat="server" CssClass="form-control" 
                                TextMode="MultiLine" 
                                placeholder="Décrivez le programme et les objectifs de la formation..."></asp:TextBox>
                </div>
            </div>
            
            <div class="form-actions">
                <asp:Button ID="btnEnregistrer" runat="server" Text="Enregistrer la Formation" 
                           CssClass="btn btn-primary" OnClick="btnEnregistrer_Click">
                </asp:Button>
                
                <asp:Button ID="btnAnnuler" runat="server" Text="Annuler" 
                           CssClass="btn btn-secondary" OnClick="btnAnnuler_Click" 
                           CausesValidation="false">
                </asp:Button>
            </div>
        </div>
    </div>
</asp:Content>