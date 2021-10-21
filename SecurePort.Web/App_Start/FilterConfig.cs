using SecurePortGestorServicios.Servicios;
using System.Web.Mvc;

namespace WebSecurePort.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new SecurePortGestorServicios.Servicios.HandleErrorAttribute());
        }
    }

}