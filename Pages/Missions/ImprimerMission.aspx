<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImprimerMission.aspx.cs" Inherits="GestionRHv2.Pages.Missions.ImprimerMission" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Ordre de Mission - Impression</title>
    <style>
        @page {
            size: A4;
            margin: 2cm;
        }
        
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }
        
        body {
            font-family: 'Times New Roman', Times, serif;
            font-size: 12pt;
            line-height: 1.6;
            color: #000;
            background: white;
        }
        
        .print-container {
            max-width: 21cm;
            margin: 0 auto;
            padding: 1cm;
            background: white;
        }
        
        /* En-tête du document */
        .document-header {
            text-align: center;
            margin-bottom: 30px;
            border-bottom: 3px double #000;
            padding-bottom: 20px;
        }
        
        .organization-name {
            font-size: 18pt;
            font-weight: bold;
            text-transform: uppercase;
            margin-bottom: 10px;
        }
        
        .document-title {
            font-size: 20pt;
            font-weight: bold;
            text-transform: uppercase;
            margin: 20px 0;
            text-decoration: underline;
        }
        
        .document-number {
            font-size: 14pt;
            margin-bottom: 10px;
        }
        
        /* Informations principales */
        .mission-info {
            margin: 30px 0;
        }
        
        .info-section {
            margin-bottom: 25px;
        }
        
        .info-section h3 {
            font-size: 14pt;
            font-weight: bold;
            text-transform: uppercase;
            margin-bottom: 10px;
            border-bottom: 1px solid #000;
            padding-bottom: 5px;
        }
        
        .info-row {
            display: flex;
            margin-bottom: 8px;
        }
        
        .info-label {
            font-weight: bold;
            width: 200px;
            flex-shrink: 0;
        }
        
        .info-value {
            flex: 1;
        }
        
        /* Section objet */
        .mission-object {
            margin: 30px 0;
            padding: 15px;
            border: 1px solid #000;
            background: #f9f9f9;
        }
        
        .mission-object h3 {
            font-size: 14pt;
            font-weight: bold;
            margin-bottom: 10px;
        }
        
        .mission-object p {
            text-align: justify;
            line-height: 1.8;
        }
        
        /* Tableau récapitulatif */
        .summary-table {
            width: 100%;
            border-collapse: collapse;
            margin: 30px 0;
        }
        
        .summary-table th,
        .summary-table td {
            border: 1px solid #000;
            padding: 10px;
            text-align: left;
        }
        
        .summary-table th {
            background: #e0e0e0;
            font-weight: bold;
            text-align: center;
        }
        
        .summary-table td.center {
            text-align: center;
        }
        
        /* Section financière */
        .financial-section {
            margin: 30px 0;
            padding: 15px;
            border: 2px solid #000;
        }
        
        .financial-section h3 {
            font-size: 14pt;
            font-weight: bold;
            margin-bottom: 15px;
            text-align: center;
        }
        
        .financial-details {
            margin: 15px 0;
        }
        
        .financial-row {
            display: flex;
            justify-content: space-between;
            margin-bottom: 10px;
            padding: 5px 0;
            border-bottom: 1px dotted #666;
        }
        
        .financial-row:last-child {
            border-bottom: none;
            font-weight: bold;
            font-size: 14pt;
            padding-top: 10px;
            border-top: 2px solid #000;
        }
        
        /* Section signatures */
        .signatures-section {
            margin-top: 50px;
            page-break-inside: avoid;
        }
        
        .signatures-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 50px;
            margin-top: 30px;
        }
        
        .signature-box {
            text-align: center;
        }
        
        .signature-title {
            font-weight: bold;
            margin-bottom: 50px;
        }
        
        .signature-line {
            border-bottom: 1px solid #000;
            margin-bottom: 5px;
            height: 40px;
        }
        
        .signature-name {
            font-size: 10pt;
        }
        
        /* Notes de bas de page */
        .footer-notes {
            margin-top: 50px;
            padding-top: 20px;
            border-top: 1px solid #666;
            font-size: 10pt;
            color: #666;
        }
        
        .footer-notes p {
            margin-bottom: 5px;
        }
        
        /* Styles d'impression */
        @media print {
            body {
                margin: 0;
                padding: 0;
            }
            
            .print-container {
                margin: 0;
                padding: 0;
            }
            
            .no-print {
                display: none !important;
            }
            
            .signatures-section {
                position: absolute;
                bottom: 0;
                width: 100%;
            }
        }
        
        /* Boutons d'action (non imprimables) */
        .print-actions {
            position: fixed;
            top: 20px;
            right: 20px;
            display: flex;
            gap: 10px;
            z-index: 1000;
        }
        
        .print-btn {
            padding: 10px 20px;
            background: #4CAF50;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 14px;
            font-weight: bold;
            text-decoration: none;
            display: inline-block;
        }
        
        .print-btn:hover {
            background: #45a049;
        }
        
        .print-btn.secondary {
            background: #666;
        }
        
        .print-btn.secondary:hover {
            background: #555;
        }
        
        /* Cachet officiel */
        .official-stamp {
            position: absolute;
            right: 50px;
            top: 50px;
            width: 150px;
            height: 150px;
            border: 3px solid #000;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-weight: bold;
            text-align: center;
            padding: 20px;
            background: rgba(255, 255, 255, 0.9);
        }
    </style>
</head>
<body onload="window.print();">
    <form id="form1" runat="server">
        <!-- Boutons d'action -->
        <div class="print-actions no-print">
            <button type="button" class="print-btn" onclick="window.print();">
                Imprimer
            </button>
            <a href="ListeMissions.aspx" class="print-btn secondary">
                Retour
            </a>
        </div>
        
        <div class="print-container">
            <!-- En-tête -->
            <div class="document-header">
                <div class="organization-name">
                    ROYAUME DU MAROC<br/>
                    MINISTÈRE DE [VOTRE MINISTÈRE]<br/>
                    [VOTRE ORGANISATION]
                </div>
                <div class="document-title">
                    ORDRE DE MISSION
                </div>
                <div class="document-number">
                    N° <asp:Label ID="lblNumeroMission" runat="server"></asp:Label> / <asp:Label ID="lblAnnee" runat="server"></asp:Label>
                </div>
                <div style="font-size: 11pt; margin-top: 10px;">
                    Date d'émission : <asp:Label ID="lblDateEmission" runat="server"></asp:Label>
                </div>
            </div>
            
            <!-- Cachet officiel -->
            <div class="official-stamp no-print">
                CACHET<br/>
                OFFICIEL
            </div>
            
            <!-- Informations de l'agent -->
            <div class="info-section">
                <h3>Identification de l'Agent</h3>
                <div class="info-row">
                    <span class="info-label">Matricule :</span>
                    <span class="info-value"><asp:Label ID="lblMatricule" runat="server"></asp:Label></span>
                </div>
                <div class="info-row">
                    <span class="info-label">Nom et Prénom :</span>
                    <span class="info-value"><asp:Label ID="lblNomComplet" runat="server"></asp:Label></span>
                </div>
                <div class="info-row">
                    <span class="info-label">Grade :</span>
                    <span class="info-value"><asp:Label ID="lblGrade" runat="server"></asp:Label></span>
                </div>
                <div class="info-row">
                    <span class="info-label">Fonction :</span>
                    <span class="info-value"><asp:Label ID="lblFonction" runat="server"></asp:Label></span>
                </div>
                <div class="info-row">
                    <span class="info-label">Service :</span>
                    <span class="info-value"><asp:Label ID="lblService" runat="server"></asp:Label></span>
                </div>
            </div>
            
            <!-- Objet de la mission -->
            <div class="mission-object">
                <h3>OBJET DE LA MISSION</h3>
                <p><asp:Label ID="lblObjetMission" runat="server"></asp:Label></p>
            </div>
            
            <!-- Tableau récapitulatif -->
            <table class="summary-table">
                <thead>
                    <tr>
                        <th colspan="4">DÉTAILS DU DÉPLACEMENT</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td><strong>Destination</strong></td>
                        <td colspan="3"><asp:Label ID="lblDestination" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><strong>Date de départ</strong></td>
                        <td class="center"><asp:Label ID="lblDateDepart" runat="server"></asp:Label></td>
                        <td><strong>Heure</strong></td>
                        <td class="center"><asp:Label ID="lblHeureDepart" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><strong>Date de retour</strong></td>
                        <td class="center"><asp:Label ID="lblDateRetour" runat="server"></asp:Label></td>
                        <td><strong>Heure</strong></td>
                        <td class="center"><asp:Label ID="lblHeureRetour" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><strong>Durée de la mission</strong></td>
                        <td class="center"><asp:Label ID="lblDuree" runat="server"></asp:Label></td>
                        <td><strong>Moyen de transport</strong></td>
                        <td class="center"><asp:Label ID="lblTransport" runat="server"></asp:Label></td>
                    </tr>
                    <asp:Panel ID="pnlDetailTransport" runat="server" Visible="false">
                    <tr>
                        <td><strong>Détails transport</strong></td>
                        <td colspan="3"><asp:Label ID="lblDetailTransport" runat="server"></asp:Label></td>
                    </tr>
                    </asp:Panel>
                </tbody>
            </table>
            
            <!-- Section financière (si applicable) -->
            <asp:Panel ID="pnlFinancier" runat="server" Visible="false" CssClass="financial-section">
                <h3>INDEMNITÉS ET FRAIS DE MISSION</h3>
                <div class="financial-details">
                    <div class="financial-row">
                        <span>Indemnités journalières (<asp:Label ID="lblNbJours" runat="server"></asp:Label> jours × <asp:Label ID="lblTauxJour" runat="server"></asp:Label> DH)</span>
                        <span><asp:Label ID="lblTotalJours" runat="server"></asp:Label> DH</span>
                    </div>
                    <asp:Panel ID="pnlKilometrage" runat="server" Visible="false">
                    <div class="financial-row">
                        <span>Indemnités kilométriques (<asp:Label ID="lblKm" runat="server"></asp:Label> km × <asp:Label ID="lblTauxKm" runat="server"></asp:Label> DH)</span>
                        <span><asp:Label ID="lblTotalKm" runat="server"></asp:Label> DH</span>
                    </div>
                    </asp:Panel>
                    <div class="financial-row">
                        <span>TOTAL GÉNÉRAL</span>
                        <span><asp:Label ID="lblMontantTotal" runat="server"></asp:Label> DH</span>
                    </div>
                </div>
                <p style="text-align: center; margin-top: 15px; font-style: italic;">
                    Montant en lettres : <asp:Label ID="lblMontantLettres" runat="server" Font-Bold="true"></asp:Label>
                </p>
            </asp:Panel>
            
            <!-- Signatures -->
            <div class="signatures-section">
                <div class="signatures-grid">
                    <div class="signature-box">
                        <div class="signature-title">L'INTÉRESSÉ(E)</div>
                        <div class="signature-line"></div>
                        <div class="signature-name">Signature</div>
                    </div>
                    <div class="signature-box">
                        <div class="signature-title">LE DIRECTEUR</div>
                        <div class="signature-line"></div>
                        <div class="signature-name">Nom et Signature</div>
                    </div>
                </div>
            </div>
            
            <!-- Notes de bas de page -->
            <div class="footer-notes">
                <p><strong>Important :</strong></p>
                <p>- Cet ordre de mission doit être présenté à toute réquisition des autorités compétentes.</p>
                <p>- Les frais de mission seront remboursés sur présentation des justificatifs originaux.</p>
                <p>- Toute modification doit faire l'objet d'un avenant signé par l'autorité compétente.</p>
            </div>
        </div>
    </form>
</body>
</html>