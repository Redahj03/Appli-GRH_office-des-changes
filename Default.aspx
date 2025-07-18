<%@ Page Title="Accueil" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GestionRHv2._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .welcome-container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 40px 20px;
        }
        
        .welcome-card {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 60px;
            border-radius: 20px;
            text-align: center;
            box-shadow: 0 10px 40px rgba(0,0,0,0.2);
        }
        
        .welcome-title {
            font-size: 3rem;
            margin-bottom: 20px;
        }
        
        .welcome-subtitle {
            font-size: 1.3rem;
            opacity: 0.9;
            margin-bottom: 40px;
        }
        
        .stats-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 30px;
            margin-top: 50px;
        }
        
        .stat-card {
            background: white;
            padding: 30px;
            border-radius: 15px;
            text-align: center;
            box-shadow: 0 5px 20px rgba(0,0,0,0.1);
            transition: transform 0.3s;
        }
        
        .stat-card:hover {
            transform: translateY(-5px);
        }
        
        .stat-icon {
            font-size: 3rem;
            margin-bottom: 15px;
        }
        
        .stat-number {
            font-size: 2.5rem;
            font-weight: 700;
            color: #2c3e50;
        }
        
        .stat-label {
            color: #7f8c8d;
            font-size: 1.1rem;
        }
        
        .quick-actions {
            margin-top: 50px;
            text-align: center;
        }
        
        .action-btn {
            display: inline-block;
            margin: 10px;
            padding: 15px 35px;
            background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
            color: white;
            text-decoration: none;
            border-radius: 50px;
            font-weight: 600;
            transition: all 0.3s;
        }
        
        .action-btn:hover {
            transform: translateY(-3px);
            box-shadow: 0 10px 25px rgba(52, 152, 219, 0.3);
        }
        
        .action-btn i {
            margin-right: 8px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="welcome-container">
        <div class="welcome-card">
            <h1 class="welcome-title">
                <i class="fas fa-users-cog"></i><br>
                Bienvenue dans le Système de Gestion RH
            </h1>
            <p class="welcome-subtitle">
                Gérez efficacement vos ressources humaines
            </p>
        </div>
        
        <div class="stats-grid">
            <div class="stat-card">
                <div class="stat-icon" style="color: #3498db;">
                    <i class="fas fa-users"></i>
                </div>
                <div class="stat-number">
                    <asp:Label ID="lblTotalAgents" runat="server" Text="0"></asp:Label>
                </div>
                <div class="stat-label">Total Agents</div>
            </div>
            
            <div class="stat-card">
                <div class="stat-icon" style="color: #2ecc71;">
                    <i class="fas fa-user-check"></i>
                </div>
                <div class="stat-number">
                    <asp:Label ID="lblAgentsActifs" runat="server" Text="0"></asp:Label>
                </div>
                <div class="stat-label">Agents Actifs</div>
            </div>
            
            <div class="stat-card">
                <div class="stat-icon" style="color: #e74c3c;">
                    <i class="fas fa-user-clock"></i>
                </div>
                <div class="stat-number">
                    <asp:Label ID="lblRetraites" runat="server" Text="0"></asp:Label>
                </div>
                <div class="stat-label">Retraités</div>
            </div>
            
            <div class="stat-card">
                <div class="stat-icon" style="color: #f39c12;">
                    <i class="fas fa-city"></i>
                </div>
                <div class="stat-number">
                    <asp:Label ID="lblVilles" runat="server" Text="0"></asp:Label>
                </div>
                <div class="stat-label">Villes</div>
            </div>
        </div>
        
        <div class="quick-actions">
            <h2>Actions Rapides</h2>
            <a href="/Pages/Agents/AjouterAgent.aspx" class="action-btn">
                <i class="fas fa-user-plus"></i>
                Ajouter un Agent
            </a>
            <a href="/Pages/Agents/ListeAgents.aspx" class="action-btn">
                <i class="fas fa-list"></i>
                Voir tous les Agents
            </a>
            <a href="/Pages/Agents/RechercheAgent.aspx" class="action-btn">
                <i class="fas fa-search"></i>
                Rechercher un Agent
            </a>
        </div>
    </div>
</asp:Content>