<%@ Page Title="Modifier un Enfant" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ModifierEnfant.aspx.cs" Inherits="GestionRHv2.Pages.Enfants.ModifierEnfant" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-card {
            background: white;
            border-radius: 8px;
            padding: 30px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            margin-top: 20px;
        }
        .form-header {
            display: flex;
            align-items: center;
            gap: 10px;
            margin-bottom: 30px;
            padding-bottom: 20px;
            border-bottom: 2px solid #f0f0f0;
        }
        .form-header h2 {
            margin: 0;
        }
        .form-group {
            margin-bottom: 20px;
        }
        .form-group label {
            font-weight: bold;
            margin-bottom: 5px;
            display: block;
        }
        .form-control {
            width: 100%;
            padding: 8px 12px;
            border: 1px solid #ddd;
            border-radius: 4px;
            font-size: 14px;
        }
        .form-control:focus {
            border-color: #FF9800;
            outline: none;
            box-shadow: 0 0 0 2px rgba(255, 152, 0, 0.1);
        }
        .form-control:disabled {
            background-color: #f5f5f5;
            cursor: not-allowed;
        }
        .required {
            color: red;
        }
        .radio-group {
            display: flex;
            gap: 20px;
            margin-top: 5px;
        }
        .radio-group label {
            font-weight: normal;
            display: flex;
            align-items: center;
            gap: 5px;
        }
        .btn-group {
            display: flex;
            gap: 10px;
            margin-top: 30px;
        }
        .btn {
            padding: 10px 20px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
            text-decoration: none;
            display: inline-block;
        }
        .btn-warning {
            background-color: #FF9800;
            color: white;
        }
        .btn-warning:hover {
            background-color: #F57C00;
        }
        .btn-secondary {
            background-color: #757575;
            color: white;
        }
        .btn-secondary:hover {
            background-color: #616161;
        }
        .row {
            display: flex;
            gap: 20px;
        }
        .col-md-6 {
            flex: 1;
        }
        .info-card {
            background-color: #FFF3E0;
            border-left: 4px solid #FF9800;
            padding: 15px;
            margin-bottom: 20px;
            border-radius: 4px;
        }
        .validation-error {
            color: red;
            font-size: 12px;
            margin-top: 5px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="form-card">
            <div class="form-header">
                <span style="font-size: 2em;">✏️</span>
                <h2>Modifier un Enfant</h2>
            </div>
            
            <!-- Message -->
            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert" role="alert">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </asp:Panel>
            
            <!-- Information de modification -->
            <div class="info-card">
                <strong>Information :</strong> Vous modifiez l'enfant N°<asp:Label ID="lblNumEnfant" runat="server" Font-Bold="true"></asp:Label> 
                de l'agent <asp:Label ID="lblAgentInfo" runat="server" Font-Bold="true"></asp:Label>
            </div>
            
            <!-- Champs non modifiables -->
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label>Matricule de l'Agent</label>
                        <asp:TextBox ID="txtMatricule" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label>Numéro de l'Enfant</label>
                        <asp:TextBox ID="txtNum" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </div>
            
            <!-- Champs modifiables -->
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label>Nom <span class="required">*</span></label>
                        <asp:TextBox ID="txtNom" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvNom" runat="server" 
                            ControlToValidate="txtNom" ErrorMessage="Le nom est requis" 
                            CssClass="validation-error" Display="Dynamic" ValidationGroup="vgEnfant" />
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label>Prénom <span class="required">*</span></label>
                        <asp:TextBox ID="txtPrenom" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPrenom" runat="server" 
                            ControlToValidate="txtPrenom" ErrorMessage="Le prénom est requis" 
                            CssClass="validation-error" Display="Dynamic" ValidationGroup="vgEnfant" />
                    </div>
                </div>
            </div>
            
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label>Sexe <span class="required">*</span></label>
                        <asp:RadioButtonList ID="rblSexe" runat="server" CssClass="radio-group" 
                            RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Value="M">♂ Masculin</asp:ListItem>
                            <asp:ListItem Value="F">♀ Féminin</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label>Date de Naissance <span class="required">*</span></label>
                        <asp:TextBox ID="txtDateNaissance" runat="server" CssClass="form-control" 
                            TextMode="Date"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDateNaissance" runat="server" 
                            ControlToValidate="txtDateNaissance" ErrorMessage="La date de naissance est requise" 
                            CssClass="validation-error" Display="Dynamic" ValidationGroup="vgEnfant" />
                        <asp:CompareValidator ID="cvDateNaissance" runat="server" 
                            ControlToValidate="txtDateNaissance" Operator="LessThan" 
                            Type="Date" ErrorMessage="La date doit être dans le passé" 
                            CssClass="validation-error" Display="Dynamic" ValidationGroup="vgEnfant" />
                    </div>
                </div>
            </div>
            
            <div class="form-group">
                <label>Lieu de Naissance</label>
                <asp:TextBox ID="txtLieuNaissance" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
            </div>
            
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label>Scolarisé</label>
                        <asp:RadioButtonList ID="rblScolarise" runat="server" CssClass="radio-group" 
                            RepeatDirection="Horizontal" RepeatLayout="Flow"
                            AutoPostBack="true" OnSelectedIndexChanged="rblScolarise_SelectedIndexChanged">
                            <asp:ListItem Value="O">Oui</asp:ListItem>
                            <asp:ListItem Value="N">Non</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="col-md-6">
                    <asp:Panel ID="pnlScolarite" runat="server">
                        <div class="form-group">
                            <label>Code Scolarité</label>
                            <asp:TextBox ID="txtCodSc" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                        </div>
                    </asp:Panel>
                </div>
            </div>
            
            <asp:Panel ID="pnlDateScolarite" runat="server">
                <div class="form-group">
                    <label>Date de Première Scolarisation</label>
                    <asp:TextBox ID="txtDatePSc" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
            </asp:Panel>
            
            <div class="form-group">
                <label>CH</label>
                <asp:TextBox ID="txtCh" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
            </div>
            
            <div class="btn-group">
                <asp:Button ID="btnModifier" runat="server" Text="💾 Enregistrer les modifications" 
                    CssClass="btn btn-warning" OnClick="btnModifier_Click" 
                    ValidationGroup="vgEnfant" />
                <a href="ListeEnfants.aspx" class="btn btn-secondary">❌ Annuler</a>
            </div>
        </div>
    </div>
    
    <script type="text/javascript">
        // Définir la date maximale à aujourd'hui pour la date de naissance
        window.onload = function () {
            var today = new Date().toISOString().split('T')[0];
            document.getElementById('<%= txtDateNaissance.ClientID %>').setAttribute('max', today);
            document.getElementById('<%= txtDatePSc.ClientID %>').setAttribute('max', today);
            
            // Configurer le comparateur de date
            var cv = document.getElementById('<%= cvDateNaissance.ClientID %>');
            if (cv) {
                cv.valuetocompare = today;
            }
        };
    </script>
</asp:Content>