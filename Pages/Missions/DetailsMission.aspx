<%@ Page Title="Détails de la Mission" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="DetailsMission.aspx.cs" Inherits="GestionRHv2.Pages.Missions.DetailsMission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .details-container {
            max-width: 1000px;
            margin: 0 auto;
            padding: 20px;
        }

        /* Header moderne */
        .details-header {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            padding: 30px;
            border-radius: 20px;
            color: white;
            margin-bottom: 30px;
            box-shadow: 0 10px 30px rgba(102, 126, 234, 0.3);
            position: relative;
        }

        .header-content {
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .header-left {
            display: flex;
            align-items: center;
            gap: 15px;
        }

        .header-icon {
            font-size: 2.5rem;
            opacity: 0.9;
        }

        .header-text h1 {
            margin: 0;
            font-size: 2rem;
            font-weight: 600;
        }

        .header-text p {
            margin: 5px 0 0 0;
            opacity: 0.9;
        }

        .mission-status {
            background: rgba(255, 255, 255, 0.2);
            padding: 10px 25px;
            border-radius: 25px;
            font-weight: 600;
            display: inline-flex;
            align-items: center;
            gap: 8px;
        }

        /* Carte principale */
        .detail-card {
            background: white;
            border-radius: 20px;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.08);
            overflow: hidden;
            margin-bottom: 25px;
        }

        .card-header {
            background: #f8f9fa;
            padding: 20px 30px;
            border-bottom: 2px solid #e9ecef;
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .card-header i {
            color: #667eea;
            font-size: 1.3rem;
        }

        .card-header h2 {
            margin: 0;
            font-size: 1.3rem;
            color: #2c3e50;
            font-weight: 600;
        }

        .card-body {
            padding: 30px;
        }

        /* Grille d'informations */
        .info-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
            gap: 25px;
        }

        .info-item {
            display: flex;
            align-items: flex-start;
            gap: 15px;
        }

        .info-icon {
            width: 40px;
            height: 40px;
            background: rgba(102, 126, 234, 0.1);
            border-radius: 10px;
            display: flex;
            align-items: center;
            justify-content: center;
            flex-shrink: 0;
        }

        .info-icon i {
            color: #667eea;
            font-size: 1.2rem;
        }

        .info-content {
            flex: 1;
        }

        .info-label {
            font-size: 0.9rem;
            color: #7f8c8d;
            margin-bottom: 5px;
        }

        .info-value {
            font-size: 1.1rem;
            color: #2c3e50;
            font-weight: 600;
        }

        /* Section objet de mission */
        .mission-description {
            background: #f8f9fa;
            padding: 25px;
            border-radius: 15px;
            margin-top: 20px;
            border-left: 4px solid #667eea;
        }

        .mission-description h3 {
            margin: 0 0 15px 0;
            color: #2c3e50;
            font-size: 1.1rem;
        }

        .mission-description p {
            line-height: 1.6;
            color: #495057;
            margin: 0;
        }

        /* Section financière */
        .financial-info {
            background: #e3f2fd;
            padding: 25px;
            border-radius: 15px;
            margin-top: 20px;
        }

        .financial-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 20px;
        }

        .financial-item {
            text-align: center;
        }

        .financial-value {
            font-size: 2rem;
            font-weight: 700;
            color: #1976d2;
            display: block;
        }

        .financial-label {
            color: #64b5f6;
            font-size: 0.9rem;
            margin-top: 5px;
        }

        /* Timeline de validation */
        .validation-timeline {
            margin-top: 30px;
        }

        .timeline-item {
            display: flex;
            gap: 20px;
            position: relative;
            padding-bottom: 30px;
        }

        .timeline-item:last-child {
            padding-bottom: 0;
        }

        .timeline-marker {
            width: 40px;
            height: 40px;
            border-radius: 50%;
            background: #667eea;
            color: white;
            display: flex;
            align-items: center;
            justify-content: center;
            flex-shrink: 0;
            position: relative;
            z-index: 2;
        }

        .timeline-item:not(:last-child)::before {
            content: '';
            position: absolute;
            left: 19px;
            top: 40px;
            bottom: 0;
            width: 2px;
            background: #e0e6ed;
            z-index: 1;
        }

        .timeline-content {
            flex: 1;
            background: #f8f9fa;
            padding: 20px;
            border-radius: 10px;
        }

        .timeline-date {
            font-size: 0.85rem;
            color: #7f8c8d;
            margin-bottom: 5px;
        }

        .timeline-status {
            font-weight: 600;
            color: #2c3e50;
            margin-bottom: 10px;
        }

        .timeline-comment {
            color: #495057;
            font-style: italic;
        }

        /* Boutons d'action */
        .action-buttons {
            display: flex;
            gap: 15px;
            justify-content: center;
            margin-top: 30px;
            padding-top: 30px;
            border-top: 2px solid #e9ecef;
        }

        .btn {
            padding: 12px 30px;
            border: none;
            border-radius: 10px;
            font-weight: 600;
            font-size: 1rem;
            cursor: pointer;
            transition: all 0.3s ease;
            display: inline-flex;
            align-items: center;
            gap: 8px;
            text-decoration: none;
        }

        .btn-primary {
            background: #667eea;
            color: white;
            box-shadow: 0 5px 15px rgba(102, 126, 234, 0.3);
        }

        .btn-primary:hover {
            background: #5a67d8;
            transform: translateY(-2px);
            box-shadow: 0 8px 20px rgba(102, 126, 234, 0.4);
        }

        .btn-secondary {
            background: #e0e6ed;
            color: #7f8c8d;
        }

        .btn-secondary:hover {
            background: #d1d8e0;
            color: #5f6c7d;
        }

        .btn-success {
            background: #27ae60;
            color: white;
        }

        .btn-success:hover {
            background: #229954;
            transform: translateY(-2px);
        }

        .btn-warning {
            background: #f39c12;
            color: white;
        }

        .btn-warning:hover {
            background: #e67e22;
            transform: translateY(-2px);
        }

        /* Responsive */
        @media (max-width: 768px) {
            .header-content {
                flex-direction: column;
                gap: 20px;
            }

            .info-grid {
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

        /* Print styles */
        @media print {
            .action-buttons {
                display: none;
            }

            .details-header {
                background: none;
                color: black;
                border: 2px solid #333;
            }

            .detail-card {
                box-shadow: none;
                border: 1px solid #ccc;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="details-container">
        <!-- Header -->
        <div class="details-header">
            <div class="header-content">
                <div class="header-left">
                    <i class="fas fa-file-alt header-icon"></i>
                    <div class="header-text">
                        <h1>Ordre de Mission #<asp:Label ID="lblMissionID" runat="server"></asp:Label></h1>
                        <p>Détails complets de la mission</p>
                    </div>
                </div>
                <div class="mission-status">
                    <i class='<%# GetStatusIcon() %>'></i>
                    <asp:Label ID="lblStatut" runat="server"></asp:Label>
                </div>
            </div>
        </div>

        <!-- Informations de l'agent -->
        <div class="detail-card">
            <div class="card-header">
                <i class="fas fa-user"></i>
                <h2>Informations de l'Agent</h2>
            </div>
            <div class="card-body">
                <div class="info-grid">
                    <div class="info-item">
                        <div class="info-icon">
                            <i class="fas fa-id-badge"></i>
                        </div>
                        <div class="info-content">
                            <div class="info-label">Matricule</div>
                            <div class="info-value"><asp:Label ID="lblMatricule" runat="server"></asp:Label></div>
                        </div>
                    </div>

                    <div class="info-item">
                        <div class="info-icon">
                            <i class="fas fa-user-tie"></i>
                        </div>
                        <div class="info-content">
                            <div class="info-label">Nom Complet</div>
                            <div class="info-value"><asp:Label ID="lblNomAgent" runat="server"></asp:Label></div>
                        </div>
                    </div>

                    <div class="info-item">
                        <div class="info-icon">
                            <i class="fas fa-award"></i>
                        </div>
                        <div class="info-content">
                            <div class="info-label">Grade</div>
                            <div class="info-value"><asp:Label ID="lblGrade" runat="server"></asp:Label></div>
                        </div>
                    </div>

                    <div class="info-item">
                        <div class="info-icon">
                            <i class="fas fa-briefcase"></i>
                        </div>
                        <div class="info-content">
                            <div class="info-label">Fonction</div>
                            <div class="info-value"><asp:Label ID="lblFonction" runat="server"></asp:Label></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Détails de la mission -->
        <div class="detail-card">
            <div class="card-header">
                <i class="fas fa-route"></i>
                <h2>Détails de la Mission</h2>
            </div>
            <div class="card-body">
                <div class="info-grid">
                    <div class="info-item">
                        <div class="info-icon">
                            <i class="fas fa-map-marker-alt"></i>
                        </div>
                        <div class="info-content">
                            <div class="info-label">Destination</div>
                            <div class="info-value"><asp:Label ID="lblDestination" runat="server"></asp:Label></div>
                        </div>
                    </div>

                    <div class="info-item">
                        <div class="info-icon">
                            <i class="fas fa-calendar-alt"></i>
                        </div>
                        <div class="info-content">
                            <div class="info-label">Date de départ</div>
                            <div class="info-value"><asp:Label ID="lblDateDepart" runat="server"></asp:Label></div>
                        </div>
                    </div>

                    <div class="info-item">
                        <div class="info-icon">
                            <i class="fas fa-calendar-check"></i>
                        </div>
                        <div class="info-content">
                            <div class="info-label">Date de retour</div>
                            <div class="info-value"><asp:Label ID="lblDateRetour" runat="server"></asp:Label></div>
                        </div>
                    </div>

                    <div class="info-item">
                        <div class="info-icon">
                            <i class="fas fa-clock"></i>
                        </div>
                        <div class="info-content">
                            <div class="info-label">Durée</div>
                            <div class="info-value"><asp:Label ID="lblDuree" runat="server"></asp:Label></div>
                        </div>
                    </div>

                    <div class="info-item">
                        <div class="info-icon">
                            <i class="fas fa-car"></i>
                        </div>
                        <div class="info-content">
                            <div class="info-label">Moyen de transport</div>
                            <div class="info-value"><asp:Label ID="lblTransport" runat="server"></asp:Label></div>
                        </div>
                    </div>

                    <div class="info-item">
                        <div class="info-icon">
                            <i class="fas fa-info-circle"></i>
                        </div>
                        <div class="info-content">
                            <div class="info-label">Détails transport</div>
                            <div class="info-value"><asp:Label ID="lblDetailTransport" runat="server"></asp:Label></div>
                        </div>
                    </div>
                </div>

                <div class="mission-description">
                    <h3>Objet de la mission</h3>
                    <p><asp:Label ID="lblObjetMission" runat="server"></asp:Label></p>
                </div>
            </div>
        </div>

        <!-- Informations financières -->
        <asp:Panel ID="pnlFinancial" runat="server" Visible="false" CssClass="detail-card">
            <div class="card-header">
                <i class="fas fa-coins"></i>
                <h2>Informations Financières</h2>
            </div>
            <div class="card-body">
                <div class="financial-info">
                    <div class="financial-grid">
                        <div class="financial-item">
                            <span class="financial-value"><asp:Label ID="lblKilometrage" runat="server"></asp:Label> km</span>
                            <span class="financial-label">Distance parcourue</span>
                        </div>
                        <div class="financial-item">
                            <span class="financial-value"><asp:Label ID="lblTauxKm" runat="server"></asp:Label> DH</span>
                            <span class="financial-label">Taux/km</span>
                        </div>
                        <div class="financial-item">
                            <span class="financial-value"><asp:Label ID="lblMontantTotal" runat="server"></asp:Label> DH</span>
                            <span class="financial-label">Montant total</span>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <!-- Historique de validation -->
        <asp:Panel ID="pnlValidation" runat="server" Visible="false" CssClass="detail-card">
            <div class="card-header">
                <i class="fas fa-history"></i>
                <h2>Historique de Validation</h2>
            </div>
            <div class="card-body">
                <div class="validation-timeline">
                    <asp:Repeater ID="rptValidation" runat="server">
                        <ItemTemplate>
                            <div class="timeline-item">
                                <div class="timeline-marker">
                                    <i class='<%# GetValidationIcon(Eval("Statut").ToString()) %>'></i>
                                </div>
                                <div class="timeline-content">
                                    <div class="timeline-date"><%# Eval("DateValidation", "{0:dd/MM/yyyy HH:mm}") %></div>
                                    <div class="timeline-status"><%# Eval("Statut") %> par <%# Eval("ValidePar") %></div>
                                    <div class="timeline-comment"><%# Eval("Commentaire") %></div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </asp:Panel>

        <!-- Boutons d'action -->
        <div class="action-buttons">
            <asp:Button ID="btnImprimer" runat="server" Text="Imprimer" 
                CssClass="btn btn-primary" OnClientClick="window.print(); return false;" />
            <asp:Button ID="btnModifier" runat="server" Text="Modifier" 
                CssClass="btn btn-warning" OnClick="btnModifier_Click" />
            <asp:Button ID="btnRetour" runat="server" Text="Retour à la liste" 
                CssClass="btn btn-secondary" OnClick="btnRetour_Click" />
        </div>
    </div>
</asp:Content>