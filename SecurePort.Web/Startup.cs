using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SecurePort.Web.Startup))]
namespace SecurePort.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }

}
