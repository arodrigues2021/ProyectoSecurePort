using System.Collections.Generic;
using SecurePort.Entities.Models;
using System;

namespace SecurePort.Services.Interfaces
{

    public interface IServiciosFormaciones
    {

        IEnumerable<FormacionesViewModel> GetAllFormaciones();

        IEnumerable<FormacionesViewModel> GetAllFormaciones(List<DependenciasUsuario> dependencias,int? categoria);

    }
}



