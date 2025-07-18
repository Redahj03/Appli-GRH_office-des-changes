<%@ Page Title="Détails du Conjoint" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="DetailsConjoint.aspx.cs" Inherits="GestionRHv2.Pages.Conjoints.DetailsConjoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .details-container {
            max-width: 1000px;
            margin: 0 auto;
            padding: 20px;
        }

        .page-header {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 30px;
            border-radius: 15px;
            margin-bottom: 30px;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
        }

        .page-header h1 {
            font-size: 2.5em;
            margin: 0;
        }

        .page-header p {
            margin-top: 10px;
            opacity: 0.9;
            font-size: 1.1em;
        }

        .info-cards {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
            gap: 25px;
            margin-bottom: 30px;
        }

        .info-card {
            background: white;
            border-radius: 15px;
            padding: 30px;
            box-shadow: 0 5px 20px rgba(0, 0, 0, 0.08);
            transition: transform 0.3s ease;
        }

        .info-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 8px 30px rgba(0, 0, 0, 0.12);
        }

        .card-title {
            font-size: 1.2em;
            color: #667eea;
            margin-bottom: 20px;
            display: flex;
            align-items: center;
            gap: 10px;
            padding-bottom: 15px;
            border-bottom: 2px solid #f0f0f0;
        }

        .card-title i {
            font-size: 1.2em;
        }

        .info-row {
            display: flex;
            justify-content: space-between;
            padding: 12px 0;
            border-bottom: 1px solid #f8f8f8;
        }

        .info-row:last-child {
            border-bottom: none;
        }

        .info-label {
            font-weight: 600;
            color: #555;
            font-size: 0.95em;
        }

        .info-value {
            color: #333;
            text-align: right;
            font-size: 0.95em;
        }

        .empty-value {
            color: #999;
            font-style: italic;
        }

        .action-buttons {
            display: flex;
            gap: 15px;
            margin-top: 30px;
            justify-content: center;
            padding: 30px;
            background: white;
            border-radius: 15px;
            box-shadow: 0 5px 20px rgba(0, 0, 0, 0.08);
        }

        .btn {
            padding: 14px 28px;
            border: none;
            border-radius: 8px;
            font-size: 1em;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
            display: inline-flex;
            align-items: center;
            gap: 8px;
            text-decoration: none;
        }

        .btn-primary {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            box-shadow: 0 4px 15px rgba(102, 126, 234, 0.3);
        }

        .btn-primary:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(102, 126, 234, 0.4);
        }

        .btn-secondary {
            background: #e0e0e0;
            color: #555;
        }

        .btn-secondary:hover {
            background: #d0d0d0;
        }

        .btn-danger {
            background: #f44336;
            color: white;
        }

        .btn-danger:hover {
            background: #da190b;
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(244, 67, 54, 0.4);
        }

        .status-badge {
            display: inline-block;
            padding: 6px 16px;
            border-radius: 20px;
            font-size: 0.9em;
            font-weight: 600;
        }

        .badge-active {
            background: #d4edda;
            color: #155724;
        }

        .badge-inactive {
            background: #f8d7da;
            color: #721c24;
        }

        .alert {
            padding: 16px 20px;
            border-radius: 8px;
            margin-bottom: 20px;
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .alert-error {
            background: #f8d7da;
            color: #721c24;
            border: 1px solid #f5c6cb;
        }

        @media (max-width: 768px) {
            .info-cards {
                grid-template-columns: 1fr;
            }
            
            .action-buttons {
                flex-direction: column;
            }
            
            .btn {
                width: 100%;
                justify-content: center;
            }
        }

        .highlight-card {
            background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
            border: 2px solid #e0e0e0;
        }

        .employee-photo {
            width: 100px;
            height: 100px;
            border-radius: 50%;
            background: #e0e0e0;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 3em;
            color: #667eea;
            margin: 0 auto 20px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="details-container">
        <div class="page-header">
            <h1><i class="fas fa-heart"></i> Détails du Conjoint</h1>
            <p>
                <asp:Label ID="lblConjointInfo" runat="server" Text="Informations complètes du conjoint"></asp:Label>
            </p>
        </div>

        <!-- Message d'erreur -->
        <asp:Panel ID="pnlError" runat="server" Visible="false">
            <div class="alert alert-error">
                <i class="fas fa-exclamation-triangle"></i>
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlDetails" runat="server">
            <div class="info-cards">
                <!-- Carte Employé -->
                <div class="info-card highlight-card">
                    <h3 class="card-title">
                        <i class="fas fa-user-tie"></i>
                        Informations de l'Employé
                    </h3>
                    <div class="employee-photo">
                        <i class="fas fa-user"></i>
                    </div>
                    <div class="info-row">
                        <span class="info-label">Matricule</span>
                        <span class="info-value">
                            <asp:Label ID="lblMatricule" runat="server" Text="-"></asp:Label>
                        </span>
                    </div>
                    <div class="info-row">
                        <span class="info-label">Nom complet</span>
                        <span class="info-value">
                            <asp:Label ID="lblEmployeNom" runat="server" Text="-"></asp:Label>
                        </span>
                    </div>
                    <div class="info-row">
                        <span class="info-label">Numéro de conjoint</span>
                        <span class="info-value">
                            <asp:Label ID="lblNumConjoint" runat="server" Text="-"></asp:Label>
                        </span>
                    </div>
                </div>

                <!-- Carte Conjoint -->
                <div class="info-card">
                    <h3 class="card-title">
                        <i class="fas fa-user-friends"></i>
                        Informations du Conjoint
                    </h3>
                    <div class="info-row">
                        <span class="info-label">Nom</span>
                        <span class="info-value">
                            <asp:Label ID="lblNom" runat="server" CssClass="empty-value" Text="Non renseigné"></asp:Label>
                        </span>
                    </div>
                    <div class="info-row">
                        <span class="info-label">Prénom</span>
                        <span class="info-value">
                            <asp:Label ID="lblPrenom" runat="server" CssClass="empty-value" Text="Non renseigné"></asp:Label>
                        </span>
                    </div>
                    <div class="info-row">
                        <span class="info-label">Profession</span>
                        <span class="info-value">
                            <asp:Label ID="lblProfession" runat="server" CssClass="empty-value" Text="Non renseignée"></asp:Label>
                        </span>
                    </div>
                </div>

                <!-- Carte Mariage -->
                <div class="info-card">
                    <h3 class="card-title">
                        <i class="fas fa-ring"></i>
                        Informations du Mariage
                    </h3>
                    <div class="info-row">
                        <span class="info-label">Date de mariage</span>
                        <span class="info-value">
                            <asp:Label ID="lblDateMariage" runat="server" CssClass="empty-value" Text="Non renseignée"></asp:Label>
                        </span>
                    </div>
                    <div class="info-row">
                        <span class="info-label">Durée du mariage</span>
                        <span class="info-value">
                            <asp:Label ID="lblDureeMariage" runat="server" Text="-"></asp:Label>
                        </span>
                    </div>
                    <div class="info-row">
                        <span class="info-label">Allocation familiale</span>
                        <span class="info-value">
                            <asp:Label ID="lblAllocation" runat="server"></asp:Label>
                        </span>
                    </div>
                </div>
            </div>

            <!-- Boutons d'action -->
            <div class="action-buttons">
                <asp:Button ID="btnEdit" runat="server" Text="Modifier" CssClass="btn btn-primary" OnClick="btnEdit_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="Supprimer" CssClass="btn btn-danger" 
                            OnClick="btnDelete_Click" OnClientClick="return confirm('Êtes-vous sûr de vouloir supprimer ce conjoint ?');" />
                <a href="ListeConjoints.aspx" class="btn btn-secondary">
                    <i class="fas fa-arrow-left"></i> Retour à la liste
                </a>
            </div>
        </asp:Panel>
    </div>
</asp:Content>