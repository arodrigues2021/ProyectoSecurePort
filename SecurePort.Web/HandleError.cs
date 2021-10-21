using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SecurePortGestorServicios.Servicios
{
    public class HandleErrorAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;
            if (filterContext.Exception is HttpException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (filterContext.Exception is UnauthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Forbidden;
            }

        }
    }
}
