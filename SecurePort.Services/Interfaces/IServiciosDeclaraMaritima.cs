using System.Collections.Generic;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    public interface IServiciosDeclaraMaritima
    {

        IEnumerable<DeclaraMaritimaViewModel> GetAllDeclaraMaritima();

        IEnumerable<DeclaraMaritimaViewModel> GetAllDeclaraMaritima(List<DependenciasUsuario> dependencias, int? categoria);


    }
   
}
