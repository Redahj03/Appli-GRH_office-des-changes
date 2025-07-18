<%@ Page Title="Gestion des Missions" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ListeMissions.aspx.cs" Inherits="GestionRHv2.Pages.Missions.ListeMissions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .missions-container {
            max-width: 1400px;
            margin: 0 auto;
            padding: 20px;
        }

        .missions-header {
            background: white;
            border-radius: 10px;
            padding: 30px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            margin-bottom: 30px;
        }

        .missions-title {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin-bottom: 30px;
        }

        .missions-title h1 {
            color: #2c3e50;
            font-size: 2.5rem;
            margin: 0;
            display: flex;
            align-items: center;
            gap: 15px;
        }

        .missions-title h1 i {
            color: #3498db;
        }

        .btn-nouvelle-mission {
            background: linear-gradient(135deg, #27ae60 0%, #2ecc71 100%);
            color: white;
            border: none;
            padding: 12px 30px;
            border-radius: 25px;
            font-size: 16px;
            font-weight: 600;
            cursor: pointer;
            display: flex;
            align-items: center;
            gap: 10px;
            transition: all 0.3s;
            text-decoration: none;
        }

        .btn-nouvelle-mission:hover {
            transform: translateY(-2px);
            box-shadow: 0 5px 20px rgba(46, 204, 113, 0.4);
            color: white;
            text-decoration: none;
        }

        /* Statistiques */
        .stats-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 20px;
            margin-bottom: 30px;
        }

        .stat-card {
            background: white;
            border-radius: 10px;
            padding: 20px;
            text-align: center;
            border: 1px solid #e0e0e0;
            transition: all 0.3s;
        }

        .stat-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 5px 20px rgba(0,0,0,0.1);
        }

        .stat-icon {
            font-size: 2.5rem;
            margin-bottom: 10px;
        }

        .stat-value {
            font-size: 2rem;
            font-weight: 700;
            color: #2c3e50;
            margin-bottom: 5px;
        }

        .stat-label {
            color: #7f8c8d;
            font-size: 14px;
        }

        /* Zone de recherche */
        .search-filters {
            background: white;
            border-radius: 10px;
            padding: 25px;
            margin-bottom: 30px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.05);
        }

        .search-row {
            display: flex;
            gap: 15px;
            align-items: flex-end;
            flex-wrap: wrap;
        }

        .search-field {
            flex: 1;
            min-width: 200px;
        }

        .search-field label {
            display: block;
            margin-bottom: 5px;
            color: #495057;
            font-weight: 500;
        }

        .search-field input,
        .search-field select {
            width: 100%;
            padding: 10px 15px;
            border: 1px solid #ddd;
            border-radius: 5px;
            font-size: 14px;
        }

        .btn-search,
        .btn-reset,
        .btn-export {
            padding: 10px 25px;
            border: none;
            border-radius: 5px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s;
            display: flex;
            align-items: center;
            gap: 8px;
        }

        .btn-search {
            background: #3498db;
            color: white;
        }

        .btn-search:hover {
            background: #2980b9;
        }

        .btn-reset {
            background: #95a5a6;
            color: white;
        }

        .btn-reset:hover {
            background: #7f8c8d;
        }

        .btn-export {
            background: #e74c3c;
            color: white;
        }

        .btn-export:hover {
            background: #c0392b;
        }

        /* Tableau */
        .table-container {
            background: white;
            border-radius: 10px;
            padding: 20px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.05);
            overflow-x: auto;
        }

        .missions-table {
            width: 100%;
            border-collapse: collapse;
        }

        .missions-table th {
            background: #f8f9fa;
            padding: 15px 10px;
            text-align: left;
            font-weight: 600;
            color: #495057;
            border-bottom: 2px solid #dee2e6;
            position: sticky;
            top: 0;
            z-index: 10;
        }

        .missions-table td {
            padding: 12px 10px;
            border-bottom: 1px solid #e9ecef;
        }

        .missions-table tr:hover {
            background: #f8f9fa;
        }

        .null-value {
            color: #bdc3c7;
            font-style: italic;
        }

        .action-buttons {
            display: flex;
            gap: 5px;
        }

        .btn-action {
            padding: 5px 10px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
            transition: all 0.2s;
        }

        .btn-view {
            background: #3498db;
            color: white;
        }

        .btn-view:hover {
            background: #2980b9;
        }

        .btn-edit {
            background: #f39c12;
            color: white;
        }

        .btn-edit:hover {
            background: #e67e22;
        }

        .btn-delete {
            background: #e74c3c;
            color: white;
        }

        .btn-delete:hover {
            background: #c0392b;
        }

        /* Badge pour les moyens de transport */
        .transport-badge {
            padding: 3px 10px;
            border-radius: 15px;
            font-size: 12px;
            font-weight: 600;
        }

        .transport-voiture {
            background: #e8f5e9;
            color: #2e7d32;
        }

        .transport-train {
            background: #e3f2fd;
            color: #1565c0;
        }

        .transport-avion {
            background: #fce4ec;
            color: #c2185b;
        }

        /* Responsive */
        @media (max-width: 768px) {
            .missions-title {
                flex-direction: column;
                gap: 20px;
                text-align: center;
            }

            .search-row {
                flex-direction: column;
            }

            .search-field {
                width: 100%;
            }

            .table-container {
                padding: 10px;
            }

            .missions-table {
                font-size: 14px;
            }

            .missions-table th,
            .missions-table td {
                padding: 8px 5px;
            }

            .action-buttons {
                flex-direction: column;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="missions-container">
        <!-- Header avec statistiques -->
        <div class="missions-header">
            <div class="missions-title">
                <h1><i class="fas fa-route"></i> Gestion des Missions</h1>
                <a href="AjouterMission.aspx" class="btn-nouvelle-mission" runat="server">
                    <i class="fas fa-plus-circle"></i>
                    Nouvelle Mission
                </a>
            </div>

            <!-- Statistiques -->
            <div class="stats-grid">
                <div class="stat-card">
                    <div class="stat-icon" style="color: #3498db;">
                        <i class="fas fa-clipboard-list"></i>
                    </div>
                    <div class="stat-value"><asp:Label ID="lblTotalMissions" runat="server" Text="0"></asp:Label></div>
                    <div class="stat-label">Total Missions</div>
                </div>
                <div class="stat-card">
                    <div class="stat-icon" style="color: #2ecc71;">
                        <i class="fas fa-users"></i>
                    </div>
                    <div class="stat-value"><asp:Label ID="lblTotalAgents" runat="server" Text="0"></asp:Label></div>
                    <div class="stat-label">Agents en Mission</div>
                </div>
                <div class="stat-card">
                    <div class="stat-icon" style="color: #e74c3c;">
                        <i class="fas fa-money-bill-wave"></i>
                    </div>
                    <div class="stat-value"><asp:Label ID="lblMontantTotal" runat="server" Text="0"></asp:Label></div>
                    <div class="stat-label">Montant Total (DH)</div>
                </div>
                <div class="stat-card">
                    <div class="stat-icon" style="color: #f39c12;">
                        <i class="fas fa-tachometer-alt"></i>
                    </div>
                    <div class="stat-value"><asp:Label ID="lblKilometrage" runat="server" Text="0"></asp:Label></div>
                    <div class="stat-label">Kilomètres Total</div>
                </div>
                <div class="stat-card">
                    <div class="stat-icon" style="color: #9b59b6;">
                        <i class="fas fa-calendar-check"></i>
                    </div>
                    <div class="stat-value"><asp:Label ID="lblMissionsMois" runat="server" Text="0"></asp:Label></div>
                    <div class="stat-label">Missions ce Mois</div>
                </div>
            </div>
        </div>

        <!-- Zone de recherche -->
        <div class="search-filters">
            <h3 style="margin-bottom: 20px; color: #2c3e50;">
                <i class="fas fa-search"></i> Rechercher une Mission
            </h3>
            <div class="search-row">
                <div class="search-field">
                    <label>Recherche</label>
                    <asp:TextBox ID="txtSearch" runat="server" placeholder="Matricule, nom, grade..." CssClass="form-control"></asp:TextBox>
                </div>
                <div class="search-field">
                    <label>Date début</label>
                    <asp:TextBox ID="txtDateDebut" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="search-field">
                    <label>Date fin</label>
                    <asp:TextBox ID="txtDateFin" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                </div>
                <asp:Button ID="btnSearch" runat="server" Text="Rechercher" CssClass="btn-search" OnClick="btnSearch_Click" />
                <asp:Button ID="btnReset" runat="server" Text="Réinitialiser" CssClass="btn-reset" OnClick="btnReset_Click" />
                <asp:Button ID="btnExportPDF" runat="server" Text="Export PDF" CssClass="btn-export" OnClick="btnExportPDF_Click" />
            </div>
        </div>

        <!-- Tableau des missions -->
        <div class="table-container">
            <asp:GridView ID="gvMissions" runat="server" CssClass="missions-table" 
                AutoGenerateColumns="False" DataKeyNames="Matricule,Date_deplacement"
                OnRowCommand="gvMissions_RowCommand" OnRowDataBound="gvMissions_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="Matricule" HeaderText="Matricule" />
                    <asp:BoundField DataField="Nom" HeaderText="Nom" />
                    <asp:BoundField DataField="Grade" HeaderText="Grade" />
                    <asp:BoundField DataField="Date_deplacement" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Moyen Transport">
                        <ItemTemplate>
                            <span class='<%# GetTransportClass(Eval("Moyen_trans")) %>'>
                                <%# GetValue(Eval("Moyen_trans")) %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Montant" HeaderText="Montant (DH)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="Kilometrage" HeaderText="Km" />
                    <asp:BoundField DataField="Nb_vacation" HeaderText="Vacations" />
                    <asp:BoundField DataField="Nb_Jour" HeaderText="Jours" />
                    <asp:BoundField DataField="Taux_Jour" HeaderText="Taux/Jour" DataFormatString="{0:N2}" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <div class="action-buttons">
                                <asp:LinkButton ID="btnView" runat="server" CommandName="View" 
                                    CommandArgument='<%# Eval("Matricule") + ";" + Eval("Date_deplacement") %>' 
                                    CssClass="btn-action btn-view" ToolTip="Voir">
                                    <i class="fas fa-eye"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" 
                                    CommandArgument='<%# Eval("Matricule") + ";" + Eval("Date_deplacement") %>' 
                                    CssClass="btn-action btn-edit" ToolTip="Modifier">
                                    <i class="fas fa-edit"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" 
                                    CommandArgument='<%# Eval("Matricule") + ";" + Eval("Date_deplacement") %>' 
                                    CssClass="btn-action btn-delete" ToolTip="Supprimer"
                                    OnClientClick="return confirm('Êtes-vous sûr de vouloir supprimer cette mission ?');">
                                    <i class="fas fa-trash"></i>
                                </asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div style="text-align: center; padding: 40px; color: #7f8c8d;">
                        <i class="fas fa-inbox" style="font-size: 3rem; margin-bottom: 10px;"></i>
                        <p>Aucune mission trouvée</p>
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>