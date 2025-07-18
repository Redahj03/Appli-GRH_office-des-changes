<%@ Page Title="Modifier l'Ordre de Mission" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ModifierMission.aspx.cs" Inherits="GestionRHv2.Pages.Missions.ModifierMission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Styles identiques à AjouterMission.aspx avec quelques modifications */
        .mission-container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
        }

        /* Header moderne */
        .page-header {
            background: linear-gradient(135deg, #f39c12 0%, #e67e22 100%);
            padding: 30px;
            border-radius: 20px;
            color: white;
            margin-bottom: 30px;
            box-shadow: 0 10px 30px rgba(243, 156, 18, 0.3);
        }

        .header-content {
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

        /* Alerte modification */
        .alert-box {
            background: #fff3cd;
            border: 1px solid #ffeeba;
            color: #856404;
            padding: 15px 20px;
            border-radius: 10px;
            margin-bottom: 20px;
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .alert-box i {
            font-size: 1.3rem;
        }

        /* Formulaire moderne */
        .form-container {
            background: white;
            border-radius: 20px;
            padding: 40px;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.08);
        }

        .form-section {
            margin-bottom: 40px;
        }

        .section-title {
            display: flex;
            align-items: center;
            gap: 10px;
            margin-bottom: 25px;
            padding-bottom: 15px;
            border-bottom: 2px solid #f0f2f5;
        }

        .section-title i {
            color: #f39c12;
            font-size: 1.3rem;
        }

        .section-title h2 {
            margin: 0;
            font-size: 1.5rem;
            color: #2c3e50;
            font-weight: 600;
        }

        .form-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
            gap: 25px;
        }

        .form-group {
            display: flex;
            flex-direction: column;
        }

        .form-group label {
            color: #2c3e50;
            font-weight: 600;
            margin-bottom: 8px;
            font-size: 0.95rem;
            display: flex;
            align-items: center;
            gap: 5px;
        }

        .form-group label .required {
            color: #e74c3c;
        }

        .form-control {
            padding: 12px 15px;
            border: 2px solid #e0e6ed;
            border-radius: 10px;
            font-size: 1rem;
            transition: all 0.3s ease;
            background: #f8f9fa;
            width: 100%;
            font-family: inherit;
        }

        .form-control:focus {
            border-color: #f39c12;
            background: white;
            outline: none;
            box-shadow: 0 0 0 3px rgba(243, 156, 18, 0.1);
        }

        .form-control:disabled {
            background: #e9ecef;
            cursor: not-allowed;
        }

        textarea.form-control {
            min-height: 120px;
            resize: vertical;
        }

        /* Select moderne */
        select.form-control {
            cursor: pointer;
            appearance: none;
            background-image: url("data:image/svg+xml;charset=UTF-8,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='none' stroke='%236c757d' stroke-width='2' stroke-linecap='round' stroke-linejoin='round'%3e%3cpolyline points='6 9 12 15 18 9'%3e%3c/polyline%3e%3c/svg%3e");
            background-repeat: no-repeat;
            background-position: right 15px center;
            background-size: 20px;
            padding-right: 45px;
        }

        /* Boutons de transport */
        .transport-options {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 15px;
        }

        .transport-option {
            position: relative;
            cursor: pointer;
        }

        .transport-option input[type="radio"] {
            position: absolute;
            opacity: 0;
        }

        .transport-label {
            display: flex;
            align-items: center;
            gap: 10px;
            padding: 15px 20px;
            border: 2px solid #e0e6ed;
            border-radius: 10px;
            background: #f8f9fa;
            transition: all 0.3s ease;
        }

        .transport-option input[type="radio"]:checked + .transport-label {
            border-color: #f39c12;
            background: rgba(243, 156, 18, 0.05);
            box-shadow: 0 0 0 3px rgba(243, 156, 18, 0.1);
        }

        .transport-label i {
            font-size: 1.5rem;
            color: #f39c12;
        }

        /* Section historique modifications */
        .history-section {
            background: #f8f9fa;
            padding: 25px;
            border-radius: 15px;
            margin-bottom: 30px;
        }

        .history-title {
            display: flex;
            align-items: center;
            gap: 10px;
            margin-bottom: 20px;
            color: #2c3e50;
            font-weight: 600;
        }

        .history-item {
            display: flex;
            align-items: center;
            gap: 15px;
            padding: 15px;
            background: white;
            border-radius: 10px;
            margin-bottom: 10px;
            border: 1px solid #e0e6ed;
        }

        .history-icon {
            width: 40px;
            height: 40px;
            background: rgba(243, 156, 18, 0.1);
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            color: #f39c12;
        }

        .history-content {
            flex: 1;
        }

        .history-date {
            font-size: 0.85rem;
            color: #7f8c8d;
        }

        /* Section calcul indemnités */
        .indemnites-section {
            background: #f8f9fa;
            padding: 25px;
            border-radius: 15px;
            margin-top: 20px;
        }

        .calcul-result {
            background: white;
            padding: 20px;
            border-radius: 10px;
            margin-top: 15px;
            border: 2px solid #e0e6ed;
        }

        .result-row {
            display: flex;
            justify-content: space-between;
            padding: 10px 0;
            border-bottom: 1px solid #f0f2f5;
        }

        .result-row:last-child {
            border-bottom: none;
            font-weight: 700;
            font-size: 1.2rem;
            color: #f39c12;
            padding-top: 15px;
        }

        /* Boutons actions */
        .form-actions {
            display: flex;
            gap: 15px;
            justify-content: flex-end;
            margin-top: 30px;
            padding-top: 30px;
            border-top: 2px solid #f0f2f5;
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
            background: #f39c12;
            color: white;
            box-shadow: 0 5px 15px rgba(243, 156, 18, 0.3);
        }

        .btn-primary:hover {
            background: #e67e22;
            transform: translateY(-2px);
            box-shadow: 0 8px 20px rgba(243, 156, 18, 0.4);
        }

        .btn-secondary {
            background: #e0e6ed;
            color: #7f8c8d;
        }

        .btn-secondary:hover {
            background: #d1d8e0;
            color: #5f6c7d;
        }

        .btn-danger {
            background: #e74c3c;
            color: white;
        }

        .btn-danger:hover {
            background: #c0392b;
            transform: translateY(-2px);
        }

        /* Loading spinner */
        .loading-spinner {
            display: none;
            text-align: center;
            padding: 20px;
        }

        .spinner {
            border: 3px solid #f3f3f3;
            border-top: 3px solid #f39c12;
            border-radius: 50%;
            width: 40px;
            height: 40px;
            animation: spin 1s linear infinite;
            margin: 0 auto;
        }

        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }

        /* Responsive */
        @media (max-width: 768px) {
            .form-grid {
                grid-template-columns: 1fr;
            }

            .form-actions {
                flex-direction: column;
            }

            .btn {
                width: 100%;
                justify-content: center;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="mission-container">
        <!-- Header -->
        <div class="page-header">
            <div class="header-content">
                <i class="fas fa-edit header-icon"></i>
                <div class="header-text">
                    <h1>Modifier l'Ordre de Mission</h1>
                    <p>Mission #<asp:Label ID="lblMissionID" runat="server"></asp:Label></p>
                </div>
            </div>
        </div>

        <!-- Alerte modification -->
        <div class="alert-box">
            <i class="fas fa-exclamation-triangle"></i>
            <div>
                <strong>Attention :</strong> Cette mission est en statut "<asp:Label ID="lblStatutActuel" runat="server"></asp:Label>". 
                Toute modification sera enregistrée dans l'historique.
            </div>
        </div>

        <!-- Formulaire -->
        <div class="form-container">
            <!-- Section Agent (lecture seule) -->
            <div class="form-section">
                <div class="section-title">
                    <i class="fas fa-user"></i>
                    <h2>Informations de l'Agent</h2>
                </div>
                <div class="form-grid">
                    <div class="form-group">
                        <label>Matricule de l'Agent</label>
                        <asp:TextBox ID="txtMatricule" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Nom de l'Agent</label>
                        <asp:TextBox ID="txtNomAgent" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Grade</label>
                        <asp:TextBox ID="txtGrade" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Fonction</label>
                        <asp:TextBox ID="txtFonction" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </div>

            <!-- Section Mission -->
            <div class="form-section">
                <div class="section-title">
                    <i class="fas fa-route"></i>
                    <h2>Détails de la Mission</h2>
                </div>
                <div class="form-grid">
                    <div class="form-group">
                        <label>
                            Ville de Destination
                            <span class="required">*</span>
                        </label>
                        <asp:TextBox ID="txtVille" runat="server" CssClass="form-control" 
                            placeholder="Ex: Casablanca"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>
                            Objet de la Mission
                            <span class="required">*</span>
                        </label>
                        <asp:TextBox ID="txtObjet" runat="server" CssClass="form-control" 
                            TextMode="MultiLine" placeholder="Décrivez l'objet de la mission..."></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>
                            Date de Départ
                            <span class="required">*</span>
                        </label>
                        <asp:TextBox ID="txtDateDepart" runat="server" CssClass="form-control" 
                            TextMode="Date"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Heure de Départ</label>
                        <asp:TextBox ID="txtHeureDepart" runat="server" CssClass="form-control" 
                            TextMode="Time"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>
                            Date de Retour
                            <span class="required">*</span>
                        </label>
                        <asp:TextBox ID="txtDateRetour" runat="server" CssClass="form-control" 
                            TextMode="Date"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Heure de Retour</label>
                        <asp:TextBox ID="txtHeureRetour" runat="server" CssClass="form-control" 
                            TextMode="Time"></asp:TextBox>
                    </div>
                </div>
            </div>

            <!-- Section Transport -->
            <div class="form-section">
                <div class="section-title">
                    <i class="fas fa-car"></i>
                    <h2>Moyen de Transport</h2>
                </div>
                
                <div class="transport-options">
                    <div class="transport-option">
                        <asp:RadioButton ID="rbVoitureService" runat="server" GroupName="Transport" 
                            Value="Voiture de service" AutoPostBack="true" 
                            OnCheckedChanged="Transport_CheckedChanged" />
                        <label class="transport-label" for="<%=rbVoitureService.ClientID%>">
                            <i class="fas fa-car"></i>
                            <span>Voiture de service</span>
                        </label>
                    </div>

                    <div class="transport-option">
                        <asp:RadioButton ID="rbVoiturePersonnelle" runat="server" GroupName="Transport" 
                            Value="Voiture personnelle" AutoPostBack="true" 
                            OnCheckedChanged="Transport_CheckedChanged" />
                        <label class="transport-label" for="<%=rbVoiturePersonnelle.ClientID%>">
                            <i class="fas fa-car-side"></i>
                            <span>Voiture personnelle</span>
                        </label>
                    </div>

                    <div class="transport-option">
                        <asp:RadioButton ID="rbTrain" runat="server" GroupName="Transport" 
                            Value="Train" AutoPostBack="true" 
                            OnCheckedChanged="Transport_CheckedChanged" />
                        <label class="transport-label" for="<%=rbTrain.ClientID%>">
                            <i class="fas fa-train"></i>
                            <span>Train</span>
                        </label>
                    </div>

                    <div class="transport-option">
                        <asp:RadioButton ID="rbAvion" runat="server" GroupName="Transport" 
                            Value="Avion" AutoPostBack="true" 
                            OnCheckedChanged="Transport_CheckedChanged" />
                        <label class="transport-label" for="<%=rbAvion.ClientID%>">
                            <i class="fas fa-plane"></i>
                            <span>Avion</span>
                        </label>
                    </div>
                </div>

                <div class="form-group" style="margin-top: 20px;">
                    <label>Détails du Transport</label>
                    <asp:TextBox ID="txtDetailTransport" runat="server" CssClass="form-control" 
                        TextMode="MultiLine" placeholder="Précisez les détails du transport (immatriculation, n° de vol, etc.)"></asp:TextBox>
                </div>
            </div>

            <!-- Section Indemnités (pour voiture personnelle) -->
            <asp:Panel ID="pnlIndemnites" runat="server" Visible="false" CssClass="form-section">
                <div class="section-title">
                    <i class="fas fa-calculator"></i>
                    <h2>Calcul des Indemnités Kilométriques</h2>
                </div>

                <div class="indemnites-section">
                    <div class="form-grid">
                        <div class="form-group">
                            <label>Puissance Fiscale</label>
                            <asp:DropDownList ID="ddlPuissanceFiscale" runat="server" CssClass="form-control" 
                                AutoPostBack="true" OnSelectedIndexChanged="CalculerIndemnites">
                                <asp:ListItem Text="-- Sélectionnez --" Value=""></asp:ListItem>
                                <asp:ListItem Text="4 CV et moins" Value="4"></asp:ListItem>
                                <asp:ListItem Text="5 CV" Value="5"></asp:ListItem>
                                <asp:ListItem Text="6 CV" Value="6"></asp:ListItem>
                                <asp:ListItem Text="7 CV" Value="7"></asp:ListItem>
                                <asp:ListItem Text="8 CV" Value="8"></asp:ListItem>
                                <asp:ListItem Text="9 CV" Value="9"></asp:ListItem>
                                <asp:ListItem Text="10 CV" Value="10"></asp:ListItem>
                                <asp:ListItem Text="11 CV et plus" Value="11"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="form-group">
                            <label>Kilométrage</label>
                            <asp:TextBox ID="txtKilometrage" runat="server" CssClass="form-control" 
                                TextMode="Number" placeholder="Distance en km" 
                                AutoPostBack="true" OnTextChanged="CalculerIndemnites"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label>Taux par km (DH)</label>
                            <asp:TextBox ID="txtTauxKm" runat="server" CssClass="form-control" 
                                ReadOnly="true" placeholder="Calculé automatiquement"></asp:TextBox>
                        </div>
                    </div>

                    <div class="calcul-result" id="divResultatCalcul" runat="server" visible="false">
                        <div class="result-row">
                            <span>Distance parcourue</span>
                            <span><asp:Label ID="lblDistance" runat="server"></asp:Label> km</span>
                        </div>
                        <div class="result-row">
                            <span>Taux kilométrique</span>
                            <span><asp:Label ID="lblTaux" runat="server"></asp:Label> DH/km</span>
                        </div>
                        <div class="result-row">
                            <span>Total indemnités</span>
                            <span><asp:Label ID="lblTotalIndemnites" runat="server"></asp:Label> DH</span>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- Section Indemnités journalières -->
            <div class="form-section">
                <div class="section-title">
                    <i class="fas fa-coins"></i>
                    <h2>Indemnités Journalières</h2>
                </div>

                <div class="form-grid">
                    <div class="form-group">
                        <label>Nombre de jours</label>
                        <asp:TextBox ID="txtNombreJours" runat="server" CssClass="form-control" 
                            ReadOnly="true" placeholder="Calculé automatiquement"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Taux journalier (DH)</label>
                        <asp:TextBox ID="txtTauxJournalier" runat="server" CssClass="form-control" 
                            TextMode="Number" placeholder="Taux par jour"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Nombre de vacations</label>
                        <asp:TextBox ID="txtNombreVacations" runat="server" CssClass="form-control" 
                            TextMode="Number" placeholder="0"></asp:TextBox>
                    </div>
                </div>
            </div>

            <!-- Historique des modifications -->
            <asp:Panel ID="pnlHistory" runat="server" Visible="false">
                <div class="history-section">
                    <div class="history-title">
                        <i class="fas fa-history"></i>
                        <span>Historique des modifications</span>
                    </div>
                    <asp:Repeater ID="rptHistory" runat="server">
                        <ItemTemplate>
                            <div class="history-item">
                                <div class="history-icon">
                                    <i class="fas fa-edit"></i>
                                </div>
                                <div class="history-content">
                                    <div><%# Eval("Action") %></div>
                                    <div class="history-date">
                                        Par <%# Eval("ModifiePar") %> le <%# Eval("DateModification", "{0:dd/MM/yyyy HH:mm}") %>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </asp:Panel>

            <!-- Actions -->
            <div class="form-actions">
                <asp:Button ID="btnAnnuler" runat="server" Text="Annuler" 
                    CssClass="btn btn-secondary" OnClick="btnAnnuler_Click" />
                <asp:Button ID="btnSauvegarder" runat="server" Text="Sauvegarder les modifications" 
                    CssClass="btn btn-primary" OnClick="btnSauvegarder_Click" />
            </div>
        </div>

        <!-- Loading -->
        <div class="loading-spinner" id="loadingDiv">
            <div class="spinner"></div>
            <p>Traitement en cours...</p>
        </div>
    </div>

    <script type="text/javascript">
        // Calculer automatiquement le nombre de jours
        function calculerNombreJours() {
            var dateDepart = document.getElementById('<%= txtDateDepart.ClientID %>').value;
            var dateRetour = document.getElementById('<%= txtDateRetour.ClientID %>').value;
            
            if (dateDepart && dateRetour) {
                var date1 = new Date(dateDepart);
                var date2 = new Date(dateRetour);
                var diffTime = Math.abs(date2 - date1);
                var diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24)) + 1;
                
                document.getElementById('<%= txtNombreJours.ClientID %>').value = diffDays;
            }
        }

        // Écouter les changements de dates
        document.getElementById('<%= txtDateDepart.ClientID %>').addEventListener('change', calculerNombreJours);
        document.getElementById('<%= txtDateRetour.ClientID %>').addEventListener('change', calculerNombreJours);
    </script>
</asp:Content>