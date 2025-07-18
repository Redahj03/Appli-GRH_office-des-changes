using System;
using System.Web;
using System.Web.Routing;

namespace GestionRHv2
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Configuration des routes
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // BundleConfig commenté car on ne l'a pas
            // BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}