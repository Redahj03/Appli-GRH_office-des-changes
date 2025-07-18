<%@ Page Title="Recherche Agent" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="RechercheAgent.aspx.cs" Inherits="GestionRHv2.Pages.Agents.RechercheAgent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }
        
        /* Container principal */
        .main-container {
            max-width: 1400px;
            margin: 0 auto;
            padding: 20px;
        }
        
        /* Carte de recherche */
        .search-card {
            background: white;
            border-radius: 20px;
            box-shadow: 0 10px 40px rgba(0, 0, 0, 0.08);
            margin-bottom: 30px;
            overflow: hidden;
        }
        
        .search-header {
            background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
            padding: 25px 30px;
        }
        
        .search-header h2 {
            color: #2c3e50;
            font-size: 1.5rem;
            font-weight: 400;
        }
        
        .search-body {
            padding: 30px;
        }
        
        .search-grid {
            display: grid;
            grid-template-columns: 1fr 2fr auto;
            gap: 25px;
            align-items: end;
        }
        
        .form-group label {
            display: block;
            margin-bottom: 10px;
            color: #5a6c7d;
            font-weight: 500;
            font-size: 0.95rem;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }
        
        .form-control {
            width: 100%;
            padding: 15px 20px;
            border: 2px solid #e1e8ed;
            border-radius: 12px;
            font-size: 16px;
            transition: all 0.3s ease;
            background: #f8f9fa;
        }
        
        .form-control:focus {
            outline: none;
            border-color: #3498db;
            background: white;
            box-shadow: 0 0 0 4px rgba(52, 152, 219, 0.1);
        }
        
        .btn-search {
            background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
            color: white;
            border: none;
            padding: 16px 40px;
            border-radius: 12px;
            font-size: 16px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
            text-transform: uppercase;
            letter-spacing: 1px;
        }
        
        .btn-search:hover {
            transform: translateY(-2px);
            box-shadow: 0 8px 20px rgba(52, 152, 219, 0.3);
        }
        
        /* Messages */
        .alert {
            padding: 20px 25px;
            margin: 20px 0;
            border-radius: 12px;
            font-weight: 500;
            display: flex;
            align-items: center;
            animation: slideIn 0.3s ease;
        }
        
        .alert i {
            margin-right: 12px;
            font-size: 1.3rem;
        }
        
        .alert-success {
            background: linear-gradient(135deg, #d4edda 0%, #c3e6cb 100%);
            color: #155724;
        }
        
        .alert-error {
            background: linear-gradient(135deg, #f8d7da 0%, #f5c6cb 100%);
            color: #721c24;
        }
        
        /* Carte résultats */
        .result-card {
            background: white;
            border-radius: 20px;
            box-shadow: 0 10px 40px rgba(0, 0, 0, 0.08);
            overflow: hidden;
            animation: fadeIn 0.5s ease;
        }
        
        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(20px); }
            to { opacity: 1; transform: translateY(0); }
        }
        
        @keyframes slideIn {
            from { opacity: 0; transform: translateY(-10px); }
            to { opacity: 1; transform: translateY(0); }
        }
        
        .info-section {
            border-bottom: 1px solid #f0f2f5;
        }
        
        .info-section:last-child {
            border-bottom: none;
        }
        
        .info-header {
            background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
            padding: 20px 30px;
            font-size: 1.2rem;
            font-weight: 500;
            color: #2c3e50;
            display: flex;
            align-items: center;
        }
        
        .info-header i {
            margin-right: 12px;
            font-size: 1.4rem;
            color: #3498db;
        }
        
        .info-body {
            padding: 25px 30px;
        }
        
        .info-row {
            display: grid;
            grid-template-columns: 250px 1fr;
            padding: 15px 0;
            border-bottom: 1px solid #f8f9fa;
            transition: background 0.2s ease;
        }
        
        .info-row:hover {
            background: #f8f9fa;
            margin: 0 -30px;
            padding: 15px 30px;
        }
        
        .info-row:last-child {
            border-bottom: none;
        }
        
        .info-label {
            font-weight: 600;
            color: #5a6c7d;
            display: flex;
            align-items: center;
        }
        
        .info-label i {
            margin-right: 10px;
            color: #95a5a6;
            font-size: 0.9rem;
        }
        
        .info-value {
            color: #2c3e50;
        }
        
        .empty-value {
            color: #bdc3c7;
            font-style: italic;
        }
        
        @media (max-width: 768px) {
            .search-grid {
                grid-template-columns: 1fr;
                gap: 20px;
            }
            
            .info-row {
                grid-template-columns: 1fr;
                gap: 8px;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main-container">
        <!-- Carte de recherche -->
        <div class="search-card">
            <div class="search-header">
                <h2><i class="fas fa-search"></i>Recherche d'Agent</h2>
            </div>
            <div class="search-body">
                <div class="search-grid">
                    <div class="form-group">
                        <label>Type de recherche</label>
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="ID">Par Matricule</asp:ListItem>
                            <asp:ListItem Value="NOM">Par Nom/Prénom</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    
                    <div class="form-group">
                        <label>Valeur recherchée</label>
                        <asp:TextBox ID="txtRecherche" runat="server" CssClass="form-control" 
                                    placeholder="Entrez votre recherche..."></asp:TextBox>
                    </div>
                    
                    <div class="form-group">
                        <asp:Button ID="btnRechercher" runat="server" Text="Rechercher" 
                                   CssClass="btn-search" OnClick="btnRechercher_Click" />
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Messages -->
        <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
        
        <!-- Résultats -->
        <asp:Panel ID="pnlResultat" runat="server" Visible="false" CssClass="result-card">
            <!-- Informations personnelles -->
            <div class="info-section">
                <div class="info-header">
                    <i class="fas fa-user-circle"></i>
                    Informations Personnelles
                </div>
                <div class="info-body">
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-id-badge"></i>Matricule</div>
                        <asp:Label ID="lblID" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-user"></i>Nom</div>
                        <asp:Label ID="lblNom" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-user"></i>Prénom</div>
                        <asp:Label ID="lblPrenom" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-id-card"></i>CIN</div>
                        <asp:Label ID="lblCIN" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-calendar"></i>Date de naissance</div>
                        <asp:Label ID="lblDateNaiss" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-map-marker-alt"></i>Lieu de naissance</div>
                        <asp:Label ID="lblLieuNaiss" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-venus-mars"></i>Sexe</div>
                        <asp:Label ID="lblSexe" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-heart"></i>Situation familiale</div>
                        <asp:Label ID="lblSituationFam" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                </div>
            </div>
            
            <!-- Informations professionnelles -->
            <div class="info-section">
                <div class="info-header">
                    <i class="fas fa-briefcase"></i>
                    Informations Professionnelles
                </div>
                <div class="info-body">
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-barcode"></i>PPR (Pecul)</div>
                        <asp:Label ID="lblPPR" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-calendar-check"></i>Date de recrutement</div>
                        <asp:Label ID="lblDateRecrutement" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-star"></i>Grade</div>
                        <asp:Label ID="lblGrade" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-user-tie"></i>Fonction</div>
                        <asp:Label ID="lblFonction" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-building"></i>Lieu d'affectation</div>
                        <asp:Label ID="lblLieuAffect" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-piggy-bank"></i>Banque (BQV)</div>
                        <asp:Label ID="lblBqv" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-university"></i>Agence (BQE)</div>
                        <asp:Label ID="lblBqe" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-credit-card"></i>Compte (CPT)</div>
                        <asp:Label ID="lblCpt" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-walking"></i>PAS</div>
                        <asp:Label ID="lblPas" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-user-clock"></i>Retraité</div>
                        <asp:Label ID="lblRetrait" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-clipboard-list"></i>Type de retraite</div>
                        <asp:Label ID="lblTypeRet" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-home"></i>Prêt logement</div>
                        <asp:Label ID="lblPretLog" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                </div>
            </div>
            
            <!-- Informations de contact -->
            <div class="info-section">
                <div class="info-header">
                    <i class="fas fa-address-book"></i>
                    Informations de Contact
                </div>
                <div class="info-body">
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-phone"></i>Téléphone</div>
                        <asp:Label ID="lblTel1" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-flag"></i>National</div>
                        <asp:Label ID="lblTel2" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-envelope"></i>Email</div>
                        <asp:Label ID="lblEmail" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-home"></i>Adresse</div>
                        <asp:Label ID="lblAdresse" runat="server" CssClass="info-value"></asp:Label>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>