#region Using

using SecurePort.Entities;

using System.Data.Entity;
using System.Web.Optimization;
using System.Web.Routing;

#endregion

namespace SecurePort.Web
{
    #region Using
    using System;
    using System.Web;
    using log4net.Config;

    #endregion

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            XmlConfigurator.Configure();
            Bootstrapper.Initialise();
            Database.SetInitializer(new ValidateDatabaseInitializer<SecurePortContext>());
        }
    }
}
