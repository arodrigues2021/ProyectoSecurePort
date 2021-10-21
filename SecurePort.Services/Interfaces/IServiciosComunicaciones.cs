using System.Collections.Generic;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    public interface IServiciosComunicaciones
    {

        IEnumerable<ComunicacionesViewModel> GetAllComunicaciones();

        IEnumerable<ComunicacionesViewModel> GetAllComunicaciones(List<DependenciasUsuario> dependencias,int? categoria);
    }
}
