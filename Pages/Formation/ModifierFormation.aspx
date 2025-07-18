<%@ Page Title="Modifier une Formation" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ModifierFormation.aspx.cs" Inherits="GestionRHv2.Pages.Formation.ModifierFormation" %>

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
        
        .form-control[readonly] {
            background-color: #f8f9fa;
            cursor: not-allowed;
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
        
        .info-box.warning {
            background: #fff3cd;
            border-left-color: #ffc107;
        }
        
        .info-box i {
            font-size: 20px;
        }
        
        .info-box.info i {
            color: #2196f3;
        }
        
        .info-box.warning i {
            color: #f39c12;
        }
        
        .info-box p {
            margin: 0;
            font-size: 14px;
        }
        
        .info-box.info p {
            color: #1976d2;
        }
        
        .info-box.warning p {
            color: #856404;
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
            background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
            color: white;
        }
        
        .btn-primary:hover {
            transform: translateY(-2px);
            box-shadow: 0 5px 15px rgba(243, 156, 18, 0.3);
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
        
        /* Statistiques actuelles */
        .current-stats {
            background: #f8f9fa;
            padding: 20px;
            border-radius: 8px;
            margin-bottom: 20px;
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
            gap: 20px;
        }
        
        .stat-item {
            text-align: center;
        }
        
        .stat-value {
            font-size: 1.5rem;
            font-weight: bold;
            color: #2c3e50;
            margin-bottom: 5px;
        }
        
        .stat-label {
            color: #7f8c8d;
            font-size: 13px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <!-- En-tête de la page -->
        <div class="page-header">
            <h1 class="page-title">
                <i class="fas fa-edit"></i> Modifier la Formation
            </h1>
            <div class="breadcrumb">
                <a href="ListeFormations.aspx">Formations</a>
                <span>/</span>
                <span>Modifier</span>
            </div>
        </div>
        
        <!-- Messages d'alerte -->
        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert">
            <i class="fas fa-info-circle"></i>
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </asp:Panel>
        
        <!-- Formulaire -->
        <div class="form-container">
            <!-- Info box si formation a des inscrits -->
            <asp:Panel ID="pnlWarning" runat="server" Visible="false" CssClass="info-box warning">
                <i class="fas fa-exclamation-triangle"></i>
                <p>
                    <asp:Label ID="lblWarning" runat="server"></asp:Label>
                </p>
            </asp:Panel>
            
            <!-- Statistiques actuelles -->
            <div class="current-stats">
                <div class="stat-item">
                    <div class="stat-value">
                        <asp:Label ID="lblStatObjet" runat="server"></asp:Label>
                    </div>
                    <div class="stat-label">N° Objet</div>
                </div>
                <div class="stat-item">
                    <div class="stat-value">
                        <asp:Label ID="lblStatInscrits" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="stat-label">Inscrits actuels</div>
                </div>
                <div class="stat-item">
                    <div class="stat-value">
                        <asp:Label ID="lblStatStatut" runat="server"></asp:Label>
                    </div>
                    <div class="stat-label">Statut</div>
                </div>
            </div>
            
            <div class="form-row">
                <div class="form-group">
                    <label>N° Objet (non modifiable)</label>
                    <asp:TextBox ID="txtObjet" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
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
                    <asp:CustomValidator ID="CvEffectif" runat="server"
                                        ControlToValidate="txtEffectif"
                                        ErrorMessage="L'effectif ne peut pas être inférieur au nombre d'inscrits actuels"
                                        CssClass="validation-error"
                                        Display="Dynamic"
                                        OnServerValidate="CvEffectif_ServerValidate" />
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
            
            <div class="info-box info">
                <i class="fas fa-info-circle"></i>
                <p>Les modifications seront appliquées immédiatement. Les agents déjà inscrits resteront inscrits.</p>
            </div>
            
            <div class="form-actions">
                <asp:Button ID="btnEnregistrer" runat="server" Text="Enregistrer les modifications" 
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