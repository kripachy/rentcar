using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using WebApplication1.App_Start;

namespace WebApplication1
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Код, выполняемый при запуске приложения
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DatabaseInitializer.EnsureSchema();
            
            // Migrate old absolute paths to relative paths (for project portability)
            FileStorageMigration.EnsureMigrated(Server);
            
            CarImageBootstrapper.EnsureSeeded(Server);
        }
    }
}