using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestionRHv2.Classes
{
    public static class MessageHelper
    {
        public enum MessageType
        {
            Success,
            Error,
            Warning,
            Info
        }

        /// <summary>
        /// Affiche un message dans un Panel avec le style approprié
        /// </summary>
        public static void ShowMessage(Panel messagePanel, Label messageLabel, string message, MessageType type)
        {
            messagePanel.Visible = true;
            messageLabel.Text = message;

            // Réinitialiser les classes CSS
            messagePanel.CssClass = "alert";

            // Appliquer la classe appropriée selon le type
            switch (type)
            {
                case MessageType.Success:
                    messagePanel.CssClass += " alert-success";
                    break;
                case MessageType.Error:
                    messagePanel.CssClass += " alert-danger";
                    break;
                case MessageType.Warning:
                    messagePanel.CssClass += " alert-warning";
                    break;
                case MessageType.Info:
                    messagePanel.CssClass += " alert-info";
                    break;
            }
        }

        /// <summary>
        /// Cache le panneau de message
        /// </summary>
        public static void HideMessage(Panel messagePanel)
        {
            messagePanel.Visible = false;
        }

        /// <summary>
        /// Crée le HTML pour un message avec icône
        /// </summary>
        public static string CreateMessageHtml(string message, MessageType type)
        {
            string icon = "";
            string alertClass = "";

            switch (type)
            {
                case MessageType.Success:
                    icon = "fa-check-circle";
                    alertClass = "alert-success";
                    break;
                case MessageType.Error:
                    icon = "fa-exclamation-circle";
                    alertClass = "alert-danger";
                    break;
                case MessageType.Warning:
                    icon = "fa-exclamation-triangle";
                    alertClass = "alert-warning";
                    break;
                case MessageType.Info:
                    icon = "fa-info-circle";
                    alertClass = "alert-info";
                    break;
            }

            return $@"
                <div class='alert {alertClass}'>
                    <i class='fas {icon}'></i>
                    {message}
                </div>";
        }

        /// <summary>
        /// Redirige vers une page avec un message dans la session
        /// </summary>
        public static void RedirectWithMessage(Page page, string url, string message, MessageType type)
        {
            page.Session["TempMessage"] = message;
            page.Session["TempMessageType"] = type.ToString();
            page.Response.Redirect(url);
        }

        /// <summary>
        /// Vérifie et affiche un message temporaire de la session
        /// </summary>
        public static void CheckAndShowTempMessage(Page page, Panel messagePanel, Label messageLabel)
        {
            if (page.Session["TempMessage"] != null)
            {
                string message = page.Session["TempMessage"].ToString();
                MessageType type = (MessageType)System.Enum.Parse(typeof(MessageType),
                    page.Session["TempMessageType"].ToString());

                ShowMessage(messagePanel, messageLabel, message, type);

                // Nettoyer la session
                page.Session.Remove("TempMessage");
                page.Session.Remove("TempMessageType");
            }
        }
    }

    /// <summary>
    /// Classe pour standardiser les messages de l'application
    /// </summary>
    public static class StandardMessages
    {
        // Messages de succès
        public const string AJOUT_REUSSI = "Enregistrement ajouté avec succès !";
        public const string MODIFICATION_REUSSIE = "Modifications enregistrées avec succès !";
        public const string SUPPRESSION_REUSSIE = "Suppression effectuée avec succès !";

        // Messages d'erreur
        public const string ERREUR_GENERALE = "Une erreur s'est produite. Veuillez réessayer.";
        public const string ERREUR_VALIDATION = "Veuillez vérifier les informations saisies.";
        public const string ERREUR_SUPPRESSION = "Impossible de supprimer cet enregistrement.";
        public const string ERREUR_ENFANT_INTROUVABLE = "Enfant introuvable.";
        public const string ERREUR_AGE_LIMITE = "L'enfant doit avoir moins de 21 ans.";
        public const string ERREUR_DATE_FUTURE = "La date de naissance ne peut pas être dans le futur.";

        // Messages d'information
        public const string AUCUN_RESULTAT = "Aucun résultat trouvé.";
        public const string CHARGEMENT_EN_COURS = "Chargement en cours...";

        // Messages de confirmation
        public const string CONFIRMER_SUPPRESSION = "Êtes-vous sûr de vouloir supprimer cet enregistrement ?";
    }
}