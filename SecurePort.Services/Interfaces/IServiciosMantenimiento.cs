using System.Collections.Generic;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    public interface IServiciosMantenimiento
    {

        IEnumerable<MantenimientoViewModel> GetAllMantenimiento();

        IEnumerable<MantenimientoViewModel> GetAllMantenimiento(List<DependenciasUsuario> dependencias,int? categoria);
    }
}
