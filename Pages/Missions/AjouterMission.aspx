<%@ Page Title="Ajouter/Modifier Mission" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AjouterMission.aspx.cs" Inherits="GestionRHv2.Pages.Missions.AjouterMission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-container {
            max-width: 900px;
            margin: 0 auto;
            padding: 20px;
        }

        .form-card {
            background: white;
            border-radius: 10px;
            padding: 30px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }

        .form-header {
            display: flex;
            align-items: center;
            gap: 15px;
            margin-bottom: 30px;
            padding-bottom: 20px;
            border-bottom: 2px solid #e0e0e0;
        }

        .form-header h1 {
            color: #2c3e50;
            font-size: 2rem;
            margin: 0;
        }

        .form-header i {
            color: #3498db;
            font-size: 2rem;
        }

        .form-section {
            margin-bottom: 30px;
        }

        .section-title {
            color: #34495e;
            font-size: 1.2rem;
            font-weight: 600;
            margin-bottom: 20px;
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .section-title i {
            color: #3498db;
            font-size: 1rem;
        }

        .form-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 20px;
        }

        .form-group {
            margin-bottom: 20px;
        }

        .form-group label {
            display: block;
            margin-bottom: 8px;
            color: #495057;
            font-weight: 500;
        }

        .form-group label.required::after {
            content: " *";
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
            box-shadow: 0 0 0 2px rgba(52, 152, 219, 0.1);
        }

        .form-control-select {
            cursor: pointer;
        }

        .help-text {
            font-size: 12px;
            color: #7f8c8d;
            margin-top: 5px;
        }

        .transport-options {
            display: flex;
            gap: 15px;
            flex-wrap: wrap;
        }

        .transport-option {
            display: flex;
            align-items: center;
            gap: 5px;
            padding: 10px 20px;
            border: 2px solid #e0e0e0;
            border-radius: 25px;
            cursor: pointer;
            transition: all 0.3s;
        }

        .transport-option:hover {
            border-color: #3498db;
            background: #f0f8ff;
        }

        .transport-option.selected {
            border-color: #3498db;
            background: #3498db;
            color: white;
        }

        .transport-option input[type="radio"] {
            display: none;
        }

        .calculated-field {
            background: #f8f9fa;
            border: 1px solid #e0e0e0;
            padding: 15px;
            border-radius: 5px;
            text-align: center;
        }

        .calculated-value {
            font-size: 1.5rem;
            font-weight: 700;
            color: #2c3e50;
        }

        .calculated-label {
            color: #7f8c8d;
            font-size: 14px;
            margin-top: 5px;
        }

        .form-actions {
            display: flex;
            gap: 15px;
            justify-content: center;
            margin-top: 30px;
            padding-top: 30px;
            border-top: 2px solid #e0e0e0;
        }

        .btn {
            padding: 12px 30px;
            border: none;
            border-radius: 5px;
            font-size: 16px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s;
            display: flex;
            align-items: center;
            gap: 10px;
            text-decoration: none;
        }

        .btn-primary {
            background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
            color: white;
        }

        .btn-primary:hover {
            transform: translateY(-2px);
            box-shadow: 0 5px 20px rgba(52, 152, 219, 0.4);
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
            border-radius: 5px;
            margin-bottom: 20px;
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .alert-info {
            background: #e3f2fd;
            color: #1565c0;
            border: 1px solid #bbdefb;
        }

        .validation-summary {
            background: #ffebee;
            color: #c62828;
            padding: 15px;
            border-radius: 5px;
            margin-bottom: 20px;
            border: 1px solid #ffcdd2;
        }

        /* Autocomplete pour l'agent */
        .autocomplete-container {
            position: relative;
        }

        .autocomplete-results {
            position: absolute;
            top: 100%;
            left: 0;
            right: 0;
            background: white;
            border: 1px solid #ddd;
            border-top: none;
            border-radius: 0 0 5px 5px;
            max-height: 200px;
            overflow-y: auto;
            z-index: 1000;
            display: none;
            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
        }

        .autocomplete-item {
            padding: 10px 15px;
            cursor: pointer;
            transition: background 0.2s;
        }

        .autocomplete-item:hover {
            background: #f0f8ff;
        }

        .autocomplete-item strong {
            color: #2c3e50;
        }

        .autocomplete-item small {
            color: #7f8c8d;
        }

        @media (max-width: 768px) {
            .form-grid {
                grid-template-columns: 1fr;
            }

            .form-actions {
                flex-direction: column;
                width: 100%;
            }

            .btn {
                width: 100%;
                justify-content: center;
            }
        }
    </style>

    <script type="text/javascript">
        function calculateTotal() {
            var montant = parseFloat(document.getElementById('<%= txtMontant.ClientID %>').value) || 0;
            var tauxJour = parseFloat(document.getElementById('<%= txtTauxJour.ClientID %>').value) || 0;
            var nbJour = parseInt(document.getElementById('<%= txtNbJour.ClientID %>').value) || 0;
            
            var total = montant + (tauxJour * nbJour);
            
            document.getElementById('totalCalcule').innerHTML = total.toFixed(2) + ' DH';
        }

        // Fonction pour gérer l'autocomplete (simplifiée)
        function setupAutocomplete() {
            var input = document.getElementById('<%= txtMatricule.ClientID %>');
            var resultsDiv = document.getElementById('autocompleteResults');
            
            input.addEventListener('input', function() {
                if (this.value.length > 2) {
                    // Ici vous pourriez faire un appel AJAX pour récupérer les agents
                    // Pour l'instant, on cache juste les résultats
                    resultsDiv.style.display = 'none';
                }
            });
        }

        window.onload = function() {
            setupAutocomplete();
        };
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-container">
        <div class="form-card">
            <div class="form-header">
                <i class="fas fa-route"></i>
                <h1><asp:Label ID="lblTitle" runat="server" Text="Nouvelle Mission"></asp:Label></h1>
            </div>

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="validation-summary" />

            <!-- Section Informations Agent -->
            <div class="form-section">
                <h3 class="section-title">
                    <i class="fas fa-user"></i>
                    Informations de l'Agent
                </h3>
                <div class="form-grid">
                    <div class="form-group autocomplete-container">
                        <label for="txtMatricule" class="required">Matricule</label>
                        <asp:TextBox ID="txtMatricule" runat="server" CssClass="form-control" 
                            placeholder="Ex: 12345" MaxLength="20" />
                        <asp:RequiredFieldValidator ID="rfvMatricule" runat="server" 
                            ControlToValidate="txtMatricule" ErrorMessage="Le matricule est obligatoire"
                            Display="Dynamic" CssClass="help-text" ForeColor="#e74c3c" />
                        <div id="autocompleteResults" class="autocomplete-results"></div>
                    </div>
                    <div class="form-group">
                        <label for="txtNom">Nom de l'Agent</label>
                        <asp:TextBox ID="txtNom" runat="server" CssClass="form-control" 
                            placeholder="Nom complet" />
                    </div>
                    <div class="form-group">
                        <label for="txtGrade">Grade</label>
                        <asp:TextBox ID="txtGrade" runat="server" CssClass="form-control" 
                            placeholder="Grade de l'agent" />
                    </div>
                    <div class="form-group">
                        <label for="txtCompte">Compte</label>
                        <asp:TextBox ID="txtCompte" runat="server" CssClass="form-control" 
                            placeholder="Numéro de compte" />
                    </div>
                </div>
            </div>

            <!-- Section Détails Mission -->
            <div class="form-section">
                <h3 class="section-title">
                    <i class="fas fa-clipboard-list"></i>
                    Détails de la Mission
                </h3>
                <div class="form-grid">
                    <div class="form-group">
                        <label for="txtDateDeplacement" class="required">Date de Déplacement</label>
                        <asp:TextBox ID="txtDateDeplacement" runat="server" CssClass="form-control" 
                            TextMode="Date" />
                        <asp:RequiredFieldValidator ID="rfvDate" runat="server" 
                            ControlToValidate="txtDateDeplacement" ErrorMessage="La date est obligatoire"
                            Display="Dynamic" CssClass="help-text" ForeColor="#e74c3c" />
                    </div>
                    <div class="form-group">
                        <label>Moyen de Transport</label>
                        <div class="transport-options">
                            <label class="transport-option">
                                <asp:RadioButton ID="rbVoiture" runat="server" GroupName="Transport" 
                                    Value="Voiture" Checked="true" />
                                <i class="fas fa-car"></i>
                                <span>Voiture</span>
                            </label>
                            <label class="transport-option">
                                <asp:RadioButton ID="rbTrain" runat="server" GroupName="Transport" 
                                    Value="Train" />
                                <i class="fas fa-train"></i>
                                <span>Train</span>
                            </label>
                            <label class="transport-option">
                                <asp:RadioButton ID="rbAvion" runat="server" GroupName="Transport" 
                                    Value="Avion" />
                                <i class="fas fa-plane"></i>
                                <span>Avion</span>
                            </label>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Section Détails Financiers -->
            <div class="form-section">
                <h3 class="section-title">
                    <i class="fas fa-calculator"></i>
                    Détails Financiers et Techniques
                </h3>
                <div class="form-grid">
                    <div class="form-group">
                        <label for="txtMontant">Montant (DH)</label>
                        <asp:TextBox ID="txtMontant" runat="server" CssClass="form-control" 
                            placeholder="0.00" TextMode="Number" step="0.01" 
                            onchange="calculateTotal()" />
                    </div>
                    <div class="form-group">
                        <label for="txtTaux">Taux</label>
                        <asp:TextBox ID="txtTaux" runat="server" CssClass="form-control" 
                            placeholder="0.00" TextMode="Number" step="0.01" />
                    </div>
                    <div class="form-group">
                        <label for="txtNbJour">Nombre de Jours</label>
                        <asp:TextBox ID="txtNbJour" runat="server" CssClass="form-control" 
                            placeholder="0" TextMode="Number" 
                            onchange="calculateTotal()" />
                    </div>
                    <div class="form-group">
                        <label for="txtTauxJour">Taux par Jour (DH)</label>
                        <asp:TextBox ID="txtTauxJour" runat="server" CssClass="form-control" 
                            placeholder="0.00" TextMode="Number" step="0.01" 
                            onchange="calculateTotal()" />
                    </div>
                    <div class="form-group">
                        <label for="txtPuissFisc">Puissance Fiscale</label>
                        <asp:TextBox ID="txtPuissFisc" runat="server" CssClass="form-control" 
                            placeholder="Ex: 7CV" />
                    </div>
                    <div class="form-group">
                        <label for="txtKilometrage">Kilométrage</label>
                        <asp:TextBox ID="txtKilometrage" runat="server" CssClass="form-control" 
                            placeholder="0" TextMode="Number" step="0.01" />
                    </div>
                    <div class="form-group">
                        <label for="txtNbVacation">Nombre de Vacations</label>
                        <asp:TextBox ID="txtNbVacation" runat="server" CssClass="form-control" 
                            placeholder="0" TextMode="Number" />
                    </div>
                    <div class="form-group">
                        <label for="txtTotalLettre">Total en Lettres</label>
                        <asp:TextBox ID="txtTotalLettre" runat="server" CssClass="form-control" 
                            placeholder="Ex: Mille deux cents dirhams" TextMode="MultiLine" Rows="2" />
                    </div>
                </div>

                <!-- Total calculé -->
                <div class="calculated-field">
                    <div class="calculated-value" id="totalCalcule">0.00 DH</div>
                    <div class="calculated-label">Total Estimé (Montant + Taux/Jour × Nb Jours)</div>
                </div>
            </div>

            <!-- Actions -->
            <div class="form-actions">
                <asp:Button ID="btnSave" runat="server" Text="Enregistrer" 
                    CssClass="btn btn-primary" OnClick="btnSave_Click" />
                <a href="ListeMissions.aspx" class="btn btn-secondary">
                    <i class="fas fa-arrow-left"></i>
                    Retour à la liste
                </a>
            </div>
        </div>
    </div>
</asp:Content>