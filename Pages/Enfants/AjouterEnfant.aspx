<%@ Page Title="Ajouter un Enfant" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AjouterEnfant.aspx.cs" Inherits="GestionRHv2.Pages.Enfants.AjouterEnfant" %>

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
            border-color: #2196F3;
            outline: none;
            box-shadow: 0 0 0 2px rgba(33, 150, 243, 0.1);
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
        .btn-primary {
            background-color: #2196F3;
            color: white;
        }
        .btn-primary:hover {
            background-color: #1976D2;
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
        .agent-info {
            background-color: #f5f5f5;
            padding: 15px;
            border-radius: 4px;
            margin-bottom: 20px;
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
                <span style="font-size: 2em;">👶</span>
                <h2>Ajouter un Enfant</h2>
            </div>
            
            <!-- Message -->
            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert" role="alert">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </asp:Panel>
            
            <!-- Information de l'agent -->
            <asp:Panel ID="pnlAgentInfo" runat="server" CssClass="agent-info" Visible="false">
                <strong>Agent parent :</strong> 
                <asp:Label ID="lblAgentInfo" runat="server"></asp:Label>
            </asp:Panel>
            
            <div class="form-group">
                <label>Matricule de l'Agent <span class="required">*</span></label>
                <asp:DropDownList ID="ddlMatricule" runat="server" CssClass="form-control" 
                    AutoPostBack="true" OnSelectedIndexChanged="ddlMatricule_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvMatricule" runat="server" 
                    ControlToValidate="ddlMatricule" InitialValue=""
                    ErrorMessage="Le matricule est requis" CssClass="validation-error" 
                    Display="Dynamic" ValidationGroup="vgEnfant" />
            </div>
            
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
                            <asp:ListItem Value="M" Selected="True">♂ Masculin</asp:ListItem>
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
                            <asp:ListItem Value="N" Selected="True">Non</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="col-md-6">
                    <asp:Panel ID="pnlScolarite" runat="server" Visible="false">
                        <div class="form-group">
                            <label>Code Scolarité</label>
                            <asp:TextBox ID="txtCodSc" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                        </div>
                    </asp:Panel>
                </div>
            </div>
            
            <asp:Panel ID="pnlDateScolarite" runat="server" Visible="false">
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
                <asp:Button ID="btnEnregistrer" runat="server" Text="💾 Enregistrer" 
                    CssClass="btn btn-primary" OnClick="btnEnregistrer_Click" 
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