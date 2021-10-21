using Owin;

namespace SecurePort.Web
{

    public partial class Startup
    {

        // Para obtener más información para configurar la autenticación, visite http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            //app.CreatePerOwinContext(SecurePortContext.Create);
        }
    }
}