<%@ Page Title="Détails de l'Enfant" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="DetailEnfant.aspx.cs" Inherits="GestionRHv2.Pages.Enfants.DetailEnfant" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .detail-card {
            background: white;
            border-radius: 8px;
            padding: 30px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            margin-top: 20px;
        }
        .detail-header {
            display: flex;
            align-items: center;
            gap: 10px;
            margin-bottom: 30px;
            padding-bottom: 20px;
            border-bottom: 2px solid #f0f0f0;
        }
        .detail-header h2 {
            margin: 0;
        }
        .detail-row {
            display: flex;
            padding: 15px 0;
            border-bottom: 1px solid #f0f0f0;
        }
        .detail-label {
            font-weight: bold;
            color: #666;
            width: 200px;
            flex-shrink: 0;
        }
        .detail-value {
            flex: 1;
            color: #333;
        }
        .btn-group {
            display: flex;
            gap: 10px;
            margin-top: 30px;
            padding-top: 20px;
            border-top: 2px solid #f0f0f0;
        }
        .btn {
            padding: 10px 20px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
            text-decoration: none;
            display: inline-block;
            color: white;
        }
        .btn-primary {
            background-color: #2196F3;
        }
        .btn-primary:hover {
            background-color: #1976D2;
        }
        .btn-warning {
            background-color: #FF9800;
        }
        .btn-warning:hover {
            background-color: #F57C00;
        }
        .btn-danger {
            background-color: #F44336;
        }
        .btn-danger:hover {
            background-color: #D32F2F;
        }
        .btn-secondary {
            background-color: #757575;
        }
        .btn-secondary:hover {
            background-color: #616161;
        }
        .gender-icon {
            font-size: 1.2em;
            margin-left: 10px;
        }
        .gender-male {
            color: #03A9F4;
        }
        .gender-female {
            color: #E91E63;
        }
        .info-section {
            margin-bottom: 30px;
        }
        .section-title {
            font-size: 1.2em;
            font-weight: bold;
            color: #444;
            margin-bottom: 15px;
            padding-bottom: 10px;
            border-bottom: 2px solid #2196F3;
        }
        .badge {
            display: inline-block;
            padding: 5px 10px;
            border-radius: 20px;
            font-size: 0.9em;
            font-weight: bold;
        }
        .badge-success {
            background-color: #4CAF50;
            color: white;
        }
        .badge-warning {
            background-color: #FF9800;
            color: white;
        }
        .agent-info-card {
            background-color: #E3F2FD;
            padding: 15px;
            border-radius: 4px;
            margin-bottom: 20px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="detail-card">
            <div class="detail-header">
                <span style="font-size: 2em;">👁️</span>
                <h2>Détails de l'Enfant</h2>
            </div>
            
            <!-- Information de l'agent parent -->
            <div class="agent-info-card">
                <div class="section-title">👤 Agent Parent</div>
                <div class="detail-row">
                    <div class="detail-label">Matricule :</div>
                    <div class="detail-value"><asp:Label ID="lblMatriculeAgent" runat="server"></asp:Label></div>
                </div>
                <div class="detail-row" style="border-bottom: none;">
                    <div class="detail-label">Nom et Prénom :</div>
                    <div class="detail-value"><asp:Label ID="lblNomAgent" runat="server"></asp:Label></div>
                </div>
            </div>
            
            <!-- Informations personnelles -->
            <div class="info-section">
                <div class="section-title">📋 Informations Personnelles</div>
                
                <div class="detail-row">
                    <div class="detail-label">Numéro de l'enfant :</div>
                    <div class="detail-value">
                        <asp:Label ID="lblNum" runat="server" Font-Bold="true" Font-Size="1.1em"></asp:Label>
                    </div>
                </div>
                
                <div class="detail-row">
                    <div class="detail-label">Nom :</div>
                    <div class="detail-value"><asp:Label ID="lblNom" runat="server"></asp:Label></div>
                </div>
                
                <div class="detail-row">
                    <div class="detail-label">Prénom :</div>
                    <div class="detail-value"><asp:Label ID="lblPrenom" runat="server"></asp:Label></div>
                </div>
                
                <div class="detail-row">
                    <div class="detail-label">Sexe :</div>
                    <div class="detail-value">
                        <asp:Label ID="lblSexe" runat="server"></asp:Label>
                        <asp:Label ID="lblSexeIcon" runat="server" CssClass="gender-icon"></asp:Label>
                    </div>
                </div>
                
                <div class="detail-row">
                    <div class="detail-label">Date de naissance :</div>
                    <div class="detail-value"><asp:Label ID="lblDateNaissance" runat="server"></asp:Label></div>
                </div>
                
                <div class="detail-row">
                    <div class="detail-label">Âge :</div>
                    <div class="detail-value"><asp:Label ID="lblAge" runat="server" Font-Bold="true"></asp:Label> ans</div>
                </div>
                
                <div class="detail-row">
                    <div class="detail-label">Lieu de naissance :</div>
                    <div class="detail-value"><asp:Label ID="lblLieuNaissance" runat="server"></asp:Label></div>
                </div>
            </div>
            
            <!-- Informations de scolarité -->
            <div class="info-section">
                <div class="section-title">🎓 Informations de Scolarité</div>
                
                <div class="detail-row">
                    <div class="detail-label">Scolarisé :</div>
                    <div class="detail-value">
                        <asp:Label ID="lblScolarise" runat="server"></asp:Label>
                    </div>
                </div>
                
                <asp:Panel ID="pnlDetailsScolarite" runat="server" Visible="false">
                    <div class="detail-row">
                        <div class="detail-label">Code Scolarité :</div>
                        <div class="detail-value"><asp:Label ID="lblCodSc" runat="server"></asp:Label></div>
                    </div>
                    
                    <div class="detail-row">
                        <div class="detail-label">Date de première scolarisation :</div>
                        <div class="detail-value"><asp:Label ID="lblDatePSc" runat="server"></asp:Label></div>
                    </div>
                </asp:Panel>
            </div>
            
            <!-- Autres informations -->
            <asp:Panel ID="pnlAutresInfos" runat="server" Visible="false">
                <div class="info-section">
                    <div class="section-title">📄 Autres Informations</div>
                    
                    <div class="detail-row">
                        <div class="detail-label">CH :</div>
                        <div class="detail-value"><asp:Label ID="lblCh" runat="server"></asp:Label></div>
                    </div>
                </div>
            </asp:Panel>
            
            <!-- Boutons d'action -->
            <div class="btn-group">
                <asp:HyperLink ID="lnkModifier" runat="server" CssClass="btn btn-warning">
                    ✏️ Modifier
                </asp:HyperLink>
                <asp:Button ID="btnSupprimer" runat="server" Text="🗑️ Supprimer" 
                    CssClass="btn btn-danger" OnClick="btnSupprimer_Click"
                    OnClientClick="return confirm('Êtes-vous sûr de vouloir supprimer cet enfant ?');" />
                <a href="ListeEnfants.aspx" class="btn btn-secondary">⬅️ Retour à la liste</a>
            </div>
        </div>
    </div>
</asp:Content>