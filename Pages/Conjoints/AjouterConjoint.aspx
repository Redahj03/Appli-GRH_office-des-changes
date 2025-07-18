<%@ Page Title="Ajouter/Modifier Conjoint" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AjouterConjoint.aspx.cs" Inherits="GestionRHv2.Pages.Conjoints.AjouterConjoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-container {
            max-width: 800px;
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
        }

        .form-card {
            background: white;
            border-radius: 15px;
            padding: 40px;
            box-shadow: 0 5px 20px rgba(0, 0, 0, 0.08);
        }

        .form-section {
            margin-bottom: 30px;
        }

        .section-title {
            font-size: 1.3em;
            color: #333;
            margin-bottom: 20px;
            padding-bottom: 10px;
            border-bottom: 2px solid #f0f0f0;
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .section-title i {
            color: #667eea;
        }

        .form-row {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 20px;
            margin-bottom: 20px;
        }

        .form-group {
            margin-bottom: 20px;
        }

        .form-group.full-width {
            grid-column: 1 / -1;
        }

        .form-label {
            display: block;
            color: #555;
            font-weight: 500;
            margin-bottom: 8px;
            font-size: 0.95em;
        }

        .form-control {
            width: 100%;
            padding: 12px 16px;
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            font-size: 1em;
            transition: all 0.3s ease;
            background: #f8f9fa;
        }

        .form-control:focus {
            outline: none;
            border-color: #667eea;
            background: white;
            box-shadow: 0 0 0 4px rgba(102, 126, 234, 0.1);
        }

        select.form-control {
            cursor: pointer;
            appearance: none;
            background-image: url("data:image/svg+xml;charset=UTF-8,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='none' stroke='%23667eea' stroke-width='2' stroke-linecap='round' stroke-linejoin='round'%3e%3cpolyline points='6 9 12 15 18 9'%3e%3c/polyline%3e%3c/svg%3e");
            background-repeat: no-repeat;
            background-position: right 12px center;
            background-size: 20px;
            padding-right: 40px;
        }

        .form-hint {
            font-size: 0.85em;
            color: #999;
            margin-top: 5px;
        }

        .button-group {
            display: flex;
            gap: 15px;
            margin-top: 40px;
            padding-top: 30px;
            border-top: 2px solid #f0f0f0;
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

        .alert {
            padding: 16px 20px;
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

        .alert-error {
            background: #f8d7da;
            color: #721c24;
            border: 1px solid #f5c6cb;
        }

        .alert-info {
            background: #d1ecf1;
            color: #0c5460;
            border: 1px solid #bee5eb;
        }

        @media (max-width: 768px) {
            .form-row {
                grid-template-columns: 1fr;
            }
            
            .button-group {
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
    <div class="form-container">
        <div class="page-header">
            <h1>
                <i class="fas fa-heart"></i>
                <asp:Label ID="lblPageTitle" runat="server" Text="Ajouter un conjoint"></asp:Label>
            </h1>
            <p>Remplissez le formulaire ci-dessous pour enregistrer les informations du conjoint</p>
        </div>

        <!-- Messages -->
        <asp:Panel ID="pnlMessage" runat="server" Visible="false">
            <div class="alert">
                <i class="fas fa-info-circle"></i>
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
        </asp:Panel>

        <div class="form-card">
            <asp:HiddenField ID="hdnConjointId" runat="server" />

            <!-- Section Employé -->
            <div class="form-section">
                <h3 class="section-title">
                    <i class="fas fa-user"></i>
                    Informations de l'employé
                </h3>
                <div class="form-row">
                    <div class="form-group full-width">
                        <label class="form-label">
                            Sélectionner l'employé (par matricule)
                        </label>
                        <asp:DropDownList ID="ddlEmploye" runat="server" CssClass="form-control">
                            <asp:ListItem Text="-- Sélectionner un employé --" Value=""></asp:ListItem>
                        </asp:DropDownList>
                        <span class="form-hint">Choisissez l'employé concerné par ce conjoint</span>
                    </div>
                </div>
            </div>

            <!-- Section Conjoint -->
            <div class="form-section">
                <h3 class="section-title">
                    <i class="fas fa-user-friends"></i>
                    Informations du conjoint
                </h3>
                <div class="form-row">
                    <div class="form-group">
                        <label class="form-label">
                            Nom
                        </label>
                        <asp:TextBox ID="txtNom" runat="server" CssClass="form-control" 
                                     placeholder="Entrez le nom"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="form-label">
                            Prénom
                        </label>
                        <asp:TextBox ID="txtPrenom" runat="server" CssClass="form-control" 
                                     placeholder="Entrez le prénom"></asp:TextBox>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group full-width">
                        <label class="form-label">
                            Profession
                        </label>
                        <asp:TextBox ID="txtProfession" runat="server" CssClass="form-control" 
                                     placeholder="Entrez la profession"></asp:TextBox>
                    </div>
                </div>
            </div>

            <!-- Section Mariage -->
            <div class="form-section">
                <h3 class="section-title">
                    <i class="fas fa-ring"></i>
                    Informations du mariage
                </h3>
                <div class="form-row">
                    <div class="form-group">
                        <label class="form-label">
                            Date de mariage
                        </label>
                        <asp:TextBox ID="txtDateMariage" runat="server" CssClass="form-control" 
                                     TextMode="Date"></asp:TextBox>
                        <span class="form-hint">Format: JJ/MM/AAAA</span>
                    </div>
                    <div class="form-group">
                        <label class="form-label">
                            Allocation familiale
                        </label>
                        <asp:DropDownList ID="ddlAllocation" runat="server" CssClass="form-control">
                            <asp:ListItem Text="-- Sélectionner --" Value=""></asp:ListItem>
                            <asp:ListItem Text="Oui" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Non" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <span class="form-hint">Le conjoint bénéficie-t-il de l'allocation familiale?</span>
                    </div>
                </div>
            </div>

            <!-- Boutons d'action -->
            <div class="button-group">
                <asp:Button ID="btnSave" runat="server" Text="Enregistrer" 
                            CssClass="btn btn-primary" OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Annuler" 
                            CssClass="btn btn-secondary" OnClick="btnCancel_Click" />
                <a href="ListeConjoints.aspx" class="btn btn-secondary">
                    <i class="fas fa-arrow-left"></i> Retour à la liste
                </a>
            </div>
        </div>
    </div>
</asp:Content>