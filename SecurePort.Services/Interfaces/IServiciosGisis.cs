using System.Collections.Generic;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{

    public interface IServiciosGisis
    {

        IEnumerable<GisisViewModel> GetAllGisis();

        IEnumerable<GisisViewModel> GetAllGisis(List<DependenciasUsuario> dependencias,int? categoria);

    }
}



