using System.Collections.Generic;
using SecurePort.Entities.Models;
using System;

namespace SecurePort.Services.Interfaces
{

    public interface IServiciosProcedimientos
    {

        IEnumerable<ProcedimientosViewModel> GetAllProcedimientos();

        IEnumerable<ProcedimientosViewModel> GetAllProcedimientos(List<DependenciasUsuario> dependencias,int? categoria);

    }
}



